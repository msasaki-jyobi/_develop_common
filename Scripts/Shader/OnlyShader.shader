Shader "Custom/ColorFilterAndReplaceShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FilterColor ("Filter Color", Color) = (1, 0, 0, 1) // �t�B���^�[��F�i�f�t�H���g�͐ԁj
        _ReplaceColor ("Replacement Color", Color) = (1, 1, 1, 1) // �u��������F�i�f�t�H���g�͔��j
        _Tolerance ("Tolerance", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off // �\���`��𖳌���

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

                // �w�肳�ꂽ�t�B���^�[�F�ɋ߂����𔻒�
                float diff = distance(col.rgb, _FilterColor.rgb);

                if (diff <= _Tolerance)
                    return _ReplaceColor; // �t�B���^�[�F�������w�肵���F�ɒu��������
                else
                    return fixed4(0, 0, 0, 0); // ���̐F�͓����ɂ���
            }
            ENDCG
        }
    }
}