using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Adapters;
using NSW.StarCitizen.Tools.Global;
using NSW.StarCitizen.Tools.Localization;
using NSW.StarCitizen.Tools.Properties;
using NSW.StarCitizen.Tools.Settings;
using NSW.StarCitizen.Tools.Update;

namespace NSW.StarCitizen.Tools.Forms
{
    public partial class LocalizationForm : Form
    {
        private readonly GameInfo _currentGame;
        private readonly GameSettings _gameSettings;
        private ILocalizationRepository _currentRepository;
        private LocalizationInstallation _currentInstallation;

        public LocalizationForm(GameInfo currentGame)
        {
            _currentGame = currentGame;
            _gameSettings = new GameSettings(currentGame);
            InitializeComponent();
            InitializeLocalization();
        }

        private void InitializeLocalization()
        {
            Text = Resources.Localization_Title;
            btnManage.Text = Resources.Localization_Manage_Text;
            btnRefresh.Text = Resources.Localization_Refresh_Text;
            btnInstall.Text = Resources.Localization_InstallVersion_Text;
            btnUninstall.Text = Resources.Localization_UninstallLocalization_Text;
            lblCurrentVersion.Text = Resources.Localization_SourceRepository_Text;
            lblServerVersion.Text = Resources.Localization_AvailableVersions_Text;
            lblCurrentLanguage.Text = Resources.Localization_CurrentLanguage;
            lblMinutes.Text = Resources.Localization_AutomaticCheck_Measure;
            cbCheckNewVersions.Text = Resources.Localization_CheckForVersionEvery_Text;
        }

        private void LocalizationForm_Load(object sender, EventArgs e)
        {
            _gameSettings.Load();
            // Repositories
            var repositories = Program.RepositoryManager.GetRepositoriesList();
            var currentRepository = Program.RepositoryManager.GetInstalledRepository(_currentGame.Mode) ?? repositories.First();
            cbRepository.DataSource = repositories;
            cbRepository.SelectedItem = currentRepository;
            SetCurrentLocalizationRepository(currentRepository);
        }

