using System;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using NSW.StarCitizen.Tools.Lib.Global;
using NSW.StarCitizen.Tools.Lib.Update;

namespace NSW.StarCitizen.Tools.Lib.Helpers
{
    public class LocalizationPackageIndex : IPackageIndex
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly string _sourcePath;

        public LocalizationPackageIndex(string sourcePath)
        {
            _sourcePath = sourcePath;
        }

        public async Task<FilesIndex> CreateLocalAsync(CancellationToken? cancellationToken = default)
        {
            try
            {
                using var builder = new FilesIndex.HashBuilder();
                await CreateBuilderAsync(builder, cancellationToken).ConfigureAwait(false);
                return builder.Build();
            }
            catch (OperationCanceledException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Failed create path package index: {_sourcePath}");
                throw;
            }
        }

        public bool VerifyExternal(FilesIndex filesIndex)
        {
            if (filesIndex.IsEmpty())
            {
                return false;
            }
            var dataPrefix = GameConstants.DataFolderName + FilesIndex.DirectorySeparatorChar;
            foreach (var filePath in filesIndex.Index.Keys)
            {
                if (!filePath.Equals(GameConstants.PatcherOriginalName, StringComparison.OrdinalIgnoreCase) &&
                    !filePath.StartsWith(dataPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }

        protected async Task CreateBuilderAsync(FilesIndex.HashBuilder builder, CancellationToken? cancellationToken = default)
        {
            await builder.AddDirectoryAsync(GameConstants.GetDataFolderPath(_sourcePath),
                GameConstants.DataFolderName, cancellationToken).ConfigureAwait(false);
            if (!await builder.AddFileAsync(GameConstants.GetEnabledPatcherPath(_sourcePath),
                GameConstants.PatcherOriginalName, cancellationToken).ConfigureAwait(false))
            {
                await builder.AddFileAsync(GameConstants.GetDisabledPatcherPath(_sourcePath),
                    GameConstants.PatcherOriginalName, cancellationToken).ConfigureAwait(false);
            }
            builder.Remove(@"data\timestamp");
        }
    }
}
