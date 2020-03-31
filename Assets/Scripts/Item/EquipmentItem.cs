using UnityEngine;

namespace Game.Item {
    public interface IEquipmentItem {

        EquipmentType type {
            get; set;
        }

        EquipmentStatus status {
            get; set;
        }

        void action ( EquipmentAction action );
        void cancel ( );
    }

    /// <summary>
    /// 장비 아이템 종류
    /// </summary>
    public enum EquipmentType {
        Weapon,
    }

    /// <summary>
    /// 장비 아이템 액션 (사용 효과)
    /// </summary>
    public enum EquipmentAction {
        Attack,
        Block,
        /// TODO : . . .
    }

    [System.Serializable]
    public class EquipmentStatus {
        public float damage;
        public float armor;
        public float life;
        public float mspeed;
        public float aspeed;
    }
}
