using UnityEngine;

namespace Game.User {
    using Game.Management;
    using JToolkit.Utility;
    using Game.Unit;
    using Game.Unit.Type;

    public class Player : MonoBehaviour {

        public Unit unit;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                                                 Unity                                                                ///
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        private void Update ( ) {

            // HACK : 임시
            Vector2 axis = new Vector2 (
                Singleton<PlayerInput>.instance.horizontal.value,
                Singleton<PlayerInput>.instance.vertical.value
            );


        }
    }
}
