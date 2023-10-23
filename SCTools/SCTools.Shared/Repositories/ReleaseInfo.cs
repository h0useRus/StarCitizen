using System;
using Newtonsoft.Json;

namespace NSW.StarCitizen.Tools.Repositories
{
    public abstract class ReleaseInfo
    {
        [JsonProperty]
        public string Name { get; }
        [JsonProperty]
        public string TagName { get; }
        [JsonProperty]
        public string FilePath { get; }
        [JsonProperty]
        public DateTimeOffset Released { get; protected set; }
        [JsonProperty]
        public bool PreRelease { get; protected set; }
        public override string ToString() => Name;

        [JsonConstructor]
        protected ReleaseInfo(string name, string tagName, string filePath)
        {
            Name = name;
            TagName = tagName;
            FilePath = filePath;
        }

        public abstract string GetVersion();

        public string Dump() => $"(Name={Name}, TagName={TagName}, FilePath={FilePath}, Released={Released}, PreRelease={PreRelease})";
    }
}