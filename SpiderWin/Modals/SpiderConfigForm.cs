using SpiderTool.Constants;
using SpiderTool.Dto.Spider;
using SpiderTool.IService;
using Utility.Extensions;

namespace SpiderWin.Modals
{
    public partial class SpiderConfigForm : Form
    {
        #region form variables
        SpiderDtoSetter backup;
        SpiderDtoSetter _currentSpider;
        List<TemplateDto> _templateList = new List<TemplateDto>();
        #endregion

        #region services
        readonly ISpiderService _coreService;
        #endregion

        public SpiderConfigForm(ISpiderService coreService, SpiderDtoSetter? spider = null)
        {
            backup = spider ?? new SpiderDtoSetter();
            _currentSpider = backup.Clone();
            _coreService = coreService;

            InitializeComponent();

        }
        private async void LoadData()
        {
            _templateList = await _coreService.GetTemplateDtoListAsync();
        }

        private void LoadForm()
        {
            ComboBoxNextPage.DataSource = (new List<TemplateDto>() { new TemplateDto() { Id = 0, Name = "" } }.Concat(_templateList)).ToList();
            ComboBoxNextPage.ValueMember = nameof(TemplateDto.Id);
            ComboBoxNextPage.DisplayMember = nameof(TemplateDto.Name);

            if (_currentSpider != null)
            {
                TxtName.Text = _currentSpider.Name;
                TxtDescription.Text = _currentSpider.Description;
                TxtPostObj.Text = _currentSpider.PostObjStr;
                ComboMethod.SelectedItem = _currentSpider.Method;

                if (_currentSpider.NextPageTemplateId != null)
                    ComboBoxNextPage.SelectedValue = _currentSpider.NextPageTemplateId;
            }
        }

        private async void SpiderConfigForm_Load(object sender, EventArgs e)
        {
            await Task.Run(() => LoadData());
            LoadForm();
        }

        private void btnAddContentReader_Click(object sender, EventArgs e)
        {
            var templateListForm = new TemplateManageForm(_coreService, _currentSpider.Templates);
            templateListForm.OnOk += (data, evt) =>
            {
                if (evt != null)
                    _currentSpider.Templates = (evt as List<int>)!;
            };
            templateListForm.ShowDialog();
        }

        private void SpiderConfigForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Modifiers == Keys.Control)
            {
                MessageBox.Show("ctrl + s");
            }
        }

        private void TemplateListMenu_Click(object sender, EventArgs e)
        {
            new TemplateManageForm(_coreService).ShowDialog();
        }

        private void FormDisabled()
        {
            BtnSubmit.Enabled = false;
            BtnCancel.Enabled = false;
        }
        private void FormEnabled()
        {
            BtnSubmit.Enabled = true;
            BtnCancel.Enabled = true;
        }

        private async void BtnSubmit_Click(object sender, EventArgs e)
        {
            FormDisabled();
            _currentSpider.Name = TxtName.Text;
            _currentSpider.Description = TxtDescription.Text;
            _currentSpider.PostObjStr = TxtPostObj.Text;
            _currentSpider.Method = ComboMethod.Text;
            _currentSpider.NextPageTemplateId = (int)ComboBoxNextPage.SelectedValue;
            var submitResult = await Task.Run(() => _coreService.SubmitSpider(_currentSpider));
            if (submitResult != StatusMessage.Success)
                MessageBox.Show(submitResult);
            FormEnabled();
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
