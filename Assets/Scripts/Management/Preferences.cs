using UnityEngine;

namespace Game.Management {
    using JToolkit.Utility;
    public class Preferences : Singleton<Preferences> {






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

            if ( instance == this ) {
                DontDestroyOnLoad ( gameObject );
            }
        }


        private void OnDestroy ( ) {
            if ( instance == this ) { _app_is_close = true; }
        }

        private void OnApplicationQuit ( ) {
            if ( instance == this ) { _app_is_close = true; }
        }
    }

    public enum GameLayer {
        Default = 0,
        TransparentFX,
        IgnoreRaycast,
        Water = 4,
        UI,

        VFX = 9,
        Shadow,
        
        Object = 12,
        Door,

        Unit = 15,

        Field = 26,
        Ground,
        Cliff,
        MapCollider,

        Room = 31       // END
    }
}