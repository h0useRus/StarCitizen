using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace NSW.StarCitizen.Tools.Localization
{
    public abstract class LocalizationRepository : ILocalizationRepository
    {
        private readonly Timer _monitorTimer;
        
        public string Name { get; }
        public string Repository { get; }
        public LocalizationRepositoryType Type { get; }
        public abstract ILocalizationInstaller Installer { get; }
        public LocalizationInfo CurrentVersion { get; set; }
        public IEnumerable<LocalizationInfo> Versions { get; set; }

        protected LocalizationRepository(LocalizationRepositoryType type, string name, string repository)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(repository)) throw new ArgumentNullException(nameof(repository));

            Type = type;
            Name = name;
            Repository = repository.Trim('/');

            _monitorTimer = new Timer();
            _monitorTimer.Elapsed += MonitorTimerOnElapsedAsync;

        }

        public abstract Task<IEnumerable<LocalizationInfo>> GetAllAsync();

        public async Task<IEnumerable<LocalizationInfo>> RefreshVersionsAsync()
        {
            Versions = (await GetAllAsync()).OrderByDescending(v => v.Name).ThenByDescending(v => v.Released).ToList();
            return Versions;
        }

        public abstract Task<string> DownloadAsync(LocalizationInfo localizationInfo);
        public abstract Task<bool> CheckAsync();

        public bool IsMonitorStarted { get; private set; }
        public int MonitorRefreshTime { get; private set; }

        protected async Task<LocalizationInfo> GetLatestAsync()
        {
            var releases = await GetAllAsync();
            return releases?.OrderByDescending(r => r.Released).First();
        }

        public void MonitorStart(int refreshTime)
        {
            if(IsMonitorStarted && MonitorRefreshTime == refreshTime) return;
            MonitorStop();
            _monitorTimer.Interval = TimeSpan.FromMinutes(refreshTime).TotalMilliseconds;
            _monitorTimer.Start();
            IsMonitorStarted = true;
            MonitorRefreshTime = refreshTime;
            MonitorStarted?.Invoke(this, Name);
            MonitorTimerOnElapsedAsync(_monitorTimer, null);
        }

        public void MonitorStop()
        {
            if(!IsMonitorStarted) return;
            _monitorTimer.Stop();
            IsMonitorStarted = false;
            MonitorStopped?.Invoke(this, Name);
        }

        private async void MonitorTimerOnElapsedAsync(object sender, ElapsedEventArgs e)
        {
            var result = await GetLatestAsync();

            if (result != null)
            {
                if(string.Compare(result.Name, CurrentVersion?.Name, StringComparison.OrdinalIgnoreCase) != 0)
                    MonitorNewVersion?.Invoke(this, result.Name);
            }
        }

        public override string ToString() => Name;

        public event EventHandler<string> MonitorStarted;
        public event EventHandler<string> MonitorStopped;
        public event EventHandler<string> MonitorNewVersion;
    }
}