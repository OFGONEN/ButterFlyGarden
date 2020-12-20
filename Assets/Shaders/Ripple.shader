Shader "Unlit/Ripple"
{
    Properties
    {
        _RippleColor       ( "Ripple Color",        Color ) = ( 0, 1, 0, 1 )
        _NoiseTex          ( "Noise Texture",       2D    ) = "cyan" {}
        _Intensity         ( "Intensity",           Float ) = 1.0
        _RippleInnerRadius ( "Ripple Inner Radius", Float ) = 0.1
        _RippleOuterRadius ( "Ripple Outer Radius", Float ) = 1.0
        _WaveOffset        ( "Wave Offset",         Float ) = 1.0
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull back 
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment

            #include "UnityCG.cginc"

            struct Input
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct VertexToFragment
            {
                float2 uv        : TEXCOORD0;
                float4 vertex    : SV_POSITION;
            };

            sampler2D _NoiseTex;
            float4    _NoiseTex_ST;
            
            float4    _RippleColor;
            float     _RippleInnerRadius;
            float     _RippleOuterRadius;
            float     _Intensity;
            float     _WaveOffset;

            VertexToFragment Vertex( Input vertexIn )
            {
                VertexToFragment vertexOut;
                vertexOut.vertex = UnityObjectToClipPos( vertexIn.vertex );
                vertexOut.uv     = TRANSFORM_TEX( vertexIn.uv, _NoiseTex );
                return vertexOut;
            }

            fixed4 Fragment( VertexToFragment fragIn ) : SV_Target
            {
                float distanceToCenter = length( fragIn.uv - float2( 0.5, 0.5 ) );

                float pixelIsOutsideTheInnerRadius = step( _RippleInnerRadius, distanceToCenter - _WaveOffset );
                float pixelIsInsideTheOuterRadius  = step( distanceToCenter - _WaveOffset, _RippleOuterRadius );
                float pixelWillBeVisible           = pixelIsOutsideTheInnerRadius * pixelIsInsideTheOuterRadius;

                return fixed4( /* RGB:   */ _RippleColor.rgb,
                               /* Alpha: */ pixelWillBeVisible * _Intensity * tex2D( _NoiseTex, fragIn.uv ).r );
            }
            ENDCG
        }
    }
}
