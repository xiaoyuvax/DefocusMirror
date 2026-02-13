namespace DefocusMirror
{
    partial class Mirror
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
            SuspendLayout();
            // 
            // Mirror
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 494);
            Name = "Mirror";
            ShowInTaskbar = false;
            Text = "离焦屏";
            FormClosed += Mirror_FormClosed;
            Load += Mirror_Load;
            Shown += Mirror_Shown;
            LocationChanged += Mirror_LocationChanged;
            MouseClick += Mirror_MouseClick;
            MouseDoubleClick += Mirror_MouseDoubleClick;
            Resize += Mirror_Resize;
            ResumeLayout(false);
        }

        #endregion
    }
}