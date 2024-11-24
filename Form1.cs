using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormLoadingAnimation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnOpenForm2_Click(object sender, EventArgs e)
        {
            using (var formLoading = new FormLoading())
            {
                // 在背景執行緒初始化 Form2，完成後關閉動畫再顯示 Form2
                var loadForm2Task = Task.Run(() =>
                {
                    var form2 = new Form2();
                    formLoading.DialogResult = DialogResult.OK;
                    form2.ShowDialog();
                });

                // 顯示 loading form
                // 以Dialog顯示，鎖住其他的表單強迫使用者等待
                formLoading.ShowDialog();

                // 等待 Form2 完成初始化並顯示
                await loadForm2Task;
            }
        }
    }
}