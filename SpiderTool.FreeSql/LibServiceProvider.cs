using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using SpiderTool.FreeSql.Domain;
using SpiderTool.Injection;

namespace SpiderTool.FreeSql
{
    public static class LibServiceProvider
    {
        public static IServiceCollection AddSpiderService(this IServiceCollection services, Func<FreeSqlBuilder> builder, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            ArgumentNullException.ThrowIfNull(services);

            var instance = builder().Build();
            services.Add(new ServiceDescriptor(typeof(IFreeSql), e => instance, serviceLifetime));

            services.AddSpiderDomain<SpiderDomain>(serviceLifetime);
            services.AddTemplateDomain<TemplateDomain>(serviceLifetime);
            services.AddTaskDomain<TaskDomain>(serviceLifetime);

            services.AddSpiderService<FreeSqlSpiderService>(serviceLifetime);
            return services;
        }
    }

}
