using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using SpiderTool.Dapper;

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
            services.AddSpiderService(new MySqlConnection(configuration.GetConnectionString("MySql")), ServiceLifetime.Transient);

            var serviceProvider = services.BuildServiceProvider();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            Application.Run(serviceProvider.GetService<Form1>()!);
        }

    }
}