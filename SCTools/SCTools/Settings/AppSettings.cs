using System.Globalization;
using System.Windows.Forms;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace NSW.StarCitizen.Tools.Settings
{
    public class AppSettings
    {
        private const string AppName = "Star Citizen Tools";
        private static readonly RegistryKey _startupKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

        public string? GameFolder { get; set; }
        public string Language
        {
            get => GetLanguage();
            set => SetLanguage(value);
        }
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

        public bool UseHttpProxy { get; set; } = false;

        public UpdateSettings Update { get; } = new UpdateSettings();

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
    }
}