Shader "Custom/SeeThroughTree"
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
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float3 _PlayerPos;
            float _Radius;
            float _Fade;
            float _SeeThroughStrength;
            float _ShouldSeeThrough;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                if (_ShouldSeeThrough < 0.5)
                    return col;

                float dist = distance(i.worldPos.xy, _PlayerPos.xy);
                float t = smoothstep(_Radius - _Fade, _Radius, dist);
                col.a *= lerp(_SeeThroughStrength, 1.0, t);

                return col;
            }
            ENDCG
        }
    }
}
