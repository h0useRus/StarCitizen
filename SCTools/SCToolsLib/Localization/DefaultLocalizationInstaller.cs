using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using NLog;
using NSW.StarCitizen.Tools.Lib.Global;
using NSW.StarCitizen.Tools.Lib.Helpers;
using NSW.StarCitizen.Tools.Lib.Update;

namespace NSW.StarCitizen.Tools.Lib.Localization
{
    public class DefaultLocalizationInstaller : ILocalizationInstaller
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public bool Verify(string zipFileName)
        {
            try
            {
                using var archive = ZipFile.OpenRead(zipFileName);
                if (archive.Entries.Count != 0)
                {
                    var rootEntry = archive.Entries[0];
                    var dataPathStart = GameConstants.DataFolderName + "/";
                    foreach (var entry in archive.Entries)
                    {
                        if (entry.FullName.StartsWith(rootEntry.FullName, StringComparison.OrdinalIgnoreCase))
                        {
                            var relativePath = entry.FullName.Substring(rootEntry.FullName.Length);
                            if (relativePath.StartsWith(dataPathStart, StringComparison.OrdinalIgnoreCase) &&
                                entry.Name.Equals(GameConstants.GlobalIniName, StringComparison.OrdinalIgnoreCase))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch
            {
                // just silently ignore
            }
            return false;
        }

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

        public InstallStatus Install(string sourceFolder, string destinationFolder, FilesIndex.DiffList diffList)
        {
            if (!Directory.Exists(destinationFolder))
            {
                _logger.Error($"Install directory is not exist: {destinationFolder}");
                return InstallStatus.FileError;
            }
            if (diffList.ChangedFiles.Count == 0 && diffList.IsReuseNotChangeFileNames())
            {
                foreach (var removeFile in diffList.RemoveFiles)
                {
                    FileUtils.DeleteFileNoThrow(Path.Combine(destinationFolder, removeFile));
                }
                return InstallStatus.Success;
            }
            var movedReusedFiles = new Dictionary<string, string>();
            DirectoryInfo? backupDataDir = null;
            var dataPathDir = new DirectoryInfo(GameConstants.GetDataFolderPath(destinationFolder));
            try
            {
                foreach (var reusePair in diffList.ReuseFiles)
                {
                    var sourceReusePath = Path.Combine(destinationFolder, reusePair.Key);
                    var destReusePath = Path.Combine(sourceFolder, reusePair.Value);
                    Directory.CreateDirectory(Path.GetDirectoryName(destReusePath));
                    File.Move(sourceReusePath, destReusePath);
                    movedReusedFiles.Add(destReusePath, sourceReusePath);
                }
                if (dataPathDir.Exists)
                {
                    var backupDataDirPath = Path.Combine(destinationFolder, "backup_" + Path.GetRandomFileName());
                    Directory.Move(dataPathDir.FullName, backupDataDirPath);
                    backupDataDir = new DirectoryInfo(backupDataDirPath);
                }
                Directory.Move(GameConstants.GetDataFolderPath(sourceFolder), dataPathDir.FullName);
                if (backupDataDir != null)
                {
                    FileUtils.DeleteDirectoryNoThrow(backupDataDir, true);
                    backupDataDir = null;
                }
                movedReusedFiles.Clear();
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
                if (backupDataDir != null)
                {
                    RestoreDirectory(backupDataDir, dataPathDir);
                }
                foreach (var pair in movedReusedFiles)
                {
                    FileUtils.MoveFileNoThrow(pair.Key, pair.Value);
                }
            }
            return InstallStatus.Success;
        }

        public bool WriteTimestamp(DateTimeOffset date, string destinationFolder)
        {
            string timestampFile = Path.Combine(GameConstants.GetDataFolderPath(destinationFolder), "timestamp");
            if (!File.Exists(timestampFile))
            {
                try
                {
                    using var timespampWriter = File.CreateText(timestampFile);
                    timespampWriter.Write(DateTimeUtils.ToUnixTimeSeconds(date.UtcDateTime));
                    timespampWriter.Flush();
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"Failed write timestamp file: {timestampFile}");
                    return false;
                }
            }
            return true;
        }

        public UninstallStatus Uninstall(string destinationFolder)
        {
            if (!Directory.Exists(destinationFolder))
                return UninstallStatus.Failed;
            var result = UninstallStatus.Success;
            var dataPathDir = new DirectoryInfo(GameConstants.GetDataFolderPath(destinationFolder));
            if (dataPathDir.Exists && !FileUtils.DeleteDirectoryNoThrow(dataPathDir, true))
                result = UninstallStatus.Partial;
            return result;
        }

        public ISet<string>? GetLanguages(string destinationFolder)
        {
            if (!Directory.Exists(destinationFolder))
                return null;
            var localizationPath = GameConstants.GetLocalizationFolderPath(destinationFolder);
            if (!Directory.Exists(localizationPath))
                return null;
            var languages = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                var directoryInfo = new DirectoryInfo(localizationPath);
                foreach (var subDirInfo in directoryInfo.EnumerateDirectories())
                {
                    if (File.Exists(Path.Combine(subDirInfo.FullName, GameConstants.GlobalIniName)))
                    {
                        languages.Add(subDirInfo.Name);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Failed enum localization folders: {destinationFolder}");
            }
            return languages;
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
            var translationExtracted = false;
            var rootEntry = archive.Entries[0];
            var dataPathStart = GameConstants.DataFolderName + "/";
            //extract only data folder and core module
            foreach (var entry in archive.Entries)
            {
                if (entry.FullName.StartsWith(rootEntry.FullName, StringComparison.OrdinalIgnoreCase))
                {
                    var relativePath = entry.FullName.Substring(rootEntry.FullName.Length);
                    if (string.IsNullOrEmpty(entry.Name) && relativePath.EndsWith("/", StringComparison.Ordinal))
                    {
                        var dir = Path.Combine(destinationFolder, relativePath);
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);
                    }
                    else if (relativePath.StartsWith(dataPathStart, StringComparison.OrdinalIgnoreCase))
                    {
                        entry.ExtractToFile(Path.Combine(destinationFolder, relativePath), true);
                        dataExtracted = true;
                        if (entry.Name.Equals(GameConstants.GlobalIniName, StringComparison.OrdinalIgnoreCase))
                        {
                            entry.ExtractToFile(Path.Combine(destinationFolder, relativePath), true);
                            translationExtracted = true;
                        }
                    }
                }
            }
            if (!dataExtracted || !translationExtracted)
            {
                _logger.Error($"Wrong localization archive: hasData={dataExtracted}, hasTranslation={translationExtracted}");
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