﻿using SpiderTool.Constants;
using SpiderTool.Dto.Spider;
using SpiderTool.IService;
using Utility.Extensions;

namespace SpiderWin.Modals
{
    public partial class SpiderConfigForm : Form
    {
        #region form variables
        SpiderEditDto _currentSpider = new SpiderEditDto();
        int _spiderId;
        List<TemplateDetailViewModel> _templateList = new List<TemplateDetailViewModel>();

        public event EventHandler<string>? OnSubmit;
        #endregion

        #region services
        readonly ISpiderService _coreService;
        #endregion

        public SpiderConfigForm(ISpiderService coreService, SpiderListItemViewModel? spider = null)
        {
            _spiderId = spider?.Id ?? 0;
            _coreService = coreService;

            InitializeComponent();

            Text = $"{Text} - {_spiderId}";

        }
        private async void LoadTemplateListData()
        {
            await Task.Run(async () =>
            {
                _currentSpider = (await _coreService.GetSpiderAsync(_spiderId))?.ToEditModel() ?? new SpiderEditDto();
                _templateList = await _coreService.GetTemplateDtoListAsync();
            });

            TxtName.Text = _currentSpider.Name;
            TxtDescription.Text = _currentSpider.Description;
            TxtPostObj.Text = _currentSpider.PostObjStr;
            TextRequestHeaders.Text = _currentSpider.Headers;
            ComboMethod.SelectedItem = _currentSpider.Method;
            labelTemplateInfo.Text = $"已选择{_currentSpider.Templates.Count}项";

            ComboBoxNextPage.DataSource = (new List<TemplateDetailViewModel>() { new TemplateDetailViewModel() { Id = 0, Name = "" } }.Concat(_templateList)).ToList();
            if (_currentSpider.NextPageTemplateId != null)
                ComboBoxNextPage.SelectedValue = _currentSpider.NextPageTemplateId;
        }

        private void PreLoadForm()
        {
            ComboBoxNextPage.ValueMember = nameof(TemplateDetailViewModel.Id);
            ComboBoxNextPage.DisplayMember = nameof(TemplateDetailViewModel.Name);

        }

        private void SpiderConfigForm_Load(object sender, EventArgs e)
        {
            PreLoadForm();

            LoadTemplateListData();
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
            Task.Run(async () =>
            {
                _templateList = await _coreService.GetTemplateDtoListAsync();
            });
        }
    }
}
