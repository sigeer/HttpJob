namespace SpiderWin.Modals
{
    partial class TxtReplaceRuleManageForm
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
            this.components = new System.ComponentModel.Container();
            this.DataGridViewMain = new System.Windows.Forms.DataGridView();
            this.BtnSave = new System.Windows.Forms.Button();
            this.DataGridMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.DataGridMenu_Item_Delete = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewMain)).BeginInit();
            this.DataGridMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // DataGridViewMain
            // 
            this.DataGridViewMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridViewMain.Location = new System.Drawing.Point(12, 12);
            this.DataGridViewMain.Name = "DataGridViewMain";
            this.DataGridViewMain.RowTemplate.Height = 25;
            this.DataGridViewMain.Size = new System.Drawing.Size(425, 298);
            this.DataGridViewMain.TabIndex = 0;
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(362, 318);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 23);
            this.BtnSave.TabIndex = 2;
            this.BtnSave.Text = "保存";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // DataGridMenu
            // 
            this.DataGridMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DataGridMenu_Item_Delete});
            this.DataGridMenu.Name = "DataGridMenu";
            this.DataGridMenu.Size = new System.Drawing.Size(101, 26);
            // 
            // DataGridMenu_Item_Delete
            // 
            this.DataGridMenu_Item_Delete.Name = "DataGridMenu_Item_Delete";
            this.DataGridMenu_Item_Delete.Size = new System.Drawing.Size(100, 22);
            this.DataGridMenu_Item_Delete.Text = "删除";
            this.DataGridMenu_Item_Delete.Click += new System.EventHandler(this.DataGridMenu_Item_Delete_Click);
            // 
            // TxtReplaceRuleManageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 353);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.DataGridViewMain);
            this.Name = "TxtReplaceRuleManageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TxtReplaceRuleManageForm";
            this.Load += new System.EventHandler(this.TxtReplaceRuleManageForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewMain)).EndInit();
            this.DataGridMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView DataGridViewMain;
        private Button BtnSave;
        private ContextMenuStrip DataGridMenu;
        private ToolStripMenuItem DataGridMenu_Item_Delete;
    }
}