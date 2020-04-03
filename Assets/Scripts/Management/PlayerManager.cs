using UnityEngine;
using System.Collections.Generic;

namespace Game.Management {
    using JToolkit.Utility;
    using Game.Unit;
    using Game.User;

    public class PlayerManager : Singleton<PlayerManager> {

        public List<Player> players = new List<Player> ( );

        public Player local_player = null;

        public GameCamera game_camera;


        public Player create ( Player.Team team, bool local = false ) {
            Player p = Player.create ( team, local );
            players.Add ( p );
            string count = players.Count.ToString ( );
            p.name += local ? count + "(Local)" : count;
            return p;
        }


        public void load ( ) {
            players.Add ( create ( Player.Team.Enemy ) );   // Enemy 플레이어 생성
            players.Add ( create ( Player.Team.Npc ) );     // NPC 플레이어 생성
        }

        public void confirm ( ) {
            if(local_player == null) {
                local_player = create ( Player.Team.User, true );
            }



            /*
            Screen.width / height = 해상도 가져올 수 있음
            World Position -> 스크린 포지션으로

            Vector2 world_area = new Vector2(Screen.width/2, Screen.height/2);
            Vector3 screen_area = Camera.main.WorldToScreenPoint(world_area);
            
            카메라.position.x + screen_area.x  
            카메라.position.y + screen_area.y  = = = > A

            카메라.position.x - screen_area.x  
            카메라.position.y + screen_area.y  = = = > B

            카메라.position.x - screen_area.x  
            카메라.position.y - screen_area.y  = = = > C

            카메라.position.x + screen_area.x  
            카메라.position.y - screen_area.y  = = = > D

            B ㅡㅡㅡㅡㅡㅡㅡ A
            |               |
            |               |
            |               |
            C ㅡㅡㅡㅡㅡㅡㅡ D
            */
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
            game_camera.camera_point.position = local_player.unit.get_position();
        }


        private void OnDestroy ( ) {
            if ( instance == this ) { _app_is_close = true; }
        }

        private void OnApplicationQuit ( ) {
            if ( instance == this ) { _app_is_close = true; }
        }

    }
}
