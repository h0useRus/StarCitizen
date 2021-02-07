using System.IO;

namespace NSW.StarCitizen.Tools.Lib.Helpers
{
    public static class FileUtils
    {
        public static bool DeleteFileNoThrow(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool DeleteDirectoryNoThrow(DirectoryInfo dir, bool recursive)
        {
            try
            {
                dir.Delete(recursive);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
