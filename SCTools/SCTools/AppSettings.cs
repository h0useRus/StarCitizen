using Newtonsoft.Json;

namespace NSW.StarCitizen.Tools
{
    public class AppSettings
    {
        public string GameFolder { get; set; }
        public LocalizationSettings Localization { get; set; } = new LocalizationSettings();
    }

    public class LocalizationSettings
    {
        [JsonIgnore]
        //4032F680BFEE01
        public byte[] OriginalPattern { get; } = { 0x40, 0x32, 0xF6, 0x80, 0xBF, 0xEE, 0x01 };
        [JsonIgnore]
        // 90909080BFEE01
        public byte[] PatchPattern { get; } = { 0x90, 0x90, 0x90, 0x80, 0xBF, 0xEE, 0x01 };
        [JsonIgnore]
        public string Author { get; set; } = "defterai";
        [JsonIgnore]
        public string Repo { get; set; } = "StarCitizenModding";
        public string LastVersion { get; set; }
    }
}