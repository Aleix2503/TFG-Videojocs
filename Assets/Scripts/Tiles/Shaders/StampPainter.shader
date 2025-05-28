Shader "Custom/StampPainter"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _StampTex ("Stamp Texture", 2D) = "white" {}
        _StampPos ("Stamp Position (UV)", Vector) = (0,0,0,0)
        _StampScale ("Stamp Scale", Float) = 1.0
        _StampRotation ("Stamp Rotation (degrees)", Float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _StampTex;
            float4 _StampPos;
            float _StampScale;
            float _StampRotation;

            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = i.uv;

                // Coordenadas relativas al centro del stamp
                float2 diff = uv - _StampPos.xy;

                // Rotar UV (rotación inversa para samplear el sprite correctamente)
                float angle = radians(-_StampRotation);
                float cosA = cos(angle);
                float sinA = sin(angle);
                float2 rotatedUV = float2(
                    diff.x * cosA - diff.y * sinA,
                    diff.x * sinA + diff.y * cosA
                );

                // Escalar coords para muestrear el stamp
                float2 stampUV = rotatedUV / _StampScale + 0.5;

                // Muestreamos el stamp solo si está dentro de rango [0,1]
                fixed4 stampCol = fixed4(0,0,0,0);
                if (stampUV.x >= 0 && stampUV.x <= 1 && stampUV.y >= 0 && stampUV.y <= 1)
                {
                    stampCol = tex2D(_StampTex, stampUV);
                }

                // Muestreamos la textura actual
                fixed4 baseCol = tex2D(_MainTex, uv);

                // Combinamos: usamos canal alpha del stamp para "pintar" blanco
                float alpha = stampCol.a;
                fixed4 result = lerp(baseCol, fixed4(1,1,1, baseCol.a), alpha);

                return result;
            }
            ENDCG
        }
    }
}
