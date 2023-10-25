using System.Diagnostics;

namespace NSW.StarCitizen.Tools.Files
{
    public class ExeFile : FileBase
    {
        public FileVersionInfo? VersionInfo { get; }

        public ExeFile(string filePath) : base(filePath)
        {
            if (File.Exists)
            {
                VersionInfo = FileVersionInfo.GetVersionInfo(File.FullName);
            }
        }
    }
}
