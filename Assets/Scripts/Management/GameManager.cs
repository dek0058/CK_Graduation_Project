using UnityEngine;
using System.Collections;

namespace Game.Management {
    using JToolkit.Utility;
    using Stage;
    using Unit;

    public class GameManager : Singleton<GameManager> {
        public const float World_Y_Position = 0.02f;

        public Transform Unit_Transform;


        public GameCamera game_camera = new GameCamera();
        public GameStage current_stage = null;
        public GameInitializer game_initializer = new GameInitializer ( );
        public GamePostProcessing post_processing = new GamePostProcessing();

        public bool is_complete = false;


        public void load_resource ( ) {
            // TODO : 추가 리소스들
            current_stage.load_resource ( );
        }


        public void initialize ( ) {
            PlayerManager.instance.initialize ( );
            AbilityManager.instance.initialize ( );
            game_initializer.initialize ( );
            current_stage?.initialize ( );

            is_complete = true;
        }


        public void unit_pause(bool value) {
            UUnit[] unit = Unit_Transform.GetComponentsInChildren<UUnit> ( );
        }


        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private IEnumerator Start ( ) {
            yield return new WaitUntil ( ( ) => ResourceLoader.instance.is_complete );
            initialize ( );
        }

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
