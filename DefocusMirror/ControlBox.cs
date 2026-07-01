using SharpDX.DXGI;

namespace DefocusMirror
{
    public partial class ControlBox : Form
    {
        private const string LAST_BOUNDS = "最近位置尺寸";
        private Mirror _mainForm;
        private MirrorMode _lastMode;

        public ControlBox()
        {
            InitializeComponent();
            var dxgiFactory = new Factory1();
            string[] adapters = [.. dxgiFactory.Adapters.Select((i, n) => $"{n} - {i.Description.Description}")];
            string[] screens = [.. Screen.AllScreens.Select((i, n) => $"{n} - {RemoveUnneededChars(i.DeviceName)}")];
            tscboFrom.Items.AddRange(adapters);
            tscboTo.Items.AddRange(screens);
            tscboFrom.SelectedIndex = tscboTo.SelectedIndex = 0;
            tscboSize.SelectedIndex = 0;
            if (tscboOutput.Text == tscboTo.Text && tscboTo.Items.Count > 1) tscboTo.SelectedIndex = 1;
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

        private void btnBlue_Click(object sender, EventArgs e)
        {
            ShowNewMirror(3f, 1f, 0f, 1f, 1f, 1f);
            _lastMode = MirrorMode.Blue;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            _mainForm?.Close();
        }

        private void btnCustom_Click(object sender, EventArgs e)
        {
            ShowNewMirror((float)numScaleR.Value, (float)numScaleG.Value, (float)numScaleB.Value, (float)numSigmaR.Value, (float)numSigmaG.Value, (float)numSigmaB.Value);
            _lastMode = MirrorMode.Custom;
        }

        private void btnRed_Click(object sender, EventArgs e)
        {
            ShowNewMirror(0, 1f, 3f, 1f, 1f, 1f);
            _lastMode = MirrorMode.Red;
        }

        public void StartByLastConfig()
        {
            switch (_lastMode)
            {
                case MirrorMode.Blue:
                    btnBlue_Click(null, null);
                    break;

                case MirrorMode.Red:
                    btnRed_Click(null, null);
                    break;

                case MirrorMode.Custom:
                    btnCustom_Click(null, null);
                    break;
            }
        }

        private void cmsNotify_DoubleClick(object sender, EventArgs e)
        {
        }

        private void cmsNotify_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tsmiClose.Enabled = _mainForm != null && !_mainForm.IsDisposed;
        }

        private void ControlBox_Activated(object sender, EventArgs e)
        {
        }

