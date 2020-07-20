using System.IO;
using System.IO.Compression;

namespace NSW.StarCitizen.Tools.Localization
{
    public class DefaultLocalizationInstaller : ILocalizationInstaller
    {
        private const string PatcherOriginalName = "patcher.bin";
        private const string PatcherLibraryName = "CIGDevelopmentTools.dll";
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
                        entry.ExtractToFile(
                            Path.Combine(destinationFolder, "Bin64", isDisabledMode ? PatcherOriginalName : PatcherLibraryName), true);
                    }

                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Validate(string destinationFolder) => true;
    }
}