using UnityEngine;
using System.Collections.Generic;

namespace Game.Stage {
    using Game.Unit;
    using Game.Management;

    public class GameStage : MonoBehaviour {

        public Dictionary<string, GameRoom> rooms = new Dictionary<string, GameRoom> ( );
        public GameRoom start_room;

        public GameRoom current_room;


        private bool do_transtion = false;


        
        public virtual void load_resource ( ) { // 게임씬에서 필요한 프리펩 로드
            ResourceLoader.instance.add ( ResourceLoader.Resource.Protagonist );
            ResourceLoader.instance.add ( ResourceLoader.Resource.Piano_Man );
            ResourceLoader.instance.add ( ResourceLoader.Resource.Melody_Missile );
        }


        public void initialize ( ) {
            foreach(var room in rooms.Values) {
                if(room == start_room) {
                    room.active ( );
                } else {
                    room.inactive ( );
                }
            }

            current_room = start_room;
        }


        public void transition_room ( GameRoom room, Unit unit, Vector3 position ) {
            StartCoroutine ( Etransition_room ( room, unit, position ) );
        }


        private void confirm ( ) {

            GameRoom[] room_array = transform.GetComponentsInChildren<GameRoom> ( );
            foreach(var room in room_array) {
                string name = room.name;
                int location = name.IndexOf ( "-" );
                if(location == -1 ) {
                    Debug.Log ( name + " 오브젝트의 이름이 잘못되어 있습니다." );
                    continue;
                }
                string pos = name.Substring ( location + 1 );
                if ( !rooms.ContainsKey ( pos ) ) {
                    rooms.Add ( pos, room );
                }
            }

            if ( start_room == null) {
                start_room = rooms["0,0"];
            }
        }



        public GameRoom get_room ( int x, int y ) {
            string pos = x + "," + y;
            return rooms.ContainsKey ( pos ) ? rooms[pos] : null;
        }


        private System.Collections.IEnumerator Etransition_room ( GameRoom room, Unit unit, Vector3 position ) {
            if(do_transtion) {  // 이미 방전환 중이므로
                yield break;
            }
            do_transtion = true;
            PlayerInput.instance.release_control ( );

            yield return StartCoroutine ( SceneFader.Efade_out ( SceneFader.FadeType.Blank ) );


            current_room.quit ( );
            current_room = room;
            current_room.join ( );
            unit.movement_system.set_position ( position );

            yield return StartCoroutine ( SceneFader.Efade_in ( ) );

            PlayerInput.instance.gain_control ( );
            do_transtion = false;
        }


        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Awake ( ) {
            confirm ( );
        }

        private void Start ( ) {
            initialize ( );
        }
    }
}
