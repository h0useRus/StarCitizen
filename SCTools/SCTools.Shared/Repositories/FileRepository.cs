using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace NSW.StarCitizen.Tools.Repositories
{
    public abstract class FileRepository : IFileRepository
    {
        private readonly System.Timers.Timer _monitorTimer;

        public string Name { get; }
        public string Repository { get; }
        public string RepositoryUrl { get; }
        public FileRepositoryType Type { get; }
        public string? CurrentVersion { get; private set; }
        public IEnumerable<ReleaseInfo>? Releases { get; private set; }
        public ReleaseInfo? LatestRelease => Releases?.FirstOrDefault();
        public bool AllowPreReleases { get; set; } = true;

        protected FileRepository(FileRepositoryType type, string name, string repository, string repositoryUrl)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));
            if (type != FileRepositoryType.Local && string.IsNullOrWhiteSpace(repository))
                throw new ArgumentNullException(nameof(repository));

            Type = type;
            Name = name;
            Repository = repository;
            RepositoryUrl = repositoryUrl;

            _monitorTimer = new System.Timers.Timer();
            _monitorTimer.Elapsed += MonitorTimerOnElapsed;
        }

        public void Dispose()
        {
            _monitorTimer.Dispose();
            GC.SuppressFinalize(this);
        }

        public abstract Task<List<ReleaseInfo>> GetAllAsync(CancellationToken cancellationToken);

        public async Task<IEnumerable<ReleaseInfo>> RefreshAsync(CancellationToken cancellationToken)
        {
            var releases = await GetAllAsync(cancellationToken);
            Releases = SortAndFilterReleases(releases).ToList();
            return Releases;
        }

        public async Task<ReleaseInfo?> GetLatestAsync(CancellationToken cancellationToken)
        {
            var releases = await GetAllAsync(cancellationToken);
            if (releases.Any())
            {
                Releases = SortAndFilterReleases(releases).ToList();
                return Releases.FirstOrDefault();
            }
            return default;
        }

        public abstract Task<DownloadResult> DownloadAsync(ReleaseInfo releaseInfo, string downloadPath, IPackageIndex? packageIndex, IDownloadProgress? downloadProgress,
            CancellationToken cancellationToken);
        public abstract Task<bool> CheckAsync(CancellationToken cancellationToken);

        public bool IsMonitorStarted { get; private set; }
        public int MonitorRefreshTime { get; private set; }

        public void SetCurrentVersion(string version) => CurrentVersion = version;

        public ReleaseInfo? UpdateCurrentVersion(string? fallbackVersion)
        {
            ReleaseInfo? foundVersion = null;
            if (Releases != null && Releases.Any())
            {
                var searchVersion = CurrentVersion ?? fallbackVersion;
                if (searchVersion != null)
                    foundVersion = Releases.SingleOrDefault(li => string.Compare(li.GetVersion(),
                        searchVersion, StringComparison.OrdinalIgnoreCase) == 0);
                foundVersion ??= Releases.FirstOrDefault();
            }
            CurrentVersion = foundVersion?.GetVersion() ?? fallbackVersion;
            return foundVersion;
        }

        public void MonitorStart(int refreshTime)
        {
            if (IsMonitorStarted)
            {
                if (MonitorRefreshTime == refreshTime)
                    return;
                _monitorTimer.Stop();
                _monitorTimer.Interval = TimeSpan.FromMinutes(refreshTime).TotalMilliseconds;
                _monitorTimer.Start();
                IsMonitorStarted = true;
                MonitorRefreshTime = refreshTime;
            }
            else
            {
                _monitorTimer.Interval = TimeSpan.FromMinutes(refreshTime).TotalMilliseconds;
                _monitorTimer.Start();
                IsMonitorStarted = true;
                MonitorRefreshTime = refreshTime;
                MonitorStarted?.Invoke(this, EventArgs.Empty);
#if DEBUG
                CheckForNewVersionAsync();
#endif
            }
        }

        public void MonitorStop()
        {
            if (!IsMonitorStarted)
                return;
            _monitorTimer.Stop();
            IsMonitorStarted = false;
            MonitorStopped?.Invoke(this, EventArgs.Empty);
        }

        private void MonitorTimerOnElapsed(object sender, ElapsedEventArgs e) => CheckForNewVersionAsync();

        private async void CheckForNewVersionAsync()
        {
            if (CurrentVersion == null)
                return;
            try
            {
                var result = await GetLatestAsync(CancellationToken.None);
                if (result != null && CurrentVersion != null &&
                    string.Compare(result.GetVersion(), CurrentVersion, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    MonitorNewVersion?.Invoke(this, result.GetVersion());
                }
            }
            catch (Exception ex)
            {
                //_logger.Error(ex, $"Failed auto check for update repository: {Repository}");
            }
        }

        private IEnumerable<ReleaseInfo> SortAndFilterReleases(IEnumerable<ReleaseInfo> releases)
        {
            if (AllowPreReleases)
                return releases.OrderByDescending(v => v.Released).ThenByDescending(v => v.GetVersion());
            return releases.Where(v => !v.PreRelease).OrderByDescending(v => v.Released).ThenByDescending(v => v.GetVersion());
        }

        public override string ToString() => Name;

        public event EventHandler? MonitorStarted;
        public event EventHandler? MonitorStopped;
        public event EventHandler<string>? MonitorNewVersion;
    }
}
