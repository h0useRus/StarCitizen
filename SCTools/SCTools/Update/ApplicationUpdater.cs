using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Helpers;
using NSW.StarCitizen.Tools.Properties;

namespace NSW.StarCitizen.Tools.Update
{
    public enum InstallUpdateStatus
    {
        Success = 0,                // update script: successfully apply update
        CopyFilesError = 1,         // update script: unable copy update files
        ExtractFilesError = 2,      // application: can't extract update package and script
        LaunchScriptError = 3       // application: unable launch update install script
    }

    public class ApplicationUpdater : IDisposable
    {
        private static readonly string _executablePath = Path.GetDirectoryName(Application.ExecutablePath);
        private static readonly string _updateScriptPath = Path.Combine(_executablePath, "update.bat");
        private static readonly string _updatesStoragePath = Path.Combine(_executablePath, "updates");
        private static readonly string _schedInstallArchivePath = Path.Combine(_updatesStoragePath, "latest.zip");
        private static readonly string _schedInstallJsonPath = Path.Combine(_updatesStoragePath, "latest.json");
        private static readonly string _installUnpackedDir = Path.Combine(_updatesStoragePath, "latest");

        private readonly IUpdateRepository _updateRepository;

        public event EventHandler<Tuple<string, string>>? Notification;

        public ApplicationUpdater()
        {
            var updateInfoFactory = GitHubUpdateInfo.Factory.NewWithVersionByTagName();
            _updateRepository = new GitHubUpdateRepository(GitHubDownloadType.Assets, updateInfoFactory, Program.Name, "h0useRus/StarCitizen");
            _updateRepository.SetCurrentVersion(Program.Version.ToString(3));
            _updateRepository.MonitorStarted += OnMonitorStarted;
            _updateRepository.MonitorStopped += OnMonitorStopped;
            _updateRepository.MonitorNewVersion += OnMonitorNewVersion;
        }

        public void Dispose()
        {
            _updateRepository.Dispose();
        }

        public void MonitorStart(int refreshTime) => _updateRepository.MonitorStart(refreshTime);

        public void MonitorStop() => _updateRepository.MonitorStop();

        public async Task<UpdateInfo?> CheckForUpdateVersionAsync(CancellationToken cancellationToken)
        {
            var latestUpdateInfo = await _updateRepository.GetLatestAsync(cancellationToken);
            if (latestUpdateInfo != null && string.Compare(latestUpdateInfo.GetVersion(),
                Program.Version.ToString(3), StringComparison.OrdinalIgnoreCase) != 0)
            {
                return latestUpdateInfo;
            }
            return null;
        }

        public async Task<string> DownloadVersionAsync(UpdateInfo version, CancellationToken cancellationToken, IDownloadProgress downloadProgress)
        {
            if (!Directory.Exists(_updatesStoragePath))
            {
                Directory.CreateDirectory(_updatesStoragePath);
            }
            return await _updateRepository.DownloadAsync(version, _updatesStoragePath, cancellationToken, downloadProgress);
        }

        public InstallUpdateStatus InstallScheduledUpdate()
        {
            if (ExtractReadyInstallUpdate() && ExtractUpdateScript())
            {
                using var updateProcess = new Process();
                updateProcess.StartInfo.UseShellExecute = false;
                updateProcess.StartInfo.RedirectStandardInput = false;
                updateProcess.StartInfo.RedirectStandardOutput = false;
                updateProcess.StartInfo.RedirectStandardError = false;
                updateProcess.StartInfo.ErrorDialog = false;
                updateProcess.StartInfo.CreateNoWindow = true;
                updateProcess.StartInfo.WorkingDirectory = _executablePath;
                updateProcess.StartInfo.FileName = _updateScriptPath;
                if (!updateProcess.Start())
                {
                    RemoveUpdateScript();
                    return InstallUpdateStatus.LaunchScriptError;
                }
                return InstallUpdateStatus.Success;
            }
            CancelScheduleInstallUpdate();
            return InstallUpdateStatus.ExtractFilesError;
        }

