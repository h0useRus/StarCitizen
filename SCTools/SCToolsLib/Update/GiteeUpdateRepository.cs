using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;
using NSW.StarCitizen.Tools.Lib.Helpers;

namespace NSW.StarCitizen.Tools.Lib.Update
{
    public class GiteeUpdateRepository : UpdateRepository
    {
        private const string GiteeApiUrl = "https://gitee.com/api/v5/repos";
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly HttpClient _httpClient;
        private readonly string _repoReleasesUrl;
        private readonly GiteeUpdateInfo.Factory _giteeUpdateInfoFactory;

        public GiteeUpdateRepository(HttpClient httpClient, GiteeUpdateInfo.Factory giteeUpdateInfoFactory, string name, string repository)
            : base(UpdateRepositoryType.Gitee, name, repository, GiteeRepositoryUrl.Build(repository))
        {
            _httpClient = httpClient;
            _repoReleasesUrl = $"{GiteeApiUrl}/{repository}/releases";
            _giteeUpdateInfoFactory = giteeUpdateInfoFactory;
        }

        public override async Task<List<UpdateInfo>> GetAllAsync(CancellationToken cancellationToken)
        {
            using var requestMessage = BuildRequestMessage(_repoReleasesUrl);
            using var response = await _httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var releases = JsonHelper.Read<GitRelease[]>(content);
            if (releases != null && releases.Any())
            {
                return GetAssetUpdates(releases).ToList();
            }
            return Enumerable.Empty<UpdateInfo>().ToList();
        }

        public override async Task<DownloadResult> DownloadAsync(UpdateInfo updateInfo, string downloadPath,
            IPackageIndex? packageIndex, CancellationToken cancellationToken, IDownloadProgress? downloadProgress)
        {
            using var requestMessage = BuildRequestMessage(updateInfo.DownloadUrl);
            using var response = await _httpClient.SendAsync(requestMessage,
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
            return DownloadResult.FromArchivePath(tempFileName);
        }

        public override async Task<bool> CheckAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var requestMessage = BuildRequestMessage(_repoReleasesUrl);
                using var response = await _httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                _logger.Warn(e, $"Failed check repository url: {_repoReleasesUrl}");
                return false;
            }
        }

        private IEnumerable<UpdateInfo> GetAssetUpdates(IEnumerable<GitRelease> releases)
        {
            foreach (var r in releases)
            {
                var info = _giteeUpdateInfoFactory.CreateWithDownloadAsset(r);
                if (info != null)
                    yield return info;
            }
        }

        private HttpRequestMessage BuildRequestMessage(string requestUri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            //if (AuthToken != null)
            //    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("token", AuthToken);
            return requestMessage;
        }

        #region Git objects
        public class GitRelease
        {
            [JsonProperty("id")]
            public int Id { get; private set; }
            [JsonProperty("name")]
            public string Name { get; }
            [JsonProperty("tag_name")]
            public string TagName { get; }
            [JsonProperty("prerelease")]
            public bool PreRelease { get; private set; }
            [JsonProperty("created_at")]
            public DateTimeOffset Created { get; private set; }
            [JsonProperty("assets")]
            public GitAsset[] Assets { get; private set; } = new GitAsset[0];

            [JsonConstructor]
            public GitRelease(string name, string tagName)
            {
                Name = name;
                TagName = tagName;
            }
        }

        public class GitAsset
        {
            [JsonProperty("name")]
            public string? Name { get; private set; }
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
