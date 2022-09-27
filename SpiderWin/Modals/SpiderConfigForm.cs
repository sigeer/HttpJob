using SpiderTool.Constants;
using SpiderTool.Dto.Spider;
using SpiderTool.IService;
using Utility.Extensions;

namespace SpiderWin.Modals
{
    public partial class SpiderConfigForm : Form
    {
        #region form variables
        SpiderDtoSetter _currentSpider;
        List<TemplateDto> _templateList = new List<TemplateDto>();

        public event EventHandler<string>? OnSubmit;
        #endregion

        #region services
        readonly ISpiderService _coreService;
        #endregion

        public SpiderConfigForm(ISpiderService coreService, SpiderDtoSetter? spider = null)
        {
            _currentSpider = spider?.Clone() ?? new SpiderDtoSetter();
            _coreService = coreService;

            InitializeComponent();

            Text = $"{Text} - {_currentSpider.Id}";

        }
        private async void LoadTemplateListData()
        {
            await Task.Run(async () =>
            {
                _templateList = await _coreService.GetTemplateDtoListAsync();
            });           
            ComboBoxNextPage.DataSource = (new List<TemplateDto>() { new TemplateDto() { Id = 0, Name = "" } }.Concat(_templateList)).ToList();
        }

        private void PreLoadForm()
        {
            ComboBoxNextPage.ValueMember = nameof(TemplateDto.Id);
            ComboBoxNextPage.DisplayMember = nameof(TemplateDto.Name);

            labelTemplateInfo.Text = $"已选择{_currentSpider.Templates.Count}项";
        }

        private void LoadForm()
        {
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

        private void SpiderConfigForm_Load(object sender, EventArgs e)
        {
            PreLoadForm();

            LoadTemplateListData();
            LoadForm();
        }

        private void SpiderConfigForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Modifiers == Keys.Control)
            {
                FormSubmit();
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

        private async void FormSubmit()
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
            else
                OnSubmit?.Invoke(this, submitResult);
            FormEnabled();
            if (submitResult == StatusMessage.Success)
                Close();
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            FormSubmit();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ShowTemplateManagerForm()
        {
            var templateListForm = new TemplateManageForm(_coreService, _currentSpider.Templates);
            templateListForm.OnSelect += (data, evt) =>
            {
                if (evt != null)
                    _currentSpider.Templates = (evt as List<int>)!;

                labelTemplateInfo.Text = $"已选择{_currentSpider.Templates.Count}项";
            };
            templateListForm.ShowDialog();
        }

        private void btnAddContentReader_Click(object sender, EventArgs e)
        {
            ShowTemplateManagerForm();
        }


        private void labelTemplateInfo_Click(object sender, EventArgs e)
        {
            ShowTemplateManagerForm();
        }

        private void BtnRefreshTemplate_Click(object sender, EventArgs e)
        {
            Task.Run(() => LoadTemplateListData());
        }
    }
}
