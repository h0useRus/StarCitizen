using System;
using System.Linq;
using NSW.StarCitizen.Tools.Global;
using NSW.StarCitizen.Tools.Helpers;

namespace NSW.StarCitizen.Tools.Localization
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
            var systemConfigFile = new CfgFile(GameConstants.GetSystemConfigPath(_currentGame.RootFolder.FullName));
            var systemConfigData = systemConfigFile.Read();
            LoadLanguageInfo(systemConfigData, languageInfo);
            // user.cfg
            var userConfigFile = new CfgFile(GameConstants.GetUserConfigPath(_currentGame.RootFolder.FullName));
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
                var userConfigFile = new CfgFile(GameConstants.GetUserConfigPath(_currentGame.RootFolder.FullName));
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

        private static bool FixUserConfigLanguageInfo(CfgData cfgData, LanguageInfo languageInfo)
        {
            if (cfgData.Any())
            {
                bool anyFieldFixed = cfgData.RemoveRow(GameConstants.SystemLanguagesKey) != null;
                if (cfgData.TryGetValue(GameConstants.CurrentLanguageKey, out var value))
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
            if (cfgData.TryGetValue(GameConstants.SystemLanguagesKey, out var value))
            {
                languageInfo.Languages.Clear();
                var languages = value.Split(',');
                foreach (var language in languages)
                {
                    languageInfo.Languages.Add(language.Trim());
                }
            }
            if (cfgData.TryGetValue(GameConstants.CurrentLanguageKey, out value))
            {
                languageInfo.Current = value;
            }
        }
    }
}
