using UnityEngine;
using System.Collections;

namespace Game.Unit {
    using JToolkit.Utility;
    using User;
    using Management;
    using Audio;
    using Ability;

    public abstract class UUnit : MonoBehaviour, IGameSpace {
        
        
        [HideInInspector]
        public uint id;
        [HideInInspector]
        public string nickname;

        public Player player;
        public UUnit owner = null;
        public MovementSystem movement_system = null;
        public UnitType unit_type = null;
        public SfxAudio sfx = null;

        public UnitData unit_data = null;
        public UnitStatus unit_status = new UnitStatus();
        public UnitOrderProperties unit_order_properties = new UnitOrderProperties ( );

        public UnitOrder unit_movemt_order = new UnitOrder ( );
        public UnitOrder unit_order = new UnitOrder ( );
        public AbilityCaster ability_caster = new AbilityCaster ( );


        public event on_revive event_revive    = null;
        public event on_dead event_dead        = null;
        public event on_attack event_attack    = null;
        public event on_damaged event_damaged  = null;

        private float fixedtimescale = 0f;

        private bool do_confirm = false;

        public enum AnimatorTag {
            Default = 0,
            Idle,       // 0
            Movement,   // 1
            Attack,     // 2
            Dead,       // 3
        }
        readonly protected EnumDictionary<AnimatorTag, string> state_tag = new EnumDictionary<AnimatorTag, string> {
            { AnimatorTag.Default,   "" },
            { AnimatorTag.Idle,      "Idle" },
            { AnimatorTag.Movement,  "Movement" },
            { AnimatorTag.Attack,    "Attack" },
            { AnimatorTag.Dead,      "Dead" },
        };
        public enum AnimatorParameter {
            Aspeed,
            OrderId,
            Action,
        }
        readonly protected EnumDictionary<AnimatorParameter, int> state_para = new EnumDictionary<AnimatorParameter, int> {
            { AnimatorParameter.Aspeed, Animator.StringToHash ( "Aspeed" ) },
            { AnimatorParameter.OrderId, Animator.StringToHash ( "OrderId" ) },
            { AnimatorParameter.Action, Animator.StringToHash ( "Action" ) },
        };

        public enum AttechmentPoint {
            Origin = 0,
            Chest,
            Head,
        }
        readonly protected EnumDictionary<AttechmentPoint, string> attech_point_name = new EnumDictionary<AttechmentPoint, string> {
            { AttechmentPoint.Origin, "Origin" },
            { AttechmentPoint.Chest, "Chest" },
            { AttechmentPoint.Head, "Head" },
        };
        protected EnumDictionary<AttechmentPoint, Transform> attech_point_transform = new EnumDictionary<AttechmentPoint, Transform> ( );

        [SerializeField]
        private GameSpace gp;
        public GameSpace game_space {
            get => gp;
            set {
                gp = value;
            }
        }




        /// <summary>
        /// 유닛을 생성합니다.
        /// </summary>
        /// <param name="position">생성할 좌표</param>
        /// <param name="angle">각도</param>
        private static T create<T> ( Vector3 position, float angle = 0f ) {
            GameObject prefab = ResourceLoader.instance.get_unit ( typeof(T).Name );
            if(prefab == null) {
                return default ( T );
            }
            GameObject obj = Instantiate ( prefab, position, Quaternion.identity );
            T unit = obj.GetComponent<T> ( );
            
            (unit as UUnit).rotate ( angle );
            return unit;
        }
        public static T create<T> ( Vector3 position, Player player, float angle = 0f ) {
            T unit = create<T> ( position, angle );
            (unit as UUnit).player = player;
            return unit;
        }
        public static T create<T> ( Vector3 position, Player player, UUnit owner, float angle = 0f ) {
            T unit = create<T> ( position, angle );
            (unit as UUnit).player = player;
            (unit as UUnit).owner = owner;
            return unit;
        }


        /// <summary>
        /// Unit class를 검증합니다.
        /// </summary>
        public virtual void confirm ( ) {
            
            if(unit_data != null) {
                UnitTableData ud = unit_data.unit_table_data;
                id = ud.id;
                nickname = ud.nickname;

                unit_status.max_hp = ud.hp;
                unit_status.current_hp = ud.hp;

                unit_status.mspeed = ud.mspeed;
                unit_status.aspeed = ud.aspeed;
                unit_status.rspeed = ud.rspeed;

                unit_status.damage = ud.damage;
                unit_status.armor = ud.armor;
                unit_status.atime = ud.atime;
                
                unit_status.flying = ud.flying;
            }


            if( unit_type == null) {
                unit_type = gameObject.GetComponent<UnitType> ( );
                unit_type.unit = this;
            } else if(unit_type != null && unit_type.unit == null) {
                unit_type.unit = this;
            }

            if ( sfx == null ) {
                sfx = transform.GetComponentInChildren<SfxAudio> ( );
            }
        

            if ( !attech_point_transform.ContainsKey ( AttechmentPoint.Origin ) ) {
                attech_point_transform.Add ( AttechmentPoint.Origin, unit_type.transform );
            }
            if ( !attech_point_transform.ContainsKey ( AttechmentPoint.Chest ) ) {
                Transform chest = unit_type.transform.Find ( attech_point_name[AttechmentPoint.Chest] );
                if(chest != null) {
                    attech_point_transform.Add ( AttechmentPoint.Chest, chest );
                }
            }
            if ( !attech_point_transform.ContainsKey ( AttechmentPoint.Head ) ) {
                Transform head = unit_type.transform.Find ( attech_point_name[AttechmentPoint.Head] );
                if ( head != null ) {
                    attech_point_transform.Add ( AttechmentPoint.Head, head );
                }
            }

            ability_caster.unit = this;
            ability_caster.info.source = this;


            if ( movement_system == null ) {
                movement_system = GetComponent<MovementSystem> ( );
            }
            movement_system.confirm ( );

            game_space = gp;
            unit_order_properties.full ( );

            do_confirm = true;
        }


