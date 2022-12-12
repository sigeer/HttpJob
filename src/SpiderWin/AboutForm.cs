using SpiderWin.Services;
using System.Diagnostics;
using System.Reflection;

namespace SpiderWin
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            var nowAssembly = Assembly.GetEntryAssembly();
            if (nowAssembly != null)
            {
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(nowAssembly.Location);
                Label_Version.Text = fileVersionInfo.ProductVersion.ToString();
            }
        }

        private async void Link_Update_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var newlyVersion = await SystemUpdate.GetNewlyVersion();
            if (Label_Version.Text != newlyVersion)
            {
                var confirmResult = MessageBox.Show($"最新版本是{newlyVersion}，是否需要更新？", "更新", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    //1.下载安装包（压缩包到临时目录）
                    //2.调起更新脚本
                    //3.退出当前程序
                }
            }

        }
    }
}
