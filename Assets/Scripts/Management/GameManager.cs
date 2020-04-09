using UnityEngine;
using System.Collections;

namespace Game.Management {
    using JToolkit.Utility;
    using Game.Stage;
    

    public class GameManager : Singleton<GameManager> {

        public Transform allocation_area;

        public GameStage current_stage;
        public GameInitializer game_initializer;


        public void load_resource ( ) {
            // TODO : 추가 리소스들
            current_stage.load_resource ( );
        }


        public void confirm ( ) {

            if ( allocation_area == null ) {
                allocation_area = GameObject.FindGameObjectWithTag ( "AllocationArea" ).transform;
            }

            if( game_initializer  == null) {
                game_initializer = new GameInitializer ( );
            }
        }


        public void initialize ( ) {
            game_initializer.initialize ( );
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
