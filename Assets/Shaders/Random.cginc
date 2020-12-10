#ifndef WHITE_NOISE
#define WHITE_NOISE

// To 1D functions.

// Get a scalar random value from a 3D value.
float Rand3DTo1D( float3 value, float3 dotDir = float3( 12.9898, 78.233, 37.719 ) )
{
	// Make value smaller To avoid artefacts.
	float3 smallValue = sin( value );
	// Get scalar value from 3D vector.
	float random = dot( smallValue, dotDir );
	// Make value more random by making it bigger and then taking the factional part.
	return frac( sin( random ) * 143758.5453 );
}

float Rand3DTo1D_Offset( float3 value, float offset, float3 dotDir = float3( 12.9898, 78.233, 37.719 ) )
{
    return cos( Rand3DTo1D( value ) * offset ) * 0.5 + 0.5;
}

float Rand2DTo1D( float2 value, float2 dotDir = float2( 12.9898, 78.233 ) )
{
	float2 smallValue = sin( value );
	float random = dot( smallValue, dotDir );
	random = frac( sin( random ) * 143758.5453 );
	return random;
}

float Rand2DTo1D_Offset( float2 value, float offset, float2 dotDir = float2( 12.9898, 78.233 ) )
{
    return cos( Rand2DTo1D( value ) * offset ) * 0.5 + 0.5;
}

float Rand1DTo1D( float3 value, float mutator = 0.546 )
{
	float random = frac( sin( value + mutator ) * 143758.5453 );
	return random;
}

float _Offset( float3 value, float offset, float mutator = 0.546 )
{
    return cos( Rand1DTo1D( value ) * offset ) * 0.5 + 0.5;
}

// To 2D functions.

float2 Rand3DTo2D( float3 value )
{
	return float2(
		Rand3DTo1D( value, float3( 12.989, 78.233, 37.719 ) ),
		Rand3DTo1D( value, float3( 39.346, 11.135, 83.155 ) )
	);
}

float2 Rand3DTo2D( float3 value, float offset )
{
	float2 random = Rand3DTo2D( value );
    return float2( sin( random.y * offset ) * 0.5 + 0.5, cos( random.x * offset ) * 0.5 + 0.5 );
}

float2 Rand2DTo2D( float2 value )
{
	return float2(
		Rand2DTo1D( value, float2( 12.989, 78.233 ) ),
		Rand2DTo1D( value, float2( 39.346, 11.135 ) )
	);
}

float2 Rand2DTo2D( float2 value, float offset )
{
	float2 random = Rand2DTo2D( value );
    return float2( sin( random.y * offset ) * 0.5 + 0.5, cos( random.x * offset ) * 0.5 + 0.5 );
}

float2 Rand1DTo2D( float value )
{
	return float2(
		Rand2DTo1D( value, 3.9812 ),
		Rand2DTo1D( value, 7.1536 )
	);
}

float2 Rand1DTo2D( float value, float offset )
{
	float2 random = Rand1DTo2D( value );
    return float2( sin( random.y * offset ) * 0.5 + 0.5, cos( random.x * offset ) * 0.5 + 0.5 );
}

// To 3D functions.

float3 Rand3DTo3D( float3 value )
{
	return float3(
		Rand3DTo1D( value, float3( 12.989, 78.233, 37.719 ) ),
		Rand3DTo1D( value, float3( 39.346, 11.135, 83.155 ) ),
		Rand3DTo1D( value, float3( 73.156, 52.235, 09.151 ) )
	);
}

float3 Rand2DTo3D( float2 value )
{
	return float3(
		Rand2DTo1D( value, float2( 12.989, 78.233 ) ),
		Rand2DTo1D( value, float2( 39.346, 11.135 ) ),
		Rand2DTo1D( value, float2( 73.156, 52.235 ) )
	);
}

float3 Rand1DTo3D( float value )
{
	return float3(
		Rand1DTo1D( value, 3.9812 ),
		Rand1DTo1D( value, 7.1536 ),
		Rand1DTo1D( value, 5.7241 )
	);
}

// Voronoi

float2 VoronoiNoise( float2 value, float offset )
{
    float2 baseCellBottomLeft = floor( value );
    float minDistToSeed = 10;
    float2 closestCell;

    [ unroll ]
    for( int y = -1; y <= 1; y++ )
    {
        [ unroll ]
        for( int x = -1; x <= 1; x++ )
        {
            float2 cellBottomLeft = baseCellBottomLeft + float2( x,y );
            float2 seedPosition   = cellBottomLeft + Rand2DTo2D( cellBottomLeft, offset );
            float2 toSeed         = seedPosition - value;
            float  distToSeed     = length( toSeed );
            if( distToSeed < minDistToSeed )
            {
                minDistToSeed = distToSeed;
                closestCell   = cellBottomLeft;
            }
        }
    }

    return float2( minDistToSeed, Rand2DTo1D( closestCell ) );
}

#endif