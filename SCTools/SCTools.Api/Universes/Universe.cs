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
        public const string LiveName = "LIVE";
        public const string PtuName = "PTU";

        private readonly ILogger _logger;
        private readonly IDisposable? _loggerScope;

        public string Name { get; }
        public bool IsActive => Files.Executable.File.Exists;
        public IFiles Files { get; }

        public Universe(string name, string baseFolder, ILogger logger)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(baseFolder))
                throw new ArgumentNullException(nameof(baseFolder));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            Name = name.ToUpper();
            Files = new Storage.Files(Path.Combine(baseFolder, Name));

            _loggerScope = _logger.BeginScope(new Dictionary<string, object>
            {
                [LogScope.Universe] = Name,
                [LogScope.UniverseFolder] = Files.RootDirectory.FullName
            });
        }

        public void Dispose() => _loggerScope?.Dispose();
        public override string ToString() => $"{Name} root path: {Files.RootDirectory.FullName}";
        //check for StarCitizen.exe
        public static bool IsValidPath(string path) => File.Exists(Path.Combine(path, Storage.Files.BinPath, Storage.Files.ExecutableName));
    }
}
