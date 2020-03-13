using UnityEngine;
using System.Collections.Generic;

namespace Game.Stage {
    using Game.Management;
    using Game.User;
    using Game.Obj;

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


        public virtual void active ( ) {
            
        }


        public virtual void inactive ( ) {
        
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

           
        }
    }
}
