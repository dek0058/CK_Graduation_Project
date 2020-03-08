using UnityEngine;
using Cinemachine;

namespace Game.User {
    public class GameCamera : MonoBehaviour {

        public CinemachineVirtualCamera cv_camera = null;
        public Transform confiner_target = null;


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
