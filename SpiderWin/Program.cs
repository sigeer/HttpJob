using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using SpiderTool;
using SpiderTool.IDomain;
using SpiderTool.Injection;
using SpiderTool.IService;
using SpiderTool.MongoDB;
using SpiderTool.SqlSugar;
using SqlSugar;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SpiderWin
{
    internal static class Program
    {
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var instance = GetRunningInstance();
            if (instance != null)
            {
                ShowWindowAsync(instance.MainWindowHandle, 1);
                SetForegroundWindow(instance.MainWindowHandle);
                return;
            }
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();


            ServiceCollection services = new ServiceCollection();

            IConfigurationBuilder cfgBuilder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfiguration configuration = cfgBuilder.Build();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<Form1>();

            services.AddSpiderService(() => new MongoClient(configuration.GetConnectionString("MongoDB")), ServiceLifetime.Singleton);

            var serviceProvider = services.BuildServiceProvider();
            if (File.Exists("scripts.dll"))
            {
                var processorAssembly = Assembly.LoadFile("scripts.dll");
                var newlyService = processorAssembly.GetTypes().FirstOrDefault(x => x.GetInterfaces().Any(y => y.BaseType == typeof(ISpiderProcessor)));
                if (newlyService != null)
                    services.AddSingleton<ISpiderProcessor>(Activator.CreateInstance(newlyService) as ISpiderProcessor);
            }

            var remoteService = serviceProvider.GetService<ISpiderService>()!;
            if (!remoteService.CanConnect())
            {
                services.AddSpiderService(new SqlSugarScope(new ConnectionConfig
                {
                    ConnectionString = configuration.GetConnectionString("Sqlite"),
                    DbType = DbType.Sqlite,
                    ConfigureExternalServices = ExternalServiceDefaultBuilder.Build()
                }), ServiceLifetime.Singleton);
                serviceProvider = services.BuildServiceProvider();
            }

            Application.Run(serviceProvider.GetService<Form1>()!);
        }

        public static Process? GetRunningInstance()
        {
            var current = Process.GetCurrentProcess();
            var processes = Process.GetProcessesByName(current.ProcessName);

            foreach (var p in processes)
            {
                if (p.Id != current.Id)
                {
                    if (p.MainModule?.FileName == current.MainModule?.FileName)
                        return p;
                }
            }
            return null;
        }

    }
}