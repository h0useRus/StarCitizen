using NSW.StarCitizen.Tools.Global;
using NSW.StarCitizen.Tools.Update;

namespace NSW.StarCitizen.Tools.Localization
{
    public interface ILocalizationRepository : IUpdateRepository
    {
        GameMode Mode { get; }
        ILocalizationInstaller Installer { get; }
    }
}