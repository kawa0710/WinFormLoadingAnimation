using System.Windows.Forms;

namespace WinFormLoadingAnimation
{
    public partial class FormLoading : Form
    {
        public FormLoading()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None;
        }
    }
}
