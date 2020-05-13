using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game.Ability {
    using Unit;
    using Spell;

    [Serializable]
    public class Ability {

        public virtual int id {
            get;
        }
        public bool is_passive = false;
        public OrderId order_id;


        public struct Info {
            public UUnit source;
            public UUnit target;
            public Vector3 position;
        }
        public Action<Info> event_use;
    }
}