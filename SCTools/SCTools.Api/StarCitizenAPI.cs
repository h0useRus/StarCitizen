using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSW.StarCitizen.Tools.API.Logging;
using NSW.StarCitizen.Tools.API.Universes;

namespace NSW.StarCitizen.Tools.API
{
    internal class StarCitizenAPI : IStarCitizenAPI
    {
        private readonly object _lock = new();
        private readonly ILogger _logger;
        private readonly IDisposable? _loggerScope;
        private readonly ConcurrentDictionary<string, IUniverse> _universes = new(StringComparer.OrdinalIgnoreCase);
        private readonly StarCitizenApiOptions _options;

        public bool IsActive => _universes.Count > 0;
        public IUniverse? Live => GetUniverse(Universe.LiveName);
        public IUniverse? PTU => GetUniverse(Universe.PtuName);
        public IReadOnlyDictionary<string, IUniverse> Universes => _universes;

        public StarCitizenAPI(IOptions<StarCitizenApiOptions> options, ILogger logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _loggerScope = _logger.BeginScope(new Dictionary<string, object>
            {
                [LogScope.Api] = nameof(StarCitizenAPI),
                [LogScope.Version] = "0.1.0",
                [LogScope.RootFolder] = _options.RootFolder
            });
            FillLocalUniverses(_options.RootFolder, _universes);
        }

        public void Dispose()
        {
            foreach (var item in _universes)
                if (_universes.TryRemove(item.Key, out var universe))
                    universe.Dispose();

            _loggerScope?.Dispose();
        }

        private void FillLocalUniverses(string rootFolder, ConcurrentDictionary<string, IUniverse> universes)
        {
            universes.Clear();
            if (Directory.Exists(rootFolder))
            {
                foreach (var dir in Directory.GetDirectories(rootFolder))
                {
                    if (TryCreateUniverse(dir, out var universe) && universe != null)
                        universes.TryAdd(universe.Name, universe);
                }
            }
        }
        private bool TryCreateUniverse(string localPath, out IUniverse? universe)
        {
            if (Universe.IsValidPath(localPath))
                try
                {
                    var name = Path.GetFileName(localPath);
                    universe = new Universe(name, Path.GetDirectoryName(localPath), _logger);
                    return true;
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Failed to create universe from path {Path}", localPath);
                }
            _logger.LogWarning("Universe not found here {Path}", localPath);
            universe = default;
            return false;
        }
        public IUniverse? GetUniverse(string name)
        {
            if (_universes.TryGetValue(name.ToUpper(), out var universe))
                return universe;
            return default;
        }

        public void Refresh()
        {
            lock (_lock)
            {
                FillLocalUniverses(_options.RootFolder, _universes);
            }
        }
    }
}
