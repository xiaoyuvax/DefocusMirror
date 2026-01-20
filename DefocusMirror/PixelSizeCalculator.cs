using SharpDX.DXGI;

namespace DefocusMirror;

public static class PixelSizeCalculator
{

    /// <summary>
    /// 一个简单的估算法是使用系统DPI设置。系统DPI（通常为96 DPI）定义了系统认为的“每逻辑英寸的像素数”。你可以通过以下代码获取DPI，然后进行估算
    /// </summary>
    /// <returns></returns>
    public static (double pixelWidth, double pixelHeight) GetPixelSizeSimple()
    {
        using Graphics graphics = Graphics.FromHwnd(IntPtr.Zero);
        float dpiX = graphics.DpiX;
        float dpiY = graphics.DpiY;
        // 估算：1英寸 = 25.4毫米
        double estimatedPixelWidthInMm = 25.4 / dpiX;
        double estimatedPixelHeightInMm = 25.4 / dpiY;

        return ((double)estimatedPixelWidthInMm, (double)estimatedPixelHeightInMm);
    }

    public static (double pixelWidth, double pixelHeight) GetPixelPhysicalSize(int adapterIndex = 0, double diagonalSizeInInches = 20.0)
    {
        // 1. 获取适配器和输出
        using (var factory = new Factory1())
        {
            using var adapter = factory.GetAdapter1(adapterIndex);
            using var output = adapter.GetOutput(0);
            var outputDescription = output.Description; // 这里包含了显示设备的描述信息[2](@ref)

            // 2. 获取屏幕分辨率（像素）
            int screenWidthInPixels = outputDescription.DesktopBounds.Right - outputDescription.DesktopBounds.Left;
            int screenHeightInPixels = outputDescription.DesktopBounds.Bottom - outputDescription.DesktopBounds.Top;
            // 注意：这里获取的是逻辑像素数，在高DPI（缩放）设置下可能与物理像素数不同[8](@ref)。

            // 方法2（实用）：通过屏幕对角线尺寸和宽高比计算
            // 假设已知屏幕对角线尺寸为24英寸

            // 计算屏幕宽度和高度（英寸）
            double diagonalInPixels = Math.Sqrt(Math.Pow(screenWidthInPixels, 2) + Math.Pow(screenHeightInPixels, 2));
            double pixelsPerInch = diagonalInPixels / (double)diagonalSizeInInches; // 估算的PPI

            double screenWidthInInches = screenWidthInPixels / pixelsPerInch;
            double screenHeightInInches = screenHeightInPixels / pixelsPerInch;

            // 4. 单位换算：英寸转毫米
            double screenWidthInMm = screenWidthInInches * 25.4;
            double screenHeightInMm = screenHeightInInches * 25.4;

            // 5. 计算单个像素的物理尺寸
            double pixelWidthInMm = screenWidthInMm / screenWidthInPixels;
            double pixelHeightInMm = screenHeightInMm / screenHeightInPixels;


            return (pixelWidthInMm, pixelHeightInMm);
        }
    }
}
