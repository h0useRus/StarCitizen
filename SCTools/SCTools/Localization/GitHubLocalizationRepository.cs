using NSW.StarCitizen.Tools.Update;

namespace NSW.StarCitizen.Tools.Localization
{
    public sealed class GitHubLocalizationRepository : GitHubUpdateRepository, ILocalizationRepository
    {
        public GitHubLocalizationRepository(string name, string repository) :
            base(GitHubDownloadType.Sources, GitHubUpdateInfo.Factory.NewWithVersionByName(), name, repository)
        {
        }

        public ILocalizationInstaller Installer { get; } = new DefaultLocalizationInstaller();
    }
}