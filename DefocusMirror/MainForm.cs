using System.Diagnostics;
using System.Runtime.InteropServices;
using SharpDX.DXGI;
using Timer = System.Windows.Forms.Timer;

namespace DefocusMirror;

public partial class MainForm : Form
{
    private const int GWL_EXSTYLE = -20;
    private const int HOTKEY_ID = 1;
    private const int HWND_TOPMOST = -1;
    private const int KEY_BREAK = 1;
    private const int KEY_MAKE = 0;

    // 键盘相关
    private const ushort KEYBOARD_OVERRUN_MAKE_CODE = 0xFF;

    private const int LWA_ALPHA = 0x2;
    private const uint MOD_CONTROL = 0x0002;
    private const uint MOD_SHIFT = 0x0004;
    private const int MOUSE_ATTRIBUTES_CHANGED = 0x04;
    private const int MOUSE_MOVE_ABSOLUTE = 0x01;

    // 输入设备
    private const int MOUSE_MOVE_RELATIVE = 0x00;

    private const int MOUSE_VIRTUAL_DESKTOP = 0x02;

    // RawInput API相关常量
    private const int RID_INPUT = 0x10000003;

    private const int RIDEV_DEVNOTIFY = 0x00002000;
    private const int RIDEV_INPUTSINK = 0x00000100;
    private const int RIDEV_NOLEGACY = 0x00000030;
    private const int RIM_TYPEHID = 2;
    private const int RIM_TYPEKEYBOARD = 1;

    // 设备类型
    private const int RIM_TYPEMOUSE = 0;

    private const int SWP_NOMOVE = 0x0002;
    private const int SWP_NOSIZE = 0x0001;
    private const uint VK_ESCAPE = 0x1B;
    private const int WM_INPUT = 0x00FF;
    private const int WS_EX_LAYERED = 0x80000;
    private const int WS_EX_NOACTIVATE = 0x8000000;
    private const int WS_EX_TOPMOST = 0x8;
    private const int WS_EX_TRANSPARENT = 0x20;
    private Timer _captureTimer;
    private int _dstScreenIndex;
    private readonly int _frameRate;
    private readonly bool _fullScreenMode;
    private Rectangle _fullScreenBounds;
    private int _outputSrcIndex = 0;
    private ScreenCapture _screenCapture;
    private readonly float _blurScaleR;
    private readonly float _blurScaleG;
    private readonly float _blurScaleB;
    private readonly float _blurSigmaR;
    private readonly float _blurSigmaG;
    private readonly float _blurSigmaB;
    private int _srcAdapterIndex;


    public frmArrow Arrow = new();

    // 原始输入处理
    private IntPtr hDeviceNotify = IntPtr.Zero;

    private IntPtr hwnd = IntPtr.Zero;

    // 屏幕映射
    private Rectangle inputDefocusedScreenBounds;

    private Rectangle inputCapturedScreenBounds;
    private float scaleX = 1.0f;
    private float scaleY = 1.0f;

    private bool FormLoaded = false;
    private bool ArrowVisible = false;
    private ControlBox _refControlBox;

    public MainForm(ControlBox controlBox, float blurScaleR = 0, float blurScaleG = 0, float blurScaleB = 0, float blurSigmaR = 1, float blurSigmaG = 1, float blurSigmaB = 1, int? adapterSrcIndex = null, int? outputSrcIndex = null, int? adapterDstIndex = null, int frameRate = 30, bool fullScreenMode = true)
    {
        InitializeComponent();
        _refControlBox = controlBox;
        _blurScaleR = blurScaleR;
        _blurScaleG = blurScaleG;
        _blurScaleB = blurScaleB;
        _blurSigmaR = blurSigmaR;
        _blurSigmaG = blurSigmaG;
        _blurSigmaB = blurSigmaB;
        _srcAdapterIndex = adapterSrcIndex ?? 0;
        _dstScreenIndex = adapterDstIndex ?? 0;
        _frameRate = frameRate;
        _fullScreenMode = fullScreenMode;
        _outputSrcIndex = outputSrcIndex ?? 0;

        InitializeMirror();
    }

