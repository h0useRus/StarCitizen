using Newtonsoft.Json;

namespace NSW.StarCitizen.Tools.Settings
{
    public class UpdateSettings
    {
        [JsonProperty]
        public bool MonitorUpdates { get; set; } = false;
        [JsonProperty]
        public int MonitorRefreshTime { get; set; } = 5;
    }
}
