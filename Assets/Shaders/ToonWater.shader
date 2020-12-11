Shader "Custom/ToonWater"
{
	Properties
    {
        _Color( "Color", Color ) 						       		   	 = ( 1, 1, 1, 1 )
        _Metallic( "Metallic", Range( 0, 1 ) )							 = 0.5
        _Smoothness( "Smoothness", Range( 0, 1 ) ) 						 = 0
        _Emission( "Emission", Range( -1, 1 ) ) 						 = 0
		_CellSize( "Cell Size", Range( 0, 50 ) ) 		       		   	 = 2
		_RippleSpeed( "Ripple Speed", Range( 1, 100 ) ) 	       		 = 6
		_RippleSlimness( "Ripple Slimness", Range( 1, 10 ) )   		   	 = 5
		_RadialShearCenter( "Radial Shear Center", Vector )    		   	 = ( 0.5, 0.5, 0, 0 )
		_RadialShearStrength( "Radial Shear Strength", Range( 0, 0.1 ) ) = 0.05
		_RadialShearOffset( "Radial Shear Offset", Vector )    		   	 = ( 0.5, 0.5, 0, 0 )
	}
	SubShader
    {
		Tags{ "RenderType" = "Opaque"   "Queue" = "Geometry" }

		CGPROGRAM

		#pragma surface Surf Standard fullforwardshadows
		#pragma target 3.0

		#include "Random.cginc"

		float4 _Color;
		float  _Metallic;
		float  _Smoothness;
		float  _Emission;
		float  _CellSize;
		float  _RippleSpeed;
		float  _RippleSlimness;
		float2 _RadialShearCenter;
		float  _RadialShearStrength;
		float2 _RadialShearOffset;

		struct Input
        {
			float3 worldPos;
		};

		void Unity_RadialShear_float( float2 UV, float2 Center, float Strength, float2 Offset, out float2 Out )
		{
			float2 delta 		= UV - Center;
			float  delta2 		= dot( delta.xy, delta.xy );
			float2 delta_offset = delta2 * Strength;
			Out 				= UV + float2( delta.y, -delta.x ) * delta_offset + Offset;
		}

		void Surf( Input i, inout SurfaceOutputStandard o )
        {
			float2 value = i.worldPos.xz / _CellSize;

			float2 shearedValue;
			Unity_RadialShear_float( value, _RadialShearCenter, _RadialShearStrength, _RadialShearOffset, shearedValue );
			
            float noise = VoronoiNoise( shearedValue, _Time * _RippleSpeed ).x;
			float noiseRaised = pow( noise, _RippleSlimness );

            o.Albedo     = _Color.rgb + ( _Color.rgb * noiseRaised );
			o.Alpha  	 = _Color.a;
			o.Metallic   = _Metallic;
			o.Smoothness = _Smoothness;
			o.Emission   = _Emission;
		}
		ENDCG
	}
	FallBack "Standard"
}
