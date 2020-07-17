using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSW.StarCitizen.Tools.Localization
{
    public interface ILocalizationRepository
    {
        string Name { get; }
        string Repository { get; }
        LocalizationRepositoryType Type { get; }
        LocalizationInfo Current { get; set; }

        Task<IEnumerable<LocalizationInfo>> GetAllAsync();
        Task<string> DownloadAsync(LocalizationInfo localizationInfo);
        void MonitorStart(int refreshTime);
        void MonitorStop();

        event EventHandler<string> MonitorStarted;
        event EventHandler<string> MonitorStopped;
        event EventHandler<string> MonitorNewVersion;
    }
}