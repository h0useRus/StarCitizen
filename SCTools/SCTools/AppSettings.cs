using Newtonsoft.Json;

namespace NSW.StarCitizen.Tools
{
    public class AppSettings
    {
        [JsonIgnore]
        //4032F680BFEE01
        public byte[] OriginalPattern { get; } = { 0x40, 0x32, 0xF6, 0x80, 0xBF, 0xEE, 0x01 };
        [JsonIgnore]
        // 90909080BFEE01
        public byte[] PatchPattern { get; } = { 0x90, 0x90, 0x90, 0x80, 0xBF, 0xEE, 0x01 };
        public string GameFolder { get; set; }
    }
}