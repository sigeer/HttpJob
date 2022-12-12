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
            var newlyVersion = await CheckUpdate.GetNewlyVersion();
            MessageBox.Show($"最新版本是{newlyVersion}");
        }
    }
}
