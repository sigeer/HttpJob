using SpiderTool;
using SpiderTool.Dto.Resource;
using SpiderTool.Dto.Spider;
using SpiderTool.IService;
using SpiderWin.Modals;
using System.Diagnostics;
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
            mainModalStatusLabel.Text = $"file:\\\\{AppDomain.CurrentDomain.BaseDirectory}";
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


            ComboxUrl.DisplayMember = nameof(ResourceHistoryDto.Url);
            ComboxUrl.ValueMember = nameof(ResourceHistoryDto.Url);
        }

        private void LoadForm()
        {
            dropConfig.DataSource = (new List<SpiderDtoSetter>() { new SpiderDtoSetter() { Id = 0, Name = "" } }.Concat(_spiderList)).ToList();

            ComboxUrl.DataSource = _historyList;
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
            if (string.IsNullOrEmpty(ComboxUrl.Text) || dropConfig.SelectedValue == null || (int)dropConfig.SelectedValue == 0)
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
               worker.TaskComplete += async (obj, evt) =>
               {
                   btnRun.Enabled = true;
                   _sw.Stop();
                   mainModalStatusLabel.Text = $"共耗时：{_sw.Elapsed.TotalSeconds.ToFixed(2)}秒";
                   ResultTxtBox.Text += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] file:///{evt} \r\n";

                   await Task.Run(() =>
                   {
                       _historyList = _coreService.GetResourceHistoryDtoList();
                   });
                   ComboxUrl.DataSource = _historyList;
               };

               BeginInvoke(new MethodInvoker(async () =>
               {
                   await worker.Start(ComboxUrl.Text, spiderId);
               }));
           }).Start();
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