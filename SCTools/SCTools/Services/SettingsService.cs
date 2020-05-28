using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace NSW.StarCitizen.Tools.Services
{
    public class SettingsService
    {
        private const string AppSettingsFileName = "settings.json";
        public static SettingsService Instance { get; } = new SettingsService();
        private SettingsService(){}

        private AppSettings _current;
        public AppSettings AppSettings => _current ??= GetAppSettings();

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

        private static AppSettings GetAppSettings()
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

        private static string GetAppSettingFileName() => Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), AppSettingsFileName);
    }
}