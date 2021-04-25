using System;
using System.IO;
using Newtonsoft.Json;
using NLog;

namespace NSW.StarCitizen.Tools.Lib.Update
{
    public class FolderUpdateInfo : UpdateInfo
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public override string GetVersion() => Name;

        [JsonConstructor]
        public FolderUpdateInfo(string name, string tagName, string downloadUrl)
            : base(name, tagName, downloadUrl)
        {
        }

        public class Factory
        {
            public static Factory New() => new Factory();

            public UpdateInfo? CreateWithArchive(string archivePath)
            {
                if (string.IsNullOrEmpty(archivePath))
                {
                    return null;
                }
                try
                {
                    string name = Path.GetFileName(archivePath);
                    return new FolderUpdateInfo(name, name, archivePath)
                    {
                        PreRelease = false,
                        Released = File.GetCreationTime(archivePath)
                    };
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"Failed create update info for archive: {archivePath}");
                    return null;
                }
            }
        }
    }
}
