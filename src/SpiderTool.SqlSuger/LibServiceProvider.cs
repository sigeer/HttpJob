﻿using Microsoft.Extensions.DependencyInjection;
using SpiderTool.Injection;
using SpiderTool.SqlSugar.Domain;
using SqlSugar;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpiderTool.SqlSugar
{
    public static class LibServiceProvider
    {
        public static IServiceCollection AddSpiderService(this IServiceCollection services, ISqlSugarClient sugarClient, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.Add(new ServiceDescriptor(typeof(ISqlSugarClient), e => sugarClient, serviceLifetime));

            services.AddSpiderDomain<SpiderDomain>(serviceLifetime);
            services.AddTemplateDomain<TemplateDomain>(serviceLifetime);
            services.AddTaskDomain<TaskDomain>(serviceLifetime);

            services.AddSpiderService<SqlSugarSpiderService>(serviceLifetime);
            DbMigration.CreateDatabase(sugarClient);
            return services;
        }
    }

    public static class ExternalServiceDefaultBuilder
    {
        public static ConfigureExternalServices Build()
        {
            return new ConfigureExternalServices()
            {
                EntityService = (property, column) =>
                {
                    var attributes = property.GetCustomAttributes(true);//get all attributes 

                    if (attributes.Any(it => it is KeyAttribute))// by attribute set primarykey
                    {
                        column.IsPrimarykey = true; //有哪些特性可以看 1.2 特性明细
                        column.IsIdentity = true;
                    }
                },
                EntityNameService = (type, entity) =>
                {
                    var attributes = type.GetCustomAttributes(true);
                    if (attributes.Any(it => it is TableAttribute))
                    {
                        entity.DbTableName = (attributes.First(it => it is TableAttribute) as TableAttribute)!.Name;
                    }
                }
            };
        }
    }
}
