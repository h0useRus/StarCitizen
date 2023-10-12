using System;
using System.Collections.Generic;
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

        public CfgData Load(ISet<string>? languages)
        {
            // languages.ini
            var languagesConfigFile = new CfgFile(Path.Combine(GameConstants.GetDataFolderPath(_currentGame.RootFolderPath), "languages.ini"));
            var languagesConfigData = languagesConfigFile.Read();
            var languageInfo = LoadLanguageInfo(languages, languagesConfigData);
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
                var anyFieldFixed = false;
                if (cfgData.TryGetValue(GameConstants.CurrentLanguageKey, out var value) && (value != null))
                {
                    if (languageInfo.Languages.ContainsKey(value))
                    {
                        languageInfo.Current = value;
                    }
                    else
                    {
                        cfgData.RemoveRow(GameConstants.CurrentLanguageKey);
                        languageInfo.Current = GameConstants.EnglishLocalization;
                        anyFieldFixed = true;
                    }
                }
                else
                {
                    languageInfo.Current = GameConstants.EnglishLocalization;
                }
                return anyFieldFixed;
            }
            languageInfo.Current = GameConstants.EnglishLocalization;
            return false;
        }

        private static LanguageInfo LoadLanguageInfo(ISet<string>? languages, CfgData languagesData)
        {
            var languageInfo = new LanguageInfo();
            if (languages != null)
            {
                foreach (var language in languages)
                {
                    if (languageInfo.Languages.ContainsKey(language))
                    {
                        continue;   // skip duplicate languages
                    }
                    languageInfo.Languages.Add(language, GetLanguageLabel(languagesData, language));
                }
            }
            if (!languageInfo.Languages.ContainsKey(GameConstants.EnglishLocalization))
            {
                languageInfo.Languages.Add(GameConstants.EnglishLocalization,
                    GetLanguageLabel(languagesData, GameConstants.EnglishLocalization));
            }
            return languageInfo;
        }

        private static string GetLanguageLabel(CfgData languagesData, string language)
        {
            if (languagesData.TryGetValue(language, out var languageLabel) &&
                languageLabel != null && !string.IsNullOrWhiteSpace(languageLabel))
            {
                return languageLabel.Trim();
            }
            return language;
        }
    }
}
