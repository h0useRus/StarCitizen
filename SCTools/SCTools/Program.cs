using System;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Forms;
using NSW.StarCitizen.Tools.Helpers;

namespace NSW.StarCitizen.Tools
{
    public static partial class Program
    {
        // DO NOT CHANGE THIS STRING
        private const string ApplicationId = "07D391A3-45C7-4271-AAE5-F08D2A697850";
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            using var singleInstance = new SingleInstance(ApplicationId);
            if (!singleInstance.Start(acrossAllUsers: false))
            {
                singleInstance.ShowFirstInstance();
                return;
            }
            try
            {
                InitLogging();
                if (AppUpdate.InstallUpdateOnLaunch(args))
                    return;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                using var mainForm = new MainForm();
                Application.AddMessageFilter(new AppMessageFilter()
                {
                    ShowFirstInstanceMsg = singleInstance.ShowFirstInstanceMsg,
                    OnRestoreInstance = mainForm.Restore,
                });
                Application.Run(mainForm);
            }
            finally
            {
                FreeLogging();
                singleInstance.Stop();
            }
        }
    }
}
