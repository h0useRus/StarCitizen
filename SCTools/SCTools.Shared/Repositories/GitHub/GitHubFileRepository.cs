using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSW.StarCitizen.Tools.Exceptions;
using NSW.StarCitizen.Tools.Extensions;
using NSW.StarCitizen.Tools.Helpers;
using NSW.StarCitizen.Tools.Repositories.GitHub.Models;

namespace NSW.StarCitizen.Tools.Repositories.GitHub
{
    public sealed class GitHubFileRepository : FileRepository
    {
        private const string Host = "github.com";
        private const string Url = $"https://{Host}";
        private const string ApiUrl = $"https://api.{Host}";
        private const string ApiReposUrl = $"{ApiUrl}/repos";
        private const string ApiRateLimitUrl = $"{ApiUrl}/rate_limit";
        private const string RawContent = "https://raw.githubusercontent.com";
        private const int DownloadBufferSize = 0x4000;

        private readonly string _apiReleasesUrl;
        private readonly string? _authenticationToken;
        private readonly HttpClient _httpClient;

        public GitHubFileRepository(string name, string repository, HttpClient httpClient, string? authenticationToken, ILogger<GitHubFileRepository> logger)
            : base(FileRepositoryType.GitHub, name, repository, $"{Url}/{repository}", logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiReleasesUrl = $"{ApiReposUrl}/{repository}/releases";
            _authenticationToken = authenticationToken;
        }

        public override async Task<bool> CheckRepositoryAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                using var requestMessage = BuildGetRequestMessage(_apiReleasesUrl);
                using var response = await _httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
                return response.IsSuccessStatusCode;
            }
            catch (Exception exception)
            {
                _logger.LogWarning("Failed check repository url: {RepositoryPath}, Exception: {Exception}", _apiReleasesUrl, exception);
                return false;
            }
        }
        public override async Task<IEnumerable<ReleaseInfo>> GetReleasesAsync(bool allowPreRelease = false, CancellationToken cancellationToken = default)
        {
            try
            {
                using var requestMessage = BuildGetRequestMessage(_apiReleasesUrl);
                using var response = await _httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
                await CheckRequestLimitStatusCodeAsync(response, cancellationToken);
                var content = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync().ConfigureAwait(false);
                var releases = content.FromJson<GitRelease[]>();
                if (releases != null && releases.Any())
                {
                    return SortAndFilterReleases(releases
                                                    .Where(GitHubReleaseInfo.IsValid)
                                                    .Select(r => GitHubReleaseInfo.MapFrom(r, true)),
                                                 allowPreRelease);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("Failed to load releases from {RepositoryPath}, with {Exception}", _apiReleasesUrl, exception);
            }
            return Enumerable.Empty<ReleaseInfo>();
        }
        public override async Task<DownloadResult> DownloadReleaseAsync(ReleaseInfo releaseInfo, string outputDirectory, IDownloadProgress? downloadProgress = null, CancellationToken cancellationToken = default)
        {
            using var requestMessage = BuildGetRequestMessage(releaseInfo.FilePath);
            using var response = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            await CheckRequestLimitStatusCodeAsync(response, cancellationToken);
            using var contentStream = await response.EnsureSuccessStatusCode().Content.ReadAsStreamAsync().ConfigureAwait(false);
            if (downloadProgress != null && response.Content.Headers.ContentLength.HasValue)
            {
                downloadProgress.Total(response.Content.Headers.ContentLength.Value);
            }
            var tempFileName = Path.Combine(outputDirectory, response.Content.Headers.ContentDisposition.FileName);
            try
            {
                using var fileStream = File.Create(tempFileName);
                await contentStream.CopyToAsync(fileStream, DownloadBufferSize, downloadProgress, cancellationToken);
            }
            catch
            {
                if (File.Exists(tempFileName) && !FileHelper.DeleteFileNoThrow(tempFileName))
                    _logger.LogWarning("Failed remove temporary file: {FileName}", tempFileName);
                throw;
            }
            return DownloadResult.FromArchivePath(tempFileName);
        }

        private HttpRequestMessage BuildGetRequestMessage(string requestUri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            if (_authenticationToken != null)
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("token", _authenticationToken);
            return requestMessage;
        }
        private async Task CheckRequestLimitStatusCodeAsync(HttpResponseMessage message, CancellationToken cancellationToken)
        {
            if (message.StatusCode == HttpStatusCode.Forbidden)
            {
                _logger.LogInformation("Check for rate limit exceed.");
                var gitRateLimit = await GetRateLimitAsync(cancellationToken).ConfigureAwait(false);
                if (gitRateLimit != null && gitRateLimit.Rate.Remaining == 0)
                {
                    var resetDateTime = DateTimeOffset.FromUnixTimeSeconds(gitRateLimit.Rate.Reset).ToLocalTime();
                    _logger.LogWarning("Request rate limit exceed: Limit={RateLimit}, Used={RateUsed}, Reset={ResetDateTime}",
                        gitRateLimit.Rate.Limit, gitRateLimit.Rate.Used, resetDateTime);
                    throw new GitHubRequestLimitExceedException($"GitHub requests limit exceeded and will be reset after {resetDateTime:HH:mm:ss}", resetDateTime);
                }
            }
        }
        private async Task<GitRateLimit?> GetRateLimitAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var requestMessage = BuildGetRequestMessage(ApiRateLimitUrl);
                using var response = await _httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
                var json = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync().ConfigureAwait(false);
                return json.FromJson<GitRateLimit>();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed get rate limit");
                return null;
            }
        }
    }
}
