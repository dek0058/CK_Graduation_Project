Shader "DynamicShadowProjector/FastShadowReceiver/Mipmapped Shadow For Lightmap Subtractive" {
	Properties {
		[NoScaleOffset] _ShadowTex ("Cookie", 2D) = "gray" {}
		_ClipScale ("Near Clip Sharpness", Float) = 100
		_Alpha ("Shadow Darkness", Range (0, 1)) = 1.0
		_AmbientColor ("Ambient Color", Color) = (0.3, 0.3, 0.3, 1.0)
		_DSPMipLevel ("Max Mip Level", float) = 4.0
		_Offset ("Offset", Range (-1, -10)) = -1.0
	}
	Subshader {
		Tags {"Queue"="Transparent-1" "IgnoreProjector"="True"}
		Pass {
			Name "PASS"
			ZWrite Off
			ColorMask RGB
			Blend DstColor Zero
			Offset -1, [_Offset]

			CGPROGRAM
			#pragma vertex DSPProjectorVertMipmapForLightmap
			#pragma fragment DSPProjectorFragMipmapForLightmapSubtractive
			#pragma multi_compile _ FSR_RECEIVER FSR_PROJECTOR_FOR_LWRP
			#pragma multi_compile ___ UNITY_HDR_ON
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
