using AutoMapper;
using Grpc.Net.Client;
using SpiderRemoteServiceClient.Mapper.Spiders;
using SpiderRemoteServiceClient.Services;
using SpiderService;

namespace SpiderWin.Server
{
    public partial class ServerSetting : Form
    {
        GrpcChannel? _channel;
        public ServerSetting()
        {
            InitializeComponent();
        }

        private void ServerSetting_Load(object sender, EventArgs e)
        {

        }

        private async void BtnTest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtServer.Text) || string.IsNullOrEmpty(TxtPort.Text))
            {
                MessageBox.Show("输入不正确");
                return;
            }
            _channel = GrpcChannel.ForAddress($"http://{TxtServer.Text}:{TxtPort.Text}");
            var client = new SpiderWorkerProtoService.SpiderWorkerProtoServiceClient(_channel);
            var service = new SpiderRemoteService(client, new Mapper(new MapperConfiguration(opt =>
            {
                opt.AddProfile<SpiderProfile>();
            })));
            try
            {
                var data = await service.Ping();
                if (data)
                    MessageBox.Show("连接成功");
                else
                    MessageBox.Show("连接失败");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
