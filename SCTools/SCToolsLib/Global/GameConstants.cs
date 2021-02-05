using System.IO;

namespace NSW.StarCitizen.Tools.Lib.Global
{
    public static class GameConstants
    {
        public const string BinFolderName = "Bin64";
        public const string DataFolderName = "data";
        public const string GameFolderName = "StarCitizen";
        public const string GameExeName = "StarCitizen.exe";
        public const string PatcherOriginalName = "patcher.bin";
        public const string PatcherLibraryName = "CIGDevelopmentTools.dll";
        public const string SystemConfigName = "system.cfg";
        public const string UserConfigName = "user.cfg";
        public const string SystemLanguagesKey = "sys_languages";
        public const string CurrentLanguageKey = "g_language";

        public static string GetGameModePath(string gamePath, GameMode mode) => Path.Combine(gamePath, mode.ToString());

        public static string GetGameExePath(string gameModePath) => Path.Combine(gameModePath, BinFolderName, GameExeName);

        public static string GetEnabledPatcherPath(string gameModePath) => Path.Combine(gameModePath, BinFolderName, PatcherLibraryName);

        public static string GetDisabledPatcherPath(string gameModePath) => Path.Combine(gameModePath, BinFolderName, PatcherOriginalName);

        public static string GetDataFolderPath(string gameModePath) => Path.Combine(gameModePath, DataFolderName);

        public static string GetSystemConfigPath(string gameModePath) => Path.Combine(gameModePath, DataFolderName, SystemConfigName);

        public static string GetUserConfigPath(string gameModePath) => Path.Combine(gameModePath, UserConfigName);
    }
}
