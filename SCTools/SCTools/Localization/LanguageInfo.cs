using System.Collections.Generic;

namespace NSW.StarCitizen.Tools.Localization
{
    public class LanguageInfo
    {
        public List<string> Languages { get; set; } = new List<string>();
        public string Current { get; set; }
        public string New { get; set; }
    }
}