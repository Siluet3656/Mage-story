Shader "Custom/FireBeam"
{
    Properties
    {
        _MainTex ("Main Texture (Gradient)", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _Color1 ("Core Color", Color) = (1,1,0.5,1)   // яркое ядро (желто-белое)
        _Color2 ("Outer Color", Color) = (1,0.3,0,0)  // красно-оранжевое затухание
        _Speed ("Scroll Speed", Float) = 2
        _Distortion ("Distortion Strength", Float) = 0.1
        _TimeValue ("Time Value", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
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

                // Прокрутка шума вдоль луча
                float2 noiseUV = i.uv + float2(t * 0.5, 0);
                float noise = tex2D(_NoiseTex, frac(noiseUV)).r;

                // Искажение UV
                float2 distortedUV = frac(i.uv + float2(0, noise * _Distortion));

                // Основной градиент по UV.y (толщина луча)
                fixed4 texCol = tex2D(_MainTex, distortedUV);

                // Цвет вдоль линии (X направление = движение)
                float gradient = saturate(i.uv.x);
                fixed4 beamColor = lerp(_Color1, _Color2, gradient);

                // Итоговый цвет
                texCol.rgb *= beamColor.rgb;
                texCol.a *= beamColor.a;

                return texCol;
            }
            ENDCG
        }
    }

    Fallback "Unlit/Transparent"
}
