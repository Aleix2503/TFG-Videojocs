Shader "Custom/TilemapMaskShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _TilemapSize ("Tilemap Size", Vector) = (1,1,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Stencil{
                Ref 1
                Comp Always
                Pass Replace
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _MaskTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float4 _TilemapSize;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }           

            fixed4 frag (v2f i) : SV_Target
            {
                float2 maskUV;
                maskUV.x = (i.worldPos.x - _TilemapSize.z) / _TilemapSize.x;
                maskUV.y = (i.worldPos.y - _TilemapSize.w) / _TilemapSize.y;

                fixed mask = tex2D(_MaskTex, maskUV).r;
                fixed4 maskColor = tex2D(_MaskTex, maskUV);

                fixed4 col = _Color;

                if (mask > 0.1)
                    return maskColor;
                else
                    return col;
            }
            ENDCG
        }
    }
}
