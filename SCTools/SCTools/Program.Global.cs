using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NSW.StarCitizen.Tools.Global;

namespace NSW.StarCitizen.Tools
{
    public static partial class Program
    {
        public static GameInfo? CurrentGame { get; set; }

        public static string Name { get; } = Assembly.GetExecutingAssembly().GetName().Name;

        public static Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;

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
    }
}