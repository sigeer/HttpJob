using SpiderTool.Dto.Spider;
using SpiderTool.IService;

namespace SpiderWin.Modals
{
    public partial class SpiderConfigForm : Form
    {
        readonly int _configId;
        readonly ISpiderService _coreService;
        List<Control> contentConfigControls = new List<Control>();
        private List<TemplateDto> _templateList = new List<TemplateDto>();
        private int _flag = 0;
        public SpiderConfigForm(int configId, ISpiderService coreService)
        {
            _configId = configId;
            _coreService = coreService;

            InitializeComponent();

        }

        private async void LoadData()
        {
            _templateList = await _coreService.GetTemplateDtoListAsync();

            this.dropdownContentReader.DisplayMember = nameof(TemplateDto.Name);
            this.dropdownContentReader.ValueMember = nameof(TemplateDto.Name);
            this.dropdownContentReader.DataSource = _templateList;
        }

        private void SpiderConfigForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnAddContentReader_Click(object sender, EventArgs e)
        {
            var btnFlag = new Button()
            {
                Name = $"btn{_flag}",
                Text = "删除",
                Location = new System.Drawing.Point(400, 260 + (contentConfigControls.Count / 2) * 30),
                Size = new System.Drawing.Size(75, 25)
            };
            var txtFlag = new ComboBox()
            {
                Name = $"txt{_flag}",
                Location = new System.Drawing.Point(90, 260 + (contentConfigControls.Count / 2) * 30),
                Size = new System.Drawing.Size(275, 25),
                DisplayMember = nameof(TemplateDto.Name),
                ValueMember = nameof(TemplateDto.Id),
                DataSource = _templateList
            };
            btnFlag.Click += (sender, e) =>
            {
                contentConfigControls.Remove(btnFlag);
                contentConfigControls.Remove(txtFlag);
                this.Controls.Remove(btnFlag);
                this.Controls.Remove(txtFlag);
                this.btnAddContentReader.Location = new Point(90, 260 + (contentConfigControls.Count / 2) * 30);
            };
            contentConfigControls.Add(txtFlag);
            contentConfigControls.Add(btnFlag);
            this.Controls.Add(txtFlag);
            this.Controls.Add(btnFlag);
            this.btnAddContentReader.Location = new Point(90, 260 + (contentConfigControls.Count / 2) * 30);
            _flag++;
        }
    }
}
