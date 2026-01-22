using SharpDX.DXGI;

namespace DefocusMirror
{
    public partial class ControlBox : Form
    {
        private MainForm _mainForm;

        public ControlBox()
        {
            InitializeComponent();
            var dxgiFactory = new Factory1();
            string[] adapters = [.. dxgiFactory.Adapters.Select((i, n) => $"{n} - {i.Description.Description}")];
            string[] screens = [.. Screen.AllScreens.Select((i, n) => $"{n} - {i.DeviceName}")];
            tscboFrom.Items.AddRange(adapters);
            tscboTo.Items.AddRange(screens);
            tscboFrom.SelectedIndex = tscboTo.SelectedIndex = 0;
            if (tscboOutput.Text == tscboTo.Text && tscboTo.Items.Count > 1) tscboTo.SelectedIndex = 1;
        }

        private static Point Centerize(Rectangle bounds, Size container)
        {
            var marginX = (container.Width - bounds.Width) / 2;
            var marginY = (container.Height - bounds.Height) / 2;
            marginX = marginX > 0 ? marginX : 50;
            marginY = marginY > 0 ? marginY : 50;
            var xOffset = bounds.Left;
            xOffset = xOffset < 0 ? marginX - xOffset : marginX;
            var yOffset = bounds.Top;
            yOffset = yOffset < 0 ? marginY - yOffset : marginY;

            return new Point(xOffset, yOffset);
        }

        private void btnBlue_Click(object sender, EventArgs e)
        {
            _mainForm?.Close();
            _mainForm = CreateNewMainForm(3f, 1f, 0f, 2f, 1f, 1f);
            _mainForm.Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            _mainForm?.Close();
        }

        private void btnCustom_Click(object sender, EventArgs e)
        {
            _mainForm?.Close();
            _mainForm = CreateNewMainForm((float)numScaleR.Value, (float)numScaleG.Value, (float)numScaleB.Value, (float)numSigmaR.Value, (float)numSigmaG.Value, (float)numSigmaB.Value);
            _mainForm.Show();
        }

        private void btnRed_Click(object sender, EventArgs e)
        {
            _mainForm?.Close();

            _mainForm = CreateNewMainForm(0, 1f, 3f, 1f, 1f, 2f);

            _mainForm.Show();
        }

        private void cmsNotify_DoubleClick(object sender, EventArgs e)
        {
        }

        private void cmsNotify_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tsmiClose.Enabled = _mainForm != null && !_mainForm.IsDisposed;
        }

        private MainForm CreateNewMainForm(float blurScaleR = 0, float blurScaleG = 0, float blurScaleB = 0, float blurSigmaR = 1, float blurSigmaG = 1, float blurSigmaB = 1)
            => new(this, blurScaleR, blurScaleG, blurScaleB, blurSigmaR, blurSigmaG, blurSigmaB, tscboFrom.SelectedIndex, tscboOutput.SelectedIndex, tscboTo.SelectedIndex, ParseFrameRate(), tsbFullScreen.Checked);

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            TopMost = true;
            BringToFront();
            TopMost = false;
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
                if (tscboFrom.SelectedItem != null) text = s.DeviceName + (tscboOutput.SelectedItem.ToString().EndsWith(s.DeviceName) ? "[源屏]" : "");
                if (tscboTo.SelectedItem != null)
                {
                    var text2 = s.DeviceName + (tscboTo.SelectedItem.ToString().EndsWith(s.DeviceName) ? "[离焦屏]" : "");
                    if (string.IsNullOrEmpty(text)) text = text2;
                    else if (text != text2) text += "\r" + text2;
                }
                else text = s.DeviceName;
                text = text.Replace(".", "").Replace("\\", "");

                var location = Centerize(new Rectangle(SystemInformation.VirtualScreen.X / 10, SystemInformation.VirtualScreen.Y / 10, SystemInformation.VirtualScreen.Width / 10, SystemInformation.VirtualScreen.Height / 10), pnlGraph.Size);
                location.Offset(s.Bounds.X / 10, s.Bounds.Y / 10);
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

                    var arrow = new frmSticker() { LabelText = display.Text, Location = new Point(0, 0), StartPosition = FormStartPosition.Manual };
                    var location = Centerize(arrow.Bounds, s.Bounds.Size);
                    location.Offset(s.Bounds.X, s.Bounds.Y);
                    arrow.Location = location;
                    arrow.Show();
                    Task.Delay(1000).ContinueWith(i => Invoke(() => arrow.Close()));
                };

                pnlGraph.Controls.Add(display);
            }
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

        private void tscboFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dxgiFactory = new Factory1();
            var adapter = dxgiFactory.Adapters[tscboFrom.SelectedIndex];
            string[] outputs = [.. adapter.Outputs.Select((i, n) => $"{n} - {i.Description.DeviceName}")];
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
            Close();
        }

        private void tsmiShowControlBox_Click(object sender, EventArgs e)
        {
            notifyIcon1_MouseDoubleClick(sender, null);
        }

        private void tsbFullScreen_Click(object sender, EventArgs e)
        {

        }

        private void tsbFullScreen_CheckedChanged(object sender, EventArgs e)
        {
            if (tsbFullScreen.Checked) tsbFullScreen.Text = "全屏模式";
            else tsbFullScreen.Text = "窗口模式";
        }

        private void ControlBox_Activated(object sender, EventArgs e)
        {

        }

        private void ControlBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }
    }
}