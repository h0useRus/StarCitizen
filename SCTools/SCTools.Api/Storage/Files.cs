using System.IO;
using NSW.StarCitizen.Tools.Files;

namespace NSW.StarCitizen.Tools.API.Storage
{
    internal class Files : IFiles
    {
        #region Directories
        public const string BinPath = "Bin64";
        public const string DataPath = "Data";
        public const string UserPath = "User";
        public const string ScreenShotsPath = "ScreenShots";
        public const string LocalizationPath = "Localization";
        #endregion

        #region Files
        public const string ExecutableName = "StarCitizen.exe";
        public const string UserConfigName = "user.cfg";
        public const string LocalizationName = "global.ini";
        #endregion

        public DirectoryInfo RootDirectory { get; }
        public DirectoryInfo UserDirectory { get; }
        public DirectoryInfo BinDirectory { get; }
        public DirectoryInfo DataDirectory { get; }
        public DirectoryInfo LocalizationDirectory { get; }
        public ExeFile Executable { get; }
        public CfgFile UserConfig { get; }
        public Files(string rootPath)
        {
            RootDirectory = new DirectoryInfo(rootPath);
            UserDirectory = new DirectoryInfo(Path.Combine(rootPath, UserPath));
            BinDirectory = new DirectoryInfo(Path.Combine(rootPath, BinPath));
            DataDirectory = new DirectoryInfo(Path.Combine(rootPath, DataPath));
            LocalizationDirectory = new DirectoryInfo(Path.Combine(rootPath, DataPath, LocalizationPath));
            Executable = new ExeFile(Path.Combine(BinDirectory.FullName, ExecutableName));
            UserConfig = new CfgFile(Path.Combine(rootPath, UserConfigName));
        }
    }
}
