using UnityEngine;
using Pathfinding;
using System.Collections;

namespace Game.AI {
    using Unit;
    
    [DisallowMultipleComponent(), RequireComponent ( typeof(Seeker) )]
    public class UnitAI : MonoBehaviour {

        public Transform target;
        public Vector3 target_position;

        public float limit_distance = 1f;

        public Seeker seeker;
        public UUnit unit;


        public bool is_active;


        public virtual void active ( ) {
            StartCoroutine ( Edispatch ( ) );
        }


        public virtual void inactive ( ) {
            is_active = false;
        }

        private Path path;
        private void on_path_complete ( Path p ) {
            if(p.error) {
                return;
            }
            path = p;
        }


        public virtual void confirm ( ) {
            if(seeker == null) {
                seeker = GetComponent<Seeker> ( );
            }

            if(unit == null) {
                unit = GetComponent<UUnit> ( );
            }

            is_active = false;
        }


        private IEnumerator Edispatch ( ) {
            if( is_active ) {
                yield break;
            }
            is_active = true;

            while ( is_active ) {
                seeker.StartPath ( unit.transform.position, target.position, on_path_complete );
                if ( path != null ) {
                    int cnt = path.vectorPath.Count;
                    Debug.Log ( cnt );
                    
                    
                    if ( cnt > 1) {
                        Vector2 direction = (path.vectorPath[1] - unit.transform.position).normalized;
                        float dist = Vector2.Distance ( unit.transform.position, target.position );
                        if(limit_distance >= dist) {
                            unit.set_order ( Order_Id.Move, false );
                        } else {
                            unit.set_order ( Order_Id.Move, true );
                            unit.move ( direction );
                        }
                    } else {
                        unit.set_order ( Order_Id.Move, false );
                    }
                }
                yield return new WaitForEndOfFrame ( );
            }

            unit.set_order ( Order_Id.Move, false );

        }

        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////


        private void Awake ( ) {
            confirm ( );
        }

        private void Update ( ) {
            /*
            if(Input.GetKeyDown(KeyCode.P)) {
                if(is_active) {
                    inactive ( );
                } else {
                    active ( );
                }
            }
            */
        }
    }
}
