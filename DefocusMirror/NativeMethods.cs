using System.Runtime.InteropServices;

namespace DefocusMirror;

internal static partial class NativeMethods
{
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern uint GetRawInputData(
        IntPtr hRawInput, uint uiCommand, IntPtr pData,
        ref uint pcbSize, uint cbSizeHeader);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool RegisterRawInputDevices(
        RAWINPUTDEVICE[] pRawInputDevices, uint uiNumDevices, uint cbSize);

    [DllImport("user32.dll")]
    internal static extern bool GetCursorInfo(ref CURSORINFO pci);

    [DllImport("user32.dll")]
    internal static extern IntPtr CopyIcon(IntPtr hIcon);

    [DllImport("user32.dll")]
    internal static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

    [DllImport("user32.dll")]
    internal static extern bool DestroyIcon(IntPtr hIcon);

    [DllImport("gdi32.dll")]
    internal static extern bool DeleteObject(IntPtr hObject);

    [StructLayout(LayoutKind.Sequential)]
    internal struct CURSORINFO
    {
        public int cbSize;
        public int flags;
        public IntPtr hCursor;
        public POINT ptScreenPos;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ICONINFO
    {
        public bool fIcon;
        public int xHotspot;
        public int yHotspot;
        public IntPtr hbmMask;
        public IntPtr hbmColor;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RAWINPUTDEVICE
    {
        public ushort usUsagePage;
        public ushort usUsage;
        public uint dwFlags;
        public IntPtr hwndTarget;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RAWINPUTHEADER
    {
        public uint dwType;
        public uint dwSize;
        public IntPtr hDevice;
        public IntPtr wParam;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct RAWINPUT
    {
        [FieldOffset(0)]
        public RAWINPUTHEADER header;

        [FieldOffset(24)]
        public RAWINPUTMOUSE mouse;

        [FieldOffset(24)]
        public RAWINPUTKEYBOARD keyboard;

        [FieldOffset(24)]
        public RAWINPUTHID hid;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RAWINPUTHID
    {
        public uint dwSizeHid;
        public uint dwCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RAWINPUTKEYBOARD
    {
        public ushort MakeCode;
        public ushort Flags;
        public ushort Reserved;
        public ushort VKey;
        public uint Message;
        public uint ExtraInformation;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct RAWINPUTMOUSE
    {
        [FieldOffset(0)] public ushort usFlags;
        [FieldOffset(4)] public uint ulButtons;
        [FieldOffset(4)] public ushort usButtonFlags;
        [FieldOffset(6)] public ushort usButtonData;
        [FieldOffset(8)] public uint ulRawButtons;
        [FieldOffset(12)] public int lLastX;
        [FieldOffset(16)] public int lLastY;
        [FieldOffset(20)] public uint dwExtraInfo;
    }

    internal const int RID_INPUT = 0x10000003;
    internal const int RIDEV_INPUTSINK = 0x00000100;
    internal const int RIDEV_DEVNOTIFY = 0x00002000;
    internal const int RIM_TYPEMOUSE = 0;
    internal const int WM_HOTKEY = 0x0312;
    internal const int WM_INPUT = 0x00FF;
}
