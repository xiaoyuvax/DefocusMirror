using System.Globalization;
using System.Reflection;
using FastColoredTextBoxNS;
using FastColoredTextBoxNS.Text;

namespace DefocusMirror;

public partial class ControlBox : Form
{
    private const string LAST_BOUNDS_KEY = "sizeLastBounds";
    private const string SIZE_FIT_HEIGHT_KEY = "sizeFitHeight";
    private const string SIZE_FIT_WIDTH_KEY = "sizeFitWidth";
    private const int MIN_SCREEN_COUNT = 2;
    private const float DEFAULT_SIGMA = 1f;
    private const int SIGMA_MULTIPLIER = 10;
    private const int MINI_MAP_DIVISOR = 10;
    private const int STICKER_DELAY_MS = 1000;
    private const int DEFAULT_FRAME_RATE = 30;

    private static readonly char[] SizeSeparators = ['x', 'X', '*', '×'];

    private Mirror _mainForm;
    private MirrorMode _lastMode;
    private bool _reallyExit;

    public ControlBox()
    {
        InitializeComponent();
        ApplyLocalization();
        LoadReadme();
        LoadScreenList();
    }

    private void LoadScreenList()
    {
        string[] screens = [.. Screen.AllScreens.Select((i, n) => $"{n} - {RemoveUnneededChars(i.DeviceName)}")];
        tscboFrom.Items.AddRange(screens);
        tscboTo.Items.AddRange(screens);
        tscboFrom.SelectedIndex = 0;
        tscboTo.SelectedIndex = screens.Length > 1 ? 1 : 0;
        tscboSize.SelectedIndex = 0;
    }

    private void ApplyLocalization()
    {
        Text = L.S("FormTitle");

        tabPage1.Text = L.S("tabPage1");
        tabPage2.Text = L.S("tabPage2");
        tabPage3.Text = L.S("tabPage3");

        toolStripLabel1.Text = L.S("lblCapture");
        toolStripLabel2.Text = L.S("lblOutput");
        toolStripLabel3.Text = L.S("lblFPS");
        toolStripLabel4.Text = L.S("lblDistance");

        btnRed.Text = L.S("btnMyopia");
        btnBlue.Text = L.S("btnHyperopia");
        btnClose0.Text = L.S("btnCloseMirror");
        btnCustom.Text = L.S("btnStart");
        btnClose.Text = L.S("btnClose");

        groupBox1.Text = L.S("grpRefPixels");
        label10.Text = L.S("lblRed");
        label11.Text = L.S("lblGreen");
        label12.Text = L.S("lblBlue");

        label1.Text = L.S("lblRSpread");
        label4.Text = L.S("lblRIntensity");
        label2.Text = L.S("lblGSpread");
        label5.Text = L.S("lblGIntensity");
        label3.Text = L.S("lblBSpread");
        label6.Text = L.S("lblBIntensity");

        label13.Text = L.S("msgDisclaimer");

        tsmiClose.Text = L.S("menuClose");
        tsmiShowControlBox.Text = L.S("menuShowBox");
        tsmiExit.Text = L.S("menuExit");
        启动近视离焦ToolStripMenuItem.Text = L.S("menuStartLast");

        tsmiLangZh.Text = L.S("langZh");
        tsmiLangEn.Text = L.S("langEn");
        tsddLanguage.Text = L.S("lblLanguage");

        notifyIcon1.Text = L.S("FormTitle");

        toolTip1.SetToolTip(btnClose0, L.S("tipCloseMirror"));
        toolTip1.SetToolTip(btnBlue, L.S("tipDefocusBtn"));
        toolTip1.SetToolTip(btnRed, L.S("tipDefocusBtn"));
        toolStripLabel1.ToolTipText = L.S("tipCapture");
        tscboFrom.ToolTipText = L.S("tipSourceScreen");
        toolStripLabel2.ToolTipText = L.S("tipOutput");
        tscboTo.ToolTipText = L.S("tipTargetScreen");
        toolStripLabel3.ToolTipText = L.S("tipFPS");
        tscboFrameRate.ToolTipText = L.S("tipFPS");
        toolStripLabel4.ToolTipText = L.S("tipDistance");
        tscboDistance.ToolTipText = L.S("tipDistance");
        tscboSize.ToolTipText = L.S("tipWindowSize");
        tsbFullScreen.ToolTipText = L.S("tipFullScreen");

        UpdateFullScreenText();
        UpdateSizeDropdownTexts();
    }

