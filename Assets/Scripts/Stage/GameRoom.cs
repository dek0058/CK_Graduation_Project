using UnityEngine;
using System.Collections.Generic;

namespace Game.Stage {
    using Management;
    using User;
    using Obj;

    public abstract class GameRoom : MonoBehaviour{


        /// <summary>
        /// Room의 상태
        /// </summary>
        public enum State {
            Inactive = 0,       // 빙이 활성화 되어있지 않음
            Active,             // 방이 활성화 되어 있음
            Clear               // 방을 클리어하였음
        }
        public State state = State.Inactive;


        public List<GameDoor> doors = new List<GameDoor> ( );



        public void join ( ) {
            active ( );

        }


        public void quit ( ) {

            inactive ( );
        }

        public virtual void save ( ) {

        }

        public virtual void load ( ) {

        }


        protected virtual void active ( ) {
            GameDoor.State s = state == State.Clear ? GameDoor.State.Open : GameDoor.State.Close;
            foreach ( var d in doors ) {
                if ( d.state != GameDoor.State.Broken ) {
                    d.set_state ( s );
                }
            }
        }


        protected virtual void inactive ( ) {
        
        }



        protected virtual void update ( ) {
        }


        /// <summary>
        /// Room을 검증합니다.
        /// </summary>
        public virtual void confirm ( ) {

           
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
    }
}
