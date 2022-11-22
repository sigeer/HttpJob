using Microsoft.Extensions.DependencyInjection;
using SpiderTool.Data;
using SpiderTool.Data.IService;
using SpiderTool.Data.Mapper;
using SpiderTool.IDomain;

namespace SpiderTool.Injection
{
    public static class LibServiceCollectionExtension
    {
        public static IServiceCollection AddService<TType, TImplemetation>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TImplemetation : class, TType
        {
            ArgumentNullException.ThrowIfNull(services);
            services.Add(new ServiceDescriptor(typeof(TType), typeof(TImplemetation), serviceLifetime));
            return services;
        }
        public static IServiceCollection AddSpiderDomain<TImplemetation>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TImplemetation : class, ISpiderDomain
        {
            return services.AddService<ISpiderDomain, TImplemetation>(serviceLifetime);
        }

        public static IServiceCollection AddTemplateDomain<TImplemetation>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TImplemetation : class, ITemplateDomain
        {
            return services.AddService<ITemplateDomain, TImplemetation>(serviceLifetime);
        }

        public static IServiceCollection AddTaskDomain<TImplemetation>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TImplemetation : class, ITaskDomain
        {
            return services.AddService<ITaskDomain, TImplemetation>(serviceLifetime);
        }


        public static IServiceCollection AddSpiderService<TImplemetation>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TImplemetation : class, ISpiderService
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            services.AddSingleton<WorkerController>(WorkerController.GetInstance());
            return services.AddService<ISpiderService, TImplemetation>(serviceLifetime);
        }

        public static IServiceCollection AddSpiderAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(x =>
            {
                x.AddProfile<SpiderProfile>();
            });
            return services;
        }
    }
}