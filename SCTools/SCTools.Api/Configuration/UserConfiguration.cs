using NSW.StarCitizen.Tools.API.Attributes;
using NSW.StarCitizen.Tools.API.Universes;

namespace NSW.StarCitizen.Tools.API.Configuration
{
    [ConfigurationFile(UniverseFile.UserConfiguration)]
    public class UserConfiguration
    {
        public TextLanguage TextLanguage { get; set; }
        public AudioLanguage AudioLanguage { get; set; }
    }
}