        /// <summary>
        /// Unit을 목표 각도로 회전시킵니다.
        /// </summary>
        public virtual void rotate ( float target ) {
            unit_status.look_at = target;
        }

        /// <summary>
        /// Unit을 즉시 회전 시킵니다.
        /// </summary>
        public void rotate ( Vector3 target ) {
            unit_type.transform.eulerAngles = target;
        }

        /// <summary>
        /// Unit을 즉시 목표 값으로 회전 시킵니다.
        /// </summary>
        public void rotate ( float x, float y, float z ) {
            unit_type.transform.Rotate ( x, y, z );
        }


        /// <summary>
        /// Unit을 목표 좌표로 즉시 이동시킵니다.
        /// </summary>
        /// <param name="target">이동할 좌표</param>
        public void position ( Vector3 target ) {
            transform.position = target;
        }


        /// <summary>
        /// Unit이 이동합니다.
        /// </summary>
        /// <param name="direction">이동 방향</param>
        public virtual void move ( Vector2 direction ) {
            unit_status.input = direction;
        }


        /// <summary>
        /// 특정 애니메이션을 실행시킵니다.
        /// </summary>
        /// <param name="name">애니메이션 이름</param>
        public void set_animation ( string name, int layer = 0 ) {
            unit_type.animator.Play ( name, layer );
        }


        /// <summary>
        /// 애니메이션을 실행시킵니다.
        /// </summary>
        /// <param name="id">애니메이션 ID</param>
        public void action_animation ( int id ) {
            unit_type.animator.SetInteger ( state_para[AnimatorParameter.OrderId], id );
            unit_type.animator.SetTrigger ( state_para[AnimatorParameter.Action] );
        }


        /// <summary>
        /// 유닛에게 피해를 입힙니다.
        /// </summary>
        /// <param name="type">피해 속성</param>
        /// <param name="amount">피해양</param>
        /// <param name="source">피해를 준 유닛</param>
        /// <returns>피해 성공 여부</returns>
        public bool damage ( DamageInfo.Type type, float amount, UUnit source = null ) {
            if( unit_status.is_invincible || unit_status.is_dead) {   // 무적이거나 죽었을 경우 데미지를 받지 않는다...
                return false;
            }

            if(amount < 0f) {   // 데미지는 마이너스가 될 수 없으므로...
                amount = 0f;
            }

            unit_type.add_damage ( type, amount, true );    // 기본 피해
            event_damaged?.Invoke ( source, this );

            float armor = unit_status.armor + unit_status.add_armor + unit_status.rate_armor;
            float melee = DamageInfo.damage ( DamageInfo.Type.Melee, unit_type.get_damage_to_value ( DamageInfo.Type.Melee ), armor );
            float spell = DamageInfo.damage ( DamageInfo.Type.Spell, unit_type.get_damage_to_value ( DamageInfo.Type.Spell ), armor );
            float universal = DamageInfo.damage ( DamageInfo.Type.Universal, unit_type.get_damage_to_value ( DamageInfo.Type.Universal ), armor );
            float result = melee + spell + universal;
            unit_status.current_hp -= result;
            unit_type.damage_clear ( );
            return true;
        }


        public void revive ( float life ) {
            unit_status.current_hp = life >= 1f ? life : 1f;
            unit_status.is_dead = false;
            active_alive();
        }


        /// <summary>
        /// 유닛이 공격을 시작했을 때 이벤트
        /// </summary>       
        protected void active_attack ( ) {
            event_attack?.Invoke ( this );
        }


        /// <summary>
        /// 유닛 회전을 갱신합니다.
        /// </summary>
        protected virtual void active_rotate ( ) {  // Fixed Update
            float y = unit_type.transform.eulerAngles.y;
            float gap = Mathf.DeltaAngle ( y, unit_status.look_at );
            float rspeed = 0f;

            if ( gap > 1f ) {
                rspeed = unit_status.rspeed * fixedtimescale;
            } else if ( gap < -1f ) {
                rspeed = -unit_status.rspeed * fixedtimescale;
            }

            // 보정
            while ( Mathf.Abs ( gap ) < Mathf.Abs ( rspeed ) ) {
                rspeed *= 0.5f;
            }

            unit_type.transform.Rotate ( 0f, rspeed, 0f );
            unit_status.angle = unit_type.transform.eulerAngles.y;
        }


