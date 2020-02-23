using UnityEngine;
using System.Collections.Generic;

namespace Game.Management {
    using JToolkit.Utility;
    using Game.Unit;
    using Game.User;

    public class PlayerManager : Singleton<PlayerManager> {


        public List<Player> players = new List<Player> ( );


        


        public Player get_player ( Unit unit ) {
            Player player = null;

            players.ForEach ( p => {
                if(p.unit.Equals(unit)) {
                    player = p;
                }
            } );

            return player;
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
