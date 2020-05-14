using UnityEngine;
using System.Collections.Generic;

namespace Game.Stage {
    using Game.Unit;
    using Game.Management;

    public class GameStage : MonoBehaviour {

        public Transform start_point;
        
        public virtual void load_resource ( ) { // 게임씬에서 필요한 프리펩 로드
            ResourceLoader.instance.add ( ResourceLoader.Resource.Roa );
            ResourceLoader.instance.add ( ResourceLoader.Resource.Pomp );
        }


        public void initialize ( ) {
            UUnit unit = UUnit.create<Unit.Character.URoa> ( start_point.position, PlayerManager.instance.local_player );
            unit.transform.parent = GameManager.instance.Unit_Transform;
            unit.transform.localScale = new Vector3 ( 1f, 1f, 1f );
            PlayerManager.instance.local_player.unit = unit;

            GameManager.instance.game_camera.cv_camera.Follow = unit.transform;
        }





        private void confirm ( ) {
            
        }


        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Awake ( ) {
            confirm ( );
        }
    }
}
