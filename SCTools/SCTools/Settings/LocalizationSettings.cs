using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NSW.StarCitizen.Tools.Lib.Global;
using NSW.StarCitizen.Tools.Lib.Update;

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
        public UpdateRepositoryType Type { get; }
        [JsonProperty]
        public string? InstalledVersion { get; set; }
        [JsonProperty]
        public string? LastVersion { get; set; }
        [JsonProperty]
        public int MonitorRefreshTime { get; set; }
        [JsonProperty]
        public bool MonitorForUpdates { get; set; }
        [JsonProperty]
        public bool AllowPreRelease { get; set; }

        [JsonConstructor]
        public LocalizationInstallation(GameMode mode, string repository, UpdateRepositoryType? type)
        {
            Mode = mode;
            Repository = repository;
            Type = type ?? UpdateRepositoryType.GitHub;
        }
    }

    public class LocalizationSource
    {
        [JsonProperty]
        public string Name { get; }
        [JsonProperty]
        public string Repository { get; }
        [JsonProperty]
        public UpdateRepositoryType Type { get; }

        [JsonConstructor]
        public LocalizationSource(string name, string repository, UpdateRepositoryType type)
        {
            Name = name;
            Repository = repository;
            Type = type;
        }

        public string GetUrl() => Type == UpdateRepositoryType.GitHub ?
            GitHubRepositoryUrl.Build(Repository) :
            FolderRepositoryUrl.Build(Program.ExecutableDir, Repository);

        public static LocalizationSource CreateGithub(string name, string repository)
            => new LocalizationSource(name, repository, UpdateRepositoryType.GitHub);

        public static LocalizationSource CreateFolder(string name, string repository)
            => new LocalizationSource(name, repository, UpdateRepositoryType.Folder);

        public static LocalizationSource? CreateFromUrl(string name, string url)
        {
            string? repositoryUrl = GitHubRepositoryUrl.Parse(url);
            if (repositoryUrl != null)
            {
                return CreateGithub(name, repositoryUrl);
            }
            string? repositoryPath = FolderRepositoryUrl.Parse(Program.ExecutableDir, url);
            if (repositoryPath != null)
            {
                return CreateFolder(name, repositoryPath);
            }
            return null;
        }

        public static LocalizationSource DefaultBaseModding { get; } = CreateGithub("Base Modding Package", "defterai/starcitizenmodding");
        public static LocalizationSource DefaultRussian { get; } = CreateGithub("Russian Community", "n1ghter/sc_ru");
        public static LocalizationSource MinimalRussian { get; } = CreateGithub("Russian Community (без названий)", "budukratok/SC_not_so_ru");
        public static LocalizationSource DefaultUkrainian { get; } = CreateGithub("Ukrainian Community", "slyf0x-ua/sc_uk");
        public static LocalizationSource DefaultKorean { get; } = CreateGithub("Korean Community", "xhatagon/sc_ko");
        public static LocalizationSource DefaultPolish { get; } = CreateGithub("Polish Community", "frosty-el-banana/sc_pl");
        public static LocalizationSource DefaultLocal { get; } = CreateFolder("Localizations Folder", "localizations");

        public static IReadOnlyList<LocalizationSource> DefaultList { get; } = new List<LocalizationSource>() {
            DefaultRussian,
            DefaultUkrainian,
            DefaultKorean,
            DefaultPolish,
        };

        public static IReadOnlyList<LocalizationSource> StandardList { get; } = new List<LocalizationSource>() {
            DefaultRussian,
            MinimalRussian,
            DefaultUkrainian,
            DefaultKorean,
            DefaultPolish,
            DefaultBaseModding,
            DefaultLocal
        };
    }
}