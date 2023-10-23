using System;
using Newtonsoft.Json;

namespace NSW.StarCitizen.Tools.Repositories.GitHub.Models
{
    internal class GitRelease
    {
        [JsonProperty("id")]
        public int Id { get; }
        [JsonProperty("name")]
        public string? Name { get; }
        [JsonProperty("tag_name")]
        public string TagName { get; } = string.Empty;
        [JsonProperty("draft")]
        public bool Draft { get; }
        [JsonProperty("prerelease")]
        public bool PreRelease { get; }
        [JsonProperty("published_at")]
        public DateTimeOffset Published { get; }
        [JsonProperty("created_at")]
        public DateTimeOffset Created { get; }
        [JsonProperty("assets")]
        public GitAsset[] Assets { get; } = Array.Empty<GitAsset>();
    }
}
