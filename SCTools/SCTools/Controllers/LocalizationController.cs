using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Adapters;
using NSW.StarCitizen.Tools.Forms;
using NSW.StarCitizen.Tools.Global;
using NSW.StarCitizen.Tools.Localization;
using NSW.StarCitizen.Tools.Properties;
using NSW.StarCitizen.Tools.Settings;
using NSW.StarCitizen.Tools.Update;

namespace NSW.StarCitizen.Tools.Controllers
{
    public sealed class LocalizationController
    {
        public readonly GameInfo CurrentGame;
        public readonly GameSettings GameSettings;
        public List<ILocalizationRepository> Repositories { get; private set; } = new List<ILocalizationRepository>();
        public LocalizationInstallation CurrentInstallation { get; private set; }
        public ILocalizationRepository CurrentRepository { get; private set; }

        public LocalizationController(GameInfo currentGame)
        {
            CurrentGame = currentGame;
            GameSettings = new GameSettings(currentGame);
            CurrentRepository = Program.RepositoryManager.GetCurrentRepository(currentGame.Mode);
            CurrentInstallation = Program.RepositoryManager.CreateRepositoryInstallation(currentGame.Mode, CurrentRepository);
        }

        public void Load()
        {
            GameSettings.Load();
            Repositories = Program.RepositoryManager.GetRepositoriesList();
            CurrentRepository = Program.RepositoryManager.GetCurrentRepository(CurrentGame.Mode, Repositories);
            CurrentInstallation = Program.RepositoryManager.CreateRepositoryInstallation(CurrentGame.Mode, CurrentRepository);
        }

        public bool SetCurrentRepository(ILocalizationRepository localizationRepository)
        {
            if (localizationRepository.Repository == CurrentRepository?.Repository)
                return false;
            CurrentRepository = localizationRepository;
            CurrentInstallation = Program.RepositoryManager.CreateRepositoryInstallation(CurrentGame.Mode, localizationRepository);
            return true;
        }

        public bool IsRepositoryInstalled(ILocalizationRepository localizationRepository)
        {
            var installation = Program.RepositoryManager.GetRepositoryInstallation(CurrentGame.Mode, localizationRepository);
            return installation != null && !string.IsNullOrEmpty(installation.InstalledVersion);
        }

        public UpdateInfo? UpdateCurrentVersion() => CurrentRepository.UpdateCurrentVersion(CurrentInstallation.LastVersion ?? CurrentInstallation.InstalledVersion);

        public LocalizationInstallationType GetInstallationType() => CurrentRepository.Installer.GetInstallationType(CurrentGame.RootFolderPath);

        public async Task<bool> RefreshVersionsAsync(Control window)
        {
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
            catch
            {
                if (!progressDlg.IsCanceledByUser)
                {
                    MessageBox.Show(Resources.Localization_Download_ErrorText,
                        Resources.Localization_Download_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(Resources.Localization_File_ErrorText,
                    Resources.Localization_File_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            bool status = false;
            using var progressDlg = new ProgressForm();
            try
            {
                window.Enabled = false;
                Cursor.Current = Cursors.WaitCursor;
                progressDlg.Text = string.Format(Resources.Localization_InstallVersion_Title, selectedUpdateInfo.GetVersion());
                var downloadDialogAdapter = new DownloadProgressDialogAdapter(progressDlg);
                progressDlg.Show(window);
                var filePath = await CurrentRepository.DownloadAsync(selectedUpdateInfo, null,
                    progressDlg.CancelToken, downloadDialogAdapter);
                var installDialogAdapter = new InstallProgressDialogAdapter(progressDlg);
                var result = CurrentRepository.Installer.Install(filePath, CurrentGame.RootFolderPath);
                switch (result)
                {
                    case InstallStatus.Success:
                        GameSettings.Load();
                        progressDlg.CurrentTaskProgress = 1.0f;
                        Program.RepositoryManager.SetInstalledRepository(CurrentGame.Mode, CurrentRepository, selectedUpdateInfo.GetVersion());
                        status = true;
                        break;
                    case InstallStatus.PackageError:
                        MessageBox.Show(Resources.Localization_Package_ErrorText,
                            Resources.Localization_Package_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case InstallStatus.VerifyError:
                        MessageBox.Show(Resources.Localization_Verify_ErrorText,
                            Resources.Localization_Verify_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case InstallStatus.FileError:
                        MessageBox.Show(Resources.Localization_File_ErrorText,
                            Resources.Localization_File_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case InstallStatus.UnknownError:
                    default:
                        MessageBox.Show(Resources.Localization_Install_ErrorText,
                            Resources.Localization_Install_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            catch
            {
                if (!progressDlg.IsCanceledByUser)
                {
                    MessageBox.Show(Resources.Localization_Download_ErrorText,
                        Resources.Localization_Download_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show(Resources.Localization_Uninstall_ErrorText,
                        Resources.Localization_Uninstall_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                var dialogResult = MessageBox.Show(Resources.Localization_Uninstall_QuestionText,
                    Resources.Localization_Uninstall_QuestionTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.No)
                    return false;
                bool status = false;
                using var progressDlg = new ProgressForm();
                try
                {
                    progressDlg.Text = Resources.Localization_UninstallLocalization_Text;
                    var uninstallDialogAdapter = new UninstallProgressDialogAdapter(progressDlg);
                    progressDlg.Show(window);
                    switch (CurrentRepository.Installer.Uninstall(CurrentGame.RootFolderPath))
                    {
                        case UninstallStatus.Success:
                            GameSettings.RemoveCurrentLanguage();
                            GameSettings.Load();
                            progressDlg.CurrentTaskProgress = 1.0f;
                            Program.RepositoryManager.RemoveInstalledRepository(CurrentGame.Mode, CurrentRepository);
                            status = true;
                            break;
                        case UninstallStatus.Partial:
                            GameSettings.RemoveCurrentLanguage();
                            GameSettings.Load();
                            progressDlg.CurrentTaskProgress = 1.0f;
                            Program.RepositoryManager.RemoveInstalledRepository(CurrentGame.Mode, CurrentRepository);
                            status = true;
                            MessageBox.Show(Resources.Localization_Uninstall_WarningText,
                                    Resources.Localization_Uninstall_WarningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        case UninstallStatus.Failed:
                        default:
                            MessageBox.Show(Resources.Localization_Uninstall_ErrorText,
                                Resources.Localization_Uninstall_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
                catch
                {
                    MessageBox.Show(Resources.Localization_Uninstall_ErrorText,
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
            finally
            {
                Cursor.Current = Cursors.Default;
                window.Enabled = true;
            }
        }
    }
}