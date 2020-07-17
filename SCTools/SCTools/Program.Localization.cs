
using System;
using System.Collections.Generic;
using NSW.StarCitizen.Tools.Localization;

namespace NSW.StarCitizen.Tools
{
    public static partial class Program
    {
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
    }
}