#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
Texture2D FluidGrid;
float Time;
float4 AetherColor;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

sampler2D FluidGridSampler = sampler_state
{
	Texture = <FluidGrid>;
    MagFilter = Linear;
    MinFilter = Linear;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

// Simple pseudo-random noise
float random(float2 st) {
    return frac(sin(dot(st.xy, float2(12.9898,78.233))) * 43758.5453123);
}

// 2D Noise
float noise(float2 st) {
    float2 i = floor(st);
    float2 f = frac(st);
    float a = random(i);
    float b = random(i + float2(1.0, 0.0));
    float c = random(i + float2(0.0, 1.0));
    float d = random(i + float2(1.0, 1.0));
    float2 u = f*f*(3.0-2.0*f);
    return lerp(a, b, u.x) + (c - a)* u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
}

float4 MainPS(VertexShaderOutput input) : COLOR0
{
    float2 uv = input.TextureCoordinates;
    
    // Read fluid data: R is displacement direction, G is intensity
    float4 fluid = tex2D(FluidGridSampler, uv);
    float displacement = (fluid.r * 2.0 - 1.0) * fluid.g * 0.05;
    
    // Distorted UVs based on fluid
    float2 distortedUV = uv + float2(displacement, displacement);
    
    // Low-frequency noise for "clouds"
    float n1 = noise(distortedUV * 3.0 + float2(Time * 0.1, Time * 0.05));
    float n2 = noise(distortedUV * 6.0 - float2(Time * 0.08, Time * 0.12));
    float clouds = (n1 * 0.6 + n2 * 0.4);
    
    // Base dark nebula color
    float4 baseColor = float4(0.02, 0.01, 0.04, 1.0);
    
    // Layer in the aether color based on noise
    float4 cloudColor = AetherColor * clouds * 0.4;
    
    // Add "ripples" highlights
    float ripples = fluid.g * 0.3;
    float4 finalColor = baseColor + cloudColor + (AetherColor * ripples);
    
    return finalColor * input.Color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
