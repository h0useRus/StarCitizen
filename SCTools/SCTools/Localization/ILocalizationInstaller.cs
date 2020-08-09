using System.Threading.Tasks;

namespace NSW.StarCitizen.Tools.Localization
{
    public enum InstallStatus
    {
        Success,
        PackageError,
        VerifyError,
        FileError,
        UnknownError,
    }

    public interface ILocalizationInstaller
    {
        InstallStatus Install(string zipFileName, string destinationFolder);
        LocalizationInstallationType GetInstallationType(string destinationFolder);
        LocalizationInstallationType RevertLocalization(string destinationFolder);
    }
}