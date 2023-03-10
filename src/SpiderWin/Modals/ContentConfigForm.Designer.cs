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
            this.label1 = new System.Windows.Forms.Label();
            this.comboType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtXPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnReplaceRule = new System.Windows.Forms.Button();
            this.comboSpider = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.BtnSave = new System.Windows.Forms.Button();
            this.labalAttr = new System.Windows.Forms.Label();
            this.TxtAttribute = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "类型";
            // 
            // comboType
            // 
            this.comboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboType.FormattingEnabled = true;
            this.comboType.Location = new System.Drawing.Point(90, 70);
            this.comboType.Name = "comboType";
            this.comboType.Size = new System.Drawing.Size(180, 25);
            this.comboType.TabIndex = 1;
            this.comboType.SelectedValueChanged += new System.EventHandler(this.ComboType_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "xPath";
            // 
            // txtXPath
            // 
            this.txtXPath.Location = new System.Drawing.Point(90, 116);
            this.txtXPath.Name = "txtXPath";
            this.txtXPath.Size = new System.Drawing.Size(179, 23);
            this.txtXPath.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 168);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "使用";
            // 
            // btnReplaceRule
            // 
            this.btnReplaceRule.Location = new System.Drawing.Point(30, 261);
            this.btnReplaceRule.Name = "btnReplaceRule";
            this.btnReplaceRule.Size = new System.Drawing.Size(233, 23);
            this.btnReplaceRule.TabIndex = 5;
            this.btnReplaceRule.Text = "文本替换规则";
            this.btnReplaceRule.UseVisualStyleBackColor = true;
            this.btnReplaceRule.Click += new System.EventHandler(this.BtnReplaceRule_Click);
            // 
            // comboSpider
            // 
            this.comboSpider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSpider.FormattingEnabled = true;
            this.comboSpider.Location = new System.Drawing.Point(89, 165);
            this.comboSpider.Name = "comboSpider";
            this.comboSpider.Size = new System.Drawing.Size(180, 25);
            this.comboSpider.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "名称";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(90, 27);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(179, 23);
            this.txtName.TabIndex = 8;
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(32, 301);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(231, 23);
            this.BtnSave.TabIndex = 9;
            this.BtnSave.Text = "保存";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // labalAttr
            // 
            this.labalAttr.AutoSize = true;
            this.labalAttr.Location = new System.Drawing.Point(17, 211);
            this.labalAttr.Name = "labalAttr";
            this.labalAttr.Size = new System.Drawing.Size(58, 17);
            this.labalAttr.TabIndex = 10;
            this.labalAttr.Text = "Attribute";
            // 
            // TxtAttribute
            // 
            this.TxtAttribute.Location = new System.Drawing.Point(89, 208);
            this.TxtAttribute.Name = "TxtAttribute";
            this.TxtAttribute.Size = new System.Drawing.Size(181, 23);
            this.TxtAttribute.TabIndex = 11;
            // 
            // ContentConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 336);
            this.Controls.Add(this.TxtAttribute);
            this.Controls.Add(this.labalAttr);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboSpider);
            this.Controls.Add(this.btnReplaceRule);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtXPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboType);
            this.Controls.Add(this.label1);
            this.Icon = global::SpiderWin.GlobalResource.icon;
            this.KeyPreview = true;
            this.Name = "ContentConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "模板设置";
            this.Load += new System.EventHandler(this.ContentConfigForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ContentConfigForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private Label labalAttr;
        private TextBox TxtAttribute;
    }
}