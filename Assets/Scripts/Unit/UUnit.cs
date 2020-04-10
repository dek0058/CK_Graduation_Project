using UnityEngine;
using System.Collections;

namespace Game.Unit {
    using JToolkit.Utility;
    using User;
    using Management;
    using System.Reflection;

    public abstract class UUnit : MonoBehaviour {
        public readonly int Animation_Speed_Hash = Animator.StringToHash ( "Aspeed" );

        public Player player;
        public UUnit owner = null;
        public MovementSystem movement_system = null;
        public UnitType unit_type = null;

        public UnitData unit_data = null;
        public UnitStatus unit_status = new UnitStatus();

        public event on_revive event_revive      = null;
        public event on_dead event_dead        = null;
        public event on_attack event_attack    = null;
        public event on_damaged event_damaged  = null;


        [HideInInspector]
        public UnitOrder unit_order = null;


        public enum AnimatorTag {
            Default = 0,
            Idle,
            Movement,
            Attack,
            Dead,
        }
        readonly public EnumDictionary<AnimatorTag, string> state_tag = new EnumDictionary<AnimatorTag, string> {
            { AnimatorTag.Default,   "" },
            { AnimatorTag.Idle,      "Idle" },
            { AnimatorTag.Movement,  "Movement" },
            { AnimatorTag.Attack,    "Attack" },
            { AnimatorTag.Dead,      "Dead" },
        };

        public enum AttechmentPoint {
            Origin = 0,
            Chest,
            Head,
        }
        readonly public EnumDictionary<AttechmentPoint, string> attech_point_name = new EnumDictionary<AttechmentPoint, string> {
            { AttechmentPoint.Origin, "Origin" },
            { AttechmentPoint.Chest, "Chest" },
            { AttechmentPoint.Head, "Head" },
        };
        private EnumDictionary<AttechmentPoint, Transform> attech_point_transform = new EnumDictionary<AttechmentPoint, Transform> ( );



