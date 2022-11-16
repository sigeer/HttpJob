using FreeSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Serilog;
using SpiderTool;
using SpiderTool.FreeSql;
using SpiderTool.Injection;
using SpiderTool.IService;
using SpiderTool.MongoDB;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Utility.Extensions;
using Utility.Serilog.Extension;

namespace SpiderWin
{
    internal static class Program
    {
        static Serilog.ILogger _logger = null!;
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

            _logger = new LoggerConfiguration().CustomWriteTo().CreateLogger();
            services.AddLogging(builder =>
            {
                builder.AddSerilog(logger: _logger, dispose: true);
            });
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<Form1>();

            services.AddSpiderService(() => new MongoClient(configuration.GetConnectionString("MongoDB")), ServiceLifetime.Singleton);

            var serviceProvider = services.BuildServiceProvider();
            if (File.Exists("scripts.dll"))
            {
                var processorAssembly = Assembly.LoadFile("scripts.dll");
                var newlyService = processorAssembly.GetTypes().FirstOrDefault(x => x.GetInterfaces().Contains(typeof(ISpiderProcessor)));
                if (newlyService != null)
                    services.AddSingleton<ISpiderProcessor>((Activator.CreateInstance(newlyService) as ISpiderProcessor)!);
            }
            var logger = serviceProvider.GetService<ILogger<Application>>()!;
            var remoteService = serviceProvider.GetService<ISpiderService>()!;
            if (configuration["DirectUseSqlite"].ToStrictBoolean() || !remoteService.CanConnect())
            {
                services.AddSpiderService(configuration.GetConnectionString("Sqlite"), DataType.Sqlite, ServiceLifetime.Singleton);
                serviceProvider = services.BuildServiceProvider();
            }

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            var form = serviceProvider.GetService<Form1>()!;
            form.Show();
            Application.Run(form);
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs ex)
        {
            _logger.Error(ex.Exception.ToString());
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs ex)
        {
            _logger.Error(ex.ExceptionObject.ToString());
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