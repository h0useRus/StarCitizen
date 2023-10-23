using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NSW.StarCitizen.Tools.Repositories
{
    public interface IFileRepository : IDisposable
    {
        /// <summary>
        /// Display name
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Repository name
        /// </summary>
        string Repository { get; }
        /// <summary>
        /// Repository location
        /// </summary>
        string RepositoryPath { get; }
        /// <summary>
        /// Repository type
        /// </summary>
        FileRepositoryType Type { get; }
        /// <summary>
        /// Check repository exists.
        /// </summary>
        Task<bool> CheckRepositoryAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// Return all <see cref="ReleaseInfo"/> files from remote repository.
        /// </summary>
        Task<IEnumerable<ReleaseInfo>> GetReleasesAsync(bool allowPreRelease = false, CancellationToken cancellationToken = default);
        /// <summary>
        /// Download <see cref="ReleaseInfo"/> file to output path.
        /// </summary>
        Task<DownloadResult> DownloadReleaseAsync(ReleaseInfo releaseInfo, string outputDirectory, IDownloadProgress? downloadProgress = null, CancellationToken cancellationToken = default);
    }
}
