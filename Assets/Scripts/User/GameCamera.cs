using UnityEngine;
using Cinemachine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Game.User {
    using Management;

    public class GameCamera : MonoBehaviour {

        public CinemachineVirtualCamera cv_camera;
        public CinemachineConfiner cv_confiner;

        public Transform camera_point;

        public PolygonCollider2D confiner_area;
        public Transform confiner_transform;

        public PixelPerfectCamera pixel_perfect_camera;


        private void pixel_perfect_adjust ( ) {
            pixel_perfect_camera.refResolutionX = Screen.width;
            pixel_perfect_camera.refResolutionY = Screen.height;
        }

        /// <summary>
        /// Game Camera를 검증합니다.
        /// </summary>
        public void confirm ( ) {
            if(cv_camera == null) {
                cv_camera = transform.GetComponentInChildren<CinemachineVirtualCamera> ( );
            }

            if( cv_confiner == null) {
                cv_confiner = transform.GetComponentInChildren<CinemachineConfiner> ( );
            }

            if( camera_point == null) {
                camera_point = GameObject.FindGameObjectWithTag ( "CameraPoint" ).transform;
            }

            if ( pixel_perfect_camera == null ) {
                pixel_perfect_camera = Camera.main.GetComponent<PixelPerfectCamera> ( );
                Preferences.instance.event_resolution_change += pixel_perfect_adjust;
            }

            cv_camera.Follow = camera_point;
        }

        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Awake ( ) {
            confirm ( );
        }

        private void LateUpdate ( ) {
            cv_camera.m_Lens.OrthographicSize = Camera.main.orthographicSize;
        }
    }
}
