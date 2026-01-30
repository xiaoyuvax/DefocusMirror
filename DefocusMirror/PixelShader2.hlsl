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

static const int radius = 2;
static const int radiusN = -2;
static const int kernelSize = 5; // 2*radius+1

float4 main(float4 position : SV_POSITION, float2 texCoord : TEXCOORD0) : SV_TARGET
{
    float2 texelSize = 1.0 / float2(screenSize_x, screenSize_y);
    float4 color = 0;
    
    // 在函数内部声明和初始化权重数组
    float weightR[kernelSize];
    float weightG[kernelSize];
    float weightB[kernelSize];
    
    // 初始化权重数组
    for (int i = 0; i < kernelSize; ++i)
    {
        weightR[i] = 0.0f;
        weightG[i] = 0.0f;
        weightB[i] = 0.0f;
    }

    // R通道：分离高斯（先水平后垂直）
    if (blurScaleR > 0.0f)
    {
        float sigma2R = blurSigmaR * blurSigmaR;
        for (int i = 0; i < kernelSize; ++i)
        {
            int offset = i + radiusN;
            weightR[i] = exp(-(offset * offset) / (2.0f * sigma2R));
        }
    }

    // G通道：分离高斯
    if (blurScaleG > 0.0f)
    {
        float sigma2G = blurSigmaG * blurSigmaG;
        for (int i = 0; i < kernelSize; ++i)
        {
            int offset = i + radiusN;
            weightG[i] = exp(-(offset * offset) / (2.0f * sigma2G));
        }
    }

    // B通道：分离高斯
    if (blurScaleB > 0.0f)
    {
        float sigma2B = blurSigmaB * blurSigmaB;
        for (int i = 0; i < kernelSize; ++i)
        {
            int offset = i + radiusN;
            weightB[i] = exp(-(offset * offset) / (2.0f * sigma2B));
        }
    }

    // R通道处理
    if (blurScaleR > 0.0f)
    {
        float sumR = 0.0f;
        float tempR = 0.0f;
        // 水平方向卷积
        for (int i = 0; i < kernelSize; ++i)
        {
            float2 offset = float2(i + radiusN, 0) * texelSize * blurScaleR;
            float weight = weightR[i];
            tempR += screenTexture.Sample(sampler0, texCoord + offset).r * weight;
            sumR += weight;
        }
        tempR /= sumR;
        sumR = 0.0f;
        for (int j = 0; j < kernelSize; ++j)
        {
            float2 offset = float2(0, j + radiusN) * texelSize * blurScaleR;
            float weight = weightR[j];
            float sampleR = screenTexture.Sample(sampler0, texCoord + offset).r;
            color.r += sampleR * weight;
            sumR += weight;
        }
        color.r /= sumR;
    }
    else
    {
        color.r = screenTexture.Sample(sampler0, texCoord).r;
    }

    // G通道处理
    if (blurScaleG > 0.0f)
    {
        float sumG = 0.0f;
        float tempG = 0.0f;
        for (int i = 0; i < kernelSize; ++i)
        {
            float2 offset = float2(i + radiusN, 0) * texelSize * blurScaleG;
            float weight = weightG[i];
            tempG += screenTexture.Sample(sampler0, texCoord + offset).g * weight;
            sumG += weight;
        }
        tempG /= sumG;
        sumG = 0.0f;
        for (int j = 0; j < kernelSize; ++j)
        {
            float2 offset = float2(0, j + radiusN) * texelSize * blurScaleG;
            float weight = weightG[j];
            float sampleG = screenTexture.Sample(sampler0, texCoord + offset).g;
            color.g += sampleG * weight;
            sumG += weight;
        }
        color.g /= sumG;
    }
    else
    {
        color.g = screenTexture.Sample(sampler0, texCoord).g;
    }

    // B通道处理
    if (blurScaleB > 0.0f)
    {
        float sumB = 0.0f;
        float tempB = 0.0f;
        for (int i = 0; i < kernelSize; ++i)
        {
            float2 offset = float2(i + radiusN, 0) * texelSize * blurScaleB;
            float weight = weightB[i];
            tempB += screenTexture.Sample(sampler0, texCoord + offset).b * weight;
            sumB += weight;
        }
        tempB /= sumB;
        sumB = 0.0f;
        for (int j = 0; j < kernelSize; ++j)
        {
            float2 offset = float2(0, j + radiusN) * texelSize * blurScaleB;
            float weight = weightB[j];
            float sampleB = screenTexture.Sample(sampler0, texCoord + offset).b;
            color.b += sampleB * weight;
            sumB += weight;
        }
        color.b /= sumB;
    }
    else
    {
        color.b = screenTexture.Sample(sampler0, texCoord).b;
    }

    return color;
}
