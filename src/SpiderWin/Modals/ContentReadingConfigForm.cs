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

        private void LoadConfigList()
        {
            Task.Run(async () =>
            {
                spiderList = await _service.GetSpiderDtoListAsync();

                Invoke(() =>
                {
                    comboSpider.DataSource = (new List<SpiderListItemViewModel>() { new SpiderListItemViewModel { Id = 0, Name = "" } }.Concat(spiderList)).ToList();

                    if (edittingModel.LinkedSpiderId > 0)
                        comboSpider.SelectedValue = edittingModel.LinkedSpiderId;
                });
            });
        }

        private void PreLoadForm()
        {
            comboType.ValueMember = nameof(TemplateType.Id);
            comboType.DisplayMember = nameof(TemplateType.Name);
            comboType.DataSource = types;

            comboSpider.DisplayMember = nameof(SpiderListItemViewModel.Name);
            comboSpider.ValueMember = nameof(SpiderListItemViewModel.Id);
        }

        private void LoadForm()
        {
            txtName.Text = edittingModel.Name;
            txtXPath.Text = edittingModel.TemplateStr;
            TxtAttribute.Text = edittingModel.ReadAttribute;
            comboType.SelectedValue = edittingModel.Type;

            HandleTypeChange();

            LoadConfigList();
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
                ReadAttribute = TxtAttribute.Text,
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
            LinkLabel_NewConfig.Visible = false;
        }

        private void ShowSelectSpider()
        {
            comboSpider.Visible = true;
            label3.Visible = true;
            LinkLabel_NewConfig.Visible = true;
        }

        private void ShowAttr()
        {
            TxtAttribute.Visible = true;
            labalAttr.Visible = true;
        }

        private void HideAttr()
        {
            TxtAttribute.Visible = false;
            labalAttr.Visible = false;
        }

        private void HandleTypeChange()
        {
            if (comboType.SelectedValue != null)
            {
                if ((int)comboType.SelectedValue == 4)
                    ShowSelectSpider();
                else
                    HideSelectSpider();

                if (new int[] { 3, 4 }.Contains((int)comboType.SelectedValue))
                    ShowAttr();
                else
                    HideAttr();
            }
        }

        private void ComboType_SelectedValueChanged(object sender, EventArgs e)
        {
            HandleTypeChange();
        }

        private void ContentConfigForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Modifiers == Keys.Control)
            {
                FormSubmit();
            }
        }

        private void LinkLabel_NewConfig_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new SpiderConfigForm(_service).ShowDialog();

            LoadConfigList();
        }
    }
}
