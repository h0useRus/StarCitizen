using Defter.StarCitizen.ConfigDB;
  
namespace NSW.StarCitizen.Tools.Repository
{
    public static class ConfigDataRepository
    {
#if DEBUG
        public static ConfigDataLoader Loader { get; } = new FileConfigDataLoader(new LocalFileSourceSettings(@"E:\Work\C-sharp\StarCitizenConfigDb"));
#else
        public static ConfigDataLoader Loader { get; } = new NetworkConfigDataLoader(HttpNetClient.Client, new GitHubSourceSettings());
#endif
    }
}
