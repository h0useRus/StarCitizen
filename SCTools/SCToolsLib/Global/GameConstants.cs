using System.IO;

namespace NSW.StarCitizen.Tools.Lib.Global
{
    public static class GameConstants
    {
        public const string BinFolderName = "Bin64";
        public const string DataFolderName = "data";
        public const string LocalizationFolderName = "localization"; 
        public const string GameFolderName = "StarCitizen";
        public const string GameExeName = "StarCitizen.exe";
        public const string UserConfigName = "user.cfg";
        public const string GlobalIniName = "global.ini";
        public const string CurrentLanguageKey = "g_language";
        public const string EnglishLocalization = "english";

        public static string GetGameModePath(string gamePath, GameMode mode) => Path.Combine(gamePath, mode.ToString());

        public static string GetGameExePath(string gameModePath) => Path.Combine(gameModePath, BinFolderName, GameExeName);

        public static string GetDataFolderPath(string gameModePath) => Path.Combine(gameModePath, DataFolderName);

        public static string GetLocalizationFolderPath(string gameModePath) => Path.Combine(gameModePath, DataFolderName, LocalizationFolderName);

        public static string GetUserConfigPath(string gameModePath) => Path.Combine(gameModePath, UserConfigName);
    }
}
