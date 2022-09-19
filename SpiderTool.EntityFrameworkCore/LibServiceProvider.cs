using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpiderTool.EntityFrameworkCore.ContextModel;
using SpiderTool.EntityFrameworkCore.Domain;
using SpiderTool.IDomain;
using SpiderTool.IService;
using SpiderTool.Service;

namespace SpiderTool.EntityFrameworkCore
{
    public static class LibServiceProvider
    {
        public static IServiceCollection AddSpiderService(this IServiceCollection services, Action<DbContextOptionsBuilder> options, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddDbContext<SpiderDbContext>(options, serviceLifetime);


            services.Add(new ServiceDescriptor(typeof(IResourceDomain), typeof(ResourceDomain), serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(ISpiderDomain), typeof(SpiderDomain), serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(ITemplateDomain), typeof(TemplateDomain), serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(ISpiderService), typeof(SpiderService), serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(SpiderWorker), typeof(SpiderWorker), serviceLifetime));
            return services;
        }
    }
}
