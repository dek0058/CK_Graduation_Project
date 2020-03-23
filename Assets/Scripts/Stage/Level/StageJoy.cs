using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Stage.Level {
    using Game.Management;
    
    public class StageJoy : GameStage {




        public override void load_resource ( ) {
            base.load_resource ( );
            ResourceLoader.instance.add ( ResourceLoader.Resource.Stage1_Joy_Music );
            ResourceLoader.instance.add ( ResourceLoader.Resource.Stage1_Boss_Music );
        }

    }
}
