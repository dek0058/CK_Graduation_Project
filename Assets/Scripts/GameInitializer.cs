using UnityEngine;

namespace Game {
    using Management;
    using Unit;

    [System.Serializable]
    public class GameInitializer {


        public void initialize ( bool load = false ) {
            if(load) {

            } else {
                // HACK
                Vector2 start_position = Vector2.zero; // 지작지점

                UUnit unit = UUnit.create<Unit.Character.URoa> ( start_position, PlayerManager.instance.local_player );
                PlayerManager.instance.local_player.unit = unit;

                // HACK
                TestGaugeUI.instance.target = unit.transform;
            }
        }

    }
}
