//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.IO.Compression;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Timers;
//using System.Windows.Forms;
//using NSW.StarCitizen.Tools.Global;
//using NSW.StarCitizen.Tools.Helpers;
//using NSW.StarCitizen.Tools.Localization;
//using NSW.StarCitizen.Tools.Properties;
//using NSW.StarCitizen.Tools.Settings;
//using Timer = System.Timers.Timer;

//namespace NSW.StarCitizen.Tools.Services
//{
    

//    public class LanguagesInfo
//    {
//        public List<string> Languages { get; set; } = new List<string>();
//        public string Current { get; set; }
//        public string New { get; set; }
//    }

//    public class LocalizationService
//    {
//        private const string BaseUrl = "https://api.github.com/repos";
//        private const string KeySysLanguages = "sys_languages";
//        private const string KeyCurLanguage = "g_language";

//        public static LocalizationService Instance { get; } = new LocalizationService();

//        private readonly HttpClient _gitClient;
//        private readonly string _repoUrl;
//        private readonly Timer _monitorTimer;

//        private LocalizationService()
//        {
//            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
//            _gitClient = new HttpClient();
//            _gitClient.DefaultRequestHeaders.UserAgent.ParseAdd("SCTools/1.0");
//            _repoUrl = $"{BaseUrl}/{SettingsService.Instance.AppSettings.SupportedSources[0].Repository}/";
//            _monitorTimer = new Timer();
//            _monitorTimer.Elapsed += MonitorTimerOnElapsedAsync;
//        }

//        public bool UnZipFile(string destinationFolder, string zipFileName)
//        {
//            try
//            {
//                using var archive = ZipFile.OpenRead(zipFileName);
//                var rootEntry = archive.Entries[0];
//                //extract only data folder
//                foreach (var entry in archive.Entries)
//                {
//                    if (entry.FullName.Contains(@"/data/"))
//                    {
//                        if (string.IsNullOrWhiteSpace(entry.Name))
//                        {
//                            var dir = Path.Combine(destinationFolder,
//                                entry.FullName.Replace(rootEntry.FullName, string.Empty));
//                            if (!Directory.Exists(dir))
//                                Directory.CreateDirectory(dir);
//                        }
//                        else
//                        {
//                            entry.ExtractToFile(
//                                Path.Combine(destinationFolder,
//                                    entry.FullName.Replace(rootEntry.FullName, string.Empty)), true);
//                        }
//                    }
//                    else if(entry.FullName.EndsWith(@"patcher.bin"))
//                    {
//                        entry.ExtractToFile(
//                            Path.Combine(destinationFolder, "Bin64", "CIGDevelopmentTools.dll"), true);
//                    }
                    
//                }

//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        public bool IsLocalizationInstalled(GameInfo gameInfo)
//            => Directory.Exists(Path.Combine(gameInfo.RootFolder.FullName, "data", "Localization"));

//        public async Task<LocalizationInfo> GetLocalizationStatusAsync(GameInfo gameInfo)
//        {
//            var lastRelease = await GetLastReleaseAsync();
//            if (lastRelease != null)
//            {
//                var result = new LocalizationInfo
//                {
//                    Release = lastRelease,
//                    Status = string.Compare(lastRelease.Name,
//                        GetSettings(gameInfo.Mode).LastVersion,
//                        StringComparison.OrdinalIgnoreCase) == 0
//                        ? LocalizationStatus.Actual
//                        : LocalizationStatus.Outdated
//                };
//                if (!IsLocalizationInstalled(gameInfo))
//                    result.Status = LocalizationStatus.NotInstalled;
//                return result;
//            }

//            return new LocalizationInfo { Status = LocalizationStatus.NotInstalled };
//        }

//        public LanguagesInfo GetLanguagesConfiguration(GameInfo gameInfo)
//        {
//            var result = new LanguagesInfo();
//            var fileName = Path.Combine(gameInfo.RootFolder.FullName, "data", "system.cfg");
//            var cfgFile = new CfgFile(fileName);
//            var data = cfgFile.Read();

//            if (data.TryGetValue(KeySysLanguages, out var value))
//            {
//                var languages = value.Split(',');
//                foreach (var language in languages)
//                {
//                    result.Languages.Add(language.Trim());
//                }
//            }

//            if (data.TryGetValue(KeyCurLanguage, out value))
//            {
//                result.Current = value;
//            }

