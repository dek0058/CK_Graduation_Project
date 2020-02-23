using UnityEngine;
using Cinemachine;

namespace Game.User {
    public class GameCamera : MonoBehaviour {

        public CinemachineVirtualCamera cv_camera = null;
        public Transform confiner_target = null;


        private CinemachineConfiner cv_confiner = null;
        private CinemachineConfiner confiner {
            get {
                if(cv_confiner == null) {
                    if(cv_camera == null) {
                        confirm ( );
                    }
                    cv_confiner = cv_camera.GetComponent<CinemachineConfiner> ( );
                }
                return cv_confiner;
            }
        }


        public void set_collider ( PolygonCollider2D collider ) {
            Vector2[] vec2_indices = collider.GetPath ( 0 );
            collider.SetPath ( 0, vec2_indices );
        }


        /// <summary>
        /// Game Camera를 검증합니다.
        /// </summary>
        public void confirm ( ) {
            if(cv_camera == null) {
                cv_camera = transform.GetComponentInChildren<CinemachineVirtualCamera> ( );
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Awake ( ) {
            confirm ( );
        }
    }
}
