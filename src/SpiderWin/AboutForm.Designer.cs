namespace SpiderWin
{
    partial class AboutForm
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
            this.Label_Version = new System.Windows.Forms.Label();
            this.Link_Update = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 279);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "当前版本：";
            // 
            // Label_Version
            // 
            this.Label_Version.AutoSize = true;
            this.Label_Version.Location = new System.Drawing.Point(74, 279);
            this.Label_Version.Name = "Label_Version";
            this.Label_Version.Size = new System.Drawing.Size(35, 17);
            this.Label_Version.TabIndex = 1;
            this.Label_Version.Text = "1.0.0";
            // 
            // Link_Update
            // 
            this.Link_Update.AutoSize = true;
            this.Link_Update.Location = new System.Drawing.Point(115, 279);
            this.Link_Update.Name = "Link_Update";
            this.Link_Update.Size = new System.Drawing.Size(56, 17);
            this.Link_Update.TabIndex = 2;
            this.Link_Update.TabStop = true;
            this.Link_Update.Text = "检查更新";
            this.Link_Update.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_Update_LinkClicked);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 305);
            this.Controls.Add(this.Link_Update);
            this.Controls.Add(this.Label_Version);
            this.Controls.Add(this.label1);
            this.Icon = global::SpiderWin.GlobalResource.icon;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "关于";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Label Label_Version;
        private LinkLabel Link_Update;
    }
}