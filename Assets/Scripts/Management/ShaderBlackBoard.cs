using UnityEngine;

namespace Game.Management {
    using JToolkit.Utility;

#if UNITY_EDITOR
    [ExecuteInEditMode]
#endif
    public class ShaderBlackBoard : Singleton<ShaderBlackBoard> {


        public Vector3 world_position = Vector3.zero;
        [Range(0, 100f)]
        public float radius = 2f;
        [Range(0, 100f)]
        public float softness = 0f;


        public bool is_update = false;


        

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                                                 Unity                                                                ///
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void Start ( ) {
           
        }

        private void Update ( ) {
#if UNITY_EDITOR
            if ( !is_update) {
                return;
            }

            Shader.SetGlobalVector ( "_world_pos", world_position );
            Shader.SetGlobalFloat ( "_radius", radius );
            Shader.SetGlobalFloat ( "_softness", softness );
#endif
        }

    }
}
