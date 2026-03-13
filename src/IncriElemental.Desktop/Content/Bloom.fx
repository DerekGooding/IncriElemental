#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
float BloomThreshold = 0.5;
float BloomIntensity = 1.0;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR0
{
	float4 baseColor = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;
    
    // Simple 5-tap box blur for bloom effect
    float2 texelSize = float2(1.0/1024.0, 1.0/768.0);
    float4 bloom = baseColor;
    bloom += tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(texelSize.x, 0));
    bloom += tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(-texelSize.x, 0));
    bloom += tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(0, texelSize.y));
    bloom += tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(0, -texelSize.y));
    bloom /= 5.0;

    // Apply threshold
    float brightness = dot(bloom.rgb, float3(0.2126, 0.7152, 0.0722));
    if (brightness < BloomThreshold)
        bloom = float4(0, 0, 0, 0);
    
    return baseColor + (bloom * BloomIntensity);
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
