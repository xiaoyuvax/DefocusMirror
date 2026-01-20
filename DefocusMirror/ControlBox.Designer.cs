namespace DefocusMirror
{
    partial class ControlBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlBox));
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            btnBlue = new Button();
            btnRed = new Button();
            tabPage2 = new TabPage();
            label6 = new Label();
            label3 = new Label();
            label5 = new Label();
            label2 = new Label();
            label4 = new Label();
            numSigmaB = new NumericUpDown();
            label1 = new Label();
            numSigmaG = new NumericUpDown();
            numScaleB = new NumericUpDown();
            numSigmaR = new NumericUpDown();
            numScaleG = new NumericUpDown();
            trbSigmaB = new TrackBar();
            numScaleR = new NumericUpDown();
            trbSigmaG = new TrackBar();
            trbScaleB = new TrackBar();
            trbSigmaR = new TrackBar();
            trbScaleG = new TrackBar();
            trbScaleR = new TrackBar();
            btnClose = new Button();
            btnCustom = new Button();
            toolStrip1 = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            tscboFrom = new ToolStripComboBox();
            toolStripLabel2 = new ToolStripLabel();
            tscboTo = new ToolStripComboBox();
            tslDistance = new ToolStripLabel();
            tscboDistance = new ToolStripComboBox();
            tslBlurCircles = new ToolStripLabel();
            notifyIcon1 = new NotifyIcon(components);
            cmsNotify = new ContextMenuStrip(components);
            tsmiClose = new ToolStripMenuItem();
            tsmiShowControlBox = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            tsmiExit = new ToolStripMenuItem();
            tscboOutput = new ToolStripComboBox();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numSigmaB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSigmaG).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numScaleB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSigmaR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numScaleG).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trbSigmaB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numScaleR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trbSigmaG).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trbScaleB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trbSigmaR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trbScaleG).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trbScaleR).BeginInit();
            toolStrip1.SuspendLayout();
            cmsNotify.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 25);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(800, 425);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(btnBlue);
            tabPage1.Controls.Add(btnRed);
            tabPage1.Location = new Point(4, 26);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(792, 395);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "快速启动";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnBlue
            // 
            btnBlue.Location = new Point(253, 29);
            btnBlue.Name = "btnBlue";
            btnBlue.Size = new Size(184, 171);
            btnBlue.TabIndex = 0;
            btnBlue.Text = "远";
            btnBlue.UseVisualStyleBackColor = true;
            btnBlue.Click += btnBlue_Click;
            // 
            // btnRed
            // 
            btnRed.Location = new Point(36, 29);
            btnRed.Name = "btnRed";
            btnRed.Size = new Size(184, 171);
            btnRed.TabIndex = 0;
            btnRed.Text = "近";
            btnRed.UseVisualStyleBackColor = true;
            btnRed.Click += btnRed_Click;
            // 
            // tabPage2
            // 
            tabPage2.BackColor = SystemColors.ControlLightLight;
            tabPage2.Controls.Add(label6);
            tabPage2.Controls.Add(label3);
            tabPage2.Controls.Add(label5);
            tabPage2.Controls.Add(label2);
            tabPage2.Controls.Add(label4);
            tabPage2.Controls.Add(numSigmaB);
            tabPage2.Controls.Add(label1);
            tabPage2.Controls.Add(numSigmaG);
            tabPage2.Controls.Add(numScaleB);
            tabPage2.Controls.Add(numSigmaR);
            tabPage2.Controls.Add(numScaleG);
            tabPage2.Controls.Add(trbSigmaB);
            tabPage2.Controls.Add(numScaleR);
            tabPage2.Controls.Add(trbSigmaG);
            tabPage2.Controls.Add(trbScaleB);
            tabPage2.Controls.Add(trbSigmaR);
            tabPage2.Controls.Add(trbScaleG);
            tabPage2.Controls.Add(trbScaleR);
            tabPage2.Controls.Add(btnClose);
            tabPage2.Controls.Add(btnCustom);
            tabPage2.Location = new Point(4, 26);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(792, 395);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "高级设置";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label6.Location = new Point(421, 259);
            label6.Name = "label6";
            label6.Size = new Size(68, 21);
            label6.TabIndex = 2;
            label6.Text = "B强度：";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label3.Location = new Point(32, 259);
            label3.Name = "label3";
            label3.Size = new Size(68, 21);
            label3.TabIndex = 2;
            label3.Text = "B扩散：";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label5.Location = new Point(421, 161);
            label5.Name = "label5";
            label5.Size = new Size(70, 21);
            label5.TabIndex = 2;
            label5.Text = "G强度：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label2.Location = new Point(32, 161);
            label2.Name = "label2";
            label2.Size = new Size(70, 21);
            label2.TabIndex = 2;
            label2.Text = "G扩散：";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label4.Location = new Point(421, 63);
            label4.Name = "label4";
            label4.Size = new Size(68, 21);
            label4.TabIndex = 2;
            label4.Text = "R强度：";
            // 
            // numSigmaB
            // 
            numSigmaB.DecimalPlaces = 2;
            numSigmaB.Font = new Font("Microsoft YaHei UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 134);
            numSigmaB.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            numSigmaB.Location = new Point(495, 254);
            numSigmaB.Maximum = new decimal(new int[] { 2, 0, 0, 0 });
            numSigmaB.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            numSigmaB.Name = "numSigmaB";
            numSigmaB.Size = new Size(68, 32);
            numSigmaB.TabIndex = 1;
            numSigmaB.Value = new decimal(new int[] { 1, 0, 0, 131072 });
            numSigmaB.ValueChanged += numSigmaB_ValueChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label1.Location = new Point(32, 63);
            label1.Name = "label1";
            label1.Size = new Size(68, 21);
            label1.TabIndex = 2;
            label1.Text = "R扩散：";
            // 
            // numSigmaG
            // 
            numSigmaG.DecimalPlaces = 2;
            numSigmaG.Font = new Font("Microsoft YaHei UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 134);
            numSigmaG.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            numSigmaG.Location = new Point(495, 156);
            numSigmaG.Maximum = new decimal(new int[] { 2, 0, 0, 0 });
            numSigmaG.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            numSigmaG.Name = "numSigmaG";
            numSigmaG.Size = new Size(68, 32);
            numSigmaG.TabIndex = 1;
            numSigmaG.Value = new decimal(new int[] { 1, 0, 0, 131072 });
            numSigmaG.ValueChanged += numSigmaG_ValueChanged;
            // 
            // numScaleB
            // 
            numScaleB.DecimalPlaces = 1;
            numScaleB.Font = new Font("Microsoft YaHei UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 134);
            numScaleB.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            numScaleB.Location = new Point(106, 254);
            numScaleB.Maximum = new decimal(new int[] { 3, 0, 0, 0 });
            numScaleB.Name = "numScaleB";
            numScaleB.Size = new Size(68, 32);
            numScaleB.TabIndex = 1;
            numScaleB.ValueChanged += numScaleB_ValueChanged;
            numScaleB.Click += numScaleB_Click;
            // 
            // numSigmaR
            // 
            numSigmaR.DecimalPlaces = 2;
            numSigmaR.Font = new Font("Microsoft YaHei UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 134);
            numSigmaR.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            numSigmaR.Location = new Point(495, 58);
            numSigmaR.Maximum = new decimal(new int[] { 2, 0, 0, 0 });
            numSigmaR.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            numSigmaR.Name = "numSigmaR";
            numSigmaR.Size = new Size(68, 32);
            numSigmaR.TabIndex = 1;
            numSigmaR.Value = new decimal(new int[] { 1, 0, 0, 131072 });
            numSigmaR.ValueChanged += numSigmaR_ValueChanged;
            // 
            // numScaleG
            // 
            numScaleG.DecimalPlaces = 1;
            numScaleG.Font = new Font("Microsoft YaHei UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 134);
            numScaleG.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            numScaleG.Location = new Point(106, 156);
            numScaleG.Maximum = new decimal(new int[] { 3, 0, 0, 0 });
            numScaleG.Name = "numScaleG";
            numScaleG.Size = new Size(68, 32);
            numScaleG.TabIndex = 1;
            numScaleG.ValueChanged += numScaleG_ValueChanged;
            numScaleG.Click += numScaleG_Click;
            // 
            // trbSigmaB
            // 
            trbSigmaB.Location = new Point(569, 254);
            trbSigmaB.Maximum = 20;
            trbSigmaB.Minimum = 1;
            trbSigmaB.Name = "trbSigmaB";
            trbSigmaB.Size = new Size(195, 45);
            trbSigmaB.TabIndex = 0;
            trbSigmaB.Value = 3;
            trbSigmaB.Scroll += trbSigmaB_Scroll;
            // 
            // numScaleR
            // 
            numScaleR.DecimalPlaces = 1;
            numScaleR.Font = new Font("Microsoft YaHei UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 134);
            numScaleR.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            numScaleR.Location = new Point(106, 58);
            numScaleR.Maximum = new decimal(new int[] { 3, 0, 0, 0 });
            numScaleR.Name = "numScaleR";
            numScaleR.Size = new Size(68, 32);
            numScaleR.TabIndex = 1;
            numScaleR.ValueChanged += numScaleR_ValueChanged;
            numScaleR.Click += numScaleR_Click;
            // 
            // trbSigmaG
            // 
            trbSigmaG.Location = new Point(569, 156);
            trbSigmaG.Maximum = 20;
            trbSigmaG.Minimum = 1;
            trbSigmaG.Name = "trbSigmaG";
            trbSigmaG.Size = new Size(195, 45);
            trbSigmaG.TabIndex = 0;
            trbSigmaG.Value = 3;
            trbSigmaG.Scroll += trbSigmaG_Scroll;
            // 
            // trbScaleB
            // 
            trbScaleB.Location = new Point(180, 254);
            trbScaleB.Maximum = 30;
            trbScaleB.Name = "trbScaleB";
            trbScaleB.Size = new Size(195, 45);
            trbScaleB.TabIndex = 0;
            trbScaleB.Value = 5;
            trbScaleB.Scroll += trbScaleB_Scroll;
            // 
            // trbSigmaR
            // 
            trbSigmaR.Location = new Point(569, 58);
            trbSigmaR.Maximum = 20;
            trbSigmaR.Minimum = 1;
            trbSigmaR.Name = "trbSigmaR";
            trbSigmaR.Size = new Size(195, 45);
            trbSigmaR.TabIndex = 0;
            trbSigmaR.Value = 3;
            trbSigmaR.Scroll += trbSigmaR_Scroll;
            // 
            // trbScaleG
            // 
            trbScaleG.Location = new Point(180, 156);
            trbScaleG.Maximum = 30;
            trbScaleG.Name = "trbScaleG";
            trbScaleG.Size = new Size(195, 45);
            trbScaleG.TabIndex = 0;
            trbScaleG.Value = 5;
            trbScaleG.Scroll += trbScaleG_Scroll;
            // 
            // trbScaleR
            // 
            trbScaleR.Location = new Point(180, 58);
            trbScaleR.Maximum = 30;
            trbScaleR.Name = "trbScaleR";
            trbScaleR.Size = new Size(195, 45);
            trbScaleR.TabIndex = 0;
            trbScaleR.Value = 5;
            trbScaleR.Scroll += trbScaleR_Scroll;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(364, 352);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(184, 42);
            btnClose.TabIndex = 0;
            btnClose.Text = "关闭";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // btnCustom
            // 
            btnCustom.Location = new Point(569, 352);
            btnCustom.Name = "btnCustom";
            btnCustom.Size = new Size(184, 42);
            btnCustom.TabIndex = 0;
            btnCustom.Text = "启动";
            btnCustom.UseVisualStyleBackColor = true;
            btnCustom.Click += btnCustom_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, tscboFrom, tscboOutput, toolStripLabel2, tscboTo, tslDistance, tscboDistance, tslBlurCircles });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(800, 25);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(56, 22);
            toolStripLabel1.Text = "捕捉自：";
            // 
            // tscboFrom
            // 
            tscboFrom.DropDownWidth = 200;
            tscboFrom.Name = "tscboFrom";
            tscboFrom.Size = new Size(120, 25);
            tscboFrom.SelectedIndexChanged += tscboFrom_SelectedIndexChanged;
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(56, 22);
            toolStripLabel2.Text = "输出到：";
            // 
            // tscboTo
            // 
            tscboTo.DropDownWidth = 200;
            tscboTo.Name = "tscboTo";
            tscboTo.Size = new Size(120, 25);
            // 
            // tslDistance
            // 
            tslDistance.Name = "tslDistance";
            tslDistance.Size = new Size(60, 22);
            tslDistance.Text = "Distance:";
            // 
            // tscboDistance
            // 
            tscboDistance.Items.AddRange(new object[] { "2000", "1000", "500" });
            tscboDistance.Name = "tscboDistance";
            tscboDistance.Size = new Size(75, 25);
            tscboDistance.Text = "2000";
            tscboDistance.SelectedIndexChanged += tscboDistance_SelectedIndexChanged;
            tscboDistance.KeyDown += tscboDistance_KeyDown;
            // 
            // tslBlurCircles
            // 
            tslBlurCircles.Name = "tslBlurCircles";
            tslBlurCircles.Size = new Size(18, 22);
            tslBlurCircles.Text = "--";
            // 
            // notifyIcon1
            // 
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.ContextMenuStrip = cmsNotify;
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "离焦屏";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // cmsNotify
            // 
            cmsNotify.Items.AddRange(new ToolStripItem[] { tsmiClose, tsmiShowControlBox, toolStripMenuItem1, tsmiExit });
            cmsNotify.Name = "cmsNotify";
            cmsNotify.Size = new Size(177, 76);
            cmsNotify.Opening += cmsNotify_Opening;
            cmsNotify.DoubleClick += cmsNotify_DoubleClick;
            // 
            // tsmiClose
            // 
            tsmiClose.Name = "tsmiClose";
            tsmiClose.Size = new Size(176, 22);
            tsmiClose.Text = "关闭离焦屏输出(&C)";
            tsmiClose.Click += tsmiClose_Click;
            // 
            // tsmiShowControlBox
            // 
            tsmiShowControlBox.Name = "tsmiShowControlBox";
            tsmiShowControlBox.Size = new Size(176, 22);
            tsmiShowControlBox.Text = "显示控制盒(&N)";
            tsmiShowControlBox.Click += tsmiShowControlBox_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(173, 6);
            // 
            // tsmiExit
            // 
            tsmiExit.Name = "tsmiExit";
            tsmiExit.Size = new Size(176, 22);
            tsmiExit.Text = "退出(&X)";
            tsmiExit.Click += tsmiExit_Click;
            // 
            // tscboOutput
            // 
            tscboOutput.Name = "tscboOutput";
            tscboOutput.Size = new Size(121, 25);
            // 
            // ControlBox
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tabControl1);
            Controls.Add(toolStrip1);
            Name = "ControlBox";
            ShowInTaskbar = false;
            Text = "控制盒";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numSigmaB).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSigmaG).EndInit();
            ((System.ComponentModel.ISupportInitialize)numScaleB).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSigmaR).EndInit();
            ((System.ComponentModel.ISupportInitialize)numScaleG).EndInit();
            ((System.ComponentModel.ISupportInitialize)trbSigmaB).EndInit();
            ((System.ComponentModel.ISupportInitialize)numScaleR).EndInit();
            ((System.ComponentModel.ISupportInitialize)trbSigmaG).EndInit();
            ((System.ComponentModel.ISupportInitialize)trbScaleB).EndInit();
            ((System.ComponentModel.ISupportInitialize)trbSigmaR).EndInit();
            ((System.ComponentModel.ISupportInitialize)trbScaleG).EndInit();
            ((System.ComponentModel.ISupportInitialize)trbScaleR).EndInit();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            cmsNotify.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button btnBlue;
        private Button btnRed;
        private TrackBar trbScaleR;
        private NumericUpDown numScaleR;
        private Label label1;
        private Label label3;
        private Label label2;
        private NumericUpDown numScaleB;
        private NumericUpDown numScaleG;
        private TrackBar trbScaleB;
        private TrackBar trbScaleG;
        private Label label6;
        private Label label5;
        private Label label4;
        private NumericUpDown numSigmaB;
        private NumericUpDown numSigmaG;
        private NumericUpDown numSigmaR;
        private TrackBar trbSigmaB;
        private TrackBar trbSigmaG;
        private TrackBar trbSigmaR;
        private Button btnCustom;
        private Button btnClose;
        private ToolStrip toolStrip1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripComboBox tscboFrom;
        private ToolStripLabel toolStripLabel2;
        private ToolStripComboBox tscboTo;
        private NotifyIcon notifyIcon1;
        private ContextMenuStrip cmsNotify;
        private ToolStripMenuItem tsmiClose;
        private ToolStripMenuItem tsmiShowControlBox;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem tsmiExit;
        private ToolStripLabel tslDistance;
        private ToolStripComboBox tscboDistance;
        private ToolStripLabel tslBlurCircles;
        private ToolStripComboBox tscboOutput;
    }
}