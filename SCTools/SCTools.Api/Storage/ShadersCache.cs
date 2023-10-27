using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace NSW.StarCitizen.Tools.Files
{
    public class ShadersCache
    {
        public static string CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Star Citizen");

        private readonly ILogger _logger;
        private DirectoryInfo? _rootDirectory;

        public DirectoryInfo RootDirectory => _rootDirectory ??= new DirectoryInfo(CachePath);

        public ShadersCache() : this(NullLogger.Instance) { }
        internal ShadersCache(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public DirectoryInfo[] GetAllCacheDirectories()
            => RootDirectory.Exists ? RootDirectory.GetDirectories() : Array.Empty<DirectoryInfo>();
        public void ClearAll()
        {
            foreach (var directory in GetAllCacheDirectories())
                try
                {
                    directory.Delete(true);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Falied to delete cache directory {DirectoryName}", directory.Name);
                }
        }

        public void Refresh() => _rootDirectory = null;
    }
}
