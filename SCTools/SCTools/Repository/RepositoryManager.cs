using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NSW.StarCitizen.Tools.Helpers;
using NSW.StarCitizen.Tools.Lib.Global;
using NSW.StarCitizen.Tools.Lib.Localization;
using NSW.StarCitizen.Tools.Lib.Update;
using NSW.StarCitizen.Tools.Properties;
using NSW.StarCitizen.Tools.Settings;

namespace NSW.StarCitizen.Tools.Repository
{
    public class RepositoryManager
    {
        private readonly ILocalizationInstaller _localizationInstaller = new DefaultLocalizationInstaller();
        private readonly List<ILocalizationRepository> _localizationRepositories = new List<ILocalizationRepository>();
        private readonly LocalizationSettings _localizationSettings;

        public GameMode GameMode { get; }

        public event EventHandler<Tuple<string, string>>? Notification;

        public enum AddStatus
        {
            Success,
            DuplicateName,
            DuplicateUrl,
            Unreachable
        }

        public RepositoryManager(GameMode gameMode)
        {
            GameMode = gameMode;
            _localizationSettings = Program.Settings.GetGameModeSettings(gameMode);
            foreach (var source in _localizationSettings.Repositories)
            {
                if (!ContainsRepository(source.Type, source.Repository))
                {
                    var repository = BuildRepository(source);
                    if (repository != null)
                        AddRepository(repository);
                }
            }
        }

        public List<ILocalizationRepository> GetRepositoriesList() => _localizationRepositories.ToList();

        public bool ContainsRepositoryName(string name) => _localizationRepositories.Any(v => string.Compare(v.Name, name, StringComparison.OrdinalIgnoreCase) == 0);

        public bool ContainsRepository(UpdateRepositoryType type, string repository) =>
            _localizationRepositories.Any(v => v.Type == type && string.Compare(v.Repository, repository, StringComparison.OrdinalIgnoreCase) == 0);

        public ILocalizationRepository? GetRepository(UpdateRepositoryType type, string repository) =>
            _localizationRepositories.FirstOrDefault(v => v.Type == type && string.Compare(v.Repository, repository, StringComparison.OrdinalIgnoreCase) == 0);

        public async Task<AddStatus> AddRepositoryAsync(LocalizationSource source, CancellationToken cancellationToken)
        {
            if (ContainsRepositoryName(source.Name))
                return AddStatus.DuplicateName;

            if (ContainsRepository(source.Type, source.Repository))
                return AddStatus.DuplicateUrl;

            var repository = BuildRepository(source);
            if (repository != null)
            {
                if (await repository.CheckAsync(cancellationToken))
                {
                    AddRepository(repository);
                    _localizationSettings.Repositories.Add(source);
                    Program.SaveAppSettings();
                    return AddStatus.Success;
                }
                repository.Dispose();
            }
            return AddStatus.Unreachable;
        }

        public async Task<AddStatus> UpdateRepositoryAsync(ILocalizationRepository repository, LocalizationSource source,
            CancellationToken cancellationToken)
        {
            bool nameChanged = repository.Name != source.Name;
            bool urlChanged = repository.Type != source.Type ||
                !string.Equals(repository.Repository, source.Repository, StringComparison.OrdinalIgnoreCase);
            if (nameChanged && ContainsRepositoryName(source.Name))
                return AddStatus.DuplicateName;
            if (urlChanged && ContainsRepository(source.Type, source.Repository))
                return AddStatus.DuplicateUrl;
            if (nameChanged || urlChanged)
            {
                var newRepository = BuildRepository(source);
                if (newRepository != null)
                {
                    if ((!urlChanged || await newRepository.CheckAsync(cancellationToken)) &&
                        ReplaceRepository(repository, newRepository, source))
                    {
                        return AddStatus.Success;
                    }
                    newRepository.Dispose();
                }
                return AddStatus.Unreachable;
            }
            return AddStatus.Success;
        }

        public bool RemoveRepository(ILocalizationRepository repository)
        {
            repository.MonitorStarted -= OnMonitorStarted;
            repository.MonitorStopped -= OnMonitorStopped;
            repository.MonitorNewVersion -= OnMonitorNewVersion;
            repository.MonitorStop();
            var result = _localizationRepositories.Remove(repository);
            var localizationSource = _localizationSettings.Repositories.FirstOrDefault(r => r.Type == repository.Type &&
                string.Compare(r.Repository, repository.Repository, StringComparison.OrdinalIgnoreCase) == 0);
            if (localizationSource != null)
            {
                _localizationSettings.Repositories.Remove(localizationSource);
                Program.SaveAppSettings();
            }
            repository.Dispose();
            return result;
        }

