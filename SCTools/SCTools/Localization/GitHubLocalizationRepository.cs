using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NSW.StarCitizen.Tools.Helpers;

namespace NSW.StarCitizen.Tools.Localization
{
    public sealed class GitHubLocalizationRepository : LocalizationRepository
    {
        private const string BaseUrl = "https://api.github.com/repos";
        private static readonly HttpClient _gitClient;
        private readonly string _repoUrl;

        static GitHubLocalizationRepository()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            _gitClient = new HttpClient();
            _gitClient.DefaultRequestHeaders.UserAgent.ParseAdd("SCTools/1.0");
        }

        public GitHubLocalizationRepository(string name, string repository) : base(LocalizationRepositoryType.GitHub, name, repository)
        {
            _repoUrl = $"{BaseUrl}/{repository}/";
        }

        public override async Task<IEnumerable<LocalizationInfo>> GetAllAsync()
        {
            try
            {
                GitRelease[] releases = null;
                using var response = await _gitClient.GetAsync(_repoUrl + "releases");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    releases = JsonHelper.Read<GitRelease[]>(content);
                }

                if (releases != null && releases.Any())
                {
                    return releases.Select(r => new LocalizationInfo
                    {
                        Name = r.Name,
                        TagName = r.TagName,
                        PreRelease = r.PreRelease,
                        Released = r.Published,
                        DownloadUrl = r.ZipUrl
                    });
                }
            }
            catch { }
            return Enumerable.Empty<LocalizationInfo>();
        }

        public override async Task<string> DownloadAsync(LocalizationInfo localizationInfo)
        {
            try
            {
                using var response = await _gitClient.GetAsync(localizationInfo.DownloadUrl);
                if (response.IsSuccessStatusCode)
                {
                    var contentStream = await response.Content.ReadAsStreamAsync();
                    var tempFileName = Path.Combine(Path.GetTempPath(), response.Content.Headers.ContentDisposition.FileName);
                    using var fileStream = File.Create(tempFileName);
                    await contentStream.CopyToAsync(fileStream);
                    return tempFileName;
                }
            }
            catch { }
            return null;
        }

        #region Git objects
        public class GitRelease
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Url { get; set; }
            [JsonProperty("assets_url")]
            public string AssetsUrl { get; set; }
            [JsonProperty("tag_name")]
            public string TagName { get; set; }
            public bool Draft { get; set; }
            [JsonProperty("prerelease")]
            public bool PreRelease { get; set; }
            [JsonProperty("zipball_url")]
            public string ZipUrl { get; set; }
            [JsonProperty("published_at")]
            public DateTimeOffset Published { get; set; }
            [JsonProperty("created_at")]
            public DateTimeOffset Created { get; set; }
        }
        #endregion
    }
}