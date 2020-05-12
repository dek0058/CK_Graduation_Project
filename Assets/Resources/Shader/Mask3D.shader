Shader "Custom/Mask3D" {
    Properties{}

    SubShader {

        Tags { 
            "RenderType" = "Opaque" 
        }
        
        Stencil {
            Ref 5
            Comp Always
            pass Replace
        }

        Pass{
            ZWrite Off
            ColorMask 0
            Cull off
        }
    }
}