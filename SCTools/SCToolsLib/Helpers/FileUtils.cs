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

        public static bool MoveFileNoThrow(string sourceFileName, string destFileName)
        {
            try
            {
                File.Move(sourceFileName, destFileName);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool CopyFileNoThrow(string sourceFileName, string destFileName, bool overwrite)
        {
            try
            {
                File.Copy(sourceFileName, destFileName, overwrite);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool CreateDirectoryNoThrow(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch
                {
                    return false;
                }
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
