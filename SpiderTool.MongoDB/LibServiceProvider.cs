using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using SpiderTool.Data.Mapper;
using SpiderTool.IDomain;
using SpiderTool.IService;
using SpiderTool.MongoDB.Domain;
using Utility.GuidHelper;

namespace SpiderTool.MongoDB
{
    public static class LibServiceProvider
    {
        public static IServiceCollection AddSpiderService(this IServiceCollection services, Func<MongoClient> options, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            ArgumentNullException.ThrowIfNull(services);
            services.AddAutoMapper(x =>
            {
                x.AddProfile<SpiderProfile>();
            });
            services.AddSingleton<Snowflake>(s => Snowflake.GetInstance(1));


            services.Add(new ServiceDescriptor(typeof(IMongoClient), x => options(), serviceLifetime));

            services.Add(new ServiceDescriptor(typeof(ISpiderDomain), typeof(SpiderDomain), serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(ITemplateDomain), typeof(TemplateDomain), serviceLifetime));
            services.Add(new ServiceDescriptor(typeof(ITaskDomain), typeof(TaskDomain), serviceLifetime));

            services.Add(new ServiceDescriptor(typeof(ISpiderService), typeof(MongoSpiderService), serviceLifetime));
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            return services;
        }
    }
}