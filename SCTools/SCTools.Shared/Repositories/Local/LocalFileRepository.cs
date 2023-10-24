using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NSW.StarCitizen.Tools.Repositories.Local
{
    public sealed class LocalFileRepository : FileRepository
    {
        public LocalFileRepository(string name, string repository, string repositoryPath, ILogger<LocalFileRepository> logger)
            : base(FileRepositoryType.Local, name, repository, repositoryPath, logger)
        {
        }

        public override Task<bool> CheckRepositoryAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public override Task<DownloadResult> DownloadReleaseAsync(ReleaseInfo releaseInfo, string outputDirectory, IDownloadProgress? downloadProgress = null, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public override Task<IEnumerable<ReleaseInfo>> GetReleasesAsync(bool allowPreRelease = false, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }
}