        public bool CanMoveRepositoryUp(ILocalizationRepository repository)
            => _localizationRepositories.CanMoveStart(_localizationRepositories.IndexOf(repository));

        public bool CanMoveRepositoryDown(ILocalizationRepository repository)
            => _localizationRepositories.CanMoveEnd(_localizationRepositories.IndexOf(repository));

        public bool MoveRepositoryUp(ILocalizationRepository repository)
        {
            if (_localizationRepositories.MoveStart(_localizationRepositories.IndexOf(repository)))
            {
                int sourceIndex = _localizationSettings.Repositories.FindIndex(r => r.Type == repository.Type &&
                    string.Compare(r.Repository, repository.Repository, StringComparison.OrdinalIgnoreCase) == 0);
                if (_localizationSettings.Repositories.MoveStart(sourceIndex))
                {
                    Program.SaveAppSettings();
                }
                return true;
            }
            return false;
        }

        public bool MoveRepositoryDown(ILocalizationRepository repository)
        {
            if (_localizationRepositories.MoveEnd(_localizationRepositories.IndexOf(repository)))
            {
                int sourceIndex = _localizationSettings.Repositories.FindIndex(r => r.Type == repository.Type &&
                    string.Compare(r.Repository, repository.Repository, StringComparison.OrdinalIgnoreCase) == 0);
                if (_localizationSettings.Repositories.MoveEnd(sourceIndex))
                {
                    Program.SaveAppSettings();
                }
                return true;
            }
            return false;
        }

        public ILocalizationRepository? GetInstalledRepository()
        {
            var info = _localizationSettings.Installations
                .FirstOrDefault(i => i.Mode == GameMode && !string.IsNullOrEmpty(i.InstalledVersion));
            return info != null ? GetRepository(info.Type, info.Repository) : null;
        }

        public ILocalizationRepository GetCurrentRepository() => GetCurrentRepository(GetRepositoriesList());

        public ILocalizationRepository GetCurrentRepository(List<ILocalizationRepository> repositories)
        {
            var installedRepository = GetInstalledRepository();
            if (installedRepository != null && repositories.Contains(installedRepository))
                return installedRepository;
            return repositories.First();
        }

        public void SetInstalledRepository(ILocalizationRepository repository, string version)
        {
            var installation = GetRepositoryInstallation(repository) ?? AddRepositoryInstallation(repository);
            installation.LastVersion = version;
            installation.InstalledVersion = version;
            var otherInstallations = _localizationSettings.Installations.Where(i => i.Mode == GameMode &&
                (i.Type != repository.Type || string.Compare(i.Repository, repository.Repository, StringComparison.OrdinalIgnoreCase) != 0));
            foreach (var otherInstallation in otherInstallations)
            {
                otherInstallation.InstalledVersion = null;
            }
            Program.SaveAppSettings();
        }

        public void RemoveInstalledRepository(ILocalizationRepository repository)
        {
            var installation = GetRepositoryInstallation(repository);
            if (installation?.InstalledVersion != null)
            {
                installation.LastVersion = installation.InstalledVersion;
                installation.InstalledVersion = null;
                Program.SaveAppSettings();
            }
        }

        public LocalizationInstallation? GetRepositoryInstallation(ILocalizationRepository repository)
        {
            return _localizationSettings.Installations.FirstOrDefault(i => i.Mode == GameMode &&
                                                                           i.Type == repository.Type && string.Compare(i.Repository, repository.Repository, StringComparison.OrdinalIgnoreCase) == 0);
        }

        public LocalizationInstallation CreateRepositoryInstallation(ILocalizationRepository repository)
        {
            var currentInstallation = GetRepositoryInstallation(repository);
            if (currentInstallation == null)
            {
                currentInstallation = AddRepositoryInstallation(repository);
                Program.SaveAppSettings();
            }
            return currentInstallation;
        }

        public GameMode? GetRepositoryUsedGameMode(ILocalizationRepository repository)
        {
            return _localizationSettings.Installations.FirstOrDefault(i => !string.IsNullOrEmpty(i.InstalledVersion) &&
                                                                           i.Type == repository.Type && string.Compare(i.Repository, repository.Repository, StringComparison.OrdinalIgnoreCase) == 0)?.Mode;
        }

