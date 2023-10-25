using System.IO;
using NSW.StarCitizen.Tools.Files;

namespace NSW.StarCitizen.Tools.API.Storage
{
    public interface IFiles
    {
        DirectoryInfo RootDirectory { get; }
        DirectoryInfo UserDirectory { get; }
        DirectoryInfo BinDirectory { get; }
        DirectoryInfo DataDirectory { get; }
        DirectoryInfo LocalizationDirectory { get; }
        ExeFile Executable { get; }
        CfgFile UserConfig { get; }
    }
}
