using UnityEngine;
using System.Collections;

namespace Game.Management {
    using JToolkit.Utility;
    using Game.Stage;
    

    public class GameManager : Singleton<GameManager> {

        public int level;
        public GameStage current_stage;



        public void load_resource ( ) {
            // TODO : 추가 리소스들
            current_stage.load_resource ( );
        }


        public void confirm ( ) {
            
        }


        public void initialize ( ) {
            PlayerManager.instance.initialize ( );
            current_stage.initialize ( );
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

            if(instance == this) {
                confirm ( );
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
