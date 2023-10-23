using Newtonsoft.Json;

namespace NSW.StarCitizen.Tools.Repositories.GitHub.Models
{
    internal class GitRateLimit
    {
        [JsonProperty("rate")]
        public GitRate Rate { get; } = GitRate.Empty;
    }
}
