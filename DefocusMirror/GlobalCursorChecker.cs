using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DefocusMirror;

public class GlobalCursorHelper
{
    public static (Bitmap CursorImage, Point Hotspot, Point Position) GetGlobalCursor()
    {
        var cursorInfo = new NativeMethods.CURSORINFO { cbSize = Marshal.SizeOf<NativeMethods.CURSORINFO>() };

        if (!NativeMethods.GetCursorInfo(ref cursorInfo))
            return (null, Point.Empty, Point.Empty);

        if (cursorInfo.flags != 1)
            return (null, Point.Empty, Point.Empty);

        IntPtr hCursor = NativeMethods.CopyIcon(cursorInfo.hCursor);
        if (hCursor == IntPtr.Zero)
            return (null, Point.Empty, Point.Empty);

        try
        {
            if (!NativeMethods.GetIconInfo(hCursor, out var iconInfo))
                return (null, Point.Empty, Point.Empty);

            try
            {
                using var icon = Icon.FromHandle(hCursor);
                Bitmap cursorBmp = icon.ToBitmap();
                var hotspot = new Point(iconInfo.xHotspot, iconInfo.yHotspot);
                var position = new Point(cursorInfo.ptScreenPos.X, cursorInfo.ptScreenPos.Y);
                return (cursorBmp, hotspot, position);
            }
            finally
            {
                if (iconInfo.hbmMask != IntPtr.Zero) NativeMethods.DeleteObject(iconInfo.hbmMask);
                if (iconInfo.hbmColor != IntPtr.Zero) NativeMethods.DeleteObject(iconInfo.hbmColor);
            }
        }
        finally
        {
            NativeMethods.DestroyIcon(hCursor);
        }
    }
}
