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
            components = new System.ComponentModel.Container();
            DataGridViewMain = new DataGridView();
            DataGridMenu = new ContextMenuStrip(components);
            DataGridMenu_Item_Delete = new ToolStripMenuItem();
            BtnSave = new Button();
            Txt_Search = new TextBox();
            Btn_Search = new Button();
            ((System.ComponentModel.ISupportInitialize)DataGridViewMain).BeginInit();
            DataGridMenu.SuspendLayout();
            SuspendLayout();
            // 
            // DataGridViewMain
            // 
            DataGridViewMain.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridViewMain.Location = new Point(12, 41);
            DataGridViewMain.Name = "DataGridViewMain";
            DataGridViewMain.RowTemplate.Height = 25;
            DataGridViewMain.Size = new Size(425, 298);
            DataGridViewMain.TabIndex = 0;
            DataGridViewMain.CellMouseUp += DataGridViewMain_CellMouseUp;
            DataGridViewMain.DefaultValuesNeeded += DataGridViewMain_DefaultValuesNeeded;
            DataGridViewMain.RowValidating += DataGridViewMain_RowValidating;
            // 
            // DataGridMenu
            // 
            DataGridMenu.Items.AddRange(new ToolStripItem[] { DataGridMenu_Item_Delete });
            DataGridMenu.Name = "DataGridMenu";
            DataGridMenu.Size = new Size(101, 26);
            // 
            // DataGridMenu_Item_Delete
            // 
            DataGridMenu_Item_Delete.Name = "DataGridMenu_Item_Delete";
            DataGridMenu_Item_Delete.Size = new Size(100, 22);
            DataGridMenu_Item_Delete.Text = "删除";
            DataGridMenu_Item_Delete.Click += DataGridMenu_Item_Delete_Click;
            // 
            // BtnSave
            // 
            BtnSave.Location = new Point(362, 345);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 23);
            BtnSave.TabIndex = 2;
            BtnSave.Text = "保存";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // Txt_Search
            // 
            Txt_Search.Location = new Point(12, 12);
            Txt_Search.Name = "Txt_Search";
            Txt_Search.Size = new Size(188, 23);
            Txt_Search.TabIndex = 3;
            Txt_Search.KeyUp += Txt_Search_KeyUp;
            // 
            // Btn_Search
            // 
            Btn_Search.Location = new Point(206, 12);
            Btn_Search.Name = "Btn_Search";
            Btn_Search.Size = new Size(75, 23);
            Btn_Search.TabIndex = 4;
            Btn_Search.Text = "查找重复项";
            Btn_Search.UseVisualStyleBackColor = true;
            Btn_Search.Click += Btn_Search_Click;
            // 
            // TxtReplaceRuleManageForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(449, 375);
            Controls.Add(Btn_Search);
            Controls.Add(Txt_Search);
            Controls.Add(BtnSave);
            Controls.Add(DataGridViewMain);
            Icon = GlobalResource.icon;
            Name = "TxtReplaceRuleManageForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "替换规则";
            Load += TxtReplaceRuleManageForm_Load;
            ((System.ComponentModel.ISupportInitialize)DataGridViewMain).EndInit();
            DataGridMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
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