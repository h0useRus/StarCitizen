
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NSW.StarCitizen.Tools.Global;
using NSW.StarCitizen.Tools.Helpers;
using NSW.StarCitizen.Tools.Localization;
using NSW.StarCitizen.Tools.Settings;

namespace NSW.StarCitizen.Tools
{
    public static partial class Program
    {
        private const string KeySysLanguages = "sys_languages";
        private const string KeyCurLanguage = "g_language";

        private static Dictionary<string, ILocalizationRepository> _localizationRepositories;
        public static Dictionary<string, ILocalizationRepository> LocalizationRepositories
        {
            get
            {
                if (_localizationRepositories == null)
                {
                    _localizationRepositories = new Dictionary<string, ILocalizationRepository>(StringComparer.OrdinalIgnoreCase);
                    foreach (var repository in Settings.Localization.Repositories)
                    {
                        if (!_localizationRepositories.ContainsKey(repository.Repository))
                        {
                            // Only git supported
                            _localizationRepositories[repository.Repository] = new GitHubLocalizationRepository(repository.Name, repository.Repository);
                        }
                    }
                }
                return _localizationRepositories;
            }
        }

        private static ILocalizationRepository _currentRepository;
        public static ILocalizationRepository CurrentRepository
        {
            get => _currentRepository;
            set => SetCurrentLocalizationRepository(value);
        }

        public static LocalizationInstallation CurrentInstallation { get; private set; }

        public static ILocalizationRepository GetCurrentLocalizationRepository()
        {
            var info = Settings.Localization.Installations?.FirstOrDefault(i => i.Mode == CurrentGame.Mode);
            if (info != null && LocalizationRepositories.ContainsKey(info.Repository))
                return LocalizationRepositories[info.Repository];
            return null;
        }

        public static void SetCurrentLocalizationRepository(ILocalizationRepository localizationRepository)
        {
            if (localizationRepository?.Repository == _currentRepository?.Repository) return;

            _currentRepository = localizationRepository;

            if (_currentRepository == null) return;

            Settings.Localization.Installations ??= new List<LocalizationInstallation>();

            CurrentInstallation = Settings.Localization.Installations
                .FirstOrDefault(i => i.Mode == CurrentGame.Mode
                                  && string.Compare(i.Repository, _currentRepository.Repository, StringComparison.OrdinalIgnoreCase) == 0);
            if (CurrentInstallation == null)
            {
                CurrentInstallation = new LocalizationInstallation
                {
                    Mode = CurrentGame.Mode,
                    Repository = _currentRepository.Repository,
                    MonitorRefreshTime = Settings.Localization.MonitorRefreshTime
                };
                Settings.Localization.Installations.Add(CurrentInstallation);
            }
            SaveAppSettings();

            _currentRepository.CurrentVersion ??= new LocalizationInfo {Name = CurrentInstallation.LastVersion ?? "N/A"};
        }

        public static LanguageInfo GetLanguagesConfiguration()
        {
            var result = new LanguageInfo();
            var fileName = Path.Combine(CurrentGame.RootFolder.FullName, "data", "system.cfg");
            var cfgFile = new CfgFile(fileName);
            var data = cfgFile.Read();

            if (data.TryGetValue(KeySysLanguages, out var value))
            {
                var languages = value.Split(',');
                foreach (var language in languages)
                {
                    result.Languages.Add(language.Trim());
                }
            }

            if (data.TryGetValue(KeyCurLanguage, out value))
            {
                result.Current = value;
            }

            return result;
        }
    }
}