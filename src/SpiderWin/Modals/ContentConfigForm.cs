using SpiderTool.Data.Constants;
using SpiderTool.Data.Dto.Spider;
using SpiderTool.Data.IService;

namespace SpiderWin.Modals
{
    public partial class ContentConfigForm : Form
    {
        readonly ISpiderService _service;

        readonly TemplateEditDto edittingModel;

        readonly List<TemplateType> types = TemplateType.GetAll();
        List<SpiderListItemViewModel> spiderList = new List<SpiderListItemViewModel>();

        public event EventHandler<string>? OnSubmit;
        public ContentConfigForm(ISpiderService service, TemplateDetailViewModel? model = null)
        {
            _service = service;
            edittingModel = model?.ToEditModel() ?? new TemplateEditDto();

            InitializeComponent();

        }

        private void ContentConfigForm_Load(object sender, EventArgs e)
        {
            PreLoadForm();
            LoadForm();
        }

        private async Task LoadData()
        {
            await Task.Run(async () =>
            {
                spiderList = await _service.GetSpiderDtoListAsync();
            });
            comboSpider.DataSource = (new List<SpiderListItemViewModel>() { new SpiderListItemViewModel { Id = 0, Name = "" } }.Concat(spiderList)).ToList();
        }

        private void PreLoadForm()
        {
            comboType.ValueMember = nameof(TemplateType.Id);
            comboType.DisplayMember = nameof(TemplateType.Name);
            comboType.DataSource = types;

            comboSpider.DisplayMember = nameof(SpiderListItemViewModel.Name);
            comboSpider.ValueMember = nameof(SpiderListItemViewModel.Id);
        }

        private async void LoadForm()
        {
            txtName.Text = edittingModel.Name;
            txtXPath.Text = edittingModel.TemplateStr;
            comboType.SelectedValue = edittingModel.Type;

            if (comboType.SelectedValue != null && (int)comboType.SelectedValue == 4)
                ShowSelectSpider();
            else
                HideSelectSpider();

            await LoadData();

            if (edittingModel.LinkedSpiderId > 0)
                comboSpider.SelectedValue = edittingModel.LinkedSpiderId;

        }

        private void BtnReplaceRule_Click(object sender, EventArgs e)
        {
            var txtReplaceRuleForm = new TxtReplaceRuleManageForm(edittingModel.ReplacementRules);
            txtReplaceRuleForm.OnOk += (sender, evt) =>
            {
                edittingModel.ReplacementRules = evt;
            };
            txtReplaceRuleForm.ShowDialog();
        }

        private bool Valid()
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtXPath.Text) || comboType.SelectedValue == null)
                return false;
            return true;
        }

        private void FormSubmit()
        {
            if (!Valid())
            {
                MessageBox.Show("表单未完成");
                return;
            }
            var submitResult = _service.SubmitTemplate(new TemplateEditDto
            {
                Id = edittingModel.Id,
                Name = txtName.Text,
                TemplateStr = txtXPath.Text,
                Type = (int)(comboType.SelectedValue!),
                LinkedSpiderId = (int?)(comboSpider.SelectedValue),
                ReplacementRules = edittingModel.ReplacementRules
            });
            if (submitResult == StatusMessage.Success)
            {
                OnSubmit?.Invoke(this, submitResult);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
                MessageBox.Show(submitResult);

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            FormSubmit();
        }

        private void HideSelectSpider()
        {
            comboSpider.Visible = false;
            label3.Visible = false;
        }

        private void ShowSelectSpider()
        {
            comboSpider.Visible = true;
            label3.Visible = true;
        }

        private void ComboType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboType.SelectedValue != null)
            {
                if ((int)comboType.SelectedValue == 4)
                    ShowSelectSpider();
                else
                    HideSelectSpider();
            }

        }

        private void ContentConfigForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Modifiers == Keys.Control)
            {
                FormSubmit();
            }
        }
    }
}
