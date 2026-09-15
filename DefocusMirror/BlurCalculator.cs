namespace DefocusMirror;

/// <summary>
/// 基于论文 "Myopia: why the retina stops inhibiting eye growth" (Sci. Reports 12:21704, 2022) 的色差离焦理论。
/// 人眼 LCA（以 570nm 为最佳焦点，Marcos et al.）：R(630nm) = −0.23D，G(538nm) = +0.24D，B(450nm) = +1.10D。
/// 通道间的屈光度差由 LCA 决定，与观看距离无关；
/// 距离只影响换算到屏幕像素时的模糊大小：
///   屏幕上模糊圈直径(mm) = 瞳孔(mm) × ΔD(屈光度) × 观看距离(mm) / 1000
///   着色器 blurScale     = 模糊圈直径(mm) / (像素间距(mm) × displayScale × KernelWidth)
/// 其中 displayScale = 目标屏宽/源屏宽（着色器在源纹理像素空间中模糊）。
/// </summary>
public static class BlurCalculator
{
    public const float PupilDiameterMm = 6.5f;
    public const int KernelWidth = 7;
    public const float DefaultViewingDistanceMm = 700f;
    public const float DefaultPixelPitchMm = 0.2646f;

    public static readonly (float R, float G, float B) LcaDiopters = (-0.23f, 0.24f, 1.10f);

    // 近视模式（red in focus）：红通道清晰，G/B 追加模糊，相对红通道的等效离焦
    public static readonly (float R, float G, float B) MyopiaDefocusDiopters = (0f, 0.47f, 1.33f);

    // 远视模式（blue in focus）：蓝通道清晰，R/G 追加模糊，相对蓝通道的等效离焦
    public static readonly (float R, float G, float B) HyperopiaDefocusDiopters = (1.33f, 0.86f, 0f);

    public static (float R, float G, float B) GetMyopiaBlurScales(
        float pixelPitchMm, float displayScale = 1f, float distanceMm = DefaultViewingDistanceMm)
        => ToBlurScales(MyopiaDefocusDiopters, pixelPitchMm, displayScale, distanceMm);

    public static (float R, float G, float B) GetHyperopiaBlurScales(
        float pixelPitchMm, float displayScale = 1f, float distanceMm = DefaultViewingDistanceMm)
        => ToBlurScales(HyperopiaDefocusDiopters, pixelPitchMm, displayScale, distanceMm);

    private static (float, float, float) ToBlurScales(
        (float R, float G, float B) defocus, float pitchMm, float displayScale, float distanceMm)
    {
        if (pitchMm <= 0) pitchMm = DefaultPixelPitchMm;
        if (displayScale <= 0) displayScale = 1f;

        float ToScale(float diopters) => diopters <= 0f ? 0f
            : PupilDiameterMm * diopters * distanceMm / 1000f / (pitchMm * displayScale * KernelWidth);

        return (ToScale(defocus.R), ToScale(defocus.G), ToScale(defocus.B));
    }
}
