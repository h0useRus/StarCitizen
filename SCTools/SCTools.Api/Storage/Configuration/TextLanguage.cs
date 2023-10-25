using NSW.StarCitizen.Tools.API.Attributes;

namespace NSW.StarCitizen.Tools.API.Storage.Configuration
{
    [ConfigurationKey("g_language")]
    public enum TextLanguage
    {
        [ConfigurationValue("chinese_(simplified)")]
        ChineseSimplified,
        [ConfigurationValue("chinese_(traditional)")]
        ChineseTraditional,
        [ConfigurationValue("english")]
        English,
        [ConfigurationValue("french_(france)")]
        French,
        [ConfigurationValue("german_(germany)")]
        German,
        [ConfigurationValue("italian_(italy)")]
        Italian,
        [ConfigurationValue("japanese_(japan)")]
        Japanese,
        [ConfigurationValue("korean_(south_korea)")]
        Korean,
        [ConfigurationValue("polish_(poland)")]
        Polish,
        [ConfigurationValue("portuguese_(brazil)")]
        Portuguese,
        [ConfigurationValue("korean_(south_korea)")] // russian are using korean temporary
        Russian,
        [ConfigurationValue("spanish_(latin_america)")]
        SpanishLatinAmerica,
        [ConfigurationValue("spanish_(spain)")]
        Spanish
    }
}
