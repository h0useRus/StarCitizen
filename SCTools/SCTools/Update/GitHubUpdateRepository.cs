using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NSW.StarCitizen.Tools.Helpers;

namespace NSW.StarCitizen.Tools.Update
{
    public class GitHubUpdateRepository : UpdateRepository
    {
        private const string BaseUrl = "https://api.github.com/repos";
        private readonly string _repoReleasesUrl;
        private readonly GitHubUpdateInfo.Factory _gitHubUpdateInfoFactory;
        public GitHubDownloadType DownloadType { get; }
        public GitHubUpdateRepository(GitHubDownloadType downloadType, GitHubUpdateInfo.Factory gitHubUpdateInfoFactory, string name, string repository) :
            base(UpdateRepositoryType.GitHub, name, repository)
        {
            DownloadType = downloadType;
            _repoReleasesUrl = $"{BaseUrl}/{repository}/releases";
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

        public override async Task<string> DownloadAsync(UpdateInfo updateInfo, string? downloadPath,
            CancellationToken cancellationToken, IDownloadProgress? downloadProgress)
        {
            using var response = await HttpNetClient.Client.GetAsync(updateInfo.DownloadUrl, cancellationToken);
            response.EnsureSuccessStatusCode();
            if (downloadProgress != null && response.Content.Headers.ContentLength.HasValue)
            {
                downloadProgress.ReportContentSize(response.Content.Headers.ContentLength.Value);
            }
            using var contentStream = await response.Content.ReadAsStreamAsync();
            var tempFileName = Path.Combine(downloadPath ?? Path.GetTempPath(), response.Content.Headers.ContentDisposition.FileName);
            try
            {
                using var fileStream = File.Create(tempFileName);
                if (downloadProgress != null)
                {
                    await contentStream.CopyToAsync(fileStream, 0x1000, cancellationToken,
                        new Progress<long>(value => downloadProgress.ReportDownloadedSize(value)));
                }
                else
                {
                    await contentStream.CopyToAsync(fileStream, 0x1000, cancellationToken);
                }
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
                using var response = await HttpNetClient.Client.GetAsync(_repoReleasesUrl, cancellationToken).ConfigureAwait(false);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        private IEnumerable<UpdateInfo> GetSourceCodeUpdates(GitRelease[] releases)
        {
            return releases.Select(r => _gitHubUpdateInfoFactory.CreateWithDownloadSourceCode(r))
                .Where(u => !string.IsNullOrWhiteSpace(u.DownloadUrl));
        }

        private IEnumerable<UpdateInfo> GetAssetUpdates(GitRelease[] releases)
        {
            return releases.Select(r => _gitHubUpdateInfoFactory.CreateWithDownloadAsset(r))
                .Where(u => !string.IsNullOrWhiteSpace(u.DownloadUrl));
        }

        #region Git objects
        public class GitRelease
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Url { get; set; }
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
            [JsonProperty("assets")]
            public GitAsset[] Assets { get; set; }
        }

        public class GitAsset
        {
            [JsonProperty("browser_download_url")]
            public string ZipUrl { get; set; }
        }
        #endregion
    }
}
