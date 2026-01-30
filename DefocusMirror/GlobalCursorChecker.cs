using System.Runtime.InteropServices;

namespace DefocusMirror;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class GlobalCursorHelper
{
    // 定义 CURSORINFO 结构
    [StructLayout(LayoutKind.Sequential)]
    public struct CURSORINFO
    {
        public Int32 cbSize; // 结构体大小
        public Int32 flags; // 光标状态标志 (0-隐藏, 1-显示)
        public IntPtr hCursor; // 光标句柄
        public POINT ptScreenPos; // 光标屏幕坐标
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public Int32 X;
        public Int32 Y;
    }

    // 定义 ICONINFO 结构
    [StructLayout(LayoutKind.Sequential)]
    public struct ICONINFO
    {
        public bool fIcon; // True for icon, False for cursor
        public Int32 xHotspot; // 热点 X 坐标
        public Int32 yHotspot; // 热点 Y 坐标
        public IntPtr hbmMask; // 掩码位图句柄
        public IntPtr hbmColor; // 颜色位图句柄
    }

    // 声明 Windows API
    [DllImport("user32.dll")]
    public static extern bool GetCursorInfo(ref CURSORINFO pci);

    [DllImport("user32.dll")]
    public static extern IntPtr CopyIcon(IntPtr hIcon);

    [DllImport("user32.dll")]
    public static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

    [DllImport("user32.dll")]
    public static extern bool DestroyIcon(IntPtr hIcon);

    [DllImport("gdi32.dll")]
    public static extern bool DeleteObject(IntPtr hObject);


    public static (Bitmap CursorImage, Point Hotspot, Point Position) GetGlobalCursor()
    {
        CURSORINFO cursorInfo = new CURSORINFO();
        cursorInfo.cbSize = Marshal.SizeOf(cursorInfo); // 必须正确设置结构体大小

        if (GetCursorInfo(ref cursorInfo))
        {
            // 检查光标是否处于显示状态
            if (cursorInfo.flags == 1) // 1 代表 CURSOR_SHOWING
            {
                IntPtr hCursor = CopyIcon(cursorInfo.hCursor); // 复制光标
                if (hCursor != IntPtr.Zero)
                {
                    try
                    {
                        if (GetIconInfo(hCursor, out ICONINFO iconInfo))
                        {
                            try
                            {
                                // 使用 Icon.FromHandle 将句柄转换为 Icon，再转为 Bitmap
                                using Icon icon = Icon.FromHandle(hCursor);
                                Bitmap cursorBmp = icon.ToBitmap();

                                // 计算光标绘制的准确位置（当前位置减去热点偏移）
                                Point hotspot = new Point(iconInfo.xHotspot, iconInfo.yHotspot);
                                Point position = new Point(cursorInfo.ptScreenPos.X, cursorInfo.ptScreenPos.Y);

                                return (cursorBmp, hotspot, position);
                            }
                            finally
                            {
                                // 清理 GDI 对象
                                if (iconInfo.hbmMask != IntPtr.Zero) DeleteObject(iconInfo.hbmMask);
                                if (iconInfo.hbmColor != IntPtr.Zero) DeleteObject(iconInfo.hbmColor);
                            }
                        }
                    }
                    finally
                    {
                        DestroyIcon(hCursor); // 必须销毁复制的图标，避免内存泄漏
                    }
                }
            }
        }
        return (null, Point.Empty, Point.Empty);
    }

}