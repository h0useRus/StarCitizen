using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using NSW.StarCitizen.Tools.Adapters;
using NSW.StarCitizen.Tools.Forms;
using NSW.StarCitizen.Tools.Lib.Global;
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
        public readonly GameInfo CurrentGame;
        public readonly GameSettings GameSettings;
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
            GameSettings.Load();
            Repositories = RepositoryManager.GetRepositoriesList();
            CurrentRepository = RepositoryManager.GetCurrentRepository(Repositories);
            CurrentInstallation = RepositoryManager.CreateRepositoryInstallation(CurrentRepository);
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

        public LocalizationInstallationType GetInstallationType() => CurrentRepository.Installer.GetInstallationType(CurrentGame.RootFolderPath);

        public async Task<bool> RefreshVersionsAsync(Control window)
        {
            _logger.Info($"Refresh localization versions: {CurrentRepository.RepositoryUrl}");
            bool status = false;
            using var progressDlg = new ProgressForm(10000);
            try
            {
                window.Enabled = false;
                Cursor.Current = Cursors.WaitCursor;
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
                        MessageBox.Show(window, Resources.Localization_Download_ErrorText + '\n' + e.Message,
                            Resources.Localization_Download_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(window, Resources.Localization_Download_ErrorText,
                            Resources.Localization_Download_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                window.Enabled = true;
                progressDlg.Hide();
            }
            return status;
        }

        public async Task<bool> InstallVersionAsync(Control window, UpdateInfo selectedUpdateInfo)
        {
            if (!CurrentGame.IsAvailable())
            {
                _logger.Error($"Install localization mode path unavailable: {CurrentGame.RootFolderPath}");
                MessageBox.Show(window, Resources.Localization_File_ErrorText,
                    Resources.Localization_File_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!Program.Settings.AcceptInstallWarning)
            {
                var dialogResult = MessageBox.Show(window, Resources.Localization_InstallWarning_Text,
                    Resources.Localization_InstallWarning_Title, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (dialogResult != DialogResult.Yes)
                {
                    return false;
                }
                Program.Settings.AcceptInstallWarning = true;
                Program.SaveAppSettings();
            }
            _logger.Info($"Install localization: {CurrentGame.Mode}, {selectedUpdateInfo.Dump()}");
            bool status = false;
            using var progressDlg = new ProgressForm();
            try
            {
                window.Enabled = false;
                Cursor.Current = Cursors.WaitCursor;
                var downloadDialogAdapter = new DownloadProgressDialogAdapter(selectedUpdateInfo.GetVersion());
                progressDlg.BindAdapter(downloadDialogAdapter);
                progressDlg.Show(window);
                var filePath = await CurrentRepository.DownloadAsync(selectedUpdateInfo, Path.GetTempPath(),
                    progressDlg.CancelToken, downloadDialogAdapter);
                progressDlg.BindAdapter(new InstallProgressDialogAdapter());
                var result = CurrentRepository.Installer.Install(filePath, CurrentGame.RootFolderPath);
                switch (result)
                {
                    case InstallStatus.Success:
                        GameSettings.Load();
                        progressDlg.CurrentTaskProgress = 1.0f;
                        RepositoryManager.SetInstalledRepository(CurrentRepository, selectedUpdateInfo.GetVersion());
                        status = true;
                        break;
                    case InstallStatus.PackageError:
                        _logger.Error($"Failed install localization due to package error: {CurrentGame.Mode}, {selectedUpdateInfo.Dump()}");
                        MessageBox.Show(progressDlg, Resources.Localization_Package_ErrorText,
                            Resources.Localization_Package_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case InstallStatus.VerifyError:
                        _logger.Error($"Failed install localization due to core verify error: {CurrentGame.Mode}, {selectedUpdateInfo.Dump()}");
                        MessageBox.Show(progressDlg, Resources.Localization_Verify_ErrorText,
                            Resources.Localization_Verify_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case InstallStatus.FileError:
                        _logger.Error($"Failed install localization due to file error: {CurrentGame.Mode}, {selectedUpdateInfo.Dump()}");
                        MessageBox.Show(progressDlg, Resources.Localization_File_ErrorText,
                            Resources.Localization_File_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    default:
                        _logger.Error($"Failed install localization: {CurrentGame.Mode}, {selectedUpdateInfo.Dump()}");
                        MessageBox.Show(progressDlg, Resources.Localization_Install_ErrorText,
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
                        MessageBox.Show(window, Resources.Localization_Download_ErrorText + '\n' + e.Message,
                            Resources.Localization_Download_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(window, Resources.Localization_Download_ErrorText,
                            Resources.Localization_Download_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                window.Enabled = true;
                progressDlg.Hide();
            }
            return status;
        }

        public bool Uninstall(Control window)
        {
            if (CurrentInstallation.InstalledVersion != null)
            {
                if (!CurrentGame.IsAvailable())
                {
                    _logger.Error($"Uninstall localization mode path unavailable: {CurrentGame.RootFolderPath}");
                    MessageBox.Show(window, Resources.Localization_Uninstall_ErrorText,
                        Resources.Localization_Uninstall_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                var dialogResult = MessageBox.Show(window, Resources.Localization_Uninstall_QuestionText,
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
                    switch (CurrentRepository.Installer.Uninstall(CurrentGame.RootFolderPath))
                    {
                        case UninstallStatus.Success:
                            GameSettings.RemoveCurrentLanguage();
                            GameSettings.Load();
                            progressDlg.CurrentTaskProgress = 1.0f;
                            RepositoryManager.RemoveInstalledRepository(CurrentRepository);
                            status = true;
                            break;
                        case UninstallStatus.Partial:
                            GameSettings.RemoveCurrentLanguage();
                            GameSettings.Load();
                            progressDlg.CurrentTaskProgress = 1.0f;
                            RepositoryManager.RemoveInstalledRepository(CurrentRepository);
                            status = true;
                            _logger.Warn($"Localization uninstalled partially: {CurrentGame.Mode}");
                            MessageBox.Show(progressDlg, Resources.Localization_Uninstall_WarningText,
                                    Resources.Localization_Uninstall_WarningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        default:
                            _logger.Error($"Failed uninstall localization: {CurrentGame.Mode}");
                            MessageBox.Show(progressDlg, Resources.Localization_Uninstall_ErrorText,
                                Resources.Localization_Uninstall_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"Error during uninstall localization: {CurrentGame.Mode}");
                    MessageBox.Show(window, Resources.Localization_Uninstall_ErrorText,
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

        public void ToggleLocalization(Control window)
        {
            try
            {
                window.Enabled = false;
                Cursor.Current = Cursors.WaitCursor;
                CurrentRepository.Installer.RevertLocalization(CurrentGame.RootFolderPath);
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error during toggle localization: {CurrentGame.Mode}");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                window.Enabled = true;
            }
        }
    }
}
