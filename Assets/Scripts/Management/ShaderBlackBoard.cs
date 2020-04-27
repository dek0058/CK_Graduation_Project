using UnityEngine;

namespace Game.Management {
    using JToolkit.Utility;

#if UNITY_EDITOR
    [ExecuteInEditMode]
#endif
    public class ShaderBlackBoard : Singleton<ShaderBlackBoard> {

        public Texture2D mask_texture;
        public Vector2 mask_position;
        public float mask_scale;

        [Range(0, 1f)]
        public float mask_alpha;
        [Range(0, 1f)]
        public float grey_alpha;
        

        public bool is_update = false;


        public void set_mask_position ( Vector2 position ) {
            this.mask_position = position;
            Shader.SetGlobalVector ( "_GreyPos", position );
        }

        public void set_grey_alpha ( float a ) {
            this.grey_alpha = a;
            Shader.SetGlobalFloat ( "_GreyAlpha", a );
        }

        public void set_mask_scale ( float scale ) {
            this.mask_scale = scale;
            Shader.SetGlobalFloat ( "_MaskScale", scale );
        }

        public void set_mask_alpha ( float a ) {
            this.mask_alpha = a;
            Shader.SetGlobalFloat ( "_MaskAlpha", a );
        }

        public void set_texture ( Texture2D texture ) {
            this.mask_texture = texture;
            Shader.SetGlobalTexture ( "_MaskTex", texture );
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                                                 Unity                                                                ///
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void Start ( ) {
            set_texture ( mask_texture );
            set_mask_position ( mask_position );
            set_mask_scale ( mask_scale );
            set_mask_alpha ( mask_alpha );
            set_grey_alpha ( grey_alpha );
        }

        private void Update ( ) {
#if UNITY_EDITOR
            if ( !is_update) {
                return;
            }
            set_texture ( mask_texture );
            set_mask_position ( mask_position );
            set_mask_scale ( mask_scale );
            set_mask_alpha ( mask_alpha );
            set_grey_alpha ( grey_alpha );
#endif
        }

    }
}
