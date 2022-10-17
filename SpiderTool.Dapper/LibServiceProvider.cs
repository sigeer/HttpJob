using Microsoft.Extensions.DependencyInjection;
using SpiderTool.Dapper.Domain;
using SpiderTool.Injection;
using System.Data;

namespace SpiderTool.Dapper
{
    public static class LibServiceProvider
    {
        public static IServiceCollection AddSpiderService(this IServiceCollection services, IDbConnection dbConnection, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.Add(new ServiceDescriptor(typeof(IDbConnection), e => dbConnection, serviceLifetime));

            services.AddSpiderDomain<SpiderDomain>(serviceLifetime);
            services.AddTemplateDomain<TemplateDomain>(serviceLifetime);
            services.AddTaskDomain<TaskDomain>(serviceLifetime);

            services.AddSpiderService<DapperService>(serviceLifetime);

            return services;
        }
    }
}