    private void InitializeMirror()
    {
        _captureTimer?.Stop();
        _screenCapture?.Dispose();
        StartPosition = FormStartPosition.Manual;
        if (_fullScreenMode)
        {
            if (Screen.AllScreens.Length > 1) Bounds = _fullScreenBounds = Screen.AllScreens[_dstScreenIndex].Bounds;
            // 设置全屏，TopMost，输入穿透
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }
        TopMost = true;

        //int extendedStyle = GetWindowLong(Handle, GWL_EXSTYLE);
        //SetWindowLong(Handle, GWL_EXSTYLE, extendedStyle | WS_EX_LAYERED | WS_EX_TRANSPARENT);

        //// 设置不透明度为255（完全不透明），实现鼠标穿透
        //SetLayeredWindowAttributes(Handle, 0, 255, LWA_ALPHA);

        // 初始化屏幕捕获
        _screenCapture = new ScreenCapture(_blurScaleR, _blurScaleG, _blurScaleB, _blurSigmaR, _blurSigmaG, _blurSigmaB, Handle, _srcAdapterIndex, _outputSrcIndex, _frameRate, _fullScreenMode ? _fullScreenBounds.Width : Bounds.Width, _fullScreenMode ? _fullScreenBounds.Height : Bounds.Height);

        // 创建并启动捕获定时器
        _captureTimer = new Timer
        {
            Interval = 1000 / _frameRate // 大约30 FPS
        };
        _captureTimer.Tick += (s, e) => CaptureAndRender();
        _captureTimer.Start();

        inputDefocusedScreenBounds = Screen.AllScreens[_dstScreenIndex].Bounds;
        inputCapturedScreenBounds = GetSourceScreenBounds();

        Arrow.StartPosition = FormStartPosition.Manual;
        Arrow.Bounds = new Rectangle(inputCapturedScreenBounds.X, inputCapturedScreenBounds.Y, 64, 64);
        // 注册RawInput设备
        RegisterRawInput();
    }

    [Flags]
    public enum MouseEventFlags
    {
        LEFTDOWN = 0x0002,
        LEFTUP = 0x0004,
        ABSOLUTE = 0x8000
    }


    protected override void WndProc(ref Message m)
    {
        switch (m.Msg)
        {
            case WM_INPUT:
                {
                    ProcessRawInput(m.LParam);
                    break;
                }
        }

        base.WndProc(ref m);
    }

    [LibraryImport("user32.dll")]
    private static partial int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

    [LibraryImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetLayeredWindowAttributes(IntPtr hwnd, int crKey, byte bAlpha, int dwFlags);

    // API声明
    [LibraryImport("user32.dll", SetLastError = true)]
    private static partial int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [LibraryImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

    private void CaptureAndRender()
    {
        _screenCapture.CaptureScreen();
        _screenCapture.Render();
    }

    private Rectangle GetSourceScreenBounds()
    {
        using var dxgiFactory = new Factory1();
        using var adapter = dxgiFactory.Adapters[_srcAdapterIndex];
        using var output = adapter.Outputs[_outputSrcIndex];
        var bounds = output.Description.DesktopBounds;
        return new Rectangle(bounds.Left, bounds.Top, bounds.Right - bounds.Left, bounds.Bottom - bounds.Top);
    }

    private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
    {
        _captureTimer.Stop();
        _screenCapture.Dispose();
        Arrow.Close();
    }

    private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyValue == 'Q' && e.Alt && e.Control) Close();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        FormLoaded = true;
        Arrow.Show();
        Arrow.TopMost = true;

    }

    private void ProcessKeyboardInput(RAWINPUT raw)
    {
        RAWINPUTKEYBOARD keyboard = raw.keyboard;

        // 获取当前鼠标位置
        Point cursorPos = Cursor.Position;

        // 只在鼠标位于源屏幕内时才重定向键盘输入
        if (!inputDefocusedScreenBounds.Contains(cursorPos))
        {
            return;
        }

        ushort vKey = keyboard.VKey;
        ushort makeCode = keyboard.MakeCode;
        ushort flags = keyboard.Flags;

        // 处理键盘事件
        bool isKeyDown = (flags & 0x01) == 0; // RI_KEY_MAKE
        bool isKeyUp = (flags & 0x01) == 1;   // RI_KEY_BREAK
        bool isE0 = (flags & 0x02) != 0;      // RI_KEY_E0
        bool isE1 = (flags & 0x04) != 0;      // RI_KEY_E1

        if (vKey == 0xFF) // 过载码
        {
            return;
        }

        if (isKeyDown)
        {
            SimulateKeyDown(vKey, isE0);
        }
        else if (isKeyUp)
        {
            SimulateKeyUp(vKey, isE0);
        }
    }

