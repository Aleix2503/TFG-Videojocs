Shader "Custom/StampPainter"
{
    Properties
    {
        _MainTex ("Base (unused)", 2D) = "white" {}
        _StampTex ("Stamp Texture", 2D) = "white" {}
        _StampPos ("Stamp UV Pos", Vector) = (0.5, 0.5, 0, 0)
        _StampRotation ("Stamp Rotation", Float) = 0
        _StampScale ("Stamp Scale", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Pass
        {
            ZWrite Off
            Cull Off
            Fog { Mode Off }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            sampler2D _StampTex;
            float4 _StampPos;      // xy = center UV
            float _StampRotation;  // degrees
            float _StampScale;

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float2 RotateUV(float2 uv, float angleRad)
            {
                float s = sin(angleRad);
                float c = cos(angleRad);
                return float2(c * uv.x - s * uv.y, s * uv.x + c * uv.y);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 delta = (i.uv - _StampPos.xy);
                delta /= _StampScale;

                float angleRad = radians(_StampRotation);
                delta = RotateUV(delta, -angleRad); // reverse rotate

                float2 stampUV = delta + 0.5; // center sample
                
                if (stampUV.x < 0 || stampUV.x > 1 || stampUV.y < 0 || stampUV.y > 1)
                    return tex2D(_MainTex, i.uv); // fuera del stamp

                fixed4 baseColor = tex2D(_MainTex, i.uv);
                fixed4 stampColor = tex2D(_StampTex, stampUV);

                // Usa alpha como máscara, blanco donde hay imagen
                return lerp(baseColor, fixed4(1, 1, 1, 1), stampColor.a);
            }
            ENDCG
        }
    }
}

