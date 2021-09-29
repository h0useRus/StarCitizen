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
        public ILocalizationInstaller Installer { get; }
        private readonly FolderUpdateInfo.Factory _folderUpdateInfoFactory = FolderUpdateInfo.Factory.New();

        public FolderLocalizationRepository(ILocalizationInstaller installer, string appFolder, GameMode mode, string name, string repository) :
            base(UpdateRepositoryType.Folder, name, repository, FolderRepositoryUrl.Build(appFolder, repository))
        {
            Mode = mode;
            Installer = installer;
        }

        public async override Task<List<UpdateInfo>> GetAllAsync(CancellationToken cancellationToken)
        {
            if (!Directory.Exists(RepositoryUrl))
            {
                throw new InvalidOperationException($"Repository path not exist: {RepositoryUrl}");
            }
            return await Task.Run(() => GetUpdatesInFolder(cancellationToken), cancellationToken);
        }

        public override Task<DownloadResult> DownloadAsync(UpdateInfo updateInfo, string downloadPath, IPackageIndex? packageIndex,
            CancellationToken cancellationToken, IDownloadProgress? downloadProgress)
        {
            if (!File.Exists(updateInfo.DownloadUrl))
            {
                throw new InvalidOperationException($"Repository file not exist: {updateInfo.DownloadUrl}");
            }
            return Task.FromResult(DownloadResult.FromArchivePath(updateInfo.DownloadUrl));
        }

        public override Task<bool> CheckAsync(CancellationToken cancellationToken)
            => Task.FromResult(Directory.Exists(RepositoryUrl));

        private List<UpdateInfo> GetUpdatesInFolder(CancellationToken cancellationToken)
        {
            string[] files = Directory.GetFiles(RepositoryUrl, "*.zip", SearchOption.TopDirectoryOnly);
            if (files != null && files.Any())
            {
                return GetUpdates(files, cancellationToken).ToList();
            }
            return Enumerable.Empty<UpdateInfo>().ToList();
        }

        private IEnumerable<UpdateInfo> GetUpdates(IEnumerable<string> releaseFiles, CancellationToken cancellationToken)
        {
            foreach (var path in releaseFiles)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                var info = _folderUpdateInfoFactory.CreateWithArchive(path);
                if (info != null && Installer.Verify(info.DownloadUrl))
                {
                    yield return info;
                }
            }
        }
    }
}
