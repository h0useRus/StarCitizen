using System;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Forms;
using NSW.StarCitizen.Tools.Helpers;
using NSW.StarCitizen.Tools.Lib.Helpers;

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
            bool updateOutdatedLocalization = args.Length == 1 &&
                args[0] == LocalizationAppRegistry.UpdateOutdatedParam;
            if (!SingleInstance.Start())
            {
                SingleInstance.ShowFirstInstance(updateOutdatedLocalization);
                return;
            }
            try
            {
                InitLogging();
                if (AppUpdate.InstallUpdateOnLaunch(args))
                    return;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm(updateOutdatedLocalization));
            }
            finally
            {
                FreeLogging();
                SingleInstance.Stop();
            }
        }
    }
}
