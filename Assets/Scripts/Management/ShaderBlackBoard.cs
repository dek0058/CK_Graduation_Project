using UnityEngine;

namespace Game.Management {
    using JToolkit.Utility;

#if UNITY_EDITOR
    [ExecuteInEditMode]
#endif
    public class ShaderBlackBoard : Singleton<ShaderBlackBoard> {

        public Vector2 position;
        public float range;
        public float power;

        public bool is_update = false;


        public void set_position ( Vector2 position ) {
            this.position = position;
            Shader.SetGlobalVector ( "_Grey_Position", position );
        }

        public void set_range ( float range ) {
            this.range = range;
            Shader.SetGlobalFloat ( "_Grey_Range", range );
        }

        public void set_power ( float power ) {
            this.power = power;
            Shader.SetGlobalFloat ( "_Grey_Power", power );
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                                                 Unity                                                                ///
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void Update ( ) {
#if UNITY_EDITOR
            if ( !is_update) {
                return;
            }
            Shader.SetGlobalVector ( "_Grey_Position", position );
            Shader.SetGlobalFloat ( "_Grey_Range", range );
            Shader.SetGlobalFloat ( "_Grey_Power", power );
#endif
        }

    }
}
