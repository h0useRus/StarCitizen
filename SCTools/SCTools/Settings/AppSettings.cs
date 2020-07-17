using System.Windows.Forms;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace NSW.StarCitizen.Tools.Settings
{
    public class AppSettings
    {
        private const string AppName = "Star Citizen Tools";
        private static readonly RegistryKey _startupKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
        
        public string GameFolder { get; set; }
        public bool RunMinimized { get; set; } = false;
        [JsonIgnore]
        public bool RunWithWindows
        {
            get => _startupKey.GetValue(AppName) != null;
            set
            {
                if (value)
                    _startupKey.SetValue(AppName, Application.ExecutablePath);
                else
                    _startupKey.DeleteValue(AppName, false);
            }
        }
        public LocalizationSettings Localization { get; set; } = new LocalizationSettings();
    }
}