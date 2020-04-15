using UnityEngine;

namespace Game {
    using Management;
    using Unit;

    [System.Serializable]
    public class GameInitializer {


        public void initialize ( bool load = false ) {
            if(load) {

            } else {
                UUnit unit = UUnit.create<Unit.Character.UProtagonist> ( Vector2.zero, PlayerManager.instance.local_player );
                PlayerManager.instance.local_player.unit = unit;
                PlayerManager.instance.game_camera.grey_camera.target = unit.transform;
            }
        }

    }
}