        private void ControlBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void ControlBox_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized) Hide();
        }

        private Mirror CreateNewMirror(float blurScaleR = 0, float blurScaleG = 0, float blurScaleB = 0, float blurSigmaR = 1, float blurSigmaG = 1, float blurSigmaB = 1)
        {
            Rectangle mirrorBounds = default;
            var tgtBounds = Mirror.GetTargetScreenBounds(tscboTo.SelectedIndex);
            var srcBounds = Mirror.GetSourceScreenBounds(tscboFrom.SelectedIndex, tscboOutput.SelectedIndex);

            var srcRatio = (float)srcBounds.Width / srcBounds.Height;

            switch (tscboSize.Text)
            {
                case "适配目标屏高":
                    mirrorBounds.Height = tgtBounds.Height + DefaultMargin.Vertical;
                    mirrorBounds.Width = (int)(srcRatio * tgtBounds.Height) + DefaultMargin.Horizontal;
                    mirrorBounds.Location = Centerize(mirrorBounds, tgtBounds);
                    break;

                case "适配目标屏宽":
                    mirrorBounds.Width = tgtBounds.Width + DefaultMargin.Horizontal;
                    mirrorBounds.Height = (int)(tgtBounds.Width / srcRatio) + DefaultMargin.Vertical;
                    mirrorBounds.Location = Centerize(mirrorBounds, tgtBounds);
                    break;

                case LAST_BOUNDS:
                    mirrorBounds = Mirror.LastBounds;
                    break;

                default:
                    {
                        var windowSize = tscboSize.Text.Split(['x', 'X', '*', '×']).Select(i => int.TryParse(i, out int s) ? s : 0).ToArray();
                        mirrorBounds = new Rectangle(0, 0, windowSize[0], windowSize[1]);
                        mirrorBounds.Location = Centerize(mirrorBounds, tgtBounds);
                        break;
                    }
            }

            if (!tsbFullScreen.Checked || (mirrorBounds.Height > 0 && mirrorBounds.Width > 0))
                return new(this, blurScaleR, blurScaleG, blurScaleB, blurSigmaR, blurSigmaG, blurSigmaB, tscboFrom.SelectedIndex, tscboOutput.SelectedIndex, tscboTo.SelectedIndex, ParseFrameRate(), tsbFullScreen.Checked, mirrorBounds);
            else MessageBox.Show("离焦屏尺寸错误！请确定数字与格式正确。格式:1920x1080", "初始尺寸", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Show();
            TopMost = true;
        }

        private void numScaleB_Click(object sender, EventArgs e)
        {
            trbScaleB.Value = (int)(numScaleB.Value);
        }

        private void numScaleB_ValueChanged(object sender, EventArgs e)
        {
        }

        private void numScaleG_Click(object sender, EventArgs e)
        {
            trbScaleG.Value = (int)(numScaleG.Value);
        }

        private void numScaleG_ValueChanged(object sender, EventArgs e)
        {
        }

        private void numScaleR_Click(object sender, EventArgs e)
        {
            trbScaleR.Value = (int)(numScaleR.Value);
        }

        private void numScaleR_Scroll(object sender, ScrollEventArgs e)
        {
        }

        private void numScaleR_ValueChanged(object sender, EventArgs e)
        {
        }

        private void numSigmaB_ValueChanged(object sender, EventArgs e)
        {
            trbSigmaB.Value = (int)(numSigmaB.Value * 10);
        }

        private void numSigmaG_ValueChanged(object sender, EventArgs e)
        {
            trbSigmaG.Value = (int)(numSigmaG.Value * 10);
        }

        private void numSigmaR_ValueChanged(object sender, EventArgs e)
        {
            trbSigmaR.Value = (int)(numSigmaR.Value * 10);
        }

        private int ParseFrameRate() => uint.TryParse(tscboFrameRate.Text, out uint n) ? (int)n : 30;

        private void pnlGraph_Resize(object sender, EventArgs e)
        {
            RefreshGraph();
        }

        private void RefreshGraph()
        {
            pnlGraph.Controls.Clear();
            foreach (var s in Screen.AllScreens)
            {
                string text = "";
                if (tscboFrom.SelectedItem != null) text = $"{s.DeviceName}{(tscboOutput.SelectedItem.ToString().EndsWith(RemoveUnneededChars(s.DeviceName)) ? ":[源屏]" : "")}";
                if (tscboTo.SelectedItem != null)
                {
                    if (tscboTo.SelectedItem.ToString().EndsWith(RemoveUnneededChars(s.DeviceName)))
                    {
                        var text2 = $"{s.DeviceName}{":[离焦屏]"}";
                        if (text.Contains(":[")) text += $"\r{text2}";
                        else text = text2;
                    }
                }
                else text = s.DeviceName;
                text = RemoveUnneededChars(text);

                var location = Centerize(new Rectangle(SystemInformation.VirtualScreen.X / 10, SystemInformation.VirtualScreen.Y / 10, SystemInformation.VirtualScreen.Width / 10, SystemInformation.VirtualScreen.Height / 10), pnlGraph.Size, 50);
                location.Offset(s.Bounds.X / 10, s.Bounds.Y / 10);  //微缩图里面要缩小10倍
                var display = new Label()
                {
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.LightCyan,
                    Text = text,
                    Location = location,
                    Size = new Size(s.Bounds.Width / 10, s.Bounds.Height / 10),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                display.Click += (sender, e) =>
                {
                    var sticker = new frmSticker() { LabelText = display.Text, Location = new Point(0, 0), StartPosition = FormStartPosition.Manual, Opacity = 0.8f };
                    var location = Centerize(sticker.Bounds, s.Bounds.Size);
                    location.Offset(s.Bounds.X, s.Bounds.Y);
                    sticker.Location = location;
                    sticker.TopMost = true;
                    sticker.Show();
                    Task.Delay(1000).ContinueWith(i => Invoke(() => sticker.Close()));
                };

                pnlGraph.Controls.Add(display);
            }
        }

        private void ShowNewMirror(float blurScaleR = 0, float blurScaleG = 0, float blurScaleB = 0, float blurSigmaR = 1, float blurSigmaG = 1, float blurSigmaB = 1)
        {
            if (Screen.AllScreens.Length < 2)
                if (MessageBox.Show("当前未检测第二个显示器或桌面未扩展到第二个显示器，无法正常启动离焦屏！\r请连接至少两个显示器并将桌面扩展（而不是复制）到第二个显示器后重试。如果选择“是”可以继续以窗体模式体验本应用。\r\r要继续吗？", "显示器数量不足", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                    == DialogResult.No) return;

            _mainForm?.Close();
            _mainForm?.Dispose();
            _mainForm = CreateNewMirror(blurScaleR, blurScaleG, blurScaleB, blurSigmaR, blurSigmaG, blurSigmaB);
            _mainForm?.Show();
        }

        private void trbScaleB_Scroll(object sender, EventArgs e)
        {
            numScaleB.Value = trbScaleB.Value / 10.0m;
        }

        private void trbScaleG_Scroll(object sender, EventArgs e)
        {
            numScaleG.Value = trbScaleG.Value / 10.0m;
        }

        private void trbScaleR_Scroll(object sender, EventArgs e)
        {
            numScaleR.Value = trbScaleR.Value / 10.0m;
        }

        private void trbSigmaB_Scroll(object sender, EventArgs e)
        {
            numSigmaB.Value = trbSigmaB.Value / 10.0m;
        }

        private void trbSigmaG_Scroll(object sender, EventArgs e)
        {
            numSigmaG.Value = trbSigmaG.Value / 10.0m;
        }

        private void trbSigmaR_Scroll(object sender, EventArgs e)
        {
            numSigmaR.Value = trbSigmaR.Value / 10.0m;
        }

        private void tsbFullScreen_CheckedChanged(object sender, EventArgs e)
        {
            tscboSize.Visible = !tsbFullScreen.Checked;
            if (tsbFullScreen.Checked)
            {
                tsbFullScreen.Text = "全屏模式";
            }
            else
            {
                tsbFullScreen.Text = "窗口模式";
            }
        }

        private void tscboFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dxgiFactory = new Factory1();
            var adapter = dxgiFactory.Adapters[tscboFrom.SelectedIndex];
            string[] outputs = [.. adapter.Outputs.Select((i, n) => $"{n} - {RemoveUnneededChars(i.Description.DeviceName)}")];
            if (outputs.Length > 0)
            {
                tscboOutput.Items.Clear();
                tscboOutput.Items.AddRange(outputs);
                tscboOutput.SelectedIndex = 0;
            }
            else tscboFrom.Items.Remove(tscboFrom.SelectedItem);
        }

        private void tscboOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshGraph();
        }

        private void tscboTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshGraph();
        }

        private void tsmiClose_Click(object sender, EventArgs e)
        {
            _mainForm?.Close();
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void tsmiShowControlBox_Click(object sender, EventArgs e)
        {
            notifyIcon1_MouseDoubleClick(sender, null);
        }

        private void tscboSize_DropDown(object sender, EventArgs e)
        {
            if (Mirror.LastBounds != default)
            {
                if (!tscboSize.Items[0].ToString().StartsWith(LAST_BOUNDS)) tscboSize.Items.Insert(0, LAST_BOUNDS);
            }
            else tscboSize.Items.Remove(LAST_BOUNDS);
        }

        private void 启动近视离焦ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartByLastConfig   ();
        }
    }
}