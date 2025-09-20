Shader "Custom/GlowBeam"
{
    Properties
    {
        _MainTex ("Main Texture (Gradient)", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _Color ("Glow Color", Color) = (1,0.4,0.1,0.6)
        _Speed ("Scroll Speed", Float) = 1
        _Distortion ("Distortion Strength", Range(0,0.2)) = 0.05
        _TimeValue ("Time Value", Float) = 0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend One One // Additive
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _Speed, _Distortion, _TimeValue;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float t = _TimeValue * _Speed;

                float2 noiseUV = i.uv + float2(t, 0);
                float noise = tex2D(_NoiseTex, frac(noiseUV)).r;

                float2 distortedUV = i.uv + float2(0, noise * _Distortion);

                fixed4 texCol = tex2D(_MainTex, frac(distortedUV));
                return texCol * _Color;
            }
            ENDCG
        }
    }
}
