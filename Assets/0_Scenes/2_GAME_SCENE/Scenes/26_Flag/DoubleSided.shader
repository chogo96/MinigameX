Shader "Custom/DoubleSided"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        Cull Off // 양면 렌더링 활성화

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos; // 월드 좌표 추가
            float3 normal;   // 법선 추가
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;

            // 카메라 방향을 구하기
            float3 viewDir = normalize(_WorldSpaceCameraPos - IN.worldPos);

            // 법선 조정: 월드 법선과 카메라 방향에 따라 설정
            float3 normal = normalize(IN.normal);
            float facing = dot(normal, viewDir);
            if (facing < 0)
            {
                // 법선이 카메라를 향하지 않으면 반전
                normal = -normal;
            }

            o.Normal = normal; // 계산된 법선을 사용

            // Metallic and smoothness
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
