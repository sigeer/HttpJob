namespace SpiderWin.Modals
{
    partial class ReplaceTextForm
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
            BtnSelectFile = new Button();
            TextFilePath = new TextBox();
            BtnDo = new Button();
            Btn_OpenDir = new Button();
            BtnConfig1 = new Button();
            BtnConfig2 = new Button();
            BtnConfig3 = new Button();
            SuspendLayout();
            // 
            // BtnSelectFile
            // 
            BtnSelectFile.Location = new Point(12, 12);
            BtnSelectFile.Name = "BtnSelectFile";
            BtnSelectFile.Size = new Size(80, 24);
            BtnSelectFile.TabIndex = 3;
            BtnSelectFile.Text = "选择文件";
            BtnSelectFile.UseVisualStyleBackColor = true;
            BtnSelectFile.Click += BtnSelectFile_Click;
            // 
            // TextFilePath
            // 
            TextFilePath.AllowDrop = true;
            TextFilePath.Location = new Point(98, 12);
            TextFilePath.Name = "TextFilePath";
            TextFilePath.Size = new Size(345, 23);
            TextFilePath.TabIndex = 4;
            TextFilePath.TextChanged += TextFilePath_TextChanged;
            TextFilePath.DragDrop += TextFilePath_DragDrop;
            TextFilePath.DragEnter += TextFilePath_DragEnter;
            // 
            // BtnDo
            // 
            BtnDo.Location = new Point(363, 52);
            BtnDo.Name = "BtnDo";
            BtnDo.Size = new Size(80, 40);
            BtnDo.TabIndex = 5;
            BtnDo.Text = "开始";
            BtnDo.UseVisualStyleBackColor = true;
            BtnDo.Click += BtnDo_Click;
            // 
            // Btn_OpenDir
            // 
            Btn_OpenDir.Location = new Point(271, 52);
            Btn_OpenDir.Name = "Btn_OpenDir";
            Btn_OpenDir.Size = new Size(86, 40);
            Btn_OpenDir.TabIndex = 6;
            Btn_OpenDir.Text = "打开目录";
            Btn_OpenDir.UseVisualStyleBackColor = true;
            Btn_OpenDir.Visible = false;
            Btn_OpenDir.Click += Btn_OpenDir_Click;
            // 
            // BtnConfig1
            // 
            BtnConfig1.Location = new Point(12, 52);
            BtnConfig1.Name = "BtnConfig1";
            BtnConfig1.Size = new Size(80, 40);
            BtnConfig1.TabIndex = 7;
            BtnConfig1.Text = "配置1";
            BtnConfig1.UseVisualStyleBackColor = true;
            BtnConfig1.Click += BtnConfig1_Click;
            // 
            // BtnConfig2
            // 
            BtnConfig2.Location = new Point(98, 52);
            BtnConfig2.Name = "BtnConfig2";
            BtnConfig2.Size = new Size(80, 40);
            BtnConfig2.TabIndex = 8;
            BtnConfig2.Text = "设置2";
            BtnConfig2.UseVisualStyleBackColor = true;
            BtnConfig2.Click += BtnConfig2_Click;
            // 
            // BtnConfig3
            // 
            BtnConfig3.Location = new Point(184, 52);
            BtnConfig3.Name = "BtnConfig3";
            BtnConfig3.Size = new Size(81, 40);
            BtnConfig3.TabIndex = 9;
            BtnConfig3.Text = "设置3";
            BtnConfig3.UseVisualStyleBackColor = true;
            BtnConfig3.Click += BtnConfig3_Click;
            // 
            // ReplaceTextForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(455, 104);
            Controls.Add(BtnConfig3);
            Controls.Add(BtnConfig2);
            Controls.Add(BtnConfig1);
            Controls.Add(Btn_OpenDir);
            Controls.Add(BtnDo);
            Controls.Add(TextFilePath);
            Controls.Add(BtnSelectFile);
            Name = "ReplaceTextForm";
            Text = "文本替换（支持正则表达式）";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtnSelectFile;
        private TextBox TextFilePath;
        private Button BtnDo;
        private Button Btn_OpenDir;
        private Button BtnConfig1;
        private Button BtnConfig2;
        private Button BtnConfig3;
    }
}