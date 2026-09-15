using System.Diagnostics;
using System.Runtime.InteropServices;
using SharpDX.DXGI;
using Timer = System.Windows.Forms.Timer;

namespace DefocusMirror;

public partial class Mirror : Form
{
    public frmArrow Arrow = new();

    private const int GWL_EXSTYLE = -20;
    private const int HOTKEY_QUIT = 1;
    private const int HOTKEY_UP = 3;
    private const int HOTKEY_DOWN = 4;
    private const int HOTKEY_RIGHT = 5;
    private const int HOTKEY_LEFT = 6;
    private const int MOVE_STEP = 10;
    private const int WS_EX_LAYERED = 0x80000;
    private const int WS_EX_NOACTIVATE = 0x8000000;
    private const int WS_EX_TOPMOST = 0x8;
    private const int WS_EX_TRANSPARENT = 0x20;

    private readonly float _blurScaleB;
    private readonly float _blurScaleG;
    private readonly float _blurScaleR;
    private readonly float _blurSigmaB;
    private readonly float _blurSigmaG;
    private readonly float _blurSigmaR;
    private readonly int _frameRate;
    private readonly bool _fullScreenMode;

    private readonly SolidBrush defaultBrush = new(Color.WhiteSmoke);
    private readonly Font defaultFont = new("Tahoma", 8, FontStyle.Regular);

    private Timer _captureTimer;
    private Rectangle _fullScreenBounds;
    private ControlBox _refControlBox;
    private ScreenCapture _screenCapture;
    private int _srcAdapterIndex;
    private int _srcOutputIndex = 0;
    private Rectangle _srcScreenBounds;
    private Rectangle _tgtScreenBounds;
    private int _tgtScreenIndex;
    private bool FormLoaded = false;

    private float scaleX = 1.0f;
    private float scaleY = 1.0f;
    private Point _preferedLocation;
    private int _titleBarHeight;

    public static Rectangle LastBounds;

    public Mirror(
        ControlBox controlBox,
        float blurScaleR = 0, float blurScaleG = 0, float blurScaleB = 0,
        float blurSigmaR = 1, float blurSigmaG = 1, float blurSigmaB = 1,
        int? adapterSrcIndex = null, int? outputSrcIndex = null,
        int? adapterDstIndex = null,
        int frameRate = 30, bool fullScreenMode = true,
        Rectangle initialBounds = default)
    {
        InitializeComponent();
        KeyPreview = true;
        StartPosition = FormStartPosition.Manual;
        Bounds = initialBounds;
        _preferedLocation = Bounds.Location;

        _refControlBox = controlBox;
        _blurScaleR = blurScaleR;
        _blurScaleG = blurScaleG;
        _blurScaleB = blurScaleB;
        _blurSigmaR = blurSigmaR;
        _blurSigmaG = blurSigmaG;
        _blurSigmaB = blurSigmaB;
        _frameRate = frameRate;
        _fullScreenMode = fullScreenMode;

        _srcOutputIndex = outputSrcIndex ?? 0;
        _srcAdapterIndex = adapterSrcIndex ?? 0;
        _tgtScreenIndex = adapterDstIndex ?? 0;

        _tgtScreenBounds = GetTargetScreenBounds(_tgtScreenIndex);
        _srcScreenBounds = GetSourceScreenBounds(_srcAdapterIndex, _srcOutputIndex);

        bool success = HotKey.RegisterHotKey(Handle, HOTKEY_QUIT, (uint)(HotKey.KeyModifiers.Ctrl | HotKey.KeyModifiers.Alt), Keys.Escape);
        success &= HotKey.RegisterHotKey(Handle, HOTKEY_LEFT, (uint)HotKey.KeyModifiers.Ctrl, Keys.Left);
        success &= HotKey.RegisterHotKey(Handle, HOTKEY_RIGHT, (uint)HotKey.KeyModifiers.Ctrl, Keys.Right);
        success &= HotKey.RegisterHotKey(Handle, HOTKEY_UP, (uint)HotKey.KeyModifiers.Ctrl, Keys.Up);
        success &= HotKey.RegisterHotKey(Handle, HOTKEY_DOWN, (uint)HotKey.KeyModifiers.Ctrl, Keys.Down);

        if (!success) MessageBox.Show("离焦屏热键注册失败(Ctl+Alt+Esc或Ctl+方向键)，可能已被其他程序占用。");

        InitializeMirror();
    }

