Shader "Custom/ToonShader_Inside"
{
    Properties
    {
        Color_38A9CEB8("MainColor", Color) = (0.682353, 0.6784314, 0.682353, 1)
        [HDR]Color_4792AA26("AmbientColor", Color) = (0.5840786, 0.5840786, 0.5840786, 1)
        Vector1_40222094("Glossiness", Float) = 32
        [HDR]Color_F4B60D4F("SpecularColor", Color) = (1, 1, 1, 1)
        [HDR]Color_67F0D29("RimColor", Color) = (1, 1, 1, 1)
        Vector1_634D3D56("RimAmount", Range(0, 1)) = 0.7
        Vector1_F7463A14("RimTreshold", Range(0, 1)) = 0.1
        [NoScaleOffset]Texture2D_D6AFCF99("Texture", 2D) = "white" {}
        Vector2_CF11CA8("Texture Tiling", Vector) = (1, 1, 0, 0)
        Vector1_3A5BAD49("AlphaClip", Float) = 0.25
        Vector1_1A78205("Shadow Intensity", Range(0, 1)) = 0
    }
        SubShader
        {
            Tags
            {
                "RenderPipeline" = "UniversalPipeline"
                "RenderType" = "Opaque"
                "Queue" = "Geometry+0"
            }

            Stencil {
                Ref 5
                Comp NotEqual
                Pass keep
            }

            Pass
            {
                Name "Pass"
                Tags
                {
            // LightMode: <None>
        }

        // Render State
        Blend One Zero, One Zero
        Cull Off
        ZTest LEqual
        ZWrite On
            // ColorMask: <None>


            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            // Keywords
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma shader_feature _ _SAMPLE_GI
            // GraphKeywords: <None>

            // Defines
            #define _AlphaClip 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_POSITION_WS 
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define SHADERPASS_UNLIT

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float4 Color_38A9CEB8;
            float4 Color_4792AA26;
            float Vector1_40222094;
            float4 Color_F4B60D4F;
            float4 Color_67F0D29;
            float Vector1_634D3D56;
            float Vector1_F7463A14;
            float2 Vector2_CF11CA8;
            float Vector1_3A5BAD49;
            float Vector1_1A78205;
            CBUFFER_END
            TEXTURE2D(Texture2D_D6AFCF99); SAMPLER(samplerTexture2D_D6AFCF99); float4 Texture2D_D6AFCF99_TexelSize;
            SAMPLER(_SampleTexture2D_2ABA6378_Sampler_3_Linear_Repeat);

            // Graph Functions

            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
            {
                Out = UV * Tiling + Offset;
            }

            // 57669e6a907a915d8aa041e49a58a21e
            #include "Assets/Settings/Custom Functions/CustomLighting.hlsl"

            struct Bindings_CustonLightToon_b5afdcf188259423b9f0e4cf37a4919f
            {
            };

            void SG_CustonLightToon_b5afdcf188259423b9f0e4cf37a4919f(float3 Vector3_ABA21BA4, Bindings_CustonLightToon_b5afdcf188259423b9f0e4cf37a4919f IN, out half3 Direction_0, out half3 Color_1, out half DistanceAtten_2, out half ShadowAtten_3)
            {
                float3 _Property_AA73B26_Out_0 = Vector3_ABA21BA4;
                half3 _CustomFunction_B294BFE5_Direction_1;
                half3 _CustomFunction_B294BFE5_Color_2;
                half _CustomFunction_B294BFE5_DistanceAtten_3;
                half _CustomFunction_B294BFE5_ShadowAtten_4;
                MainLight_half(_Property_AA73B26_Out_0, _CustomFunction_B294BFE5_Direction_1, _CustomFunction_B294BFE5_Color_2, _CustomFunction_B294BFE5_DistanceAtten_3, _CustomFunction_B294BFE5_ShadowAtten_4);
                Direction_0 = _CustomFunction_B294BFE5_Direction_1;
                Color_1 = _CustomFunction_B294BFE5_Color_2;
                DistanceAtten_2 = _CustomFunction_B294BFE5_DistanceAtten_3;
                ShadowAtten_3 = _CustomFunction_B294BFE5_ShadowAtten_4;
            }

            void Unity_Normalize_float3(float3 In, out float3 Out)
            {
                Out = normalize(In);
            }

            void Unity_DotProduct_float3(float3 A, float3 B, out float Out)
            {
                Out = dot(A, B);
            }

            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }

            void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
            {
                Out = smoothstep(Edge1, Edge2, In);
            }

            void Unity_Add_float(float A, float B, out float Out)
            {
                Out = A + B;
            }

            void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
            {
                Out = A * B;
            }

            void Unity_Add_float3(float3 A, float3 B, out float3 Out)
            {
                Out = A + B;
            }

            void Unity_Power_float(float A, float B, out float Out)
            {
                Out = pow(A, B);
            }

            void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
            {
                Out = A * B;
            }

            void Unity_Subtract_float(float A, float B, out float Out)
            {
                Out = A - B;
            }

            // Graph Vertex
            // GraphVertex: <None>

            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float3 WorldSpaceNormal;
                float3 WorldSpaceViewDirection;
                float3 AbsoluteWorldSpacePosition;
                float4 uv0;
            };

            struct SurfaceDescription
            {
                float3 Color;
                float Alpha;
                float AlphaClipThreshold;
            };

            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float2 _Property_57D2A45F_Out_0 = Vector2_CF11CA8;
                float2 _TilingAndOffset_7C972BDD_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, _Property_57D2A45F_Out_0, float2 (0, 0), _TilingAndOffset_7C972BDD_Out_3);
                float4 _SampleTexture2D_2ABA6378_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_D6AFCF99, samplerTexture2D_D6AFCF99, _TilingAndOffset_7C972BDD_Out_3);
                float _SampleTexture2D_2ABA6378_R_4 = _SampleTexture2D_2ABA6378_RGBA_0.r;
                float _SampleTexture2D_2ABA6378_G_5 = _SampleTexture2D_2ABA6378_RGBA_0.g;
                float _SampleTexture2D_2ABA6378_B_6 = _SampleTexture2D_2ABA6378_RGBA_0.b;
                float _SampleTexture2D_2ABA6378_A_7 = _SampleTexture2D_2ABA6378_RGBA_0.a;
                float4 _Property_6878758E_Out_0 = Color_38A9CEB8;
                float4 _Property_1FB4195D_Out_0 = Color_4792AA26;
                Bindings_CustonLightToon_b5afdcf188259423b9f0e4cf37a4919f _CustonLightToon_817A5C69;
                half3 _CustonLightToon_817A5C69_Direction_0;
                half3 _CustonLightToon_817A5C69_Color_1;
                half _CustonLightToon_817A5C69_DistanceAtten_2;
                half _CustonLightToon_817A5C69_ShadowAtten_3;
                SG_CustonLightToon_b5afdcf188259423b9f0e4cf37a4919f(IN.AbsoluteWorldSpacePosition, _CustonLightToon_817A5C69, _CustonLightToon_817A5C69_Direction_0, _CustonLightToon_817A5C69_Color_1, _CustonLightToon_817A5C69_DistanceAtten_2, _CustonLightToon_817A5C69_ShadowAtten_3);
                float3 _Normalize_F5962C7A_Out_1;
                Unity_Normalize_float3(IN.WorldSpaceNormal, _Normalize_F5962C7A_Out_1);
                float _DotProduct_5150B6B9_Out_2;
                Unity_DotProduct_float3(_Normalize_F5962C7A_Out_1, _CustonLightToon_817A5C69_Direction_0, _DotProduct_5150B6B9_Out_2);
                float _Multiply_67E5056D_Out_2;
                Unity_Multiply_float(_DotProduct_5150B6B9_Out_2, _CustonLightToon_817A5C69_ShadowAtten_3, _Multiply_67E5056D_Out_2);
                float _Smoothstep_AC7072B8_Out_3;
                Unity_Smoothstep_float(0, 0.01, _Multiply_67E5056D_Out_2, _Smoothstep_AC7072B8_Out_3);
                float _Property_CCD530A8_Out_0 = Vector1_1A78205;
                float _Add_CFB3A05E_Out_2;
                Unity_Add_float(_Smoothstep_AC7072B8_Out_3, _Property_CCD530A8_Out_0, _Add_CFB3A05E_Out_2);
                float3 _Multiply_1F04D33E_Out_2;
                Unity_Multiply_float(_CustonLightToon_817A5C69_Color_1, (_Add_CFB3A05E_Out_2.xxx), _Multiply_1F04D33E_Out_2);
                float3 _Add_2F51ED5C_Out_2;
                Unity_Add_float3((_Property_1FB4195D_Out_0.xyz), _Multiply_1F04D33E_Out_2, _Add_2F51ED5C_Out_2);
                float4 _Property_69BDDB41_Out_0 = Color_F4B60D4F;
                float3 _Normalize_3310E6D8_Out_1;
                Unity_Normalize_float3(IN.WorldSpaceViewDirection, _Normalize_3310E6D8_Out_1);
                float3 _Add_A8399A65_Out_2;
                Unity_Add_float3(_CustonLightToon_817A5C69_Direction_0, _Normalize_3310E6D8_Out_1, _Add_A8399A65_Out_2);
                float3 _Normalize_B8657118_Out_1;
                Unity_Normalize_float3(_Add_A8399A65_Out_2, _Normalize_B8657118_Out_1);
                float _DotProduct_2DECA4F3_Out_2;
                Unity_DotProduct_float3(_Normalize_F5962C7A_Out_1, _Normalize_B8657118_Out_1, _DotProduct_2DECA4F3_Out_2);
                float _Multiply_79957313_Out_2;
                Unity_Multiply_float(_Smoothstep_AC7072B8_Out_3, _DotProduct_2DECA4F3_Out_2, _Multiply_79957313_Out_2);
                float _Property_19DC3E9C_Out_0 = Vector1_40222094;
                float _Power_90199A9B_Out_2;
                Unity_Power_float(_Property_19DC3E9C_Out_0, 2, _Power_90199A9B_Out_2);
                float _Power_88926877_Out_2;
                Unity_Power_float(_Multiply_79957313_Out_2, _Power_90199A9B_Out_2, _Power_88926877_Out_2);
                float _Smoothstep_E3C9D213_Out_3;
                Unity_Smoothstep_float(0.005, 0.01, _Power_88926877_Out_2, _Smoothstep_E3C9D213_Out_3);
                float4 _Multiply_1E08D292_Out_2;
                Unity_Multiply_float(_Property_69BDDB41_Out_0, (_Smoothstep_E3C9D213_Out_3.xxxx), _Multiply_1E08D292_Out_2);
                float3 _Add_AAABB604_Out_2;
                Unity_Add_float3(_Add_2F51ED5C_Out_2, (_Multiply_1E08D292_Out_2.xyz), _Add_AAABB604_Out_2);
                float4 _Property_61C0DAB6_Out_0 = Color_67F0D29;
                float _Property_2E106FBE_Out_0 = Vector1_634D3D56;
                float _Subtract_EF19FE8D_Out_2;
                Unity_Subtract_float(_Property_2E106FBE_Out_0, 0.01, _Subtract_EF19FE8D_Out_2);
                float _Add_39478444_Out_2;
                Unity_Add_float(_Property_2E106FBE_Out_0, 0.01, _Add_39478444_Out_2);
                float _DotProduct_9FB73F4E_Out_2;
                Unity_DotProduct_float3(_Normalize_F5962C7A_Out_1, _Normalize_3310E6D8_Out_1, _DotProduct_9FB73F4E_Out_2);
                float _Subtract_A569663B_Out_2;
                Unity_Subtract_float(1, _DotProduct_9FB73F4E_Out_2, _Subtract_A569663B_Out_2);
                float _Property_1B9013B7_Out_0 = Vector1_F7463A14;
                float _Power_CCF0E8A5_Out_2;
                Unity_Power_float(_DotProduct_5150B6B9_Out_2, _Property_1B9013B7_Out_0, _Power_CCF0E8A5_Out_2);
                float _Multiply_48E47E17_Out_2;
                Unity_Multiply_float(_Subtract_A569663B_Out_2, _Power_CCF0E8A5_Out_2, _Multiply_48E47E17_Out_2);
                float _Smoothstep_35137146_Out_3;
                Unity_Smoothstep_float(_Subtract_EF19FE8D_Out_2, _Add_39478444_Out_2, _Multiply_48E47E17_Out_2, _Smoothstep_35137146_Out_3);
                float4 _Multiply_5EB7E8A3_Out_2;
                Unity_Multiply_float(_Property_61C0DAB6_Out_0, (_Smoothstep_35137146_Out_3.xxxx), _Multiply_5EB7E8A3_Out_2);
                float3 _Add_3A4FF4AD_Out_2;
                Unity_Add_float3(_Add_AAABB604_Out_2, (_Multiply_5EB7E8A3_Out_2.xyz), _Add_3A4FF4AD_Out_2);
                float3 _Multiply_920E4A2_Out_2;
                Unity_Multiply_float((_Property_6878758E_Out_0.xyz), _Add_3A4FF4AD_Out_2, _Multiply_920E4A2_Out_2);
                float3 _Multiply_9C4937B7_Out_2;
                Unity_Multiply_float((_SampleTexture2D_2ABA6378_RGBA_0.xyz), _Multiply_920E4A2_Out_2, _Multiply_9C4937B7_Out_2);
                float _Property_2BC58D6D_Out_0 = Vector1_3A5BAD49;
                surface.Color = _Multiply_9C4937B7_Out_2;
                surface.Alpha = _SampleTexture2D_2ABA6378_A_7;
                surface.AlphaClipThreshold = _Property_2BC58D6D_Out_0;
                return surface;
            }

            // --------------------------------------------------
            // Structs and Packing

            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };

            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS;
                float3 normalWS;
                float4 texCoord0;
                float3 viewDirectionWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };

            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                float3 interp00 : TEXCOORD0;
                float3 interp01 : TEXCOORD1;
                float4 interp02 : TEXCOORD2;
                float3 interp03 : TEXCOORD3;
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };

            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                output.interp00.xyz = input.positionWS;
                output.interp01.xyz = input.normalWS;
                output.interp02.xyzw = input.texCoord0;
                output.interp03.xyz = input.viewDirectionWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }

            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                output.positionWS = input.interp00.xyz;
                output.normalWS = input.interp01.xyz;
                output.texCoord0 = input.interp02.xyzw;
                output.viewDirectionWS = input.interp03.xyz;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }

            // --------------------------------------------------
            // Build Graph Inputs

            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

                // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
                float3 unnormalizedNormalWS = input.normalWS;
                const float renormFactor = 1.0 / length(unnormalizedNormalWS);


                output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph


                output.WorldSpaceViewDirection = input.viewDirectionWS; //TODO: by default normalized in HD, but not in universal
                output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
                output.uv0 = input.texCoord0;
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                return output;
            }


            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"

            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }

                // Render State
                Blend One Zero, One Zero
                Cull Off
                ZTest LEqual
                ZWrite On
                // ColorMask: <None>


                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                // Debug
                // <None>

                // --------------------------------------------------
                // Pass

                // Pragmas
                #pragma prefer_hlslcc gles
                #pragma exclude_renderers d3d11_9x
                #pragma target 2.0
                #pragma multi_compile_instancing

                // Keywords
                #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                // GraphKeywords: <None>

                // Defines
                #define _AlphaClip 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD0
                #define VARYINGS_NEED_TEXCOORD0
                #define SHADERPASS_SHADOWCASTER

                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

                // --------------------------------------------------
                // Graph

                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float4 Color_38A9CEB8;
                float4 Color_4792AA26;
                float Vector1_40222094;
                float4 Color_F4B60D4F;
                float4 Color_67F0D29;
                float Vector1_634D3D56;
                float Vector1_F7463A14;
                float2 Vector2_CF11CA8;
                float Vector1_3A5BAD49;
                float Vector1_1A78205;
                CBUFFER_END
                TEXTURE2D(Texture2D_D6AFCF99); SAMPLER(samplerTexture2D_D6AFCF99); float4 Texture2D_D6AFCF99_TexelSize;
                SAMPLER(_SampleTexture2D_2ABA6378_Sampler_3_Linear_Repeat);

                // Graph Functions

                void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                {
                    Out = UV * Tiling + Offset;
                }

                // Graph Vertex
                // GraphVertex: <None>

                // Graph Pixel
                struct SurfaceDescriptionInputs
                {
                    float4 uv0;
                };

                struct SurfaceDescription
                {
                    float Alpha;
                    float AlphaClipThreshold;
                };

                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    float2 _Property_57D2A45F_Out_0 = Vector2_CF11CA8;
                    float2 _TilingAndOffset_7C972BDD_Out_3;
                    Unity_TilingAndOffset_float(IN.uv0.xy, _Property_57D2A45F_Out_0, float2 (0, 0), _TilingAndOffset_7C972BDD_Out_3);
                    float4 _SampleTexture2D_2ABA6378_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_D6AFCF99, samplerTexture2D_D6AFCF99, _TilingAndOffset_7C972BDD_Out_3);
                    float _SampleTexture2D_2ABA6378_R_4 = _SampleTexture2D_2ABA6378_RGBA_0.r;
                    float _SampleTexture2D_2ABA6378_G_5 = _SampleTexture2D_2ABA6378_RGBA_0.g;
                    float _SampleTexture2D_2ABA6378_B_6 = _SampleTexture2D_2ABA6378_RGBA_0.b;
                    float _SampleTexture2D_2ABA6378_A_7 = _SampleTexture2D_2ABA6378_RGBA_0.a;
                    float _Property_2BC58D6D_Out_0 = Vector1_3A5BAD49;
                    surface.Alpha = _SampleTexture2D_2ABA6378_A_7;
                    surface.AlphaClipThreshold = _Property_2BC58D6D_Out_0;
                    return surface;
                }

                // --------------------------------------------------
                // Structs and Packing

                // Generated Type: Attributes
                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    float4 uv0 : TEXCOORD0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };

                // Generated Type: Varyings
                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float4 texCoord0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };

                // Generated Type: PackedVaryings
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    float4 interp00 : TEXCOORD0;
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };

                // Packed Type: Varyings
                PackedVaryings PackVaryings(Varyings input)
                {
                    PackedVaryings output = (PackedVaryings)0;
                    output.positionCS = input.positionCS;
                    output.interp00.xyzw = input.texCoord0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }

                // Unpacked Type: Varyings
                Varyings UnpackVaryings(PackedVaryings input)
                {
                    Varyings output = (Varyings)0;
                    output.positionCS = input.positionCS;
                    output.texCoord0 = input.interp00.xyzw;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }

                // --------------------------------------------------
                // Build Graph Inputs

                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





                    output.uv0 = input.texCoord0;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                    return output;
                }


                // --------------------------------------------------
                // Main

                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"

                ENDHLSL
            }

            Pass
            {
                Name "DepthOnly"
                Tags
                {
                    "LightMode" = "DepthOnly"
                }

                    // Render State
                    Blend One Zero, One Zero
                    Cull Off
                    ZTest LEqual
                    ZWrite On
                    ColorMask 0


                    HLSLPROGRAM
                    #pragma vertex vert
                    #pragma fragment frag

                    // Debug
                    // <None>

                    // --------------------------------------------------
                    // Pass

                    // Pragmas
                    #pragma prefer_hlslcc gles
                    #pragma exclude_renderers d3d11_9x
                    #pragma target 2.0
                    #pragma multi_compile_instancing

                    // Keywords
                    // PassKeywords: <None>
                    // GraphKeywords: <None>

                    // Defines
                    #define _AlphaClip 1
                    #define ATTRIBUTES_NEED_NORMAL
                    #define ATTRIBUTES_NEED_TANGENT
                    #define ATTRIBUTES_NEED_TEXCOORD0
                    #define VARYINGS_NEED_TEXCOORD0
                    #define SHADERPASS_DEPTHONLY

                    // Includes
                    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                    #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

                    // --------------------------------------------------
                    // Graph

                    // Graph Properties
                    CBUFFER_START(UnityPerMaterial)
                    float4 Color_38A9CEB8;
                    float4 Color_4792AA26;
                    float Vector1_40222094;
                    float4 Color_F4B60D4F;
                    float4 Color_67F0D29;
                    float Vector1_634D3D56;
                    float Vector1_F7463A14;
                    float2 Vector2_CF11CA8;
                    float Vector1_3A5BAD49;
                    float Vector1_1A78205;
                    CBUFFER_END
                    TEXTURE2D(Texture2D_D6AFCF99); SAMPLER(samplerTexture2D_D6AFCF99); float4 Texture2D_D6AFCF99_TexelSize;
                    SAMPLER(_SampleTexture2D_2ABA6378_Sampler_3_Linear_Repeat);

                    // Graph Functions

                    void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                    {
                        Out = UV * Tiling + Offset;
                    }

                    // Graph Vertex
                    // GraphVertex: <None>

                    // Graph Pixel
                    struct SurfaceDescriptionInputs
                    {
                        float4 uv0;
                    };

                    struct SurfaceDescription
                    {
                        float Alpha;
                        float AlphaClipThreshold;
                    };

                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                    {
                        SurfaceDescription surface = (SurfaceDescription)0;
                        float2 _Property_57D2A45F_Out_0 = Vector2_CF11CA8;
                        float2 _TilingAndOffset_7C972BDD_Out_3;
                        Unity_TilingAndOffset_float(IN.uv0.xy, _Property_57D2A45F_Out_0, float2 (0, 0), _TilingAndOffset_7C972BDD_Out_3);
                        float4 _SampleTexture2D_2ABA6378_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_D6AFCF99, samplerTexture2D_D6AFCF99, _TilingAndOffset_7C972BDD_Out_3);
                        float _SampleTexture2D_2ABA6378_R_4 = _SampleTexture2D_2ABA6378_RGBA_0.r;
                        float _SampleTexture2D_2ABA6378_G_5 = _SampleTexture2D_2ABA6378_RGBA_0.g;
                        float _SampleTexture2D_2ABA6378_B_6 = _SampleTexture2D_2ABA6378_RGBA_0.b;
                        float _SampleTexture2D_2ABA6378_A_7 = _SampleTexture2D_2ABA6378_RGBA_0.a;
                        float _Property_2BC58D6D_Out_0 = Vector1_3A5BAD49;
                        surface.Alpha = _SampleTexture2D_2ABA6378_A_7;
                        surface.AlphaClipThreshold = _Property_2BC58D6D_Out_0;
                        return surface;
                    }

                    // --------------------------------------------------
                    // Structs and Packing

                    // Generated Type: Attributes
                    struct Attributes
                    {
                        float3 positionOS : POSITION;
                        float3 normalOS : NORMAL;
                        float4 tangentOS : TANGENT;
                        float4 uv0 : TEXCOORD0;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        uint instanceID : INSTANCEID_SEMANTIC;
                        #endif
                    };

                    // Generated Type: Varyings
                    struct Varyings
                    {
                        float4 positionCS : SV_POSITION;
                        float4 texCoord0;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        uint instanceID : CUSTOM_INSTANCE_ID;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                        #endif
                    };

                    // Generated Type: PackedVaryings
                    struct PackedVaryings
                    {
                        float4 positionCS : SV_POSITION;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        uint instanceID : CUSTOM_INSTANCE_ID;
                        #endif
                        float4 interp00 : TEXCOORD0;
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                        #endif
                    };

                    // Packed Type: Varyings
                    PackedVaryings PackVaryings(Varyings input)
                    {
                        PackedVaryings output = (PackedVaryings)0;
                        output.positionCS = input.positionCS;
                        output.interp00.xyzw = input.texCoord0;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        output.instanceID = input.instanceID;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        output.cullFace = input.cullFace;
                        #endif
                        return output;
                    }

                    // Unpacked Type: Varyings
                    Varyings UnpackVaryings(PackedVaryings input)
                    {
                        Varyings output = (Varyings)0;
                        output.positionCS = input.positionCS;
                        output.texCoord0 = input.interp00.xyzw;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        output.instanceID = input.instanceID;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        output.cullFace = input.cullFace;
                        #endif
                        return output;
                    }

                    // --------------------------------------------------
                    // Build Graph Inputs

                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                    {
                        SurfaceDescriptionInputs output;
                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





                        output.uv0 = input.texCoord0;
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                    #else
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                    #endif
                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                        return output;
                    }


                    // --------------------------------------------------
                    // Main

                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"

                    ENDHLSL
                }

        }
            FallBack "Hidden/Shader Graph/FallbackError"
}
