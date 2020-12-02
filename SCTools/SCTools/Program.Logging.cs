using System;
using System.Threading;
using System.Windows.Forms;
using NLog;
using NLog.Config;
using NSW.StarCitizen.Tools.Properties;

namespace NSW.StarCitizen.Tools
{
    public static partial class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static void InitLogging()
        {
            Application.ThreadException += OnThreadException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            LogManager.AutoShutdown = true;
#if DEBUG
            LogManager.ThrowConfigExceptions = true;
            LogManager.GlobalThreshold = LogLevel.Trace;
#else
            LogManager.GlobalThreshold = LogLevel.Info;
#endif
            LogManager.Configuration = XmlLoggingConfiguration.CreateFromXmlString(Resources.LoggingConfig);
            _logger.Info("Logging initialized");
        }

        public static void FreeLogging()
        {
            _logger.Info("Logging stopped");
            Application.ThreadException -= OnThreadException;
            AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
            LogManager.Flush();
            LogManager.Shutdown();
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception exception) 
                _logger.Fatal(exception, "Unhandled exception");
            else
                _logger.Fatal($"Unhandled exception: {e.ExceptionObject}");
            LogManager.Flush();
        }

        private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            _logger.Fatal(e.Exception, "Thread exception");
            LogManager.Flush();
        }
    }
}
