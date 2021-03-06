﻿using UnityEngine;

namespace Game.Ability.Spell {
    using Management;
    using Unit;
    using Effect;

    public class APurgatoryArea : Ability {

        public const int ID = (int)OrderId.PurgatoryArea;
        public override int id => ID;

        public PurgatoryArea effect;

        public APurgatoryArea ( ) {
            order_id = OrderId.PurgatoryArea;
            cooltime = 0.1f;
            ignore_doing = true;
            event_use += use;
            effect = create ( ).GetComponent<PurgatoryArea> ( );
            effect.active ( false );
        }


        private void use ( Info info ) {
            if(!effect.is_active) {
                effect.active ( true );
                effect.source = info.source;
                ShaderBlackBoard.instance.alpha = 1f;
                ShaderBlackBoard.instance.radius = 2.8f;
            } else {
                effect.active ( false );
                effect.source = null;
                ShaderBlackBoard.instance.alpha = 0f;
                ShaderBlackBoard.instance.radius = 0f;
            }
        }


        private GameObject create ( ) {
            GameObject obj = GameObject.Instantiate ( ResourceLoader.instance.get_prefab ( ResourceLoader.Resource.Purgatory_Area ) as GameObject, GameManager.instance.Unit_Transform );
            float radius = ShaderBlackBoard.instance.radius;
            obj.transform.localScale = new Vector3 ( radius, radius, radius );
            return obj;
        }
    }
}