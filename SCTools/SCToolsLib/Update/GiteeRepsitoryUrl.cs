using System;

namespace NSW.StarCitizen.Tools.Lib.Update
{
    public static class GiteeRepositoryUrl
    {
        private const string GiteeHost = "gitee.com";
        private const string GiteeUrl = "https://gitee.com/";

        public static string? Parse(string? url)
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                var repositoryUrl = uri.AbsolutePath.Trim('/');
                if (!string.IsNullOrWhiteSpace(repositoryUrl) &&
                    uri.Host.Equals(GiteeHost, StringComparison.OrdinalIgnoreCase))
                {
                    return repositoryUrl;
                }
            }
            return null;
        }

        public static string Build(string repository) => GiteeUrl + repository;
    }
}
