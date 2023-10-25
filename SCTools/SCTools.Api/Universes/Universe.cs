using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using NSW.StarCitizen.Tools.API.Logging;
using NSW.StarCitizen.Tools.API.Storage;

namespace NSW.StarCitizen.Tools.API.Universes
{
    internal class Universe : IUniverse
    {
        private readonly ILogger _logger;
        private readonly IDisposable? _loggerScope;

        public UniverseType Type { get; }
        public bool IsActive => Files.Executable.File.Exists;
        public IFiles Files { get; }

        public Universe(UniverseType type, string baseFolder, ILogger logger)
        {
            Type = type;
            Files = new Storage.Files(Path.Combine(baseFolder, type.ToString()));
            _logger = logger;
            _loggerScope = _logger.BeginScope(new Dictionary<string, object>
            {
                [LogScope.Universe] = type,
                [LogScope.UniverseFolder] = Files.RootDirectory.FullName
            });
        }

        public void Dispose() => _loggerScope?.Dispose();

        public override string ToString() => $"{Type} root path: {Files.RootDirectory.FullName}";
    }
}
