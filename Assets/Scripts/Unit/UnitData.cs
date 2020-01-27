using UnityEngine;

namespace Game.Unit {

    [CreateAssetMenu(fileName = "NewUnitData", menuName = "Unit/Data")]
    public class UnitData : ScriptableObject {

        public uint id;                 // 고유 ID
        public string nickname;         // 이름

        public float hp;                // 체력
        public float mspeed;            // 이동속도
        public float aspeed;            // 공격속도
        public float rspeed;            // 회전속도
        public float damage;            // 공격력
        public float armor;             // 방어력

        // TODO : 추가 스텟 AND 추가해야할 능력부여
    }
}
