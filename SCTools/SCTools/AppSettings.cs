using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Win32;
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
        public List<LocalizationSource> SupportedSources { get; set; }
    }

    public class LocalizationSettingsInfo
    {
        public GameMode Mode { get; set; }
        public string LastVersion { get; set; }
        public string Source { get; set; }
    }

    public enum RepositoryType
    {
        Unknown,
        GitHub
    }

    public class LocalizationSource
    {
        public string Name { get; set; }
        public string Repository { get; set; }
        public RepositoryType RepositoryType { get; set; }
        public static LocalizationSource Default { get; } = new LocalizationSource
        {
            Name = "Russian Community", Repository = "defterai/StarCitizenModding", RepositoryType = RepositoryType.GitHub
        };
    }

    public class LocalizationSettings
    {
        public List<LocalizationSettingsInfo> Infos { get; set; }
        public bool MonitorForUpdates { get; set; } = false;
        public int MonitorRefreshTime { get; set; } = 5;
    }
}