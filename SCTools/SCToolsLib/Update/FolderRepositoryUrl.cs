using System;
using System.IO;

namespace NSW.StarCitizen.Tools.Lib.Update
{
    public static class FolderRepositoryUrl
    {
        public static string? Parse(string rootPath, string url)
        {
            if (Path.IsPathRooted(url))
            {
                string urlPath = Path.GetFullPath(url);
                string rootDir = rootPath;
                if (!rootDir.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    rootDir += Path.DirectorySeparatorChar;
                }
                if (urlPath.StartsWith(rootDir, StringComparison.OrdinalIgnoreCase))
                {
                    string relativePath = urlPath.Substring(rootDir.Length);
                    return string.IsNullOrEmpty(relativePath) ? "." : relativePath;
                }
                return url;
            }
            if (Directory.Exists(Path.Combine(rootPath, url)))
            {
                return url;
            }
            return null;
        }

        public static string Build(string rootPath, string repository)
            => Path.IsPathRooted(repository) ? repository : Path.Combine(rootPath, repository);
    }
}
