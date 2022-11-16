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
            this.DataGridMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.DataGridMenu_Item_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.BtnSave = new System.Windows.Forms.Button();
            this.Txt_Search = new System.Windows.Forms.TextBox();
            this.Btn_Search = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewMain)).BeginInit();
            this.DataGridMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // DataGridViewMain
            // 
            this.DataGridViewMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridViewMain.Location = new System.Drawing.Point(12, 41);
            this.DataGridViewMain.Name = "DataGridViewMain";
            this.DataGridViewMain.RowTemplate.Height = 25;
            this.DataGridViewMain.Size = new System.Drawing.Size(425, 298);
            this.DataGridViewMain.TabIndex = 0;
            this.DataGridViewMain.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewMain_CellMouseUp);
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
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(362, 345);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 23);
            this.BtnSave.TabIndex = 2;
            this.BtnSave.Text = "保存";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // Txt_Search
            // 
            this.Txt_Search.Location = new System.Drawing.Point(12, 12);
            this.Txt_Search.Name = "Txt_Search";
            this.Txt_Search.Size = new System.Drawing.Size(188, 23);
            this.Txt_Search.TabIndex = 3;
            this.Txt_Search.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Txt_Search_KeyUp);
            // 
            // Btn_Search
            // 
            this.Btn_Search.Location = new System.Drawing.Point(206, 12);
            this.Btn_Search.Name = "Btn_Search";
            this.Btn_Search.Size = new System.Drawing.Size(75, 23);
            this.Btn_Search.TabIndex = 4;
            this.Btn_Search.Text = "查找重复项";
            this.Btn_Search.UseVisualStyleBackColor = true;
            this.Btn_Search.Click += new System.EventHandler(this.Btn_Search_Click);
            // 
            // TxtReplaceRuleManageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 375);
            this.Controls.Add(this.Btn_Search);
            this.Controls.Add(this.Txt_Search);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.DataGridViewMain);
            this.Name = "TxtReplaceRuleManageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "替换规则";
            this.Load += new System.EventHandler(this.TxtReplaceRuleManageForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewMain)).EndInit();
            this.DataGridMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView DataGridViewMain;
        private Button BtnSave;
        private ContextMenuStrip DataGridMenu;
        private ToolStripMenuItem DataGridMenu_Item_Delete;
        private TextBox Txt_Search;
        private Button Btn_Search;
    }
}