using Newtonsoft.Json;

namespace NSW.StarCitizen.Tools.Repositories.GitHub.Models
{
    internal class GitAsset
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("browser_download_url")]
        public string ZipUrl { get; } = string.Empty;
    }
}
