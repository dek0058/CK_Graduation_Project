using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Equipment {
    public class Equipment : MonoBehaviour {

        public EquipmentStatus status = new EquipmentStatus ( );

        public void confirm ( ) {
 
        }

        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Awake ( ) {
            confirm ( );
        }
    }

    [System.Serializable]
    public class EquipmentStatus {
        public float life;
        public float damage;
        public float armor;
        public float mspeed;
        public float aspeed;
    }

}
