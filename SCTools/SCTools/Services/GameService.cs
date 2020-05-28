using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace NSW.StarCitizen.Tools.Services
{
    public enum GameMode
    {
        LIVE,
        PTU
    }

    public class GameInfo
    {
        public GameMode Mode { get; }
        public DirectoryInfo RootFolder { get; }
        public FileInfo ExeFile { get; }
        public string ExeVersion { get; }

        public GameInfo(DirectoryInfo rootFolder, GameMode mode, FileInfo exeFile)
        {
            Mode = mode;
            RootFolder = rootFolder;
            ExeFile = exeFile;
            ExeVersion = FileVersionInfo.GetVersionInfo(exeFile.FullName).FileVersion.Replace(',','.');
        }

        public override string ToString() => Mode.ToString();
    }

    public class GameService
    {
        public static GameService Instance { get; } = new GameService();

        public const string ExeName = "StarCitizen.exe";
        public const string BinFolder = "Bin64";
        public DirectoryInfo GamePath { get; private set; }
        public bool IsReady => GamePath != null;

        public bool SetFolder(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
                return false;

            if (!File.Exists(GetExePath(path, GameMode.LIVE)) && !File.Exists(GetExePath(path, GameMode.PTU)))
                return false;

            GamePath = new DirectoryInfo(path);
            return true;
        }

        public IEnumerable<GameInfo> GetModes()
        {
            var result = new List<GameInfo>();
            if (IsReady)
            {
                foreach (var directory in GamePath.GetDirectories())
                {
                    if (Enum.TryParse(directory.Name, true, out GameMode mode))
                    {
                        var exeFileInfo = new FileInfo(GetExePath(GamePath.FullName, mode));
                        if (exeFileInfo.Exists)
                            result.Add(new GameInfo(directory, mode, exeFileInfo));
                    }
                }
            }

            return result;
        }

        private static string GetExePath(string rootPath, GameMode mode) => Path.Combine(rootPath, mode.ToString(), BinFolder, ExeName);
    }
}