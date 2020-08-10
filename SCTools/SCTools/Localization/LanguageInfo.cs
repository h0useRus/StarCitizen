using System;
using System.Collections.Generic;

namespace NSW.StarCitizen.Tools.Localization
{
    public class LanguageInfo
    {
        public ISet<string> Languages { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        public string Current { get; set; }
        public string New { get; set; }
    }
}