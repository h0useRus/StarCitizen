using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using NSW.StarCitizen.Tools.Adapters;
using NSW.StarCitizen.Tools.Forms;
using NSW.StarCitizen.Tools.Helpers;
using NSW.StarCitizen.Tools.Lib.Global;
using NSW.StarCitizen.Tools.Lib.Helpers;
using NSW.StarCitizen.Tools.Lib.Localization;
using NSW.StarCitizen.Tools.Lib.Update;
using NSW.StarCitizen.Tools.Properties;
using NSW.StarCitizen.Tools.Repository;
using NSW.StarCitizen.Tools.Settings;

namespace NSW.StarCitizen.Tools.Controllers
{
    public sealed class LocalizationController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public GameInfo CurrentGame { get; }
        public GameSettings GameSettings { get; }
        public RepositoryManager RepositoryManager { get; }
        public List<ILocalizationRepository> Repositories { get; private set; }
        public LocalizationInstallation CurrentInstallation { get; private set; }
        public ILocalizationRepository CurrentRepository { get; private set; }
        public bool IsLoaded { get; private set; }

        public LocalizationController(GameInfo currentGame)
        {
            CurrentGame = currentGame;
            GameSettings = new GameSettings(currentGame);
            RepositoryManager = Program.RepositoryManagers[currentGame.Mode];
            Repositories = RepositoryManager.GetRepositoriesList();
            CurrentRepository = RepositoryManager.GetCurrentRepository(Repositories);
            CurrentInstallation = RepositoryManager.CreateRepositoryInstallation(CurrentRepository);
        }

        public void Load()
        {
            Repositories = RepositoryManager.GetRepositoriesList();
            CurrentRepository = RepositoryManager.GetCurrentRepository(Repositories);
            CurrentInstallation = RepositoryManager.CreateRepositoryInstallation(CurrentRepository);
            GameSettings.Load(CurrentRepository.Installer.GetLanguages(CurrentGame.RootFolderPath));
            IsLoaded = true;
        }

        public bool SetCurrentRepository(ILocalizationRepository localizationRepository)
        {
            if (localizationRepository.Type == CurrentRepository.Type &&
                localizationRepository.Repository == CurrentRepository.Repository)
                return false;
            CurrentRepository = localizationRepository;
            CurrentInstallation = RepositoryManager.CreateRepositoryInstallation(localizationRepository);
            return true;
        }

        public bool IsRepositoryInstalled(ILocalizationRepository localizationRepository)
        {
            var installation = RepositoryManager.GetRepositoryInstallation(localizationRepository);
            return installation != null && !string.IsNullOrEmpty(installation.InstalledVersion);
        }

        public UpdateInfo? UpdateCurrentVersion() => CurrentRepository.UpdateCurrentVersion(CurrentInstallation.LastVersion ?? CurrentInstallation.InstalledVersion);

        public void UpdateCurrentVersionToLatest()
        {
            var latestUpdateInfo = CurrentRepository.LatestUpdateInfo;
            if (latestUpdateInfo != null)
            {
                CurrentRepository.SetCurrentVersion(latestUpdateInfo.GetVersion());
            }
        }

