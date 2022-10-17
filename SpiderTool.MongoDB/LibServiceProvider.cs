using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using SpiderTool.Data.Mapper;
using SpiderTool.Injection;
using SpiderTool.MongoDB.Domain;
using Utility.GuidHelper;

namespace SpiderTool.MongoDB
{
    public static class LibServiceProvider
    {
        public static IServiceCollection AddSpiderService(this IServiceCollection services, Func<MongoClient> options, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            ArgumentNullException.ThrowIfNull(services);
            services.AddSpiderAutoMapper();

            services.Add(new ServiceDescriptor(typeof(IMongoClient), x => options(), serviceLifetime));

            services.AddSpiderDomain<SpiderDomain>(serviceLifetime);
            services.AddTemplateDomain<TemplateDomain>(serviceLifetime);
            services.AddTaskDomain<TaskDomain>(serviceLifetime);

            services.AddSpiderService<MongoSpiderService>(serviceLifetime);
            return services;
        }
    }
}