using UnityEngine;
using System;

namespace Game.Management {
    using JToolkit.Utility;
    public class Preferences : Singleton<Preferences> {


        public GameResolution game_resolution = new GameResolution ( );

        public event Action event_resolution_change;


        public void set_resolution ( GameResolutionType type ) {
            game_resolution.set ( type );
            event_resolution_change?.Invoke ( );
        }

        public void set_fullscreen ( bool value ) {
            game_resolution.fullscreen ( value );
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

        Unit_Collider = 8,
        Unit_Collision,
        Unit_Shadow,


        Door = 13,

        Path_Ground = 21,
        Path_Air,

        Map_Ground = 27,
        Map_Cliff,
        Map_Border,
    }
}