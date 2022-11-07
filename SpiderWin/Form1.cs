using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SpiderTool;
using SpiderTool.Dto.Spider;
using SpiderTool.Dto.Tasks;
using SpiderTool.IService;
using SpiderWin.Constants;
using SpiderWin.Modals;
using SpiderWin.Server;
using SpiderWin.Services;
using System.Diagnostics;
using System.Text;
using Utility.Extensions;

namespace SpiderWin
{
    public partial class Form1 : Form
    {
        ISpiderService _coreService;
        readonly ISpiderService localServiceBackup;
        readonly IServiceProvider _serviceProvider;
        readonly ILogger<Form1> _logger;

        List<SpiderListItemViewModel> _spiderList = new List<SpiderListItemViewModel>();
        List<TaskListItemViewModel> _taskList = new List<TaskListItemViewModel>();

        readonly CancellationTokenSource tokenSource = new CancellationTokenSource();
        public Form1(ISpiderService coreService , IServiceProvider serviceProvider, ILogger<Form1> logger)
        {
            _coreService = coreService;
            localServiceBackup = _coreService;
            _serviceProvider = serviceProvider;
            _logger = logger;

            InitializeComponent();
        }

        private void btnShowConfig_Click(object sender, EventArgs e)
        {
            var form = new SpiderConfigForm(_coreService, ComboxSpider.SelectedItem as SpiderListItemViewModel);
            form.OnSubmit += (obj, evt) =>
            {
                LoadSpiderList();
            };
            form.ShowDialog();
        }

        private void InitDataGrid(DataGridView grid)
        {
            grid.ReadOnly = true;
            grid.Columns.Add(nameof(TaskListItemViewModel.Status), "状态");
            grid.Columns.Add(nameof(TaskListItemViewModel.Id), nameof(TaskListItemViewModel.Id));
            grid.Columns.Add(nameof(TaskListItemViewModel.Description), "描述");
            grid.Columns.Add(nameof(TaskListItemViewModel.RootUrl), nameof(TaskListItemViewModel.RootUrl));
            grid.Columns.Add(nameof(TaskListItemViewModel.SpiderId), nameof(TaskListItemViewModel.SpiderId));
            grid.Columns.Add(nameof(TaskListItemViewModel.CreateTime), "创建时间");
            grid.Columns.Add(nameof(TaskListItemViewModel.CompleteTime), "完成时间");
        }

        private void PreLoadForm()
        {
            ComboxSpider.DisplayMember = nameof(SpiderListItemViewModel.Name);
            ComboxSpider.ValueMember = nameof(SpiderListItemViewModel.Id);


            ComboxUrl.DisplayMember = nameof(TaskListItemViewModel.RootUrl);
            ComboxUrl.ValueMember = nameof(TaskListItemViewModel.RootUrl);

            InitDataGrid(DataGrid_InProgressTasks);
            InitDataGrid(DataGrid_OtherTasks);
        }

        private void LoadForm()
        {
            LoadTaskList();
            LoadTaskHistory();
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
            ComboxSpider.DataSource = (new List<SpiderListItemViewModel>() { new SpiderListItemViewModel() { Id = 0, Name = "--请选择--" } }.Concat(_spiderList)).ToList();
        }

        private void LoadTaskHistory()
        {
            Task.Run(async () =>
            {
                var _taskHistoryList = await _coreService.GetTaskHistoryListAsync();
                BeginInvoke(() =>
                {
                    ComboxUrl.DataSource = _taskHistoryList;
                });
            });

        }

