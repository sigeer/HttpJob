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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.labelMethod = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dropdownContentReader = new System.Windows.Forms.ComboBox();
            this.btnAddContentReader = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(22, 19);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(32, 17);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "名称";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(89, 16);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(547, 23);
            this.textBox1.TabIndex = 1;
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(22, 53);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(32, 17);
            this.labelDescription.TabIndex = 2;
            this.labelDescription.Text = "描述";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(89, 53);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(547, 58);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "GET",
            "POST"});
            this.comboBox1.Location = new System.Drawing.Point(89, 120);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 25);
            this.comboBox1.TabIndex = 4;
            // 
            // labelMethod
            // 
            this.labelMethod.AutoSize = true;
            this.labelMethod.Location = new System.Drawing.Point(12, 123);
            this.labelMethod.Name = "labelMethod";
            this.labelMethod.Size = new System.Drawing.Size(56, 17);
            this.labelMethod.TabIndex = 5;
            this.labelMethod.Text = "请求方式";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "POST BODY";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(89, 160);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(547, 52);
            this.richTextBox2.TabIndex = 7;
            this.richTextBox2.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 235);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "label2";
            // 
            // dropdownContentReader
            // 
            this.dropdownContentReader.FormattingEnabled = true;
            this.dropdownContentReader.Location = new System.Drawing.Point(89, 232);
            this.dropdownContentReader.Name = "dropdownContentReader";
            this.dropdownContentReader.Size = new System.Drawing.Size(275, 25);
            this.dropdownContentReader.TabIndex = 9;
            // 
            // btnAddContentReader
            // 
            this.btnAddContentReader.Location = new System.Drawing.Point(90, 270);
            this.btnAddContentReader.Name = "btnAddContentReader";
            this.btnAddContentReader.Size = new System.Drawing.Size(275, 25);
            this.btnAddContentReader.TabIndex = 10;
            this.btnAddContentReader.Text = "添加读取模板";
            this.btnAddContentReader.UseVisualStyleBackColor = true;
            this.btnAddContentReader.Click += new System.EventHandler(this.btnAddContentReader_Click);
            // 
            // SpiderConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 554);
            this.Controls.Add(this.btnAddContentReader);
            this.Controls.Add(this.dropdownContentReader);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelMethod);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.labelName);
            this.Name = "SpiderConfigForm";
            this.Text = "SpiderConfigForm";
            this.Load += new System.EventHandler(this.SpiderConfigForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label labelName;
        private TextBox textBox1;
        private Label labelDescription;
        private RichTextBox richTextBox1;
        private ComboBox comboBox1;
        private Label labelMethod;
        private Label label1;
        private RichTextBox richTextBox2;
        private Label label2;
        private ComboBox dropdownContentReader;
        private Button btnAddContentReader;
    }
}