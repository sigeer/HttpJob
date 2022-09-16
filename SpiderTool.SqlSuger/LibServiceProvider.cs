using Microsoft.Extensions.DependencyInjection;
using SpiderTool.Dto.Spider;
using SpiderTool.IDomain;
using SpiderTool.IService;
using SpiderTool.Service;
using SpiderTool.SqlSugar.Domain;
using SqlSugar;

namespace SpiderTool.SqlSugar
{
    public static class LibServiceProvider
    {
        public static IServiceCollection AddSpiderService(this IServiceCollection services, ISqlSugarClient sugarClient, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.Add(new ServiceDescriptor(typeof(ISqlSugarClient), e => sugarClient, serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(IResourceDomain), typeof(ResourceDomain), serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(IResourceDomain), typeof(ResourceDomain), serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(ISpiderDomain), typeof(SpiderDomain), serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(ITemplateDomain), typeof(TemplateDomain), serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(SpiderWorker), typeof(SpiderWorker), serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(ISpiderService), typeof(SpiderService), serviceLifetime));

            return services;
        }
    }
}
