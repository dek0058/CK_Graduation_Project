using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game.Unit {
    using Management;
    using JToolkit.Utility;

    public class MovementSystem : MonoBehaviour {

        public enum PathType {
            Ground = GameLayer.Path_Ground,
            Air = GameLayer.Path_Air,
        }

        public PathType path_type = PathType.Ground;
        
        public Collider2D shadow_collider;
        public Rigidbody2D rigidbody2d;
        public UUnit unit;

        [SerializeField]
        private GameObject path_obj;
        [SerializeField]
        private Collider2D path_collider;

        private Vector2 next_velocity = Vector2.zero;

        public List<UUnit> collisions = new List<UUnit> ( );
        public event shadow_collision_enter event_collision_enter;
        public event shadow_collision_exit event_collision_exit;


        public bool is_grounded {
            get; private set;
        }


        /// <summary>
        /// 객체를 목표좌표로 즉시 이동시킵니다.
        /// </summary>
        /// <param name="target">이동할 좌표</param>
        public void set_position ( Vector3 target ) {
            rigidbody2d.MovePosition ( target );
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


        public void lerp_flying ( float dist, float speed ) {
            if(speed <= 0f) {
                return;
            }
            StartCoroutine ( Elerp_flying ( dist, speed ) );
        }


        public void set_path_type ( PathType type ) {
            path_type = type;
            path_obj.layer = (int)path_type;
        }


        private void update_movement ( ) {
            if(next_velocity == Vector2.zero) {
                rigidbody2d.velocity = Vector2.zero;
                return;
            }

            rigidbody2d.velocity = next_velocity * Time.fixedDeltaTime * unit.unit_status.rhythm;
            next_velocity = Vector3.zero;
        }


        private void update_flying ( ) {
            float cur = unit.unit_status.current_flying;
            float flying = unit.unit_status.flying + cur;
            if(flying < 0f) {
                flying = 0f;
            }
            unit.unit_type.transform.localPosition = new Vector3 ( 0f, flying, 0f );

            if( cur > 0f) {
                unit.unit_status.current_flying += Physics2D.gravity.y * Time.fixedDeltaTime * unit.unit_status.rhythm;
            } else if( cur < 0f ) {
                unit.unit_status.current_flying = 0f;
            }
        }


        private void update_ground ( ) {
            int mask = 0;
            mask |= 1 << (int)GameLayer.Map_Ground;
            mask |= 1 << (int)GameLayer.Map_Cliff;
            Collider2D[] colliders = Physics2D.OverlapPointAll ( transform.position, mask );

            bool result = false;
            for ( int i = 0; i < colliders.Length; ++i ) {
                if ( colliders[i].gameObject.layer == (int)GameLayer.Map_Ground ) {
                    result = true; break;
                }
            }
            is_grounded = result;
        }

        /// <summary>
        /// MovementSystem class를 검증합니다.
        /// </summary>
        public void confirm ( ) {

            if ( shadow_collider == null ) {
                shadow_collider = GetComponent<Collider2D> ( );
            }


            if ( unit == null ) {
                unit = GetComponentInParent<UUnit> ( );
            }

            if ( rigidbody2d == null ) {
                rigidbody2d = GetComponentInParent<Rigidbody2D> ( );
            }

            if( path_obj == null) {
                path_obj = transform.GetChild ( 0 ).gameObject;
            }

            if ( path_obj != null && path_collider == null ) {
                path_collider = path_obj.GetComponent<Collider2D> ( );
            }

            set_path_type ( path_type );
            is_grounded = true;
        }


        private IEnumerator Eadd_force ( Vector2 force ) {
            float x = force.x >= 0f ? 1f : -1f;
            float y = force.y >= 0f ? 1f : -1f;
            float force_x = Mathf.Abs ( force.x );
            float force_y = Mathf.Abs ( force.y );
            Vector2 normal = new Vector2 ( force_x, force_y ).normalized;

            bool loop = true;
            while(loop) {
                if ( force_x < 0.001f && force_y < 0.001f ) {
                    loop = false;
                    continue;
                }

                force_x = force_x - (rigidbody2d.mass * Time.fixedDeltaTime * unit.unit_status.rhythm * normal.x);
                force_y = force_y - (rigidbody2d.mass * Time.fixedDeltaTime * unit.unit_status.rhythm * normal.y);
                if(force_x < 0f) {
                    force_x = 0f;
                }
                if(force_y < 0f) {
                    force_y = 0f;
                }
                move ( new Vector2 ( force_x * x, force_y * y ) );
                yield return new WaitForFixedUpdate ( );
            }
        }


        private float lerp_flying_dist;
        private float lerp_flying_speed;
        private float lerp_flying_previous_position;
        private float lerp_flying_time;
        private bool do_lerp_flying = false;
        private IEnumerator Elerp_flying ( float dist, float speed ) {
            lerp_flying_dist = dist;
            lerp_flying_speed = speed;
            lerp_flying_previous_position = unit.get_attech_point ( UUnit.AttechmentPoint.Origin ).localPosition.y;
            lerp_flying_time = 0f;
            if ( do_lerp_flying ) {
                yield break;
            }
            do_lerp_flying = true;
            while(do_lerp_flying) {
                lerp_flying_time += Time.fixedDeltaTime * lerp_flying_speed * unit.unit_status.rhythm;
                if(lerp_flying_time > 1f) {
                    lerp_flying_time = 1f;
                }
                unit.unit_status.flying = Mathf.Lerp ( lerp_flying_previous_position, lerp_flying_dist, lerp_flying_time );
                if (lerp_flying_time == 1f) {
                    break;
                }
                yield return new WaitForFixedUpdate ( );
            }

            do_lerp_flying = false;
        }

        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Update ( ) {
#if false
            if ( shadow_collider is CapsuleCollider2D ) {
                CapsuleCollider2D shadow = shadow_collider as CapsuleCollider2D;
                CapsuleCollider2D path = path_collider as CapsuleCollider2D;
                path.isTrigger = false;
                path.direction = shadow.direction;
                path.offset = shadow.offset;
                path.size = shadow.size * 0.99f;
            }
#endif

            if ( path_obj.layer != (int)path_type ) {    // Layer 조정
                set_path_type ( path_type );
            }
        }

        private void FixedUpdate ( ) {
            update_movement ( );
            update_flying ( );
            update_ground ( );
        }

        private void OnTriggerEnter2D ( Collider2D collision ) {
            UUnit unit = collision.GetComponentInParent<UUnit> ( );
            if(unit != null) {
                if ( !collisions.Contains ( unit ) ) {
                    collisions.Add ( unit );
                }
            }
            event_collision_enter?.Invoke ( collision );
        }

        private void OnTriggerExit2D ( Collider2D collision ) {
            UUnit unit = collision.GetComponentInParent<UUnit> ( );
            if ( unit != null ) {
                if( collisions.Contains ( unit ) ) {
                    collisions.Remove ( unit );
                }
            }

            event_collision_exit?.Invoke ( collision );
        }
    }

    public delegate void shadow_collision_enter ( Collider2D collision );
    public delegate void shadow_collision_exit ( Collider2D collision );
}
