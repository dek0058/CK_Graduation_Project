using UnityEngine;
using System.Collections.Generic;

namespace Game.Management {
    using JToolkit.Utility;
    using Game.Stage;

    public class StageManager : Singleton<StageManager> {

        public int level;

        public GameStage current_stage;



        public void load ( ) {
            
            current_stage.load_resource ( );
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
