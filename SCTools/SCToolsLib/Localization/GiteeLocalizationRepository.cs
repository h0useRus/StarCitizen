using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NSW.StarCitizen.Tools.Lib.Global;
using NSW.StarCitizen.Tools.Lib.Update;

namespace NSW.StarCitizen.Tools.Lib.Localization
{
    public class GiteeLocalizationRepository : GiteeUpdateRepository, ILocalizationRepository
    {
        public GameMode Mode { get; }

        public ILocalizationInstaller Installer { get; }

        public GiteeLocalizationRepository(ILocalizationInstaller installer, HttpClient httpClient, GameMode mode, string name, string repository)
            : base(httpClient, GiteeUpdateInfo.Factory.NewWithVersionByName(), name, repository)
        {
            Mode = mode;
            Installer = installer;
        }

        public override async Task<List<UpdateInfo>> GetAllAsync(CancellationToken cancellationToken)
        {
            var updates = await base.GetAllAsync(cancellationToken).ConfigureAwait(false);
            return updates.Where(i => IsTagNameForMode(i.TagName, Mode)).ToList();
        }

        private static bool IsTagNameForMode(string tagName, GameMode mode)
        {
            if (mode != GameMode.LIVE)
            {
                return tagName.EndsWith(string.Format(CultureInfo.InvariantCulture, "-{0}", mode), StringComparison.OrdinalIgnoreCase);
            }
            int index = tagName.LastIndexOf("-", StringComparison.Ordinal);
            if (index < 0 || index == tagName.Length - 1)
            {
                return true;
            }
            if (Enum.TryParse(tagName.Substring(index + 1), true, out GameMode tagMode))
            {
                return tagMode == mode;
            }
            return true;
        }
    }
}
