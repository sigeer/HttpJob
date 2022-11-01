using SpiderTool.Dto.Spider;
using SpiderTool.IService;
using Utility.Extensions;

namespace SpiderWin.Modals
{
    public partial class TxtReplaceRuleForm : Form
    {
        int _flag = 0;
        public List<Control> extraControlList = new List<Control>();

        List<ReplacementRuleDto> edittingModel;

        public event EventHandler<List<ReplacementRuleDto>>? OnOk;

        public TxtReplaceRuleForm(List<ReplacementRuleDto>? data = null)
        {
            edittingModel = data?.Clone() ?? new List<ReplacementRuleDto>();

            InitializeComponent();
        }

        private void TxtReplaceRuleForm_Load(object sender, EventArgs e)
        {
            UpdateUIByData();
        }

        private void UpdateUIByData()
        {
            edittingModel.ForEach(x =>
            {
                AddControl(x.ReplacementOldStr ?? "", x.ReplacementNewlyStr ?? "");
            });
            SortExtraControls();
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

        private void AddControl(string valOld = "", string valNew = "")
        {
            var txtOld = new TextBox()
            {
                Name = $"txtOld{_flag}",
                Size = new System.Drawing.Size(180, 25),
                Text = valOld
            };
            var txtNew = new TextBox()
            {
                Name = $"txtNew{_flag}",
                Size = new System.Drawing.Size(180, 25),
                Text = valNew
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

            _flag++;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            AddControl();
            SortExtraControls();

        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            var temp = new List<ReplacementRuleDto>();
            for (int i = 0; i < extraControlList.Count; i += 3)
            {
                temp.Add(new ReplacementRuleDto
                {
                    ReplacementOldStr = extraControlList[i].Text,
                    ReplacementNewlyStr = extraControlList[i + 1].Text
                });
            }
            edittingModel = temp;
            OnOk?.Invoke(this, edittingModel);
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
