
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Collections;
using System.Collections.Generic;

namespace Game.Unit {
    using Management;
    using JToolkit.Utility;

    public class MovementSystem : MonoBehaviour {

        public enum PathType {
            Origin_Ground = GameLayer.Origin_Path_Ground,
            Origin_Air = GameLayer.Origin_Path_Air,
            Purgatory_Ground = GameLayer.Purgatory_Path_Ground,
            Purgatory_Air = GameLayer.Purgatory_Path_Air,
        }

        public PathType path_type = PathType.Origin_Ground;
        
        public Collider2D shadow_collider;
        public Rigidbody2D rigidbody2d;
        public UUnit unit;

        // Shadow Object & Lighting
        [SerializeField]
        private GameObject path_obj;
        [SerializeField]
        private Collider2D path_collider;
        [SerializeField]
        private Light2D shadow_light;
        //

        private Vector2 next_velocity = Vector2.zero;

        public List<UUnit> collisions = new List<UUnit> ( );
        public event shadow_collision_enter event_collision_enter;
        public event shadow_collision_exit event_collision_exit;


        public bool is_grounded {
            get; private set;
        }
        public bool is_path_grounded {
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
            if(path_obj == null) {
                return;
            }
            path_type = type;
            path_obj.layer = (int)path_type;
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

            if ( path_obj == null ) {
                if ( transform.childCount > 0 ) {
                    path_obj = transform.GetChild ( 0 ).gameObject;
                }
            }

            if ( path_obj != null ) {
                if ( path_collider == null ) {
                    path_collider = path_obj.GetComponent<Collider2D> ( );
                }

                switch ( shadow_collider ) {
                    case BoxCollider2D box when shadow_collider is BoxCollider2D: {
                        if ( path_collider == null ) {
                            path_collider = path_obj.AddComponent<BoxCollider2D> ( );
                        } else if ( !(path_collider is BoxCollider2D) ) {
                            DestroyImmediate ( path_collider );
                            path_collider = path_obj.AddComponent<BoxCollider2D> ( );
                        }
                        BoxCollider2D path = path_collider as BoxCollider2D;
                        path.size = box.size * 0.99f;

                    }
                    break;
                    case CapsuleCollider2D capsule when shadow_collider is CapsuleCollider2D: {
                        if ( path_collider == null ) {
                            path_collider = path_obj.AddComponent<CapsuleCollider2D> ( );
                        } else if ( !(path_collider is CapsuleCollider2D) ) {
                            DestroyImmediate ( path_collider );
                            path_collider = path_obj.AddComponent<CapsuleCollider2D> ( );
                        }
                        CapsuleCollider2D path = path_collider as CapsuleCollider2D;
                        path.direction = capsule.direction;
                        path.size = capsule.size * 0.99f;

                    }
                    break;
                    case CircleCollider2D circle when shadow_collider is CircleCollider2D: {
                        if ( path_collider == null ) {
                            path_collider = path_obj.AddComponent<CircleCollider2D> ( );
                        } else if ( !(path_collider is CircleCollider2D) ) {
                            DestroyImmediate ( path_collider );
                            path_collider = path_obj.AddComponent<CircleCollider2D> ( );
                        }
                        CircleCollider2D path = path_collider as CircleCollider2D;
                        path.radius = circle.radius * 0.99f;

                    }
                    break;
                    case PolygonCollider2D polygon when shadow_collider is PolygonCollider2D: {
                        if ( path_collider == null ) {
                            path_collider = path_obj.AddComponent<PolygonCollider2D> ( );
                        } else if ( !(path_collider is PolygonCollider2D) ) {
                            DestroyImmediate ( path_collider );
                            path_collider = path_obj.AddComponent<PolygonCollider2D> ( );
                        }

                        PolygonCollider2D path = path_collider as PolygonCollider2D;
                        Vector2[] points = polygon.points;
                        for ( int i = 0; i < points.Length; ++i ) {
                            points[i] *= 0.99f;
                        }
                        path.points = points;

                    }
                    break;
                }
                path_collider.isTrigger = false;
                path_collider.offset = shadow_collider.offset;
            }

            if ( shadow_light == null ) {
                shadow_light = GetComponent<Light2D> ( );
            }

            set_path_type ( path_type );
            is_grounded = true;
        }


        private void update_layer ( ) {
            switch ( path_type ) {
                case PathType.Origin_Ground: is_path_grounded = true; break;
                case PathType.Origin_Air: is_path_grounded = false; break;
                case PathType.Purgatory_Ground: is_path_grounded = true; break;
                case PathType.Purgatory_Air: is_path_grounded = false; break;
            }

            if ( path_obj == null ) {
                return;
            }

            if ( path_obj.layer != (int)path_type ) {    // Layer 조정
                set_path_type ( path_type );
            }
        }


        private void update_shadow ( ) {
            if(shadow_light == null) {
                return;
            }
            shadow_light.intensity = GameManager.instance.gb_light.shadow_intensity;
        }


        private void update_movement ( ) {  // Fixed
            if(next_velocity == Vector2.zero) {
                rigidbody2d.velocity = Vector2.zero;
                return;
            }

            rigidbody2d.velocity = next_velocity * Time.fixedDeltaTime * unit.unit_status.rhythm;
            next_velocity = Vector3.zero;
        }


        private void update_flying ( ) {    // Fixed
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


        private void update_ground ( ) {    // Fixed
            int mask = 0;

            if ( path_type == PathType.Origin_Ground || path_type == PathType.Origin_Air ) {
                mask |= 1 << (int)GameLayer.Origin_Map_Ground;
                mask |= 1 << (int)GameLayer.Origin_Map_Cliff;
            } else if ( path_type == PathType.Purgatory_Ground || path_type == PathType.Purgatory_Air ) {
                mask |= 1 << (int)GameLayer.Purgatory_Map_Ground;
                mask |= 1 << (int)GameLayer.Purgatory_Map_Cliff;
            }

            Collider2D[] colliders = Physics2D.OverlapPointAll ( transform.position, mask );

            bool result = false;
            for ( int i = 0; i < colliders.Length; ++i ) {
                if ( colliders[i].gameObject.layer == (int)GameLayer.Origin_Map_Ground ) {
                    result = true; break;
                }
            }
            is_grounded = result;
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
            update_layer ( );
            update_shadow ( );
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
