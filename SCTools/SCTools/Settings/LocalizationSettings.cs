using System;
using System.Collections.Generic;
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
        public DateTime? LastRegularCheckTime { get; set; }

        public bool CanLaunchRegularUpdatesCheck(DateTime nowTime)
            => LastRegularCheckTime == null || nowTime.Subtract(LastRegularCheckTime.Value).TotalDays >= 7;
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
            Mode = mode == GameMode.EPTU ? GameMode.PTU : mode; // Use PTU for ePTU
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

        public string GetUrl() => Type switch
        {
            UpdateRepositoryType.GitHub => GitHubRepositoryUrl.Build(Repository),
            UpdateRepositoryType.Gitee => GiteeRepositoryUrl.Build(Repository),
            UpdateRepositoryType.Folder => FolderRepositoryUrl.Build(Program.ExecutableDir, Repository),
            _ => throw new NotSupportedException("Not supported update repository type"),
        };

        public static LocalizationSource CreateGithub(string name, string repository)
            => new LocalizationSource(name, repository, UpdateRepositoryType.GitHub);

        public static LocalizationSource CreateGitee(string name, string repository)
            => new LocalizationSource(name, repository, UpdateRepositoryType.Gitee);

        public static LocalizationSource CreateFolder(string name, string repository)
            => new LocalizationSource(name, repository, UpdateRepositoryType.Folder);

        public static LocalizationSource? CreateFromUrl(string name, string url)
        {
            string? gitHubRepositoryUrl = GitHubRepositoryUrl.Parse(url);
            if (gitHubRepositoryUrl != null)
            {
                return CreateGithub(name, gitHubRepositoryUrl);
            }
            string? giteeRepositoryUrl = GiteeRepositoryUrl.Parse(url);
            if (giteeRepositoryUrl != null)
            {
                return CreateGitee(name, giteeRepositoryUrl);
            }
            string? repositoryPath = FolderRepositoryUrl.Parse(Program.ExecutableDir, url);
            if (repositoryPath != null)
            {
                return CreateFolder(name, repositoryPath);
            }
            return null;
        }

        public static LocalizationSource DefaultBaseModding { get; } = CreateGithub("Base Modding Package", "defterai/starcitizenmodding");
        public static LocalizationSource DefaultRussianGitHub { get; } = CreateGithub("Russian Community", "n1ghter/sc_ru");
        public static LocalizationSource DefaultRussianGitee { get; } = CreateGitee("Russian Community (зеркало)", "defter/SC_ru");
        public static LocalizationSource MinimalRussian { get; } = CreateGithub("Russian Community (без названий)", "budukratok/SC_not_so_ru");
        public static LocalizationSource DefaultUkrainian { get; } = CreateGithub("Ukrainian Community", "slyf0x-ua/sc_uk");
        public static LocalizationSource DefaultChineseGitee { get; } = CreateGitee("Chinese Community", "mwfaw/SC_CN_zh");
        public static LocalizationSource DefaultChineseGitHub { get; } = CreateGithub("Chinese Community (international)", "Terrencetodd/SC_CN_zh");
        public static LocalizationSource DefaultKorean { get; } = CreateGithub("Korean Community", "xhatagon/sc_ko");
        public static LocalizationSource DefaultPolish { get; } = CreateGithub("Polish Community", "frosty-el-banana/sc_pl");
        public static LocalizationSource DefaultLocal { get; } = CreateFolder("Localizations Folder", "localizations");

        public static IReadOnlyList<LocalizationSource> DefaultList { get; } = new List<LocalizationSource>() {
            DefaultRussianGitHub,
            DefaultUkrainian,
            DefaultChineseGitee,
            DefaultChineseGitHub,
            DefaultKorean,
            DefaultPolish,
        };

        public static IReadOnlyList<LocalizationSource> StandardList { get; } = new List<LocalizationSource>() {
            DefaultRussianGitHub,
            DefaultRussianGitee,
            MinimalRussian,
            DefaultUkrainian,
            DefaultChineseGitee,
            DefaultChineseGitHub,
            DefaultKorean,
            DefaultPolish,
            DefaultBaseModding,
            DefaultLocal
        };
    }
}