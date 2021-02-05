using System;
using System.Collections.Generic;

namespace NSW.StarCitizen.Tools.Lib.Localization
{
    public class LanguageInfo
    {
        public ISet<string> Languages { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        public string? Current { get; set; }
    }
}