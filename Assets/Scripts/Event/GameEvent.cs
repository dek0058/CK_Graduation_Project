using UnityEngine;

namespace Game.Event {
    public abstract class GameEvent : MonoBehaviour {

        public bool is_event;
        public abstract void execute ( );
    }
}
