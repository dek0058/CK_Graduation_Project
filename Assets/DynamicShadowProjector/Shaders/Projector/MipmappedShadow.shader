Shader "DynamicShadowProjector/Projector/Mipmapped Shadow" {
	Properties {
		[NoScaleOffset] _ShadowTex ("Cookie", 2D) = "gray" {}
		_ClipScale ("Near Clip Sharpness", Float) = 100
		_Alpha ("Shadow Darkness", Range (0, 1)) = 1.0
		_Ambient ("Ambient", Range (0, 1)) = 0.3
		_DSPMipLevel ("Max Mip Level", float) = 4.0
		_Offset ("Offset", Range (-1, -10)) = -1.0
	}
	Subshader {
		Tags {"Queue"="Transparent-1"}
		Pass {
			Name "PASS"
			ZWrite Off
			ColorMask RGB
			Blend DstColor Zero
			Offset -1, [_Offset]

			CGPROGRAM
			#pragma vertex DSPProjectorVertMipmap
			#pragma fragment DSPProjectorFragMipmap
			#pragma multi_compile _ FSR_RECEIVER FSR_PROJECTOR_FOR_LWRP
			#pragma multi_compile_fog
			#pragma target 3.0
			#include "UnityCG.cginc"
			#include "DSPProjector.cginc"
			#include "DSPMipmappedShadow.cginc"
			ENDCG
		}
	}
	CustomEditor "DynamicShadowProjector.ProjectorShaderGUI"
}
