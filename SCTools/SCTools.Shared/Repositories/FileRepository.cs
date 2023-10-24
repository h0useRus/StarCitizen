using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NSW.StarCitizen.Tools.Repositories
{
    public abstract class FileRepository : IFileRepository
    {
        private bool _disposed;

        protected readonly ILogger _logger;

        public string Name { get; }
        public string Repository { get; }
        public string RepositoryPath { get; }
        public FileRepositoryType Type { get; }

        protected FileRepository(FileRepositoryType type, string name, string repository, string repositoryPath, ILogger logger)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(repository))
                throw new ArgumentNullException(nameof(repository));
            if (string.IsNullOrWhiteSpace(repositoryPath))
                throw new ArgumentNullException(nameof(repositoryPath));

            Type = type;
            Name = name;
            Repository = repository;
            RepositoryPath = repositoryPath;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public abstract Task<bool> CheckRepositoryAsync(CancellationToken cancellationToken = default);
        public abstract Task<IEnumerable<ReleaseInfo>> GetReleasesAsync(bool allowPreRelease = false, CancellationToken cancellationToken = default);
        public abstract Task<DownloadResult> DownloadReleaseAsync(ReleaseInfo releaseInfo, string outputFolder, IDownloadProgress? downloadProgress = null, CancellationToken cancellationToken = default);

        protected virtual IEnumerable<ReleaseInfo> SortAndFilterReleases(IEnumerable<ReleaseInfo> releases, bool allowPreRelease)
            => releases.Where(ri => allowPreRelease || !ri.PreRelease)
                       .OrderByDescending(ri => (ri.Released, ri.GetVersion()));

        protected virtual void Dispose(bool disposing) { }
        public void Dispose()
        {
            if (_disposed)
                return;
            Dispose(_disposed);
            _disposed = true;
        }
    }
}
