using System;
using NSW.StarCitizen.Tools.Properties;

namespace NSW.StarCitizen.Tools.Localization
{
    public class LocalizationInfo
    {
        public string Name { get; set; }
        public string TagName { get; set; }
        public string DownloadUrl { get; set; }
        public DateTimeOffset Released { get; set; }
        public bool PreRelease { get; set; }
        public bool Actual => !string.IsNullOrWhiteSpace(DownloadUrl);
        public override string ToString() => Name;

        public static LocalizationInfo Empty { get; } = new LocalizationInfo { Name = Resources.Localization_Press_Refresh_Button };
    }
}