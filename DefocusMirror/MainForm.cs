using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using Timer = System.Windows.Forms.Timer;

namespace DefocusMirror;

public partial class MainForm : Form
{
    private const int GWL_EXSTYLE = -20;
    private const int HWND_TOPMOST = -1;
    private const int LWA_ALPHA = 0x2;
    private const int SWP_NOMOVE = 0x0002;
    private const int SWP_NOSIZE = 0x0001;
    private const int WS_EX_LAYERED = 0x80000;
    private const int WS_EX_NOACTIVATE = 0x8000000;
    private const int WS_EX_TOPMOST = 0x8;
    private const int WS_EX_TRANSPARENT = 0x20;
    private readonly Timer _captureTimer;
    private readonly int _dstScreenIndex;
    private readonly ScreenCapture _screenCapture;
    private readonly int _srcAdapterIndex;
    public MainForm(int blurRadiusR = 0, int blurRadiusG = 0, int blurRadiusB = 0, float blurSigmaR = 1, float blurSigmaG = 1, float blurSigmaB = 1, int? adapterSrcIndex = null, int? outputSrcIndex = null, int? adapterDstIndex = null)
    {
        InitializeComponent();

        _srcAdapterIndex = adapterSrcIndex ?? 0;
        _dstScreenIndex = adapterDstIndex ?? 0;
        StartPosition = FormStartPosition.Manual;
        if (Screen.AllScreens.Length > 1) Bounds = Screen.AllScreens[_dstScreenIndex].Bounds;
        // 设置全屏，TopMost，输入穿透
        FormBorderStyle = FormBorderStyle.None;
        WindowState = FormWindowState.Maximized;
        TopMost = true;

        int extendedStyle = GetWindowLong(Handle, GWL_EXSTYLE);
        SetWindowLong(Handle, GWL_EXSTYLE, extendedStyle | WS_EX_LAYERED | WS_EX_TRANSPARENT);

        // 设置不透明度为255（完全不透明），实现鼠标穿透
        SetLayeredWindowAttributes(Handle, 0, 255, LWA_ALPHA);

        // 初始化屏幕捕获
        _screenCapture = new ScreenCapture(blurRadiusR, blurRadiusG, blurRadiusB, blurSigmaR, blurSigmaG, blurSigmaB, Handle, _srcAdapterIndex);

        // 创建并启动捕获定时器
        _captureTimer = new Timer
        {
            Interval = 33 // 大约30 FPS
        };
        _captureTimer.Tick += (s, e) => CaptureAndRender();
        _captureTimer.Start();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _captureTimer.Stop();
        _screenCapture.Dispose();
        base.OnFormClosing(e);
    }

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
    private static extern bool SetLayeredWindowAttributes(IntPtr hwnd, int crKey, byte bAlpha, int dwFlags);

    // API声明
    [DllImport("user32.dll", SetLastError = true)]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

    private void CaptureAndRender()
    {
        _screenCapture.CaptureScreen();
        _screenCapture.Render();
    }
    private void MainForm_Load(object sender, EventArgs e)
    {
    }
}