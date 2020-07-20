using System;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Forms;

namespace NSW.StarCitizen.Tools
{
    public static partial class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
