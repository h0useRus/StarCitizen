using System;
using System.Threading;
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

        public FilesIndex CreateLocal(CancellationToken? cancellationToken = default)
        {
            try
            {
                return CreateBuilder(cancellationToken).Build();
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Failed create path package index: {_sourcePath}");
                throw e;
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

        protected FilesIndex.Builder CreateBuilder(CancellationToken? cancellationToken = default)
        {
            var builder = new FilesIndex.HashBuilder();
            builder.AddDirectory(GameConstants.GetDataFolderPath(_sourcePath), GameConstants.DataFolderName, cancellationToken);
            if (!builder.AddFile(GameConstants.GetEnabledPatcherPath(_sourcePath), GameConstants.PatcherOriginalName, cancellationToken))
            {
                builder.AddFile(GameConstants.GetDisabledPatcherPath(_sourcePath), GameConstants.PatcherOriginalName, cancellationToken);
            }
            builder.Remove(@"data\timestamp");
            return builder;
        }
    }
}
