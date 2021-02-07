using System;

namespace NSW.StarCitizen.Tools.Lib.Update
{
    public static class GitHubRepositoryUrl
    {
        private const string GitHubUrl = "https://github.com/";

        public static string? Parse(string? url)
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                var repositoryUrl = uri.AbsolutePath.Trim('/');
                if (!string.IsNullOrWhiteSpace(repositoryUrl))
                {
                    return repositoryUrl;
                }
            }
            return null;
        }

        public static string Build(string repository) => GitHubUrl + repository;
    }
}
