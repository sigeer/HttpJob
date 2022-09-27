using SpiderTool.Constants;
using SpiderTool.Dto.Spider;
using SpiderTool.IService;
using Utility.Extensions;

namespace SpiderWin.Modals
{
    public partial class ContentConfigForm : Form
    {
        readonly ISpiderService _service;

        TemplateDto edittingModel;

        List<TemplateType> types = TemplateType.GetAll();
        List<SpiderDtoSetter> spiderList = new List<SpiderDtoSetter>();

        public event EventHandler<string>? OnSubmit;
        public ContentConfigForm(ISpiderService service, TemplateDto? model = null)
        {
            _service = service;
            edittingModel = model?.Clone() ?? new TemplateDto();

            InitializeComponent();

        }

        private void ContentConfigForm_Load(object sender, EventArgs e)
        {
            PreLoadForm();
            LoadForm();
        }

        private async void LoadData()
        {
            await Task.Run(() =>
            {
                spiderList = _service.GetSpiderDtoList();
            });
            comboSpider.DataSource = (new List<SpiderDtoSetter>() { new SpiderDtoSetter { Id = 0, Name = "" } }.Concat(spiderList)).ToList();
        }

        private void PreLoadForm()
        {
            comboType.ValueMember = nameof(TemplateType.Id);
            comboType.DisplayMember = nameof(TemplateType.Name);
            comboType.DataSource = types;

            comboSpider.DisplayMember = nameof(SpiderDtoSetter.Name);
            comboSpider.ValueMember = nameof(SpiderDtoSetter.Id);
        }

        private void LoadForm()
        {
            txtName.Text = edittingModel.Name;
            txtXPath.Text = edittingModel.TemplateStr;
            comboType.SelectedValue = edittingModel.Type;

            if (comboType.SelectedValue != null && (int)comboType.SelectedValue == 4)
                ShowSelectSpider();
            else
                HideSelectSpider();

            LoadData();

            if (edittingModel.LinkedSpiderId > 0)
                comboSpider.SelectedValue = edittingModel.LinkedSpiderId;

        }

        private void btnReplaceRule_Click(object sender, EventArgs e)
        {
            var txtReplaceRuleForm = new TxtReplaceRuleForm(_service, edittingModel.ReplacementRules);
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
            var submitResult = _service.SubmitTemplate(new TemplateDto
            {
                Id = edittingModel.Id,
                Name = txtName.Text,
                TemplateStr = txtXPath.Text,
                Type = (int)(comboType.SelectedValue),
                LinkedSpiderId = (int)(comboSpider.SelectedValue),
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

        private void comboType_SelectedValueChanged(object sender, EventArgs e)
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