        private void cbRepository_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbRepository.SelectedItem is ILocalizationRepository repository)
            {
                SetCurrentLocalizationRepository(repository);
            }
        }

        private void cbVersions_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbVersions.SelectedItem is UpdateInfo info)
            {
                btnInstall.Enabled = true;
                _currentRepository.SetCurrentVersion(info.GetVersion());
                UpdateButtonsVisibility();
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            using var progressDlg = new ProgressForm(10000);
            try
            {
                Enabled = false;
                Cursor.Current = Cursors.WaitCursor;
                progressDlg.Text = Resources.Localization_RefreshAvailableVersion_Title;
                progressDlg.UserCancelText = Resources.Localization_Stop_Text;
                progressDlg.Show(this);
                await _currentRepository.RefreshUpdatesAsync(progressDlg.CancelToken);
                progressDlg.CurrentTaskProgress = 1.0f;
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
                Enabled = true;
                progressDlg.Hide();
                UpdateAvailableVersions();
            }
        }

        private async void btnInstall_Click(object sender, EventArgs e)
        {
            if (cbVersions.SelectedItem is UpdateInfo selectedUpdateInfo)
            {
                using var progressDlg = new ProgressForm();
                try
                {
                    Enabled = false;
                    Cursor.Current = Cursors.WaitCursor;
                    progressDlg.Text = string.Format(Resources.Localization_InstallVersion_Title, selectedUpdateInfo.GetVersion());
                    var downloadDialogAdapter = new DownloadProgressDialogAdapter(progressDlg);
                    progressDlg.Show(this);
                    var filePath = await _currentRepository.DownloadAsync(selectedUpdateInfo, null,
                        progressDlg.CancelToken, downloadDialogAdapter);
                    var installDialogAdapter = new InstallProgressDialogAdapter(progressDlg);
                    var result = _currentRepository.Installer.Install(filePath, _currentGame.RootFolder.FullName);
                    switch (result)
                    {
                        case InstallStatus.Success:
                            _gameSettings.Load();
                            progressDlg.CurrentTaskProgress = 1.0f;
                            Program.RepositoryManager.SetInstalledRepository(_currentGame.Mode, _currentRepository);
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
                    Enabled = true;
                    progressDlg.Hide();
                    UpdateControls();
                }
            }
        }

        private void btnUninstall_Click(object sender, EventArgs e)
        {
            if (_currentInstallation.InstalledVersion != null)
            {
                var dialogResult = MessageBox.Show(Resources.Localization_Uninstall_QuestionText,
                    Resources.Localization_Uninstall_QuestionTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.No)
                    return;
                using var progressDlg = new ProgressForm();
                try
                {
                    progressDlg.Text = Resources.Localization_UninstallLocalization_Text;
                    var uninstallDialogAdapter = new UninstallProgressDialogAdapter(progressDlg);
                    progressDlg.Show(this);
                    switch (_currentRepository.Installer.Uninstall(_currentGame.RootFolder.FullName))
                    {
                        case UninstallStatus.Success:
                            _gameSettings.RemoveCurrentLanguage();
                            _gameSettings.Load();
                            progressDlg.CurrentTaskProgress = 1.0f;
                            Program.RepositoryManager.RemoveInstalledRepository(_currentGame.Mode, _currentRepository);
                            break;
                        case UninstallStatus.Partial:
                            _gameSettings.RemoveCurrentLanguage();
                            _gameSettings.Load();
                            progressDlg.CurrentTaskProgress = 1.0f;
                            Program.RepositoryManager.RemoveInstalledRepository(_currentGame.Mode, _currentRepository);
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
                    UpdateControls();
                    cbRepository.Refresh();
                }
            }
        }

        private void btnLocalizationDisable_Click(object sender, EventArgs e)
        {
            try
            {
                Enabled = false;
                Cursor.Current = Cursors.WaitCursor;
                _currentRepository.Installer.RevertLocalization(_currentGame.RootFolder.FullName);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                Enabled = true;
                UpdateControls();
            }
        }

        private void cbCheckNewVersions_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCheckNewVersions.Checked != _currentInstallation.MonitorForUpdates)
            {
                _currentInstallation.MonitorForUpdates = cbCheckNewVersions.Checked;
                Program.SaveAppSettings();
                Program.RepositoryManager.RunMonitors();
            }
        }

        private void cbRefreshTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRefreshTime.SelectedItem.ToString() != _currentInstallation.MonitorRefreshTime.ToString())
            {
                _currentInstallation.MonitorRefreshTime = int.Parse(cbRefreshTime.SelectedItem.ToString());
                Program.SaveAppSettings();
                Program.RepositoryManager.RunMonitors();
            }
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            using var dialog = new ManageRepositoriesForm();
            dialog.ShowDialog(this);
            LocalizationForm_Load(sender, e);
        }

        private void cbLanguages_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if ((cbLanguages.SelectedItem is string currentLanguage) && !_gameSettings.SaveCurrentLanguage(currentLanguage))
            {
                cbLanguages.SelectedItem = _gameSettings.LanguageInfo.Current;
                /// TODO: Add dialog with error
            }
        }

        private void cbRepository_DrawItem(object sender, DrawItemEventArgs e)
        {
            ILocalizationRepository repository = (ILocalizationRepository)cbRepository.Items[e.Index];
            var localizationInstallation = Program.RepositoryManager.GetRepositoryInstallation(_currentGame.Mode, repository);
            bool isInstalled = localizationInstallation != null && !string.IsNullOrEmpty(localizationInstallation.InstalledVersion);
            using var brush = new SolidBrush(isInstalled ? e.ForeColor : Color.Gray);
            using var font = new Font(cbRepository.Font, isInstalled ? FontStyle.Bold : FontStyle.Regular);
            e.DrawBackground();
            e.Graphics.DrawString(repository.Name, font, brush, e.Bounds);
            e.DrawFocusRectangle();
        }

        private void SetCurrentLocalizationRepository(ILocalizationRepository localizationRepository)
        {
            if (localizationRepository.Repository == _currentRepository?.Repository)
                return;
            _currentRepository = localizationRepository;
            _currentInstallation = Program.RepositoryManager.CreateRepositoryInstallation(_currentGame.Mode, localizationRepository);
            UpdateAvailableVersions();
            UpdateControls();
        }

        private void UpdateAvailableVersions()
        {
            var currentUpdateInfo = _currentRepository.UpdateCurrentVersion(
                 _currentInstallation.LastVersion ?? _currentInstallation.InstalledVersion);
            if (_currentRepository.UpdateReleases != null)
            {
                btnInstall.Enabled = currentUpdateInfo != null;
                cbVersions.DataSource = _currentRepository.UpdateReleases;
                cbVersions.SelectedItem = currentUpdateInfo;
            }
            else
            {
                btnInstall.Enabled = false;
                cbVersions.DataSource = new[] { Resources.Localization_Press_Refresh_Button };
            }
        }

        private void UpdateControls()
        {
            //Languages
            var installedVersion = _currentInstallation.InstalledVersion;
            if (!string.IsNullOrEmpty(installedVersion))
            {
                lblSelectedVersion.Text = Resources.Localization_Installed_Version;
                tbCurrentVersion.Text = installedVersion;
                if (_gameSettings.LanguageInfo.Languages.Any())
                {
                    cbLanguages.DataSource = _gameSettings.LanguageInfo.Languages.ToList();
                    cbLanguages.SelectedItem = _gameSettings.LanguageInfo.Current;
                    cbLanguages.Enabled = true;
                }
                else
                {
                    cbLanguages.Enabled = false;
                }
                lblCurrentLanguage.Visible = true;
                cbLanguages.Visible = true;
            }
            else
            {
                var lastVersion = _currentInstallation.LastVersion;
                if (!string.IsNullOrEmpty(lastVersion))
                {
                    lblSelectedVersion.Text = Resources.Localization_Latest_Version;
                    tbCurrentVersion.Text = lastVersion;
                }
                else
                {
                    lblSelectedVersion.Text = Resources.Localization_Installed_Version;
                    tbCurrentVersion.Text = "N/A";
                }
                lblCurrentLanguage.Visible = false;
                cbLanguages.Visible = false;
            }
            // enable disable
            switch (_currentRepository.Installer.GetInstallationType(_currentGame.RootFolder.FullName))
            {
                case LocalizationInstallationType.None:
                    btnLocalizationDisable.Visible = false;
                    break;
                case LocalizationInstallationType.Enabled:
                    btnLocalizationDisable.Visible = !string.IsNullOrEmpty(installedVersion);
                    btnLocalizationDisable.Text = Resources.Localization_Button_Disable_localization;
                    break;
                case LocalizationInstallationType.Disabled:
                    btnLocalizationDisable.Visible = !string.IsNullOrEmpty(installedVersion);
                    btnLocalizationDisable.Text = Resources.Localization_Button_Enable_localization;
                    break;
            }
            // monitoring
            cbCheckNewVersions.Checked = _currentInstallation.MonitorForUpdates;
            cbRefreshTime.SelectedItem = _currentInstallation.MonitorRefreshTime.ToString();
            UpdateButtonsVisibility();
        }

        private void UpdateButtonsVisibility()
        {
            if (_currentInstallation.InstalledVersion != null && _currentRepository.CurrentVersion != null &&
                string.Compare(_currentInstallation.InstalledVersion, _currentRepository.CurrentVersion, StringComparison.OrdinalIgnoreCase) == 0)
            {
                btnInstall.Visible = false;
                btnUninstall.Visible = true;
            }
            else
            {
                btnInstall.Visible = true;
                btnUninstall.Visible = false;
            }
        }
    }
}
