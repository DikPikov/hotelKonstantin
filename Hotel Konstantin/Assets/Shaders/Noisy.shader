Shader "Custom/Noisy"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
            _Strenght("Strenght", Range(0, 5000)) = 0
         [HideInInspector] _UnscaledTime("UnscaledTime", Float) = 1
         [ToggleOff]   _Animated("Animated", Range(0, 1)) = 0
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

        int _Animated;
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
            fixed time = _Time * _Animated;
        if (time == 0) {
            time = 1;
            }
            return tex2D(_MainTex, pixel.uv_MainTex) * pixel.color * _Color * Noise(pixel.uv_MainTex * time * _UnscaledTime);
        }

        ENDCG
}
       
    }
    FallBack "Diffuse"
}