//            return result;
//        }

//        public LanguagesInfo UpdateLanguage(GameInfo gameInfo, LanguagesInfo languages)
//        {
//            if (string.IsNullOrWhiteSpace(languages.New) || string.Compare(languages.Current, languages.New, StringComparison.OrdinalIgnoreCase) == 0)
//            {
//                languages.New = null;
//                return languages;
//            }


//            var fileName = Path.Combine(gameInfo.RootFolder.FullName, "data", "system.cfg");
//            var cfgFile = new CfgFile(fileName);
//            var data = cfgFile.Read();
//            data.AddOrUpdateRow(KeyCurLanguage, languages.New);

//            if (data.Save())
//            {
//                languages.Current = languages.New;
//            }

//            languages.New = null;
//            return languages;
//        }

//        public LocalizationSettingsInfo GetSettings(GameMode mode)
//        {
//            if (SettingsService.Instance.AppSettings.Localization.Infos == null)
//                return new LocalizationSettingsInfo { Mode = mode };

//            foreach (var localizationSettingsInfo in SettingsService.Instance.AppSettings.Localization.Infos)
//            {
//                if (localizationSettingsInfo.Mode == mode)
//                    return localizationSettingsInfo;
//            }

//            return new LocalizationSettingsInfo { Mode = mode };
//        }

//        public void UpdateLastPatchVersion(GameMode mode, string patchVersion)
//        {
//            if (SettingsService.Instance.AppSettings.Localization.Infos == null)
//            {
//                SettingsService.Instance.AppSettings.Localization.Infos = new List<LocalizationSettingsInfo>
//                {
//                    new LocalizationSettingsInfo {Mode = mode, LastVersion = patchVersion}
//                };
//            }
//            else
//            {
//                var found = false;
//                foreach (var localizationSettingsInfo in SettingsService.Instance.AppSettings.Localization.Infos)
//                {
//                    if (localizationSettingsInfo.Mode == mode)
//                    {
//                        localizationSettingsInfo.LastVersion = patchVersion;
//                        found = true;
//                        break;
//                    }
//                }

//                if (!found)
//                {
//                    SettingsService.Instance.AppSettings.Localization.Infos.Add(new LocalizationSettingsInfo { Mode = mode, LastVersion = patchVersion });
//                }
//            }

//            SettingsService.Instance.SaveAppSettings();
//        }

        
//        public void MonitorStart()
//        {
//            _monitorTimer.Stop();
//            _monitorTimer.Interval = TimeSpan
//                .FromMinutes(SettingsService.Instance.AppSettings.Localization.MonitorRefreshTime).TotalMilliseconds;
//            _monitorTimer.Start();
//            ShowBalloonTip(500, "Запущен мониторинг версий локализаций.");
//        }

//        public void MonitorStop()
//        {
//            _monitorTimer.Stop();
//            ShowBalloonTip(500, "Остановлен мониторинг версий локализаций.", ToolTipIcon.Warning);
//        }

//        private async void MonitorTimerOnElapsedAsync(object sender, ElapsedEventArgs e)
//        {
//            var result = await GetLastReleaseAsync();

//            if (result != null)
//            {
//                var showVersion = false;
//                if (SettingsService.Instance.AppSettings.Localization.Infos == null || SettingsService.Instance.AppSettings.Localization.Infos.Count == 0)
//                {
//                    showVersion = true;
//                }
//                else
//                {
//                    foreach (var localizationSettingsInfo in SettingsService.Instance.AppSettings.Localization.Infos)
//                    {
//                        if (string.Compare(localizationSettingsInfo.LastVersion, result.Name, StringComparison.OrdinalIgnoreCase) != 0)
//                        {
//                            showVersion = true;
//                        }
//                    }
//                }

//                if (showVersion)
//                {
//                    ShowBalloonTip(1000, $"Обнаружена новая версия '{result.Name}', обновить?");
//                }
//            }
//        }


//        private NotifyIcon _tray;
//        public void RegisterNotification(NotifyIcon notifyIcon)
//        {
//            _tray = notifyIcon;
//        }

//        private void ShowBalloonTip(int timeOut, string text, ToolTipIcon icon = ToolTipIcon.Info)
//            => _tray?.ShowBalloonTip(timeOut, text, Resources.Localization_Title, icon);
//    }
//}