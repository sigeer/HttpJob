using AutoMapper;
using Grpc.Net.Client;
using SpiderRemoteServiceClient.Mapper.Spiders;
using SpiderRemoteServiceClient.Services;
using SpiderTool.Data;
using SpiderTool.IService;
using SpiderWin.Services;

namespace SpiderWin.Server
{
    public partial class ServerSetting : Form
    {
        ISpiderRemoteService? _service;

        public event EventHandler<ISpiderService>? OnChangeConnection;
        public ServerSetting()
        {
            InitializeComponent();
        }

        private void ServerSetting_Load(object sender, EventArgs e)
        {

        }

        private async void BtnTest_Click(object sender, EventArgs e)
        {
            await Connect();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void BtnOk_Click(object sender, EventArgs e)
        {
            await Connect(() =>
            {
                Close();
            });
        }

        private async Task Connect(Action? okCallBack = null)
        {
            if (string.IsNullOrEmpty(TxtServer.Text) || string.IsNullOrEmpty(TxtPort.Text))
            {
                MessageBox.Show("输入不正确");
                return;
            }
            ServiceFactory.GrpcDisconnect();
            ServiceFactory.GrpcChannel = GrpcChannel.ForAddress($"http://{TxtServer.Text}:{TxtPort.Text}");
            _service = new SpiderRemoteService(ServiceFactory.GrpcChannel, new Mapper(new MapperConfiguration(opt =>
            {
                opt.AddProfile<SpiderProtoProfile>();
            })), WorkerController.GetInstance());
            try
            {
                BtnOk.Enabled = false;
                BtnTest.Enabled = false;
                var data = await _service.Ping();
                if (data)
                {
                    OnChangeConnection?.Invoke(this, _service);
                    MessageBox.Show("连接成功");
                    okCallBack?.Invoke();
                }
                else
                    MessageBox.Show("连接失败");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                BtnOk.Enabled = true;
                BtnTest.Enabled = true;
            }
        }
    }
}
