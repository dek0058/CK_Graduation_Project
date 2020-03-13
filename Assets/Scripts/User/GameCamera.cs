using UnityEngine;
using Cinemachine;

namespace Game.User {
    public class GameCamera : MonoBehaviour {

        public CinemachineVirtualCamera cv_camera = null;
        public Transform camera_point;

        [Header ( "테스트용(Hack)" )]
        public Unit.Unit source;
        public Unit.Unit target;

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
