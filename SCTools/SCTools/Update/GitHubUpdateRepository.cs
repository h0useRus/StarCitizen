using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;
using NSW.StarCitizen.Tools.Helpers;

namespace NSW.StarCitizen.Tools.Update
{
    public class GitHubUpdateRepository : UpdateRepository
    {
        private const string GitHubApiUrl = "https://api.github.com/repos";
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly string _repoReleasesUrl;
        private readonly GitHubUpdateInfo.Factory _gitHubUpdateInfoFactory;
        public GitHubDownloadType DownloadType { get; }
        public GitHubUpdateRepository(GitHubDownloadType downloadType, GitHubUpdateInfo.Factory gitHubUpdateInfoFactory, string name, string repository) :
            base(UpdateRepositoryType.GitHub, name, repository, GitHubRepositoryUrl.Build(repository))
        {
            DownloadType = downloadType;
            _repoReleasesUrl = $"{GitHubApiUrl}/{repository}/releases";
            _gitHubUpdateInfoFactory = gitHubUpdateInfoFactory;
        }

        public override async Task<IEnumerable<UpdateInfo>> GetAllAsync(CancellationToken cancellationToken)
        {
            using var response = await HttpNetClient.Client.GetAsync(_repoReleasesUrl, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var releases = JsonHelper.Read<GitRelease[]>(content);
            if (releases != null && releases.Any())
            {
                return DownloadType switch
                {
                    GitHubDownloadType.Assets => GetAssetUpdates(releases),
                    GitHubDownloadType.Sources => GetSourceCodeUpdates(releases),
                    _ => throw new NotSupportedException("Not supported download type"),
                };
            }
            return Enumerable.Empty<UpdateInfo>();
        }

        public override async Task<string> DownloadAsync(UpdateInfo updateInfo, string downloadPath,
            CancellationToken cancellationToken, IDownloadProgress? downloadProgress)
        {
            using var response = await HttpNetClient.Client.GetAsync(updateInfo.DownloadUrl,
                HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();
            using var contentStream = await response.Content.ReadAsStreamAsync();
            if (downloadProgress != null && response.Content.Headers.ContentLength.HasValue)
            {
                downloadProgress.ReportContentSize(response.Content.Headers.ContentLength.Value);
            }
            var tempFileName = Path.Combine(downloadPath, response.Content.Headers.ContentDisposition.FileName);
            try
            {
                using var fileStream = File.Create(tempFileName);
                if (downloadProgress != null)
                {
                    await contentStream.CopyToAsync(fileStream, 0x4000, cancellationToken,
                        new Progress<long>(downloadProgress.ReportDownloadedSize));
                }
                else
                {
                    await contentStream.CopyToAsync(fileStream, 0x4000, cancellationToken);
                }
            }
            catch
            {
                if (File.Exists(tempFileName) && !FileUtils.DeleteFileNoThrow(tempFileName))
                    _logger.Warn($"Failed remove temporary file: {tempFileName}");
                throw;
            }
            return tempFileName;
        }

        public override async Task<bool> CheckAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var response = await HttpNetClient.Client.GetAsync(_repoReleasesUrl, cancellationToken).ConfigureAwait(false);
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                _logger.Warn(e, $"Failed check repository url: {_repoReleasesUrl}");
                return false;
            }
        }
        private IEnumerable<UpdateInfo> GetSourceCodeUpdates(IEnumerable<GitRelease> releases)
        {
            foreach (var r in releases)
            {
                var info = _gitHubUpdateInfoFactory.CreateWithDownloadSourceCode(r);
                if (info != null) yield return info;
            }
        }

        private IEnumerable<UpdateInfo> GetAssetUpdates(IEnumerable<GitRelease> releases)
        {
            foreach (var r in releases)
            {
                var info = _gitHubUpdateInfoFactory.CreateWithDownloadAsset(r);
                if (info != null) yield return info;
            }
        }

        #region Git objects
        public class GitRelease
        {
            [JsonProperty("id")]
            public int Id { get; private set; }
            [JsonProperty("name")]
            public string Name { get; }
            [JsonProperty("url")]
            public string Url { get; }
            [JsonProperty("tag_name")]
            public string TagName { get; }
            [JsonProperty("draft")]
            public bool Draft { get; private set; }
            [JsonProperty("prerelease")]
            public bool PreRelease { get; private set; }
            [JsonProperty("zipball_url")]
            public string ZipUrl { get; }
            [JsonProperty("published_at")]
            public DateTimeOffset Published { get; private set; }
            [JsonProperty("created_at")]
            public DateTimeOffset Created { get; private set; }
            [JsonProperty("assets")]
            public GitAsset[] Assets { get; private set; } = new GitAsset[0];

            [JsonConstructor]
            public GitRelease(string name, string url, string tagName, string zipUrl)
            {
                Name = name;
                Url = url;
                TagName = tagName;
                ZipUrl = zipUrl;
            }
        }

        public class GitAsset
        {
            [JsonProperty("browser_download_url")]
            public string ZipUrl { get; }

            [JsonConstructor]
            public GitAsset(string zipUrl)
            {
                ZipUrl = zipUrl;
            }
        }
        #endregion
    }
}
