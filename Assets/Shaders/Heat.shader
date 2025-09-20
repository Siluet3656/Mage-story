Shader "Custom/HeatDistortion"
{
    Properties
    {
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _Strength ("Distortion Strength", Range(0,0.2)) = 0.05
        _Speed ("Scroll Speed", Float) = 1.0
        _Tint ("Tint Color", Color) = (1, 0.5, 0, 0.2) // лёгкий оттенок огня
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        GrabPass { } // захватываем фон (фон рендера попадает в _GrabTexture)

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
            float _Strength;
            float _Speed;
            fixed4 _Tint;
            sampler2D _GrabTexture;
            float4 _GrabTexture_TexelSize;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 grabPos : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _NoiseTex);
                o.grabPos = ComputeGrabScreenPos(o.pos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // прокрутка шума
                float2 noiseUV = i.uv + float2(_Time.y * _Speed, 0);
                float2 noise = tex2D(_NoiseTex, noiseUV).rg - 0.5;

                // смещаем фоновые UV
                float2 distortedUV = i.grabPos.xy / i.grabPos.w + noise * _Strength;

                fixed4 col = tex2Dproj(_GrabTexture, float4(distortedUV * i.grabPos.w, i.grabPos.z, i.grabPos.w));

                // добавляем лёгкий оттенок огня
                col.rgb = lerp(col.rgb, col.rgb + _Tint.rgb, _Tint.a);

                return col;
            }
            ENDCG
        }
    }
}
