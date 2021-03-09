using Defter.StarCitizen.ConfigDB;
  
namespace NSW.StarCitizen.Tools.Repository
{
    public static class ConfigDataRepository
    {
#if DEBUG
        public static LocalFileSourceSettings SourceSettings { get; } = new LocalFileSourceSettings(System.Environment.GetEnvironmentVariable("LOCAL_SC_CONFIG_DB"));
        public static ConfigDataLoader Loader { get; } = new FileConfigDataLoader(SourceSettings);
#else
        public static GitHubSourceSettings SourceSettings { get; } = new GitHubSourceSettings();
        public static ConfigDataLoader Loader { get; } = new NetworkConfigDataLoader(HttpNetClient.Client, SourceSettings);
#endif
    }
}
