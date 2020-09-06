using NSW.StarCitizen.Tools.Update;

namespace NSW.StarCitizen.Tools.Localization
{
    public interface ILocalizationRepository : IUpdateRepository
    {
        ILocalizationInstaller Installer { get; }
    }
}