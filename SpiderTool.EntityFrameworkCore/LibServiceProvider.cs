using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpiderTool.EntityFrameworkCore.ContextModel;
using SpiderTool.EntityFrameworkCore.Domain;
using SpiderTool.Injection;

namespace SpiderTool.EntityFrameworkCore
{
    public static class LibServiceProvider
    {
        public static IServiceCollection AddSpiderService(this IServiceCollection services, Action<DbContextOptionsBuilder> options, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddDbContext<SpiderDbContext>(options, serviceLifetime);

            services.AddSpiderDomain<SpiderDomain>(serviceLifetime);
            services.AddTemplateDomain<TemplateDomain>(serviceLifetime);
            services.AddTaskDomain<TaskDomain>(serviceLifetime);

            services.AddSpiderService<EFSpiderService>(serviceLifetime);
            return services;
        }
    }
}
