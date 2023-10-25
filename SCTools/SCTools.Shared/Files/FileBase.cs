using System.IO;

namespace NSW.StarCitizen.Tools.Files
{
    public abstract class FileBase
    {
        public FileInfo File { get; }

        protected FileBase(string filePath)
        {
            File = new FileInfo(filePath);
        }
    }
}
