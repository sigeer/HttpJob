using SpiderTool;
using SpiderTool.Dto.Resource;
using SpiderTool.Dto.Spider;
using SpiderTool.IService;
using SpiderTool.Tasks;
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
        List<TaskDto> _taskList = new List<TaskDto>();


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


            ComboxUrl.DisplayMember = nameof(TaskDto.RootUrl);
            ComboxUrl.ValueMember = nameof(TaskDto.RootUrl);
        }

        private void LoadForm()
        {
            dropConfig.DataSource = (new List<SpiderDtoSetter>() { new SpiderDtoSetter() { Id = 0, Name = "" } }.Concat(_spiderList)).ToList();

            ComboxUrl.DataSource = Enumerable.DistinctBy(_taskList, x => x.RootUrl).ToList();

            DataGridTasks.ReadOnly = true;
            DataGridTasks.Columns.Add(nameof(TaskDto.Id), nameof(TaskDto.Id));
            DataGridTasks.Columns.Add(nameof(TaskDto.RootUrl), nameof(TaskDto.RootUrl));
            DataGridTasks.Columns.Add(nameof(TaskDto.SpiderId), nameof(TaskDto.SpiderId));
            DataGridTasks.Columns.Add(nameof(TaskDto.CreateTime), "创建时间");
            DataGridTasks.Columns.Add(nameof(TaskDto.Status), "状态");
            DataGridTasks.Columns.Add(nameof(TaskDto.CompleteTime), "完成时间");
            LoadDataGridTask();
        }

        private async void LoadData()
        {
            PreLoadForm();
            await Task.Run(() =>
            {
                _spiderList = _coreService.GetSpiderDtoList();
                _taskList = _coreService.GetTaskList();
            });
            LoadForm();
        }

        private void LoadDataGridTask()
        {
            DataGridTasks.Rows.Clear();
            _taskList.ForEach(x =>
            {
                var row = new DataGridViewRow();
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.Id, ValueType = typeof(int) });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.RootUrl });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.SpiderId, ValueType = typeof(int) });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.CreateTime.ToString("yyyy-MM-dd HH:mm:ss") });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.StatusName });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.CompleteTime == null ? "-" : x.CompleteTime.Value.ToString("yyyy-MM-dd HH:mm:ss") });
                DataGridTasks.Rows.Add(row);
            });
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
            var spiderId = (int)dropConfig.SelectedValue;
            new Task(() =>
           {
               Stopwatch sw = new Stopwatch();
               var worker = new SpiderWorker(_coreService);
               worker.TaskComplete += (obj, evt) =>
               {
                   btnRun.Enabled = true;
                   sw.Stop();
                   mainModalStatusLabel.Text = $"共耗时：{sw.Elapsed.TotalSeconds.ToFixed(2)}秒";
                   ResultTxtBox.Text += $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] file:///{evt} \r\n";
               };
               worker.OnLog += async (obj, evt) =>
               {
                   await Task.Run(() =>
                   {
                       _taskList = _coreService.GetTaskList();
                   });
                   LoadDataGridTask();
               };

               BeginInvoke(new MethodInvoker(async () =>
               {
                   sw.Restart();
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