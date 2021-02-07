using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using NLog;
using NSW.StarCitizen.Tools.Lib.Global;
using NSW.StarCitizen.Tools.Lib.Helpers;
using NSW.StarCitizen.Tools.Lib.Properties;

namespace NSW.StarCitizen.Tools.Lib.Localization
{
    public class DefaultLocalizationInstaller : ILocalizationInstaller
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public InstallStatus Install(string zipFileName, string destinationFolder)
        {
            if (!Directory.Exists(destinationFolder))
            {
                _logger.Error($"Install directory is not exist: {destinationFolder}");
                return InstallStatus.FileError;
            }
            DirectoryInfo? unpackDataDir = null;
            DirectoryInfo? backupDataDir = null;
            var dataPathDir = new DirectoryInfo(GameConstants.GetDataFolderPath(destinationFolder));
            try
            {
                var unpackDataDirPath = Path.Combine(destinationFolder, "temp_" + Path.GetRandomFileName());
                unpackDataDir = Directory.CreateDirectory(unpackDataDirPath);
                if (!Unpack(zipFileName, unpackDataDir.FullName))
                {
                    _logger.Error($"Failed unpack install package to: {unpackDataDirPath}");
                    return InstallStatus.PackageError;
                }
                var newLibraryPath = Path.Combine(unpackDataDir.FullName, GameConstants.PatcherOriginalName);
                using var libraryCertVerifier = new FileCertVerifier(Resources.CoreSigning);
                if (!libraryCertVerifier.VerifyFile(newLibraryPath))
                {
                    _logger.Error("Core certificate is invalid. Abort installation");
                    return InstallStatus.VerifyError;
                }
                if (dataPathDir.Exists)
                {
                    var backupDataDirPath = Path.Combine(destinationFolder, "backup_" + Path.GetRandomFileName());
                    Directory.Move(dataPathDir.FullName, backupDataDirPath);
                    backupDataDir = new DirectoryInfo(backupDataDirPath);
                }
                Directory.Move(GameConstants.GetDataFolderPath(unpackDataDir.FullName), dataPathDir.FullName);
                if (backupDataDir != null)
                {
                    FileUtils.DeleteDirectoryNoThrow(backupDataDir, true);
                    backupDataDir = null;
                }
                var enabledLibraryPath = GameConstants.GetEnabledPatcherPath(destinationFolder);
                if (File.Exists(enabledLibraryPath))
                {
                    File.Delete(enabledLibraryPath);
                    File.Move(newLibraryPath, enabledLibraryPath);
                }
                else
                {
                    var disabledLibraryPath = GameConstants.GetDisabledPatcherPath(destinationFolder);
                    if (File.Exists(disabledLibraryPath))
                    {
                        File.Delete(disabledLibraryPath);
                    }
                    File.Move(newLibraryPath, disabledLibraryPath);
                }
            }
            catch (CryptographicException e)
            {
                _logger.Error(e, "Exception during verify core");
                return InstallStatus.VerifyError;
            }
            catch (IOException e)
            {
                _logger.Error(e, "I/O exception during install");
                return InstallStatus.FileError;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Unexpected exception during install");
                return InstallStatus.UnknownError;
            }
            finally
            {
                if (unpackDataDir != null)
                {
                    FileUtils.DeleteDirectoryNoThrow(unpackDataDir, true);
                }
                if (backupDataDir != null)
                {
                    RestoreDirectory(backupDataDir, dataPathDir);
                }
            }
            return InstallStatus.Success;
        }

        public UninstallStatus Uninstall(string destinationFolder)
        {
            if (!Directory.Exists(destinationFolder))
                return UninstallStatus.Failed;
            string enabledLibraryPath = GameConstants.GetEnabledPatcherPath(destinationFolder);
            if (File.Exists(enabledLibraryPath) && !FileUtils.DeleteFileNoThrow(enabledLibraryPath))
                return UninstallStatus.Failed;
            var result = UninstallStatus.Success;
            var disabledLibraryPath = GameConstants.GetDisabledPatcherPath(destinationFolder);
            if (File.Exists(disabledLibraryPath) && !FileUtils.DeleteFileNoThrow(disabledLibraryPath))
                result = UninstallStatus.Partial;
            var dataPathDir = new DirectoryInfo(GameConstants.GetDataFolderPath(destinationFolder));
            if (dataPathDir.Exists && !FileUtils.DeleteDirectoryNoThrow(dataPathDir, true))
                result = UninstallStatus.Partial;
            return result;
        }

