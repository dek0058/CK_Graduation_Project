using UnityEngine;

namespace Game.Obj {
    using Game.Stage;
    using Game.User;
    using Game.Management;
    using Game.Unit;

    public class GameDoor : MonoBehaviour {

        [HideInInspector]
        public Transform position;
        
        [HideInInspector]
        public GameRoom current_room;
        public GameDoor connect_door;

        


        public void transportRoom ( Player p ) {
            quit ( p );
            connect_door.join ( p );
        }



        public void join ( Player p ) {

            Transform unit_transform = p.unit.transform;

            unit_transform.SetParent ( current_room.parent_character );
            unit_transform.position = position.position;


            current_room.join ( p );
        }
        
        public void quit ( Player p ) {
            current_room.quit ( p );
        }


        public void open ( ) {
        
        }


        public void close ( ) {
        
        }



        private void confirm ( ) {
            if ( position == null ) {
                position = transform.Find ( "-Position" );
            }

            if ( current_room == null ) {
                current_room = transform.parent.parent.GetComponent<GameRoom>();
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Awake ( ) {
            confirm ( );
        }



        private void OnTriggerEnter2D ( Collider2D collision ) {
            // HACK
            if (current_room.state != GameRoom.State.Clear) {
                return;
            }

            Unit u = collision.gameObject.GetComponent<Unit> ( );
            Player p = PlayerManager.instance.get_player ( u );

            

            // HACK
            if(p == null) {
                return;
            }

            transportRoom ( p );
        }
    }
}
