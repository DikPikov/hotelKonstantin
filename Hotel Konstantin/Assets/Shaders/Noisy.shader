Shader "Custom/Noisy"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
            _Strenght("Strenght", Range(0, 5000)) = 0
         _UnscaledTime("UnscaledTime", Float) = 1
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 200

            Pass{
            Blend SrcAlpha OneMinusSrcAlpha

             CGPROGRAM

        #pragma vertex vert
        #pragma fragment frag
        #pragma target 3.0

        sampler2D _MainTex;

        struct Mesh
        {
            float4 vertex : POSITION;
            float4 color : COLOR;
            float2 uv_MainTex : TEXCOORD0;
        };

        struct Pixel
        {
            float4 vertex : POSITION;
            float4 color : COLOR;
            float2 uv_MainTex : TEXCOORD0;
        };

        fixed _Strenght;
        fixed4 _Color;
        fixed _UnscaledTime;

        fixed Noise(float2 uv) {
            return frac(sin(dot(uv, float2(12.9898, 78.233))) * _Strenght);
        }

        Pixel vert(Mesh mesh) {
            Pixel pixel;

            pixel.vertex = UnityObjectToClipPos(mesh.vertex);
            pixel.color = mesh.color;
            pixel.uv_MainTex = mesh.uv_MainTex;

            return pixel;
        }

        fixed4 frag(Pixel pixel) : SV_Target
        {
            return tex2D(_MainTex, pixel.uv_MainTex) * pixel.color * _Color * Noise(pixel.uv_MainTex * _UnscaledTime);
        }

        ENDCG
}
       
    }
    FallBack "Diffuse"
}
