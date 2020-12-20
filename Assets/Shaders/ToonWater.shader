Shader "Custom/ToonWater"
{
	Properties
    {
		// Shader keywords set from the inspector.
		[Toggle] _Color_Blend_Lerp( "Use Lerp To Combine Colors", Float ) = 0 // Will set _COLOR_BLEND_LERP_ON when toggled.

		// Regular properties.
		[Space( 10 )]
		[Header( PBR )]
		[Space( 2 )]
		_BaseColor           ( "Base Color", 			Color 			) = ( 0, 1, 0, 1 )
        _Metallic            ( "Metallic", 				Range( 0, 1 ) 	) = 0.5
        _Smoothness          ( "Smoothness", 			Range( 0, 1 ) 	) = 0
        _Emission            ( "Emission", 				Range( -1, 1 ) 	) = 0
		[Space( 10 )]
		[Header( Ripple )]
		[Space( 2 )]
		_RippleColor         ( "Ripple Color", 			Color 			) = ( 0, 0, 1, 1 )
        _CellSize            ( "Cell Size", 			Range( 0, 50 ) 	) = 2
        _RippleSlimness      ( "Ripple Slimness", 		Range( 1, 50 ) 	) = 5
        _RippleSpeed         ( "Ripple Speed", 			Range( 1, 100 ) ) = 6
		[Space( 10 )]
		[Header( Radial Shearing )]
		[Space( 2 )]
        _RadialShearStrength ( "Radial Shear Strength", Range( 0, 0.1 ) ) = 0.05
        _RadialShearCenter   ( "Radial Shear Center", 	Vector 			) = ( 0.5, 0.5, 0, 0 )
        _RadialShearOffset   ( "Radial Shear Offset", 	Vector 			) = ( 0.5, 0.5, 0, 0 )
	}
	SubShader
    {
		Tags{ "RenderType" = "Opaque"   "Queue" = "Geometry" }

		CGPROGRAM

		#pragma surface Surf Standard fullforwardshadows
		#pragma target 3.0

		#pragma shader_feature_local _COLOR_BLEND_LERP_ON

		#include "Random.cginc"

		float4 _BaseColor;
		float4 _RippleColor;
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

		void Surf( Input input, inout SurfaceOutputStandard output )
        {
			float2 value = input.worldPos.xz / _CellSize;

			float2 shearedValue;
			Unity_RadialShear_float( value, _RadialShearCenter, _RadialShearStrength, _RadialShearOffset, shearedValue );
			
            float noise       = VoronoiNoise( shearedValue, _Time * _RippleSpeed ).x;
            float noiseRaised = pow( noise, _RippleSlimness );

		#ifdef _COLOR_BLEND_LERP_ON
            output.Albedo     = lerp(_BaseColor.rgb, _RippleColor.rgb, noiseRaised );
		#else
            output.Albedo     = _BaseColor.rgb + ( _RippleColor.rgb * noiseRaised );
		#endif
			output.Alpha  	 = _BaseColor.a;
			output.Metallic   = _Metallic;
			output.Smoothness = _Smoothness;
			output.Emission   = _Emission;
		}
		ENDCG
	}
	FallBack "Standard"
}
