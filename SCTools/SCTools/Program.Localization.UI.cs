using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Helpers;

namespace NSW.StarCitizen.Tools
{
    public static partial class Program
    {
        public static void UpdateUiLanguage()
        {
            foreach (var form in Application.OpenForms)
            {
                if (form is ILocalizedForm localizedForm)
                {
                    localizedForm.UpdateLocalizedControls();
                }
            }
        }

        public static Dictionary<string, string> GetSupportedUiLanguages()
        {
            var languages = new Dictionary<string, string> {
                { "en-US", "english" }
            };
            var neutralCultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures)
                .Where(c => Directory.Exists(c.TwoLetterISOLanguageName));
            foreach (var neutralCulture in neutralCultures)
            {
                var culture = CultureInfo.CreateSpecificCulture(neutralCulture.Name);
                if (!languages.ContainsKey(culture.Name))
                {
                    languages.Add(culture.Name, neutralCulture.NativeName);
                }
            }
            return languages;
        }
    }
}
