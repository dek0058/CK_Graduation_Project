using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game.Ability {
    using Unit;

    [Serializable]
    public class Ability {

        public virtual int id {
            get;
        }
        public bool is_passive = false;
        public OrderId order_id;

        


        





        public readonly static Dictionary<int, Ability> ability_list = new Dictionary<int, Ability> {
            
        };

    }
}