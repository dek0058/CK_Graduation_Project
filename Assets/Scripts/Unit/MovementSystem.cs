using UnityEngine;
using System.Collections;

namespace Game.Unit {
    using Game.Management;

    public class MovementSystem : MonoBehaviour {

        public enum PathType {
            Null = 0,
            Ground,
            Air,
        }

        public PathType path_type = PathType.Null;
        
        public new Collider2D collider;
        [HideInInspector]
        public new Rigidbody2D rigidbody;
        [HideInInspector]
        public Unit unit;


        private Vector2 next_velocity = Vector2.zero;


        public bool is_grounded {
            get; private set;
        }


        /// <summary>
        /// 객체를 목표좌표로 즉시 이동시킵니다.
        /// </summary>
        /// <param name="target">이동할 좌표</param>
        public void set_position ( Vector3 target ) {
            rigidbody.MovePosition ( target );
        }


        /// <summary>
        /// 객체에게 속도를 부여합니다.
        /// </summary>
        /// <param name="velocity">속도</param>
        public void move ( Vector2 velocity ) {
            velocity.x = float.IsNaN(velocity.x) ? 0f : velocity.x;
            velocity.y = float.IsNaN ( velocity.y ) ? 0f : velocity.y;
            next_velocity += velocity;
        }


        public void add_force ( Vector2 force ) {
            StartCoroutine ( Eadd_force ( force ) );
        }


        private void update_movement ( ) {
            if(next_velocity == Vector2.zero) {
                rigidbody.velocity = Vector2.zero;
                return;
            }

            rigidbody.velocity = next_velocity * unit.unit_status.rhythm;
            next_velocity = Vector3.zero;
        }



        private void check_ground ( ) {
            switch ( path_type ) {
                case PathType.Null:

                    // Null 상태는 공중 상태와 같기 때문
                case PathType.Air:


                case PathType.Ground: {
                    Vector2 offset = collider.offset;
                    Vector2 origin = (Vector2)transform.position;
                    Vector2 size = bottom_collider_size ( );
                    int mask = 1 << (int)GameLayer.Ground;

                    Collider2D[] ground = Physics2D.OverlapCapsuleAll ( origin, size, CapsuleDirection2D.Horizontal, unit.unit_status.angle, mask );

                    foreach(var tile in ground) {

                    }
                }
                    break;

                default: return;
            }
        }


        private void check_collision ( ) {
        
        }




        private Vector2 bottom_collider_size ( ) {
            Vector2 size = Vector2.zero;
            switch ( collider ) {
                case CapsuleCollider2D capsule when collider is CapsuleCollider2D: {
                    float x = capsule.size.x;
                    float y = capsule.size.y;
                    size = capsule.direction == CapsuleDirection2D.Vertical ? new Vector2 ( x / 2.22f, 0.1f ) : new Vector2 ( x / 1.34f, 0.1f );
                }
                break;
            }

            return size;
        }


        /// <summary>
        /// MovementSystem class를 검증합니다.
        /// </summary>
        public void confirm ( ) {

            if ( unit == null ) {
                unit = GetComponent<Unit> ( );
            }

            if ( rigidbody == null ) {
                rigidbody = GetComponent<Rigidbody2D> ( );
            }
            
            if(unit != null) {
                if ( collider == null ) {
                    collider = unit.unit_model.transform.GetComponent<Collider2D> ( );
                }
            }
            
            is_grounded = true;
        }


        private IEnumerator Eadd_force ( Vector2 force ) {
            float x = force.x >= 0f ? 1f : -1f;
            float y = force.y >= 0f ? 1f : -1f;
            float force_x = Mathf.Abs ( force.x );
            float force_y = Mathf.Abs ( force.y );
            float friction = 0.88f;
            bool loop = true;

            while(loop) {
                if ( force_x < 0.001f && force_y < 0.001f ) {
                    loop = false;
                    Debug.Log ( "stop" );
                    continue;
                }

                force_x = force_x * friction;
                force_y = force_y * friction;
                move ( new Vector2 ( force_x * x, force_y * y ) );
                yield return new WaitForFixedUpdate ( );
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void FixedUpdate ( ) {
            check_ground ( );
            check_collision ( );
            update_movement ( );
            //rigidbody.AddForce ( next_movement );
            //next_movement = Vector3.zero;
        }

    }
}
