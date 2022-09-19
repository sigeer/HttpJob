using SpiderTool;
using SpiderTool.Dto.Resource;
using SpiderTool.Dto.Spider;
using SpiderTool.IService;
using SpiderWin.Modals;
using System.Diagnostics;

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
            var selectedConfigId = (int?)(dropConfig.SelectedValue);
            var form = new SpiderConfigForm(selectedConfigId ?? 0, _coreService);
            form.ShowDialog();
        }

        private void BindDropdown()
        {
            dropConfig.DisplayMember = nameof(SpiderDtoSetter.Name);
            dropConfig.ValueMember = nameof(SpiderDtoSetter.Id);
            dropConfig.DataSource = _spiderList;

            listBoxUrl.DataSource = _historyList;
            listBoxUrl.DisplayMember = nameof(ResourceHistoryDto.Url) + " " + nameof(ResourceHistoryDto.Name);
            listBoxUrl.ValueMember = nameof(ResourceHistoryDto.Url);
        }

        private async void LoadData()
        {
            await Task.Run(() =>
            {
                _spiderList = _coreService.GetSpiderDtoList();
                _historyList = _coreService.GetResourceHistoryDtoList();
            });
            BindDropdown();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private async void btnRun_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUrl.Text))
            {
                MessageBox.Show("请输入URL");
                return;
            }
            mainModalStatusLabel.Text = "运行中...";
            _sw.Restart();
            btnRun.Enabled = false;
            var spiderId = (dropConfig.SelectedItem as SpiderDtoSetter)!.Id;
            await Task.Run(async () =>
            {
                var worker = new SpiderWorker(_coreService);
                await worker.Start(txtUrl.Text, spiderId);
            });
            btnRun.Enabled = true;
            _sw.Stop();
            mainModalStatusLabel.Text = $"共耗时：{_sw.Elapsed.TotalSeconds}秒 {AppDomain.CurrentDomain.BaseDirectory}";
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
    }
}