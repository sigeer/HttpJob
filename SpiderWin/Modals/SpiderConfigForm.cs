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

        public event EventHandler? OnSubmit;
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

            Text = $"{Text} - {_currentSpider.Id}";

        }
        private async void LoadData()
        {
            _templateList = await _coreService.GetTemplateDtoListAsync();
        }

        private void PreLoadForm()
        {
            ComboBoxNextPage.ValueMember = nameof(TemplateDto.Id);
            ComboBoxNextPage.DisplayMember = nameof(TemplateDto.Name);

            labelTemplateInfo.Text = $"已选择{_currentSpider.Templates.Count}项";
        }

        private void LoadForm()
        {
            ComboBoxNextPage.DataSource = (new List<TemplateDto>() { new TemplateDto() { Id = 0, Name = "" } }.Concat(_templateList)).ToList();


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
            PreLoadForm();
            await Task.Run(() => LoadData());
            LoadForm();
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
            else
                OnSubmit?.Invoke(this, e);
            FormEnabled();
            Close();
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
    }
}
