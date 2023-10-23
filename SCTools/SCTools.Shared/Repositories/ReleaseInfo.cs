using System;
using Newtonsoft.Json;

namespace NSW.StarCitizen.Tools.Repositories
{
    public abstract class ReleaseInfo
    {
        [JsonProperty]
        public string? Name { get; internal set; }
        [JsonProperty]
        public string TagName { get; internal set; } = string.Empty;
        [JsonProperty]
        public string FilePath { get; internal set; } = string.Empty;
        [JsonProperty]
        public DateTimeOffset Released { get; protected set; }
        [JsonProperty]
        public bool PreRelease { get; protected set; }
        public override string ToString() => Name ?? TagName;

        public abstract string GetVersion();

        public string Dump() => $"(Name={Name}, TagName={TagName}, FilePath={FilePath}, Released={Released}, PreRelease={PreRelease})";
    }
}