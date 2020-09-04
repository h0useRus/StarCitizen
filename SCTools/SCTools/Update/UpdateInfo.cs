using System;

namespace NSW.StarCitizen.Tools.Update
{
    public abstract class UpdateInfo
    {
        public string Name { get; set; }
        public string TagName { get; set; }
        public string DownloadUrl { get; set; }
        public DateTimeOffset Released { get; set; }
        public bool PreRelease { get; set; }
        public override string ToString() => Name;

        public abstract string GetVersion();
    }
}