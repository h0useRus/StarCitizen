using Defter.StarCitizen.ConfigDB;

namespace NSW.StarCitizen.Tools.Repository
{
    public static class ConfigDataRepository
    {
        public static ConfigDataLoader Loader { get; }

        static ConfigDataRepository()
        {
#if DEBUG
            Loader = new FileConfigDataLoader(new LocalFileSourceSettings(@"E:\Work\C-sharp\StarCitizenConfigDb"));
#else
            Loader = new NetworkConfigDataLoader(HttpNetClient.Client, new GitHubSourceSettings()
            {
                User = "defterai"
            });
#endif
        }
    }
}
