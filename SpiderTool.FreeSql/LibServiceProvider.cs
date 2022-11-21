using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using SpiderTool.FreeSql.Domain;
using SpiderTool.Injection;

namespace SpiderTool.FreeSql
{
    public static class LibServiceProvider
    {
        public static IServiceCollection AddSpiderService(this IServiceCollection services, string? connectionStr, DataType dbType, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentException.ThrowIfNullOrEmpty(connectionStr);

            var instance = new FreeSqlBuilder().UseConnectionString(dbType, connectionStr).Build();
            instance.Aop.ConfigEntityProperty += (s, e) =>
            {
                if (e.Property.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), false).Any())
                {
                    e.ModifyResult.IsPrimary = true;
                    e.ModifyResult.IsIdentity = true;
                }
            };
            services.Add(new ServiceDescriptor(typeof(IFreeSql), e => instance, serviceLifetime));
            DbMigration.CreateDatabase(instance, dbType.ToString());

            services.AddSpiderDomain<SpiderDomain>(serviceLifetime);
            services.AddTemplateDomain<TemplateDomain>(serviceLifetime);
            services.AddTaskDomain<TaskDomain>(serviceLifetime);

            services.AddSpiderService<FreeSqlSpiderService>(serviceLifetime);
            return services;
        }
    }

}
