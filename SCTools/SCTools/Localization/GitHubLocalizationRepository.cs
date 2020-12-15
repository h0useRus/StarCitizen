using NSW.StarCitizen.Tools.Global;
using NSW.StarCitizen.Tools.Update;

namespace NSW.StarCitizen.Tools.Localization
{
    public sealed class GitHubLocalizationRepository : GitHubUpdateRepository, ILocalizationRepository
    {
        public GameMode Mode { get; }

        public GitHubLocalizationRepository(GameMode mode, string name, string repository) :
            base(GitHubDownloadType.Sources, GitHubUpdateInfo.Factory.NewWithVersionByName(), name, repository)
        {
            Mode = mode;
        }

        public ILocalizationInstaller Installer { get; } = new DefaultLocalizationInstaller();
    }
}