    private void UpdateFullScreenText()
    {
        tsbFullScreen.Text = tsbFullScreen.Checked ? L.S("btnFullscreen") : L.S("btnWindowed");
    }

    private void UpdateSizeDropdownTexts()
    {
        for (int i = 0; i < tscboSize.Items.Count; i++)
        {
            var txt = tscboSize.Items[i]?.ToString();
            if (txt == "适配目标屏高" || txt == "Fit target height")
                tscboSize.Items[i] = L.S("sizeFitHeight");
            else if (txt == "适配目标屏宽" || txt == "Fit target width")
                tscboSize.Items[i] = L.S("sizeFitWidth");
            else if (txt == "最近位置尺寸" || txt == "Last position & size")
                tscboSize.Items[i] = L.S("sizeLastBounds");
        }
    }

    private void LoadReadme()
    {
        try
        {
            var culture = L.CurrentCulture;
            var suffix = culture.StartsWith("en") ? "En" : "Zh";
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(dir, $"Readme{suffix}.md");
            if (File.Exists(path))
            {
                fctbReadme.Text = File.ReadAllText(path);
                fctbReadme.SelectionStart = 0;
                fctbReadme.SelectionLength = 0;
                fctbReadme.DoSelectionVisible();
            }
        }
        catch { }
    }

    private void SwitchLanguage(string culture)
    {
        L.ApplyCulture(culture);
        L.SaveCulture();
        ApplyLocalization();
        LoadReadme();
        RefreshGraph();
    }

    private static Point Centerize(Rectangle bounds, Rectangle container)
    {
        var location = Centerize(bounds, container.Size);
        location.Offset(container.X, container.Y);
        return location;
    }

    private static Point Centerize(Rectangle bounds, Size container, int defaultMargin = 0)
    {
        var marginX = (container.Width - bounds.Width) / 2;
        var marginY = (container.Height - bounds.Height) / 2;
        marginX = marginX > 0 ? marginX : defaultMargin;
        marginY = marginY > 0 ? marginY : defaultMargin;
        var xOffset = bounds.Left;
        xOffset = xOffset < 0 ? marginX - xOffset : marginX;
        var yOffset = bounds.Top;
        yOffset = yOffset < 0 ? marginY - yOffset : marginY;
        return new Point(xOffset, yOffset);
    }

    private static string RemoveUnneededChars(string s) => s.Replace(".", "").Replace("\\", "");

    private float ParseDistance() => uint.TryParse(tscboDistance.Text, out uint mm) && mm >= 100 ? mm : BlurCalculator.DefaultViewingDistanceMm;

    private float GetDisplayScale()
    {
        if (tscboFrom.SelectedIndex < 0 || tscboTo.SelectedIndex < 0) return 1f;
        if (tscboFrom.SelectedIndex >= Screen.AllScreens.Length || tscboTo.SelectedIndex >= Screen.AllScreens.Length) return 1f;

        var (adapter, output) = Mirror.FindAdapterOutputForScreen(Screen.AllScreens[tscboFrom.SelectedIndex].DeviceName);
        var srcBounds = Mirror.GetSourceScreenBounds(adapter, output);
        var tgtBounds = Mirror.GetTargetScreenBounds(tscboTo.SelectedIndex);
        return srcBounds.Width > 0 ? (float)tgtBounds.Width / srcBounds.Width : 1f;
    }

    private void btnBlue_Click(object sender, EventArgs e)
    {
        var (r, g, b) = BlurCalculator.GetHyperopiaBlurScales(BlurCalculator.DefaultPixelPitchMm, GetDisplayScale(), ParseDistance());
        ShowNewMirror(r, g, b, DEFAULT_SIGMA, DEFAULT_SIGMA, DEFAULT_SIGMA);
        _lastMode = MirrorMode.Blue;
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        _mainForm?.Close();
    }

    private void btnCustom_Click(object sender, EventArgs e)
    {
        ShowNewMirror(
            (float)numScaleR.Value, (float)numScaleG.Value, (float)numScaleB.Value,
            (float)numSigmaR.Value, (float)numSigmaG.Value, (float)numSigmaB.Value);
        _lastMode = MirrorMode.Custom;
    }

