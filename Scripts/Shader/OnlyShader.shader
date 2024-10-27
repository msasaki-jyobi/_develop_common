Shader "Custom/ColorFilterAndReplaceShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FilterColor ("Filter Color", Color) = (1, 0, 0, 1) // フィルター基準色（デフォルトは赤）
        _ReplaceColor ("Replacement Color", Color) = (1, 1, 1, 1) // 置き換える色（デフォルトは白）
        _Tolerance ("Tolerance", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off // 表裏描画を無効化

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _FilterColor;
            float4 _ReplaceColor;
            float _Tolerance;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // 指定されたフィルター色に近いかを判定
                float diff = distance(col.rgb, _FilterColor.rgb);

                if (diff <= _Tolerance)
                    return _ReplaceColor; // フィルター色部分を指定した色に置き換える
                else
                    return fixed4(0, 0, 0, 0); // 他の色は透明にする
            }
            ENDCG
        }
    }
}
