using UnityEngine;
using System.Collections.Generic;

namespace Game.Stage {
    using Game.Unit;
    using Game.Management;

    public class GameStage : MonoBehaviour {

        
        
        public virtual void load_resource ( ) { // 게임씬에서 필요한 프리펩 로드
            ResourceLoader.instance.add ( ResourceLoader.Resource.Roa );
        }


        public void initialize ( ) {
            
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