        public LocalizationInstallationType GetInstallationType(string destinationFolder)
        {
            if (!Directory.Exists(destinationFolder))
                return LocalizationInstallationType.None;
            if (File.Exists(GameConstants.GetEnabledPatcherPath(destinationFolder)))
                return LocalizationInstallationType.Enabled;
            if (File.Exists(GameConstants.GetDisabledPatcherPath(destinationFolder)))
                return LocalizationInstallationType.Disabled;
            return LocalizationInstallationType.None;
        }

        public LocalizationInstallationType RevertLocalization(string destinationFolder)
        {
            if (!Directory.Exists(destinationFolder))
                return LocalizationInstallationType.None;
            string enabledLibraryPath = GameConstants.GetEnabledPatcherPath(destinationFolder);
            string disabledLibraryPath = GameConstants.GetDisabledPatcherPath(destinationFolder);

            if (File.Exists(enabledLibraryPath))
            {
                if (File.Exists(disabledLibraryPath))
                    FileUtils.DeleteFileNoThrow(disabledLibraryPath);
                File.Move(enabledLibraryPath, disabledLibraryPath);
                return LocalizationInstallationType.Disabled;
            }

            if (File.Exists(disabledLibraryPath))
            {
                if (File.Exists(enabledLibraryPath))
                    FileUtils.DeleteFileNoThrow(enabledLibraryPath);
                File.Move(disabledLibraryPath, enabledLibraryPath);
                return LocalizationInstallationType.Enabled;
            }

            return LocalizationInstallationType.None;
        }

        private static bool Unpack(string zipFileName, string destinationFolder)
        {
            using var archive = ZipFile.OpenRead(zipFileName);
            if (archive.Entries.Count == 0)
            {
                _logger.Error($"Failed unpack archive. No entries found: {zipFileName}");
                return false;
            }
            var dataExtracted = false;
            var coreExtracted = false;
            var rootEntry = archive.Entries[0];
            var dataPathStart = GameConstants.DataFolderName + "/";
            //extract only data folder and core module
            foreach (var entry in archive.Entries)
            {
                if (entry.FullName.StartsWith(rootEntry.FullName, true, CultureInfo.InvariantCulture))
                {
                    var relativePath = entry.FullName.Substring(rootEntry.FullName.Length);
                    if (string.IsNullOrEmpty(entry.Name) && relativePath.EndsWith("/"))
                    {
                        var dir = Path.Combine(destinationFolder, relativePath);
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);
                    }
                    else if (relativePath.StartsWith(dataPathStart, true, CultureInfo.InvariantCulture))
                    {
                        entry.ExtractToFile(Path.Combine(destinationFolder, relativePath), true);
                        dataExtracted = true;
                    }
                    else if (relativePath.Equals(GameConstants.PatcherOriginalName, StringComparison.OrdinalIgnoreCase))
                    {
                        entry.ExtractToFile(Path.Combine(destinationFolder, relativePath), true);
                        coreExtracted = true;
                    }
                }
            }
            if (!dataExtracted || !coreExtracted)
            {
                _logger.Error($"Wrong localization archive: hasData={dataExtracted}, hasCore={coreExtracted}");
                return false;
            }
            return true;
        }

        private static void RestoreDirectory(DirectoryInfo dir, DirectoryInfo destDir)
        {
            if (dir.Exists)
            {
                try
                {
                    FileUtils.DeleteDirectoryNoThrow(destDir, true);
                    Directory.Move(dir.FullName, destDir.FullName);
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"Unable restore data from directory: {dir.FullName}");
                    FileUtils.DeleteDirectoryNoThrow(dir, true);
                }
            }
        }
    }
}