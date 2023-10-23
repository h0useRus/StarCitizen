using Newtonsoft.Json;

namespace NSW.StarCitizen.Tools.Repositories.GitHub.Models
{
    internal class GitRate
    {
        [JsonProperty("limit")]
        public int Limit { get; }
        [JsonProperty("remaining")]
        public int Remaining { get; }
        [JsonProperty("reset")]
        public long Reset { get; }
        [JsonProperty("used")]
        public int Used { get; }

        public static GitRate Empty { get; } = new();
    }
}
