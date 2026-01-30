namespace DefocusMirror
{
    partial class frmArrow
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
            CursorImage = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)CursorImage).BeginInit();
            SuspendLayout();
            // 
            // CursorImage
            // 
            CursorImage.BackgroundImageLayout = ImageLayout.None;
            CursorImage.Location = new Point(0, 0);
            CursorImage.Name = "CursorImage";
            CursorImage.Size = new Size(36, 36);
            CursorImage.SizeMode = PictureBoxSizeMode.StretchImage;
            CursorImage.TabIndex = 0;
            CursorImage.TabStop = false;
            // 
            // frmArrow
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Magenta;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(64, 64);
            ControlBox = false;
            Controls.Add(CursorImage);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmArrow";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "frmArrow";
            TransparencyKey = Color.Magenta;
            ((System.ComponentModel.ISupportInitialize)CursorImage).EndInit();
            ResumeLayout(false);
        }

        #endregion

        public PictureBox CursorImage;
    }
}