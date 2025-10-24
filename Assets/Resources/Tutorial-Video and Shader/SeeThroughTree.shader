Shader "Custom/SeeThroughTree_URP2D"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PlayerPos ("Player Position", Vector) = (0,0,0,0)
        _Radius ("Radius", Float) = 0.5
        _Fade ("Fade Width", Float) = 0.3
        _SeeThroughStrength ("SeeThrough Alpha", Range(0,1)) = 0.2
        _ShouldSeeThrough ("Should See Through", Float) = 1.0
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "RenderType"="Transparent" 
            "RenderPipeline"="UniversalPipeline"
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            Tags { "LightMode" = "Universal2D" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                half2 lightingUV : TEXCOORD2;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;

            TEXTURE2D(_ShapeLightTexture0);
            SAMPLER(sampler_ShapeLightTexture0);
            TEXTURE2D(_ShapeLightTexture1);
            SAMPLER(sampler_ShapeLightTexture1);
            TEXTURE2D(_ShapeLightTexture2);
            SAMPLER(sampler_ShapeLightTexture2);
            TEXTURE2D(_ShapeLightTexture3);
            SAMPLER(sampler_ShapeLightTexture3);

            half4 _ShapeLightBlendFactors0;
            half4 _ShapeLightBlendFactors1;
            half4 _ShapeLightMaskFilter0;
            half4 _ShapeLightMaskFilter1;
            half4 _ShapeLightInvertedFilter0;
            half4 _ShapeLightInvertedFilter1;

            float3 _PlayerPos;
            float _Radius;
            float _Fade;
            float _SeeThroughStrength;
            float _ShouldSeeThrough;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = TransformObjectToWorld(v.vertex.xyz);
                o.lightingUV = ComputeScreenPos(o.vertex).xy;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                // Apply see-through effect
                if (_ShouldSeeThrough > 0.5)
                {
                    float dist = distance(i.worldPos.xy, _PlayerPos.xy);
                    float t = smoothstep(_Radius - _Fade, _Radius, dist);
                    col.a *= lerp(_SeeThroughStrength, 1.0, t);
                }

                // Sample 2D lights
                half4 shapeLight0 = SAMPLE_TEXTURE2D(_ShapeLightTexture0, sampler_ShapeLightTexture0, i.lightingUV);
                half4 shapeLight1 = SAMPLE_TEXTURE2D(_ShapeLightTexture1, sampler_ShapeLightTexture1, i.lightingUV);
                half4 shapeLight2 = SAMPLE_TEXTURE2D(_ShapeLightTexture2, sampler_ShapeLightTexture2, i.lightingUV);
                half4 shapeLight3 = SAMPLE_TEXTURE2D(_ShapeLightTexture3, sampler_ShapeLightTexture3, i.lightingUV);

                // Blend lights
                half4 light = shapeLight0 * _ShapeLightBlendFactors0.x;
                light += shapeLight1 * _ShapeLightBlendFactors0.y;
                light += shapeLight2 * _ShapeLightBlendFactors0.z;
                light += shapeLight3 * _ShapeLightBlendFactors0.w;

                // Apply lighting to color
                col.rgb *= light.rgb;

                return col;
            }
            ENDHLSL
        }
    }
    
    Fallback "Sprites/Default"
}