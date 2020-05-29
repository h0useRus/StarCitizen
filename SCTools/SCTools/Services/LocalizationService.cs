using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NSW.StarCitizen.Tools.Helpers;

namespace NSW.StarCitizen.Tools.Services
{
    public enum PatchStatus
    {
        NotSupported,
        Original,
        Patched
    }

    public enum LocalizationStatus
    {
        NotInstalled,
        Actual,
        Outdated
    }

    public class PatchInfo
    {
        public PatchStatus Status { get; set; } = PatchStatus.NotSupported;
        public FileInfo File { get; set; }
        public long Index { get; set; } = -1;
    }

    public class ReleaseInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        [JsonProperty("assets_url")]
        public string AssetsUrl { get; set; }
        [JsonProperty("tag_name")]
        public string TagName { get; set; }
        [JsonProperty("prerelease")]
        public bool PreRelease { get; set; }
        [JsonProperty("zipball_url")]
        public string ZipUrl { get; set; }
        [JsonProperty("published_at")]
        public DateTimeOffset Published { get; set; }
    }

    public class LocalizationInfo
    {
        public LocalizationStatus Status { get; set; }
        public ReleaseInfo Release { get; set; }
    }

    public class LanguagesInfo
    {
        public List<string> Languages { get; set; } = new List<string>();
        public string Current { get; set; }
        public string New { get; set; }
    }

    public class LocalizationService
    {
        private const string BaseUrl = "https://api.github.com/repos";
        private const string KeySysLanguages = "sys_languages";
        private const string KeyCurLanguage = "g_language";

        public static LocalizationService Instance { get; } = new LocalizationService();

        private readonly HttpClient _gitClient;
        private readonly string _repoUrl;
        private LocalizationService()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            _gitClient = new HttpClient();
            _gitClient.DefaultRequestHeaders.UserAgent.ParseAdd("SCTools/1.0");
            _repoUrl = $"{BaseUrl}/{SettingsService.Instance.AppSettings.Localization.Author}/{SettingsService.Instance.AppSettings.Localization.Repo}/";
        }

        public PatchInfo GetPatchSupport(GameInfo gameInfo)
        {
            using var stream = gameInfo.ExeFile.OpenRead();
            //check for original
            var index = StreamHelper.IndexOf(stream, SettingsService.Instance.AppSettings.Localization.OriginalPattern);
            if (index > 0)
                return new PatchInfo { Status = PatchStatus.Original, File = gameInfo.ExeFile, Index = index};
            //check for patch
            stream.Seek(0, SeekOrigin.Begin);
            index = StreamHelper.IndexOf(stream, SettingsService.Instance.AppSettings.Localization.PatchPattern);
            if (index > 0)
                return new PatchInfo { Status = PatchStatus.Patched, File = gameInfo.ExeFile, Index = index };
            // not found any
            return new PatchInfo();
        }

        public PatchInfo Patch(PatchInfo patchInfo)
        {
            if (patchInfo.Status == PatchStatus.Original)
            {
                if (StreamHelper.UpdateFile(patchInfo.File, patchInfo.Index, SettingsService.Instance.AppSettings.Localization.PatchPattern))
                {
                    patchInfo.Status = PatchStatus.Patched;
                }
            }
            else if (patchInfo.Status == PatchStatus.Patched)
            {
                if (StreamHelper.UpdateFile(patchInfo.File, patchInfo.Index, SettingsService.Instance.AppSettings.Localization.OriginalPattern))
                {
                    patchInfo.Status = PatchStatus.Original;
                }
            }

            return patchInfo;
        }

        public async Task<ReleaseInfo[]> GetReleasesAsync()
        {
            try
            {
                using var response = await _gitClient.GetAsync(_repoUrl + "releases");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ReleaseInfo[]>(content);
                }
            }
            catch { }

            return null;
        }

        public async Task<ReleaseInfo> GetLastReleaseAsync()
        {
            var releases = await GetReleasesAsync();
            return releases?.Length > 0 ? releases[0] : null;
        }

        public async Task<string> DownloadAsync(ReleaseInfo releaseInfo)
        {
            try
            {
                using var response = await _gitClient.GetAsync(releaseInfo.ZipUrl);
                if (response.IsSuccessStatusCode)
                {
                    var contentStream = await response.Content.ReadAsStreamAsync();
                    var tempFileName = Path.Combine(Path.GetTempPath(),
                        response.Content.Headers.ContentDisposition.FileName);
                    using var fileStream = File.Create(tempFileName);
                    await contentStream.CopyToAsync(fileStream);
                    return tempFileName;
                }
            }
            catch { }

            return null;
        }

        public bool UnZipFile(string destinationFolder, string zipFileName)
        {
            try
            {
                using var archive = ZipFile.OpenRead(zipFileName);
                var rootEntry = archive.Entries[0];
                //extract only data folder
                foreach (var entry in archive.Entries.Where(e => e.FullName.Contains(@"/data/")))
                {
                    if (string.IsNullOrWhiteSpace(entry.Name))
                    {
                        var dir = Path.Combine(destinationFolder, entry.FullName.Replace(rootEntry.FullName, string.Empty));
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);
                    }
                    else
                    {
                        entry.ExtractToFile(Path.Combine(destinationFolder, entry.FullName.Replace(rootEntry.FullName, string.Empty)), true);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsLocalizationInstalled(GameInfo gameInfo)
            => Directory.Exists(Path.Combine(gameInfo.RootFolder.FullName, "data", "Localization"));

        public async Task<LocalizationInfo> GetLocalizationStatusAsync(GameInfo gameInfo)
        {
            var lastRelease = await GetLastReleaseAsync();
            if (lastRelease != null)
            {
                var result = new LocalizationInfo
                {
                    Release = lastRelease,
                    Status = string.Compare(lastRelease.Name,
                        SettingsService.Instance.AppSettings.Localization.LastVersion,
                        StringComparison.OrdinalIgnoreCase) == 0
                        ? LocalizationStatus.Actual
                        : LocalizationStatus.Outdated
                };
                if (!IsLocalizationInstalled(gameInfo))
                    result.Status = LocalizationStatus.NotInstalled;
                return result;
            }

            return new LocalizationInfo { Status = LocalizationStatus.NotInstalled };
        }

        public LanguagesInfo GetLanguagesConfiguration(GameInfo gameInfo)
        {
            var result = new LanguagesInfo();
            var fileName = Path.Combine(gameInfo.RootFolder.FullName, "data", "system.cfg");
            var cfgFile = new CfgReader(fileName);
            var keys = cfgFile.ReadKeys();
            if (keys.ContainsKey(KeySysLanguages) && !string.IsNullOrWhiteSpace(keys[KeySysLanguages]))
            {
                var languages = keys[KeySysLanguages].Split(',');
                foreach (var language in languages)
                {
                    result.Languages.Add(language.Trim());
                }
            }

            if (keys.ContainsKey(KeyCurLanguage))
            {
                result.Current = keys[KeyCurLanguage];
            }

            return result;
        }

        public async Task<LanguagesInfo> UpdateLanguageAsync(GameInfo gameInfo, LanguagesInfo languages)
        {
            if (string.IsNullOrWhiteSpace(languages.New) || string.Compare(languages.Current, languages.New, StringComparison.OrdinalIgnoreCase) == 0)
            {
                languages.New = null;
                return languages;
            }


            var fileName = Path.Combine(gameInfo.RootFolder.FullName, "data", "system.cfg");
            var cfgFile = new CfgReader(fileName);
            if (await cfgFile.UpdateKeyAsync(KeyCurLanguage, languages.New))
            {
                languages.Current = languages.New;
            }

            languages.New = null;
            return languages;
        }
    }
}