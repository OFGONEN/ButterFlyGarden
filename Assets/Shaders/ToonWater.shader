Shader "Custom/ToonWater"
{
	Properties
    {
		// Shader keywords set from the inspector.
		[Toggle] _Color_Blend_Lerp( "Use Lerp To Combine Colors", Float ) = 0 // Will set _COLOR_BLEND_LERP_ON when toggled.

		// Regular properties.
		[Space( 6 )]
		[Header( PBR )]
		[Space( 2 )]
		_BaseColor           ( "Base Color", 			Color 			) = ( 0, 1, 0, 1 )
        _Metallic            ( "Metallic", 				Range( 0, 1 ) 	) = 0.5
        _Smoothness          ( "Smoothness", 			Range( 0, 1 ) 	) = 0
        _Emission            ( "Emission", 				Range( -1, 1 ) 	) = 0

		[Space( 6 )]
		[Header( Waves )]
		[Space( 2 )]
		_WaveSpeed			 ( "Wave Speed",			Range( 0, 0.2 ) ) = 0.01
		_Displacement		 ( "Displacement",			Range( 0, 0.3 ) ) = 0.01

		[Space( 6 )]
		[Header( Caustics )]
		[Space( 2 )]
		_CausticsColor       ( "Caustics Color", 		Color 			) = ( 0, 0, 1, 1 )
        _CellSize            ( "Cell Size", 			Range( 0, 50 ) 	) = 2
        _CausticsSlimness    ( "Caustics Slimness", 	Range( 1, 50 ) 	) = 5
        _CausticsSpeed       ( "Caustics Speed", 		Range( 1, 100 ) ) = 6

		[Space( 6 )]
		[Header( Radial Shearing )]
		[Space( 2 )]
        _RadialShearStrength ( "Radial Shear Strength", Range( 0, 0.1 ) ) = 0.05
        _RadialShearCenter   ( "Radial Shear Center", 	Vector 			) = ( 0.5, 0.5, 0, 0 )
        _RadialShearOffset   ( "Radial Shear Offset", 	Vector 			) = ( 0.5, 0.5, 0, 0 )
	}
	SubShader
    {
		Tags{ "RenderType" = "Transparent"   "Queue" = "Geometry" }
		ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull back 
        LOD 100

		CGPROGRAM

		#pragma surface Surf Standard fullforwardshadows vertex:Vert alpha:fade
		#pragma target 3.0

		#pragma shader_feature_local _COLOR_BLEND_LERP_ON

		#include "Random.cginc"

		float4 _BaseColor;
		float  _Metallic;
		float  _Smoothness;
		float  _Emission;

		float  	  _WaveSpeed;
		float  	  _Displacement;

		float4 _CausticsColor;
		float  _CellSize;
		float  _CausticsSlimness;
		float  _CausticsSpeed;

		float  _RadialShearStrength;
		float2 _RadialShearCenter;
		float2 _RadialShearOffset;

		struct Input
        {
			float3 worldPos;
		};

		float2 unity_gradientNoise_dir(float2 p)
		{
			p = p % 289;
			float x = (34 * p.x + 1) * p.x % 289 + p.y;
			x = (34 * x + 1) * x % 289;
			x = frac(x / 41) * 2 - 1;
			return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
		}

		float unity_gradientNoise(float2 p)
		{
			float2 ip = floor(p);
			float2 fp = frac(p);
			float d00 = dot(unity_gradientNoise_dir(ip), fp);
			float d01 = dot(unity_gradientNoise_dir(ip + float2(0, 1)), fp - float2(0, 1));
			float d10 = dot(unity_gradientNoise_dir(ip + float2(1, 0)), fp - float2(1, 0));
			float d11 = dot(unity_gradientNoise_dir(ip + float2(1, 1)), fp - float2(1, 1));
			fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
			return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x);
		}

		float Unity_GradientNoise_float( float2 UV, float Scale )
		{
			return unity_gradientNoise( UV * Scale ) + 0.5;
		}

		void Vert( inout appdata_full vertexIn )
		{
			float gradientNoise = Unity_GradientNoise_float( vertexIn.texcoord + ( _Time.y * _WaveSpeed ), 5 );
			vertexIn.vertex.xyz += mul( unity_WorldToObject, vertexIn.normal * gradientNoise * _Displacement );
      	}

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
			
            float noise       = VoronoiNoise( shearedValue, _Time * _CausticsSpeed ).x;
            float noiseRaised = pow( noise, _CausticsSlimness );

		#ifdef _COLOR_BLEND_LERP_ON
            output.Albedo     = lerp( _BaseColor.rgb, _CausticsColor.rgb, noiseRaised );
		#else
            output.Albedo     = _BaseColor.rgb + ( _CausticsColor.rgb * noiseRaised );
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