        /// <summary>
        /// 유닛 이동을 갱신합니다.
        /// </summary>
        protected virtual void active_move ( ) {    // Fixed Update
            float speed = unit_status.rhythm * unit_status.mspeed;
            unit_status.direction = unit_status.input;

            movement_system.move ( (unit_status.direction / unit_status.direction.magnitude) * speed );
        }


        /// <summary>
        /// 유닛이 죽었을때 이벤트
        /// </summary>
        protected virtual void active_dead ( ) {    // Late Update
            unit_status.is_dead = true;
            if(unit_status.current_hp > 0f) {
                unit_status.current_hp = 0f;
            }

            event_dead?.Invoke ( this );
        }


        /// <summary>
        /// 유닛이 살아났을때 이벤트
        /// </summary>
        protected virtual void active_alive ( ) {   // Late Update
            event_revive?.Invoke ( this );
        }



        protected virtual void active_update ( ) {
            //ability_caster.update ( );
            for ( int i = 0; i < ability_caster.abilitys.Count; ++i ) {
                if ( ability_caster.abilitys[i].is_passive ) {
                    continue;
                }
                if ( unit_order.order == ability_caster.abilitys[i].order_id ) {
                    if ( ability_caster.abilitys[i].is_cooltime ) {
                        break;
                    }

                    // HACK : 스펠마다 단계가 있어야 함.
                    unit_order.execute ( );
                    ability_caster.abilitys[i].ability.event_use ( ability_caster.info );
                    StartCoroutine ( ability_caster.Ecooltime_update ( ability_caster.abilitys[i] ) );

                    break;
                }
            }


            if (get_animator() != null) {
                get_animator ( ).SetFloat ( state_para[AnimatorParameter.Aspeed], unit_status.rhythm );
            }

            
            if ( unit_movemt_order.order == OrderId.Stop ||
                unit_movemt_order.order == OrderId.None ) {

                if ( unit_movemt_order.order == OrderId.Stop ) {
                    unit_movemt_order.execute ( );
                }
                unit_status.look_at = unit_status.angle;
                if ( get_animator ( ).GetInteger ( state_para[AnimatorParameter.OrderId] ) != 0 ) {
                    action_animation ( 0 );
                }
            }
        }


        protected virtual void active_fixedupdate ( ) {
            fixedtimescale = Time.fixedDeltaTime * unit_status.rhythm;

            if (unit_order_properties.get(UnitOrderProperties.Properties.Rotation)) {
                active_rotate ( );
            }
            
            if(unit_order_properties.get(UnitOrderProperties.Properties.Movement)) {
                active_move ( );
            }
        }


        protected virtual void active_lateupdate ( ) {
            if ( unit_status.current_hp <= 0.664f ||
                unit_status.is_dead ) {
                active_dead ( );
            }

            unit_movemt_order.update ( );
            unit_order.update ( );
            
        }


        public Vector3 get_position ( ) {
            return unit_type.transform.position;
        }


        public Vector3 get_rotation ( ) {
            return unit_type.transform.eulerAngles;
        }


        public Animator get_animator ( ) {
            return unit_type.animator;
        }


        public Transform get_attech_point ( AttechmentPoint point ) {
            if(attech_point_transform.ContainsKey(point)) {
                return attech_point_transform[point];
            } else {
                return attech_point_transform[AttechmentPoint.Origin];
            }
        }


        /// <summary>
        /// 현재 Animator State Machine을 가져옵니다.
        /// </summary>
        /// <returns>Current Animator State Machine</returns>
        public AnimatorStateInfo get_animator_state ( int layer ) {
            return get_animator ( ).GetCurrentAnimatorStateInfo ( layer );
        }


        /// <summary>
        /// 다음 Animator State Machine을 가져옵니다.
        /// </summary>
        /// <returns>Current Animator State Machine</returns>
        public AnimatorStateInfo get_animator_nextstate ( int layer ) {
            return get_animator ( ).GetNextAnimatorStateInfo ( layer );
        }


        /// <summary>
        /// 현재 Animator State Machine의 해시값과 일치하는지 검증합니다.
        /// </summary>
        public bool equals_animator_hash ( int layer, int hash ) {
            return get_animator_state ( layer ).fullPathHash == hash;
        }


        public void destroy ( ) {
            Destroy ( gameObject );
        }

        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private IEnumerator Start ( ) {
            yield return new WaitUntil ( ( ) => GameManager.instance.is_complete );
            confirm ( );
        }

        private void Update ( ) {
            if( !do_confirm ) {
                return;
            }
            active_update ( );
        }

        private void FixedUpdate ( ) {
            if ( !do_confirm ) {
                return;
            }
            active_fixedupdate ( );
        }

        private void LateUpdate ( ) {
            if ( !do_confirm ) {
                return;
            }
            active_lateupdate ( );
        }
    }
}