using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NSW.StarCitizen.Tools.Global;
using NSW.StarCitizen.Tools.Properties;
using NSW.StarCitizen.Tools.Settings;

namespace NSW.StarCitizen.Tools.Localization
{
    public class RepositoryManager
    {
        private readonly Dictionary<string, ILocalizationRepository> _localizationRepositories
            = new Dictionary<string, ILocalizationRepository>(StringComparer.OrdinalIgnoreCase);

        public event EventHandler<Tuple<string, string>>? Notification;

        public enum AddStatus
        {
            Success,
            DuplicateName,
            DuplicateUrl,
            Unreachable
        }

        public RepositoryManager()
        {
            foreach (var localizationSource in Program.Settings.Localization.Repositories)
            {
                if (!_localizationRepositories.ContainsKey(localizationSource.Repository))
                {
                    // Only git supported
                    AddRepository(new GitHubLocalizationRepository(localizationSource.Name, localizationSource.Repository));
                }
            }
        }

        public List<ILocalizationRepository> GetRepositoriesList() => _localizationRepositories.Values.ToList();

        public bool ContainsRepositoryName(string name) => _localizationRepositories.Values.Any(v => string.Compare(v.Name, name, StringComparison.OrdinalIgnoreCase) == 0);

        public bool ContainsRepositoryUrl(string url) => _localizationRepositories.Values.Any(v => string.Compare(v.Repository, url, StringComparison.OrdinalIgnoreCase) == 0);

        public ILocalizationRepository? GetRepository(string url)
        {
            if (_localizationRepositories.ContainsKey(url))
                return _localizationRepositories[url];
            return null;
        }

        public async Task<AddStatus> AddRepositoryAsync(string name, string url, CancellationToken cancellationToken)
        {
            if (ContainsRepositoryName(name))
                return AddStatus.DuplicateName;

            if (ContainsRepositoryUrl(url))
                return AddStatus.DuplicateUrl;

            ILocalizationRepository repository = new GitHubLocalizationRepository(name, url);
            if (await repository.CheckAsync(cancellationToken))
            {
                AddRepository(repository);
                Program.Settings.Localization.Repositories.Add(new LocalizationSource
                {
                    Name = repository.Name,
                    Repository = repository.Repository,
                    Type = repository.Type.ToString()
                });
                Program.SaveAppSettings();
                return AddStatus.Success;
            }
            repository.Dispose();
            return AddStatus.Unreachable;
        }

        public bool RemoveRepository(ILocalizationRepository repository)
        {
            repository.MonitorStarted -= OnMonitorStarted;
            repository.MonitorStopped -= OnMonitorStopped;
            repository.MonitorNewVersion -= OnMonitorNewVersion;
            repository.MonitorStop();
            var result = _localizationRepositories.Remove(repository.Repository);
            var localizationSource = Program.Settings.Localization.Repositories
                .FirstOrDefault(r => string.Compare(r.Repository, repository.Repository, StringComparison.OrdinalIgnoreCase) == 0);
            if (localizationSource != null)
            {
                Program.Settings.Localization.Repositories.Remove(localizationSource);
                Program.SaveAppSettings();
            }
            repository.Dispose();
            return result;
        }

        public ILocalizationRepository? GetInstalledRepository(GameMode gameMode)
        {
            var info = Program.Settings.Localization.Installations.FirstOrDefault(i => (i.Mode == gameMode) &&
                !string.IsNullOrEmpty(i.InstalledVersion));
            return info != null ? GetRepository(info.Repository) : null;
        }

        public void SetInstalledRepository(GameMode gameMode, ILocalizationRepository repository)
        {
            var installation = GetRepositoryInstallation(gameMode, repository);
            if (installation == null)
                installation = AddRepositoryInstallation(gameMode, repository);
            installation.LastVersion = repository.CurrentVersion;
            installation.InstalledVersion = repository.CurrentVersion;
            var otherInstallations = Program.Settings.Localization.Installations.Where(i => (i.Mode == gameMode) &&
                (string.Compare(i.Repository, repository.Repository, StringComparison.OrdinalIgnoreCase) != 0));
            foreach (var otherInstallation in otherInstallations)
            {
                otherInstallation.InstalledVersion = null;
            }
            Program.SaveAppSettings();
        }

