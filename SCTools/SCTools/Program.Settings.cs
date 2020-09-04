using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Helpers;
using NSW.StarCitizen.Tools.Settings;

namespace NSW.StarCitizen.Tools
{
    public static partial class Program
    {
        private static readonly string _appSettingsFileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "settings.json");
       
        private static AppSettings? _appSettings;
        public static AppSettings Settings => _appSettings ??= GetAppSettings();
        private static AppSettings GetAppSettings()
        {
            var appSettings = JsonHelper.ReadFile<AppSettings>(_appSettingsFileName) ?? new AppSettings();
            ValidateAppSettings(appSettings);
            return appSettings;
        }
        private static void ValidateAppSettings(AppSettings appSettings)
        {
            if (appSettings.Localization.Repositories.Count > 0) return;
            appSettings.Localization.Repositories = new List<LocalizationSource>(LocalizationSource.DefaultList);
            SaveAppSettings(appSettings);
        }
        public static bool SaveAppSettings() => SaveAppSettings(Settings);
        private static bool SaveAppSettings(AppSettings appSettings) => JsonHelper.WriteFile(_appSettingsFileName, appSettings);
    }
}