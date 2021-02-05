using System;
using Newtonsoft.Json;

namespace NSW.StarCitizen.Tools.Lib.Update
{
    public abstract class UpdateInfo
    {
        [JsonProperty]
        public string Name { get; }
        [JsonProperty]
        public string TagName { get; }
        [JsonProperty]
        public string DownloadUrl { get; }
        [JsonProperty]
        public DateTimeOffset Released { get; protected set; }
        [JsonProperty]
        public bool PreRelease { get; protected set; }
        public override string ToString() => Name;

        [JsonConstructor]
        protected UpdateInfo(string name, string tagName, string downloadUrl)
        {
            Name = name;
            TagName = tagName;
            DownloadUrl = downloadUrl;
        }

        public abstract string GetVersion();

        public string Dump() => $"(Name={Name}, TagName={TagName}, DownloadUrl={DownloadUrl}, Released={Released}, PreRelease={PreRelease})";
    }
}