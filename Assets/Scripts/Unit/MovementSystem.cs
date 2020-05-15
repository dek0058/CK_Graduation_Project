using UnityEngine;
using System.Collections;

namespace Game.Unit {
    using Management;
    using JToolkit.Utility;

    public class MovementSystem : MonoBehaviour {

        public UUnit unit;
        public new Rigidbody rigidbody;

        private Vector3 next_velocity = Vector3.zero;
        private float fixedtimescale = 0f;
        private float flying = 0f;
        private float airborn = 0f;

        private bool grounded;
        public bool is_grounded {
            get => grounded;
        }

        public bool is_flying {
            get => flying > 0f;    
        }

        [SerializeField]
        private float radius = 0.05f;
        private float pre_ground_y = GameManager.World_Y_Position;


        /// <summary>
        /// 객체에게 속도를 부여합니다.
        /// </summary>
        /// <param name="velocity">속도</param>
        public void move ( Vector3 velocity ) {
            velocity.x = float.IsNaN(velocity.x) ? 0f : velocity.x;
            velocity.y = float.IsNaN ( velocity.y ) ? 0f : velocity.y;
            velocity.z = float.IsNaN ( velocity.z ) ? 0f : velocity.z;
            next_velocity += velocity;
        }


        public void move ( Vector2 velocity ) {
            velocity.x = float.IsNaN ( velocity.x ) ? 0f : velocity.x;
            velocity.y = float.IsNaN ( velocity.y ) ? 0f : velocity.y;
            next_velocity += new Vector3(velocity.x, 0f, velocity.y);
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



        /// <summary>
        /// MovementSystem class를 검증합니다.
        /// </summary>
        public void confirm ( ) {
            if( unit == null) {
                unit = GetComponent<UUnit> ( );
            }

            if(rigidbody == null) {
                rigidbody = GetComponent<Rigidbody> ( );
            }

            flying = unit.unit_status.flying;
            airborn = unit.unit_status.airborn;
            transform.position = new Vector3 ( transform.position.x, GameManager.World_Y_Position + flying + airborn, transform.position.z );
        }


        public void update_movement ( ) {  // Fixed
            rigidbody.velocity = next_velocity * fixedtimescale;
            next_velocity = Vector3.zero;
        }


        private void update_airborn ( ) {    // Fixed
            if ( airborn > 0f ) {
                unit.unit_status.airborn += (Physics.gravity.y * fixedtimescale);
            } else if ( airborn < 0f ) {
                unit.unit_status.airborn = 0f;
            }

            float amount = flying + airborn;
            float ground_y = pre_ground_y + GameManager.World_Y_Position + amount;

            Vector3 position = new Vector3 ( transform.position.x, ground_y, transform.position.z );
            transform.position = position;
        }


        public void update_grounded ( ) {   // Update
            Vector3 origin = transform.position + Vector3.up;
            Ray ray = new Ray ( origin, Vector3.down );
            RaycastHit hit;
            LayerMask layer = 1 << (int)GameLayer.Field_Both;
            layer |= 1 << (int)GameLayer.Field_Origin;
            layer |= 1 << (int)GameLayer.Field_Purgatory;


            bool collision = Physics.SphereCast ( ray, radius, out hit, Mathf.Infinity, layer );
            if (collision ) {
                if ( hit.normal.y >= 1.0f) {
                    grounded = true;
                    pre_ground_y = hit.point.y;
                    Debug.Log ( pre_ground_y );
                } else {
                    grounded = false;
                }

            } else {
                grounded = false;
            }
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

                force_x = force_x - (rigidbody.mass * Time.fixedDeltaTime * unit.unit_status.rhythm * normal.x);
                force_y = force_y - (rigidbody.mass * Time.fixedDeltaTime * unit.unit_status.rhythm * normal.y);
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
           if(unit == null) {
                confirm ( );
                if(unit == null) {
                    Debug.LogError ( "유닛을 찾을 수 없어 GameObject를 삭제합니다." );
                    Destroy ( gameObject );
                }
                return;
            }
            
            update_grounded ( );
        }

        private void FixedUpdate ( ) {
            if(unit == null) {
                return;
            }

            flying = unit.unit_status.flying;
            airborn = unit.unit_status.airborn;
            fixedtimescale = Time.fixedDeltaTime * unit.unit_status.rhythm;
            
            if(unit.is_pause) {
                return;
            }

            update_movement ( );
            update_airborn ( );
        }


        // Editor Mode
#if UNITY_EDITOR
        private void OnDrawGizmosSelected ( ) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere ( transform.position, radius );
        }
#endif
    }
}
