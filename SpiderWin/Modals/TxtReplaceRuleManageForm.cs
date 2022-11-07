using SpiderTool.Dto.Spider;

namespace SpiderWin.Modals
{
    public partial class TxtReplaceRuleManageForm : Form
    {
        readonly List<ReplacementRuleDto>? _rules;
        public event EventHandler<List<ReplacementRuleDto>>? OnOk;
        public TxtReplaceRuleManageForm(List<ReplacementRuleDto>? rules = null)
        {
            _rules = rules;

            InitializeComponent();
        }

        private void TxtReplaceRuleManageForm_Load(object sender, EventArgs e)
        {
            DataGridViewMain.Columns.Add(new DataGridViewColumn()
            {
                HeaderText = "忽视大小写",
                Name = nameof(ReplacementRuleDto.ReplacementOldStr),
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
            DataGridViewMain.ContextMenuStrip = DataGridMenu;

            _rules?.ForEach(x =>
            {
                var row = new DataGridViewRow();

                var chkCell = new DataGridViewCheckBoxCell() { Value = x.IgnoreCase, ValueType = typeof(bool?) };
                row.Cells.Add(chkCell);
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.ReplacementOldStr, ValueType = typeof(string) });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.ReplacementNewlyStr, ValueType = typeof(string) });

                DataGridViewMain.Rows.Add(row);
            });
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            List<ReplacementRuleDto> data = new List<ReplacementRuleDto>();
            for (int i = 0; i < DataGridViewMain.Rows.Count - 1; i++)
            {
                var row = DataGridViewMain.Rows[i];
                var col1 = row.Cells[1].Value?.ToString();
                var col2 = row.Cells[2].Value?.ToString();
                if (string.IsNullOrEmpty(col1))
                    continue;
                data.Add(new ReplacementRuleDto
                {
                    IgnoreCase = (bool)(row.Cells[0]?.Value ?? false),
                    ReplacementOldStr = col1.ToString(),
                    ReplacementNewlyStr = col2?.ToString(),
                });
            }
            OnOk?.Invoke(this, data);
            Close();
        }

        private void DataGridMenu_Item_Delete_Click(object sender, EventArgs e)
        {
            var selectedRow = DataGridViewMain.Rows[DataGridViewMain.SelectedCells[0].RowIndex];
            if (selectedRow != null && !selectedRow.IsNewRow)
            {
                DataGridViewMain.Rows.Remove(selectedRow);
            }
        }
    }
}
