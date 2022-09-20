using SpiderTool;
using SpiderTool.Dto.Resource;
using SpiderTool.Dto.Spider;
using SpiderTool.IService;
using SpiderWin.Modals;
using System.Diagnostics;
using System.Web;
using Utility.Extensions;

namespace SpiderWin
{
    public partial class Form1 : Form
    {
        readonly ISpiderService _coreService;
        List<SpiderDtoSetter> _spiderList = new List<SpiderDtoSetter>();
        List<ResourceHistoryDto> _historyList = new List<ResourceHistoryDto>();
        Stopwatch _sw = new Stopwatch();

        public Form1(ISpiderService coreService)
        {
            _coreService = coreService;

            InitializeComponent();
            mainModalStatusLabel.Text = $"{AppDomain.CurrentDomain.BaseDirectory}";
        }

        private void btnShowConfig_Click(object sender, EventArgs e)
        {
            var form = new SpiderConfigForm(_coreService, dropConfig.SelectedItem as SpiderDtoSetter);
            form.ShowDialog();
        }

        private void PreLoadForm()
        {
            dropConfig.DisplayMember = nameof(SpiderDtoSetter.Name);
            dropConfig.ValueMember = nameof(SpiderDtoSetter.Id);

            listBoxUrl.DisplayMember = nameof(ResourceHistoryDto.Url);
            listBoxUrl.ValueMember = nameof(ResourceHistoryDto.Url);
        }

        private void LoadForm()
        {
            dropConfig.DataSource = (new List<SpiderDtoSetter>() { new SpiderDtoSetter() { Id = 0, Name = "" } }.Concat(_spiderList)).ToList();

            listBoxUrl.DataSource = _historyList;
        }

        private async void LoadData()
        {
            PreLoadForm();
            await Task.Run(() =>
            {
                _spiderList = _coreService.GetSpiderDtoList();
                _historyList = _coreService.GetResourceHistoryDtoList();
            });
            LoadForm();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUrl.Text) || dropConfig.SelectedValue == null || (int)dropConfig.SelectedValue == 0)
            {
                MessageBox.Show("请输入URL");
                return;
            }
            mainModalStatusLabel.Text = "运行中...";
            _sw.Restart();
            btnRun.Enabled = false;
            var spiderId = (int)dropConfig.SelectedValue;
            new Task(() =>
           {
               var worker = new SpiderWorker(_coreService);
               worker.TaskComplete += (obj, evt) =>
               {
                   btnRun.Enabled = true;
                   _sw.Stop();
                   mainModalStatusLabel.Text = $"共耗时：{_sw.Elapsed.TotalSeconds.ToFixed(2)}秒";
                   ResultTxtBox.Text += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] file:///{evt} \r\n";
               };

               BeginInvoke(new MethodInvoker(async () =>
               {
                   await worker.Start(txtUrl.Text, spiderId);
               }));
           }).Start();
        }

        private void txtUrl_KeyUp(object sender, KeyEventArgs e)
        {
            var eObj = (sender as TextBox)!;
            if (eObj.Name == nameof(txtUrl))
            {
                if (txtUrl.Text.Length > 0)
                    listBoxUrl.Visible = true;
                else
                    listBoxUrl.Visible = false;
            }
        }

        private void listBoxUrl_Click(object sender, EventArgs e)
        {
            ListBox eObj = (sender as ListBox)!;
            var info = eObj.SelectedItem as ResourceHistoryDto;
            if (info != null)
            {
                if (info.SpiderId != null)
                    dropConfig.SelectedValue = info.SpiderId;
                if (info.Url != null)
                {
                    txtUrl.Text = info.Url;
                    eObj.Visible = false;
                    txtUrl.Select(txtUrl.Text.Length, 1); //光标定位到最后
                }

            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            var eObj = (sender as Control)!;
            if (eObj.Name != nameof(txtUrl))
            {
                listBoxUrl.Visible = false;
            }
        }

        private void MenuNewSpider_Click(object sender, EventArgs e)
        {
            new SpiderConfigForm(_coreService).ShowDialog();
        }

        private void UseLocalMenu_Click(object sender, EventArgs e)
        {
            //使用本地服务
        }

        private void UseServiceMenu_Click(object sender, EventArgs e)
        {
            //使用服务器服务
        }
    }
}