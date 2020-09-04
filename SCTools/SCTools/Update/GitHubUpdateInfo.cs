using System.Linq;

namespace NSW.StarCitizen.Tools.Update
{
    public class GitHubUpdateInfo : UpdateInfo
    {
        private readonly bool _namedVersion = false;

        public override string GetVersion() => _namedVersion ? Name : TagName;

        public GitHubUpdateInfo()
        {
        }

        private GitHubUpdateInfo(bool namedVersion)
        {
            _namedVersion = namedVersion;
        }

        public class Factory
        {
            private readonly bool _namedVersion;

            public static Factory NewWithVersionByName() => new Factory(true);
            public static Factory NewWithVersionByTagName() => new Factory(false);

            Factory(bool namedVersion)
            {
                _namedVersion = namedVersion;
            }

            public UpdateInfo CreateWithDownloadSourceCode(GitHubUpdateRepository.GitRelease release) => new GitHubUpdateInfo(_namedVersion)
            {
                Name = release.Name,
                TagName = release.TagName,
                PreRelease = release.PreRelease,
                Released = release.Published,
                DownloadUrl = release.ZipUrl
            };

            public UpdateInfo CreateWithDownloadAsset(GitHubUpdateRepository.GitRelease release) => new GitHubUpdateInfo(_namedVersion)
            {
                Name = release.Name,
                TagName = release.TagName,
                PreRelease = release.PreRelease,
                Released = release.Published,
                DownloadUrl = release.Assets?.FirstOrDefault()?.ZipUrl
            };
        }
    }
}
