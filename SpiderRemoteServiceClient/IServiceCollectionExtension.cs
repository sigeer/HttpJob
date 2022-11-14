using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using SpiderRemoteServiceClient.Mapper.Spiders;
using SpiderRemoteServiceClient.Services;
using SpiderService;
using SpiderTool.Data;

namespace SpiderRemoteServiceClient
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddSpiderService(this IServiceCollection services, string? serverUrl, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            ArgumentException.ThrowIfNullOrEmpty(serverUrl);

            var channel = GrpcChannel.ForAddress(serverUrl);
            services.AddSingleton<GrpcChannel>(channel);
            services.AddAutoMapper(x =>
            {
                x.AddProfile<SpiderProtoProfile>();
            });
            services.AddSingleton<WorkerController>(WorkerController.GetInstance());
            services.Add(new ServiceDescriptor(typeof(ISpiderRemoteService), typeof(SpiderRemoteService), serviceLifetime));

            return services;
        }
    }
}
