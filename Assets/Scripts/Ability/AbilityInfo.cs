using UnityEngine;

namespace Game.Ability {
    using Unit;

    public class AbilityInfo {
        public Ability ability;
        public float cooltime = 0f;

        public bool is_passive {
            get => ability.is_passive;
        }

        public OrderId order_id {
            get => ability.order_id;
        }

        public int id {
            get => ability.id;
        }


        public bool is_cooltime {
            get => cooltime > 0f;
        }


        public AbilityInfo ( Ability ability ) {
            this.ability = ability;
        }
    }
}
