Shader "Custom/CoreBeam"
{
    Properties
    {
        _MainTex ("Main Texture (Gradient)", 2D) = "white" {}
        _Color ("Beam Color", Color) = (1,1,0.7,1)
        _Speed ("Scroll Speed", Float) = 5
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
            float4 _MainTex_ST;
            fixed4 _Color;
            float _Speed;
            float _TimeValue;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
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
                float2 uv = i.uv;
                uv.x += _TimeValue * _Speed; // прокрутка

                fixed4 texCol = tex2D(_MainTex, frac(uv));
                return texCol * _Color;
            }
            ENDCG
        }
    }
}
