using System.Runtime.InteropServices;

namespace DefocusMirror;

[StructLayout(LayoutKind.Sequential)]
public struct BlurParams(float width, float height, float blurScaleR, float blurScaleG, float blurScaleB, float blurSigmaR, float blurSigmaG, float blurSigmaB)
{
    public float Width = width;
    public float Height = height;
    public float BlurScaleR = blurScaleR;
    public float BlurScaleG = blurScaleG;
    public float BlurScaleB = blurScaleB;
    public float BlurSigmaR = blurSigmaR;
    public float BlurSigmaG = blurSigmaG;
    public float BlurSigmaB = blurSigmaB;
}