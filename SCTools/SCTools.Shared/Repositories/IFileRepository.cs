using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NSW.StarCitizen.Tools.Repositories
{
    public interface IFileRepository : IDisposable
    {
        string Name { get; }
        string Repository { get; }
        string RepositoryUrl { get; }
        FileRepositoryType Type { get; }
        string? CurrentVersion { get; }
        IEnumerable<ReleaseInfo>? Releases { get; }
        ReleaseInfo? LatestRelease { get; }
        bool AllowPreReleases { get; set; }

        Task<List<ReleaseInfo>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<ReleaseInfo>> RefreshAsync(CancellationToken cancellationToken);
        Task<ReleaseInfo?> GetLatestAsync(CancellationToken cancellationToken);
        Task<DownloadResult> DownloadAsync(ReleaseInfo releaseInfo, string downloadPath, IPackageIndex? packageIndex, IDownloadProgress? downloadProgress, CancellationToken cancellationToken);
        Task<bool> CheckAsync(CancellationToken cancellationToken);
        void SetCurrentVersion(string version);
        ReleaseInfo? UpdateCurrentVersion(string? fallbackVersionName);

        bool IsMonitorStarted { get; }
        int MonitorRefreshTime { get; }
        void MonitorStart(int refreshTime);
        void MonitorStop();

        event EventHandler? MonitorStarted;
        event EventHandler? MonitorStopped;
        event EventHandler<string>? MonitorNewVersion;
    }
}
