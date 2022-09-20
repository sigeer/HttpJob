using SpiderTool.Dto.Spider;
using SpiderTool.IService;
using Utility.Extensions;

namespace SpiderWin.Modals
{
    public partial class ContentConfigForm : Form
    {
        readonly ISpiderService _service;

        TemplateDto backup;
        TemplateDto edittingModel;

        List<TemplateType> types = TemplateType.GetAll();
        List<SpiderDtoSetter> spiderList = new List<SpiderDtoSetter>();
        public ContentConfigForm(ISpiderService service, TemplateDto? model = null)
        {
            _service = service;
            backup = model ?? new TemplateDto();
            edittingModel = backup.Clone();

            InitializeComponent();

        }

        private async void ContentConfigForm_Load(object sender, EventArgs e)
        {
            PreLoadForm();
            await Task.Run(() =>
            {
                LoadData();
            });
            LoadForm();
        }

        private void LoadData()
        {
            spiderList = _service.GetSpiderDtoList();
        }

        private void PreLoadForm()
        {
            comboType.ValueMember = nameof(TemplateType.Id);
            comboType.DisplayMember = nameof(TemplateType.Name);

            comboSpider.DisplayMember = nameof(SpiderDtoSetter.Name);
            comboSpider.ValueMember = nameof(SpiderDtoSetter.Id);

            txtName.Text = edittingModel.Name;
            txtXPath.Text = edittingModel.TemplateStr;
            comboType.SelectedValue = edittingModel.Type;

            if (comboType.SelectedValue != null && (int)comboType.SelectedValue == 4)
                ShowSelectSpider();
            else
                HideSelectSpider();

            if (edittingModel.LinkedSpiderId > 0)
                comboSpider.SelectedValue = edittingModel.LinkedSpiderId;

        }

        private void LoadForm()
        {
            comboType.DataSource = types;

            var ds = (new List<SpiderDtoSetter>() { new SpiderDtoSetter { Id = 0, Name = "" } }.Concat(spiderList)).ToList();
            comboSpider.DataSource = ds;

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
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtXPath.Text))
                return false;
            return true;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!Valid())
            {
                MessageBox.Show("表单未完成");
                return;
            }
            _service.SubmitTemplate(new TemplateDto
            {
                Id = edittingModel.Id,
                Name = txtName.Text,
                TemplateStr = txtXPath.Text,
                Type = (int)(comboType.SelectedValue),
                LinkedSpiderId = (int)(comboSpider.SelectedValue),
                ReplacementRules = edittingModel.ReplacementRules
            });
            DialogResult = DialogResult.OK;
            Close();
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
    }
}
