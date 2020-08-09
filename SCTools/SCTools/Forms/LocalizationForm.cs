using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Adapters;
using NSW.StarCitizen.Tools.Global;
using NSW.StarCitizen.Tools.Localization;
using NSW.StarCitizen.Tools.Properties;


namespace NSW.StarCitizen.Tools.Forms
{
    public partial class LocalizationForm : Form
    {
        public readonly GameInfo _currentGame;
        public readonly GameSettings _gameSettings;

        public LocalizationForm(GameInfo currentGame)
        {
            _currentGame = currentGame;
            _gameSettings = new GameSettings(currentGame);
            InitializeComponent();
        }

        private void LocalizationForm_Load(object sender, EventArgs e)
        {
            _gameSettings.Load();
            // Repositories
            cbRepository.DataSource = Program.LocalizationRepositories.Values.ToList();
            var current = Program.GetCurrentLocalizationRepository();
            if (current != null) cbRepository.SelectedItem = current;
        }

        private void cbRepository_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox cb)
            {
                Program.CurrentRepository = (ILocalizationRepository)cb.SelectedItem;
                cbVersions.DataSource = Program.CurrentRepository.Versions ?? new[] { LocalizationInfo.Empty };
            }
            UpdateControls();
        }

        private void UpdateControls()
        {
            //Languages
            var installedVersion = Program.CurrentInstallation.InstalledVersion;
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
                var lastVersion = Program.CurrentInstallation.LastVersion;
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
            switch (Program.CurrentRepository.Installer.GetInstallationType(_currentGame.RootFolder.FullName))
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
            cbCheckNewVersions.Checked = Program.CurrentInstallation.MonitorForUpdates;
            cbRefreshTime.SelectedItem = Program.CurrentInstallation.MonitorRefreshTime.ToString();
        }

        private void cbVersions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox cb)
            {
                if (cb.SelectedItem is LocalizationInfo info)
                {
                    btnInstall.Enabled = info.Actual;
                    if(info.Actual)
                        Program.CurrentRepository.CurrentVersion = info;
                }
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            if (Program.CurrentRepository != null)
            {
                using var progressDlg = new ProgressForm(10000);
                try
                {
                    Enabled = false;
                    Cursor.Current = Cursors.WaitCursor;
                    progressDlg.Text = "Refresh available versions";
                    progressDlg.Show(this);
                    await Program.CurrentRepository.RefreshVersionsAsync(progressDlg.CancelToken);
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
                    cbVersions.DataSource = Program.CurrentRepository.Versions ?? new[] { LocalizationInfo.Empty };
                }
            }
        }

        private async void btnInstall_Click(object sender, EventArgs e)
        {
            if (Program.CurrentRepository?.CurrentVersion != null)
            {
                using var progressDlg = new ProgressForm();
                try
                {
                    Enabled = false;
                    Cursor.Current = Cursors.WaitCursor;
                    var installRepository = Program.CurrentRepository;
                    progressDlg.Text = $"Install version: {installRepository.CurrentVersion}";
                    var downloadDialogAdapter = new DownloadProgressDialogAdapter(progressDlg);
                    progressDlg.Show(this);
                    var filePath = await installRepository.DownloadAsync(installRepository.CurrentVersion, progressDlg.CancelToken, downloadDialogAdapter);
                    var installDialogAdapter = new InstallProgressDialogAdapter(progressDlg);
                    var result = installRepository.Installer.Install(filePath, _currentGame.RootFolder.FullName);
                    switch (result)
                    {
                        case InstallStatus.Success:
                            _gameSettings.Load();
                            progressDlg.CurrentTaskProgress = 1.0f;
                            Program.UpdateCurrentInstallationRepository(installRepository);
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

        private void btnLocalizationDisable_Click(object sender, EventArgs e)
        {
            try
            {
                Enabled = false;
                Cursor.Current = Cursors.WaitCursor;
                Program.CurrentRepository.Installer.RevertLocalization(_currentGame.RootFolder.FullName);
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
            if (cbCheckNewVersions.Checked != Program.CurrentInstallation.MonitorForUpdates)
            {
                Program.CurrentInstallation.MonitorForUpdates = cbCheckNewVersions.Checked;
                Program.SaveAppSettings();
                Program.RunMonitors();
            }
        }

        private void cbRefreshTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRefreshTime.SelectedItem.ToString() != Program.CurrentInstallation.MonitorRefreshTime.ToString())
            {
                Program.CurrentInstallation.MonitorRefreshTime = int.Parse(cbRefreshTime.SelectedItem.ToString());
                Program.SaveAppSettings();
                Program.RunMonitors();
            }
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            var dlg = new ManageRepositoriesForm();
            dlg.ShowDialog(this);
            LocalizationForm_Load(sender, e);
        }

        private void cbLanguages_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var currentLanguage = cbLanguages.SelectedItem;
            if (currentLanguage != null && !_gameSettings.SaveCurrentLanguage(currentLanguage.ToString()))
            {
                cbLanguages.SelectedItem = _gameSettings.LanguageInfo.Current;
                /// TODO: Add dialog with error
            }
        }

        private void cbRepository_DrawItem(object sender, DrawItemEventArgs e)
        {
            ILocalizationRepository repository = (ILocalizationRepository)cbRepository.Items[e.Index];
            var localizationInstallation = Program.GetLocalizationInstallationFromRepository(repository);
            bool isInstalled = localizationInstallation != null && !string.IsNullOrEmpty(localizationInstallation.InstalledVersion);
            using var brush = new SolidBrush(isInstalled ? e.ForeColor : Color.Gray);
            using var font = new Font(cbRepository.Font, isInstalled ? FontStyle.Bold : FontStyle.Regular);
            e.DrawBackground();
            e.Graphics.DrawString(repository.Name, font, brush, e.Bounds);
            e.DrawFocusRectangle();
        }
    }
}
