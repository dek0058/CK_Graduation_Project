using UnityEngine;

namespace Game.User {

    public class Player : MonoBehaviour {

        public enum Team {
            User,
            Npc,
            Enemy,
        }
        public Team team;

        [SerializeField]
        protected bool local = false;
        public bool is_local {
            get => local;
        }

        protected int alliance = 0;

        /// <summary>
        /// 플레이어를 생성합니다.
        /// </summary>
        public static Player create ( Team team, bool local = false ) {
            System.Type t = local ? typeof(LocalPlayer) : typeof ( Player );
            GameObject obj = new GameObject ( "Player ", t );
            Player p = obj.GetComponent<Player> ( );
            p.team = team;
            p.local = local;
            p.alliance |= 1 << (int)team;
            return p;
        }


        public void set_alliance ( Team team, bool value ) {
            if ( value ) {
                alliance |= 1 << (int)team;
            } else {
                alliance &= ~(1 << (int)team);
            }
        }


        /// <summary>
        /// 목표 플레이어가 동맹 플레이어인지 확인합니다.
        /// </summary>
        /// <param name="p">목표 플레이어</param>
        public bool is_alliance ( Player p ) {
            int bit = (alliance >> (int)team) & 1;
            return bit == 1 ? true : false;
        }

        /// <summary>
        /// 목표 플레이어가 적 플레이어인지 확인합니다.
        /// </summary>
        /// <param name="p">목표 플레이어</param>
        public bool is_enemy ( Player p ) {
            int bit = (alliance >> (int)team) & 1;
            return bit == 1 ? false : true;
        }


        public virtual void confirm ( ) {
        }


        protected virtual void update ( ) {
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
