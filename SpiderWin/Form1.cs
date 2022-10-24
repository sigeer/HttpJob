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
        ISpiderService localServiceBackup;

        List<SpiderListItemViewModel> _spiderList = new List<SpiderListItemViewModel>();
        List<TaskSimpleViewModel> _taskHistoryList = new List<TaskSimpleViewModel>();

        StringBuilder logSb = new StringBuilder();

        CancellationTokenSource tokenSource = new CancellationTokenSource();
        public Form1(ISpiderService coreService)
        {
            _coreService = coreService;
            localServiceBackup = _coreService;

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

        private void PreLoadForm()
        {
            ComboxSpider.DisplayMember = nameof(SpiderListItemViewModel.Name);
            ComboxSpider.ValueMember = nameof(SpiderListItemViewModel.Id);


            ComboxUrl.DisplayMember = nameof(TaskListItemViewModel.RootUrl);
            ComboxUrl.ValueMember = nameof(TaskListItemViewModel.RootUrl);

            DataGridTasks.ReadOnly = true;
            DataGridTasks.Columns.Add(nameof(TaskListItemViewModel.Status), "状态");
            DataGridTasks.Columns.Add(nameof(TaskListItemViewModel.Id), nameof(TaskListItemViewModel.Id));
            DataGridTasks.Columns.Add(nameof(TaskListItemViewModel.Description), "描述");
            DataGridTasks.Columns.Add(nameof(TaskListItemViewModel.RootUrl), nameof(TaskListItemViewModel.RootUrl));
            DataGridTasks.Columns.Add(nameof(TaskListItemViewModel.SpiderId), nameof(TaskListItemViewModel.SpiderId));
            DataGridTasks.Columns.Add(nameof(TaskListItemViewModel.CreateTime), "创建时间");
            DataGridTasks.Columns.Add(nameof(TaskListItemViewModel.CompleteTime), "完成时间");
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

        private async void LoadTaskHistory()
        {
            await Task.Run(async () =>
            {
                _taskHistoryList = await _coreService.GetTaskHistoryListAsync();
            });
            ComboxUrl.DataSource = _taskHistoryList;
        }

        private void LoadTaskList()
        {
            Task.Run(async () =>
            {
                var taskList = await _coreService.GetTaskListAsync();
                BeginInvoke(() =>
                {
                    DataGridTasks.Rows.Clear();
                    taskList.ForEach(x =>
                    {
                        var row = new DataGridViewRow();

                        var rowStyle = new DataGridViewCellStyle();
                        rowStyle.BackColor = ConstantsVariable.TaskColor[(TaskType)x.Status];

                        row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.StatusName, Style = rowStyle });
                        row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.Id, ValueType = typeof(int), Style = rowStyle });
                        row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.Description, Style = rowStyle });
                        row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.RootUrl, Style = rowStyle });
                        row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.SpiderId, ValueType = typeof(int), Style = rowStyle });
                        row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"), Style = rowStyle });

                        row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.CompleteTime == null ? "-" : x.CompleteTime.Value.ToString("yyyy-MM-dd HH:mm:ss"), Style = rowStyle });

                        DataGridTasks.Rows.Add(row);
                    });
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
                var worker = new SpiderWorker(spiderId, _coreService, url);
                Stopwatch childSW = new Stopwatch();

                worker.OnTaskInit += (obj, taskId) =>
                {
                    childSW.Start();
                    PrintLog($"任务{taskId}开始==========", string.Empty);
                    LoadTaskHistory();
                };
                worker.OnTaskStart += (obj, taskId) =>
                {
                    PrintLog($"任务{taskId}将保存到", $"\"file://{worker.CurrentDir}\"");
                };
                worker.OnTaskComplete += (obj, task) =>
                {
                    childSW.Stop();

                    var cost = $"共耗时：{childSW.Elapsed.TotalSeconds.ToFixed(2)}秒";
                    mainModalStatusLabel.Text = cost;
                    PrintLog($"任务{task.TaskId}结束==========", $"{cost} \"file:///{task.CurrentDir}\"");
                };
                worker.OnTaskStatusChanged += (obj, taskId) =>
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
            if (e.RowIndex >= 0 && e.RowIndex < DataGridTasks.Rows.Count - 1)
            {
                var selectedRow = DataGridTasks.Rows[e.RowIndex];
                if (selectedRow != null)
                {
                    ComboxUrl.SelectedValue = selectedRow.Cells[3].Value;
                    ComboxSpider.SelectedValue = selectedRow.Cells[4].Value;
                }
            }
        }

        private void LinkExportLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (logSb.Length == 0)
                return;

            var saveLogDialog = new SaveFileDialog()
            {
                Title = "导出日志",
                Filter = "*.txt|*.log",
                FileName = DateTime.Now.ToString("yyyy-MM-dd-HH"),
            };
            saveLogDialog.ShowDialog();
            using var fs = saveLogDialog.OpenFile();
            var txtBytes = Encoding.UTF8.GetBytes(logSb.ToString());
            fs.Write(txtBytes, 0, txtBytes.Length);
            logSb.Clear();
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
                var msg = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] >>{type}：{str} \r\n";
                logSb.Append(msg);
                ResultTxtBox.AppendText(msg);
                ResultTxtBox.Focus();
            });
        }

        private void LinkClearLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ResultTxtBox.Clear();
        }
    }
}