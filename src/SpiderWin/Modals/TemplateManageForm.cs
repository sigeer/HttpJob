using SpiderTool.Data.Dto.Spider;
using SpiderTool.Data.IService;

namespace SpiderWin.Modals
{
    public partial class TemplateManageForm : Form
    {
        public event EventHandler<List<int>>? OnSelect;
        readonly ISpiderService _service;
        readonly List<int> _selected;
        List<TemplateDetailViewModel> _templates = new List<TemplateDetailViewModel>();
        public TemplateManageForm(ISpiderService service, List<int>? selected = null)
        {
            _service = service;
            _selected = selected ?? new List<int>();
            InitializeComponent();
        }

        private async Task LoadData()
        {
            _templates = await _service.GetTemplateDtoListAsync();
        }

        private void PreLoadForm()
        {
            dataGridView1.Columns.Add("Checked", "勾选");
            dataGridView1.Columns.Add("Operation", "操作");
            dataGridView1.Columns.Add(nameof(TemplateDetailViewModel.Id), nameof(TemplateDetailViewModel.Id));
            dataGridView1.Columns.Add(nameof(TemplateDetailViewModel.Name), nameof(TemplateDetailViewModel.Name));
            dataGridView1.Columns.Add(nameof(TemplateDetailViewModel.Type), "类型");
            dataGridView1.Columns.Add(nameof(TemplateDetailViewModel.TemplateStr), "XPath");
            dataGridView1.Columns.Add(nameof(TemplateDetailViewModel.LinkedSpiderId), nameof(TemplateDetailViewModel.LinkedSpiderId));
        }

        private void LoadForm()
        {
            dataGridView1.Rows.Clear();
            _templates.ForEach(x =>
            {
                var row = new DataGridViewRow();
                row.Cells.Add(new DataGridViewCheckBoxCell() { Value = _selected.Contains(x.Id), ValueType = typeof(bool) });
                row.Cells.Add(new DataGridViewButtonCell() { Value = "编辑", Tag = x });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.Id, ValueType = typeof(int) });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.Name });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.TypeName });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.TemplateStr });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = x.LinkedSpiderId });
                dataGridView1.Rows.Add(row);
            });
        }
        private async void TemplateManageForm_Load(object sender, EventArgs e)
        {
            PreLoadForm();
            await Task.Run(async () =>
            {
                await LoadData();
            });
            LoadForm();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            var selected = new List<int>();
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                var row = dataGridView1.Rows[i];
                if (row.Cells[0].Value != null && (bool)row.Cells[0].Value == true)
                    selected.Add((int)row.Cells[2].Value);
            }
            OnSelect?.Invoke(this, selected);
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                var btn = row.Cells[e.ColumnIndex];
                var form = new ContentConfigForm(_service, btn.Tag as TemplateDetailViewModel);
                form.OnSubmit += async (obj, evt) =>
                {
                    await Task.Run(async () =>
                    {
                        await LoadData();
                    });
                    LoadForm();
                };
                form.ShowDialog();
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var form = new ContentConfigForm(_service);
            form.OnSubmit += async (obj, evt) =>
            {
                await Task.Run(async () =>
                {
                    await LoadData();
                });
                LoadForm();
            };
            form.ShowDialog();
        }
    }
}
