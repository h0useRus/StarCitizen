using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using NSW.StarCitizen.Tools.API.Configuration;
using NSW.StarCitizen.Tools.API.Logging;

namespace NSW.StarCitizen.Tools.API.Universes
{
    internal class Universe : IUniverse
    {
        private readonly ILogger _logger;
        private readonly IDisposable? _loggerScope;

        public UniverseType Type { get; }
        public string RootFolder { get; }
        public string UserFolder => Path.Combine(RootFolder, UniverseFolder.User);
        public string BinFolder => Path.Combine(RootFolder, UniverseFolder.Bin);
        public string DataFolder => Path.Combine(RootFolder, UniverseFolder.Data);
        public string LocalizationFolder => Path.Combine(RootFolder, UniverseFolder.DataLocalization);
        public string ExecutableFile => Path.Combine(BinFolder, UniverseFile.Executable);
        public UserConfiguration UserConfiguration => throw new NotImplementedException();

        public Universe(UniverseType type, string baseFolder, ILogger logger)
        {
            Type = type;
            RootFolder = Path.Combine(baseFolder, type.ToString());
            _logger = logger;
            _loggerScope = _logger.BeginScope(new Dictionary<string, object>
            {
                [LogScope.Universe] = type.ToString(),
                [LogScope.UniverseFolder] = RootFolder
            });
        }

        public void Dispose() => _loggerScope?.Dispose();

        public override string ToString() => $"{Type} root path: {RootFolder}";
    }
}
