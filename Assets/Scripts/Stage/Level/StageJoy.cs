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

            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room01_Do_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room01_Re_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room01_Mi_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room01_Pa_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room01_Sol_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room01_Ra_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room01_Si_SFX);

            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room02_Sound01_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room02_Sound02_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room02_Sound03_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room02_Sound04_SFX);

            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room03_Cello01_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room03_Cello02_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room03_Cello03_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room03_Cello04_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room03_Cello05_SFX);

            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room03_Piano01_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room03_Piano02_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room03_Piano03_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room03_Piano04_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room03_Piano05_SFX);

            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room03_Trumpet01_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room03_Trumpet02_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room03_Trumpet03_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room03_Trumpet04_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room03_Trumpet05_SFX);

            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room03_Violin01_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room03_Violin02_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room03_Violin03_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room03_Violin04_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Room03_Violin05_SFX);
        }
    }
}