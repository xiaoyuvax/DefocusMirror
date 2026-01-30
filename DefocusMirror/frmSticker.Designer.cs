namespace DefocusMirror
{
    partial class frmSticker
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
            label1 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Microsoft YaHei UI", 26.25F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(476, 180);
            label1.TabIndex = 0;
            label1.Text = "//./Display1";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // frmSticker
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Info;
            ClientSize = new Size(476, 180);
            ControlBox = false;
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "frmSticker";
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
    }
}