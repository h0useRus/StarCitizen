using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;
using NSW.StarCitizen.Tools.Lib.Helpers;
using AuthenticationHeaderValue = System.Net.Http.Headers.AuthenticationHeaderValue;

namespace NSW.StarCitizen.Tools.Lib.Update
{
    public class GitHubUpdateRepository : UpdateRepository
    {
        private const string GitHubApiUrl = "https://api.github.com/repos";
        private const string GitHubApiRateLimitUrl = "https://api.github.com/rate_limit";
        private const string GitHubRawContent = "https://raw.githubusercontent.com";
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly HttpClient _httpClient;
        private readonly string _repoReleasesUrl;
        private readonly GitHubUpdateInfo.Factory _gitHubUpdateInfoFactory;
        public GitHubDownloadType DownloadType { get; }
        public string? AuthToken { get; set; }

        public GitHubUpdateRepository(HttpClient httpClient, GitHubDownloadType downloadType,
            GitHubUpdateInfo.Factory gitHubUpdateInfoFactory, string name, string repository) :
            base(UpdateRepositoryType.GitHub, name, repository, GitHubRepositoryUrl.Build(repository))
        {
            DownloadType = downloadType;
            _httpClient = httpClient;
            _repoReleasesUrl = $"{GitHubApiUrl}/{repository}/releases";
            _gitHubUpdateInfoFactory = gitHubUpdateInfoFactory;
        }

        public override async Task<List<UpdateInfo>> GetAllAsync(CancellationToken cancellationToken)
        {
            using var requestMessage = buildRequestMessage(_repoReleasesUrl);
            using var response = await _httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
            await CheckRequestLimitStatusCodeAsync(response, cancellationToken);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var releases = JsonHelper.Read<GitRelease[]>(content);
            if (releases != null && releases.Any())
            {
                return DownloadType switch
                {
                    GitHubDownloadType.Assets => GetAssetUpdates(releases).ToList(),
                    GitHubDownloadType.Sources => GetSourceCodeUpdates(releases).ToList(),
                    _ => throw new NotSupportedException("Not supported download type"),
                };
            }
            return Enumerable.Empty<UpdateInfo>().ToList();
        }

        public override async Task<DownloadResult> DownloadAsync(UpdateInfo updateInfo, string downloadPath, CancellationToken cancellationToken, IDownloadProgress? downloadProgress)
        {
            if (updateInfo is GitHubUpdateInfo gitHubUpdateInfo)
            {
                var diffList = await DownloadIncrementalAsync(gitHubUpdateInfo, downloadPath, cancellationToken, downloadProgress);
                if (diffList != null)
                {
                    return new DownloadResult
                    {
                        DiffList = diffList
                    };
                }
            }
            using var requestMessage = buildRequestMessage(updateInfo.DownloadUrl);
            using var response = await _httpClient.SendAsync(requestMessage,
                HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            await CheckRequestLimitStatusCodeAsync(response, cancellationToken);
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
            return new DownloadResult
            {
                ArchiveFilePath = tempFileName
            };
        }

        public override async Task<bool> CheckAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var requestMessage = buildRequestMessage(_repoReleasesUrl);
                using var response = await _httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                _logger.Warn(e, $"Failed check repository url: {_repoReleasesUrl}");
                return false;
            }
        }

        private async Task<FilesIndex.DiffList?> DownloadIncrementalAsync(GitHubUpdateInfo updateInfo, string downloadPath, CancellationToken cancellationToken, IDownloadProgress? downloadProgress)
        {
            if (PackageIndex == null || updateInfo.IndexDownloadUrl == null)
            {
                return null;
            }
            try
            {
                var sourceIndex = PackageIndex.CreateLocal(cancellationToken);
                if (sourceIndex.IsEmpty())
                {
                    return null;
                }
                // 1. download & parse index file
                var targetIndex = await DownloadFilesIndexAsync(updateInfo.IndexDownloadUrl, cancellationToken);
                if (!PackageIndex.VerifyExternal(targetIndex))
                {
                    _logger.Warn($"Invalid repository index file: {updateInfo.Dump()}");
                    return null;
                }
                // 2. compute diff files list
                var diffFiles = FilesIndex.BuildDiffList(sourceIndex, targetIndex);
                // 3. download missing target files one by one
                downloadProgress?.ReportContentSize(targetIndex.GetFilesSize(diffFiles.ChangedFiles));
                long downloadedBytes = 0;
                foreach (var sourceFilePath in diffFiles.ChangedFiles)
                {
                    var sourceFileUrl = $"{GitHubRawContent}/{Repository}/{updateInfo.TagName}/{sourceFilePath}";
                    var downloadFilePath = Path.Combine(downloadPath, sourceFilePath);
                    Directory.CreateDirectory(Path.GetDirectoryName(downloadFilePath));
                    await DownloadFileAsync(sourceFileUrl, downloadFilePath, cancellationToken);
                    downloadedBytes += targetIndex.GetFileSize(sourceFilePath);
                    downloadProgress?.ReportDownloadedSize(downloadedBytes);
                }
                return diffFiles;
            }
            catch (GitHubRequestLimitExceedException e)
            {
                throw e;
            }
            catch (OperationCanceledException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                _logger.Warn(e, $"Failed download incremental update: {updateInfo.Dump()}");
                if (downloadProgress != null)
                {
                    downloadProgress.ReportDownloadedSize(0);
                    downloadProgress.ReportContentSize(0);
                }
            }
            return null;
        }