    private void ProcessMouseInput(RAWINPUT raw)
    {
        // 获取当前鼠标位置
        Point cursorPos = Cursor.Position;

        // 检查鼠标是否在源屏幕内
        if (!inputCapturedScreenBounds.Contains(cursorPos)) return;


        // 将源屏幕坐标映射到离焦屏幕
        Point targetPos = new(
            (int)((cursorPos.X - inputCapturedScreenBounds.Left) * scaleX) + inputDefocusedScreenBounds.Left,
            (int)((cursorPos.Y - inputCapturedScreenBounds.Top) * scaleY) + inputDefocusedScreenBounds.Top
                             );

        Arrow.Location = new Point(targetPos.X, targetPos.Y);
        if (!_refControlBox.Visible) Arrow.TopMost = true;
        if (ArrowVisible == false)
        {
            ArrowVisible = true;
            Arrow.BringToFront();
            Arrow.TopMost = true;
            
        }

        //RAWINPUTMOUSE mouse = raw.mouse;
        //// 处理鼠标移动
        //if ((mouse.usFlags & MOUSE_MOVE_RELATIVE) == MOUSE_MOVE_RELATIVE)
        //{
        //    // 相对移动
        //    //mouse.lLastX = (int)(mouse.lLastX * scaleX);
        //    //mouse.lLastY = (int)(mouse.lLastY * scaleY);
        //    //SimulateMouseMoveRelative(mouse.lLastX, mouse.lLastY);
        //}

        //// 处理鼠标按钮
        //ushort buttonFlags = mouse.usButtonFlags;

        //// 左键
        //if ((buttonFlags & 0x0001) != 0) // RI_MOUSE_LEFT_BUTTON_DOWN
        //{
        //    SimulateMouseEvent(MOUSEEVENTF.LEFTDOWN, targetPos);
        //}
        //if ((buttonFlags & 0x0002) != 0) // RI_MOUSE_LEFT_BUTTON_UP
        //{
        //    SimulateMouseEvent(MOUSEEVENTF.LEFTUP, targetPos);
        //}

        //// 右键
        //if ((buttonFlags & 0x0004) != 0) // RI_MOUSE_RIGHT_BUTTON_DOWN
        //{
        //    SimulateMouseEvent(MOUSEEVENTF.RIGHTDOWN, targetPos);
        //}
        //if ((buttonFlags & 0x0008) != 0) // RI_MOUSE_RIGHT_BUTTON_UP
        //{
        //    SimulateMouseEvent(MOUSEEVENTF.RIGHTUP, targetPos);
        //}

        //// 中键
        //if ((buttonFlags & 0x0010) != 0) // RI_MOUSE_MIDDLE_BUTTON_DOWN
        //{
        //    SimulateMouseEvent(MOUSEEVENTF.MIDDLEDOWN, targetPos);
        //}
        //if ((buttonFlags & 0x0020) != 0) // RI_MOUSE_MIDDLE_BUTTON_UP
        //{
        //    SimulateMouseEvent(MOUSEEVENTF.MIDDLEUP, targetPos);
        //}

        //// 鼠标滚轮
        //if ((buttonFlags & 0x0400) != 0) // RI_MOUSE_WHEEL
        //{
        //    short wheelDelta = (short)mouse.usButtonData;
        //    SimulateMouseWheel(wheelDelta);
        //}

        //// 水平滚轮
        //if ((buttonFlags & 0x0800) != 0) // RI_MOUSE_HWHEEL
        //{
        //    short hWheelDelta = (short)mouse.usButtonData;
        //    SimulateMouseHWheel(hWheelDelta);
        //}
    }

