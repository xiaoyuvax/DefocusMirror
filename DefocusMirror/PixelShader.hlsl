Texture2D screenTexture : register(t0);
SamplerState sampler0 : register(s0);
cbuffer ScreenSizeBuffer : register(b0)
{
    float screenSize_x : packoffset(c0.x);
    float screenSize_y : packoffset(c0.y);
    float blurScaleR : packoffset(c0.z);
    float blurScaleG : packoffset(c0.w);
    float blurScaleB : packoffset(c1.x);
    float blurSigmaR : packoffset(c1.y);
    float blurSigmaG : packoffset(c1.z);
    float blurSigmaB : packoffset(c1.w);
}

static const int radius = 3;
static const int radiusN = -3;

//因为输入的screenTexture的实际传入格式是B8G8R8A8_UNORM，而Texture2D是按R8G8B8A8，所以相关参数需要对调。
float4 main(float4 position : SV_POSITION, float2 texCoord : TEXCOORD0) : SV_TARGET
{
    float2 texelSize = 1.0 / float2(screenSize_x, screenSize_y);
    float4 color = 0;
    
    
    // R 通道    
    if (blurScaleR == 0)
    {
        color.r = screenTexture.Sample(sampler0, texCoord).r;
    }
    else
    {
        float sumR = 0.0;
        for (int i = radiusN; i <= radius; i++)
        {
            for (int j = radiusN; j <= radius; j++)
            {
                float2 offset = float2(i, j) * texelSize * blurScaleR;
                float weight = exp(-(i * i + j * j) / (2.0 * blurSigmaR * blurSigmaR));
                color.r += screenTexture.Sample(sampler0, texCoord + offset).r * weight;
                sumR += weight;
            }
        }
        color.r /= sumR;
    }

    // G 通道
    if (blurScaleG == 0)
    {
        color.g = screenTexture.Sample(sampler0, texCoord).g;
    }
    else
    {
        float sumG = 0.0;
        for (int ig = radiusN; ig <= radius; ig++)
        {
            for (int jg = radiusN; jg <= radius; jg++)
            {
                float2 offset = float2(ig, jg) * texelSize * blurScaleG;
                float weight = exp(-(ig * ig + jg * jg) / (2.0 * blurSigmaG * blurSigmaG));
                color.g += screenTexture.Sample(sampler0, texCoord + offset).g * weight;
                sumG += weight;
            }
        }
        color.g /= sumG;
    }

    // B 通道
    if (blurScaleB == 0)
    {
        color.b = screenTexture.Sample(sampler0, texCoord).b;
    }
    else
    {
        float sumB = 0.0;
        for (int ib = radiusN; ib <= radius; ib++)
        {
            for (int jb = radiusN; jb <= radius; jb++)
            {
                float2 offset = float2(ib, jb) * texelSize * blurScaleB;
                float weight = exp(-(ib * ib + jb * jb) / (2.0 * blurSigmaB * blurSigmaB));
                color.b += screenTexture.Sample(sampler0, texCoord + offset).b * weight;
                sumB += weight;
            }
        }
        color.b /= sumB;
    }

    return color;
}