        private async Task<FilesIndex> DownloadFilesIndexAsync(string downloadUrl, CancellationToken cancellationToken)
        {
            using var requestMessage = buildRequestMessage(downloadUrl);
            using var response = await _httpClient.SendAsync(requestMessage,
                HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            await CheckRequestLimitStatusCodeAsync(response, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            using var indexStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            using var indexStreamReader = new StreamReader(indexStream);
            return FilesIndex.LoadFromStream(indexStreamReader, cancellationToken);
        }

        private async Task DownloadFileAsync(string downloadUrl, string downloadFilePath, CancellationToken cancellationToken)
        {
            using var requestMessage = buildRequestMessage(downloadUrl);
            using var response = await _httpClient.SendAsync(requestMessage,
                HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            using var contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            try
            {
                using var fileStream = File.Create(downloadFilePath);
                await contentStream.CopyToAsync(fileStream, 0x4000, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                if (File.Exists(downloadFilePath) && !FileUtils.DeleteFileNoThrow(downloadFilePath))
                    _logger.Warn($"Failed remove temporary file: {downloadFilePath}");
                throw;
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

        private async Task<GitRateLimit?> GetRateLimitAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var requestMessage = buildRequestMessage(GitHubApiRateLimitUrl);
                using var response = await _httpClient.SendAsync(requestMessage, cancellationToken)
                    .ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return JsonHelper.Read<GitRateLimit>(await response.Content.ReadAsStringAsync()
                    .ConfigureAwait(false));
            }
            catch (Exception e)
            {
                _logger.Error(e, "Failed get rate limit");
                return null;
            }
        }

        private async Task CheckRequestLimitStatusCodeAsync(HttpResponseMessage message, CancellationToken cancellationToken)
        {
            if (message.StatusCode == HttpStatusCode.Forbidden)
            {
                _logger.Info("Check for rate limit exceed");
                var gitRateLimit = await GetRateLimitAsync(cancellationToken)
                    .ConfigureAwait(false);
                if (gitRateLimit != null && gitRateLimit.Rate.Remaining == 0)
                {
                    var resetDateTime = DateTimeUtils.FromUnixTimeSeconds(gitRateLimit.Rate.Reset).ToLocalTime();
                    _logger.Warn(
                        $"Request rate limit exceed: Limit={gitRateLimit.Rate.Limit}, Used={gitRateLimit.Rate.Used}, Reset={resetDateTime}");
                    throw new GitHubRequestLimitExceedException(
                        $"GitHub requests limit exceeded and will be reset after {resetDateTime.ToShortTimeString()}", resetDateTime);
                }
            }
        }

        private HttpRequestMessage buildRequestMessage(string requestUri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            if (AuthToken != null)
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("token", AuthToken);
            return requestMessage;
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

            public bool IsIndexFileUrl() => ZipUrl != null && ZipUrl.EndsWith("/index.txt");
        }

        public class GitRateLimit
        {
            [JsonProperty("rate")]
            public GitRate Rate { get; }

            [JsonConstructor]
            public GitRateLimit(GitRate rate)
            {
                Rate = rate;
            }
        }

        public class GitRate
        {
            [JsonProperty("limit")]
            public int Limit { get; private set; }
            [JsonProperty("remaining")]
            public int Remaining { get; private set; }
            [JsonProperty("reset")]
            public long Reset { get; private set; }
            [JsonProperty("used")]
            public int Used { get; private set; }
        }
        #endregion
    }
}
