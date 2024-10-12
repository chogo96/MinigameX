Shader "Custom/DoubleSideColor"
{
    Properties
    {
        _Color ("Main Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        LOD 100
        Tags { "RenderType"="Opaque" }

        // Cull Off를 추가하여 양면 렌더링 활성화
        Cull Off

        Pass
        {
            Tags { "RenderType"="Opaque" }

            // Vertex Shader
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc" // Unity 기본 함수를 사용하기 위한 헤더 포함

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL; // Normals 추가
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };

            float4 _Color;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return i.color;
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
