using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Event {
    using Management;

    public class EventStageJoyStart : MonoBehaviour {

        public void execute ( ) {
            GameManager.instance.unit_pause ( true );

        }

    }
}