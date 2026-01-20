using System.Runtime.InteropServices;

namespace DefocusMirror;

[StructLayout(LayoutKind.Sequential)]
public struct BlurParams(float width, float height, int blurRadiusR, int blurRadiusG, int blurRadiusB, float blurSigmaR, float blurSigmaG, float blurSigmaB)
{
    public float Width = width;
    public float Height = height;
    public int BlurRadiusR = blurRadiusR;
    public int BlurRadiusG = blurRadiusG;
    public int BlurRadiusB = blurRadiusB;
    public float BlurSigmaR = blurSigmaR;
    public float BlurSigmaG = blurSigmaG;
    public float BlurSigmaB = blurSigmaB;
}