using System.Collections.Generic;
using NSW.StarCitizen.Tools.Global;

namespace NSW.StarCitizen.Tools.Settings
{
    public class LocalizationSettings
    {
        public List<LocalizationSource> Repositories { get; set; } = new List<LocalizationSource>();
        public List<LocalizationInstallation> Installations { get; set; } = new List<LocalizationInstallation>();
        public int MonitorRefreshTime { get; set; } = 5;
    }

    public class LocalizationInstallation
    {
        public GameMode Mode { get; set; }
        public string? InstalledVersion { get; set; }
        public string? LastVersion { get; set; }
        public string Repository { get; set; }
        public bool MonitorForUpdates { get; set; }
        public int MonitorRefreshTime { get; set; }
    }

    public class LocalizationSource
    {
        public string Name { get; set; }
        public string Repository { get; set; }
        public string Type { get; set; }

        public static LocalizationSource DefaultBaseModding { get; } = new LocalizationSource
        {
            Name = "Base Modding Package",
            Repository = "defterai/starcitizenmodding",
            Type = "GitHub"
        };

        public static LocalizationSource DefaultRussian { get; } = new LocalizationSource
        {
            Name = "Russian Community",
            Repository = "n1ghter/sc_ru",
            Type = "GitHub"
        };

        public static LocalizationSource DefaultChinese { get; } = new LocalizationSource
        {
            Name = "Chinese Community",
            Repository = "terrencetodd/sc_cn_zh",
            Type = "GitHub"
        };

        public static IReadOnlyList<LocalizationSource> DefaultList { get; } = new List<LocalizationSource>() {
            DefaultRussian,
            DefaultChinese,
        };

        public static IReadOnlyList<LocalizationSource> StandardList { get; } = new List<LocalizationSource>() {
            DefaultRussian,
            DefaultChinese,
            DefaultBaseModding
        };
    }
}