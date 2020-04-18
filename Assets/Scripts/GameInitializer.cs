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
                Vector2 start_position = GameManager.instance.current_stage.start_room.transform.position; // 지작지점

                UUnit unit = UUnit.create<Unit.Character.UProtagonist> ( start_position, PlayerManager.instance.local_player );
                PlayerManager.instance.local_player.unit = unit;
                PlayerManager.instance.game_camera.grey_camera.target = unit.transform;
            }
        }

    }
}
