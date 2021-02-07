using NSW.StarCitizen.Tools.Lib.Global;
using NSW.StarCitizen.Tools.Lib.Update;

namespace NSW.StarCitizen.Tools.Lib.Localization
{
    public interface ILocalizationRepository : IUpdateRepository
    {
        GameMode Mode { get; }
        ILocalizationInstaller Installer { get; }
    }
}