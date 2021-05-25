using Microsoft.Win32;

namespace NSW.StarCitizen.Tools.Lib.Helpers
{
    public static class LocalizationAppRegistry
    {
        private const string RegKeyLocalization = @"SOFTWARE\StarCitizenLocalization";
        private const string DefaultApp = "DefaultApp";
        public const string UpdateOutdatedParam = "update_outdated";

        public static string? GetDefaultLocalizationApp()
        {
            using var localizationKey = Registry.CurrentUser.OpenSubKey(RegKeyLocalization);
            if (localizationKey != null)
            {
                var defaultAppValue = localizationKey.GetValue(DefaultApp);
                if (defaultAppValue is string defaultAppPath)
                {
                    return defaultAppPath;
                }
            }
            return null;
        }

        public static bool SetDefaultLocalizationApp(string? executablePath)
        {
            using var localizationKey = Registry.CurrentUser.CreateSubKey(RegKeyLocalization,
                RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (localizationKey != null)
            {
                if (executablePath != null)
                    localizationKey.SetValue(DefaultApp, executablePath);
                else
                    localizationKey.DeleteValue(DefaultApp);
                return true;
            }
            return false;
        }
    }
}
