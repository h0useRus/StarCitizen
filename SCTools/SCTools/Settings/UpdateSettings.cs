using System;
using Newtonsoft.Json;
using NSW.StarCitizen.Tools.Lib.Update;

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
        [JsonProperty]
        public UpdateRepositoryType RepositoryType { get; set; } = UpdateRepositoryType.GitHub;
        [JsonProperty]
        public DateTime? LastRegularCheckTime { get; set; }

        public bool CanLaunchRegularUpdatesCheck(DateTime nowTime)
            => !MonitorUpdates && (LastRegularCheckTime == null || nowTime.Subtract(LastRegularCheckTime.Value).TotalDays >= 7);
    }
}
