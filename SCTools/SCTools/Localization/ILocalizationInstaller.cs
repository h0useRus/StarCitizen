using System.Threading.Tasks;

namespace NSW.StarCitizen.Tools.Localization
{
    public interface ILocalizationInstaller
    {
        bool Unpack(string zipFileName, string destinationFolder, bool isDisabledMode);
        bool Validate(string destinationFolder, bool isDisabledMode);
        LocalizationInstallationType GetInstallationType(string destinationFolder);
        LocalizationInstallationType RevertLocalization(string destinationFolder);
    }
}