using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Item {
    using Unit;

    public class Item : MonoBehaviour {

        public enum ItemType {
            Normal,
            Consumable,
            Equipment,
        }
        public ItemType item_type;

        public Unit owner;



        public event on_use event_use;





        public virtual void confirm ( ) {
            
        }


        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Awake ( ) {
            confirm ( );
        }
    }

    public delegate void on_use ( );
}