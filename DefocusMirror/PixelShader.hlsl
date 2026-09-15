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
static const int kernelSize = 7;

float4 main(float4 position : SV_POSITION, float2 texCoord : TEXCOORD0) : SV_TARGET
{
    float2 texelSize = 1.0 / float2(screenSize_x, screenSize_y);
    float4 color = 0;

    // R channel - 2D Gaussian blur
    if (blurScaleR == 0)
    {
        color.r = screenTexture.Sample(sampler0, texCoord).r;
    }
    else
    {
        float sigma2R = 2.0 * blurSigmaR * blurSigmaR;
        float sumR = 0.0;
        for (int i = radiusN; i <= radius; i++)
        {
            for (int j = radiusN; j <= radius; j++)
            {
                float2 offset = float2(i, j) * texelSize * blurScaleR;
                float weight = exp(-(i * i + j * j) / sigma2R);
                color.r += screenTexture.Sample(sampler0, texCoord + offset).r * weight;
                sumR += weight;
            }
        }
        color.r /= sumR;
    }

    // G channel - 2D Gaussian blur
    if (blurScaleG == 0)
    {
        color.g = screenTexture.Sample(sampler0, texCoord).g;
    }
    else
    {
        float sigma2G = 2.0 * blurSigmaG * blurSigmaG;
        float sumG = 0.0;
        for (int ig = radiusN; ig <= radius; ig++)
        {
            for (int jg = radiusN; jg <= radius; jg++)
            {
                float2 offset = float2(ig, jg) * texelSize * blurScaleG;
                float weight = exp(-(ig * ig + jg * jg) / sigma2G);
                color.g += screenTexture.Sample(sampler0, texCoord + offset).g * weight;
                sumG += weight;
            }
        }
        color.g /= sumG;
    }

    // B channel - 2D Gaussian blur
    if (blurScaleB == 0)
    {
        color.b = screenTexture.Sample(sampler0, texCoord).b;
    }
    else
    {
        float sigma2B = 2.0 * blurSigmaB * blurSigmaB;
        float sumB = 0.0;
        for (int ib = radiusN; ib <= radius; ib++)
        {
            for (int jb = radiusN; jb <= radius; jb++)
            {
                float2 offset = float2(ib, jb) * texelSize * blurScaleB;
                float weight = exp(-(ib * ib + jb * jb) / sigma2B);
                color.b += screenTexture.Sample(sampler0, texCoord + offset).b * weight;
                sumB += weight;
            }
        }
        color.b /= sumB;
    }

    return color;
}
