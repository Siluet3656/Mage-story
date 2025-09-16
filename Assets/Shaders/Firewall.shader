Shader "Custom/Fire2D_BuiltIn"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _Color1 ("Bottom Color", Color) = (1,1,0,1)
        _Color2 ("Mid Color", Color)    = (1,0.5,0,1)
        _Color3 ("Top Color", Color)    = (1,0,0,0)
        _Speed ("Flame Speed", Float) = 2
        _Distortion ("Distortion Strength", Float) = 0.1
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
            fixed4 _Color1, _Color2, _Color3;
            float _Speed, _Distortion;
            float _TimeValue;

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

                // Шум для движения
                float2 noiseUV = i.uv + float2(0, frac(t * 0.2));
                float n = tex2D(_NoiseTex, noiseUV).r;

                // Искажение UV
                float2 distortedUV = frac(i.uv + float2(n * _Distortion, -t * 0.2));

                // Основная текстура (градиент)
                fixed4 col = tex2D(_MainTex, distortedUV);

                // Цветовой градиент
                float gradient = saturate(i.uv.y * 2.0);
                fixed4 fireColor = lerp(_Color1, _Color2, gradient);
                fireColor = lerp(fireColor, _Color3, pow(gradient, 2.0));

                col.rgb *= fireColor.rgb;
                col.a *= fireColor.a;

                return col;
            }
            ENDCG
        }
    }

    Fallback "Sprites/Default"
}
