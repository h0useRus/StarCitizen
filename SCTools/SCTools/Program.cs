using System;
using System.Threading;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Forms;
using NSW.StarCitizen.Tools.Helpers;

namespace NSW.StarCitizen.Tools
{
    public static partial class Program
    {
        // DO NOT CHANGE THIS STRING
        public const string ApplicationId = "07D391A3-45C7-4271-AAE5-F08D2A697850";
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (!SingleInstance.Start())
            {
                SingleInstance.ShowFirstInstance();
                return;
            }
            try
            {
                if (InstallUpdateOnLaunch(args))
                    return;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.ThreadException += Application_ThreadException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                Application.Run(new MainForm());
            }
            finally
            {
                SingleInstance.Stop();
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
        }
    }
}
