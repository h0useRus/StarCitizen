using System.Linq;
using Newtonsoft.Json;

namespace NSW.StarCitizen.Tools.Lib.Update
{
    public class GitHubUpdateInfo : UpdateInfo
    {
        private readonly bool _namedVersion;

        [JsonProperty]
        public string? IndexDownloadUrl { get; protected set; }
        public override string GetVersion() => _namedVersion ? Name : TagName;

        [JsonConstructor]
        public GitHubUpdateInfo(string name, string tagName, string downloadUrl)
            : base(name, tagName, downloadUrl)
        {

        }

        private GitHubUpdateInfo(string name, string tagName, string downloadUrl, bool namedVersion)
            : base(name, tagName, downloadUrl)
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

            public UpdateInfo? CreateWithDownloadSourceCode(GitHubUpdateRepository.GitRelease release)
            {
                if (string.IsNullOrEmpty(release.Name) || string.IsNullOrEmpty(release.TagName) ||
                    release.Published == null || string.IsNullOrEmpty(release.ZipUrl))
                {
                    return null;
                }
                return new GitHubUpdateInfo(release.Name, release.TagName, release.ZipUrl, _namedVersion)
                {
                    PreRelease = release.PreRelease,
                    Released = release.Published.Value,
                    IndexDownloadUrl = release.Assets.FirstOrDefault(a => a.IsIndexFileUrl())?.ZipUrl
                };
            }

            public UpdateInfo? CreateWithDownloadAsset(GitHubUpdateRepository.GitRelease release)
            {
                var downloadUrl = release.Assets.FirstOrDefault()?.ZipUrl;
                if (string.IsNullOrEmpty(release.Name) || string.IsNullOrEmpty(release.TagName) ||
                    release.Published == null ||
                    (downloadUrl == null) || string.IsNullOrEmpty(downloadUrl))
                {
                    return null;
                }
                return new GitHubUpdateInfo(release.Name, release.TagName, downloadUrl, _namedVersion)
                {
                    PreRelease = release.PreRelease,
                    Released = release.Published.Value,
                };
            }
        }
    }
}
