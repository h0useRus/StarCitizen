using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NSW.StarCitizen.Tools.Global;
using NSW.StarCitizen.Tools.Properties;
using NSW.StarCitizen.Tools.Settings;
using NSW.StarCitizen.Tools.Update;

namespace NSW.StarCitizen.Tools.Localization
{
    public class RepositoryManager
    {
        private readonly List<ILocalizationRepository> _localizationRepositories = new List<ILocalizationRepository>();

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
            foreach (var source in Program.Settings.Localization.Repositories)
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
                    Program.Settings.Localization.Repositories.Add(source);
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
            var localizationSource = Program.Settings.Localization.Repositories
                .FirstOrDefault(r => r.Type == repository.Type && string.Compare(r.Repository, repository.Repository, StringComparison.OrdinalIgnoreCase) == 0);
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
            return info != null ? GetRepository(info.Type, info.Repository) : null;
        }

        public ILocalizationRepository GetCurrentRepository(GameMode gameMode) => GetCurrentRepository(gameMode, GetRepositoriesList());

        public ILocalizationRepository GetCurrentRepository(GameMode gameMode, List<ILocalizationRepository> repositories)
        {
            var installedRepository = GetInstalledRepository(gameMode);
            if (installedRepository != null && repositories.Contains(installedRepository))
                return installedRepository;
            return repositories.First();
        }

        public void SetInstalledRepository(GameMode gameMode, ILocalizationRepository repository, string version)
        {
            var installation = GetRepositoryInstallation(gameMode, repository) ?? AddRepositoryInstallation(gameMode, repository);
            installation.LastVersion = version;
            installation.InstalledVersion = version;
            var otherInstallations = Program.Settings.Localization.Installations.Where(i => (i.Mode == gameMode) &&
                (i.Type != repository.Type || string.Compare(i.Repository, repository.Repository, StringComparison.OrdinalIgnoreCase) != 0));
            foreach (var otherInstallation in otherInstallations)
            {
                otherInstallation.InstalledVersion = null;
            }
            Program.SaveAppSettings();
        }

        public void RemoveInstalledRepository(GameMode gameMode, ILocalizationRepository repository)
        {
            var installation = GetRepositoryInstallation(gameMode, repository);
            if (installation?.InstalledVersion != null)
            {
                installation.LastVersion = installation.InstalledVersion;
                installation.InstalledVersion = null;
                Program.SaveAppSettings();
            }
        }

        public LocalizationInstallation? GetRepositoryInstallation(GameMode gameMode, ILocalizationRepository repository)
        {
            return Program.Settings.Localization.Installations.FirstOrDefault(i => i.Mode == gameMode &&
                i.Type == repository.Type && string.Compare(i.Repository, repository.Repository, StringComparison.OrdinalIgnoreCase) == 0);
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
            return Program.Settings.Localization.Installations.FirstOrDefault(i => !string.IsNullOrEmpty(i.InstalledVersion) &&
                i.Type == repository.Type && string.Compare(i.Repository, repository.Repository, StringComparison.OrdinalIgnoreCase) == 0)?.Mode;
        }

        public void RunMonitors()
        {
            foreach (var installation in Program.Settings.Localization.Installations)
            {
                if (!string.IsNullOrWhiteSpace(installation.Repository))
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
        }

        private void AddRepository(ILocalizationRepository repository)
        {
            repository.MonitorStarted += OnMonitorStarted;
            repository.MonitorStopped += OnMonitorStopped;
            repository.MonitorNewVersion += OnMonitorNewVersion;
            _localizationRepositories.Add(repository);
        }

        private static ILocalizationRepository? BuildRepository(LocalizationSource source) =>
            source.Type == UpdateRepositoryType.GitHub ? new GitHubLocalizationRepository(source.Name, source.Repository) : null;

        private static LocalizationInstallation AddRepositoryInstallation(GameMode gameMode, ILocalizationRepository repository)
        {
            var installation = new LocalizationInstallation(gameMode, repository.Repository, repository.Type)
            {
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
