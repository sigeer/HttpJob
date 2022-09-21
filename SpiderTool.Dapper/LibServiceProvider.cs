using Microsoft.Extensions.DependencyInjection;
using SpiderTool.Dapper.Domain;
using SpiderTool.IDomain;
using SpiderTool.IService;
using SpiderTool.Service;
using SpiderTool.Tasks;
using System.Data;

namespace SpiderTool.Dapper
{
    public static class LibServiceProvider
    {
        public static IServiceCollection AddSpiderService(this IServiceCollection services, IDbConnection dbConnection, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.Add(new ServiceDescriptor(typeof(IDbConnection), e => dbConnection, serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(IResourceDomain), typeof(ResourceDomain), serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(ISpiderDomain), typeof(SpiderDomain), serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(ITemplateDomain), typeof(TemplateDomain), serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(ITaskDomain), typeof(TaskDomain), serviceLifetime));

            services.Add(new ServiceDescriptor(typeof(ISpiderService), typeof(SpiderService), serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(SpiderWorker), typeof(SpiderWorker), serviceLifetime));

            return services;
        }
    }
}
