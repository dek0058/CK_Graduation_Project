using UnityEngine;

namespace Game.Management {
    using JToolkit.Utility;
    using Game.User;

    public class CameraManager : Singleton<CameraManager> {

        private static GameCamera Game_Camera = null;
        public GameCamera game_camera {
            get {
                if(Game_Camera == null) {
                    Game_Camera = GameObject.FindGameObjectWithTag ( "GameCamera" ).GetComponent<GameCamera> ( );
                    if(Game_Camera == null) {
                        // TODO : 게임 카메라 생성
                    }
                }
                return Game_Camera;
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////


        private void OnEnable ( ) {
            if ( instance == null ) {
                instance = this;
            } else {
                if ( instance != this ) {
                    Destroy ( gameObject );
                }
            }
        }


        private void OnDestroy ( ) {
            if ( instance == this ) { _app_is_close = true; }
        }

        private void OnApplicationQuit ( ) {
            if ( instance == this ) { _app_is_close = true; }
        }
    }
}
