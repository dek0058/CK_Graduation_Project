using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game.Management {
    using JToolkit.Utility;
    using Ability;
    using Ability.Spell;
    using Unit;

    public class AbilityManager : Singleton<AbilityManager> {

        public readonly Dictionary<int, Ability> ability_list = new Dictionary<int, Ability> ( );

        public void initialize ( ) {
            ability_list.Add (  APurgatoryArea.ID, new APurgatoryArea ( ) );
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
