#if !defined(DSP_PROJECTOR_CGINC_INCLUDED)
#define DSP_PROJECTOR_CGINC_INCLUDED
#include "UnityCG.cginc"

struct DSP_V2F_PROJECTOR {
	float4 uvShadow : TEXCOORD0;
	half2 alpha     : COLOR;  // fixed precision is ok for most GPU, but I saw a problem on Tegra 3.
	UNITY_FOG_COORDS(1)
	float4 pos : SV_POSITION;
};

#if defined(FSR_PROJECTOR_FOR_LWRP)
CBUFFER_START(DSP_ProjectorTransform)
float4x4 _FSRWorldToProjector;
float4 _FSRWorldProjectDir;
CBUFFER_END
void fsrTransformVertex(float4 v, out float4 clipPos, out float4 shadowUV)
{
	float4 worldPos;
	worldPos.xyz = mul(unity_ObjectToWorld, v).xyz;
	worldPos.w = 1.0f;
#if defined(STEREO_CUBEMAP_RENDER_ON)
    worldPos.xyz += ODSOffset(worldPos.xyz, unity_HalfStereoSeparation.x);
#endif
	clipPos = mul(UNITY_MATRIX_VP, worldPos);
	shadowUV = mul(_FSRWorldToProjector, worldPos);
}
float3 fsrProjectorDir()
{
	return UnityWorldToObjectDir(_FSRWorldProjectDir.xyz);
}
#elif defined(FSR_RECEIVER) // FSR_RECEIVER keyword is used by Projection Receiver Renderer component which is contained in Fast Shadow Receiver.
CBUFFER_START(DSP_ProjectorTransform)
float4x4 _FSRProjector;
float4 _FSRProjectDir;
CBUFFER_END
void fsrTransformVertex(float4 v, out float4 clipPos, out float4 shadowUV)
{
	clipPos = UnityObjectToClipPos(v);
	shadowUV = mul(_FSRProjector, v);
}
float3 fsrProjectorDir()
{
	return _FSRProjectDir.xyz;
}
#else
CBUFFER_START(DSP_ProjectorTransform)
float4x4 unity_Projector;
float4x4 unity_ProjectorClip;
CBUFFER_END
void fsrTransformVertex(float4 v, out float4 clipPos, out float4 shadowUV)
{
	clipPos = UnityObjectToClipPos(v);
	shadowUV = mul (unity_Projector, v);
	shadowUV.z = mul (unity_ProjectorClip, v).x;
}
float3 fsrProjectorDir()
{
	return normalize(float3(unity_Projector[2][0],unity_Projector[2][1], unity_Projector[2][2]));
}
#endif

CBUFFER_START(DSP_ProjectorParams)
uniform float _ClipScale;  // compatible with Fast Shadow Receiver
uniform fixed _Alpha;      // compatible with Fast Shadow Receiver
uniform fixed _Ambient;    // compatible with Fast Shadow Receiver
CBUFFER_END

sampler2D _ShadowTex;
sampler2D _LightTex;

half DSPCalculateDiffuseLightAlpha(float4 vertex, float3 normal)
{
	float diffuse = -dot(normal, fsrProjectorDir());
	return _Alpha * diffuse;
}

half DSPCalculateDiffuseShadowAlpha(float4 vertex, float3 normal)
{
	float diffuse = -dot(normal, fsrProjectorDir());
	// this calculation is not linear. it is better to do in fragment shader. but in most case, it won't be a problem.
	return _Alpha * diffuse / (_Ambient + saturate(diffuse));
}

DSP_V2F_PROJECTOR DSPProjectorVertLightNoFalloff(float4 vertex : POSITION, float3 normal : NORMAL)
{
	DSP_V2F_PROJECTOR o;
	fsrTransformVertex(vertex, o.pos, o.uvShadow);
	o.alpha.x = _ClipScale * o.uvShadow.z;
	o.alpha.y = DSPCalculateDiffuseLightAlpha(vertex, normal);
	UNITY_TRANSFER_FOG(o, o.pos);
	return o;
}

