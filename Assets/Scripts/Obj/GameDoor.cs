using UnityEngine;
using Spine.Unity;

namespace Game.Obj {
    using Game.Stage;
    using Game.User;
    using Game.Management;
    using Game.Unit;

    public class GameDoor : MonoBehaviour {

        public enum State {
            Close = 0,
            Open,
            Broken,
        }
        public State state = State.Close;

        
        public GameRoom my_room = null;
        public GameRoom next_room;


        [HideInInspector]
        public Transform teleport_transform = null;

        public Collider2D block_collider;
        public Collider2D trigger_collider;


        public void set_state ( State state ) {
            this.state = state;
        }


        public void set_nextroom ( GameRoom room, Vector3 position ) {
            next_room = room;
            teleport_transform.position = position;
        }


        protected virtual void open ( ) {
            if(block_collider != null) {
                block_collider.enabled = false;
            }
            trigger_collider.enabled = true;
        }


        protected virtual void close ( ) {
            if ( block_collider != null ) {
                block_collider.enabled = true;
            }
            trigger_collider.enabled = false;
        }


        protected virtual void transition ( Collider2D collision ) {
        }


        protected virtual void update ( ) {
        }

        /// <summary>
        /// GameDoor를 검증합니다.
        /// </summary>
        protected virtual void confirm ( ) {

            if( my_room  == null) {
                my_room = transform.parent.GetComponent<GameRoom> ( );
            }

            if ( teleport_transform == null ) {
                teleport_transform = transform.GetChild ( 0 );
                if(teleport_transform == null) {
                    GameObject obj = new GameObject ( "Teleport Position" );
                    obj.transform.SetParent ( transform );
                    obj.transform.position = my_room.transform.position;
                    teleport_transform = obj.transform;
                }
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////
        private void Awake ( ) {
            confirm ( );
        }


        private void Update ( ) {
            update ( );
        }


        private void OnTriggerEnter2D ( Collider2D collision ) {
            transition ( collision );
        }
    }
}
