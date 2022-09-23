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
        List<TaskDto> _taskList = new List<TaskDto>();

        public Form1(ISpiderService coreService)
        {
            _coreService = coreService;

            InitializeComponent();
            mainModalStatusLabel.Text = $"file:\\\\{AppDomain.CurrentDomain.BaseDirectory}";
        }

        private void btnShowConfig_Click(object sender, EventArgs e)
        {
            var form = new SpiderConfigForm(_coreService, ComboxSpider.SelectedItem as SpiderDtoSetter);
            form.OnSubmit += (obj, evt) =>
            {
                LoadSpiderList();
            };
            form.ShowDialog();
        }

        private void PreLoadForm()
        {
            ComboxSpider.DisplayMember = nameof(SpiderDtoSetter.Name);
            ComboxSpider.ValueMember = nameof(SpiderDtoSetter.Id);


            ComboxUrl.DisplayMember = nameof(TaskDto.RootUrl);
            ComboxUrl.ValueMember = nameof(TaskDto.RootUrl);

            DataGridTasks.ReadOnly = true;
            DataGridTasks.Columns.Add(nameof(TaskDto.Id), nameof(TaskDto.Id));
            DataGridTasks.Columns.Add("描述", nameof(TaskDto.Description));
            DataGridTasks.Columns.Add(nameof(TaskDto.RootUrl), nameof(TaskDto.RootUrl));
            DataGridTasks.Columns.Add(nameof(TaskDto.SpiderId), nameof(TaskDto.SpiderId));
            DataGridTasks.Columns.Add(nameof(TaskDto.CreateTime), "创建时间");
            DataGridTasks.Columns.Add(nameof(TaskDto.Status), "状态");
            DataGridTasks.Columns.Add(nameof(TaskDto.CompleteTime), "完成时间");
        }

        private void LoadForm()
        {
            LoadTaskList();
            LoadSpiderList();
        }

        private void LoadData()
        {
            PreLoadForm();

            LoadForm();
        }

        private async void LoadSpiderList()
        {
            await Task.Run(() =>
            {
                _spiderList = _coreService.GetSpiderDtoList();
            });
            ComboxSpider.DataSource = (new List<SpiderDtoSetter>() { new SpiderDtoSetter() { Id = 0, Name = "--请选择--" } }.Concat(_spiderList)).ToList();
        }

        private async void LoadTaskList()
        {
            await Task.Run(() =>
            {
                _taskList = _coreService.GetTaskList();
            });
            ComboxUrl.DataSource = Enumerable.DistinctBy(_taskList, x => x.RootUrl).ToList();
            DataGridTasks.Rows.Clear();
            _taskList.ForEach(x =>
            {
                var row = new DataGridViewRow();
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.Id, ValueType = typeof(int) });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.Description });
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
            if (string.IsNullOrEmpty(ComboxUrl.Text) || ComboxSpider.SelectedValue == null || (int)ComboxSpider.SelectedValue == 0)
            {
                MessageBox.Show("请输入URL");
                return;
            }
            mainModalStatusLabel.Text = "运行中...";
            var spiderId = (int)ComboxSpider.SelectedValue;
            new Task(() =>
           {
               Stopwatch sw = new Stopwatch();
               var worker = new SpiderWorker(_coreService);
               worker.OnTaskStart += (obj, taskId) =>
               {
                   sw.Start();
                   ResultTxtBox.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] >>任务{taskId}开始==========\r\n");
               };
               worker.OnTaskComplete += (obj, taskId) =>
               {
                   sw.Stop();

                   var cost = $"共耗时：{sw.Elapsed.TotalSeconds.ToFixed(2)}秒";
                   mainModalStatusLabel.Text = cost;
                   ResultTxtBox.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] >>任务{taskId}结束=========={cost} file:///{worker.CurrentDir} \r\n");

                   MessageBox.Show($"任务完成，{cost}。");
               };
               worker.OnTaskStatusChanged += (obj, taskId) =>
               {
                   LoadTaskList();
               };

               BeginInvoke(new MethodInvoker(async () =>
               {
                   await worker.Start(ComboxUrl.Text, spiderId);
               }));
           }).Start();
        }

        private void MenuNewSpider_Click(object sender, EventArgs e)
        {
            var form = new SpiderConfigForm(_coreService);
            form.OnSubmit += (obj, evt) =>
            {
                LoadSpiderList();
            };
            form.ShowDialog();
        }

        private void UseLocalMenu_Click(object sender, EventArgs e)
        {
            //使用本地服务
        }

        private void UseServiceMenu_Click(object sender, EventArgs e)
        {
            //使用服务器服务
        }

        private void ResultTxtBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.LinkText))
                Process.Start("explorer.exe", e.LinkText);
        }

        private void DataGridTasks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < DataGridTasks.Rows.Count - 1)
            {
                var selectedRow = DataGridTasks.Rows[e.RowIndex];
                if (selectedRow != null)
                {
                    ComboxUrl.SelectedValue = selectedRow.Cells[2].Value;
                    ComboxSpider.SelectedValue = selectedRow.Cells[3].Value;
                }
            }
        }
    }
}