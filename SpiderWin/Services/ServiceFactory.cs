using Grpc.Net.Client;

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
