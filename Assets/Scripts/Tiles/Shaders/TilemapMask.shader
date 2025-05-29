Shader "Custom/TilemapMaskShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _ClickedTileUV ("Clicked Tile UV", Vector) = (0,0,0,0)
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }


            fixed4 _Color;

            float2 _ClickedTileUV;
            float2 _TilemapSize;

            fixed4 baseCol = (1,1,1,1);
           

            fixed4 frag (v2f i) : SV_Target
            {
                half4 isWhite = step(0.99, 1) * step(0.99, 1) * step(0.99, 1);
                fixed4 col = lerp(_Color, baseCol, isWhite);

                fixed mask = tex2D(_MaskTex, i.uv).r;

                float2 tileUV = floor(i.uv * _TilemapSize) / _TilemapSize;
                float matchX = step(0.01, 1.0 - abs(tileUV.x - _ClickedTileUV.x));
                float matchY = step(0.01, 1.0 - abs(tileUV.y - _ClickedTileUV.y));
                float isClicked = matchX * matchY;

                if (mask > 0.1)
                    return fixed4(1,1,1,1);
                else
                    return col;
            }
            ENDCG
        }
    }
}