    private void ProcessRawInput(IntPtr rawInputPointer)
    {
        uint dwSize = 0;
        GetRawInputData(rawInputPointer, RID_INPUT, IntPtr.Zero, ref dwSize, (uint)Marshal.SizeOf<RAWINPUTHEADER>());

        if (dwSize == 0) return;

        IntPtr buffer = Marshal.AllocHGlobal((int)dwSize);
        try
        {
            if (GetRawInputData(rawInputPointer, RID_INPUT, buffer, ref dwSize, (uint)Marshal.SizeOf<RAWINPUTHEADER>()) != dwSize)
            {
                Debug.WriteLine("获取RawInput数据失败");
                return;
            }

            RAWINPUT raw = Marshal.PtrToStructure<RAWINPUT>(buffer);

            switch (raw.header.dwType)
            {
                case RIM_TYPEMOUSE:
                    ProcessMouseInput(raw);
                    break;

                    //case RIM_TYPEKEYBOARD:
                    //    //ProcessKeyboardInput(raw);
                    //    break;
            }
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

    // 注册RawInput
    private void RegisterRawInput()
    {
        RAWINPUTDEVICE[] rid = new RAWINPUTDEVICE[2];

        // 注册鼠标
        rid[0].usUsagePage = 0x01;
        rid[0].usUsage = 0x02; // 鼠标
        rid[0].dwFlags = RIDEV_INPUTSINK | RIDEV_DEVNOTIFY;
        rid[0].hwndTarget = this.Handle;

        // 注册键盘
        rid[1].usUsagePage = 0x01;
        rid[1].usUsage = 0x06; // 键盘
        rid[1].dwFlags = RIDEV_INPUTSINK | RIDEV_DEVNOTIFY;
        rid[1].hwndTarget = this.Handle;

        if (!RegisterRawInputDevices(rid, (uint)rid.Length, (uint)Marshal.SizeOf(rid[0])))
        {
            int error = Marshal.GetLastWin32Error();
            Debug.WriteLine($"注册RawInput失败: {error}");
        }
    }

    // 模拟键盘按下
    private void SimulateKeyDown(ushort vKey, bool isExtended)
    {
        INPUT input = new()
        {
            type = 1, // INPUT_KEYBOARD
            u = new INPUT_UNION
            {
                ki = new KEYBDINPUT
                {
                    wVk = vKey,
                    wScan = 0,
                    dwFlags = isExtended ? (uint)(KEYEVENTF.EXTENDEDKEY | KEYEVENTF.KEYDOWN) : (uint)KEYEVENTF.KEYDOWN,
                    time = 0,
                    dwExtraInfo = GetMessageExtraInfo()
                }
            }
        };

        INPUT[] inputs = { input };
        SendInput(1, inputs, Marshal.SizeOf<INPUT>());
    }

    // 模拟键盘释放
    private void SimulateKeyUp(ushort vKey, bool isExtended)
    {
        INPUT input = new()
        {
            type = 1, // INPUT_KEYBOARD
            u = new INPUT_UNION
            {
                ki = new KEYBDINPUT
                {
                    wVk = vKey,
                    wScan = 0,
                    dwFlags = isExtended ? (uint)(KEYEVENTF.EXTENDEDKEY | KEYEVENTF.KEYUP) : (uint)KEYEVENTF.KEYUP,
                    time = 0,
                    dwExtraInfo = GetMessageExtraInfo()
                }
            }
        };

        INPUT[] inputs = { input };
        SendInput(1, inputs, Marshal.SizeOf<INPUT>());
    }

    // 模拟鼠标事件
    private void SimulateMouseEvent(MOUSEEVENTF dwFlags, Point position)
    {
        // 计算绝对坐标
        int absoluteX = (int)(position.X * 65536.0 / SystemInformation.VirtualScreen.Width);
        int absoluteY = (int)(position.Y * 65536.0 / SystemInformation.VirtualScreen.Height);

        INPUT input = new()
        {
            type = 0, // INPUT_MOUSE
            u = new INPUT_UNION
            {
                mi = new MOUSEINPUT
                {
                    dx = absoluteX,
                    dy = absoluteY,
                    mouseData = 0,
                    dwFlags = (uint)(dwFlags | MOUSEEVENTF.ABSOLUTE | MOUSEEVENTF.VIRTUALDESK),
                    time = 0,
                    dwExtraInfo = GetMessageExtraInfo()
                }
            }
        };

        INPUT[] inputs = [input];

        ////MouseClick配对处理

        var posTemp = Cursor.Position;
        Cursor.Position = position;

        Thread.Sleep(120);

        var r = SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<INPUT>());
        if (r == inputs.Length) Task.Delay(10).ContinueWith(i => Cursor.Position = posTemp);

        //Debug.WriteLine($"{DateTime.Now:HHmmss.fff}INPUT SENT:" + dwFlags);
    }

    // 模拟水平滚轮
    private void SimulateMouseHWheel(short wheelDelta)
    {
        INPUT input = new()
        {
            type = 0, // INPUT_MOUSE
            u = new INPUT_UNION
            {
                mi = new MOUSEINPUT
                {
                    dx = 0,
                    dy = 0,
                    mouseData = (uint)wheelDelta,
                    dwFlags = (uint)MOUSEEVENTF.HWHEEL,
                    time = 0,
                    dwExtraInfo = GetMessageExtraInfo()
                }
            }
        };

        INPUT[] inputs = { input };
        SendInput(1, inputs, Marshal.SizeOf<INPUT>());
    }

    // 模拟鼠标相对移动
    private void SimulateMouseMoveRelative(int dx, int dy)
    {
        INPUT input = new()
        {
            type = 0, // INPUT_MOUSE
            u = new INPUT_UNION
            {
                mi = new MOUSEINPUT
                {
                    dx = dx,
                    dy = dy,
                    mouseData = 0,
                    dwFlags = (uint)MOUSEEVENTF.MOVE,
                    time = 0,
                    dwExtraInfo = GetMessageExtraInfo()
                }
            }
        };

        INPUT[] inputs = { input };
        SendInput(1, inputs, Marshal.SizeOf<INPUT>());
    }

    // 模拟鼠标滚轮
    private void SimulateMouseWheel(short wheelDelta)
    {
        INPUT input = new()
        {
            type = 0, // INPUT_MOUSE
            u = new INPUT_UNION
            {
                mi = new MOUSEINPUT
                {
                    dx = 0,
                    dy = 0,
                    mouseData = (uint)wheelDelta,
                    dwFlags = (uint)MOUSEEVENTF.WHEEL,
                    time = 0,
                    dwExtraInfo = GetMessageExtraInfo()
                }
            }
        };

        INPUT[] inputs = { input };
        SendInput(1, inputs, Marshal.SizeOf<INPUT>());
    }

    #region Win32 API Declarations

    private enum KEYEVENTF : uint
    {
        KEYDOWN = 0x0000,  // 按键按下
        EXTENDEDKEY = 0x0001,
        KEYUP = 0x0002,
        SCANCODE = 0x0008,
        UNICODE = 0x0004
    }

    [Flags]
    private enum MOUSEEVENTF : uint
    {
        ABSOLUTE = 0x8000,
        HWHEEL = 0x01000,
        MOVE = 0x0001,
        MOVE_NOCOALESCE = 0x2000,
        LEFTDOWN = 0x0002,
        LEFTUP = 0x0004,
        RIGHTDOWN = 0x0008,
        RIGHTUP = 0x0010,
        MIDDLEDOWN = 0x0020,
        MIDDLEUP = 0x0040,
        VIRTUALDESK = 0x4000,
        WHEEL = 0x0800,
        XDOWN = 0x0080,
        XUP = 0x0100
    }

    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCursorPos(out Point lpPoint);

    [DllImport("user32.dll")]
    private static extern IntPtr GetMessageExtraInfo();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetRawInputData(IntPtr hRawInput, uint uiCommand, IntPtr pData, ref uint pcbSize, uint cbSizeHeader);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool RegisterRawInputDevices(RAWINPUTDEVICE[] pRawInputDevices, uint uiNumDevices, uint cbSize);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    private void MainForm_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        this.Close();
    }

