using System;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Win32;
using Newtonsoft.Json;
using NLog;
using NSW.StarCitizen.Tools.Lib.Global;

namespace NSW.StarCitizen.Tools.Settings
{
    public class AppSettings
    {
        private const string AppName = "Star Citizen Tools";
        private const string RegKeyAutoRun = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        [JsonProperty]
        public string? GameFolder { get; set; }
        [JsonProperty]
        public string Language
        {
            get => GetLanguage();
            set => SetLanguage(value);
        }
        [JsonProperty]
        public bool RunMinimized { get; set; }
        [JsonIgnore]
        public bool RunWithWindows
        {
            get => IsRunWithWindows();
            set => SetRunWithWindows(value);
        }
        [JsonProperty]
        public bool UseHttpProxy { get; set; }
        [JsonProperty]
        public bool TopMostWindow { get; set; } = true;
        [JsonProperty]
        public bool AcceptInstallWarning { get; set; }
        [JsonProperty]
        public UpdateSettings Update { get; } = new UpdateSettings();
        [JsonProperty]
        public LocalizationSettings Localization { get; } = new LocalizationSettings();
        [JsonProperty]
        public LocalizationSettings LocalizationPtu { get; } = new LocalizationSettings();
        [JsonProperty]
        public string? AuthToken { get; private set; }

        public LocalizationSettings GetGameModeSettings(GameMode gameMode) =>
            gameMode switch
            {
                GameMode.LIVE => Localization,
                GameMode.PTU => LocalizationPtu,
                _ => throw new NotSupportedException("Not supported game mode: " + gameMode)
            };

        private static string GetLanguage()
        {
            if (CultureInfo.DefaultThreadCurrentCulture != null)
                return CultureInfo.DefaultThreadCurrentCulture.Name;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InstalledUICulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InstalledUICulture;
            return CultureInfo.InstalledUICulture.Name;
        }

        private static void SetLanguage(string? cultureName)
        {
            if (cultureName != null)
            {
                try
                {
                    var culture = CultureInfo.CreateSpecificCulture(cultureName);
                    CultureInfo.DefaultThreadCurrentCulture = culture;
                    CultureInfo.DefaultThreadCurrentUICulture = culture;
                }
                catch (CultureNotFoundException e)
                {
                    _logger.Warn(e, $"Culture not found: {cultureName}. Using default");
                    CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InstalledUICulture;
                    CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InstalledUICulture;
                }
            }
        }

        private static bool IsRunWithWindows()
        {
            using var startupKey = Registry.CurrentUser.OpenSubKey(RegKeyAutoRun);
            return startupKey?.GetValue(AppName) != null;
        }

        private static void SetRunWithWindows(bool value)
        {
            using var startupKey = Registry.CurrentUser.OpenSubKey(RegKeyAutoRun, true);
            if (startupKey != null)
            {
                if (value)
                    startupKey.SetValue(AppName, Application.ExecutablePath);
                else
                    startupKey.DeleteValue(AppName, false);
            }
        }
    }
}