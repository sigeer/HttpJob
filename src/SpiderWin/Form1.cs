using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SpiderTool;
using SpiderTool.Data.Dto.Spider;
using SpiderTool.Data.Dto.Tasks;
using SpiderTool.Data.IService;
using SpiderWin.Constants;
using SpiderWin.Modals;
using SpiderWin.Server;
using SpiderWin.Services;
using System.Diagnostics;
using Utility.Common;
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
        SpiderWokerCollection _spiders = new SpiderWokerCollection();

        readonly DelayedTaskPool _delayedTaskPool = DelayedTaskPool.GetInstance();
        public Form1(ISpiderService coreService, IServiceProvider serviceProvider, WinFormLogSink logSink, ILogger<Form1> logger)
        {
            logSink.LogReceived += (obj, evt) => PrintLog(evt);
            _logger = logger;

            _coreService = coreService;
            localServiceBackup = _coreService;
            _serviceProvider = serviceProvider;

            _delayedTaskPool.ChangeDelayedDuration(100);
            Configs.InitBaseDir();

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

            InitDataGrid(DataGrid_InProgressTasks);
            InitDataGrid(DataGrid_OtherTasks);
        }

        private void LoadForm()
        {
            LoadTaskList(true);
            LoadSpiderList();
        }

        private async void LoadSpiderList()
        {
            await Task.Run(() =>
            {
                _spiderList = _coreService.GetSpiderDtoList();
            });
            ComboxSpider.DataSource = (new List<SpiderListItemViewModel>() { new SpiderListItemViewModel() { Id = 0, Name = "--请选择--" } }.Concat(_spiderList)).ToList();
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
                row.ContextMenuStrip = DataGridMenu;
                grid.Rows.Add(row);
            });
        }

        private void LoadTaskList(bool refreshUrlDropdown = true)
        {
            Task.Run(async () =>
            {
                _taskList = await _coreService.GetTaskListAsync();
                BeginInvoke(() =>
                {
                    if (refreshUrlDropdown)
                        ComboxUrl.DataSource = _taskList.Select(x => x.RootUrl).Distinct().ToList();

                    LoadDataGridData(_taskList.Where(x => x.Status == (int)TaskType.InProgress || x.Status == (int)TaskType.NotEffective).ToList(), DataGrid_InProgressTasks);
                    LoadDataGridData(_taskList.Where(x => x.Status == (int)TaskType.Completed || x.Status == (int)TaskType.Canceled).ToList(), DataGrid_OtherTasks);
                });
            });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _logger.LogInformation("窗口已启动");
            PreLoadForm();

            LoadForm();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ComboxUrl.Text) || ComboxSpider.SelectedValue == null || (int)ComboxSpider.SelectedValue == 0)
            {
                MessageBox.Show("请输入URL");
                return;
            }


            var spiderId = (int)ComboxSpider.SelectedValue;
            if (_taskList.Any(x => x.IsWorking && x.RootUrl == ComboxUrl.Text && x.SpiderId == spiderId))
            {
                MessageBox.Show("已有一个相同的任务在运行。");
                return;
            }

            if (_taskList.Any(x => !x.IsWorking && x.RootUrl == ComboxUrl.Text && x.SpiderId == spiderId))
            {
                var isConfirmed = MessageBox.Show("已存在一个相同的任务，是否继续？", "提示", MessageBoxButtons.YesNo);
                if (isConfirmed == DialogResult.Yes)
                {
                    var currentTask = NewWorkTask(spiderId, ComboxUrl.Text);
                    mainModalStatusLabel.Text = "运行中...";
                    currentTask.Start();
                }
            }
            else
            {
                var currentTask = NewWorkTask(spiderId, ComboxUrl.Text);
                mainModalStatusLabel.Text = "运行中...";
                currentTask.Start();
            }

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

                    _logger.LogInformation($"任务 {spider.TaskId} 正在初始化==========");
                };
                worker.OnTaskStart += (obj, spider) =>
                {
                    _logger.LogInformation($"任务 {spider.TaskId} 开始==========");
                };
                worker.OnTaskComplete += (obj, spider) =>
                {
                    childSW.Stop();

                    var cost = $"耗时：{childSW.Elapsed.TotalSeconds.ToFixed(2)}秒";
                    _logger.LogInformation($"任务 {spider.TaskId} 结束========= {cost} 保存路径：file://{spider.CurrentDir}");
                };
                worker.OnTaskStatusChanged += (obj, task) =>
                {
                    _delayedTaskPool.AddTask(nameof(LoadTaskList), () => LoadTaskList(task.Status == TaskType.Completed));
                };
                worker.OnNewTask += (obj, spider) =>
                {
                    _logger.LogInformation($"创建子任务: {spider.TaskId}");
                };
                _spiders.Add(worker);

                BeginInvoke(new MethodInvoker(async () =>
                {
                    await worker.Start();
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
            UseLocalMenu.Checked = true;
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
                UseServiceMenu.Checked = true;
            };
            setting.ShowDialog();
        }

        private void ResultTxtBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.LinkText) && e.LinkText.StartsWith("file"))
                OpenDir(e.LinkText);
        }

        private void DataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var rows = ((DataGridView)sender).Rows;
            if (e.RowIndex >= 0 && e.RowIndex < rows.Count)
            {
                var selectedRow = rows[e.RowIndex];
                SelectTask(selectedRow);
            }
        }

        private void SelectTask(DataGridViewRow? selectedRow)
        {
            if (selectedRow != null && selectedRow.Index >= 0 && !selectedRow.IsNewRow)
            {
                ComboxUrl.Text = selectedRow.Cells[3].Value.ToString();
                ComboxSpider.SelectedValue = selectedRow.Cells[4].Value;
            }
        }

        private void RemoveTask(DataGridViewRow? selectedRow)
        {
            if (selectedRow != null && selectedRow.Index >= 0 && !selectedRow.IsNewRow)
            {
                var taskId = (int)selectedRow.Cells[1].Value;
                _coreService.RemoveTask(taskId);
                LoadTaskList();
            }
        }

        private void BtnCacel_Click(object sender, EventArgs e)
        {
            _coreService.StopAllTask();
        }

        private void PrintLog(string str)
        {
            if (IsDisposed)
                return;

            if (!string.IsNullOrWhiteSpace(str))
            {
                if (InvokeRequired)
                {
                    Invoke(() =>
                    {
                        ResultTxtBox.AppendText(str);
                        ResultTxtBox.Focus();
                    });
                }
                else
                {

                    ResultTxtBox.AppendText(str);
                    ResultTxtBox.Focus();
                }
            }
        }

        private void LinkClearLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ResultTxtBox.Clear();
        }

        private void MenuItem_Dir_Click(object sender, EventArgs e)
        {
            OpenDir(Configs.BaseDir);
        }

        private void OpenDir(string dir)
        {
            if (!Directory.Exists(dir))
                dir = Configs.BaseDir;

            Process.Start("explorer.exe", dir);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                HideModalFromNormal();
            }
            else
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

        private void MenuItem_LogDir_Click(object sender, EventArgs e)
        {
            OpenDir(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs"));
        }

        private void MenuItem_UseTask_Click(object sender, EventArgs e)
        {
            var grid = tabControl1.TabIndex == 0 ? DataGrid_InProgressTasks : DataGrid_OtherTasks;
            SelectTask(grid.SelectedRows[0]);

        }

        private void MenuItem_OpenSaveDir_Click(object sender, EventArgs e)
        {
            var grid = tabControl1.SelectedIndex == 0 ? DataGrid_InProgressTasks : DataGrid_OtherTasks;
            var row = grid.SelectedRows[0];
            if (row == null)
                return;

            if (row.Index >= 0 && !row.IsNewRow)
            {
                var taskId = (int?)row.Cells[1].Value;
                var dir = Configs.BaseDir;

                var worker = _spiders.FirstOrDefault(x => x.TaskId == taskId);
                if (worker != null)
                    dir = worker.CurrentDir;

                _logger.LogDebug($"尝试打开{dir}");
                OpenDir(dir);
            }
        }

        private void MenuItem_Remove_Click(object sender, EventArgs e)
        {
            var grid = tabControl1.SelectedIndex == 0 ? DataGrid_InProgressTasks : DataGrid_OtherTasks;
            var row = grid.SelectedRows[0];
            if (row == null)
                return;

            RemoveTask(row);
        }

        private void MenuItem_Cancel_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex != 0)
            {
                MessageBox.Show("已完成的任务无法再取消。");
                return;
            }

            var grid = DataGrid_InProgressTasks;
            var row = grid.SelectedRows[0];
            if (row != null && row.Index >= 0 && !row.IsNewRow)
            {
                var taskId = (int)row.Cells[1].Value;
                _coreService.StopTask(taskId);
                LoadTaskList();

                var worker = _spiders.FirstOrDefault(x => x.TaskId == taskId);
                if (worker != null && Directory.Exists(worker.CurrentDir))
                    Directory.Delete(worker.CurrentDir, true);
            }
        }

        private void DataGrid_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            ///右键菜单
            if (e.Button == MouseButtons.Right)
            {
                var ctrl = sender as DataGridView;
                if (ctrl != null && e.RowIndex >= 0)
                {
                    ctrl.ClearSelection();
                    ctrl.Rows[e.RowIndex].Selected = true;
                    ctrl.CurrentCell = ctrl.Rows[e.RowIndex].Cells[e.ColumnIndex.ToMax(0)];
                    DataGridMenu.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void Link_Refresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoadTaskList();
        }

        private void HideModalFromNormal()
        {
            //将程序从任务栏移除显示
            this.ShowInTaskbar = false;
            //隐藏窗口
            Hide();
            //this.Visible = false;
            //显示托盘图标
            notifyIcon1.Visible = true;
            notifyIcon1.Text = Text;
            notifyIcon1.ShowBalloonTip(1000, Text, "窗口已被最小化到托盘", ToolTipIcon.Info);
        }

        private void ShowModalFromMinimum()
        {
            //设置程序允许显示在任务栏
            this.ShowInTaskbar = true;
            //设置窗口可见
            Show();
            //this.Visible = true;
            //设置窗口状态
            this.WindowState = FormWindowState.Normal;
            //设置窗口为活动状态，防止被其他窗口遮挡。
            this.Activate();
        }
        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                ShowModalFromMinimum();
        }

        private void MenuItem_ShowModal_Click(object sender, EventArgs e)
        {
            ShowModalFromMinimum();
        }

        private void MenuItem_Exit_Click(object sender, EventArgs e)
        {
            var inWorkingTasks = _taskList.Where(x => x.IsWorking).Select(x => x.Id).ToList();
            if (inWorkingTasks.Count == 0)
                Application.Exit();

            if (inWorkingTasks.Count > 0)
            {
                var result = MessageBox.Show("尚有未完成的任务，是否取消任务并关闭？", "提示", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    _coreService.BulkUpdateTaskStatus(inWorkingTasks, (int)TaskType.Canceled);
                    Application.Exit();
                }
                else
                {
                    return;
                }
            }

        }

        protected override void OnLeave(EventArgs e)
        {
            KeyboardHook.UnregisterHotKey(Handle, 100);
            KeyboardHook.UnregisterHotKey(Handle, 101);
            base.OnLeave(e);
        }

        protected override void OnActivated(EventArgs e)
        {
            //MessageBox.Show("OnActivated");
            KeyboardHook.RegisterHotKey(Handle, 100, KeyboardHook.KeyModifiers.Alt, Keys.Add);
            KeyboardHook.RegisterHotKey(Handle, 101, KeyboardHook.KeyModifiers.Alt, Keys.Subtract);
            base.OnActivated(e);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            //按快捷键 
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100:
                            //此处填写快捷键响应代码  
                            ShowModalFromMinimum();
                            break;
                        case 101:
                            //此处填写快捷键响应代码
                            HideModalFromNormal();
                            break;
                        case 102:
                            //此处填写快捷键响应代码
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        private void MenuItem_About_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void MenuItem_SetDir_Click(object sender, EventArgs e)
        {
            var folder = new FolderBrowserDialog();
            folder.InitialDirectory = Configs.BaseDir;
            if (folder.ShowDialog() == DialogResult.OK)
            {
                Configs.UpdateBaseDir(folder.SelectedPath);
            }
        }

        private void MenuItem_Replace_Click(object sender, EventArgs e)
        {
            new ReplaceTextForm().ShowDialog();
        }
    }
}