using System.Linq;
using Newtonsoft.Json;

namespace NSW.StarCitizen.Tools.Lib.Update
{
    public class GiteeUpdateInfo : UpdateInfo
    {
        private readonly bool _namedVersion;

        [JsonProperty]
        public string? IndexDownloadUrl { get; protected set; }
        public override string GetVersion() => _namedVersion ? Name : TagName;

        [JsonConstructor]
        public GiteeUpdateInfo(string name, string tagName, string downloadUrl)
            : base(name, tagName, downloadUrl)
        {

        }

        private GiteeUpdateInfo(string name, string tagName, string downloadUrl, bool namedVersion)
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

            public UpdateInfo? CreateWithDownloadAsset(GiteeUpdateRepository.GitRelease release)
            {
                var downloadUrl = release.Assets.FirstOrDefault(a => a.Name != null)?.ZipUrl;
                if (string.IsNullOrEmpty(release.Name) || string.IsNullOrEmpty(release.TagName) ||
                    (downloadUrl == null) || string.IsNullOrEmpty(downloadUrl))
                {
                    return null;
                }
                return new GiteeUpdateInfo(release.Name, release.TagName, downloadUrl, _namedVersion)
                {
                    PreRelease = release.PreRelease,
                    Released = release.Created,
                };
            }
        }
    }
}
