﻿namespace SpiderWin
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
            this.MenuItem_Dir = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.mainModalStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ResultTxtBox = new System.Windows.Forms.RichTextBox();
            this.ComboxUrl = new System.Windows.Forms.ComboBox();
            this.DataGrid_InProgressTasks = new System.Windows.Forms.DataGridView();
            this.BtnCacel = new System.Windows.Forms.Button();
            this.LinkClearLog = new System.Windows.Forms.LinkLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.DataGrid_OtherTasks = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid_InProgressTasks)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid_OtherTasks)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnShowConfig
            // 
            this.btnShowConfig.Location = new System.Drawing.Point(552, 46);
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
            this.label1.Location = new System.Drawing.Point(11, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "URL";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "选择配置";
            // 
            // ComboxSpider
            // 
            this.ComboxSpider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboxSpider.FormattingEnabled = true;
            this.ComboxSpider.Location = new System.Drawing.Point(78, 46);
            this.ComboxSpider.Name = "ComboxSpider";
            this.ComboxSpider.Size = new System.Drawing.Size(468, 25);
            this.ComboxSpider.TabIndex = 5;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(77, 86);
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
            this.MenuNewSpider,
            this.MenuItem_Dir});
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
            this.工作设置ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
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
            this.MenuNewSpider.Size = new System.Drawing.Size(148, 22);
            this.MenuNewSpider.Text = "新增爬虫";
            this.MenuNewSpider.Click += new System.EventHandler(this.MenuNewSpider_Click);
            // 
            // MenuItem_Dir
            // 
            this.MenuItem_Dir.Name = "MenuItem_Dir";
            this.MenuItem_Dir.Size = new System.Drawing.Size(148, 22);
            this.MenuItem_Dir.Text = "打开保存目录";
            this.MenuItem_Dir.Click += new System.EventHandler(this.MenuItem_Dir_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainModalStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 430);
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
            this.ResultTxtBox.Location = new System.Drawing.Point(76, 312);
            this.ResultTxtBox.Name = "ResultTxtBox";
            this.ResultTxtBox.ReadOnly = true;
            this.ResultTxtBox.Size = new System.Drawing.Size(548, 77);
            this.ResultTxtBox.TabIndex = 11;
            this.ResultTxtBox.Text = "";
            this.ResultTxtBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.ResultTxtBox_LinkClicked);
            // 
            // ComboxUrl
            // 
            this.ComboxUrl.FormattingEnabled = true;
            this.ComboxUrl.Location = new System.Drawing.Point(77, 3);
            this.ComboxUrl.Name = "ComboxUrl";
            this.ComboxUrl.Size = new System.Drawing.Size(547, 25);
            this.ComboxUrl.TabIndex = 12;
            // 
            // DataGrid_InProgressTasks
            // 
            this.DataGrid_InProgressTasks.AllowUserToAddRows = false;
            this.DataGrid_InProgressTasks.AllowUserToDeleteRows = false;
            this.DataGrid_InProgressTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGrid_InProgressTasks.Location = new System.Drawing.Point(6, 6);
            this.DataGrid_InProgressTasks.Name = "DataGrid_InProgressTasks";
            this.DataGrid_InProgressTasks.ReadOnly = true;
            this.DataGrid_InProgressTasks.RowTemplate.Height = 25;
            this.DataGrid_InProgressTasks.Size = new System.Drawing.Size(527, 149);
            this.DataGrid_InProgressTasks.TabIndex = 13;
            this.DataGrid_InProgressTasks.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridTasks_CellDoubleClick);
            // 
            // BtnCacel
            // 
            this.BtnCacel.Location = new System.Drawing.Point(158, 86);
            this.BtnCacel.Name = "BtnCacel";
            this.BtnCacel.Size = new System.Drawing.Size(75, 23);
            this.BtnCacel.TabIndex = 17;
            this.BtnCacel.Text = "停止";
            this.BtnCacel.UseVisualStyleBackColor = true;
            this.BtnCacel.Click += new System.EventHandler(this.BtnCacel_Click);
            // 
            // LinkClearLog
            // 
            this.LinkClearLog.AutoSize = true;
            this.LinkClearLog.Location = new System.Drawing.Point(569, 92);
            this.LinkClearLog.Name = "LinkClearLog";
            this.LinkClearLog.Size = new System.Drawing.Size(56, 17);
            this.LinkClearLog.TabIndex = 18;
            this.LinkClearLog.TabStop = true;
            this.LinkClearLog.Text = "清理输出";
            this.LinkClearLog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClearLog_LinkClicked);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(78, 115);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(547, 191);
            this.tabControl1.TabIndex = 19;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.DataGrid_InProgressTasks);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(539, 161);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "正在进行";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.DataGrid_OtherTasks);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(539, 161);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "其他";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // DataGrid_OtherTasks
            // 
            this.DataGrid_OtherTasks.AllowUserToAddRows = false;
            this.DataGrid_OtherTasks.AllowUserToDeleteRows = false;
            this.DataGrid_OtherTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGrid_OtherTasks.Location = new System.Drawing.Point(6, 6);
            this.DataGrid_OtherTasks.Name = "DataGrid_OtherTasks";
            this.DataGrid_OtherTasks.ReadOnly = true;
            this.DataGrid_OtherTasks.RowTemplate.Height = 25;
            this.DataGrid_OtherTasks.Size = new System.Drawing.Size(527, 150);
            this.DataGrid_OtherTasks.TabIndex = 0;
            this.DataGrid_OtherTasks.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridTasks_CellDoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ComboxUrl);
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Controls.Add(this.btnShowConfig);
            this.panel1.Controls.Add(this.LinkClearLog);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.BtnCacel);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ComboxSpider);
            this.panel1.Controls.Add(this.btnRun);
            this.panel1.Controls.Add(this.ResultTxtBox);
            this.panel1.Location = new System.Drawing.Point(3, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(671, 399);
            this.panel1.TabIndex = 20;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(674, 452);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "爬虫";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid_InProgressTasks)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid_OtherTasks)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private DataGridView DataGrid_InProgressTasks;
        private Button BtnCacel;
        private LinkLabel LinkClearLog;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private DataGridView DataGrid_OtherTasks;
        private Panel panel1;
        private ToolStripMenuItem MenuItem_Dir;
    }
}