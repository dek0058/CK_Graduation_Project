using UnityEngine;

namespace Game.Unit {
    /// <summary>
    /// Unit이 가진 능력치를 나타냅니다.
    /// </summary>
    [System.Serializable]
    public class UnitStatus {
        public bool is_dead;
        public bool is_invincible;

        public float current_hp;            // 현재 체력       
        public float max_hp;                // 최대 체력

        public float rhythm;                // 유닛 속도
        public float mspeed;                // 이동 속도
        public float aspeed;                // 공격 속도


        public float damage;                // 공격력
        public float armor;                 // 방어력
        public float atime;                 // 공격 쿨타임



        // 현재 상태 값

        public float angle;                 // 유닛의 각도
        public float look_at;               // 바라볼 각도

        public Vector2 input;               // 바라봐야 할 방향
        public Vector2 direction;           // 현재 방향
        public Vector2 axis;                // 바라보고 있는 축

        public float flying;                // 기본 높이 (고정)
        public float current_flying;        // 현재 높이


        // 추가 스텟

        public float add_hp {               // 추가증가 체력
            get; private set;
        }
        public float rate_hp {              // 비율증가 체력
            get; private set;
        }


        public float add_mspeed {           // 추가증가 이동속도
            get; private set;
        }
        public float rate_mspeed {          // 비율증가 이동속도
            get; private set;
        }


        public float add_aspeed {           // 추가증가 공격속도
            get; private set;
        }
        public float rate_aspeed {          // 비율증가 공격속도
            get; private set;
        }

        
        public float add_damage {           // 추가증가 공격력
            get; private set;
        }
        public float rate_damage {          // 비율 증가 공격력
            get; private set;
        }


        public float add_armor {            // 추가증가 방어력
            get; private set;
        }
        public float rate_armor {           // 비율증가 방어력
            get; private set;
        }

        // TODO : 추가할 스텟들
    }
}