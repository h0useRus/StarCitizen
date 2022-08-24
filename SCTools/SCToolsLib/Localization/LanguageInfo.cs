using System;
using System.Collections.Generic;

namespace NSW.StarCitizen.Tools.Lib.Localization
{
    public class LanguageInfo
    {
        public IDictionary<string, string> Languages { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public string? Current { get; set; }
    }
}