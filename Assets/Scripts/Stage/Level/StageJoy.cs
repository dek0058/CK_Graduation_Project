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

            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Piano_Do_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Piano_Re_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Piano_Mi_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Piano_Pa_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Piano_Sol_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Piano_Ra_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Piano_Si_SFX);

            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Drum_Do_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Drum_Re_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Drum_Mi_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Drum_Pa_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Drum_Sol_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Drum_Ra_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Drum_Si_SFX);

            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Guitar_Do_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Guitar_Re_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Guitar_Mi_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Guitar_Pa_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Guitar_Sol_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Guitar_Ra_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Guitar_Si_SFX);
            
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Bass_Do_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Bass_Re_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Bass_Mi_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Bass_Pa_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Bass_Sol_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Bass_Ra_SFX);
            ResourceLoader.instance.add(ResourceLoader.Resource.Stage1_Bass_Si_SFX);
        }
    }
}