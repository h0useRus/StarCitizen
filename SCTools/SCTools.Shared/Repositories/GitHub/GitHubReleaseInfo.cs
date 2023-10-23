using System;
using System.Linq;
using NSW.StarCitizen.Tools.Repositories.GitHub.Models;

namespace NSW.StarCitizen.Tools.Repositories.GitHub
{
    internal sealed class GitHubReleaseInfo : ReleaseInfo
    {
        public override string GetVersion() => TagName;

        public string[] Assets { get; private set; } = Array.Empty<string>();

        public static GitHubReleaseInfo MapFrom(GitRelease gitRelease)
        {
            var assets = gitRelease.Assets.OrderBy(a => a.Id).Select(a => a.ZipUrl).ToArray();
            return new GitHubReleaseInfo
            {
                Name = gitRelease.Name,
                TagName = gitRelease.TagName,
                FilePath = assets[0],
                PreRelease = gitRelease.PreRelease,
                Released = gitRelease.Published,
                Assets = assets
            };
        }

        public static bool IsValid(GitRelease gitRelease)
            => !gitRelease.Draft &&
               gitRelease.Assets.Any();
    }
}
