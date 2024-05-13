namespace SpiderWin.Modals
{
    partial class ContentConfigForm
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
            label1 = new Label();
            comboType = new ComboBox();
            label2 = new Label();
            txtXPath = new TextBox();
            label3 = new Label();
            btnReplaceRule = new Button();
            comboSpider = new ComboBox();
            label4 = new Label();
            txtName = new TextBox();
            BtnSave = new Button();
            TxtAttribute = new TextBox();
            labalAttr = new Label();
            LinkLabel_NewConfig = new LinkLabel();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(43, 73);
            label1.Name = "label1";
            label1.Size = new Size(32, 17);
            label1.TabIndex = 0;
            label1.Text = "类型";
            // 
            // comboType
            // 
            comboType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboType.FormattingEnabled = true;
            comboType.Location = new Point(90, 70);
            comboType.Name = "comboType";
            comboType.Size = new Size(160, 25);
            comboType.TabIndex = 1;
            comboType.SelectedValueChanged += ComboType_SelectedValueChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(36, 119);
            label2.Name = "label2";
            label2.Size = new Size(39, 17);
            label2.TabIndex = 2;
            label2.Text = "xPath";
            // 
            // txtXPath
            // 
            txtXPath.Location = new Point(90, 116);
            txtXPath.Name = "txtXPath";
            txtXPath.Size = new Size(160, 23);
            txtXPath.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(43, 208);
            label3.Name = "label3";
            label3.Size = new Size(32, 17);
            label3.TabIndex = 4;
            label3.Text = "使用";
            // 
            // btnReplaceRule
            // 
            btnReplaceRule.Location = new Point(36, 250);
            btnReplaceRule.Name = "btnReplaceRule";
            btnReplaceRule.Size = new Size(214, 23);
            btnReplaceRule.TabIndex = 5;
            btnReplaceRule.Text = "文本替换规则";
            btnReplaceRule.UseVisualStyleBackColor = true;
            btnReplaceRule.Click += BtnReplaceRule_Click;
            // 
            // comboSpider
            // 
            comboSpider.DropDownStyle = ComboBoxStyle.DropDownList;
            comboSpider.FormattingEnabled = true;
            comboSpider.Location = new Point(89, 205);
            comboSpider.Name = "comboSpider";
            comboSpider.Size = new Size(161, 25);
            comboSpider.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(39, 30);
            label4.Name = "label4";
            label4.Size = new Size(32, 17);
            label4.TabIndex = 7;
            label4.Text = "名称";
            // 
            // txtName
            // 
            txtName.Location = new Point(90, 27);
            txtName.Name = "txtName";
            txtName.Size = new Size(160, 23);
            txtName.TabIndex = 8;
            // 
            // BtnSave
            // 
            BtnSave.Location = new Point(38, 290);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(212, 23);
            BtnSave.TabIndex = 9;
            BtnSave.Text = "保存";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // TxtAttribute
            // 
            TxtAttribute.Location = new Point(89, 159);
            TxtAttribute.Name = "TxtAttribute";
            TxtAttribute.Size = new Size(161, 23);
            TxtAttribute.TabIndex = 13;
            // 
            // labalAttr
            // 
            labalAttr.AutoSize = true;
            labalAttr.Location = new Point(17, 162);
            labalAttr.Name = "labalAttr";
            labalAttr.Size = new Size(58, 17);
            labalAttr.TabIndex = 12;
            labalAttr.Text = "Attribute";
            // 
            // LinkLabel_NewConfig
            // 
            LinkLabel_NewConfig.AutoSize = true;
            LinkLabel_NewConfig.Location = new Point(256, 208);
            LinkLabel_NewConfig.Name = "LinkLabel_NewConfig";
            LinkLabel_NewConfig.Size = new Size(32, 17);
            LinkLabel_NewConfig.TabIndex = 14;
            LinkLabel_NewConfig.TabStop = true;
            LinkLabel_NewConfig.Text = "新增";
            LinkLabel_NewConfig.LinkClicked += LinkLabel_NewConfig_LinkClicked;
            // 
            // ContentConfigForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(317, 324);
            Controls.Add(LinkLabel_NewConfig);
            Controls.Add(TxtAttribute);
            Controls.Add(labalAttr);
            Controls.Add(BtnSave);
            Controls.Add(txtName);
            Controls.Add(label4);
            Controls.Add(comboSpider);
            Controls.Add(btnReplaceRule);
            Controls.Add(label3);
            Controls.Add(txtXPath);
            Controls.Add(label2);
            Controls.Add(comboType);
            Controls.Add(label1);
            Icon = GlobalResource.icon;
            KeyPreview = true;
            Name = "ContentConfigForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "内容读取配置";
            Load += ContentConfigForm_Load;
            KeyDown += ContentConfigForm_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox comboType;
        private Label label2;
        private TextBox txtXPath;
        private Label label3;
        private Button btnReplaceRule;
        private ComboBox comboSpider;
        private Label label4;
        private TextBox txtName;
        private Button BtnSave;
        private TextBox TxtAttribute;
        private Label labalAttr;
        private LinkLabel LinkLabel_NewConfig;
    }
}