        public UpdateInfo? GetScheduledUpdateInfo()
        {
            if (File.Exists(_schedInstallArchivePath))
                return JsonHelper.ReadFile<GitHubUpdateInfo>(_schedInstallJsonPath);
            return null;
        }

        public bool IsAlreadyInstalledVersion(UpdateInfo updateInfo)
        {
            return string.Compare(updateInfo.GetVersion(), Program.Version.ToString(3), StringComparison.OrdinalIgnoreCase) == 0;
        }

        public void ApplyScheduledUpdateProps(UpdateInfo updateInfo)
        {
            _updateRepository.SetCurrentVersion(updateInfo.GetVersion());
        }

        public bool ScheduleInstallUpdate(UpdateInfo updateInfo, string filePath)
        {
            if (File.Exists(filePath))
            {
                _updateRepository.SetCurrentVersion(Program.Version.ToString(3));
                try
                {
                    if (!Directory.Exists(_updatesStoragePath))
                    {
                        Directory.CreateDirectory(_updatesStoragePath);
                    }
                    if (File.Exists(_schedInstallArchivePath))
                    {
                        File.Delete(_schedInstallArchivePath);
                    }
                    File.Move(filePath, _schedInstallArchivePath);
                    if (JsonHelper.WriteFile(_schedInstallJsonPath, updateInfo))
                    {
                        _updateRepository.SetCurrentVersion(updateInfo.GetVersion());
                        return true;
                    }
                }
                catch
                {
                    CancelScheduleInstallUpdate();
                    return false;
                }
            }
            return false;
        }

        public bool CancelScheduleInstallUpdate()
        {
            _updateRepository.SetCurrentVersion(Program.Version.ToString(3));
            if (File.Exists(_schedInstallJsonPath))
                FileUtils.DeleteFileNoThrow(_schedInstallJsonPath);
            return File.Exists(_schedInstallArchivePath) &&
                FileUtils.DeleteFileNoThrow(_schedInstallArchivePath);
        }

        public void RemoveUpdateScript()
        {
            if (File.Exists(_updateScriptPath))
            {
                FileUtils.DeleteFileNoThrow(_updateScriptPath);
            }
        }

        private bool ExtractUpdateScript()
        {
            try
            {
                File.WriteAllText(_updateScriptPath, Resources.UpdateScript);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool ExtractReadyInstallUpdate()
        {
            var installUnpackedDir = new DirectoryInfo(_installUnpackedDir);
            var extractTempDir = new DirectoryInfo(Path.Combine(_updatesStoragePath, "temp_" + Path.GetRandomFileName()));
            try
            {
                if (installUnpackedDir.Exists && !FileUtils.DeleteDirectoryNoThrow(installUnpackedDir, true))
                    return false;
                using var archive = ZipFile.OpenRead(_schedInstallArchivePath);
                extractTempDir.Create();
                archive.ExtractToDirectory(extractTempDir.FullName);
                if (!File.Exists(Path.Combine(extractTempDir.FullName, "SCTools.exe")))
                    throw new NotSupportedException("Not supported upgrade package");
                Directory.Move(extractTempDir.FullName, _installUnpackedDir);
            }
            catch
            {
                if (extractTempDir.Exists)
                    FileUtils.DeleteDirectoryNoThrow(extractTempDir, true);
                if (installUnpackedDir.Exists)
                    FileUtils.DeleteDirectoryNoThrow(installUnpackedDir, true);
                return false;
            }
            return true;
        }

        private void OnMonitorStarted(object sender, string e)
        {
            Notification?.Invoke(sender, new Tuple<string, string>(_updateRepository.Name,
                Resources.Localization_Start_Monitoring));
        }

        private void OnMonitorStopped(object sender, string e)
        {
            Notification?.Invoke(sender, new Tuple<string, string>(_updateRepository.Name,
                Resources.Localization_Stop_Monitoring));
        }

        private void OnMonitorNewVersion(object sender, string e)
        {
            Notification?.Invoke(sender, new Tuple<string, string>(_updateRepository.Name,
                string.Format(Resources.Localization_Found_New_Version, e)));
        }
    }
}
