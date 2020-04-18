using UnityEngine;
using System.Collections.Generic;

namespace Game.Management {
    using JToolkit.Utility;
    using Game.Unit;
    using Game.User;

    public class PlayerManager : Singleton<PlayerManager> {

        public List<Player> players = new List<Player> ( );
        public LocalPlayer local_player = null;
        public GameCamera game_camera;


        public Player create ( Player.Team team, bool local = false ) {
            Player p = Player.create ( team, local );
            p.transform.SetParent ( transform );
            players.Add ( p );
            string count = players.Count.ToString ( );
            p.name += local ? count + "(Local)" : count;
            return p;
        }

        public void initialize ( ) {
            if(local_player == null) {
                local_player = create ( Player.Team.User, true ) as LocalPlayer;
            }

            create ( Player.Team.Enemy );   // Enemy 플레이어 생성
            create ( Player.Team.Npc );     // NPC 플레이어 생성
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


        private void LateUpdate ( ) {
            // HACK
            if ( local_player != null && local_player.unit != null ) {
                game_camera.camera_point.position = local_player.unit.get_position ( );
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
