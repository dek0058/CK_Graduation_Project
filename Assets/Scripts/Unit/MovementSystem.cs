using UnityEngine;

namespace Game.Unit {
    public class MovementSystem : MonoBehaviour {

        public enum Path_Type {
            Null = 0,
            Ground,
            Air,
        }

        
        [HideInInspector]
        public new Collider collider;

        [HideInInspector]
        public new Rigidbody rigidbody;


        public Path_Type path_type = Path_Type.Null;
        


        private Vector3 previous_position = Vector3.zero;
        private Vector3 current_position = Vector3.zero;
        private Vector3 next_movement = Vector3.zero;


        public Vector3 velocity {
            get; private set;
        }

        public bool is_grounded {
            get; private set;
        }

        

        /// <summary>
        /// 객체를 목표좌표로 즉시 이동시킵니다.
        /// </summary>
        /// <param name="target">이동할 좌표</param>
        public void set_position ( Vector3 target ) {
            previous_position = target;
            rigidbody.MovePosition ( target );
        }


        /// <summary>
        /// 객체에게 속도를 부여합니다.
        /// </summary>
        /// <param name="velocity">속도</param>
        public void move ( Vector3 velocity ) {
            velocity.x = float.IsNaN(velocity.x) ? 0f : velocity.x;
            velocity.y = float.IsNaN ( velocity.y ) ? 0f : velocity.y;
            velocity.z = float.IsNaN ( velocity.z ) ? 0f : velocity.z;


            next_movement += velocity;
        }



        private void check_ground ( ) {
            switch ( path_type ) {
                case Path_Type.Null:

                    // Null 상태는 공중 상태와 같기 때문
                case Path_Type.Air:

                    break;

                case Path_Type.Ground:

                    break;

                default: return;
            }
        }



        private void check_collision ( ) {
        
        }


        /// <summary>
        /// MovementSystem class를 검증합니다.
        /// </summary>
        public void confirm ( ) {
            collider = GetComponent<Collider> ( );
            rigidbody = GetComponent<Rigidbody> ( );

            is_grounded = true;
        }


        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////


        private void Awake ( ) {
            confirm ( );
        }


        private void FixedUpdate ( ) {
            previous_position = rigidbody.position;
            current_position = previous_position + next_movement;
            velocity = (current_position - previous_position) / Time.fixedDeltaTime;

            rigidbody.MovePosition ( current_position );
            next_movement = Vector3.zero;

            check_collision ( );
            check_ground ( );
        }

    }
}
