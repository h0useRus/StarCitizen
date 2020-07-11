using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace NSW.StarCitizen.Tools.Services
{
    public class SettingsService
    {
        private static readonly string _appSettingsFileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "settings.json");
        private static readonly JsonSerializerSettings _jsonSettings = GetJsonSettings();
        private static JsonSerializerSettings GetJsonSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
            return settings;
        }

        public static SettingsService Instance { get; } = new SettingsService();
        private SettingsService(){}

        private AppSettings _current;
        public AppSettings AppSettings => _current ??= GetAppSettings();

        public bool SaveAppSettings() => SaveAppSettings(AppSettings);

        private bool SaveAppSettings(AppSettings appSettings)
        {
            try
            {
                File.WriteAllText(_appSettingsFileName, JsonConvert.SerializeObject(appSettings, Formatting.Indented, _jsonSettings));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private AppSettings GetAppSettings()
        {
            var fileName = _appSettingsFileName;
            var appSettings = new AppSettings();
            if (File.Exists(fileName))
                try
                {
                    return JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(fileName), _jsonSettings);
                }
                catch { }

            Validate(appSettings);
            return appSettings;
        }

        private void Validate(AppSettings appSettings)
        {
            if (appSettings.SupportedSources != null && appSettings.SupportedSources.Count > 0) return;
            appSettings.SupportedSources = new List<LocalizationSource> {LocalizationSource.Default};
            SaveAppSettings(appSettings);
        }
    }
}