Shader "Custom/Fire2D_Waves"
{
    Properties
    {
        _MainTex ("Main Texture (Gradient)", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _Color1 ("Bottom Color", Color) = (1,1,0,1)
        _Color2 ("Top Color", Color) = (1,0,0,0)
        _Speed ("Flame Speed", Float) = 2
        _Distortion ("Distortion Strength", Float) = 0.1
        _WaveStrength ("Wave Strength", Float) = 0.05
        _WaveFrequency ("Wave Frequency", Float) = 10.0
        _TimeValue ("Time Value", Float) = 0
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha
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
            fixed4 _Color1, _Color2;
            float _Speed, _Distortion, _TimeValue;
            float _WaveStrength, _WaveFrequency;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
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

                // Прокрутка шума вверх
                float2 noiseUV = i.uv + float2(0, t * 0.2);
                float noise = tex2D(_NoiseTex, frac(noiseUV)).r;

                // === Лёгкие волны ===
                float wave = sin(i.uv.y * _WaveFrequency + t * 2.0) * _WaveStrength;

                // Итоговые UV
                float2 distortedUV = frac(i.uv + float2(noise * _Distortion + wave, -t * 0.1));

                // Базовый градиент
                fixed4 col = tex2D(_MainTex, distortedUV);

                // Вертикальный градиент для цвета
                float gradient = saturate(i.uv.y);
                fixed4 fireColor = lerp(_Color1, _Color2, gradient);

                // Цвет + альфа
                col.rgb *= fireColor.rgb;
                col.a *= fireColor.a;

                return col;
            }
            ENDCG
        }
    }

    Fallback "Sprites/Default"
}
