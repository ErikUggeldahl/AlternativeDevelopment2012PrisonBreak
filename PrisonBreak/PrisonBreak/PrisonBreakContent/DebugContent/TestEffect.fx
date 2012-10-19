float4x4 World;
float4x4 View;
float4x4 Projection;

Texture diffuse;
sampler TextureSampler = sampler_state
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
	//t += 0.5;

    return float4(texCoords.x, texCoords.y, 1, 1);
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
