// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Toon"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "white" {}
        _BandTex("Band LUT", 2D) = "white" {}
        _Outline_Bold("Outline Bold", Range(0, 1)) = 0.1
        _Brightness("Brightness", Range(0, 1)) = 0.3
        _Strength("Strength", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        Cull Back
        CGPROGRAM
        #pragma surface surf _BandedLighting

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BandTex;
        };

        struct SurfaceOutputCustom
        {
            fixed3 Albedo;
            fixed3 Normal;
            fixed3 Emission;
            half Specular;
            fixed Gloss;
            fixed Alpha;
            float3 BandLUT;
        };

        sampler2D _MainTex;
        sampler2D _BandTex;

        fixed4 _Color;
        float _Brightness;
        float _Strength;

        void surf (Input IN, inout SurfaceOutputCustom o)
        {
            float4 Main = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = Main.rgb;
            o.Alpha = 1.0;

            float4 LUT = tex2D(_BandTex, IN.uv_BandTex);
            o.BandLUT = LUT.rgb;
        }

        
        float4 Lighting_BandedLighting(SurfaceOutputCustom s, float3 lightDir, float3 viewDir, float atten)
        {
            float NdotL = max(0.0, dot(s.Normal, lightDir)) * _Strength + _Brightness;
            float3 diffuse = tex2D(_BandTex, float2(NdotL, 0.5)).rgb ;

            float scolor;
            float3 v = normalize(lightDir + viewDir);
            float HdotN = saturate(dot(v, s.Normal));
            float pHdotN = pow(HdotN, 500.0);
            //scolor = pHdotN * 1.0;

            float smooth = smoothstep(0.005, 0.01, pHdotN);
            scolor = smooth * 1.0;

            float4 color;
            color.rgb = ((s.Albedo * _Color) + scolor) * diffuse * _LightColor0.rgb * atten;
            color.a = s.Alpha;

            return color;
        }
        ENDCG

        // Pass to render object as a shadow caster
        Pass
        {
            Cull Back
            Name "CastShadow"
            Tags { "LightMode" = "ShadowCaster" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"

            struct v2f
            {
                V2F_SHADOW_CASTER;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }

            float4 frag(v2f i) : COLOR
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }

        
        Pass
        {
            Cull Front
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            //sampler2D _MainTex;
            //float4 _MainTex_ST;
            float _Outline_Bold;

            v2f vert(appdata v)
            {
                v2f o;

                float3 normal = normalize(v.normal);
                float3 position = v.vertex + normal * (_Outline_Bold * 0.1);

                o.vertex = UnityObjectToClipPos(position);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return 0.0;
            }
            ENDCG
        }
        
    }
}
