using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using NSW.StarCitizen.Tools.Helpers;
using NSW.StarCitizen.Tools.Properties;

namespace NSW.StarCitizen.Tools.Localization
{
    public class DefaultLocalizationInstaller : ILocalizationInstaller
    {
        private const string PatcherOriginalName = "patcher.bin";
        private const string PatcherLibraryName = "CIGDevelopmentTools.dll";
        private const string DataDirName = "data";

        public InstallStatus Install(string zipFileName, string destinationFolder)
        {
            DirectoryInfo unpackDir = null;
            string unpackDirPath = Path.Combine(destinationFolder, "temp_" + Path.GetRandomFileName());
            try
            {
                unpackDir = Directory.CreateDirectory(unpackDirPath);
                if (!Unpack(zipFileName, unpackDir.FullName))
                {
                    return InstallStatus.PackageError;
                }
                string newLibraryPath = Path.Combine(unpackDir.FullName, PatcherOriginalName);
                FileCertVerifier libraryCertVerifier = new FileCertVerifier(Resources.CoreSigning);
                if (!libraryCertVerifier.VerifyFile(newLibraryPath))
                {
                    return InstallStatus.VerifyError;
                }
                DirectoryInfo dataPathDir = new DirectoryInfo(Path.Combine(destinationFolder, DataDirName));
                if (dataPathDir.Exists)
                    dataPathDir.Delete(true);
                Directory.Move(Path.Combine(unpackDir.FullName, DataDirName), dataPathDir.FullName);
                string enabledLibraryPath = GetEnabledLibraryPath(destinationFolder);
                if (File.Exists(enabledLibraryPath))
                {
                    File.Delete(enabledLibraryPath);
                    File.Move(newLibraryPath, enabledLibraryPath);
                }
                else
                {
                    string disabledLibraryPath = GetDisabledLibraryPath(destinationFolder);
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
                if (unpackDir != null)
                {
                    DeleteDirectoryRecursive(unpackDir);
                }
            }
            return InstallStatus.Success;
        }

        public LocalizationInstallationType GetInstallationType(string destinationFolder)
        {
            if (File.Exists(GetEnabledLibraryPath(destinationFolder)))
                return LocalizationInstallationType.Enabled;
            if (File.Exists(GetDisabledLibraryPath(destinationFolder)))
                return LocalizationInstallationType.Disabled;
            return LocalizationInstallationType.None;
        }

        public LocalizationInstallationType RevertLocalization(string destinationFolder)
        {
            string enabledLibraryPath = GetEnabledLibraryPath(destinationFolder);
            string disabledLibraryPath = GetDisabledLibraryPath(destinationFolder);

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
            var dataPathStart = DataDirName + "/";
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
                    else if (relativePath.Equals(PatcherOriginalName, StringComparison.OrdinalIgnoreCase))
                    {
                        entry.ExtractToFile(Path.Combine(destinationFolder, relativePath), true);
                        coreExtracted = true;
                    }
                }
            }
            return dataExtracted && coreExtracted;
        }

        private static string GetEnabledLibraryPath(string destinationFolder)
        {
            return Path.Combine(destinationFolder, Program.BinFolder, PatcherLibraryName);
        }

        private static string GetDisabledLibraryPath(string destinationFolder)
        {
            return Path.Combine(destinationFolder, Program.BinFolder, PatcherOriginalName);
        }

        private static void DeleteDirectoryRecursive(DirectoryInfo dir)
        {
            try
            {
                dir.Delete(true);
            }
            catch
            {
                // ignore directory not removed
            }
        }
    }
}