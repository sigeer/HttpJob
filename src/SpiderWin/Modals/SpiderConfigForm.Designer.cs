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
            labelName = new Label();
            TxtName = new TextBox();
            labelDescription = new Label();
            TxtDescription = new RichTextBox();
            ComboMethod = new ComboBox();
            labelMethod = new Label();
            label1 = new Label();
            TxtPostObj = new RichTextBox();
            label2 = new Label();
            label3 = new Label();
            ComboBoxNextPage = new ComboBox();
            menuStrip1 = new MenuStrip();
            设置ToolStripMenuItem = new ToolStripMenuItem();
            TemplateListMenu = new ToolStripMenuItem();
            BtnSubmit = new Button();
            BtnCancel = new Button();
            labelTemplateInfo = new LinkLabel();
            BtnRefreshTemplate = new Button();
            labelHeader = new Label();
            TextRequestHeaders = new RichTextBox();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(22, 36);
            labelName.Name = "labelName";
            labelName.Size = new Size(32, 17);
            labelName.TabIndex = 0;
            labelName.Text = "名称";
            // 
            // TxtName
            // 
            TxtName.Location = new Point(89, 33);
            TxtName.Name = "TxtName";
            TxtName.Size = new Size(547, 23);
            TxtName.TabIndex = 1;
            // 
            // labelDescription
            // 
            labelDescription.AutoSize = true;
            labelDescription.Location = new Point(22, 70);
            labelDescription.Name = "labelDescription";
            labelDescription.Size = new Size(32, 17);
            labelDescription.TabIndex = 2;
            labelDescription.Text = "描述";
            // 
            // TxtDescription
            // 
            TxtDescription.Location = new Point(89, 70);
            TxtDescription.Name = "TxtDescription";
            TxtDescription.Size = new Size(547, 58);
            TxtDescription.TabIndex = 3;
            TxtDescription.Text = "";
            // 
            // ComboMethod
            // 
            ComboMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboMethod.FormattingEnabled = true;
            ComboMethod.Items.AddRange(new object[] { "GET", "POST" });
            ComboMethod.Location = new Point(89, 137);
            ComboMethod.Name = "ComboMethod";
            ComboMethod.Size = new Size(121, 25);
            ComboMethod.TabIndex = 4;
            // 
            // labelMethod
            // 
            labelMethod.AutoSize = true;
            labelMethod.Location = new Point(12, 140);
            labelMethod.Name = "labelMethod";
            labelMethod.Size = new Size(56, 17);
            labelMethod.TabIndex = 5;
            labelMethod.Text = "请求方式";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(2, 240);
            label1.Name = "label1";
            label1.Size = new Size(77, 17);
            label1.TabIndex = 6;
            label1.Text = "POST BODY";
            // 
            // TxtPostObj
            // 
            TxtPostObj.Location = new Point(89, 240);
            TxtPostObj.Name = "TxtPostObj";
            TxtPostObj.Size = new Size(547, 52);
            TxtPostObj.TabIndex = 7;
            TxtPostObj.Text = "";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 348);
            label2.Name = "label2";
            label2.Size = new Size(56, 17);
            label2.TabIndex = 8;
            label2.Text = "内容读取";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(11, 309);
            label3.Name = "label3";
            label3.Size = new Size(68, 17);
            label3.TabIndex = 11;
            label3.Text = "下一页设置";
            // 
            // ComboBoxNextPage
            // 
            ComboBoxNextPage.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBoxNextPage.FormattingEnabled = true;
            ComboBoxNextPage.Location = new Point(89, 306);
            ComboBoxNextPage.Name = "ComboBoxNextPage";
            ComboBoxNextPage.Size = new Size(127, 25);
            ComboBoxNextPage.TabIndex = 12;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { 设置ToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(648, 25);
            menuStrip1.TabIndex = 13;
            menuStrip1.Text = "menuStrip1";
            // 
            // 设置ToolStripMenuItem
            // 
            设置ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { TemplateListMenu });
            设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            设置ToolStripMenuItem.Size = new Size(44, 21);
            设置ToolStripMenuItem.Text = "设置";
            // 
            // TemplateListMenu
            // 
            TemplateListMenu.Name = "TemplateListMenu";
            TemplateListMenu.Size = new Size(180, 22);
            TemplateListMenu.Text = "模板列表";
            TemplateListMenu.Click += TemplateListMenu_Click;
            // 
            // BtnSubmit
            // 
            BtnSubmit.Location = new Point(561, 391);
            BtnSubmit.Name = "BtnSubmit";
            BtnSubmit.Size = new Size(75, 23);
            BtnSubmit.TabIndex = 14;
            BtnSubmit.Text = "保存";
            BtnSubmit.UseVisualStyleBackColor = true;
            BtnSubmit.Click += BtnSubmit_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.Location = new Point(480, 391);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 23);
            BtnCancel.TabIndex = 15;
            BtnCancel.Text = "取消";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // labelTemplateInfo
            // 
            labelTemplateInfo.AutoSize = true;
            labelTemplateInfo.Location = new Point(89, 348);
            labelTemplateInfo.Name = "labelTemplateInfo";
            labelTemplateInfo.Size = new Size(63, 17);
            labelTemplateInfo.TabIndex = 16;
            labelTemplateInfo.TabStop = true;
            labelTemplateInfo.Text = "已选择0项";
            labelTemplateInfo.Click += labelTemplateInfo_Click;
            // 
            // BtnRefreshTemplate
            // 
            BtnRefreshTemplate.Location = new Point(222, 306);
            BtnRefreshTemplate.Name = "BtnRefreshTemplate";
            BtnRefreshTemplate.Size = new Size(45, 25);
            BtnRefreshTemplate.TabIndex = 17;
            BtnRefreshTemplate.Text = "刷新";
            BtnRefreshTemplate.UseVisualStyleBackColor = true;
            BtnRefreshTemplate.Click += BtnRefreshTemplate_Click;
            // 
            // labelHeader
            // 
            labelHeader.AutoSize = true;
            labelHeader.Location = new Point(22, 171);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new Size(44, 17);
            labelHeader.TabIndex = 18;
            labelHeader.Text = "请求头";
            // 
            // TextRequestHeaders
            // 
            TextRequestHeaders.Location = new Point(89, 168);
            TextRequestHeaders.Name = "TextRequestHeaders";
            TextRequestHeaders.Size = new Size(547, 66);
            TextRequestHeaders.TabIndex = 19;
            TextRequestHeaders.Text = "";
            // 
            // SpiderConfigForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(648, 426);
            Controls.Add(TextRequestHeaders);
            Controls.Add(labelHeader);
            Controls.Add(BtnRefreshTemplate);
            Controls.Add(labelTemplateInfo);
            Controls.Add(BtnCancel);
            Controls.Add(BtnSubmit);
            Controls.Add(ComboBoxNextPage);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(TxtPostObj);
            Controls.Add(label1);
            Controls.Add(labelMethod);
            Controls.Add(ComboMethod);
            Controls.Add(TxtDescription);
            Controls.Add(labelDescription);
            Controls.Add(TxtName);
            Controls.Add(labelName);
            Controls.Add(menuStrip1);
            Icon = GlobalResource.icon;
            KeyPreview = true;
            MainMenuStrip = menuStrip1;
            Name = "SpiderConfigForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "爬虫设置";
            Load += SpiderConfigForm_Load;
            KeyDown += SpiderConfigForm_KeyDown;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private Label label3;
        private ComboBox ComboBoxNextPage;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem 设置ToolStripMenuItem;
        private ToolStripMenuItem TemplateListMenu;
        private Button BtnSubmit;
        private Button BtnCancel;
        private LinkLabel labelTemplateInfo;
        private Button BtnRefreshTemplate;
        private Label labelHeader;
        private RichTextBox TextRequestHeaders;
    }
}