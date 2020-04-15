using UnityEngine;

namespace Game.Unit
{

    [CreateAssetMenu(fileName = "NewUnitData", menuName = "Unit/Data")]
    public class UnitData : ScriptableObject
    {
        public UnitTableData unit_table_data;
    }

    [System.Serializable]
    public struct UnitTableData
    {
        public static int GetGoogleSheetGID() { return 2079198335; }

        public uint id;                 // 고유 ID
        public string nickname;         // 이름

        public float hp;                // 체력
        public float mspeed;            // 이동속도
        public float aspeed;            // 공격속도
        public float rspeed;            // 회전속도
        public float damage;            // 공격력
        public float armor;             // 방어력
        public float atime;             // 공격 쿨타임

        // TODO : 추가 스텟 AND 추가해야할 능력부여

        public UnitTableData(string _id, string _nickname, string _hp, string _mspeed, string _aspeed, string _rspeed, string _damage, string _armor, string _atime)
        {
            id = uint.Parse(_id);
            nickname = _nickname;
            hp = float.Parse(_hp);
            mspeed = float.Parse(_mspeed);
            aspeed = float.Parse(_aspeed);
            rspeed = float.Parse(_rspeed);
            damage = float.Parse(_damage);
            armor = float.Parse(_armor);
            atime = float.Parse(_atime);
        }
    }
}