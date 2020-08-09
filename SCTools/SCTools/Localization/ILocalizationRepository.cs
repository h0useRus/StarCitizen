using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NSW.StarCitizen.Tools.Localization
{
    public interface IDownloadProgress
    {
        //
        // Summary:
        //     Reports a content download size in bytes.
        //
        // Parameters:
        //   value:
        //     The value of the content size.
        void ReportContentSize(long value);
        //
        // Summary:
        //     Reports a downloaded size in bytes.
        //
        // Parameters:
        //   value:
        //     The value of the download progress.
        void ReportDownloadedSize(long value);
    }

    public interface ILocalizationRepository
    {
        string Name { get; }
        string Repository { get; }
        LocalizationRepositoryType Type { get; }
        ILocalizationInstaller Installer { get; }
        LocalizationInfo CurrentVersion { get; set; }
        IEnumerable<LocalizationInfo> Versions { get; set; }

        Task<IEnumerable<LocalizationInfo>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<LocalizationInfo>> RefreshVersionsAsync(CancellationToken cancellationToken);
        Task<string> DownloadAsync(LocalizationInfo localizationInfo, CancellationToken cancellationToken, IDownloadProgress downloadProgress);
        Task<bool> CheckAsync(CancellationToken cancellationToken);

        bool IsMonitorStarted { get; }
        int MonitorRefreshTime { get; }
        void MonitorStart(int refreshTime);
        void MonitorStop();

        event EventHandler<string> MonitorStarted;
        event EventHandler<string> MonitorStopped;
        event EventHandler<string> MonitorNewVersion;
    }
}