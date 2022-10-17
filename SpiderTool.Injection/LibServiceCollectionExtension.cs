using Microsoft.Extensions.DependencyInjection;
using SpiderTool.IDomain;
using SpiderTool.IService;

namespace SpiderTool.Injection
{
    public static class LibServiceCollectionExtension
    {
        public static IServiceCollection AddSpiderDomain<TImplemetation>(this IServiceCollection services, 
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) 
            where TImplemetation : ISpiderDomain
        {
            ArgumentNullException.ThrowIfNull(services);
            services.Add(new ServiceDescriptor(typeof(ISpiderDomain), typeof(TImplemetation), serviceLifetime));
            return services;
        }

        public static IServiceCollection AddTemplateDomain<TImplemetation>(this IServiceCollection services, 
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TImplemetation : ITemplateDomain
        {
            ArgumentNullException.ThrowIfNull(services);
            services.Add(new ServiceDescriptor(typeof(ITemplateDomain), typeof(TImplemetation), serviceLifetime));
            return services;
        }

        public static IServiceCollection AddTaskDomain<TImplemetation>(this IServiceCollection services, 
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TImplemetation : ITaskDomain
        {
            ArgumentNullException.ThrowIfNull(services);
            services.Add(new ServiceDescriptor(typeof(ITaskDomain), typeof(TImplemetation), serviceLifetime));
            return services;
        }

        public static IServiceCollection AddSpiderService<TImplemetation>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TImplemetation : ISpiderService
        {
            ArgumentNullException.ThrowIfNull(services);
            services.Add(new ServiceDescriptor(typeof(ISpiderService), typeof(TImplemetation), serviceLifetime));


            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            return services;
        }
    }
}