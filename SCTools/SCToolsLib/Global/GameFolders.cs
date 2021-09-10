using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NSW.StarCitizen.Tools.Lib.Global
{
    public class GameFolders
    {
        public static List<GameInfo> GetGameModes(string gameFolder)
        {
            var result = new List<GameInfo>();
            if (Directory.Exists(gameFolder))
            {
                foreach (GameMode mode in Enum.GetValues(typeof(GameMode)))
                {
                    GameInfo? gameInfo = GameInfo.Create(mode, gameFolder);
                    if (gameInfo != null)
                        result.Add(gameInfo);
                }
            }
            return result;
        }

        public static GameInfo? GetDefaultOrFirstGameMode(List<GameInfo> gameModes, GameInfo? defaultGameMode)
        {
            if (defaultGameMode != null)
            {
                var result = gameModes.FirstOrDefault(i => i.Mode == defaultGameMode.Mode);
                if (result != null)
                {
                    return result;
                }
            }
            return gameModes.FirstOrDefault();
        }

        public static string? SearchGameFolder(string searchPath)
        {
            var directory = new DirectoryInfo(searchPath);
            if (directory.Exists)
            {
                if (IsContainGameModes(searchPath))
                    return searchPath;
                if (string.Compare(directory.Name, GameConstants.BinFolderName, StringComparison.OrdinalIgnoreCase) == 0)
                    return directory.Parent?.Parent?.FullName;
                if (IsGameModeFolderName(directory.Name))
                    return directory.Parent?.FullName;
                var searchFolder = Path.Combine(searchPath, GameConstants.GameFolderName);
                return Directory.Exists(searchFolder) ? searchFolder : searchPath;
            }
            return null;
        }

        private static bool IsContainGameModes(string gameFolder)
        {
            foreach (GameMode mode in Enum.GetValues(typeof(GameMode)))
            {
                if (GameInfo.Create(mode, gameFolder) != null)
                    return true;
            }
            return false;
        }

        private static bool IsGameModeFolderName(string folderName)
        {
            foreach (GameMode mode in Enum.GetValues(typeof(GameMode)))
            {
                if (string.Compare(folderName, mode.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
                    return true;
            }
            return false;
        }
    }
}
