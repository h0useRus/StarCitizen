using System;
using System.IO;
using System.Linq;
using NSW.StarCitizen.Tools.Lib.Global;
using NSW.StarCitizen.Tools.Lib.Helpers;

namespace NSW.StarCitizen.Tools.Lib.Localization
{
    public class GameSettings
    {
        private readonly GameInfo _currentGame;
        private string? _userConfigLanguage;
        public LanguageInfo LanguageInfo { get; private set; } = new LanguageInfo();

        public GameSettings(GameInfo currentGame)
        {
            _currentGame = currentGame;
        }

        public CfgData Load()
        {
            var languageInfo = new LanguageInfo();
            // system.cfg
            var systemConfigFile = new CfgFile(GameConstants.GetSystemConfigPath(_currentGame.RootFolderPath));
            var systemConfigData = systemConfigFile.Read();
            // languages.ini
            var languagesConfigFile = new CfgFile(Path.Combine(GameConstants.GetDataFolderPath(_currentGame.RootFolderPath), "languages.ini"));
            var languagesConfigData = languagesConfigFile.Read();
            LoadLanguageInfo(systemConfigData, languagesConfigData, languageInfo);
            // user.cfg
            var userConfigFile = new CfgFile(GameConstants.GetUserConfigPath(_currentGame.RootFolderPath));
            var userConfigData = userConfigFile.Read();
            if (userConfigData.TryGetValue(GameConstants.CurrentLanguageKey, out var value))
                _userConfigLanguage = value;
            else
                _userConfigLanguage = null;
            if (FixUserConfigLanguageInfo(userConfigData, languageInfo))
            {
                userConfigFile.Save(userConfigData);
            }
            LanguageInfo = languageInfo;
            return userConfigData;
        }

        public bool SaveConfig(CfgData config)
        {
            var userConfigFile = new CfgFile(GameConstants.GetUserConfigPath(_currentGame.RootFolderPath));
            if (_userConfigLanguage != null)
            {
                config.AddOrUpdateRow(GameConstants.CurrentLanguageKey, _userConfigLanguage);
            }
            return userConfigFile.Save(config);
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
                    if (languageInfo.Languages.ContainsKey(value))
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

        private static void LoadLanguageInfo(CfgData cfgData, CfgData languagesData, LanguageInfo languageInfo)
        {
            if (cfgData.TryGetValue(GameConstants.SystemLanguagesKey, out var value) && (value != null))
            {
                languageInfo.Languages.Clear();
                var languages = value.Split(',');
                foreach (var language in languages)
                {
                    var trimmedLanguage = language.Trim();
                    if (languageInfo.Languages.ContainsKey(trimmedLanguage))
                    {
                        continue;   // skip duplicate languages
                    }
                    if (languagesData.TryGetValue(trimmedLanguage, out var languageLabel) &&
                        languageLabel != null && !string.IsNullOrWhiteSpace(languageLabel))
                    {
                        languageInfo.Languages.Add(trimmedLanguage, languageLabel.Trim());
                    }
                    else
                    {
                        languageInfo.Languages.Add(trimmedLanguage, trimmedLanguage);
                    }
                }
            }
            if (cfgData.TryGetValue(GameConstants.CurrentLanguageKey, out value) && (value != null))
            {
                languageInfo.Current = value;
            }
        }
    }
}
