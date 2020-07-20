using System;
using System.Collections.Generic;
using System.IO;
using NSW.StarCitizen.Tools.Global;

namespace NSW.StarCitizen.Tools
{
    public static partial class Program
    {
        public const string ExeName = "StarCitizen.exe";
        public const string BinFolder = "Bin64";

        public static GameInfo CurrentGame { get; set; }

        public static bool SetGameFolder(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
                return false;

            if (!File.Exists(GetGameExePath(path, GameMode.LIVE)) && !File.Exists(GetGameExePath(path, GameMode.PTU)))
                return false;

            if (string.Compare(Settings.GameFolder, path, StringComparison.OrdinalIgnoreCase) != 0)
            {
                Settings.GameFolder = path;
                SaveAppSettings(Settings);
            }

            return true;
        }
        public static IEnumerable<GameInfo> GetGameModes()
        {
            var result = new List<GameInfo>();
            if (Directory.Exists(Settings.GameFolder))
            {
                foreach (var directory in new DirectoryInfo(Settings.GameFolder).GetDirectories())
                {
                    if (Enum.TryParse(directory.Name, true, out GameMode mode))
                    {
                        var exeFileInfo = new FileInfo(GetGameExePath(Settings.GameFolder, mode));
                        if (exeFileInfo.Exists)
                            result.Add(new GameInfo(directory, mode, exeFileInfo));
                    }
                }
            }
            return result;
        }
        private static string GetGameExePath(string rootPath, GameMode mode) => Path.Combine(rootPath, mode.ToString(), BinFolder, ExeName);
    }
}