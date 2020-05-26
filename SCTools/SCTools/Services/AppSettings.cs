namespace NSW.StarCitizen.Tools.Services
{
    public class AppSettings
    {
        //4032F680BFEE01
        public static byte[] OriginalPattern { get; } = { 0x40, 0x32, 0xF6, 0x80, 0xBF, 0xEE, 0x01 };
        // 90909080BFEE01
        public static byte[] PatchPattern { get; } = { 0x90, 0x90, 0x90, 0x80, 0xBF, 0xEE, 0x01 };
    }
}