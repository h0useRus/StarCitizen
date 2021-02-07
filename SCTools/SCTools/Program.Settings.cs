using System.IO;
using NSW.StarCitizen.Tools.Lib.Helpers;
using NSW.StarCitizen.Tools.Settings;

namespace NSW.StarCitizen.Tools
{
    public static partial class Program
    {
        private const string AppSettingsFileName = "settings.json";

        private static AppSettings? _appSettings;
        public static AppSettings Settings => _appSettings ??= GetAppSettings();
        private static AppSettings GetAppSettings()
        {
            var appSettings = JsonHelper.ReadFile<AppSettings>(Path.Combine(ExecutableDir, AppSettingsFileName)) ?? new AppSettings();
            ValidateAppSettings(appSettings);
            return appSettings;
        }

        private static void ValidateAppSettings(AppSettings appSettings)
        {
            if (FixLocalizationSettings(appSettings.Localization) | FixLocalizationSettings(appSettings.LocalizationPtu))
            {
                SaveAppSettings(appSettings);
            }
        }

        public static bool SaveAppSettings() => SaveAppSettings(Settings);
        private static bool SaveAppSettings(AppSettings appSettings) =>
            JsonHelper.WriteFile(Path.Combine(ExecutableDir, AppSettingsFileName), appSettings);

        private static bool FixLocalizationSettings(LocalizationSettings settings)
        {
            if (settings.Repositories.Count == 0)
            {
                settings.Repositories.AddRange(LocalizationSource.DefaultList);
                return true;
            }
            return false;
        }
    }
}