DSP_V2F_PROJECTOR DSPProjectorVertNoFalloff(float4 vertex : POSITION, float3 normal : NORMAL)
{
	DSP_V2F_PROJECTOR o;
	fsrTransformVertex(vertex, o.pos, o.uvShadow);
	o.alpha.x = _ClipScale * o.uvShadow.z;
	o.alpha.y = DSPCalculateDiffuseShadowAlpha(vertex, normal);
	UNITY_TRANSFER_FOG(o, o.pos);
	return o;
}

DSP_V2F_PROJECTOR DSPProjectorVertLightLinearFalloff(float4 vertex : POSITION, float3 normal : NORMAL)
{
	DSP_V2F_PROJECTOR o;
	fsrTransformVertex(vertex, o.pos, o.uvShadow);
	float z = o.uvShadow.z;
	o.alpha.x = _ClipScale * z;
	o.alpha.y = DSPCalculateDiffuseLightAlpha(vertex, normal) * (1.0f - z); // falloff
	UNITY_TRANSFER_FOG(o, o.pos);
	return o;
}

DSP_V2F_PROJECTOR DSPProjectorVertLinearFalloff(float4 vertex : POSITION, float3 normal : NORMAL)
{
	DSP_V2F_PROJECTOR o;
	fsrTransformVertex(vertex, o.pos, o.uvShadow);
	float z = o.uvShadow.z;
	o.alpha.x = _ClipScale * z;
	o.alpha.y = DSPCalculateDiffuseShadowAlpha(vertex, normal);
	o.alpha.y *= (1.0f - z); // falloff
	UNITY_TRANSFER_FOG(o, o.pos);
	return o;
}

fixed DSPGetShadowAlpha(DSP_V2F_PROJECTOR i)
{
	return saturate(saturate(i.alpha.x)*i.alpha.y);
}

fixed4 DSPCalculateFinalLightColor(fixed4 texColor, DSP_V2F_PROJECTOR i)
{
	fixed alpha = saturate(saturate(i.alpha.x)*i.alpha.y);
	texColor.rgb *= alpha;
	UNITY_APPLY_FOG_COLOR(i.fogCoord, texColor, fixed4(0,0,0,0));
	return texColor;
}

fixed4 DSPCalculateFinalShadowColor(fixed4 texColor, DSP_V2F_PROJECTOR i)
{
	fixed alpha = DSPGetShadowAlpha(i);
	texColor.rgb = lerp(fixed3(1,1,1), texColor.rgb, alpha);
	UNITY_APPLY_FOG_COLOR(i.fogCoord, texColor, fixed4(1,1,1,1));
	return texColor;
}

fixed4 DSPProjectorFrag(DSP_V2F_PROJECTOR i) : SV_Target
{
	fixed4 shadow = tex2Dproj(_ShadowTex, UNITY_PROJ_COORD(i.uvShadow));
	return DSPCalculateFinalShadowColor(shadow, i);
}

fixed4 DSPProjectorFragLight(DSP_V2F_PROJECTOR i) : SV_Target
{
	fixed4 light = tex2Dproj(_LightTex, UNITY_PROJ_COORD(i.uvShadow));
	return DSPCalculateFinalLightColor(light, i);
}

fixed4 DSPProjectorFragLightWithShadow(DSP_V2F_PROJECTOR i) : SV_Target
{
	fixed4 light = tex2Dproj(_LightTex, UNITY_PROJ_COORD(i.uvShadow));
	fixed3 shadow = tex2Dproj(_ShadowTex, UNITY_PROJ_COORD(i.uvShadow));
	light.rgb *= shadow.rgb;
	return DSPCalculateFinalLightColor(light, i);
}
#endif // !defined(DSP_PROJECTOR_CGINC_INCLUDED)
