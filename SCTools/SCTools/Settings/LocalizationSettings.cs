using System.Collections.Generic;
using Newtonsoft.Json;
using NSW.StarCitizen.Tools.Global;

namespace NSW.StarCitizen.Tools.Settings
{
    public class LocalizationSettings
    {
        [JsonProperty]
        public List<LocalizationSource> Repositories { get; } = new List<LocalizationSource>();
        [JsonProperty]
        public List<LocalizationInstallation> Installations { get; } = new List<LocalizationInstallation>();
        [JsonProperty]
        public int MonitorRefreshTime { get; set; } = 5;
    }

    public class LocalizationInstallation
    {
        [JsonProperty]
        public GameMode Mode { get; }
        [JsonProperty]
        public string Repository { get; }
        [JsonProperty]
        public string? InstalledVersion { get; set; }
        [JsonProperty]
        public string? LastVersion { get; set; }
        [JsonProperty]
        public int MonitorRefreshTime { get; set; }
        [JsonProperty]
        public bool MonitorForUpdates { get; set; }
        [JsonProperty]
        public bool AllowPreRelease { get; set; } = false;

        [JsonConstructor]
        public LocalizationInstallation(GameMode mode, string repository)
        {
            Mode = mode;
            Repository = repository;
        }
    }

    public class LocalizationSource
    {
        [JsonProperty]
        public string Name { get; }
        [JsonProperty]
        public string Repository { get; }
        [JsonProperty]
        public string Type { get; }

        [JsonConstructor]
        public LocalizationSource(string name, string repository, string type)
        {
            Name = name;
            Repository = repository;
            Type = type;
        }

        public static LocalizationSource DefaultBaseModding { get; } = new LocalizationSource("Base Modding Package", "defterai/starcitizenmodding", "GitHub");
        public static LocalizationSource DefaultRussian { get; } = new LocalizationSource("Russian Community", "n1ghter/sc_ru", "GitHub");
        public static LocalizationSource DefaultUkrainian { get; } = new LocalizationSource("Ukrainian Community", "slyf0x-ua/sc_uk", "GitHub");
        public static LocalizationSource DefaultKorean { get; } = new LocalizationSource("Korean Community", "xhatagon/sc_ko", "GitHub");
        public static LocalizationSource DefaultPolish { get; } = new LocalizationSource("Polish Community", "frosty-el-banana/sc_pl", "GitHub");
        public static LocalizationSource DefaultChinese { get; } = new LocalizationSource("Chinese Community", "terrencetodd/sc_cn_zh", "GitHub");

        public static IReadOnlyList<LocalizationSource> DefaultList { get; } = new List<LocalizationSource>() {
            DefaultRussian,
            DefaultUkrainian,
            DefaultKorean,
            DefaultPolish,
            DefaultChinese,
        };

        public static IReadOnlyList<LocalizationSource> StandardList { get; } = new List<LocalizationSource>() {
            DefaultRussian,
            DefaultUkrainian,
            DefaultKorean,
            DefaultPolish,
            DefaultChinese,
            DefaultBaseModding
        };
    }
}