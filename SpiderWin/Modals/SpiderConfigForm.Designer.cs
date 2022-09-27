namespace SpiderWin.Modals
{
    partial class SpiderConfigForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelName = new System.Windows.Forms.Label();
            this.TxtName = new System.Windows.Forms.TextBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.TxtDescription = new System.Windows.Forms.RichTextBox();
            this.ComboMethod = new System.Windows.Forms.ComboBox();
            this.labelMethod = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtPostObj = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAddContentReader = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ComboBoxNextPage = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TemplateListMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.BtnSubmit = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.labelTemplateInfo = new System.Windows.Forms.LinkLabel();
            this.BtnRefreshTemplate = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(22, 36);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(32, 17);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "名称";
            // 
            // TxtName
            // 
            this.TxtName.Location = new System.Drawing.Point(89, 33);
            this.TxtName.Name = "TxtName";
            this.TxtName.Size = new System.Drawing.Size(547, 23);
            this.TxtName.TabIndex = 1;
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(22, 70);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(32, 17);
            this.labelDescription.TabIndex = 2;
            this.labelDescription.Text = "描述";
            // 
            // TxtDescription
            // 
            this.TxtDescription.Location = new System.Drawing.Point(89, 70);
            this.TxtDescription.Name = "TxtDescription";
            this.TxtDescription.Size = new System.Drawing.Size(547, 58);
            this.TxtDescription.TabIndex = 3;
            this.TxtDescription.Text = "";
            // 
            // ComboMethod
            // 
            this.ComboMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboMethod.FormattingEnabled = true;
            this.ComboMethod.Items.AddRange(new object[] {
            "GET",
            "POST"});
            this.ComboMethod.Location = new System.Drawing.Point(89, 137);
            this.ComboMethod.Name = "ComboMethod";
            this.ComboMethod.Size = new System.Drawing.Size(121, 25);
            this.ComboMethod.TabIndex = 4;
            // 
            // labelMethod
            // 
            this.labelMethod.AutoSize = true;
            this.labelMethod.Location = new System.Drawing.Point(12, 140);
            this.labelMethod.Name = "labelMethod";
            this.labelMethod.Size = new System.Drawing.Size(56, 17);
            this.labelMethod.TabIndex = 5;
            this.labelMethod.Text = "请求方式";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 177);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "POST BODY";
            // 
            // TxtPostObj
            // 
            this.TxtPostObj.Location = new System.Drawing.Point(89, 177);
            this.TxtPostObj.Name = "TxtPostObj";
            this.TxtPostObj.Size = new System.Drawing.Size(547, 52);
            this.TxtPostObj.TabIndex = 7;
            this.TxtPostObj.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 285);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "内容读取";
            // 
            // btnAddContentReader
            // 
            this.btnAddContentReader.Location = new System.Drawing.Point(89, 281);
            this.btnAddContentReader.Name = "btnAddContentReader";
            this.btnAddContentReader.Size = new System.Drawing.Size(127, 25);
            this.btnAddContentReader.TabIndex = 10;
            this.btnAddContentReader.Text = "选择模板";
            this.btnAddContentReader.UseVisualStyleBackColor = true;
            this.btnAddContentReader.Click += new System.EventHandler(this.btnAddContentReader_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 246);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 17);
            this.label3.TabIndex = 11;
            this.label3.Text = "下一页设置";
            // 
            // ComboBoxNextPage
            // 
            this.ComboBoxNextPage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxNextPage.FormattingEnabled = true;
            this.ComboBoxNextPage.Location = new System.Drawing.Point(89, 243);
            this.ComboBoxNextPage.Name = "ComboBoxNextPage";
            this.ComboBoxNextPage.Size = new System.Drawing.Size(127, 25);
            this.ComboBoxNextPage.TabIndex = 12;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(648, 25);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TemplateListMenu});
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.设置ToolStripMenuItem.Text = "设置";
            // 
            // TemplateListMenu
            // 
            this.TemplateListMenu.Name = "TemplateListMenu";
            this.TemplateListMenu.Size = new System.Drawing.Size(124, 22);
            this.TemplateListMenu.Text = "模板列表";
            this.TemplateListMenu.Click += new System.EventHandler(this.TemplateListMenu_Click);
            // 
            // BtnSubmit
            // 
            this.BtnSubmit.Location = new System.Drawing.Point(561, 320);
            this.BtnSubmit.Name = "BtnSubmit";
            this.BtnSubmit.Size = new System.Drawing.Size(75, 23);
            this.BtnSubmit.TabIndex = 14;
            this.BtnSubmit.Text = "保存";
            this.BtnSubmit.UseVisualStyleBackColor = true;
            this.BtnSubmit.Click += new System.EventHandler(this.BtnSubmit_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(467, 320);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 15;
            this.BtnCancel.Text = "取消";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // labelTemplateInfo
            // 
            this.labelTemplateInfo.AutoSize = true;
            this.labelTemplateInfo.Location = new System.Drawing.Point(216, 285);
            this.labelTemplateInfo.Name = "labelTemplateInfo";
            this.labelTemplateInfo.Size = new System.Drawing.Size(0, 17);
            this.labelTemplateInfo.TabIndex = 16;
            this.labelTemplateInfo.Click += new System.EventHandler(this.labelTemplateInfo_Click);
            // 
            // BtnRefreshTemplate
            // 
            this.BtnRefreshTemplate.Location = new System.Drawing.Point(222, 243);
            this.BtnRefreshTemplate.Name = "BtnRefreshTemplate";
            this.BtnRefreshTemplate.Size = new System.Drawing.Size(45, 25);
            this.BtnRefreshTemplate.TabIndex = 17;
            this.BtnRefreshTemplate.Text = "刷新";
            this.BtnRefreshTemplate.UseVisualStyleBackColor = true;
            this.BtnRefreshTemplate.Click += new System.EventHandler(this.BtnRefreshTemplate_Click);
            // 
            // SpiderConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(648, 355);
            this.Controls.Add(this.BtnRefreshTemplate);
            this.Controls.Add(this.labelTemplateInfo);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnSubmit);
            this.Controls.Add(this.ComboBoxNextPage);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnAddContentReader);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TxtPostObj);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelMethod);
            this.Controls.Add(this.ComboMethod);
            this.Controls.Add(this.TxtDescription);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.TxtName);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SpiderConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "爬虫设置";
            this.Load += new System.EventHandler(this.SpiderConfigForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpiderConfigForm_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label labelName;
        private TextBox TxtName;
        private Label labelDescription;
        private RichTextBox TxtDescription;
        private ComboBox ComboMethod;
        private Label labelMethod;
        private Label label1;
        private RichTextBox TxtPostObj;
        private Label label2;
        private Button btnAddContentReader;
        private Label label3;
        private ComboBox ComboBoxNextPage;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem 设置ToolStripMenuItem;
        private ToolStripMenuItem TemplateListMenu;
        private Button BtnSubmit;
        private Button BtnCancel;
        private LinkLabel labelTemplateInfo;
        private Button BtnRefreshTemplate;
    }
}