using System.Threading;
using System.Windows.Forms;

namespace WinFormLoadingAnimation
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // 模擬載入時間 (非同步等待 5 秒)
            Thread.Sleep(5000);

            Label lbl = new Label { Text = "loading complete", AutoSize = true };
            this.Controls.Add(lbl);
        }
    }
}