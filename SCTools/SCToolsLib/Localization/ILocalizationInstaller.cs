
using System;

namespace NSW.StarCitizen.Tools.Lib.Localization
{
    public enum InstallStatus
    {
        Success,
        PackageError,
        VerifyError,
        FileError,
        UnknownError,
    }

    public enum UninstallStatus
    {
        Success,
        Partial,
        Failed
    }

    public interface ILocalizationInstaller
    {
        bool Verify(string zipFileName);
        InstallStatus Install(string zipFileName, string destinationFolder);
        bool WriteTimestamp(DateTimeOffset date, string destinationFolder);
        UninstallStatus Uninstall(string destinationFolder);
        LocalizationInstallationType GetInstallationType(string destinationFolder);
        LocalizationInstallationType RevertLocalization(string destinationFolder);
    }
}