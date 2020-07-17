using System.Collections.Generic;
using NSW.StarCitizen.Tools.Global;

namespace NSW.StarCitizen.Tools.Settings
{
    public class LocalizationSettings
    {
        public List<LocalizationSource> Repositories { get; set; }
        public List<LocalizationInfo> Infos { get; set; }
        public bool MonitorForUpdates { get; set; } = false;
        public int MonitorRefreshTime { get; set; } = 5;
    }

    public class LocalizationInfo
    {
        public GameMode Mode { get; set; }
        public string LastVersion { get; set; }
        public string Source { get; set; }
        public bool MonitorForUpdates { get; set; }
        public int MonitorRefreshTime { get; set; }
    }

    public class LocalizationSource
    {
        public string Name { get; set; }
        public string Repository { get; set; }
        public string Type { get; set; }
        public static LocalizationSource Default { get; } = new LocalizationSource
        {
            Name = "Russian Community",
            Repository = "defterai/StarCitizenModding",
            Type = "GitHub"
        };
    }
}