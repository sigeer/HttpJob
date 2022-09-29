using AutoMapper;
using Grpc.Net.Client;
using SpiderRemoteServiceClient.Mapper.Spiders;
using SpiderRemoteServiceClient.Services;
using SpiderService;
using SpiderTool.IService;

namespace SpiderWin.Server
{
    public partial class ServerSetting : Form
    {
        GrpcChannel? currentChannel;
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
            if (currentChannel != null)
            {
                currentChannel.Dispose();
                currentChannel = null;
            }
            currentChannel = GrpcChannel.ForAddress($"http://{TxtServer.Text}:{TxtPort.Text}");
            var client = new SpiderWorkerProtoService.SpiderWorkerProtoServiceClient(currentChannel);
            _service = new SpiderRemoteService(client, new Mapper(new MapperConfiguration(opt =>
            {
                opt.AddProfile<SpiderProfile>();
            })));
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
