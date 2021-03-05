using Defter.StarCitizen.ConfigDB;
  
namespace NSW.StarCitizen.Tools.Repository
{
    public static class ConfigDataRepository
    {
#if DEBUG
        public static LocalFileSourceSettings SourceSettings = new LocalFileSourceSettings(@"E:\Work\C-sharp\StarCitizenConfigDb");
        public static ConfigDataLoader Loader { get; } = new FileConfigDataLoader(SourceSettings);
#else
        public static GitHubSourceSettings SourceSettings = new GitHubSourceSettings();
        public static ConfigDataLoader Loader { get; } = new NetworkConfigDataLoader(HttpNetClient.Client, SourceSettings);
#endif
    }
}
