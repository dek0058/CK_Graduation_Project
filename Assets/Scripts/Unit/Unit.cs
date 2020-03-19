using UnityEngine;
using System.Collections;

namespace Game.Unit {
    using JToolkit.Utility;
    using User;

    public abstract class Unit : MonoBehaviour {
        public readonly int Animation_Speed_Hash = Animator.StringToHash ( "Aspeed" );

        public Player player;
        public Unit owner = null;

        public UnitData unit_data = null;
        public UnitModel unit_model = new UnitModel();
        public UnitStatus unit_status = new UnitStatus();

        public event on_dead event_revive      = null;
        public event on_dead event_dead        = null;
        public event on_attack event_attack    = null;
        public event on_damaged event_damaged  = null;


        [HideInInspector]
        public MovementSystem movement_system = null;

        [HideInInspector]
        public UnitType unit_type = null;

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
            {AnimatorTag.Default, "" },
            {AnimatorTag.Idle, "Idle" },
            {AnimatorTag.Movement, "Movement" },
            {AnimatorTag.Attack, "Attack" },
            {AnimatorTag.Dead, "Dead" },
        };



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
            unit_model.transform.eulerAngles = target;
        }

        /// <summary>
        /// Unit을 즉시 목표 값으로 회전 시킵니다.
        /// </summary>
        public void rotate ( float x, float y, float z ) {
            unit_model.transform.Rotate ( x, y, z );
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
            unit_model.animator.Play ( name, layer );
        }

        /// <summary>
        /// 특정 애니메이션을 실행시킵니다.
        /// </summary>
        /// <param name="hash">애니메이션 값</param>
        public void set_animation ( int hash, int layer = 0 ) {
            unit_model.animator.Play ( hash, layer );
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


        public void damage ( DamageInfo.Type type, float amount, Unit source = null ) {
            if( unit_status.is_invincible || unit_status.is_dead) {   // 무적이거나 죽었을 경우 데미지를 받지 않는다...
                return;
            }

            if(amount < 0f) {   // 데미지는 마이너스가 될 수 없으므로...
                amount = 0f;
            }

            unit_type.add_damage ( type, amount, true );    // 기본 피해
            event_damaged?.Invoke ( source, this );

            float armor = unit_status.armor + unit_status.add_armor + unit_status.rate_armor;
            float life = unit_status.current_hp;
            float melee = DamageInfo.damage ( DamageInfo.Type.Melee, unit_type.get_damage_to_value ( DamageInfo.Type.Melee ), armor );
            float spell = DamageInfo.damage ( DamageInfo.Type.Spell, unit_type.get_damage_to_value ( DamageInfo.Type.Melee ), armor );
            float universal = DamageInfo.damage ( DamageInfo.Type.Universal, unit_type.get_damage_to_value ( DamageInfo.Type.Melee ), armor );
            float result = melee + spell + universal;
            unit_status.current_hp -= result;
        }


        /// <summary>
        /// Unit class를 검증합니다.
        /// </summary>
        public virtual void confirm ( ) {

            // UnitModel 검증
            if ( unit_model.parent == null ) {
                unit_model.parent = transform.Find ( "Model" );
            }

            if ( unit_model.transform == null ) {
                unit_model.transform = unit_model.parent.GetChild ( 0 );
                unit_model.animator = unit_model.transform?.GetComponent<Animator> ( );
            }

            

            if( unit_type  == null) {
                unit_type = gameObject.GetComponentInChildren<UnitType> ( );
            }

            if( unit_order == null) {
                unit_order = new UnitOrder ( );
            }

            if ( movement_system == null ) {
                movement_system = GetComponentInChildren<MovementSystem> ( );
                movement_system.confirm ( );
            }

            // TODO : 추가 검증
        }


        /// <summary>
        /// 유닛 회전을 갱신합니다.
        /// </summary>
        protected virtual void active_rotate ( ) {  // Fixed Update
            float y = unit_model.transform.eulerAngles.y;
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

            unit_model.transform.Rotate ( 0f, rspeed, 0f );
            unit_status.angle = unit_model.transform.eulerAngles.y;
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
            return unit_model.transform.position;
        }


        public Vector3 get_rotation ( ) {
            return unit_model.transform.eulerAngles;
        }


        public Animator get_animator ( ) {
            return unit_model.animator;
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
    /// Unit에 포함된 모델 객체의 속성들을 나타냅니다.
    /// </summary>
    [System.Serializable]
    public class UnitModel {
        public Transform parent;
        public Transform transform;
        public Animator animator;

        public UnitModel ( ) => (parent, transform, animator) = (null, null, null);
        public UnitModel ( Transform parent, Transform transform ) {
            this.parent = parent;
            this.transform = transform;
            animator = transform?.GetComponent<Animator> ( );
        }
    }


    public delegate void on_revive ( Unit unit );
    public delegate void on_dead ( Unit unit );
    public delegate void on_attack ( Unit source );
    public delegate void on_damaged ( Unit source, Unit target );
}