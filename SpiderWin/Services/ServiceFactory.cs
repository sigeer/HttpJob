using AutoMapper;
using Grpc.Net.Client;
using SpiderRemoteServiceClient.Mapper.Spiders;
using SpiderRemoteServiceClient.Services;
using SpiderService;
using SpiderTool.IService;

namespace SpiderWin.Services
{
    public class ServiceFactory
    {
        public static GrpcChannel? GrpcChannel { get; set; }

        public static void GrpcDisconnect()
        {
            if (GrpcChannel != null)
            {
                GrpcChannel.Dispose();
                GrpcChannel = null;
            }
        }
    }
}
