using UnityEngine;

namespace Game.Stage {
    using Game.Management;
    using Game.User;
    using Game.Obj;

    public abstract class GameRoom : MonoBehaviour{

        [HideInInspector]
        public Transform position;
        [HideInInspector]
        public Transform parent_tilemap;
        [HideInInspector]
        public Transform parent_door;
        [HideInInspector]
        public Transform parent_object;
        [HideInInspector]
        public Transform parent_character;

        /// <summary>
        /// Room의 상태
        /// </summary>
        public enum State {
            Inactive = 0,       // 빙이 활성화 되어있지 않음
            Active,             // 방이 활성화 되어 있음
            Clear               // 방을 클리어하였음
        }
        public State state = State.Inactive;



        public void join ( Player p ) {
            Debug.Log ( gameObject.name+ "join" );
            CameraManager.instance.game_camera.set_collider ( position.GetComponent<PolygonCollider2D> ( ) );
            CameraManager.instance.game_camera.confiner_target = position;
            CameraManager.instance.game_camera.cv_camera.Follow = position;
        }


        public void quit ( Player p ) {
            Debug.Log ( gameObject.name + "quit" );
        }


        public void active ( bool active ) {
            if(active) {
                gameObject.SetActive ( true );
                active_on ( );

            } else {
                active_off ( );
                gameObject.SetActive ( false );
            }
        }


        protected virtual void active_on ( ) {
            if ( state != State.Clear ) {
                state = State.Active;
            }
            load ( );
        }

        protected virtual void active_off ( ) {
            if ( state == State.Active ) {
                state = State.Inactive;
            }
            save ( );
        }



        public virtual void save ( ) {
            
        }

        public virtual void load ( ) {
            
        }





        protected void update ( ) {

            switch ( state ) {
                case State.Inactive:


                    break;
                case State.Active:
                    
                    

                    break;
                case State.Clear:


                    break;
            }
        }


        /// <summary>
        /// Room을 검증합니다.
        /// </summary>
        public virtual void confirm ( ) {

            if( position == null ) {
                position = transform.Find ( "-Position" );
            }

            if (parent_tilemap == null) {
                parent_tilemap = transform.Find ( "Tilemap" );
            }

            if ( parent_object == null ) {
                parent_object = transform.Find ( "Door" );
            }

            if ( parent_object == null ) {
                parent_object = transform.Find ( "Object" );
            }

            if ( parent_character == null ) {
                parent_character = transform.Find ( "Character" );
            }
        }
    }
}
