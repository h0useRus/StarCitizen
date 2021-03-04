using Newtonsoft.Json;

namespace NSW.StarCitizen.Tools.Settings
{
    public class UpdateSettings
    {
        [JsonProperty]
        public bool MonitorUpdates { get; set; }
        [JsonProperty]
        public bool AllowPreReleases { get; set; }
        [JsonProperty]
        public int MonitorRefreshTime { get; set; } = 5;
    }
}
