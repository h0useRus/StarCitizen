using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NSW.StarCitizen.Tools.Global;

namespace NSW.StarCitizen.Tools
{
    public static partial class Program
    {
        public static GameInfo? CurrentGame { get; set; }

        public static string Name { get; } = Assembly.GetExecutingAssembly().GetName().Name;

        public static Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;

        public static bool SetGameFolder(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            var gameInfos = GetGameModes(path);
            if (!gameInfos.Any())
                return false;

            if (string.Compare(Settings.GameFolder, path, StringComparison.OrdinalIgnoreCase) != 0)
            {
                Settings.GameFolder = path;
                SaveAppSettings(Settings);
            }

            return true;
        }

        public static IEnumerable<GameInfo> GetGameModes(string? gameFolder)
        {
            var result = new List<GameInfo>();
            if (gameFolder != null && Directory.Exists(gameFolder))
            {
                foreach (GameMode mode in Enum.GetValues(typeof(GameMode)))
                {
                    var gameModeDir = new DirectoryInfo(GameConstants.GetGameModePath(gameFolder, mode));
                    if (gameModeDir.Exists)
                    {
                        var exeFileInfo = new FileInfo(GameConstants.GetGameExePath(gameModeDir.FullName));
                        if (exeFileInfo.Exists)
                            result.Add(new GameInfo(gameModeDir, mode, exeFileInfo));
                    }
                }
            }
            return result;
        }

        public static IEnumerable<GameInfo> GetGameModes()
        {
            return GetGameModes(Settings.GameFolder);
        }
    }
}