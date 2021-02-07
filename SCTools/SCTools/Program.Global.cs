using System;
using System.IO;
using System.Reflection;
using NSW.StarCitizen.Tools.Lib.Global;

namespace NSW.StarCitizen.Tools
{
    public static partial class Program
    {
        public static GameInfo? CurrentGame { get; set; }

        public static string Name { get; } = Assembly.GetExecutingAssembly().GetName().Name;

        public static Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;

        public static string ExecutableDir { get; } = GetExecutableDir();

        private static string GetExecutableDir()
        {
            var location = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase);
            var dirInfo = new FileInfo(location.LocalPath).Directory;
            if (dirInfo == null) throw new NullReferenceException("No assembly executable directory");
            return dirInfo.FullName;
        }
    }
}