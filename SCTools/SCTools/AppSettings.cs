using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Win32;
using Newtonsoft.Json;
using NSW.StarCitizen.Tools.Services;

namespace NSW.StarCitizen.Tools
{
    public class AppSettings
    {
        private const string AppName = "Star Citizen Tools";
        private static RegistryKey StartupKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
        

        public string GameFolder { get; set; }
        public bool RunMinimized { get; set; } = false;
        public bool RunWithWindows
        {
            get => StartupKey.GetValue(AppName) != null;
            set
            {
                if (value)
                    StartupKey.SetValue(AppName, Application.ExecutablePath);
                else
                    StartupKey.DeleteValue(AppName, false);
            }
        }
        public LocalizationSettings Localization { get; set; } = new LocalizationSettings();
    }

    public class LocalizationSettingsInfo
    {
        public GameMode Mode { get; set; }
        public string LastVersion { get; set; }
        
    }

    public class LocalizationSettings
    {
        [JsonIgnore]
        //4032F680BFEE01
        public byte[] OriginalPattern { get; } = { 0x40, 0x32, 0xF6, 0x80, 0xBF, 0xEE, 0x01 };
        [JsonIgnore]
        // 90909080BFEE01
        public byte[] PatchPattern { get; } = { 0x90, 0x90, 0x90, 0x80, 0xBF, 0xEE, 0x01 };
        [JsonIgnore]
        public string Author { get; set; } = "defterai";
        [JsonIgnore]
        public string Repo { get; set; } = "StarCitizenModding";
        public List<LocalizationSettingsInfo> Infos { get; set; }
        public bool MonitorForUpdates { get; set; } = false;
        public int MonitorRefreshTime { get; set; } = 5;
    }
}