    public static Rectangle GetSourceScreenBounds(int srcAdapterIndex, int srcOutputIndex)
    {
        using var dxgiFactory = new Factory1();
        using var adapter = dxgiFactory.Adapters[srcAdapterIndex];
        using var output = adapter.Outputs[srcOutputIndex];
        var bounds = output.Description.DesktopBounds;
        return new Rectangle(bounds.Left, bounds.Top, bounds.Right - bounds.Left, bounds.Bottom - bounds.Top);
    }

    /// <summary>
    /// 根据屏幕设备名（如 \\.\DISPLAY1）查找其所属的 DXGI 适配器/输出索引。
    /// 按坐标匹配：Screen 的 Bounds 与 DXGI Output 的 DesktopBounds 应一致。
    /// </summary>
    public static (int adapterIndex, int outputIndex) FindAdapterOutputForScreen(string deviceName, int fallbackAdapter = 0, int fallbackOutput = 0)
    {
        var screen = Screen.AllScreens.FirstOrDefault(s => s.DeviceName == deviceName);
        if (screen == null) return (fallbackAdapter, fallbackOutput);

        using var dxgiFactory = new Factory1();
        for (int a = 0; a < dxgiFactory.Adapters.Length; a++)
        {
            using var adapter = dxgiFactory.Adapters[a];
            for (int o = 0; o < adapter.Outputs.Length; o++)
            {
                using var output = adapter.Outputs[o];
                var b = output.Description.DesktopBounds;
                var outputRect = new Rectangle(b.Left, b.Top, b.Right - b.Left, b.Bottom - b.Top);
                if (outputRect == screen.Bounds) return (a, o);
            }
        }
        return (fallbackAdapter, fallbackOutput);
    }

    public static Rectangle GetTargetScreenBounds(int tgtScreenIndex) => Screen.AllScreens[tgtScreenIndex].Bounds;

    protected override void WndProc(ref Message m)
    {
        switch (m.Msg)
        {
            case NativeMethods.WM_INPUT:
                ProcessRawInput(m.LParam);
                break;

            case NativeMethods.WM_HOTKEY:
                int id = m.WParam.ToInt32();
                switch (id)
                {
                    case HOTKEY_QUIT:
                        Close();
                        return;
                    case HOTKEY_LEFT:
                        Location = new Point(Location.X - MOVE_STEP, Location.Y);
                        return;
                    case HOTKEY_RIGHT:
                        Location = new Point(Location.X + MOVE_STEP, Location.Y);
                        return;
                    case HOTKEY_DOWN:
                        Location = new Point(Location.X, Location.Y + MOVE_STEP);
                        return;
                    case HOTKEY_UP:
                        Location = new Point(Location.X, Location.Y - MOVE_STEP);
                        return;
                }
                break;
        }

        base.WndProc(ref m);
    }

    private void CaptureAndRender()
    {
        if (Visible)
        {
            _screenCapture.CaptureScreen();
            _screenCapture.Render();
        }
    }

    private void InitializeMirror()
    {
        _captureTimer?.Stop();
        _screenCapture?.Dispose();
        TopMost = true;
        Arrow.Bounds = new Rectangle(_srcScreenBounds.X, _srcScreenBounds.Y, 64, 64);

        if (_fullScreenMode)
        {
            if (Screen.AllScreens.Length == 1) _fullScreenBounds = _srcScreenBounds;
            else Bounds = _fullScreenBounds = _tgtScreenBounds;

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            scaleX = (float)_tgtScreenBounds.Width / _srcScreenBounds.Width;
            scaleY = (float)_tgtScreenBounds.Height / _srcScreenBounds.Height;
        }
        else
        {
            if (_preferedLocation != default) Location = _preferedLocation;
            else if (!_tgtScreenBounds.Contains(Location)) Location = _tgtScreenBounds.Location;
            scaleX = (float)ClientSize.Width / _srcScreenBounds.Width;
            scaleY = (float)ClientSize.Height / _srcScreenBounds.Height;
            _titleBarHeight = Height - ClientSize.Height;
        }

        _screenCapture = new ScreenCapture(
            _blurScaleR, _blurScaleG, _blurScaleB,
            _blurSigmaR, _blurSigmaG, _blurSigmaB,
            Handle, _srcAdapterIndex, _srcOutputIndex, _frameRate,
            _fullScreenMode ? _fullScreenBounds.Width : Bounds.Width,
            _fullScreenMode ? _fullScreenBounds.Height : Bounds.Height);

        _captureTimer = new Timer { Interval = 1000 / _frameRate };
        _captureTimer.Tick += (s, e) => CaptureAndRender();
        _captureTimer.Start();

        RegisterRawInput();
    }

