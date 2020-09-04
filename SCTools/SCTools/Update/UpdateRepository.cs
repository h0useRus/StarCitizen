using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace NSW.StarCitizen.Tools.Update
{
    public abstract class UpdateRepository : IUpdateRepository
    {
        private readonly System.Timers.Timer _monitorTimer;

        public string Name { get; }
        public string Repository { get; }
        public UpdateRepositoryType Type { get; }
        public string? CurrentVersion { get; private set; }
        public IEnumerable<UpdateInfo>? UpdateReleases { get; private set; }
        public UpdateInfo? LatestUpdateInfo => UpdateReleases?.OrderByDescending(r => r.Released)?.FirstOrDefault();

        protected UpdateRepository(UpdateRepositoryType type, string name, string repository)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(repository))
                throw new ArgumentNullException(nameof(repository));

            Type = type;
            Name = name;
            Repository = repository.Trim('/');

            _monitorTimer = new System.Timers.Timer();
            _monitorTimer.Elapsed += MonitorTimerOnElapsedAsync;
        }

        public void Dispose()
        {
            _monitorTimer.Dispose();
        }

        public abstract Task<IEnumerable<UpdateInfo>> GetAllAsync(CancellationToken cancellationToken);

        public async Task<IEnumerable<UpdateInfo>> RefreshUpdatesAsync(CancellationToken cancellationToken)
        {
            var releases = await GetAllAsync(cancellationToken);
            UpdateReleases = releases.OrderByDescending(v => v.GetVersion()).ThenByDescending(v => v.Released).ToList();
            return UpdateReleases;
        }

        public async Task<UpdateInfo?> GetLatestAsync(CancellationToken cancellationToken)
        {
            var releases = await GetAllAsync(cancellationToken);
            if (releases.Any())
            {
                UpdateReleases = releases.OrderByDescending(v => v.GetVersion()).ThenByDescending(v => v.Released).ToList();
                return releases.OrderByDescending(r => r.Released).First();
            }
            return null;
        }

        public abstract Task<string> DownloadAsync(UpdateInfo updateInfo, string? downloadPath, CancellationToken cancellationToken, IDownloadProgress? downloadProgress);
        public abstract Task<bool> CheckAsync(CancellationToken cancellationToken);

        public bool IsMonitorStarted { get; private set; }
        public int MonitorRefreshTime { get; private set; }

        public void SetCurrentVersion(string version)
        {
            if (version != null)
                CurrentVersion = version;
        }

        public UpdateInfo? UpdateCurrentVersion(string? fallbackVersion)
        {
            UpdateInfo? foundVersion = null;
            if (UpdateReleases != null && UpdateReleases.Any())
            {
                var searchVersion = CurrentVersion ?? fallbackVersion;
                if (searchVersion != null)
                    foundVersion = UpdateReleases.SingleOrDefault(li => string.Compare(li.GetVersion(),
                        searchVersion, StringComparison.OrdinalIgnoreCase) == 0);
                foundVersion ??= UpdateReleases.FirstOrDefault();
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
                MonitorStarted?.Invoke(this, Name);
            }
            MonitorTimerOnElapsedAsync(_monitorTimer, null);
        }

        public void MonitorStop()
        {
            if (!IsMonitorStarted)
                return;
            _monitorTimer.Stop();
            IsMonitorStarted = false;
            MonitorStopped?.Invoke(this, Name);
        }

        private async void MonitorTimerOnElapsedAsync(object sender, ElapsedEventArgs? e)
        {
            try
            {
                using var cancellationTokenSource = new CancellationTokenSource();
                var result = await GetLatestAsync(cancellationTokenSource.Token);
                if (result != null && string.Compare(result.GetVersion(), CurrentVersion, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    MonitorNewVersion?.Invoke(this, result.GetVersion());
                }
            }
            catch { }
        }

        public override string ToString() => Name;

        public event EventHandler<string>? MonitorStarted;
        public event EventHandler<string>? MonitorStopped;
        public event EventHandler<string>? MonitorNewVersion;
    }
}
