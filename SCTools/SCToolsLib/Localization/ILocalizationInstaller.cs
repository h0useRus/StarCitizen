using System;
using System.Diagnostics;
using NSW.StarCitizen.Tools.Lib.Update;

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
        InstallStatus Install(string sourceFolder, string destinationFolder, FilesIndex.DiffList diffList);
        bool WriteTimestamp(DateTimeOffset date, string destinationFolder);
        UninstallStatus Uninstall(string destinationFolder);
        LocalizationInstallationType GetInstallationType(string destinationFolder);
        LocalizationInstallationType SetEnableLocalization(string destinationFolder, bool enabled);
        FileVersionInfo? GetPatcherFileVersionInfo(string destinationFolder);
    }
}