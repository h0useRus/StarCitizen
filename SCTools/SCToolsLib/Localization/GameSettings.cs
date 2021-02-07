using System;
using System.Linq;
using NSW.StarCitizen.Tools.Lib.Global;
using NSW.StarCitizen.Tools.Lib.Helpers;

namespace NSW.StarCitizen.Tools.Lib.Localization
{
    public class GameSettings
    {
        private readonly GameInfo _currentGame;
        public LanguageInfo LanguageInfo { get; private set; } = new LanguageInfo();

        public GameSettings(GameInfo currentGame)
        {
            _currentGame = currentGame;
        }

        public void Load()
        {
            var languageInfo = new LanguageInfo();
            // system.cfg
            var systemConfigFile = new CfgFile(GameConstants.GetSystemConfigPath(_currentGame.RootFolderPath));
            var systemConfigData = systemConfigFile.Read();
            LoadLanguageInfo(systemConfigData, languageInfo);
            // user.cfg
            var userConfigFile = new CfgFile(GameConstants.GetUserConfigPath(_currentGame.RootFolderPath));
            var userConfigData = userConfigFile.Read();
            if (FixUserConfigLanguageInfo(userConfigData, languageInfo))
            {
                userConfigFile.Save(userConfigData);
            }
            LanguageInfo = languageInfo;
        }

        public bool SaveCurrentLanguage(string languageName)
        {
            if (!string.IsNullOrEmpty(languageName))
            {
                if (string.Compare(LanguageInfo.Current, languageName, StringComparison.OrdinalIgnoreCase) == 0)
                    return true;
                var userConfigFile = new CfgFile(GameConstants.GetUserConfigPath(_currentGame.RootFolderPath));
                var userConfigData = userConfigFile.Read();
                userConfigData.AddOrUpdateRow(GameConstants.CurrentLanguageKey, languageName);
                if (userConfigFile.Save(userConfigData))
                {
                    LanguageInfo.Current = languageName;
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool RemoveCurrentLanguage()
        {
            var userConfigFile = new CfgFile(GameConstants.GetUserConfigPath(_currentGame.RootFolderPath));
            var userConfigData = userConfigFile.Read();
            if (userConfigData.RemoveRow(GameConstants.CurrentLanguageKey) != null &&
                userConfigFile.Save(userConfigData))
            {
                LanguageInfo.Current = null;
                return true;
            }
            return false;
        }

        private static bool FixUserConfigLanguageInfo(CfgData cfgData, LanguageInfo languageInfo)
        {
            if (cfgData.Any())
            {
                var anyFieldFixed = cfgData.RemoveRow(GameConstants.SystemLanguagesKey) != null;
                if (cfgData.TryGetValue(GameConstants.CurrentLanguageKey, out var value) && (value != null))
                {
                    if (languageInfo.Languages.Contains(value))
                    {
                        languageInfo.Current = value;
                    }
                    else
                    {
                        cfgData.RemoveRow(GameConstants.CurrentLanguageKey);
                        anyFieldFixed = true;
                    }
                }
                return anyFieldFixed;
            }
            return false;
        }

        private static void LoadLanguageInfo(CfgData cfgData, LanguageInfo languageInfo)
        {
            if (cfgData.TryGetValue(GameConstants.SystemLanguagesKey, out var value) && (value != null))
            {
                languageInfo.Languages.Clear();
                var languages = value.Split(',');
                foreach (var language in languages)
                {
                    languageInfo.Languages.Add(language.Trim());
                }
            }
            if (cfgData.TryGetValue(GameConstants.CurrentLanguageKey, out value) && (value != null))
            {
                languageInfo.Current = value;
            }
        }
    }
}
