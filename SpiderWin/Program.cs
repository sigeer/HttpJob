using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpiderRemoteServiceClient.Services;
using SpiderTool.SqlSugar;
using SqlSugar;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpiderWin
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();


            ServiceCollection services = new ServiceCollection();

            IConfigurationBuilder cfgBuilder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfiguration configuration = cfgBuilder.Build();
            services.AddSingleton<IConfiguration>(configuration);

            services.AddScoped<Form1>();
            var sqlClient = new SqlSugarScope(new ConnectionConfig
            {
                ConnectionString = "data source=database.db",
                DbType = DbType.Sqlite,
                ConfigureExternalServices = ExternalServiceDefaultBuilder.Build()
            });
            services.AddSpiderService(sqlClient, ServiceLifetime.Singleton);
            services.AddSingleton<ISpiderRemoteService, SpiderRemoteService>();

            var serviceProvider = services.BuildServiceProvider();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            sqlClient.CreateDatabase(DbType.Sqlite);
            Application.Run(serviceProvider.GetService<Form1>()!);
        }

    }
}