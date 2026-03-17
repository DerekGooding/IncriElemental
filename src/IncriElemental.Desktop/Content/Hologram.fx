#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
float Time;
float4 Color;

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
	float2 uv = input.TextureCoordinates;
    
    // Scanlines
    float scanline = sin(uv.y * 800.0 + Time * 10.0) * 0.1;
    
    // Glitch jitter
    float jitter = sin(Time * 50.0 + uv.y * 10.0) > 0.98 ? 0.02 * sin(Time * 100.0) : 0.0;
    uv.x += jitter;
    
    float4 texColor = tex2D(SpriteTextureSampler, uv);
    
    // Runic character cycling effect (hacky way: modulate alpha/color based on time)
    float charCycle = sin(Time * 20.0 + uv.x * 100.0) * 0.5 + 0.5;
    
    float4 result = texColor * input.Color;
    result.rgba += scanline;
    result.rgb = lerp(result.rgb, Color.rgb, charCycle * 0.3);
    
    // Fade out based on alpha in input color
    return result * input.Color.a;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
