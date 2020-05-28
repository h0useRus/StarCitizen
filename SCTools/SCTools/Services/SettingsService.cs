using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace NSW.StarCitizen.Tools.Services
{
    public class SettingsService
    {
        private const string AppSettingsFileName = "settings.json";
        public static SettingsService Instance { get; } = new SettingsService();

        private AppSettings _current;
        public AppSettings AppSettings => _current ??= GetAppSettings();

        private AppSettings GetAppSettings()
        {
            var fileName = GetAppSettingFileName();
            if (File.Exists(fileName))
                try
                {
                    return JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(fileName));
                }
                catch
                {

                }
            return new AppSettings();
        }

        public bool SaveAppSettings()
        {
            try
            {
                File.WriteAllText(GetAppSettingFileName(), JsonConvert.SerializeObject(AppSettings));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GetAppSettingFileName() => Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), AppSettingsFileName);
    }
}