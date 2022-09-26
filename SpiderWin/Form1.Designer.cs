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
            this.components = new System.ComponentModel.Container();
            this.btnShowConfig = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ComboxSpider = new System.Windows.Forms.ComboBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.工作设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UseLocalMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.UseServiceMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuNewSpider = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.mainModalStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ResultTxtBox = new System.Windows.Forms.RichTextBox();
            this.ComboxUrl = new System.Windows.Forms.ComboBox();
            this.DataGridTasks = new System.Windows.Forms.DataGridView();
            this.LinkExportLog = new System.Windows.Forms.LinkLabel();
            this.DataGridMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TaskStop = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridTasks)).BeginInit();
            this.DataGridMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnShowConfig
            // 
            this.btnShowConfig.Location = new System.Drawing.Point(554, 84);
            this.btnShowConfig.Name = "btnShowConfig";
            this.btnShowConfig.Size = new System.Drawing.Size(73, 25);
            this.btnShowConfig.TabIndex = 0;
            this.btnShowConfig.Text = "编辑爬虫";
            this.btnShowConfig.UseVisualStyleBackColor = true;
            this.btnShowConfig.Click += new System.EventHandler(this.btnShowConfig_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "URL";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "选择配置";
            // 
            // ComboxSpider
            // 
            this.ComboxSpider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboxSpider.FormattingEnabled = true;
            this.ComboxSpider.Location = new System.Drawing.Point(80, 84);
            this.ComboxSpider.Name = "ComboxSpider";
            this.ComboxSpider.Size = new System.Drawing.Size(468, 25);
            this.ComboxSpider.TabIndex = 5;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(79, 124);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 6;
            this.btnRun.Text = "运行";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(674, 25);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.工作设置ToolStripMenuItem,
            this.MenuNewSpider});
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.设置ToolStripMenuItem.Text = "设置";
            // 
            // 工作设置ToolStripMenuItem
            // 
            this.工作设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UseLocalMenu,
            this.UseServiceMenu});
            this.工作设置ToolStripMenuItem.Name = "工作设置ToolStripMenuItem";
            this.工作设置ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.工作设置ToolStripMenuItem.Text = "工作设置";
            // 
            // UseLocalMenu
            // 
            this.UseLocalMenu.Checked = true;
            this.UseLocalMenu.CheckOnClick = true;
            this.UseLocalMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UseLocalMenu.Name = "UseLocalMenu";
            this.UseLocalMenu.Size = new System.Drawing.Size(112, 22);
            this.UseLocalMenu.Text = "本地";
            this.UseLocalMenu.Click += new System.EventHandler(this.UseLocalMenu_Click);
            // 
            // UseServiceMenu
            // 
            this.UseServiceMenu.CheckOnClick = true;
            this.UseServiceMenu.Name = "UseServiceMenu";
            this.UseServiceMenu.Size = new System.Drawing.Size(112, 22);
            this.UseServiceMenu.Text = "服务器";
            this.UseServiceMenu.Click += new System.EventHandler(this.UseServiceMenu_Click);
            // 
            // MenuNewSpider
            // 
            this.MenuNewSpider.Name = "MenuNewSpider";
            this.MenuNewSpider.Size = new System.Drawing.Size(124, 22);
            this.MenuNewSpider.Text = "新增爬虫";
            this.MenuNewSpider.Click += new System.EventHandler(this.MenuNewSpider_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainModalStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 395);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(674, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // mainModalStatusLabel
            // 
            this.mainModalStatusLabel.Name = "mainModalStatusLabel";
            this.mainModalStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // ResultTxtBox
            // 
            this.ResultTxtBox.Location = new System.Drawing.Point(78, 296);
            this.ResultTxtBox.Name = "ResultTxtBox";
            this.ResultTxtBox.ReadOnly = true;
            this.ResultTxtBox.Size = new System.Drawing.Size(548, 96);
            this.ResultTxtBox.TabIndex = 11;
            this.ResultTxtBox.Text = "";
            this.ResultTxtBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.ResultTxtBox_LinkClicked);
            // 
            // ComboxUrl
            // 
            this.ComboxUrl.FormattingEnabled = true;
            this.ComboxUrl.Location = new System.Drawing.Point(79, 41);
            this.ComboxUrl.Name = "ComboxUrl";
            this.ComboxUrl.Size = new System.Drawing.Size(547, 25);
            this.ComboxUrl.TabIndex = 12;
            // 
            // DataGridTasks
            // 
            this.DataGridTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridTasks.ContextMenuStrip = this.DataGridMenu;
            this.DataGridTasks.Location = new System.Drawing.Point(78, 153);
            this.DataGridTasks.Name = "DataGridTasks";
            this.DataGridTasks.RowTemplate.Height = 25;
            this.DataGridTasks.Size = new System.Drawing.Size(548, 137);
            this.DataGridTasks.TabIndex = 13;
            this.DataGridTasks.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridTasks_CellDoubleClick);
            this.DataGridTasks.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridTasks_CellMouseUp);
            // 
            // LinkExportLog
            // 
            this.LinkExportLog.AutoSize = true;
            this.LinkExportLog.Location = new System.Drawing.Point(571, 127);
            this.LinkExportLog.Name = "LinkExportLog";
            this.LinkExportLog.Size = new System.Drawing.Size(56, 17);
            this.LinkExportLog.TabIndex = 16;
            this.LinkExportLog.TabStop = true;
            this.LinkExportLog.Text = "导出日志";
            this.LinkExportLog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkExportLog_LinkClicked);
            // 
            // DataGridMenu
            // 
            this.DataGridMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TaskStop});
            this.DataGridMenu.Name = "DataGridMenu";
            this.DataGridMenu.Size = new System.Drawing.Size(181, 48);
            // 
            // TaskStop
            // 
            this.TaskStop.Name = "TaskStop";
            this.TaskStop.Size = new System.Drawing.Size(180, 22);
            this.TaskStop.Text = "中断";
            this.TaskStop.Click += new System.EventHandler(this.TaskStop_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 417);
            this.Controls.Add(this.LinkExportLog);
            this.Controls.Add(this.DataGridTasks);
            this.Controls.Add(this.ComboxUrl);
            this.Controls.Add(this.ResultTxtBox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.ComboxSpider);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnShowConfig);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "爬虫";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridTasks)).EndInit();
            this.DataGridMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private DataGridView DataGridTasks;
        private LinkLabel LinkExportLog;
        private ContextMenuStrip DataGridMenu;
        private ToolStripMenuItem TaskStop;
    }
}