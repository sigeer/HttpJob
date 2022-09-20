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
            this.btnShowConfig = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dropConfig = new System.Windows.Forms.ComboBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.工作设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.本地ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.服务器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listBoxUrl = new System.Windows.Forms.ListBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.mainModalStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnShowConfig
            // 
            this.btnShowConfig.Location = new System.Drawing.Point(80, 136);
            this.btnShowConfig.Name = "btnShowConfig";
            this.btnShowConfig.Size = new System.Drawing.Size(129, 23);
            this.btnShowConfig.TabIndex = 0;
            this.btnShowConfig.Text = "查看爬虫配置";
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
            // txtUrl
            // 
            this.txtUrl.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtUrl.Location = new System.Drawing.Point(81, 41);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(546, 23);
            this.txtUrl.TabIndex = 2;
            this.txtUrl.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtUrl_KeyUp);
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
            // dropConfig
            // 
            this.dropConfig.FormattingEnabled = true;
            this.dropConfig.Location = new System.Drawing.Point(80, 84);
            this.dropConfig.Name = "dropConfig";
            this.dropConfig.Size = new System.Drawing.Size(547, 25);
            this.dropConfig.TabIndex = 5;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(79, 184);
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
            this.工作设置ToolStripMenuItem});
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.设置ToolStripMenuItem.Text = "设置";
            // 
            // 工作设置ToolStripMenuItem
            // 
            this.工作设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.本地ToolStripMenuItem,
            this.服务器ToolStripMenuItem});
            this.工作设置ToolStripMenuItem.Name = "工作设置ToolStripMenuItem";
            this.工作设置ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.工作设置ToolStripMenuItem.Text = "工作设置";
            // 
            // 本地ToolStripMenuItem
            // 
            this.本地ToolStripMenuItem.Name = "本地ToolStripMenuItem";
            this.本地ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.本地ToolStripMenuItem.Text = "本地";
            // 
            // 服务器ToolStripMenuItem
            // 
            this.服务器ToolStripMenuItem.Name = "服务器ToolStripMenuItem";
            this.服务器ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.服务器ToolStripMenuItem.Text = "服务器";
            // 
            // listBoxUrl
            // 
            this.listBoxUrl.FormattingEnabled = true;
            this.listBoxUrl.ItemHeight = 17;
            this.listBoxUrl.Location = new System.Drawing.Point(80, 64);
            this.listBoxUrl.Name = "listBoxUrl";
            this.listBoxUrl.Size = new System.Drawing.Size(546, 89);
            this.listBoxUrl.TabIndex = 9;
            this.listBoxUrl.Visible = false;
            this.listBoxUrl.Click += new System.EventHandler(this.listBoxUrl_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainModalStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 226);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(674, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // mainModalStatusLabel
            // 
            this.mainModalStatusLabel.Name = "mainModalStatusLabel";
            this.mainModalStatusLabel.Size = new System.Drawing.Size(131, 17);
            this.mainModalStatusLabel.Text = "toolStripStatusLabel1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 248);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.listBoxUrl);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.dropConfig);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnShowConfig);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "爬虫";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Click += new System.EventHandler(this.Form1_Click);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnShowConfig;
        private Label label1;
        private TextBox txtUrl;
        private Label label2;
        private ComboBox dropConfig;
        private Button btnRun;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem 设置ToolStripMenuItem;
        private ToolStripMenuItem 工作设置ToolStripMenuItem;
        private ToolStripMenuItem 本地ToolStripMenuItem;
        private ToolStripMenuItem 服务器ToolStripMenuItem;
        private ListBox listBoxUrl;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel mainModalStatusLabel;
    }
}