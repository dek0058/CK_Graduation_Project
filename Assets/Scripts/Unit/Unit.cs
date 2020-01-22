using UnityEngine;

namespace Game.Unit {
    using Game.Unit.Type;

    public abstract class Unit : MonoBehaviour {

        public UnitModel unit_model = new UnitModel();
        public UnitStatus unit_status = new UnitStatus();

        public string nickname = "";


        [HideInInspector]
        public MovementSystem movement_system = null;

        [HideInInspector]
        public UnitType unit_type = null;



        /// <summary>
        /// Unit을 회전시킵니다.
        /// </summary>
        /// <param name="angle">회전 값</param>
        public abstract void rotate ( float angle );

        /// <summary>
        /// Unit을 축을 기준으로 회전시킵니다.
        /// </summary>
        /// <param name="axis">축(X,Y,Z)</param>
        /// <param name="angle">회전 값</param>
        public void rotate ( Vector3 axis, float angle ) {
            unit_model.transform.Rotate ( axis, angle * unit_status.rotation_speed );
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
        public abstract void move ( Vector3 direction );


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
        }
    }


    [System.Serializable]
    public class UnitStatus {
        public float current_hp {           // 현재 체력
            get; set;
        }            
        public float max_hp {               // 최대 체력
            get; set;
        }


        public float movement_speed {       // 이동 속도
            get; set;
        }
        public float attack_speed {         // 공격 속도
            get; set;
        }
        public float rotation_speed {       // 회전 속도
            get; set;
        }


        public float damage {               // 공격력
            get; set;
        }
        public float armor {                // 방어력
            get; set;
        }


        public float angle {                // 각도
            get; set;
        }

        // TODO : 추가할 스텟들
    }


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