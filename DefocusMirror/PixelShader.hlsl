Texture2D screenTexture : register(t0);
SamplerState sampler0 : register(s0);
cbuffer ScreenSizeBuffer : register(b0)
{
    float screenSize_x : packoffset(c0.x);
    float screenSize_y : packoffset(c0.y);
    int blurRadiusR : packoffset(c0.z);
    int blurRadiusG : packoffset(c0.w);
    int blurRadiusB : packoffset(c1.x);
    float blurSigmaR : packoffset(c1.y);
    float blurSigmaG : packoffset(c1.z);
    float blurSigmaB : packoffset(c1.w);
}

const float scale = 1.0;

float4 main(float4 position : SV_POSITION, float2 texCoord : TEXCOORD0) : SV_TARGET
{
    float2 texelSize = 1.0 / float2(screenSize_x, screenSize_y);
    float4 color = 0;
    int blurRadiusRNeg = -blurRadiusR;
    int blurRadiusGNeg = -blurRadiusG;
    int blurRadiusBNeg = -blurRadiusB;
    
    
    
    // R 通道    
    if (blurRadiusR == 0)
    {
        color.r = screenTexture.Sample(sampler0, texCoord).r;
    }
    else
    {
        float sumR = 0.0;
        for (int i = blurRadiusRNeg; i <= blurRadiusG; i++)
        {
            for (int j = blurRadiusRNeg; j <= blurRadiusG; j++)
            {
                float2 offset = float2(i, j) * texelSize * scale;
                float weight = exp(-(i * i + j * j) / (2.0 * blurSigmaR * blurSigmaR));
                color.r += screenTexture.Sample(sampler0, texCoord + offset).r * weight;
                sumR += weight;
            }
        }
        color.r /= sumR;
    }

    // G 通道
    if (blurRadiusG == 0)
    {
        color.g = screenTexture.Sample(sampler0, texCoord).g;
    }
    else
    {
        float sumG = 0.0;
        for (int ig = blurRadiusGNeg; ig <= blurRadiusG; ig++)
        {
            for (int j = blurRadiusGNeg; j <= blurRadiusG; j++)
            {
                float2 offset = float2(ig, j) * texelSize * scale;
                float weight = exp(-(ig * ig + j * j) / (2.0 * blurSigmaG * blurSigmaG));
                color.g += screenTexture.Sample(sampler0, texCoord + offset).g * weight;
                sumG += weight;
            }
        }
        color.g /= sumG;
    }

    // B 通道
    if (blurRadiusB == 0)
    {
        color.b = screenTexture.Sample(sampler0, texCoord).b;
    }
    else
    {
        float sumB = 0.0;
        for (int ib = blurRadiusBNeg; ib <= blurRadiusG; ib++)
        {
            for (int j = blurRadiusBNeg; j <= blurRadiusG; j++)
            {
                float2 offset = float2(ib, j) * texelSize * scale;
                float weight = exp(-(ib * ib + j * j) / (2.0 * blurSigmaB * blurSigmaB));
                color.b += screenTexture.Sample(sampler0, texCoord + offset).b * weight;
                sumB += weight;
            }
        }
        color.b /= sumB;
    }

    return color;
}
