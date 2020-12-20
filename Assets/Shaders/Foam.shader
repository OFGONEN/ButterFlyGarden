Shader "Unlit/Foam"
{
    Properties
    {
        _Color      ( "Color",        Color          ) = ( 1, 1, 1, 1 )
        _FoamRadius ( "Foam Radius",  Range( 0, 1 )  ) = 0.4
        _Intensity  ( "Intensity",    Range( 0, 1 )  ) = 0.26
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Geometry" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull back 
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct VertexInput
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct VertexToFragment
            {
                float2 uv     : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color;
            fixed  _FoamRadius;
            fixed  _Intensity;

            VertexToFragment vert( VertexInput vertexInput )
            {
                VertexToFragment output;
                output.vertex = UnityObjectToClipPos( vertexInput.vertex );
                output.uv     = vertexInput.uv;
                return output;
            }

            fixed4 frag( VertexToFragment fragmentOutput ) : SV_Target
            {
                fixed distanceToCenter = length( fragmentOutput.uv - fixed2( 0.5, 0.5 ) );
                fixed insideFoam = step( distanceToCenter, _FoamRadius );

                return fixed4( _Color.rgb, insideFoam *_Intensity );
            }
            ENDCG
        }
    }
}
