
using System;
using System.Collections.Generic;
using System.Linq;
using NSW.StarCitizen.Tools.Localization;
using NSW.StarCitizen.Tools.Properties;
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
            Notification?.Invoke(sender, new Tuple<string, string>(r.Name, string.Format(Resources.Localization_Found_New_Version, e)));
        }

        private static void OnMonitorStopped(object sender, string e)
        {
            var r = (ILocalizationRepository)sender;
            Notification?.Invoke(sender, new Tuple<string, string>(r.Name, Resources.Localization_Stop_Monitoring));
        }

        private static void OnMonitorStarted(object sender, string e)
        {
            var r = (ILocalizationRepository)sender;
            Notification?.Invoke(sender, new Tuple<string, string>(r.Name, Resources.Localization_Start_Monitoring));
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

        public static void UpdateCurrentInstallationRepository(ILocalizationRepository localizationRepository)
        {
            CurrentInstallation.Repository = localizationRepository.Repository;
            CurrentInstallation.LastVersion = localizationRepository.CurrentVersion.Name;
            CurrentInstallation.InstalledVersion = localizationRepository.CurrentVersion.Name;
            Settings.Localization.Installations ??= new List<LocalizationInstallation>();
            var otherInstallations = Settings.Localization.Installations.Where(i => (i.Mode == CurrentGame.Mode) &&
                (string.Compare(i.Repository, localizationRepository.Repository, StringComparison.OrdinalIgnoreCase) != 0));
            foreach (var otherInstallation in otherInstallations)
            {
                otherInstallation.InstalledVersion = null;
            }
            SaveAppSettings();
        }

        public static LocalizationInstallation GetLocalizationInstallationFromRepository(ILocalizationRepository localizationRepository)
        {
            return Settings.Localization.Installations.FirstOrDefault(i => (i.Mode == CurrentGame.Mode) &&
                (string.Compare(i.Repository, localizationRepository.Repository, StringComparison.OrdinalIgnoreCase) == 0));
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