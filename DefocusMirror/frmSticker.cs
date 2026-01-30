namespace DefocusMirror
{
  

    public partial class frmSticker : Form
    {
        public frmSticker()
        {
            InitializeComponent();
        }

        public string LabelText
        {
            get => label1.Text;
            set => label1.Text = value;
        }
    }
}
