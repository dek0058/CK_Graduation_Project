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
        [Range ( 0, 1f )]
        public float alpha = 1f;

#if UNITY_EDITOR
        public bool is_update = false;
        public Transform target;
#endif

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                                                 Unity                                                                ///
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void Update ( ) {
#if UNITY_EDITOR
            if ( !is_update) {
                return;
            }

            if( target ) {
                world_position = new Vector3 ( target.position.x, world_position.y, target.position.z );
            }
#endif
            Shader.SetGlobalVector ( "_world_pos", world_position );
            Shader.SetGlobalFloat ( "_radius", radius );
            Shader.SetGlobalFloat ( "_alpha", alpha );
        }


        private void OnEnable ( ) {
            if ( instance == null ) {
                instance = this;
            } else {
                if ( instance != this ) {
                    Destroy ( gameObject );
                }
            }
        }


        private void OnDestroy ( ) {
            if ( instance == this ) { _app_is_close = true; }
        }

        private void OnApplicationQuit ( ) {
            if ( instance == this ) { _app_is_close = true; }
        }
    }
}
