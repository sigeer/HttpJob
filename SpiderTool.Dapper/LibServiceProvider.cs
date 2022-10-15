using Microsoft.Extensions.DependencyInjection;
using SpiderTool.Dapper.Domain;
using SpiderTool.IDomain;
using SpiderTool.IService;
using System.Data;

namespace SpiderTool.Dapper
{
    public static class LibServiceProvider
    {
        public static IServiceCollection AddSpiderService(this IServiceCollection services, IDbConnection dbConnection, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.Add(new ServiceDescriptor(typeof(IDbConnection), e => dbConnection, serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(ISpiderDomain), typeof(SpiderDomain), serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(ITemplateDomain), typeof(TemplateDomain), serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(ITaskDomain), typeof(TaskDomain), serviceLifetime));

            services.Add(new ServiceDescriptor(typeof(ISpiderService), typeof(DapperService), serviceLifetime));

            return services;
        }
    }
}
