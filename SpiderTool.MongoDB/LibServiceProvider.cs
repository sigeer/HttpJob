using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using SpiderTool.IDomain;
using SpiderTool.IService;
using SpiderTool.MongoDB.Domain;
using SpiderTool.Service;

namespace SpiderTool.MongoDB
{
    public static class LibServiceProvider
    {
        public static IServiceCollection AddSpiderService(this IServiceCollection services, Func<MongoClient> options, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.Add(new ServiceDescriptor(typeof(IMongoClient), x => options(), serviceLifetime));

            services.Add(new ServiceDescriptor(typeof(ISpiderDomain), typeof(SpiderDomain), serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(ITemplateDomain), typeof(TemplateDomain), serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(ITaskDomain), typeof(TaskDomain), serviceLifetime));

            services.Add(new ServiceDescriptor(typeof(ISpiderService), typeof(SpiderService), serviceLifetime));
            return services;
        }
    }
}