    private void MainForm_Resize(object sender, EventArgs e)
    {
        if (FormLoaded) InitializeMirror();
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct HARDWAREINPUT
    {
        public uint uMsg;
        public ushort wParamL;
        public ushort wParamH;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct INPUT
    {
        public uint type;
        public INPUT_UNION u;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct INPUT_UNION
    {
        [FieldOffset(0)]
        public MOUSEINPUT mi;

        [FieldOffset(0)]
        public KEYBDINPUT ki;

        [FieldOffset(0)]
        public HARDWAREINPUT hi;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct RAWINPUT
    {
        [FieldOffset(0)]
        public RAWINPUTHEADER header;  // 24字节

        [FieldOffset(24)]  // 64位下偏移24
        public RAWINPUTMOUSE mouse;

        [FieldOffset(24)]
        public RAWINPUTKEYBOARD keyboard;

        [FieldOffset(24)]
        public RAWINPUTHID hid;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct RAWINPUTDEVICE
    {
        public ushort usUsagePage;
        public ushort usUsage;
        public uint dwFlags;
        public IntPtr hwndTarget;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct RAWINPUTHEADER
    {
        public uint dwType;
        public uint dwSize;
        public IntPtr hDevice;
        public IntPtr wParam;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct RAWINPUTHID
    {
        public uint dwSizeHid;
        public uint dwCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct RAWINPUTKEYBOARD
    {
        public ushort MakeCode;
        public ushort Flags;
        public ushort Reserved;
        public ushort VKey;
        public uint Message;
        public uint ExtraInformation;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct RAWINPUTMOUSE
    {
        [FieldOffset(0)] public ushort usFlags;
        [FieldOffset(4)] public uint ulButtons;  // 注意：偏移4是因为有2字节填充

        // 或者使用联合体：
        [FieldOffset(4)] public ushort usButtonFlags;

        [FieldOffset(6)] public ushort usButtonData;

        [FieldOffset(8)] public uint ulRawButtons;
        [FieldOffset(12)] public int lLastX;
        [FieldOffset(16)] public int lLastY;
        [FieldOffset(20)] public uint dwExtraInfo;
    }

    #endregion Win32 API Declarations
}