        /// <summary>
        /// 유닛을 생성합니다.
        /// </summary>
        /// <param name="position">생성할 좌표</param>
        /// <param name="angle">각도</param>
        public static T create<T> ( Vector2 position, float angle = 0f ) {
            GameObject prefab = ResourceLoader.instance.get_unit ( typeof(T).Name );
            if(prefab == null) {
                return default ( T );
            }
            GameObject obj = Instantiate ( prefab, position, Quaternion.identity );
            T unit = obj.GetComponent<T> ( );
            
            (unit as UUnit).rotate ( angle );
            return unit;
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
            movement_system.set_position ( target );
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
        /// 특정 애니메이션을 실행시킵니다.
        /// </summary>
        /// <param name="hash">애니메이션 값</param>
        public void set_animation ( int hash, int layer = 0 ) {
            unit_type.animator.Play ( hash, layer );
        }


        public virtual void load_data ( Object data ) {
            // TODO : 불러올 유닛 능력치 데이터들
        }


        public virtual void save_data ( Object data ) {
            // TODO : 저장할 유닛의 능력치 데이터들
        }


        public void active_attack ( ) {
            event_attack?.Invoke ( this );
        }


        public void revive ( float life ) {
            unit_status.current_hp = life >= 1f ? life : 1f;
            unit_status.is_dead = false;
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


        /// <summary>
        /// Unit class를 검증합니다.
        /// </summary>
        public virtual void confirm ( ) {

            if( unit_type == null) {
                unit_type = gameObject.GetComponentInChildren<UnitType> ( );
                unit_type.unit = this;
            } else if(unit_type != null && unit_type.unit == null) {
                unit_type.unit = this;
            }
           

            if( unit_order == null) {
                unit_order = new UnitOrder ( );
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


            if ( movement_system == null ) {
                movement_system = GetComponentInChildren<MovementSystem> ( );
            }

            movement_system.confirm ( );
            // TODO : 추가 검증
        }


        /// <summary>
        /// 유닛 회전을 갱신합니다.
        /// </summary>
        protected virtual void active_rotate ( ) {  // Fixed Update
            float y = unit_type.transform.eulerAngles.y;
            float gap = Mathf.DeltaAngle ( y, unit_status.look_at );
            float rspeed = 0f;

            if ( gap > 1f ) {
                rspeed = unit_status.rspeed * Time.fixedDeltaTime;
            } else if ( gap < -1f ) {
                rspeed = -unit_status.rspeed * Time.fixedDeltaTime;
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
            unit_status.direction = unit_status.input;
            movement_system.move ( (unit_status.direction / unit_status.direction.magnitude) * unit_status.mspeed );
        }


        protected virtual void active_dead ( ) {    // Late Update
            unit_status.is_dead = true;
            if(unit_status.current_hp > 0f) {
                unit_status.current_hp = 0f;
            }

            event_dead?.Invoke ( this );
        }


        protected virtual void active_alive ( ) {   // Late Update
            event_revive?.Invoke ( this );
        }



        protected virtual void active_update ( ) {     
        }


        protected virtual void active_fixedupdate ( ) {
            if ( !unit_order.get_active ( UnitOrder.Active.Rotate ) ) {
                active_rotate ( );
            }

            if ( !unit_order.get_active ( UnitOrder.Active.Move ) ) {
                active_move ( );
            }
        }


        protected virtual void active_lateupdate ( ) {
            if ( unit_status.current_hp <= 0.664f ||
                unit_status.is_dead ) {
                active_dead ( );
                return;
            } else if ( !unit_status.is_dead ) {
                if ( get_animator ( ) != null ) {
                    if ( get_animator_state ( 0 ).IsTag ( state_tag[AnimatorTag.Dead] ) ) {
                        active_alive ( );
                    }
                }
            }

            if ( unit_order.get_order ( Order_Id.Stop ) ) {
                unit_status.look_at = unit_status.angle;
                unit_order.set_order ( Order_Id.Stop, false );
            }

            if(get_animator() != null) {
                get_animator ( ).SetFloat ( Animation_Speed_Hash, unit_status.rhythm );
            }
        }



        public void set_order ( Order_Id id, bool value ) {
            unit_order.set_order ( id, value );
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
            movement_system.collisions.Clear ( );
            Destroy ( gameObject );
        }

        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Awake ( ) {
            confirm ( );
        }

        private void Update ( ) {
            active_update ( );
        }

        private void FixedUpdate ( ) {
            active_fixedupdate ( );
        }

        private void LateUpdate ( ) {
            active_lateupdate ( );
        }
    }


    /// <summary>
    /// Unit이 가진 능력치를 나타냅니다.
    /// </summary>
    [System.Serializable]
    public class UnitStatus {
        public bool is_dead;
        public bool is_invincible;

        public float current_hp;            // 현재 체력       
        public float max_hp;                // 최대 체력

        public float rhythm;                // 유닛 속도
        public float mspeed;                // 이동 속도
        public float aspeed;                // 공격 속도
        public float rspeed;                // 회전 속도


        public float damage;                // 공격력
        public float armor;                 // 방어력
        public float attack_cooltime;       // 공격 쿨타임



        // 현재 상태 값

        public float angle;                 // 유닛의 각도
        public float look_at;               // 바라볼 각도

        public Vector2 input;               // 바라봐야 할 방향
        public Vector2 direction;           // 현재 방향
        public Vector2 axis;                // 바라보고 있는 축

        public float flying;                // 기본 높이 (고정)
        public float current_flying;        // 현재 높이


        // 추가 스텟

        public float add_hp {               // 추가증가 체력
            get; private set;
        }
        public float rate_hp {              // 비율증가 체력
            get; private set;
        }


        public float add_mspeed {           // 추가증가 이동속도
            get; private set;
        }
        public float rate_mspeed {          // 비율증가 이동속도
            get; private set;
        }


        public float add_aspeed {           // 추가증가 공격속도
            get; private set;
        }
        public float rate_aspeed {          // 비율증가 공격속도
            get; private set;
        }

        
        public float add_damage {           // 추가증가 공격력
            get; private set;
        }
        public float rate_damage {          // 비율 증가 공격력
            get; private set;
        }


        public float add_armor {            // 추가증가 방어력
            get; private set;
        }
        public float rate_armor {           // 비율증가 방어력
            get; private set;
        }

        // TODO : 추가할 스텟들
    }

    /// <summary>
    /// 소생하였을 때 이벤트
    /// </summary>
    /// <param name="unit">살아난 유닛</param>
    public delegate void on_revive ( UUnit unit );

    /// <summary>
    /// 죽었을 때 이벤트
    /// </summary>
    /// <param name="unit">죽은 유닛</param>
    public delegate void on_dead ( UUnit unit );

    /// <summary>
    /// 유닛이 공격을 시작했을 때 이벤트
    /// </summary>
    /// <param name="source">공격한 유닛</param>
    public delegate void on_attack ( UUnit source );

    /// <summary>
    /// 유닛이 피해를 받는 중일 때 이벤트
    /// </summary>
    /// <param name="source">피해를 입힌 유닛</param>
    /// <param name="target">피해를 받는 유닛</param>
    public delegate void on_damaged ( UUnit source, UUnit target );

}