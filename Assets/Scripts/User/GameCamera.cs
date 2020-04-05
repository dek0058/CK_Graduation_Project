using UnityEngine;
using Cinemachine;

namespace Game.User {
    public class GameCamera : MonoBehaviour {

        public GreyCamera grey_camera;
        
        public CinemachineVirtualCamera cv_camera;
        public CinemachineConfiner cv_confiner;

        public Transform camera_point;

        public PolygonCollider2D confiner_area;
        public Transform confiner_transform;

        CinemachinePixelPerfect a;

        /// <summary>
        /// Game Camera를 검증합니다.
        /// </summary>
        public void confirm ( ) {
            if(grey_camera == null) {
                grey_camera = GetComponentInChildren<GreyCamera> ( );
            }

            if(cv_camera == null) {
                cv_camera = transform.GetComponentInChildren<CinemachineVirtualCamera> ( );
            }

            if( cv_confiner == null) {
                cv_confiner = transform.GetComponentInChildren<CinemachineConfiner> ( );
            }

            if( camera_point == null) {
                camera_point = GameObject.FindGameObjectWithTag ( "CameraPoint" ).transform;
            }

            cv_camera.Follow = camera_point;
        }

        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Awake ( ) {
            confirm ( );
        }
    }
}
