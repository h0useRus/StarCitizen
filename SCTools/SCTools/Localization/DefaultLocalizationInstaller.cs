using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using NSW.StarCitizen.Tools.Global;
using NSW.StarCitizen.Tools.Helpers;
using NSW.StarCitizen.Tools.Properties;

namespace NSW.StarCitizen.Tools.Localization
{
    public class DefaultLocalizationInstaller : ILocalizationInstaller
    {
        public InstallStatus Install(string zipFileName, string destinationFolder)
        {
            DirectoryInfo unpackDataDir = null;
            DirectoryInfo backupDataDir = null;
            DirectoryInfo dataPathDir = new DirectoryInfo(GameConstants.GetDataFolderPath(destinationFolder));
            try
            {
                string unpackDataDirPath = Path.Combine(destinationFolder, "temp_" + Path.GetRandomFileName());
                unpackDataDir = Directory.CreateDirectory(unpackDataDirPath);
                if (!Unpack(zipFileName, unpackDataDir.FullName))
                {
                    return InstallStatus.PackageError;
                }
                string newLibraryPath = Path.Combine(unpackDataDir.FullName, GameConstants.PatcherOriginalName);
                using var libraryCertVerifier = new FileCertVerifier(Resources.CoreSigning);
                if (!libraryCertVerifier.VerifyFile(newLibraryPath))
                {
                    return InstallStatus.VerifyError;
                }
                if (dataPathDir.Exists)
                {
                    string backupDataDirPath = Path.Combine(destinationFolder, "backup_" + Path.GetRandomFileName());
                    Directory.Move(dataPathDir.FullName, backupDataDirPath);
                    backupDataDir = new DirectoryInfo(backupDataDirPath);
                }
                Directory.Move(GameConstants.GetDataFolderPath(unpackDataDir.FullName), dataPathDir.FullName);
                if (backupDataDir != null)
                {
                    FileUtils.DeleteDirectoryNoThrow(backupDataDir, true);
                    backupDataDir = null;
                }
                string enabledLibraryPath = GameConstants.GetEnabledPatcherPath(destinationFolder);
                if (File.Exists(enabledLibraryPath))
                {
                    File.Delete(enabledLibraryPath);
                    File.Move(newLibraryPath, enabledLibraryPath);
                }
                else
                {
                    string disabledLibraryPath = GameConstants.GetDisabledPatcherPath(destinationFolder);
                    if (File.Exists(disabledLibraryPath))
                    {
                        File.Delete(disabledLibraryPath);
                    }
                    File.Move(newLibraryPath, disabledLibraryPath);
                }
            }
            catch (CryptographicException)
            {
                return InstallStatus.VerifyError;
            }
            catch (IOException)
            {
                return InstallStatus.FileError;
            }
            catch (Exception)
            {
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
            string enabledLibraryPath = GameConstants.GetEnabledPatcherPath(destinationFolder);
            if (File.Exists(enabledLibraryPath) && !FileUtils.DeleteFileNoThrow(enabledLibraryPath))
                return UninstallStatus.Failed;
            var result = UninstallStatus.Success;
            string disabledLibraryPath = GameConstants.GetDisabledPatcherPath(destinationFolder);
            if (File.Exists(disabledLibraryPath) && !FileUtils.DeleteFileNoThrow(disabledLibraryPath))
                result = UninstallStatus.Partial;
            DirectoryInfo dataPathDir = new DirectoryInfo(GameConstants.GetDataFolderPath(destinationFolder));
            if (dataPathDir.Exists && !FileUtils.DeleteDirectoryNoThrow(dataPathDir, true))
                result = UninstallStatus.Partial;
            return result;
        }

        public LocalizationInstallationType GetInstallationType(string destinationFolder)
        {
            if (File.Exists(GameConstants.GetEnabledPatcherPath(destinationFolder)))
                return LocalizationInstallationType.Enabled;
            if (File.Exists(GameConstants.GetDisabledPatcherPath(destinationFolder)))
                return LocalizationInstallationType.Disabled;
            return LocalizationInstallationType.None;
        }

        public LocalizationInstallationType RevertLocalization(string destinationFolder)
        {
            string enabledLibraryPath = GameConstants.GetEnabledPatcherPath(destinationFolder);
            string disabledLibraryPath = GameConstants.GetDisabledPatcherPath(destinationFolder);

            if (File.Exists(enabledLibraryPath))
            {
                File.Move(enabledLibraryPath, disabledLibraryPath);
                return LocalizationInstallationType.Disabled;
            }

            if (File.Exists(disabledLibraryPath))
            {
                File.Move(disabledLibraryPath, enabledLibraryPath);
                return LocalizationInstallationType.Enabled;
            }

            return LocalizationInstallationType.None;
        }

        private bool Unpack(string zipFileName, string destinationFolder)
        {
            using var archive = ZipFile.OpenRead(zipFileName);
            if (archive.Entries.Count == 0)
            {
                return false;
            }
            bool dataExtracted = false;
            bool coreExtracted = false;
            var rootEntry = archive.Entries[0];
            var dataPathStart = GameConstants.DataFolderName + "/";
            //extract only data folder and core module
            foreach (var entry in archive.Entries)
            {
                if (entry.FullName.StartsWith(rootEntry.FullName, true, CultureInfo.InvariantCulture))
                {
                    string relativePath = entry.FullName.Substring(rootEntry.FullName.Length);
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
            return dataExtracted && coreExtracted;
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
                catch
                {
                    FileUtils.DeleteDirectoryNoThrow(dir, true);
                }
            }
        }
    }
}