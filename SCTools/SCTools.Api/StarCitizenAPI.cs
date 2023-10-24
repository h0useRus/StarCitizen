using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSW.StarCitizen.Tools.API.Logging;
using NSW.StarCitizen.Tools.API.Universes;

namespace NSW.StarCitizen.Tools.API
{
    internal class StarCitizenAPI : IStarCitizenAPI
    {
        private readonly ILogger _logger;
        private readonly IDisposable? _loggerScope;
        private readonly ConcurrentDictionary<string, IUniverse> _universes = new(StringComparer.OrdinalIgnoreCase);
        private readonly StarCitizenApiOptions _options;

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
        }

        public IUniverse Live => GetUniverse(UniverseType.LIVE);
        public IUniverse PTU => GetUniverse(UniverseType.PTU);

        public IUniverse GetUniverse(UniverseType type)
        {
            var key = type.ToString();
            return _universes.GetOrAdd(key, new Universe(type, _options.RootFolder, _logger));
        }

        public void Dispose()
        {
            foreach (var item in _universes)
                if (_universes.TryRemove(item.Key, out var universe))
                    universe.Dispose();

            _loggerScope?.Dispose();
        }
    }
}