    private void btnRed_Click(object sender, EventArgs e)
    {
        var (r, g, b) = BlurCalculator.GetMyopiaBlurScales(BlurCalculator.DefaultPixelPitchMm, GetDisplayScale(), ParseDistance());
        ShowNewMirror(r, g, b, DEFAULT_SIGMA, DEFAULT_SIGMA, DEFAULT_SIGMA);
        _lastMode = MirrorMode.Red;
    }

    public void StartByLastConfig()
    {
        switch (_lastMode)
        {
            case MirrorMode.Blue:
                btnBlue_Click(null!, null!);
                break;
            case MirrorMode.Red:
                btnRed_Click(null!, null!);
                break;
            case MirrorMode.Custom:
                btnCustom_Click(null!, null!);
                break;
        }
    }

    private void cmsNotify_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        tsmiClose.Enabled = _mainForm != null && !_mainForm.IsDisposed;
    }

    private void ControlBox_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (_reallyExit || e.CloseReason == CloseReason.ApplicationExitCall || e.CloseReason == CloseReason.WindowsShutDown)
            return;
        Hide();
        e.Cancel = true;
    }

    private void ControlBox_SizeChanged(object sender, EventArgs e)
    {
        if (WindowState == FormWindowState.Minimized) Hide();
    }

    private Mirror CreateNewMirror(float blurScaleR = 0, float blurScaleG = 0, float blurScaleB = 0, float blurSigmaR = 1, float blurSigmaG = 1, float blurSigmaB = 1)
    {
        if (tscboFrom.SelectedIndex < 0 || tscboTo.SelectedIndex < 0)
        {
            MessageBox.Show(L.S("msgSelectScreen"), L.S("msgParamError"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return null;
        }

        if (tscboFrom.SelectedIndex == tscboTo.SelectedIndex)
        {
            MessageBox.Show(L.S("msgSameScreen"), L.S("msgParamError"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return null;
        }

        var srcScreen = Screen.AllScreens[tscboFrom.SelectedIndex];
        var (srcAdapter, srcOutput) = Mirror.FindAdapterOutputForScreen(srcScreen.DeviceName);

        Rectangle mirrorBounds = default;
        var tgtBounds = Mirror.GetTargetScreenBounds(tscboTo.SelectedIndex);
        var srcBounds = Mirror.GetSourceScreenBounds(srcAdapter, srcOutput);
        var srcRatio = (float)srcBounds.Width / srcBounds.Height;

        var sizeText = tscboSize.Text;
        var fitHeight = L.S(SIZE_FIT_HEIGHT_KEY);
        var fitWidth = L.S(SIZE_FIT_WIDTH_KEY);
        var lastBounds = L.S(LAST_BOUNDS_KEY);

        if (sizeText == fitHeight)
        {
            mirrorBounds.Height = tgtBounds.Height + DefaultMargin.Vertical;
            mirrorBounds.Width = (int)(srcRatio * tgtBounds.Height) + DefaultMargin.Horizontal;
            mirrorBounds.Location = Centerize(mirrorBounds, tgtBounds);
        }
        else if (sizeText == fitWidth)
        {
            mirrorBounds.Width = tgtBounds.Width + DefaultMargin.Horizontal;
            mirrorBounds.Height = (int)(tgtBounds.Width / srcRatio) + DefaultMargin.Vertical;
            mirrorBounds.Location = Centerize(mirrorBounds, tgtBounds);
        }
        else if (sizeText.StartsWith(lastBounds))
        {
            mirrorBounds = Mirror.LastBounds;
        }
        else
        {
            var parts = tscboSize.Text.Split(SizeSeparators)
                .Select(i => int.TryParse(i, out int s) ? s : 0)
                .ToArray();

            if (parts.Length < 2 || parts[0] <= 0 || parts[1] <= 0)
            {
                MessageBox.Show(L.S("msgInvalidSize"), L.S("msgInitialSize"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            mirrorBounds = new Rectangle(0, 0, parts[0], parts[1]);
            mirrorBounds.Location = Centerize(mirrorBounds, tgtBounds);
        }

        if (tsbFullScreen.Checked || (mirrorBounds.Height > 0 && mirrorBounds.Width > 0))
        {
            return new(this, blurScaleR, blurScaleG, blurScaleB, blurSigmaR, blurSigmaG, blurSigmaB,
                srcAdapter, srcOutput, tscboTo.SelectedIndex,
                ParseFrameRate(), tsbFullScreen.Checked, mirrorBounds);
        }

        MessageBox.Show(L.S("msgInvalidSize"), L.S("msgInitialSize"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        return null;
    }

    private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        Show();
        WindowState = FormWindowState.Normal;
        Activate();
        TopMost = true;
        TopMost = false;
    }

    private void numScaleB_Click(object sender, EventArgs e) => trbScaleB.Value = (int)numScaleB.Value;
    private void numScaleG_Click(object sender, EventArgs e) => trbScaleG.Value = (int)numScaleG.Value;
    private void numScaleR_Click(object sender, EventArgs e) => trbScaleR.Value = (int)numScaleR.Value;
    private void numSigmaB_ValueChanged(object sender, EventArgs e) => trbSigmaB.Value = (int)(numSigmaB.Value * SIGMA_MULTIPLIER);
    private void numSigmaG_ValueChanged(object sender, EventArgs e) => trbSigmaG.Value = (int)(numSigmaG.Value * SIGMA_MULTIPLIER);
    private void numSigmaR_ValueChanged(object sender, EventArgs e) => trbSigmaR.Value = (int)(numSigmaR.Value * SIGMA_MULTIPLIER);

    private int ParseFrameRate() => uint.TryParse(tscboFrameRate.Text, out uint n) ? (int)n : DEFAULT_FRAME_RATE;

    private void pnlGraph_Resize(object sender, EventArgs e) => RefreshGraph();

    private void RefreshGraph()
    {
        pnlGraph.Controls.Clear();

        var srcLabel = L.S("srcScreen");
        var mirrorLabel = L.S("mirrorScreen");

        foreach (var s in Screen.AllScreens)
        {
            string text = "";
            if (tscboFrom.SelectedItem != null)
                text = $"{s.DeviceName}{(tscboFrom.SelectedItem.ToString().EndsWith(RemoveUnneededChars(s.DeviceName)) ? $":[{srcLabel}]" : "")}";

            if (tscboTo.SelectedItem != null)
            {
                if (tscboTo.SelectedItem.ToString().EndsWith(RemoveUnneededChars(s.DeviceName)))
                {
                    var text2 = $"{s.DeviceName}:[{mirrorLabel}]";
                    if (text.Contains(":[")) text += $"\r{text2}";
                    else text = text2;
                }
            }
            else text = s.DeviceName;

            text = RemoveUnneededChars(text);

            var location = Centerize(
                new Rectangle(
                    SystemInformation.VirtualScreen.X / MINI_MAP_DIVISOR,
                    SystemInformation.VirtualScreen.Y / MINI_MAP_DIVISOR,
                    SystemInformation.VirtualScreen.Width / MINI_MAP_DIVISOR,
                    SystemInformation.VirtualScreen.Height / MINI_MAP_DIVISOR),
                pnlGraph.Size, 50);
            location.Offset(s.Bounds.X / MINI_MAP_DIVISOR, s.Bounds.Y / MINI_MAP_DIVISOR);

            var display = new Label
            {
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.LightCyan,
                Text = text,
                Location = location,
                Size = new Size(s.Bounds.Width / MINI_MAP_DIVISOR, s.Bounds.Height / MINI_MAP_DIVISOR),
                TextAlign = ContentAlignment.MiddleCenter
            };

            display.Click += (sender, e) =>
            {
                var sticker = new frmSticker
                {
                    LabelText = display.Text,
                    Location = new Point(0, 0),
                    StartPosition = FormStartPosition.Manual,
                    Opacity = 0.8f
                };
                var stickerLocation = Centerize(sticker.Bounds, s.Bounds.Size);
                stickerLocation.Offset(s.Bounds.X, s.Bounds.Y);
                sticker.Location = stickerLocation;
                sticker.TopMost = true;
                sticker.Show();
                Task.Delay(STICKER_DELAY_MS).ContinueWith(_ => Invoke(() => sticker.Close()));
            };

            pnlGraph.Controls.Add(display);
        }
    }

    private void ShowNewMirror(float blurScaleR = 0, float blurScaleG = 0, float blurScaleB = 0, float blurSigmaR = 1, float blurSigmaG = 1, float blurSigmaB = 1)
    {
        if (Screen.AllScreens.Length < MIN_SCREEN_COUNT)
        {
            if (MessageBox.Show(
                L.S("msgExtendDesktop"),
                L.S("msgNoMonitor"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                == DialogResult.No) return;
        }

        _mainForm?.Close();
        _mainForm?.Dispose();
        _mainForm = CreateNewMirror(blurScaleR, blurScaleG, blurScaleB, blurSigmaR, blurSigmaG, blurSigmaB);
        _mainForm?.Show();
    }

    private void trbScaleB_Scroll(object sender, EventArgs e) => numScaleB.Value = trbScaleB.Value / 10.0m;
    private void trbScaleG_Scroll(object sender, EventArgs e) => numScaleG.Value = trbScaleG.Value / 10.0m;
    private void trbScaleR_Scroll(object sender, EventArgs e) => numScaleR.Value = trbScaleR.Value / 10.0m;
    private void trbSigmaB_Scroll(object sender, EventArgs e) => numSigmaB.Value = trbSigmaB.Value / 10.0m;
    private void trbSigmaG_Scroll(object sender, EventArgs e) => numSigmaG.Value = trbSigmaG.Value / 10.0m;
    private void trbSigmaR_Scroll(object sender, EventArgs e) => numSigmaR.Value = trbSigmaR.Value / 10.0m;

    private void tsbFullScreen_CheckedChanged(object sender, EventArgs e)
    {
        tscboSize.Visible = !tsbFullScreen.Checked;
        UpdateFullScreenText();
    }

    private void tscboFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (tscboFrom.SelectedIndex >= 0
            && tscboFrom.SelectedIndex == tscboTo.SelectedIndex
            && tscboTo.Items.Count > 1)
        {
            tscboTo.SelectedIndex = tscboFrom.SelectedIndex == 0 ? 1 : 0;
        }
        RefreshGraph();
    }

    private void tscboTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (tscboTo.SelectedIndex >= 0
            && tscboTo.SelectedIndex == tscboFrom.SelectedIndex
            && tscboFrom.Items.Count > 1)
        {
            tscboFrom.SelectedIndex = tscboTo.SelectedIndex == 0 ? 1 : 0;
        }
        RefreshGraph();
    }

    private void tsmiClose_Click(object sender, EventArgs e) => _mainForm?.Close();

    private void tsmiExit_Click(object sender, EventArgs e)
    {
        _reallyExit = true;
        notifyIcon1.Visible = false;
        Application.Exit();
    }

    private void tsmiShowControlBox_Click(object sender, EventArgs e)
    {
        notifyIcon1_MouseDoubleClick(sender!, null!);
    }

    private void tscboSize_DropDown(object sender, EventArgs e)
    {
        var lastBoundsKey = L.S(LAST_BOUNDS_KEY);
        if (Mirror.LastBounds != default)
        {
            if (tscboSize.Items.Count == 0 || !tscboSize.Items[0].ToString()!.StartsWith(lastBoundsKey))
                tscboSize.Items.Insert(0, lastBoundsKey);
        }
        else
        {
            tscboSize.Items.Remove(lastBoundsKey);
        }
    }

    private void 启动近视离焦ToolStripMenuItem_Click(object sender, EventArgs e) => StartByLastConfig();

    private void tsddLanguage_Click(object sender, EventArgs e)
    {
        // Handled by individual menu items
    }

    private void tsmiLangZh_Click(object sender, EventArgs e) => SwitchLanguage("zh-CN");
    private void tsmiLangEn_Click(object sender, EventArgs e) => SwitchLanguage("en");

    private void cmsNotify_DoubleClick(object sender, EventArgs e) { }
    private void ControlBox_Activated(object sender, EventArgs e) { }
    private void numScaleB_ValueChanged(object sender, EventArgs e) { }
    private void numScaleG_ValueChanged(object sender, EventArgs e) { }
    private void numScaleR_ValueChanged(object sender, EventArgs e) { }

    private void fctbReadme_TextChanged(object sender, TextChangedEventArgs e)
    {
        fctbReadme.OnSyntaxHighlight(e);
    }
}
