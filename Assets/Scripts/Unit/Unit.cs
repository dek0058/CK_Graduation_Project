using UnityEngine;

namespace Game.Unit {
    using JToolkit.Utility;
    using Game.Unit.Type;

    public abstract class Unit : MonoBehaviour {

        public UnitData unit_data = null;
        public UnitModel unit_model = new UnitModel();
        public UnitStatus unit_status = new UnitStatus();


        [HideInInspector]
        public MovementSystem movement_system = null;

        [HideInInspector]
        public UnitType unit_type = null;

        [HideInInspector]
        public UnitOrder unit_order = null;


        public enum Animator_Tag {
            Default = 0,
            Idle,
            Movement,
            Attack,
            Dead,
        }

        public EnumDictionary<Animator_Tag, string> state_tag = new EnumDictionary<Animator_Tag, string> {
            {Animator_Tag.Default, "" },
            {Animator_Tag.Idle, "Idle" },
            {Animator_Tag.Movement, "Movement" },
            {Animator_Tag.Attack, "Attack" },
            {Animator_Tag.Attack, "Dead" },
        };


        /// <summary>
        /// Action Animation 시작점
        /// </summary>
        public virtual void action_begin ( ) {
            unit_status.look_at = get_rotation ( ).y;
        }

        /// <summary>
        /// Action Animation 종료점
        /// </summary>
        public virtual void action_end ( ) {
            unit_status.look_at = get_rotation ( ).y;
            unit_status.input = Vector2.zero;
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

            if( movement_system  == null ) {
                movement_system = GetComponent<MovementSystem> ( );
            }

            if( unit_type  == null) {
                unit_type = gameObject.GetComponentInChildren<UnitType> ( );
                unit_type.schedules.Add ( UnitType.Begin, action_begin );
                unit_type.schedules.Add ( UnitType.End, action_end );
            }

            if( unit_order == null) {
                unit_order = new UnitOrder ( );
            }

            // TODO : 추가 검증
        }


        /// <summary>
        /// 유닛 회전을 갱신합니다.
        /// </summary>
        protected virtual void active_rotate ( ) {
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
        /// 유닛을 갱신합니다.
        /// </summary>
        protected virtual void active_update ( ) {
        }


        /// <summary>
        /// 유닛 이동을 갱신합니다.
        /// </summary>
        protected virtual void active_move ( ) {
            unit_status.direction = Vector2.MoveTowards ( unit_status.direction, unit_status.input, Time.fixedDeltaTime );
            movement_system.move ( (unit_status.direction / unit_status.direction.magnitude) * unit_status.mspeed * Time.fixedDeltaTime );
        }


        protected void active ( ) { // FixedUpdate
            active_update ( );

            if ( !unit_order.get_active ( UnitOrder.Active.Rotate ) ) {
                active_rotate ( );
            }

            if ( !unit_order.get_active ( UnitOrder.Active.Move ) ) {
                active_move ( );
            }
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
    }


    /// <summary>
    /// Unit이 가진 능력치를 나타냅니다.
    /// </summary>
    [System.Serializable]
    public class UnitStatus {
        public float current_hp;            // 현재 체력       
        public float max_hp;                // 최대 체력


        public float mspeed;                // 이동 속도
        public float aspeed;                // 공격 속도
        public float rspeed;                // 회전 속도


        public float damage;                // 공격력
        public float armor;                 // 방어력



        // 현재 상태 값

        public float angle;                 // 유닛의 각도
        public float look_at;               // 바라볼 각도

        public Vector2 input;               // 바라봐야 할 방향
        public Vector2 direction;           // 현재 방향
        public Vector2 axis;                // 바라보고 있는 축



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
}