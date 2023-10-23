using System;
using System.Linq;
using Newtonsoft.Json;
using NSW.StarCitizen.Tools.Repositories.GitHub.Models;

namespace NSW.StarCitizen.Tools.Repositories.GitHub
{
    internal sealed class GitHubReleaseInfo : ReleaseInfo
    {
        private readonly bool _namedVersion;

        public override string GetVersion() => _namedVersion ? Name : TagName;

        public string[] Assets { get; private set; } = Array.Empty<string>();

        [JsonConstructor]
        public GitHubReleaseInfo(string name, string tagName, string downloadUrl)
            : base(name, tagName, downloadUrl)
        {

        }

        private GitHubReleaseInfo(string name, string tagName, string downloadUrl, bool namedVersion)
            : base(name, tagName, downloadUrl)
        {
            _namedVersion = namedVersion;
        }

        public static GitHubReleaseInfo MapFrom(GitRelease gitRelease, bool namedVersion)
        {
            var assets = gitRelease.Assets.OrderBy(a => a.Id).Select(a => a.ZipUrl).ToArray();
            return new GitHubReleaseInfo(gitRelease.Name, gitRelease.TagName, assets[0], namedVersion)
            {
                PreRelease = gitRelease.PreRelease,
                Released = gitRelease.Published,
                Assets = assets
            };
        }

        public static bool IsValid(GitRelease gitRelease)
            => !gitRelease.Draft &&
               !string.IsNullOrWhiteSpace(gitRelease.Name) &&
               !string.IsNullOrWhiteSpace(gitRelease.TagName) &&
               gitRelease.Assets.Any();
    }
}
