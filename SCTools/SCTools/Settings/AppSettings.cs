using System.Globalization;
using System.Windows.Forms;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace NSW.StarCitizen.Tools.Settings
{
    public class AppSettings
    {
        private const string AppName = "Star Citizen Tools";
        private const string RegKeyAutoRun = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        [JsonProperty]
        public string? GameFolder { get; set; }
        [JsonProperty]
        public string Language
        {
            get => GetLanguage();
            set => SetLanguage(value);
        }
        [JsonProperty]
        public bool RunMinimized { get; set; } = false;
        [JsonIgnore]
        public bool RunWithWindows
        {
            get => IsRunWithWindows();
            set => SetRunWithWindows(value);
        }
        [JsonProperty]
        public bool UseHttpProxy { get; set; } = false;
        [JsonProperty]
        public UpdateSettings Update { get; } = new UpdateSettings();
        [JsonProperty]
        public LocalizationSettings Localization { get; } = new LocalizationSettings();

        private string GetLanguage()
        {
            if (CultureInfo.DefaultThreadCurrentCulture != null)
                return CultureInfo.DefaultThreadCurrentCulture.Name;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InstalledUICulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InstalledUICulture;
            return CultureInfo.InstalledUICulture.Name;
        }

        private void SetLanguage(string cultureName)
        {
            if (cultureName != null)
            {
                try
                {
                    var culture = CultureInfo.CreateSpecificCulture(cultureName);
                    CultureInfo.DefaultThreadCurrentCulture = culture;
                    CultureInfo.DefaultThreadCurrentUICulture = culture;
                }
                catch (CultureNotFoundException)
                {
                    CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InstalledUICulture;
                    CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InstalledUICulture;
                }
            }
        }

        private bool IsRunWithWindows()
        {
            using var startupKey = Registry.CurrentUser.OpenSubKey(RegKeyAutoRun);
            return startupKey != null && startupKey.GetValue(AppName) != null;
        }

        private void SetRunWithWindows(bool value)
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