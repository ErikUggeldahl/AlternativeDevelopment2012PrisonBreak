float3 light;
float2 pos;
float2 dim;

Texture diffuse;
sampler textureSampler = sampler_state
{
    Texture = (diffuse);
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

float4 PixelShaderFunction(float2 texCoords : TEXCOORD0) : COLOR0
{
	//float4 t = tex2D(TextureSampler, texCoords);
	//float v = lerp(1, 0, texCoords.y);

	//float3 light = float3(1000, 1000, 2000);
	//float2 pos = float2(0, 0);
	//float2 dim = float2(100, 100);
	float2 l = pos + dim * texCoords;
	float d = sqrt(pow(light.x - l.x, 2) + pow(light.y - l.y, 2));
	d /= light.z;
	d = lerp(1, 0.5, d);
	d = clamp(d, 0.5, 1);
	float4 f = tex2D(textureSampler, texCoords);
	f *= float4(d, d, d, 1);

    return f;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
