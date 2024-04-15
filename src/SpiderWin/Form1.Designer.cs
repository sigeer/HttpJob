namespace SpiderWin
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            btnShowConfig = new Button();
            label1 = new Label();
            label2 = new Label();
            ComboxSpider = new ComboBox();
            btnRun = new Button();
            menuStrip1 = new MenuStrip();
            设置ToolStripMenuItem = new ToolStripMenuItem();
            工作设置ToolStripMenuItem = new ToolStripMenuItem();
            UseLocalMenu = new ToolStripMenuItem();
            UseServiceMenu = new ToolStripMenuItem();
            MenuNewSpider = new ToolStripMenuItem();
            MenuItem_SetDir = new ToolStripMenuItem();
            MenuItem_Dir = new ToolStripMenuItem();
            MenuItem_LogDir = new ToolStripMenuItem();
            帮助ToolStripMenuItem = new ToolStripMenuItem();
            MenuItem_About = new ToolStripMenuItem();
            工具ToolStripMenuItem = new ToolStripMenuItem();
            MenuItem_Replace = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            mainModalStatusLabel = new ToolStripStatusLabel();
            ResultTxtBox = new RichTextBox();
            ComboxUrl = new ComboBox();
            DataGrid_InProgressTasks = new DataGridView();
            DataGridMenu = new ContextMenuStrip(components);
            MenuItem_UseTask = new ToolStripMenuItem();
            MenuItem_OpenSaveDir = new ToolStripMenuItem();
            MenuItem_Cancel = new ToolStripMenuItem();
            MenuItem_Remove = new ToolStripMenuItem();
            BtnCacel = new Button();
            LinkClearLog = new LinkLabel();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            DataGrid_OtherTasks = new DataGridView();
            panel1 = new Panel();
            Link_Refresh = new LinkLabel();
            notifyIcon1 = new NotifyIcon(components);
            IconMenu = new ContextMenuStrip(components);
            MenuItem_ShowModal = new ToolStripMenuItem();
            MenuItem_Exit = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DataGrid_InProgressTasks).BeginInit();
            DataGridMenu.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DataGrid_OtherTasks).BeginInit();
            panel1.SuspendLayout();
            IconMenu.SuspendLayout();
            SuspendLayout();
            // 
            // btnShowConfig
            // 
            btnShowConfig.Location = new Point(552, 46);
            btnShowConfig.Name = "btnShowConfig";
            btnShowConfig.Size = new Size(73, 25);
            btnShowConfig.TabIndex = 0;
            btnShowConfig.Text = "编辑爬虫";
            btnShowConfig.UseVisualStyleBackColor = true;
            btnShowConfig.Click += btnShowConfig_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(11, 6);
            label1.Name = "label1";
            label1.Size = new Size(31, 17);
            label1.TabIndex = 1;
            label1.Text = "URL";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(8, 46);
            label2.Name = "label2";
            label2.Size = new Size(56, 17);
            label2.TabIndex = 3;
            label2.Text = "选择配置";
            // 
            // ComboxSpider
            // 
            ComboxSpider.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboxSpider.FormattingEnabled = true;
            ComboxSpider.Location = new Point(78, 46);
            ComboxSpider.Name = "ComboxSpider";
            ComboxSpider.Size = new Size(468, 25);
            ComboxSpider.TabIndex = 5;
            // 
            // btnRun
            // 
            btnRun.Location = new Point(77, 86);
            btnRun.Name = "btnRun";
            btnRun.Size = new Size(75, 23);
            btnRun.TabIndex = 6;
            btnRun.Text = "运行";
            btnRun.UseVisualStyleBackColor = true;
            btnRun.Click += btnRun_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { 设置ToolStripMenuItem, 帮助ToolStripMenuItem, 工具ToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(674, 25);
            menuStrip1.TabIndex = 8;
            menuStrip1.Text = "menuStrip1";
            // 
            // 设置ToolStripMenuItem
            // 
            设置ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 工作设置ToolStripMenuItem, MenuNewSpider, MenuItem_SetDir, MenuItem_Dir, MenuItem_LogDir });
            设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            设置ToolStripMenuItem.Size = new Size(44, 21);
            设置ToolStripMenuItem.Text = "设置";
            // 
            // 工作设置ToolStripMenuItem
            // 
            工作设置ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { UseLocalMenu, UseServiceMenu });
            工作设置ToolStripMenuItem.Name = "工作设置ToolStripMenuItem";
            工作设置ToolStripMenuItem.Size = new Size(180, 22);
            工作设置ToolStripMenuItem.Text = "工作设置";
            // 
            // UseLocalMenu
            // 
            UseLocalMenu.Checked = true;
            UseLocalMenu.CheckState = CheckState.Checked;
            UseLocalMenu.Name = "UseLocalMenu";
            UseLocalMenu.Size = new Size(180, 22);
            UseLocalMenu.Text = "本地";
            UseLocalMenu.Click += UseLocalMenu_Click;
            // 
            // UseServiceMenu
            // 
            UseServiceMenu.Name = "UseServiceMenu";
            UseServiceMenu.Size = new Size(180, 22);
            UseServiceMenu.Text = "服务器";
            UseServiceMenu.Click += UseServiceMenu_Click;
            // 
            // MenuNewSpider
            // 
            MenuNewSpider.Name = "MenuNewSpider";
            MenuNewSpider.Size = new Size(180, 22);
            MenuNewSpider.Text = "新增爬虫";
            MenuNewSpider.Click += MenuNewSpider_Click;
            // 
            // MenuItem_SetDir
            // 
            MenuItem_SetDir.Name = "MenuItem_SetDir";
            MenuItem_SetDir.Size = new Size(180, 22);
            MenuItem_SetDir.Text = "设置保存目录";
            MenuItem_SetDir.Click += MenuItem_SetDir_Click;
            // 
            // MenuItem_Dir
            // 
            MenuItem_Dir.Name = "MenuItem_Dir";
            MenuItem_Dir.Size = new Size(180, 22);
            MenuItem_Dir.Text = "打开保存目录";
            MenuItem_Dir.Click += MenuItem_Dir_Click;
            // 
            // MenuItem_LogDir
            // 
            MenuItem_LogDir.Name = "MenuItem_LogDir";
            MenuItem_LogDir.Size = new Size(180, 22);
            MenuItem_LogDir.Text = "打开日志目录";
            MenuItem_LogDir.Click += MenuItem_LogDir_Click;
            // 
            // 帮助ToolStripMenuItem
            // 
            帮助ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { MenuItem_About });
            帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            帮助ToolStripMenuItem.Size = new Size(44, 21);
            帮助ToolStripMenuItem.Text = "帮助";
            // 
            // MenuItem_About
            // 
            MenuItem_About.Name = "MenuItem_About";
            MenuItem_About.Size = new Size(100, 22);
            MenuItem_About.Text = "关于";
            MenuItem_About.Click += MenuItem_About_Click;
            // 
            // 工具ToolStripMenuItem
            // 
            工具ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { MenuItem_Replace });
            工具ToolStripMenuItem.Name = "工具ToolStripMenuItem";
            工具ToolStripMenuItem.Size = new Size(44, 21);
            工具ToolStripMenuItem.Text = "工具";
            // 
            // MenuItem_Replace
            // 
            MenuItem_Replace.Name = "MenuItem_Replace";
            MenuItem_Replace.Size = new Size(124, 22);
            MenuItem_Replace.Text = "文本替换";
            MenuItem_Replace.Click += MenuItem_Replace_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { mainModalStatusLabel });
            statusStrip1.Location = new Point(0, 430);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(674, 22);
            statusStrip1.TabIndex = 10;
            statusStrip1.Text = "statusStrip1";
            // 
            // mainModalStatusLabel
            // 
            mainModalStatusLabel.Name = "mainModalStatusLabel";
            mainModalStatusLabel.Size = new Size(0, 17);
            // 
            // ResultTxtBox
            // 
            ResultTxtBox.Location = new Point(76, 312);
            ResultTxtBox.Name = "ResultTxtBox";
            ResultTxtBox.ReadOnly = true;
            ResultTxtBox.Size = new Size(548, 77);
            ResultTxtBox.TabIndex = 11;
            ResultTxtBox.Text = "";
            ResultTxtBox.LinkClicked += ResultTxtBox_LinkClicked;
            // 
            // ComboxUrl
            // 
            ComboxUrl.FormattingEnabled = true;
            ComboxUrl.Location = new Point(77, 3);
            ComboxUrl.Name = "ComboxUrl";
            ComboxUrl.Size = new Size(547, 25);
            ComboxUrl.TabIndex = 12;
            // 
            // DataGrid_InProgressTasks
            // 
            DataGrid_InProgressTasks.AllowUserToAddRows = false;
            DataGrid_InProgressTasks.AllowUserToDeleteRows = false;
            DataGrid_InProgressTasks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGrid_InProgressTasks.Location = new Point(6, 6);
            DataGrid_InProgressTasks.Name = "DataGrid_InProgressTasks";
            DataGrid_InProgressTasks.ReadOnly = true;
            DataGrid_InProgressTasks.RowTemplate.Height = 25;
            DataGrid_InProgressTasks.Size = new Size(527, 152);
            DataGrid_InProgressTasks.TabIndex = 13;
            DataGrid_InProgressTasks.CellDoubleClick += DataGrid_CellDoubleClick;
            DataGrid_InProgressTasks.CellMouseUp += DataGrid_CellMouseUp;
            // 
            // DataGridMenu
            // 
            DataGridMenu.Items.AddRange(new ToolStripItem[] { MenuItem_UseTask, MenuItem_OpenSaveDir, MenuItem_Cancel, MenuItem_Remove });
            DataGridMenu.Name = "DataGridMenu";
            DataGridMenu.Size = new Size(149, 92);
            // 
            // MenuItem_UseTask
            // 
            MenuItem_UseTask.Name = "MenuItem_UseTask";
            MenuItem_UseTask.Size = new Size(148, 22);
            MenuItem_UseTask.Text = "使用";
            MenuItem_UseTask.Click += MenuItem_UseTask_Click;
            // 
            // MenuItem_OpenSaveDir
            // 
            MenuItem_OpenSaveDir.Name = "MenuItem_OpenSaveDir";
            MenuItem_OpenSaveDir.Size = new Size(148, 22);
            MenuItem_OpenSaveDir.Text = "打开保存目录";
            MenuItem_OpenSaveDir.Click += MenuItem_OpenSaveDir_Click;
            // 
            // MenuItem_Cancel
            // 
            MenuItem_Cancel.Name = "MenuItem_Cancel";
            MenuItem_Cancel.Size = new Size(148, 22);
            MenuItem_Cancel.Text = "取消任务";
            MenuItem_Cancel.Click += MenuItem_Cancel_Click;
            // 
            // MenuItem_Remove
            // 
            MenuItem_Remove.Name = "MenuItem_Remove";
            MenuItem_Remove.Size = new Size(148, 22);
            MenuItem_Remove.Text = "移除";
            MenuItem_Remove.Click += MenuItem_Remove_Click;
            // 
            // BtnCacel
            // 
            BtnCacel.Location = new Point(158, 86);
            BtnCacel.Name = "BtnCacel";
            BtnCacel.Size = new Size(75, 23);
            BtnCacel.TabIndex = 17;
            BtnCacel.Text = "停止";
            BtnCacel.UseVisualStyleBackColor = true;
            BtnCacel.Click += BtnCacel_Click;
            // 
            // LinkClearLog
            // 
            LinkClearLog.AutoSize = true;
            LinkClearLog.Location = new Point(569, 92);
            LinkClearLog.Name = "LinkClearLog";
            LinkClearLog.Size = new Size(56, 17);
            LinkClearLog.TabIndex = 18;
            LinkClearLog.TabStop = true;
            LinkClearLog.Text = "清理输出";
            LinkClearLog.LinkClicked += LinkClearLog_LinkClicked;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(78, 115);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(547, 191);
            tabControl1.TabIndex = 19;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(DataGrid_InProgressTasks);
            tabPage1.Location = new Point(4, 26);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(539, 161);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "正在进行";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(DataGrid_OtherTasks);
            tabPage2.Location = new Point(4, 26);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(539, 161);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "其他";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // DataGrid_OtherTasks
            // 
            DataGrid_OtherTasks.AllowUserToAddRows = false;
            DataGrid_OtherTasks.AllowUserToDeleteRows = false;
            DataGrid_OtherTasks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGrid_OtherTasks.Location = new Point(6, 6);
            DataGrid_OtherTasks.Name = "DataGrid_OtherTasks";
            DataGrid_OtherTasks.ReadOnly = true;
            DataGrid_OtherTasks.RowTemplate.Height = 25;
            DataGrid_OtherTasks.Size = new Size(527, 152);
            DataGrid_OtherTasks.TabIndex = 0;
            DataGrid_OtherTasks.CellDoubleClick += DataGrid_CellDoubleClick;
            DataGrid_OtherTasks.CellMouseUp += DataGrid_CellMouseUp;
            // 
            // panel1
            // 
            panel1.Controls.Add(Link_Refresh);
            panel1.Controls.Add(ComboxUrl);
            panel1.Controls.Add(tabControl1);
            panel1.Controls.Add(btnShowConfig);
            panel1.Controls.Add(LinkClearLog);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(BtnCacel);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(ComboxSpider);
            panel1.Controls.Add(btnRun);
            panel1.Controls.Add(ResultTxtBox);
            panel1.Location = new Point(3, 28);
            panel1.Name = "panel1";
            panel1.Size = new Size(671, 399);
            panel1.TabIndex = 20;
            // 
            // Link_Refresh
            // 
            Link_Refresh.AutoSize = true;
            Link_Refresh.Location = new Point(531, 92);
            Link_Refresh.Name = "Link_Refresh";
            Link_Refresh.Size = new Size(32, 17);
            Link_Refresh.TabIndex = 20;
            Link_Refresh.TabStop = true;
            Link_Refresh.Text = "刷新";
            Link_Refresh.LinkClicked += Link_Refresh_LinkClicked;
            // 
            // notifyIcon1
            // 
            notifyIcon1.ContextMenuStrip = IconMenu;
            notifyIcon1.Icon = GlobalResource.icon;
            notifyIcon1.Visible = true;
            notifyIcon1.MouseClick += NotifyIcon_MouseClick;
            // 
            // IconMenu
            // 
            IconMenu.Items.AddRange(new ToolStripItem[] { MenuItem_ShowModal, MenuItem_Exit });
            IconMenu.Name = "IconMenu";
            IconMenu.Size = new Size(101, 48);
            // 
            // MenuItem_ShowModal
            // 
            MenuItem_ShowModal.Name = "MenuItem_ShowModal";
            MenuItem_ShowModal.Size = new Size(100, 22);
            MenuItem_ShowModal.Text = "打开";
            MenuItem_ShowModal.Click += MenuItem_ShowModal_Click;
            // 
            // MenuItem_Exit
            // 
            MenuItem_Exit.Name = "MenuItem_Exit";
            MenuItem_Exit.Size = new Size(100, 22);
            MenuItem_Exit.Text = "关闭";
            MenuItem_Exit.Click += MenuItem_Exit_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(674, 452);
            Controls.Add(panel1);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            Icon = GlobalResource.icon;
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "爬虫";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)DataGrid_InProgressTasks).EndInit();
            DataGridMenu.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)DataGrid_OtherTasks).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            IconMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnShowConfig;
        private Label label1;
        private Label label2;
        private ComboBox ComboxSpider;
        private Button btnRun;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem 设置ToolStripMenuItem;
        private ToolStripMenuItem 工作设置ToolStripMenuItem;
        private ToolStripMenuItem UseLocalMenu;
        private ToolStripMenuItem UseServiceMenu;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel mainModalStatusLabel;
        private ToolStripMenuItem MenuNewSpider;
        private RichTextBox ResultTxtBox;
        private ComboBox ComboxUrl;
        private DataGridView DataGrid_InProgressTasks;
        private Button BtnCacel;
        private LinkLabel LinkClearLog;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private DataGridView DataGrid_OtherTasks;
        private Panel panel1;
        private ToolStripMenuItem MenuItem_Dir;
        private ToolStripMenuItem MenuItem_LogDir;
        private ContextMenuStrip DataGridMenu;
        private ToolStripMenuItem MenuItem_UseTask;
        private ToolStripMenuItem MenuItem_OpenSaveDir;
        private ToolStripMenuItem MenuItem_Cancel;
        private ToolStripMenuItem MenuItem_Remove;
        private LinkLabel Link_Refresh;
        private NotifyIcon notifyIcon1;
        private ContextMenuStrip IconMenu;
        private ToolStripMenuItem MenuItem_ShowModal;
        private ToolStripMenuItem MenuItem_Exit;
        private ToolStripMenuItem 帮助ToolStripMenuItem;
        private ToolStripMenuItem MenuItem_About;
        private ToolStripMenuItem MenuItem_SetDir;
        private ToolStripMenuItem 工具ToolStripMenuItem;
        private ToolStripMenuItem MenuItem_Replace;
    }
}