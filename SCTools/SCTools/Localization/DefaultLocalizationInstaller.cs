using System.IO;
using System.IO.Compression;
using NSW.StarCitizen.Tools.Helpers;
using NSW.StarCitizen.Tools.Properties;

namespace NSW.StarCitizen.Tools.Localization
{
    public class DefaultLocalizationInstaller : ILocalizationInstaller
    {
        private const string PatcherOriginalName = "patcher.bin";
        private const string PatcherLibraryName = "CIGDevelopmentTools.dll";

        private static string GetLibraryName(string destinationFolder, bool isDisabledMode)
            => Path.Combine(destinationFolder, Program.BinFolder, isDisabledMode ? PatcherOriginalName : PatcherLibraryName);
        public bool Unpack(string zipFileName, string destinationFolder, bool isDisabledMode)
        {
            try
            {
                using var archive = ZipFile.OpenRead(zipFileName);
                var rootEntry = archive.Entries[0];
                //extract only data folder
                foreach (var entry in archive.Entries)
                {
                    if (entry.FullName.Contains(@"/data/"))
                    {
                        if (string.IsNullOrWhiteSpace(entry.Name))
                        {
                            var dir = Path.Combine(destinationFolder,
                                entry.FullName.Replace(rootEntry.FullName, string.Empty));
                            if (!Directory.Exists(dir))
                                Directory.CreateDirectory(dir);
                        }
                        else
                        {
                            entry.ExtractToFile(
                                Path.Combine(destinationFolder,
                                    entry.FullName.Replace(rootEntry.FullName, string.Empty)), true);
                        }
                    }
                    else if (entry.FullName.EndsWith(PatcherOriginalName))
                    {
                        entry.ExtractToFile(GetLibraryName(destinationFolder, isDisabledMode), true);
                    }

                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Validate(string destinationFolder, bool isDisabledMode)
        {
            var fileName = GetLibraryName(destinationFolder, isDisabledMode);
            return VerifyHelper.VerifyFile(Resources.SigningCertificate, fileName);
        }

        public LocalizationInstallationType GetInstallationType(string destinationFolder)
        {
            if (File.Exists(GetLibraryName(destinationFolder, true)))
                return LocalizationInstallationType.Disabled;
            return File.Exists(GetLibraryName(destinationFolder, false))
                ? LocalizationInstallationType.Enabled
                : LocalizationInstallationType.None;
        }

        public LocalizationInstallationType RevertLocalization(string destinationFolder)
        {
            if (File.Exists(GetLibraryName(destinationFolder, false)))
            {
                File.Move(GetLibraryName(destinationFolder, false), GetLibraryName(destinationFolder, true));
                return LocalizationInstallationType.Disabled;
            }

            if (File.Exists(GetLibraryName(destinationFolder, true)))
            {
                File.Move(GetLibraryName(destinationFolder, true), GetLibraryName(destinationFolder, false));
                return LocalizationInstallationType.Enabled;
            }

            return LocalizationInstallationType.None;
        }
    }
}