        public async Task<bool> RefreshVersionsAsync(IWin32Window window)
        {
            _logger.Info($"Refresh localization versions: {CurrentRepository.RepositoryUrl}");
            bool status = false;
            using var progressDlg = new ProgressForm(30000);
            try
            {
                progressDlg.Text = Resources.Localization_RefreshAvailableVersion_Title;
                progressDlg.UserCancelText = Resources.Localization_Stop_Text;
                progressDlg.Show(window);
                await CurrentRepository.RefreshUpdatesAsync(progressDlg.CancelToken);
                progressDlg.CurrentTaskProgress = 1.0f;
                status = true;
            }
            catch (Exception e)
            {
                if (!progressDlg.IsCanceledByUser)
                {
                    _logger.Error(e, "Error during refresh localization versions");
                    if (e is HttpRequestException)
                    {
                        RtlAwareMessageBox.Show(window, Resources.Localization_Download_ErrorText + '\n' + e.Message,
                            Resources.Localization_Download_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        RtlAwareMessageBox.Show(window, Resources.Localization_Download_ErrorText,
                            Resources.Localization_Download_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            finally
            {
                progressDlg.Hide();
            }
            return status;
        }

        public async Task<bool> InstallVersionAsync(IWin32Window window, UpdateInfo selectedUpdateInfo)
        {
            if (!CurrentGame.IsAvailable())
            {
                _logger.Error($"Install localization mode path unavailable: {CurrentGame.RootFolderPath}");
                RtlAwareMessageBox.Show(window, Resources.Localization_File_ErrorText,
                    Resources.Localization_File_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            _logger.Info($"Install localization: {CurrentGame.Mode}, {selectedUpdateInfo.Dump()}");
            bool status = false;
            DirectoryInfo? downloadDirInfo = null;
            using var progressDlg = new ProgressForm();
            try
            {
                var downloadDialogAdapter = new DownloadProgressDialogAdapter(selectedUpdateInfo.GetVersion());
                progressDlg.BindAdapter(downloadDialogAdapter);
                progressDlg.Show(window);
                if (!(selectedUpdateInfo is FolderUpdateInfo))
                {
                    downloadDirInfo = Directory.CreateDirectory(Path.Combine(CurrentGame.RootFolderPath, "download_" + Path.GetRandomFileName()));
                }
                var packageIndex = new LocalizationPackageIndex(CurrentGame.RootFolderPath);
                var downloadResult = await CurrentRepository.DownloadAsync(selectedUpdateInfo,
                    downloadDirInfo != null ? downloadDirInfo.FullName : string.Empty, packageIndex,
                    progressDlg.CancelToken, downloadDialogAdapter);
                progressDlg.BindAdapter(new InstallProgressDialogAdapter());
                using var gameMutex = new GameMutex();
                if (!GameMutexController.AcquireWithRetryDialog(progressDlg, gameMutex))
                {
                    _logger.Info($"Install localization aborted by user because game running");
                    return false;
                }
                var installStatus = downloadResult switch
                {
                    FullDownoadResult fullResult => CurrentRepository.Installer.Install(fullResult.ArchiveFilePath, CurrentGame.RootFolderPath),
                    IncrementalDownloadResult incrementalResult => CurrentRepository.Installer.Install(incrementalResult.DownloadPath, CurrentGame.RootFolderPath, incrementalResult.DiffList),
                    _ => throw new InvalidOperationException("Download result is empty")
                };
                switch (installStatus)
                {
                    case InstallStatus.Success:
                        if (!(selectedUpdateInfo is FolderUpdateInfo))
                        {
                            CurrentRepository.Installer.WriteTimestamp(selectedUpdateInfo.Released, CurrentGame.RootFolderPath);
                        }
                        GameSettings.Load(CurrentRepository.Installer.GetLanguages(CurrentGame.RootFolderPath));
                        gameMutex.Release();
                        progressDlg.CurrentTaskProgress = 1.0f;
                        RepositoryManager.SetInstalledRepository(CurrentRepository, selectedUpdateInfo.GetVersion());
                        status = true;
                        break;
                    case InstallStatus.PackageError:
                        gameMutex.Release();
                        _logger.Error($"Failed install localization due to package error: {CurrentGame.Mode}, {selectedUpdateInfo.Dump()}");
                        RtlAwareMessageBox.Show(progressDlg, Resources.Localization_Package_ErrorText,
                            Resources.Localization_Package_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case InstallStatus.VerifyError:
                        gameMutex.Release();
                        _logger.Error($"Failed install localization due to core verify error: {CurrentGame.Mode}, {selectedUpdateInfo.Dump()}");
                        RtlAwareMessageBox.Show(progressDlg, Resources.Localization_Verify_ErrorText,
                            Resources.Localization_Verify_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case InstallStatus.FileError:
                        gameMutex.Release();
                        _logger.Error($"Failed install localization due to file error: {CurrentGame.Mode}, {selectedUpdateInfo.Dump()}");
                        RtlAwareMessageBox.Show(progressDlg, Resources.Localization_File_ErrorText,
                            Resources.Localization_File_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    default:
                        gameMutex.Release();
                        _logger.Error($"Failed install localization: {CurrentGame.Mode}, {selectedUpdateInfo.Dump()}");
                        RtlAwareMessageBox.Show(progressDlg, Resources.Localization_Install_ErrorText,
                            Resources.Localization_Install_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            catch (Exception e)
            {
                if (!progressDlg.IsCanceledByUser)
                {
                    _logger.Error(e, $"Error during install localization: {CurrentGame.Mode}, {selectedUpdateInfo.Dump()}");
                    if (e is HttpRequestException)
                    {
                        RtlAwareMessageBox.Show(window, Resources.Localization_Download_ErrorText + '\n' + e.Message,
                            Resources.Localization_Download_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        RtlAwareMessageBox.Show(window, Resources.Localization_Download_ErrorText,
                            Resources.Localization_Download_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            finally
            {
                progressDlg.Hide();
                if (downloadDirInfo != null && downloadDirInfo.Exists &&
                    !FileUtils.DeleteDirectoryNoThrow(downloadDirInfo, true))
                {
                    _logger.Warn($"Failed remove download directory: {downloadDirInfo.FullName}");
                }
            }
            return status;
        }

        public bool Uninstall(IWin32Window window)
        {
            if (CurrentInstallation.InstalledVersion != null)
            {
                if (!CurrentGame.IsAvailable())
                {
                    _logger.Error($"Uninstall localization mode path unavailable: {CurrentGame.RootFolderPath}");
                    RtlAwareMessageBox.Show(window, Resources.Localization_Uninstall_ErrorText,
                        Resources.Localization_Uninstall_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                var dialogResult = RtlAwareMessageBox.Show(window, Resources.Localization_Uninstall_QuestionText,
                    Resources.Localization_Uninstall_QuestionTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.No)
                    return false;
                _logger.Info($"Uninstall localization: {CurrentGame.Mode}, {CurrentInstallation.Repository} {CurrentInstallation.InstalledVersion}");
                bool status = false;
                using var progressDlg = new ProgressForm();
                try
                {
                    progressDlg.BindAdapter(new UninstallProgressDialogAdapter());
                    progressDlg.Show(window);
                    using var gameMutex = new GameMutex();
                    if (!GameMutexController.AcquireWithRetryDialog(progressDlg, gameMutex))
                    {
                        _logger.Info($"Uninstall localization aborted by user because game running");
                        return false;
                    }
                    switch (CurrentRepository.Installer.Uninstall(CurrentGame.RootFolderPath))
                    {
                        case UninstallStatus.Success:
                            GameSettings.RemoveCurrentLanguage();
                            GameSettings.Load(null);
                            gameMutex.Release();
                            progressDlg.CurrentTaskProgress = 1.0f;
                            RepositoryManager.RemoveInstalledRepository(CurrentRepository);
                            status = true;
                            break;
                        case UninstallStatus.Partial:
                            GameSettings.RemoveCurrentLanguage();
                            GameSettings.Load(null);
                            gameMutex.Release();
                            progressDlg.CurrentTaskProgress = 1.0f;
                            RepositoryManager.RemoveInstalledRepository(CurrentRepository);
                            status = true;
                            _logger.Warn($"Localization uninstalled partially: {CurrentGame.Mode}");
                            RtlAwareMessageBox.Show(progressDlg, Resources.Localization_Uninstall_WarningText,
                                    Resources.Localization_Uninstall_WarningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        default:
                            gameMutex.Release();
                            _logger.Error($"Failed uninstall localization: {CurrentGame.Mode}");
                            RtlAwareMessageBox.Show(progressDlg, Resources.Localization_Uninstall_ErrorText,
                                Resources.Localization_Uninstall_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"Error during uninstall localization: {CurrentGame.Mode}");
                    RtlAwareMessageBox.Show(window, Resources.Localization_Uninstall_ErrorText,
                        Resources.Localization_Uninstall_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    progressDlg.Hide();
                }
                return status;
            }
            return true;
        }
    }
}
