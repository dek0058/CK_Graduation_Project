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

    /// <summary>
    /// Unity Object Layer
    /// </summary>
    public enum GameLayer {
        Default = 0,
        TransparentFX,
        IgnoreRaycast,
        Water = 4,
        UI,

        // Unit
        Unit_Collider = 8,
        Unit_Collision,
        Unit_Shadow,

        Origin_Unit_Collider,
        Origin_Unit_Collision,
        Origin_Unit_Shadow,

        Purgatory_Unit_Collider,
        Purgatory_Unit_Collision,
        Purgatory_Unit_Shadow,

        // Movement
        Origin_Path_Ground,
        Origin_Path_Air,
        Purgatory_Path_Ground,
        Purgatory_Path_Air,

        // Object
        Origin_Door,
        Purgatory_Door,


        // Map
        Origin_Map_Ground = 25,
        Origin_Map_Cliff,
        Origin_Map_Border,
        Purgatory_Map_Ground,
        Purgatory_Map_Cliff,
        Purgatory_Map_Border,
    }
}