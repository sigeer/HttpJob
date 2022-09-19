using SpiderTool.Dto.Spider;
using SpiderTool.IService;

namespace SpiderWin.Modals
{
    public partial class ContentConfigForm : Form
    {
        readonly ISpiderService _service;
        List<ReplacementRuleDto> replacements = new List<ReplacementRuleDto>();
        public ContentConfigForm(ISpiderService service)
        {
            _service = service;

            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {

        }

        private void btnReplaceRule_Click(object sender, EventArgs e)
        {
            var txtReplaceRuleForm = new TxtReplaceRuleForm(_service);
            var dialogResult = txtReplaceRuleForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                for (int i = 0; i < txtReplaceRuleForm.extraControlList.Count; i += 3)
                {
                    replacements.Add(new ReplacementRuleDto
                    {
                        ReplacementOldStr = txtReplaceRuleForm.extraControlList[i].Text,
                        ReplacementNewlyStr = txtReplaceRuleForm.extraControlList[i + 1].Text
                    });
                }
            }
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
                Name = txtName.Text,
                TemplateStr = txtXPath.Text,
                Type = (int)(comboType.SelectedItem),
                LinkedSpiderId = (int)(comboType.SelectedItem),
                ReplacementRules = replacements
            });
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