        public void RunMonitors()
        {
            foreach (var installation in _localizationSettings.Installations)
            {
                var repository = GetRepository(installation.Type, installation.Repository);
                if (repository != null)
                {
                    repository.AllowPreReleases = installation.AllowPreRelease;
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

        public void StopMonitors()
        {
            foreach (var installation in _localizationSettings.Installations)
            {
                var repository = GetRepository(installation.Type, installation.Repository);
                repository?.MonitorStop();
            }
        }

        public void UpdateAllowIncrementalDownload(bool allowed)
        {
            foreach (var repository in _localizationRepositories)
            {
                if (repository is GitHubLocalizationRepository githubRepository)
                {
                    githubRepository.AllowIncrementalDownload = allowed;
                }
            }
        }

        private void AddRepository(ILocalizationRepository repository)
        {
            repository.MonitorStarted += OnMonitorStarted;
            repository.MonitorStopped += OnMonitorStopped;
            repository.MonitorNewVersion += OnMonitorNewVersion;
            _localizationRepositories.Add(repository);
        }

        private bool ReplaceRepository(ILocalizationRepository repository, ILocalizationRepository newRepository, LocalizationSource newSource)
        {
            bool monitorWasRunning = repository.IsMonitorStarted;
            repository.MonitorStarted -= OnMonitorStarted;
            repository.MonitorStopped -= OnMonitorStopped;
            repository.MonitorNewVersion -= OnMonitorNewVersion;
            repository.MonitorStop();
            int repoIndex = _localizationRepositories.IndexOf(repository);
            if (repoIndex < 0)
            {
                return false;
            }
            _localizationRepositories[repoIndex] = newRepository;
            newRepository.MonitorNewVersion += OnMonitorNewVersion;
            if (monitorWasRunning)
                newRepository.MonitorStart(repository.MonitorRefreshTime);
            newRepository.MonitorStarted += OnMonitorStarted;
            newRepository.MonitorStopped += OnMonitorStopped;
            int sourceIndex = _localizationSettings.Repositories.FindIndex(r => r.Type == repository.Type &&
                string.Compare(r.Repository, repository.Repository, StringComparison.OrdinalIgnoreCase) == 0);
            if (sourceIndex >= 0)
                _localizationSettings.Repositories[sourceIndex] = newSource;
            else
                _localizationSettings.Repositories.Add(newSource);
            Program.SaveAppSettings();
            repository.Dispose();
            return true;
        }

        private ILocalizationRepository? BuildRepository(LocalizationSource source) => source.Type switch
        {
            UpdateRepositoryType.GitHub => new GitHubLocalizationRepository(_localizationInstaller, HttpNetClient.Client, GameMode, source.Name, source.Repository)
            {
                AuthToken = Program.Settings.AuthToken,
                AllowIncrementalDownload = Program.Settings.AllowIncrementalDownload,
            },
            UpdateRepositoryType.Gitee => new GiteeLocalizationRepository(_localizationInstaller, HttpNetClient.Client, GameMode, source.Name, source.Repository),
            UpdateRepositoryType.Folder => new FolderLocalizationRepository(_localizationInstaller, Program.ExecutableDir, GameMode, source.Name, source.Repository),
            _ => null,
        };

        private LocalizationInstallation AddRepositoryInstallation(ILocalizationRepository repository)
        {
            var installation = new LocalizationInstallation(GameMode, repository.Repository, repository.Type)
            {
                MonitorRefreshTime = TimePresets.GetRefreshTimePresets(repository.Type).First()
            };
            _localizationSettings.Installations.Add(installation);
            return installation;
        }

        private void OnMonitorNewVersion(object sender, string version)
        {
            var r = (ILocalizationRepository)sender;
            Notification?.Invoke(sender, new Tuple<string, string>(BuildRepositoryNotificationTitle(r),
                string.Format(CultureInfo.CurrentUICulture, Resources.Localization_Found_New_Version, version)));
        }

        private void OnMonitorStopped(object sender, EventArgs eventArgs)
        {
            var r = (ILocalizationRepository)sender;
            Notification?.Invoke(sender, new Tuple<string, string>(BuildRepositoryNotificationTitle(r), Resources.Localization_Stop_Monitoring));
        }

        private void OnMonitorStarted(object sender, EventArgs eventArgs)
        {
            var r = (ILocalizationRepository)sender;
            Notification?.Invoke(sender, new Tuple<string, string>(BuildRepositoryNotificationTitle(r), Resources.Localization_Start_Monitoring));
        }

        private static string BuildRepositoryNotificationTitle(ILocalizationRepository repository)
            => string.Format(CultureInfo.CurrentUICulture, "{0}: {1}", repository.Mode, repository.Name);
    }
}