        public void RemoveInstalledRepository(GameMode gameMode, ILocalizationRepository repository)
        {
            var installation = GetRepositoryInstallation(gameMode, repository);
            if (installation != null && installation.InstalledVersion != null)
            {
                installation.LastVersion = installation.InstalledVersion;
                installation.InstalledVersion = null;
                Program.SaveAppSettings();
            }
        }

        public LocalizationInstallation? GetRepositoryInstallation(GameMode gameMode, ILocalizationRepository repository)
        {
            return Program.Settings.Localization.Installations.FirstOrDefault(i => i.Mode == gameMode &&
                string.Compare(i.Repository, repository.Repository, StringComparison.OrdinalIgnoreCase) == 0);
        }

        public LocalizationInstallation CreateRepositoryInstallation(GameMode gameMode, ILocalizationRepository repository)
        {
            var currentInstallation = GetRepositoryInstallation(gameMode, repository);
            if (currentInstallation == null)
            {
                currentInstallation = AddRepositoryInstallation(gameMode, repository);
                Program.SaveAppSettings();
            }
            return currentInstallation;
        }

        public GameMode? GetRepositoryUsedGameMode(ILocalizationRepository repository)
        {
            return Program.Settings.Localization.Installations.Where(i => !string.IsNullOrEmpty(i.InstalledVersion) &&
                (string.Compare(i.Repository, repository.Repository, StringComparison.OrdinalIgnoreCase) == 0))?
                .FirstOrDefault()?.Mode;
        }

        public void RunMonitors()
        {
            foreach (var installation in Program.Settings.Localization.Installations)
            {
                if (!string.IsNullOrWhiteSpace(installation.Repository))
                {
                    var repository = GetRepository(installation.Repository);
                    if (repository != null)
                    {
                        if (repository.IsMonitorStarted != installation.MonitorForUpdates
                            || repository.MonitorRefreshTime != installation.MonitorRefreshTime)
                        {
                            if (installation.MonitorForUpdates)
                            {
                                repository.UpdateCurrentVersion(installation.LastVersion ?? installation.InstalledVersion);
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
        }

        private void AddRepository(ILocalizationRepository repository)
        {
            repository.MonitorStarted += OnMonitorStarted;
            repository.MonitorStopped += OnMonitorStopped;
            repository.MonitorNewVersion += OnMonitorNewVersion;
            _localizationRepositories[repository.Repository] = repository;
        }

        private LocalizationInstallation AddRepositoryInstallation(GameMode gameMode, ILocalizationRepository repository)
        {
            var installation = new LocalizationInstallation
            {
                Mode = gameMode,
                Repository = repository.Repository,
                MonitorRefreshTime = Program.Settings.Localization.MonitorRefreshTime
            };
            Program.Settings.Localization.Installations.Add(installation);
            return installation;
        }

        private void OnMonitorNewVersion(object sender, string e)
        {
            var r = (ILocalizationRepository)sender;
            Notification?.Invoke(sender, new Tuple<string, string>(r.Name, string.Format(Resources.Localization_Found_New_Version, e)));
        }

        private void OnMonitorStopped(object sender, string e)
        {
            var r = (ILocalizationRepository)sender;
            Notification?.Invoke(sender, new Tuple<string, string>(r.Name, Resources.Localization_Stop_Monitoring));
        }

        private void OnMonitorStarted(object sender, string e)
        {
            var r = (ILocalizationRepository)sender;
            Notification?.Invoke(sender, new Tuple<string, string>(r.Name, Resources.Localization_Start_Monitoring));
        }
    }
}
