using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Game {
    [System.Serializable]
    public class GlobalLight {

        public Light2D global_light;


        public float intensity {
            get => global_light.intensity;
            set => global_light.intensity = value;
        }


        public float shadow_intensity {
            get => intensity / 3f;
        }

    }
}
