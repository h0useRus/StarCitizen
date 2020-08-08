
using System;
using System.Collections.Generic;
using System.Linq;
using NSW.StarCitizen.Tools.Global;
using NSW.StarCitizen.Tools.Helpers;
using NSW.StarCitizen.Tools.Localization;
using NSW.StarCitizen.Tools.Settings;

namespace NSW.StarCitizen.Tools
{
    public static partial class Program
    {
        private static Dictionary<string, ILocalizationRepository> _localizationRepositories;
        public static Dictionary<string, ILocalizationRepository> LocalizationRepositories
        {
            get
            {
                if (_localizationRepositories == null)
                {
                    _localizationRepositories = new Dictionary<string, ILocalizationRepository>(StringComparer.OrdinalIgnoreCase);
                    foreach (var repository in Settings.Localization.Repositories)
                    {
                        if (!_localizationRepositories.ContainsKey(repository.Repository))
                        {
                            // Only git supported
                            var repo = new GitHubLocalizationRepository(repository.Name, repository.Repository);
                            repo.MonitorStarted += OnMonitorStarted;
                            repo.MonitorStopped += OnMonitorStopped;
                            repo.MonitorNewVersion+= OnMonitorNewVersion;
                            _localizationRepositories[repository.Repository] = repo;
                        }
                    }
                }
                return _localizationRepositories;
            }
        }

        private static void OnMonitorNewVersion(object sender, string e)
        {
            var r = (ILocalizationRepository)sender;
            Notification?.Invoke(sender, new Tuple<string, string>(r.Name, $"Found new version {e}."));
        }

        private static void OnMonitorStopped(object sender, string e)
        {
            var r = (ILocalizationRepository)sender;
            Notification?.Invoke(sender, new Tuple<string, string>(r.Name, "Stop monitoring."));
        }

        private static void OnMonitorStarted(object sender, string e)
        {
            var r = (ILocalizationRepository)sender;
            Notification?.Invoke(sender, new Tuple<string, string>(r.Name, "Start monitoring."));
        }

        private static ILocalizationRepository _currentRepository;
        public static ILocalizationRepository CurrentRepository
        {
            get => _currentRepository;
            set => SetCurrentLocalizationRepository(value);
        }

        public static LocalizationInstallation CurrentInstallation { get; private set; }

        public static ILocalizationRepository GetCurrentLocalizationRepository()
        {
            var info = Settings.Localization.Installations?.FirstOrDefault(i => i.Mode == CurrentGame.Mode);
            if (info != null && LocalizationRepositories.ContainsKey(info.Repository))
                return LocalizationRepositories[info.Repository];
            return null;
        }

        public static void SetCurrentLocalizationRepository(ILocalizationRepository localizationRepository)
        {
            if (localizationRepository?.Repository == _currentRepository?.Repository) return;

            _currentRepository = localizationRepository;

            if (_currentRepository == null) return;

            Settings.Localization.Installations ??= new List<LocalizationInstallation>();

            CurrentInstallation = Settings.Localization.Installations
                .FirstOrDefault(i => i.Mode == CurrentGame.Mode
                                  && string.Compare(i.Repository, _currentRepository.Repository, StringComparison.OrdinalIgnoreCase) == 0);
            if (CurrentInstallation == null)
            {
                CurrentInstallation = new LocalizationInstallation
                {
                    Mode = CurrentGame.Mode,
                    Repository = _currentRepository.Repository,
                    MonitorRefreshTime = Settings.Localization.MonitorRefreshTime
                };
                Settings.Localization.Installations.Add(CurrentInstallation);
            }
            SaveAppSettings();

            _currentRepository.CurrentVersion ??= new LocalizationInfo {Name = CurrentInstallation.LastVersion ?? "N/A"};
        }

        public static LanguageInfo GetLanguagesConfiguration()
        {
            var result = new LanguageInfo();
            var fileName = GameConstants.GetSystemConfigPath(CurrentGame.RootFolder.FullName);
            var cfgFile = new CfgFile(fileName);
            var data = cfgFile.Read();

            if (data.TryGetValue(GameConstants.SystemLanguagesKey, out var value))
            {
                var languages = value.Split(',');
                foreach (var language in languages)
                {
                    result.Languages.Add(language.Trim());
                }
            }

            if (data.TryGetValue(GameConstants.CurrentLanguageKey, out value))
            {
                result.Current = value;
            }

            return result;
        }

        public static void RunMonitors()
        {
            if (Settings.Localization?.Installations != null)
                foreach (var installation in Settings.Localization.Installations)
                {
                    if (!string.IsNullOrWhiteSpace(installation.Repository)
                        && LocalizationRepositories.ContainsKey(installation.Repository))
                    {
                        var repository = LocalizationRepositories[installation.Repository];

                        if (repository.CurrentVersion?.Name != installation.LastVersion)
                            repository.CurrentVersion = new LocalizationInfo {Name = installation.LastVersion};

                        if (repository.IsMonitorStarted != installation.MonitorForUpdates
                            || repository.MonitorRefreshTime != installation.MonitorRefreshTime)
                        {
                            if (installation.MonitorForUpdates)
                            {
                                repository.MonitorStart(installation.MonitorRefreshTime);
                            }
                            else
                            {
                                repository.MonitorStop();
                            }
                        }
                    }
                }
        }

        public static event EventHandler<Tuple<string, string>> Notification;
    }
}