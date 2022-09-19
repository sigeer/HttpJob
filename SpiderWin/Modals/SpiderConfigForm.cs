using SpiderTool.Dto.Spider;
using SpiderTool.IService;

namespace SpiderWin.Modals
{
    public partial class SpiderConfigForm : Form
    {
        readonly int _spiderId;
        SpiderDto? _currentSpider;
        readonly ISpiderService _coreService;
        List<Control> contentConfigControls = new List<Control>();
        private List<TemplateDto> _templateList = new List<TemplateDto>();
        private int _flag = 0;
        public SpiderConfigForm(int configId, ISpiderService coreService)
        {
            _spiderId = configId;
            _coreService = coreService;

            InitializeComponent();

        }
        private async void LoadData()
        {
            _templateList = await _coreService.GetTemplateDtoListAsync();
            _currentSpider = _coreService.GetSpider(_spiderId);

        }

        private void LoadForm()
        {
            ComboBoxNextPage.DataSource = _templateList;
            ComboBoxNextPage.ValueMember = nameof(TemplateDto.Id);
            ComboBoxNextPage.DisplayMember = nameof(TemplateDto.TemplateStr);

            if (_currentSpider != null)
            {
                TxtName.Text = _currentSpider.Name;
                TxtDescription.Text = _currentSpider.Description;
                TxtPostObj.Text = _currentSpider.PostObjStr;
                ComboMethod.SelectedText = _currentSpider.Method;

                if (_currentSpider.NextPageTemplate != null)
                    ComboBoxNextPage.SelectedValue = _currentSpider.NextPageTemplate.Id;

                for (int i = 0; i < _currentSpider.TemplateList.Count; i++)
                {
                    AddControlGroup(_currentSpider.TemplateList[i].Id);
                }
                SortExtraControls();
            }
        }

        private void SortExtraControls()
        {
            for (int i = 0; i < contentConfigControls.Count; i += 2)
            {
                var btn = contentConfigControls[i];
                var txt = contentConfigControls[i + 1];
                btn.Location = new Point(400, 280 + i * 15);
                txt.Location = new Point(90, 280 + i * 15);
            }
            btnAddContentReader.Location = new Point(90, 280 + (contentConfigControls.Count / 2) * 30);
        }

        private async void SpiderConfigForm_Load(object sender, EventArgs e)
        {
            await Task.Run(() => LoadData());
            LoadForm();
        }

        private void AddControlGroup(int defaultSelectValue = 0)
        {
            var btnFlag = new Button()
            {
                Name = $"btn{_flag}",
                Text = "删除",
                Size = new System.Drawing.Size(75, 25)
            };
            var txtFlag = new ComboBox()
            {
                Name = $"txt{_flag}",
                Size = new System.Drawing.Size(275, 25),
                DisplayMember = nameof(TemplateDto.Name),
                ValueMember = nameof(TemplateDto.Id),
                DataSource = _templateList
            };
            if (defaultSelectValue != 0)
                txtFlag.SelectedValue = defaultSelectValue;
            btnFlag.Click += (sender, e) =>
            {
                contentConfigControls.Remove(btnFlag);
                contentConfigControls.Remove(txtFlag);
                this.Controls.Remove(btnFlag);
                this.Controls.Remove(txtFlag);
                SortExtraControls();
            };
            contentConfigControls.Add(btnFlag);
            contentConfigControls.Add(txtFlag);

            this.Controls.Add(btnFlag);
            this.Controls.Add(txtFlag);
            _flag++;
        }

        private void btnAddContentReader_Click(object sender, EventArgs e)
        {
            AddControlGroup();
            SortExtraControls();
        }

        private void TemplateConfig_Click(object sender, EventArgs e)
        {
            new ContentConfigForm(_coreService).ShowDialog();
        }

        private void SpiderConfigForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.S && e.Modifiers == Keys.Control)
            {
                var templates = new List<int>();
                for (int i = 0; i < contentConfigControls.Count; i+=2)
                {
                    var combo = contentConfigControls[i + 1] as ComboBox;
                    templates.Add((int)combo!.SelectedValue);
                }
                _coreService.SubmitSpider(new SpiderDtoSetter
                {
                    Id = _spiderId,
                    Name = TxtName.Text,
                    Description = TxtDescription.Text,
                    PostObjStr = TxtPostObj.Text,
                    Method = ComboMethod.Text,
                    NextPageTemplateId = (int)ComboBoxNextPage.SelectedValue,
                    Templates = templates
                });
            }
        }

        private void TemplateListMenu_Click(object sender, EventArgs e)
        {
            new TemplateManageForm(_coreService).ShowDialog();
        }
    }
}
