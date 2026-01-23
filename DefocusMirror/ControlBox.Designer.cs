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
            pnlGraph = new Panel();
            groupBox1 = new GroupBox();
            panel1 = new Panel();
            label12 = new Label();
            label11 = new Label();
            label10 = new Label();
            label8 = new Label();
            label7 = new Label();
            label9 = new Label();
            btnClose0 = new Button();
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
            tscboOutput = new ToolStripComboBox();
            toolStripLabel2 = new ToolStripLabel();
            tscboTo = new ToolStripComboBox();
            toolStripLabel3 = new ToolStripLabel();
            tscboFrameRate = new ToolStripComboBox();
            tscboSize = new ToolStripComboBox();
            tsbFullScreen = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            notifyIcon1 = new NotifyIcon(components);
            cmsNotify = new ContextMenuStrip(components);
            tsmiClose = new ToolStripMenuItem();
            tsmiShowControlBox = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            tsmiExit = new ToolStripMenuItem();
            toolTip1 = new ToolTip(components);
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
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
            tabControl1.Size = new Size(976, 425);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = SystemColors.Control;
            tabPage1.Controls.Add(pnlGraph);
            tabPage1.Controls.Add(groupBox1);
            tabPage1.Controls.Add(btnClose0);
            tabPage1.Controls.Add(btnBlue);
            tabPage1.Controls.Add(btnRed);
            tabPage1.Location = new Point(4, 26);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(968, 395);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "快速启动";
            // 
            // pnlGraph
            // 
            pnlGraph.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlGraph.AutoScroll = true;
            pnlGraph.BackColor = Color.LightYellow;
            pnlGraph.Location = new Point(0, 0);
            pnlGraph.Name = "pnlGraph";
            pnlGraph.Size = new Size(705, 392);
            pnlGraph.TabIndex = 3;
            pnlGraph.Resize += pnlGraph_Resize;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox1.Controls.Add(panel1);
            groupBox1.Location = new Point(714, 269);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(237, 118);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "参考像素";
            // 
            // panel1
            // 
            panel1.BackColor = Color.Black;
            panel1.Controls.Add(label12);
            panel1.Controls.Add(label11);
            panel1.Controls.Add(label10);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(label9);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 19);
            panel1.Name = "panel1";
            panel1.Size = new Size(231, 96);
            panel1.TabIndex = 5;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold);
            label12.ForeColor = Color.Blue;
            label12.Location = new Point(162, 27);
            label12.Name = "label12";
            label12.Size = new Size(49, 22);
            label12.TabIndex = 5;
            label12.Text = "蓝(B)";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold);
            label11.ForeColor = Color.Lime;
            label11.Location = new Point(92, 27);
            label11.Name = "label11";
            label11.Size = new Size(50, 22);
            label11.TabIndex = 5;
            label11.Text = "绿(G)";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold);
            label10.ForeColor = Color.Red;
            label10.Location = new Point(25, 27);
            label10.Name = "label10";
            label10.Size = new Size(49, 22);
            label10.TabIndex = 5;
            label10.Text = "红(R)";
            // 
            // label8
            // 
            label8.BackColor = Color.Lime;
            label8.Location = new Point(111, 67);
            label8.Name = "label8";
            label8.Size = new Size(1, 1);
            label8.TabIndex = 3;
            // 
            // label7
            // 
            label7.BackColor = Color.Red;
            label7.Location = new Point(43, 67);
            label7.Name = "label7";
            label7.Size = new Size(1, 1);
            label7.TabIndex = 2;
            // 
            // label9
            // 
            label9.BackColor = Color.Blue;
            label9.Location = new Point(179, 67);
            label9.Name = "label9";
            label9.Size = new Size(1, 1);
            label9.TabIndex = 4;
            // 
            // btnClose0
            // 
            btnClose0.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose0.Location = new Point(714, 213);
            btnClose0.Name = "btnClose0";
            btnClose0.Size = new Size(234, 50);
            btnClose0.TabIndex = 0;
            btnClose0.Text = "关闭离焦屏";
            toolTip1.SetToolTip(btnClose0, "关闭离焦屏输出，鼠标双击离焦屏或在离焦屏上按下CTL+ALT+Q亦可达到相同效果。注意：如果要退出程序，请点右下角托盘图标，鼠标右键菜单中选择“退出”。");
            btnClose0.UseVisualStyleBackColor = true;
            btnClose0.Click += btnClose_Click;
            // 
            // btnBlue
            // 
            btnBlue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBlue.Location = new Point(717, 83);
            btnBlue.Name = "btnBlue";
            btnBlue.Size = new Size(234, 50);
            btnBlue.TabIndex = 0;
            btnBlue.Text = "远视离焦/(R15:G3:B1px)";
            toolTip1.SetToolTip(btnBlue, "启动后离焦屏上将显示一个带m字符的镜像鼠标用于模拟操作离焦屏，请注意与真鼠标指针的差别，不要搞混了。");
            btnBlue.UseVisualStyleBackColor = true;
            btnBlue.Click += btnBlue_Click;
            // 
            // btnRed
            // 
            btnRed.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRed.Location = new Point(717, 17);
            btnRed.Name = "btnRed";
            btnRed.Size = new Size(234, 50);
            btnRed.TabIndex = 0;
            btnRed.Text = "近视离焦/(R1:G3:B15px)";
            toolTip1.SetToolTip(btnRed, "启动后离焦屏上将显示一个带m字符的镜像鼠标用于模拟操作离焦屏，请注意与真鼠标指针的差别，不要搞混了。");
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
            tabPage2.Size = new Size(968, 395);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "高级设置";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label6.Location = new Point(520, 259);
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
            label5.Location = new Point(520, 161);
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
            label4.Location = new Point(520, 63);
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
            numSigmaB.Location = new Point(594, 254);
            numSigmaB.Maximum = new decimal(new int[] { 2, 0, 0, 0 });
            numSigmaB.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            numSigmaB.Name = "numSigmaB";
            numSigmaB.Size = new Size(68, 32);
            numSigmaB.TabIndex = 1;
            numSigmaB.Value = new decimal(new int[] { 1, 0, 0, 0 });
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
            numSigmaG.Location = new Point(594, 156);
            numSigmaG.Maximum = new decimal(new int[] { 2, 0, 0, 0 });
            numSigmaG.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            numSigmaG.Name = "numSigmaG";
            numSigmaG.Size = new Size(68, 32);
            numSigmaG.TabIndex = 1;
            numSigmaG.Value = new decimal(new int[] { 1, 0, 0, 0 });
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
            numScaleB.Value = new decimal(new int[] { 3, 0, 0, 0 });
            numScaleB.ValueChanged += numScaleB_ValueChanged;
            numScaleB.Click += numScaleB_Click;
            // 
            // numSigmaR
            // 
            numSigmaR.DecimalPlaces = 2;
            numSigmaR.Font = new Font("Microsoft YaHei UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 134);
            numSigmaR.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            numSigmaR.Location = new Point(594, 58);
            numSigmaR.Maximum = new decimal(new int[] { 2, 0, 0, 0 });
            numSigmaR.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            numSigmaR.Name = "numSigmaR";
            numSigmaR.Size = new Size(68, 32);
            numSigmaR.TabIndex = 1;
            numSigmaR.Value = new decimal(new int[] { 1, 0, 0, 0 });
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
            numScaleG.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numScaleG.ValueChanged += numScaleG_ValueChanged;
            numScaleG.Click += numScaleG_Click;
            // 
            // trbSigmaB
            // 
            trbSigmaB.Location = new Point(668, 254);
            trbSigmaB.Maximum = 20;
            trbSigmaB.Minimum = 1;
            trbSigmaB.Name = "trbSigmaB";
            trbSigmaB.Size = new Size(277, 45);
            trbSigmaB.TabIndex = 0;
            trbSigmaB.Value = 10;
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
            trbSigmaG.Location = new Point(668, 156);
            trbSigmaG.Maximum = 20;
            trbSigmaG.Minimum = 1;
            trbSigmaG.Name = "trbSigmaG";
            trbSigmaG.Size = new Size(277, 45);
            trbSigmaG.TabIndex = 0;
            trbSigmaG.Value = 10;
            trbSigmaG.Scroll += trbSigmaG_Scroll;
            // 
            // trbScaleB
            // 
            trbScaleB.Location = new Point(180, 254);
            trbScaleB.Maximum = 30;
            trbScaleB.Name = "trbScaleB";
            trbScaleB.Size = new Size(285, 45);
            trbScaleB.TabIndex = 0;
            trbScaleB.Value = 30;
            trbScaleB.Scroll += trbScaleB_Scroll;
            // 
            // trbSigmaR
            // 
            trbSigmaR.Location = new Point(668, 58);
            trbSigmaR.Maximum = 20;
            trbSigmaR.Minimum = 1;
            trbSigmaR.Name = "trbSigmaR";
            trbSigmaR.Size = new Size(277, 45);
            trbSigmaR.TabIndex = 0;
            trbSigmaR.Value = 10;
            trbSigmaR.Scroll += trbSigmaR_Scroll;
            // 
            // trbScaleG
            // 
            trbScaleG.Location = new Point(180, 156);
            trbScaleG.Maximum = 30;
            trbScaleG.Name = "trbScaleG";
            trbScaleG.Size = new Size(285, 45);
            trbScaleG.TabIndex = 0;
            trbScaleG.Value = 10;
            trbScaleG.Scroll += trbScaleG_Scroll;
            // 
            // trbScaleR
            // 
            trbScaleR.Location = new Point(180, 58);
            trbScaleR.Maximum = 30;
            trbScaleR.Name = "trbScaleR";
            trbScaleR.Size = new Size(285, 45);
            trbScaleR.TabIndex = 0;
            trbScaleR.Scroll += trbScaleR_Scroll;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(558, 328);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(184, 42);
            btnClose.TabIndex = 0;
            btnClose.Text = "关闭";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // btnCustom
            // 
            btnCustom.Location = new Point(761, 328);
            btnCustom.Name = "btnCustom";
            btnCustom.Size = new Size(184, 42);
            btnCustom.TabIndex = 0;
            btnCustom.Text = "启动";
            btnCustom.UseVisualStyleBackColor = true;
            btnCustom.Click += btnCustom_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, tscboFrom, tscboOutput, toolStripLabel2, tscboTo, toolStripLabel3, tscboFrameRate, tscboSize, tsbFullScreen, toolStripSeparator1 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(976, 25);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(56, 22);
            toolStripLabel1.Text = "捕捉自：";
            toolStripLabel1.ToolTipText = "选择需要捕捉画面的适配器和屏幕输出";
            // 
            // tscboFrom
            // 
            tscboFrom.DropDownWidth = 200;
            tscboFrom.Name = "tscboFrom";
            tscboFrom.Size = new Size(120, 25);
            tscboFrom.ToolTipText = "需要捕捉画面的适配器";
            tscboFrom.SelectedIndexChanged += tscboFrom_SelectedIndexChanged;
            // 
            // tscboOutput
            // 
            tscboOutput.Name = "tscboOutput";
            tscboOutput.Size = new Size(121, 25);
            tscboOutput.ToolTipText = "需要捕捉画面的适配器上可选的屏幕";
            tscboOutput.SelectedIndexChanged += tscboOutput_SelectedIndexChanged;
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(56, 22);
            toolStripLabel2.Text = "输出到：";
            toolStripLabel2.ToolTipText = "选择需要输出离焦画面的屏幕";
            // 
            // tscboTo
            // 
            tscboTo.DropDownWidth = 200;
            tscboTo.Name = "tscboTo";
            tscboTo.Size = new Size(120, 25);
            tscboTo.ToolTipText = "用于显示离焦屏画面的屏幕。如果强制选择跟源屏幕相同，那么不会有输出。";
            tscboTo.SelectedIndexChanged += tscboTo_SelectedIndexChanged;
            // 
            // toolStripLabel3
            // 
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new Size(44, 22);
            toolStripLabel3.Text = "帧率：";
            toolStripLabel3.ToolTipText = "离焦屏画面输出的帧率 - 如果显卡性能不佳可以设低一点，但一般30帧就好了，看视频或游戏可以设高点。";
            // 
            // tscboFrameRate
            // 
            tscboFrameRate.Items.AddRange(new object[] { "30", "50", "60", "25" });
            tscboFrameRate.Name = "tscboFrameRate";
            tscboFrameRate.Size = new Size(75, 25);
            tscboFrameRate.Text = "30";
            tscboFrameRate.ToolTipText = "离焦屏画面输出的帧率 - 如果显卡性能不佳可以设低一点，但一般30帧就好了，看视频或游戏可以设高点。";
            // 
            // tscboSize
            // 
            tscboSize.Alignment = ToolStripItemAlignment.Right;
            tscboSize.DropDownWidth = 100;
            tscboSize.Items.AddRange(new object[] { "1920x1080", "1680x1050", "1440x900", "1280x1024", "1280x960", "1280x720", "1024x768", "800x600" });
            tscboSize.Name = "tscboSize";
            tscboSize.Size = new Size(80, 25);
            tscboSize.ToolTipText = "窗口模式下离焦屏的初始尺寸。用户后续可以自由调整窗口尺寸。";
            tscboSize.Visible = false;
            // 
            // tsbFullScreen
            // 
            tsbFullScreen.Alignment = ToolStripItemAlignment.Right;
            tsbFullScreen.Checked = true;
            tsbFullScreen.CheckOnClick = true;
            tsbFullScreen.CheckState = CheckState.Checked;
            tsbFullScreen.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsbFullScreen.Image = (Image)resources.GetObject("tsbFullScreen.Image");
            tsbFullScreen.ImageTransparentColor = Color.Magenta;
            tsbFullScreen.Name = "tsbFullScreen";
            tsbFullScreen.Size = new Size(60, 22);
            tsbFullScreen.Text = "全屏模式";
            tsbFullScreen.ToolTipText = "全屏/窗体模式，适用于两个屏幕解析度不一样但又想保持输出画面纵横比的情况，或者用户想自己控制离焦屏尺寸或比例。";
            tsbFullScreen.CheckedChanged += tsbFullScreen_CheckedChanged;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Alignment = ToolStripItemAlignment.Right;
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
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
            // ControlBox
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(976, 450);
            Controls.Add(tabControl1);
            Controls.Add(toolStrip1);
            MinimizeBox = false;
            Name = "ControlBox";
            ShowInTaskbar = false;
            Text = "离焦屏 - 控制盒";
            Activated += ControlBox_Activated;
            FormClosing += ControlBox_FormClosing;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
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
        private ToolStripComboBox tscboOutput;
        private Button btnClose0;
        private ToolStripLabel toolStripLabel3;
        private ToolStripComboBox tscboFrameRate;
        private GroupBox groupBox1;
        private Label label9;
        private Label label8;
        private Label label7;
        private Panel panel1;
        private Label label12;
        private Label label11;
        private Label label10;
        private ToolStripButton tsbFullScreen;
        private ToolStripSeparator toolStripSeparator1;
        private Panel pnlGraph;
        private ToolStripComboBox tscboSize;
        private ToolTip toolTip1;
    }
}