using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NSW.StarCitizen.Tools.Lib.Global;
using NSW.StarCitizen.Tools.Lib.Update;

namespace NSW.StarCitizen.Tools.Lib.Localization
{
    public sealed class FolderLocalizationRepository : UpdateRepository, ILocalizationRepository
    {
        public GameMode Mode { get; }
        public ILocalizationInstaller Installer { get; } = new DefaultLocalizationInstaller();
        private readonly FolderUpdateInfo.Factory _folderUpdateInfoFactory = FolderUpdateInfo.Factory.New();

        public FolderLocalizationRepository(GameMode mode, string name, string repository) :
            base(UpdateRepositoryType.Folder, name, repository, repository)
        {
            Mode = mode;
        }

        public override Task<List<UpdateInfo>> GetAllAsync(CancellationToken cancellationToken)
        {
            if (!Directory.Exists(RepositoryUrl))
            {
                throw new InvalidOperationException($"Repository path not exist: {RepositoryUrl}");
            }
            string[] files = Directory.GetFiles(RepositoryUrl, "*.zip", SearchOption.TopDirectoryOnly);
            if (files != null && files.Any())
            {
                return Task.FromResult(GetUpdates(files).ToList());
            }
            return Task.FromResult(Enumerable.Empty<UpdateInfo>().ToList());
        }

        public override Task<string> DownloadAsync(UpdateInfo updateInfo, string downloadPath,
            CancellationToken cancellationToken, IDownloadProgress? downloadProgress)
        {
            if (!File.Exists(updateInfo.DownloadUrl))
            {
                throw new InvalidOperationException($"Repository file not exist: {updateInfo.DownloadUrl}");
            }
            return Task.FromResult(updateInfo.DownloadUrl);
        }

        public override Task<bool> CheckAsync(CancellationToken cancellationToken)
            => Task.FromResult(Directory.Exists(RepositoryUrl));

        private IEnumerable<UpdateInfo> GetUpdates(IEnumerable<string> releaseFiles)
        {
            foreach (var path in releaseFiles)
            {
                var info = _folderUpdateInfoFactory.CreateWithArchive(path);
                if (info != null && Installer.Verify(info.DownloadUrl))
                {
                    yield return info;
                }
            }
        }
    }
}
