using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
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

        public override ILocalizationInstaller Installer { get; } = new DefaultLocalizationInstaller();

        public override async Task<IEnumerable<LocalizationInfo>> GetAllAsync(CancellationToken cancellationToken)
        {
            using var response = await _gitClient.GetAsync(_repoUrl + "releases", cancellationToken);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            GitRelease[] releases = JsonHelper.Read<GitRelease[]>(content);
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
            return Enumerable.Empty<LocalizationInfo>();
        }

        public override async Task<string> DownloadAsync(LocalizationInfo localizationInfo, CancellationToken cancellationToken)
        {
            using var response = await _gitClient.GetAsync(localizationInfo.DownloadUrl, cancellationToken);
            response.EnsureSuccessStatusCode();
            var contentStream = await response.Content.ReadAsStreamAsync();
            var tempFileName = Path.Combine(Path.GetTempPath(), response.Content.Headers.ContentDisposition.FileName);
            try
            {
                using var fileStream = File.Create(tempFileName);
                await contentStream.CopyToAsync(fileStream);
            }
            catch (Exception e)
            {
                if (File.Exists(tempFileName))
                    File.Delete(tempFileName);
                throw e;
            }
            return tempFileName;
        }

        public override async Task<bool> CheckAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var response = await _gitClient.GetAsync(_repoUrl + "releases", cancellationToken);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
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