    private void Mirror_FormClosed(object sender, FormClosedEventArgs e)
    {
        HotKey.UnregisterHotKey(Handle, HOTKEY_QUIT);
        HotKey.UnregisterHotKey(Handle, HOTKEY_UP);
        HotKey.UnregisterHotKey(Handle, HOTKEY_DOWN);
        HotKey.UnregisterHotKey(Handle, HOTKEY_LEFT);
        HotKey.UnregisterHotKey(Handle, HOTKEY_RIGHT);
        _captureTimer.Stop();
        _screenCapture.Dispose();
        Arrow.Close();
    }

    private void Mirror_Load(object sender, EventArgs e) => FormLoaded = true;

    private void ProcessMouseInput(NativeMethods.RAWINPUT raw)
    {
        Point cursorPos = Cursor.Position;

        if (!_srcScreenBounds.Contains(cursorPos)) return;

        Point targetPos = _fullScreenMode
            ? new((int)((cursorPos.X - _srcScreenBounds.Left) * scaleX) + _tgtScreenBounds.Left,
                  (int)((cursorPos.Y - _srcScreenBounds.Top) * scaleY) + _tgtScreenBounds.Top)
            : new((int)((cursorPos.X - _srcScreenBounds.Left) * scaleX) + Left + DefaultMargin.Size.Width,
                  (int)((cursorPos.Y - _srcScreenBounds.Top) * scaleY) + Top + _titleBarHeight - DefaultMargin.Size.Height);

        try
        {
            var (cursorImage, hotspot, position) = GlobalCursorHelper.GetGlobalCursor();
            if (cursorImage == null) return;

            Arrow.Size = Arrow.CursorImage.Size = cursorImage.Size;
            Arrow.Location = new Point(targetPos.X - hotspot.X, targetPos.Y - hotspot.Y);

            using var g = Graphics.FromImage(cursorImage);
            g.DrawString("m", defaultFont, defaultBrush, new PointF(Arrow.Width / 2F + 4, Arrow.Height / 2F + 4));

            Arrow.CursorImage.Image = cursorImage;

            if (!_refControlBox.Visible && !Arrow.TopMost)
                Arrow.TopMost = true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ProcessMouseInput failed: {ex.Message}");
        }
    }

    private void ProcessRawInput(IntPtr rawInputPointer)
    {
        uint dwSize = 0;
        NativeMethods.GetRawInputData(rawInputPointer, NativeMethods.RID_INPUT, IntPtr.Zero, ref dwSize, (uint)Marshal.SizeOf<NativeMethods.RAWINPUTHEADER>());

        if (dwSize == 0) return;

        IntPtr buffer = Marshal.AllocHGlobal((int)dwSize);
        try
        {
            if (NativeMethods.GetRawInputData(rawInputPointer, NativeMethods.RID_INPUT, buffer, ref dwSize, (uint)Marshal.SizeOf<NativeMethods.RAWINPUTHEADER>()) != dwSize)
            {
                Debug.WriteLine("获取RawInput数据失败");
                return;
            }

            var raw = Marshal.PtrToStructure<NativeMethods.RAWINPUT>(buffer);

            if (raw.header.dwType == NativeMethods.RIM_TYPEMOUSE)
                ProcessMouseInput(raw);
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

    private void RegisterRawInput()
    {
        var rid = new NativeMethods.RAWINPUTDEVICE[1];
        rid[0].usUsagePage = 0x01;
        rid[0].usUsage = 0x02;
        rid[0].dwFlags = NativeMethods.RIDEV_INPUTSINK | NativeMethods.RIDEV_DEVNOTIFY;
        rid[0].hwndTarget = Handle;

        if (!NativeMethods.RegisterRawInputDevices(rid, (uint)rid.Length, (uint)Marshal.SizeOf(rid[0])))
        {
            int error = Marshal.GetLastWin32Error();
            Debug.WriteLine($"注册RawInput失败: {error}");
        }
    }

    private void Mirror_MouseClick(object sender, MouseEventArgs e)
    {
        InitializeMirror();
        ShowArrow();
    }

    private void Mirror_MouseDoubleClick(object sender, MouseEventArgs e) => Close();

    private void Mirror_Resize(object sender, EventArgs e)
    {
        if (FormLoaded) InitializeMirror();
    }

    private void Mirror_Shown(object sender, EventArgs e) => ShowArrow();

    private void ShowArrow()
    {
        Arrow.Show();
        Arrow.TopMost = true;
    }

    private void Mirror_LocationChanged(object sender, EventArgs e)
    {
        if (FormLoaded)
        {
            _preferedLocation = Location;
            LastBounds = Bounds;
        }
    }
}
