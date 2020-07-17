using System.Diagnostics;
using System.IO;

namespace NSW.StarCitizen.Tools.Global
{
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
            ExeVersion = FileVersionInfo.GetVersionInfo(exeFile.FullName).FileVersion.Replace(',', '.');
        }

        public override string ToString() => Mode.ToString();
    }
}