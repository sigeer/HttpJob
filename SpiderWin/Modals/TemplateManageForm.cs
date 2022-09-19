using SpiderTool.Dto.Spider;
using SpiderTool.IService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpiderWin.Modals
{
    public partial class TemplateManageForm : Form
    {
        readonly ISpiderService _service;

        List<TemplateDto> _templates = new List<TemplateDto>();
        public TemplateManageForm(ISpiderService service)
        {
            _service = service;
            InitializeComponent();
        }

        private async Task LoadData()
        {
            _templates = await _service.GetTemplateDtoListAsync();
        }

        private void LoadForm()
        {
            dataGridView1.DataSource = _templates;
            //dataGridView1.Columns.Add(nameof(TemplateDto.Id), nameof(TemplateDto.Id));
            //dataGridView1.Columns.Add(nameof(TemplateDto.Name), nameof(TemplateDto.Name));
            //dataGridView1.Columns.Add(nameof(TemplateDto.Type), nameof(TemplateDto.Type));
            //dataGridView1.Columns.Add(nameof(TemplateDto.TemplateStr), nameof(TemplateDto.TemplateStr));
            //dataGridView1.Columns.Add(nameof(TemplateDto.LinkedSpiderId), nameof(TemplateDto.LinkedSpiderId));
        }
        private async void TemplateManageForm_Load(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                await LoadData();
            });
            LoadForm();
        }
    }
}
