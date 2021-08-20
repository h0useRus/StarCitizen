using System;
using Defter.StarCitizen.ConfigDB;
using NSW.StarCitizen.Tools.Lib.Update;

namespace NSW.StarCitizen.Tools.Repository
{
    public static class ConfigDataRepository
    {
#if DEBUG
        public static LocalFileSourceSettings SourceSettings { get; } = new LocalFileSourceSettings(System.Environment.GetEnvironmentVariable("LOCAL_SC_CONFIG_DB"));
        public static ConfigDataLoader Loader { get; } = new FileConfigDataLoader(SourceSettings);
#else
        private static UpdateRepositoryType RepositoryType { get; set; } = Program.Settings.Update.RepositoryType;
        public static INetworkSourceSettings SourceSettings { get; private set; } = CreateSourceSettings(RepositoryType);
        public static ConfigDataLoader Loader { get; private set; } = new NetworkConfigDataLoader(HttpNetClient.Client, SourceSettings);

        public static void UpdateLoader(UpdateRepositoryType repostoryType)
        {
            if (RepositoryType != repostoryType)
            {
                RepositoryType = repostoryType;
                SourceSettings = CreateSourceSettings(repostoryType);
                Loader = new NetworkConfigDataLoader(HttpNetClient.Client, SourceSettings);
            }
        }

        private static INetworkSourceSettings CreateSourceSettings(UpdateRepositoryType repostoryType) => repostoryType switch
        {
            UpdateRepositoryType.GitHub => new GitHubSourceSettings(),
            UpdateRepositoryType.Gitee => new GiteeSourceSettings(),
            _ => throw new NotSupportedException($"Not supported repository type {repostoryType} as configDB source"),
        };
#endif
    }
}
