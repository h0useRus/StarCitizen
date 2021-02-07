using System;
using System.Collections.Generic;
using System.Linq;
using NSW.StarCitizen.Tools.Lib.Global;
using NSW.StarCitizen.Tools.Repository;

namespace NSW.StarCitizen.Tools
{
    public static partial class Program
    {
        private static Dictionary<GameMode, RepositoryManager>? _repositoryManagers;

        public static IReadOnlyDictionary<GameMode, RepositoryManager> RepositoryManagers => _repositoryManagers ??= CreateRepositoryManagers();

        public static void RunRepositoryMonitors(List<GameInfo> gameInfos)
        {
            foreach (var entry in RepositoryManagers)
            {
                if (gameInfos.Any(gi => gi.Mode == entry.Key))
                {
                    entry.Value.RunMonitors();
                }
                else
                {
                    entry.Value.StopMonitors();
                }
            }
        }

        public static void StopRepositoryMonitors()
        {
            foreach (var repositoryManager in RepositoryManagers.Values)
            {
                repositoryManager.StopMonitors();
            }
        }

        private static Dictionary<GameMode, RepositoryManager> CreateRepositoryManagers()
        {
            var modes = Enum.GetValues(typeof(GameMode));
            var result = new Dictionary<GameMode, RepositoryManager>(modes.Length);
            foreach (GameMode mode in modes)
            {
                result.Add(mode, new RepositoryManager(mode));
            }
            return result;
        }
    }
}