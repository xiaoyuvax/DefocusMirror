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
        }

        private void btnCustom_Click(object sender, EventArgs e)
        {
            _mainForm?.Close();
            _mainForm = new MainForm((int)numScaleR.Value, (int)numScaleG.Value, (int)numScaleB.Value, (float)numSigmaR.Value, (float)numSigmaG.Value, (float)numSigmaB.Value, tscboFrom.SelectedIndex, tscboOutput.SelectedIndex, tscboTo.SelectedIndex);
            _mainForm.Show();
        }

        private void btnRed_Click(object sender, EventArgs e)
        {
            _mainForm?.Close();
            _mainForm = new MainForm(0, 5, 13, 1, 0.5f, 1.3f, tscboFrom.SelectedIndex, tscboOutput.SelectedIndex, tscboTo.SelectedIndex);
            _mainForm.Show();
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

        private void btnBlue_Click(object sender, EventArgs e)
        {
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            _mainForm?.Close();
        }

        private void tsmiShowControlBox_Click(object sender, EventArgs e)
        {
            BringToFront();
        }

        private void tsmiClose_Click(object sender, EventArgs e)
        {
            _mainForm?.Close();
        }

        private void cmsNotify_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tsmiClose.Enabled = _mainForm != null && !_mainForm.IsDisposed;
        }

        private void cmsNotify_DoubleClick(object sender, EventArgs e)
        {
            BringToFront();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tscboDistance_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (uint.TryParse(tscboDistance.Text, out uint n))
            {
                var circles = BlurCalculator.GetRGBBlurCircleDiameters(n);
                var pixelSize = PixelSizeCalculator.GetPixelPhysicalSize(tscboTo.SelectedIndex);
                tslBlurCircles.Text = $"{Math.Abs(circles.R) / pixelSize.pixelWidth:F2}x{Math.Abs(circles.G) / pixelSize.pixelWidth:F2}x{Math.Abs(circles.B) / pixelSize.pixelWidth:F2}mm";
            }
        }


        private void tscboDistance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tscboDistance_SelectedIndexChanged(sender, new EventArgs());
            }
        }

        private void tscboFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dxgiFactory = new Factory1();
            var adapter = dxgiFactory.Adapters[tscboFrom.SelectedIndex];
            string[] outputs = [.. adapter.Outputs.Select((i, n) => $"{n} - {i.Description.DeviceName}")];
            tscboOutput.Items.AddRange(outputs);
            tscboOutput.SelectedIndex = 0;
        }
    }
}