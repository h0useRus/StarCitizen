using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSW.StarCitizen.Tools.Extensions;
using NSW.StarCitizen.Tools.Helpers;
using NSW.StarCitizen.Tools.Repositories.GitHub;
using NSW.StarCitizen.Tools.Repositories.GitHub.Models;

namespace NSW.StarCitizen.Tools.Repositories.Gitee
{
    public sealed class GiteeFileRepository : FileRepository
    {
        private const string Host = "gitee.com";
        private const string Url = $"https://{Host}";
        private const string ApiUrl = $"{Url}/api/v5/repos";
        private const int DownloadBufferSize = 0x4000;

        private readonly string _apiReleasesUrl;
        private readonly HttpClient _httpClient;

        public GiteeFileRepository(string name, string repository, HttpClient httpClient, ILogger<GiteeFileRepository> logger)
            : base(FileRepositoryType.Gitee, name, repository, $"{Url}/{repository}", logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiReleasesUrl = $"{ApiUrl}/{repository}/releases";
        }

        public override async Task<bool> CheckRepositoryAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                using var response = await _httpClient.GetAsync(_apiReleasesUrl, cancellationToken).ConfigureAwait(false);
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
                using var response = await _httpClient.GetAsync(_apiReleasesUrl, cancellationToken).ConfigureAwait(false);
                var content = await response
                                        .EnsureSuccessStatusCode()
                                        .Content.ReadAsStringAsync()
                                        .ConfigureAwait(false);
                var releases = content.FromJson<GitRelease[]>();
                if (releases != null && releases.Any())
                {
                    return SortAndFilterReleases(releases
                                                    .Where(GitHubReleaseInfo.IsValid)
                                                    .Select(GitHubReleaseInfo.MapFrom),
                                                 allowPreRelease);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("Failed to load releases from {RepositoryPath}, with {Exception}", _apiReleasesUrl, exception);
            }
            return Enumerable.Empty<ReleaseInfo>();
        }
        public override async Task<DownloadResult> DownloadReleaseAsync(ReleaseInfo releaseInfo, string outputFolder, IDownloadProgress? downloadProgress = null, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.GetAsync(releaseInfo.FilePath, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            using var contentStream = await response
                                                .EnsureSuccessStatusCode()
                                                .Content.ReadAsStreamAsync()
                                                .ConfigureAwait(false);
            if (downloadProgress != null && response.Content.Headers.ContentLength.HasValue)
            {
                downloadProgress.Total(response.Content.Headers.ContentLength.Value);
            }
            var outputFileName = Path.Combine(outputFolder, response.Content.Headers.ContentDisposition.FileName);
            try
            {
                using var fileStream = File.Create(outputFileName);
                await contentStream.CopyToAsync(fileStream, DownloadBufferSize, downloadProgress, cancellationToken);
            }
            catch
            {
                if (File.Exists(outputFileName) && !FileHelper.DeleteFileNoThrow(outputFileName))
                    _logger.LogWarning("Failed to remove output file: {FileName}", outputFileName);
                throw;
            }
            return DownloadResult.FromArchivePath(outputFileName);
        }
    }
}
