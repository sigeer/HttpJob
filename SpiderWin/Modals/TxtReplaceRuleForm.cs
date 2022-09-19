using SpiderTool.IService;

namespace SpiderWin.Modals
{
    public partial class TxtReplaceRuleForm : Form
    {
        int _flag = 0;
        public List<Control> extraControlList = new List<Control>();
        readonly ISpiderService _service;
        public TxtReplaceRuleForm(ISpiderService service)
        {
            _service = service;
            InitializeComponent();
        }
        private void SortExtraControls()
        {
            for (int i = 0; i < extraControlList.Count; i += 3)
            {
                var txtOld = extraControlList[i];
                var txtNew = extraControlList[i + 1];
                var btnDel = extraControlList[i + 2];
                txtOld.Location = new Point(30, 30 + i * 15);
                txtNew.Location = new Point(210, 30 + i * 15);
                btnDel.Location = new Point(410, 30 + i * 15);
            }
            BtnAdd.Location = new Point(30, 30 + (extraControlList.Count / 2) * 30);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var txtOld = new TextBox()
            {
                Name = $"txtOld{_flag}",
                Size = new System.Drawing.Size(180, 25)
            };
            var txtNew = new TextBox()
            {
                Name = $"txtNew{_flag}",
                Size = new System.Drawing.Size(180, 25)
            };
            var btnDel = new Button()
            {
                Name = $"btnDel{_flag}",
                Text = "删除",
                Size = new System.Drawing.Size(60, 25)
            };
            btnDel.Click += (sender, e) =>
            {
                extraControlList.Remove(txtOld);
                extraControlList.Remove(txtNew);
                extraControlList.Remove(btnDel);
                this.Controls.Remove(txtOld);
                this.Controls.Remove(txtNew);
                this.Controls.Remove(btnDel);
                SortExtraControls();
            };
            extraControlList.Add(txtOld);
            extraControlList.Add(txtNew);
            extraControlList.Add(btnDel);

            this.Controls.Add(txtOld);
            this.Controls.Add(txtNew);
            this.Controls.Add(btnDel);

            SortExtraControls();
            _flag++;
        }
    }
}