        private void LoadDataGridData(List<TaskListItemViewModel> list, DataGridView grid)
        {
            grid.Rows.Clear();
            list.ForEach(x =>
            {
                var row = new DataGridViewRow();

                var rowStyle = new DataGridViewCellStyle
                {
                    BackColor = ConstantsVariable.TaskColor[(TaskType)x.Status]
                };

                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.StatusName, Style = rowStyle });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.Id, ValueType = typeof(int), Style = rowStyle });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.Description, Style = rowStyle });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.RootUrl, Style = rowStyle });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.SpiderId, ValueType = typeof(int), Style = rowStyle });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"), Style = rowStyle });

                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.CompleteTime == null ? "-" : x.CompleteTime.Value.ToString("yyyy-MM-dd HH:mm:ss"), Style = rowStyle });

                grid.Rows.Add(row);
            });
        }

        private void LoadTaskList()
        {
            Task.Run(async () =>
            {
                _taskList = await _coreService.GetTaskListAsync();
                BeginInvoke(() =>
                {
                    LoadDataGridData(_taskList.Where(x => x.Status == (int)TaskType.InProgress || x.Status == (int)TaskType.NotEffective).ToList(), DataGrid_InProgressTasks);
                    LoadDataGridData(_taskList.Where(x => x.Status == (int)TaskType.Completed || x.Status == (int)TaskType.Canceled).ToList(), DataGrid_OtherTasks);
                });
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

            var currentTask = NewWorkTask(spiderId, ComboxUrl.Text);
            currentTask.Start();
        }

        private Task NewWorkTask(int spiderId, string url)
        {
            return new Task(() =>
            {
                var worker = new SpiderWorker(_serviceProvider.GetService<ILogger<SpiderWorker>>()!, spiderId, url, _coreService);
                Stopwatch childSW = new Stopwatch();

                worker.OnTaskInit += (obj, spider) =>
                {
                    childSW.Start();
                    PrintLog($"任务{spider.TaskId}开始==========", string.Empty);
                    LoadTaskHistory();
                };
                worker.OnTaskStart += (obj, spider) =>
                {
                    PrintLog($"任务{spider.TaskId}将保存到", $"\"file://{spider.CurrentDir}\"");
                };
                worker.OnTaskComplete += (obj, spider) =>
                {
                    childSW.Stop();

                    var cost = $"共耗时：{childSW.Elapsed.TotalSeconds.ToFixed(2)}秒";
                    mainModalStatusLabel.Text = $"任务{spider.TaskId}结束  {cost}";
                    PrintLog($"任务{spider.TaskId}结束==========", $"{cost} \"file:///{spider.CurrentDir}\"");
                };
                worker.OnTaskStatusChanged += (obj, task) =>
                {
                    LoadTaskList();
                };
                worker.OnNewTask += (obj, spider) =>
                {
                    PrintLog("创建了子任务", string.Empty);
                };
                worker.OnLog += (obj, logStr) =>
                {
                    PrintLog("日志", logStr);
                };

                BeginInvoke(new MethodInvoker(async () =>
                {
                    await worker.Start(tokenSource.Token);
                }));
            });
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
            UseServiceMenu.Checked = false;
            ServiceFactory.GrpcDisconnect();
            _coreService = localServiceBackup;
            LoadForm();
        }

        private void UseServiceMenu_Click(object sender, EventArgs e)
        {
            //使用服务器服务
            var setting = new ServerSetting();
            setting.OnChangeConnection += (obj, evt) =>
            {
                _coreService = evt;
                LoadForm();
                UseLocalMenu.Checked = false;
            };
            setting.ShowDialog();
        }

        private void ResultTxtBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.LinkText) && e.LinkText.StartsWith("file"))
                Process.Start("explorer.exe", e.LinkText);
        }

        private void DataGridTasks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var rows = ((DataGridView)sender).Rows;
            if (e.RowIndex >= 0 && e.RowIndex < rows.Count)
            {
                var selectedRow = rows[e.RowIndex];
                if (selectedRow != null)
                {
                    ComboxUrl.SelectedValue = selectedRow.Cells[3].Value;
                    ComboxSpider.SelectedValue = selectedRow.Cells[4].Value;
                }
            }
        }


        //private void DataGridTasks_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        if (e.RowIndex >= 0)
        //        {
        //            DataGridTasks.ClearSelection();
        //            DataGridTasks.Rows[e.RowIndex].Selected = true;
        //            DataGridTasks.CurrentCell = DataGridTasks.Rows[e.RowIndex].Cells[e.ColumnIndex];
        //            DataGridMenu.Show(MousePosition.X, MousePosition.Y);
        //        }
        //    }
        //}

        private void BtnCacel_Click(object sender, EventArgs e)
        {
            tokenSource.Cancel();
        }

        private void PrintLog(string type, string str)
        {
            BeginInvoke(() =>
            {
                var msg = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] >>{type}：{str} \r\n";
                _logger.LogInformation($"{type}：{str}");
                ResultTxtBox.AppendText(msg);
                ResultTxtBox.Focus();
            });
        }

        private void LinkClearLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ResultTxtBox.Clear();
        }

        private void MenuItem_Dir_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "download"));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var inWorkingTasks = _taskList.Where(x => x.IsWorking).Select(x => x.Id).ToList();
            if (inWorkingTasks.Count > 0)
            {
                var result = MessageBox.Show("尚有未完成的任务，是否取消任务并关闭？", "提示", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    _coreService.BulkUpdateTaskStatus(inWorkingTasks, (int)TaskType.Canceled);
                    Dispose();
                }
                else
                {
                    e.Cancel = true;
                }
            }

        }
    }
}