Shader "OutlineBox/URP"
{
    Properties
    {
        _OutlineColor("Color", Color) = (1,0.85,0.2,1)
        _OutlineWidth("Width", Float) = 0.05
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite On
        Cull Front

        Pass
        {
            Name "Outline"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes { float4 posOS : POSITION; };
            struct Varyings  { float4 posCS : SV_POSITION; };

            float4 _OutlineColor;
            float  _OutlineWidth;

            Varyings vert(Attributes v)
            {
                Varyings o;
                // 沿原点方向均匀扩张，不沿法线 — 面永远闭合
                float3 dir = normalize(v.posOS.xyz);
                float3 p = v.posOS.xyz + dir * _OutlineWidth;
                o.posCS = TransformObjectToHClip(float4(p, 1));
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                return half4(_OutlineColor.rgb, _OutlineColor.a);
            }
            ENDHLSL
        }
    }
}
