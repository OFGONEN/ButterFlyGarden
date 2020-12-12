Shader "Unlit/Foam"
{
    Properties
    {
        _Color            ( "Color", Color )               = ( 1, 1, 1, 1 )
        _MainTex          ( "Texture", 2D )                = "white" {}
        _NoiseTex         ( "Noise Texture", 2D )          = "cyan" {}
        _Intensity        ( "Intensity", Float )           = 1.0
        _FoamRadius       ( "Foam Radius", Float )         = 0.1
        _RippleInnerRadius( "Ripple Inner Radius", Float ) = 0.1
        _RippleOuterRadius( "Ripple Outer Radius", Float ) = 1.0
        _WaveOffset       ( "Wave Offset", Float )         = 1.0
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
                float3 objCoords : TEXCOORD1;
                float4 vertex    : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4    _MainTex_ST;
            float4    _NoiseTex_ST;
            
            float4    _Color;
            float     _FoamRadius;
            float     _RippleInnerRadius;
            float     _RippleOuterRadius;
            float     _Intensity;
            float     _WaveOffset;

            VertexToFragment Vertex( Input vertexIn )
            {
                VertexToFragment vertexOut;
                vertexOut.vertex    = UnityObjectToClipPos( vertexIn.vertex );
                vertexOut.objCoords = vertexIn.vertex.xyz;
                vertexOut.uv        = TRANSFORM_TEX( vertexIn.uv, _MainTex );
                vertexOut.uv        = TRANSFORM_TEX( vertexIn.uv, _NoiseTex );
                return vertexOut;
            }

            fixed4 Fragment( VertexToFragment fragIn ) : SV_Target
            {
                fixed4 color          = tex2D(  _MainTex, fragIn.uv ) * _Color;
                float  rippleTexAlpha = tex2D( _NoiseTex, fragIn.uv );

                float distanceToCenter = length( fragIn.uv - float2( 0.5, 0.5 ) );

                color.a = 
                    /* Handle foam area: */
                      step( distanceToCenter, _FoamRadius )                      // Make sure pixel is inside the foam area.
                    * _Intensity
                    /* Handle rings: */
                    + step( _FoamRadius, distanceToCenter )                      // Make sure pixel is outside the foam area.
                    * step( _RippleInnerRadius, distanceToCenter - _WaveOffset ) // Make sure pixel is outside the inner ring radius.
                    * step( distanceToCenter - _WaveOffset, _RippleOuterRadius ) // Make sure pixel is inside  the outer ring radius. 
                    * _Intensity * rippleTexAlpha;                               // Don't forget to incorporate ripple texture in rings.
                
                return color;
            }
            ENDCG
        }
    }
}
