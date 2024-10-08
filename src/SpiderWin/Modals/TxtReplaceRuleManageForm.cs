﻿using SpiderTool.Data.Dto.Spider;
using Utility.Extensions;

namespace SpiderWin.Modals
{
    public partial class TxtReplaceRuleManageForm : Form
    {
        readonly List<ReplacementRuleDto>? _rules;
        public event EventHandler<List<ReplacementRuleDto>>? OnOk;
        const int Col_UseRegex = 0;
        const int Col_OldStr = 1;
        const int Col_NewStr = 2;
        public TxtReplaceRuleManageForm(List<ReplacementRuleDto>? rules = null)
        {
            _rules = rules;

            InitializeComponent();
        }

        private void TxtReplaceRuleManageForm_Load(object sender, EventArgs e)
        {
            DataGridViewMain.Columns.Add(new DataGridViewColumn()
            {
                HeaderText = "使用正则",
                Name = nameof(ReplacementRuleDto.UseRegex),
                Width = 80,
                CellTemplate = new DataGridViewCheckBoxCell()
            });
            DataGridViewMain.Columns.Add(new DataGridViewColumn()
            {
                HeaderText = "待替换",
                Name = nameof(ReplacementRuleDto.ReplacementOldStr),
                Width = (DataGridViewMain.Width - 120) / 2,
                CellTemplate = new DataGridViewTextBoxCell()
            });
            DataGridViewMain.Columns.Add(new DataGridViewColumn()
            {
                HeaderText = "替换值",
                Name = nameof(ReplacementRuleDto.ReplacementNewlyStr),
                Width = (DataGridViewMain.Width - 120) / 2,
                CellTemplate = new DataGridViewTextBoxCell()
            });
            DataGridViewMain.DataError += (obj, e) =>
            {
                if (DataGridViewMain.Rows[e.RowIndex].IsNewRow)
                    return;
            };

            _rules?.ForEach(x =>
            {
                var row = new DataGridViewRow();
                row.Cells.Add(new DataGridViewCheckBoxCell() { Value = x.UseRegex, ValueType = typeof(bool?) });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.ReplacementOldStr, ValueType = typeof(string) });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.ReplacementNewlyStr, ValueType = typeof(string) });
                row.ContextMenuStrip = DataGridMenu;
                DataGridViewMain.Rows.Add(row);
            });
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            List<ReplacementRuleDto> data = new List<ReplacementRuleDto>();
            for (int i = 0; i < DataGridViewMain.Rows.Count - 1; i++)
            {
                var row = DataGridViewMain.Rows[i];
                var col1 = row.Cells[Col_OldStr].Value?.ToString();
                var col2 = row.Cells[Col_NewStr].Value?.ToString();
                if (string.IsNullOrEmpty(col1))
                    continue;
                data.Add(new ReplacementRuleDto
                {
                    UseRegex = (bool)(row.Cells[Col_UseRegex]?.Value ?? true),
                    ReplacementOldStr = col1.ToString(),
                    ReplacementNewlyStr = col2?.ToString(),
                });
            }
            OnOk?.Invoke(this, data);
            Close();
        }



        private void DataGridViewMain_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            ///右键菜单
            if (e.Button == MouseButtons.Right)
            {
                var ctrl = sender as DataGridView;
                if (ctrl != null && e.RowIndex >= 0)
                {
                    ctrl.ClearSelection();
                    ctrl.Rows[e.RowIndex].Selected = true;
                    ctrl.CurrentCell = ctrl.Rows[e.RowIndex].Cells[e.ColumnIndex.ToMax(0)];
                    DataGridMenu.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void Search()
        {
            List<DataGridViewRow> filteredRows = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in DataGridViewMain.Rows)
            {
                var cellOldVal = row.Cells[Col_OldStr].Value?.ToString() ?? "";
                var cellNewVal = row.Cells[Col_NewStr].Value?.ToString() ?? "";
                if (cellOldVal.Contains(Txt_Search.Text) || cellNewVal.Contains(Txt_Search.Text))
                {
                    filteredRows.Add(row);
                }
            }
            DataGridViewMain.ClearSelection();
            for (int i = 0; i < filteredRows.Count; i++)
            {
                var row = filteredRows[i];
                if (i == 0)
                    DataGridViewMain.FirstDisplayedScrollingRowIndex = row.Index;
                row.Selected = true;
            }
        }

        private void Btn_Search_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Txt_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search();
            }
        }

        private void DataGridViewMain_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[Col_UseRegex].Value = true;
        }

        private void DataGridViewMain_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (sender is DataGridView dgv && dgv.Rows[e.RowIndex].IsNewRow)
            {
                if (string.IsNullOrEmpty(dgv.Rows[e.RowIndex].Cells[Col_OldStr].Value?.ToString()))
                {
                    e.Cancel = true; // Cancel the addition if validation fails
                }
            }
        }

        private void DataGrid_MenuItem_Insert_Click(object sender, EventArgs e)
        {
            if (DataGridViewMain.CurrentRow.IsNewRow)
                return;

            var selectedIndex = DataGridViewMain.CurrentRow.Index;

            var row = new DataGridViewRow();
            row.Cells.Add(new DataGridViewCheckBoxCell() { Value = true, ValueType = typeof(bool?) });
            row.Cells.Add(new DataGridViewTextBoxCell() { Value = "", ValueType = typeof(string) });
            row.Cells.Add(new DataGridViewTextBoxCell() { Value = "", ValueType = typeof(string) });
            row.ContextMenuStrip = DataGridMenu;
            DataGridViewMain.Rows.Insert(selectedIndex, row);
        }

        private void DataGridMenu_Item_Delete_Click(object sender, EventArgs e)
        {
            var selectedRow = DataGridViewMain.Rows[DataGridViewMain.CurrentRow.Index];
            if (selectedRow != null && !selectedRow.IsNewRow)
            {
                DataGridViewMain.Rows.Remove(selectedRow);
            }
        }
    }
}
