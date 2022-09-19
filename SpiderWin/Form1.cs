using SpiderTool;
using SpiderTool.Dto.Spider;
using SpiderTool.IService;
using SpiderWin.Modals;

namespace SpiderWin
{
    public partial class Form1 : Form
    {
        readonly ISpiderService _coreService;
        List<SpiderDtoSetter> _spiderList = new List<SpiderDtoSetter>();
        public Form1(ISpiderService coreService)
        {
            _coreService = coreService;

            InitializeComponent();
        }

        private void btnShowConfig_Click(object sender, EventArgs e)
        {
            var selectedConfig = (dropConfig.SelectedItem as SpiderDtoSetter);
            var form = new SpiderConfigForm(selectedConfig?.Id ?? 0, _coreService);
            form.ShowDialog();
        }

        private void BindDropdown()
        {
            dropConfig.DisplayMember = nameof(SpiderDtoSetter.Name);
            dropConfig.ValueMember = nameof(SpiderDtoSetter.Id);
            dropConfig.DataSource = _spiderList;
        }

        private async void LoadData()
        {
            await Task.Run(() =>
            {
                _spiderList = _coreService.GetSpiderDtoList();
            });
            BindDropdown();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUrl.Text))
            {
                MessageBox.Show("«Î ‰»ÎURL");
                return;
            }
            var worker = new SpiderWorker(_coreService);
            _ = worker.Start(txtUrl.Text, (dropConfig.SelectedItem as SpiderDtoSetter)!.Id);
        }
    }
}