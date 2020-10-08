using System.Diagnostics;
using System.IO;

namespace NSW.StarCitizen.Tools.Global
{
    public sealed class GameInfo
    {
        public GameMode Mode { get; }
        public string RootFolderPath { get; }
        public string ExeFilePath { get; }
        public string ExeVersion { get; }

        public static GameInfo? Create(GameMode mode, string gamePath)
        {
            var rootFolderPath = GameConstants.GetGameModePath(gamePath, mode);
            if (Directory.Exists(rootFolderPath))
            {
                var exeFilePath = GameConstants.GetGameExePath(rootFolderPath);
                if (File.Exists(exeFilePath))
                {
                    return new GameInfo(mode, rootFolderPath, exeFilePath);
                }
            }
            return null;
        }

        private GameInfo(GameMode mode, string rootFolderPath, string exeFilePath)
        {
            Mode = mode;
            RootFolderPath = rootFolderPath;
            ExeFilePath = exeFilePath;
            ExeVersion = FileVersionInfo.GetVersionInfo(exeFilePath).FileVersion.Replace(',', '.');
        }

        public bool IsAvailable() => File.Exists(ExeFilePath);

        public override string ToString() => Mode.ToString();
    }
}