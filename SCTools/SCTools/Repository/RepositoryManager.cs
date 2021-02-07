using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NSW.StarCitizen.Tools.Lib.Global;
using NSW.StarCitizen.Tools.Lib.Localization;
using NSW.StarCitizen.Tools.Lib.Update;
using NSW.StarCitizen.Tools.Properties;
using NSW.StarCitizen.Tools.Settings;

namespace NSW.StarCitizen.Tools.Repository
{
    public class RepositoryManager
    {
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

        private void AddRepository(ILocalizationRepository repository)
        {
            repository.MonitorStarted += OnMonitorStarted;
            repository.MonitorStopped += OnMonitorStopped;
            repository.MonitorNewVersion += OnMonitorNewVersion;
            _localizationRepositories.Add(repository);
        }

        private ILocalizationRepository? BuildRepository(LocalizationSource source)
        {
            if (source.Type == UpdateRepositoryType.GitHub)
            {
                return new GitHubLocalizationRepository(HttpNetClient.Client, GameMode, source.Name, source.Repository)
                {
                    AuthToken = Program.Settings.AuthToken
                };
            }
            return null;
        }

        private LocalizationInstallation AddRepositoryInstallation(ILocalizationRepository repository)
        {
            var installation = new LocalizationInstallation(GameMode, repository.Repository, repository.Type)
            {
                MonitorRefreshTime = _localizationSettings.MonitorRefreshTime
            };
            _localizationSettings.Installations.Add(installation);
            return installation;
        }

        private void OnMonitorNewVersion(object sender, string version)
        {
            var r = (ILocalizationRepository)sender;
            Notification?.Invoke(sender, new Tuple<string, string>($"{r.Mode}: {r.Name}",
                string.Format(Resources.Localization_Found_New_Version, version)));
        }

        private void OnMonitorStopped(object sender, EventArgs eventArgs)
        {
            var r = (ILocalizationRepository)sender;
            Notification?.Invoke(sender, new Tuple<string, string>($"{r.Mode}: {r.Name}", Resources.Localization_Stop_Monitoring));
        }

        private void OnMonitorStarted(object sender, EventArgs eventArgs)
        {
            var r = (ILocalizationRepository)sender;
            Notification?.Invoke(sender, new Tuple<string, string>($"{r.Mode}: {r.Name}", Resources.Localization_Start_Monitoring));
        }
    }
}
