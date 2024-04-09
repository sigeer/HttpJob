using FreeSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using SpiderTool;
using SpiderTool.FreeSql;
using SpiderTool.Injection;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
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

            var sink = new WinFormLogSink();

            _logger = new LoggerConfiguration()
                .CustomWriteTo()
                .WriteTo.Sink(sink)
                .CreateLogger();

            services.AddLogging(builder =>
            {
                builder.AddSerilog(logger: _logger, dispose: true);
            });
            services.AddSingleton<WinFormLogSink>(sink);
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<Form1>();

            services.AddSpiderService(configuration.GetConnectionString("Sqlite"), DataType.Sqlite, ServiceLifetime.Singleton);

            var serviceProvider = services.BuildServiceProvider();
            if (File.Exists("scripts.dll"))
            {
                var processorAssembly = Assembly.LoadFile("scripts.dll");
                var newlyService = processorAssembly.GetTypes().FirstOrDefault(x => x.GetInterfaces().Contains(typeof(ISpiderProcessor)));
                if (newlyService != null)
                    services.AddSingleton<ISpiderProcessor>((Activator.CreateInstance(newlyService) as ISpiderProcessor)!);
            }

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.ProcessExit += (s, e) =>
            {
                _logger.Information("程序关闭");
            };
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            _logger.Information("程序启动");
            var form = serviceProvider.GetService<Form1>()!;
            form.Show();
            Application.Run(form);
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs ex)
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