using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Controllers;
using NSW.StarCitizen.Tools.Helpers;
using NSW.StarCitizen.Tools.Lib.Global;
using NSW.StarCitizen.Tools.Lib.Localization;
using NSW.StarCitizen.Tools.Lib.Update;
using NSW.StarCitizen.Tools.Properties;

namespace NSW.StarCitizen.Tools.Forms
{
    public partial class LocalizationForm : Form, ILocalizedForm
    {
        private readonly LocalizationController _controller;

        public LocalizationForm(GameInfo currentGame)
        {
            _controller = new LocalizationController(currentGame);
            InitializeComponent();
            UpdateLocalizedControls();
        }

        public void UpdateLocalizedControls()
        {
            Text = Resources.Localization_Title + " - " + _controller.CurrentGame.Mode;
            btnManage.Text = Resources.Localization_Manage_Text;
            btnRefresh.Text = Resources.Localization_Refresh_Text;
            btnInstall.Text = Resources.Localization_InstallVersion_Text;
            btnUninstall.Text = Resources.Localization_UninstallLocalization_Text;
            lblCurrentVersion.Text = Resources.Localization_SourceRepository_Text;
            lblServerVersion.Text = Resources.Localization_AvailableVersions_Text;
            lblCurrentLanguage.Text = Resources.Localization_CurrentLanguage;
            cbAllowPreReleaseVersions.Text = Resources.Localization_DisplayPreReleases_Text;
            lblMinutes.Text = Resources.Localization_AutomaticCheck_Measure;
            cbCheckNewVersions.Text = Resources.Localization_CheckForVersionEvery_Text;
            if (_controller.IsLoaded)
            {
                UpdateAvailableVersions();
                UpdateControls();
            }
        }

        private void LocalizationForm_Load(object sender, EventArgs e)
        {
            _controller.Load();
            cbRepository.DataSource = _controller.Repositories;
            cbRepository.SelectedItem = _controller.CurrentRepository;
            UpdateAvailableVersions();
            UpdateControls();
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
                _controller.CurrentRepository.SetCurrentVersion(info.GetVersion());
                UpdateButtonsVisibility();
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await _controller.RefreshVersionsAsync(this);
            UpdateAvailableVersions();
            UpdateButtonsVisibility();
        }

        private async void btnInstall_Click(object sender, EventArgs e)
        {
            if (cbVersions.SelectedItem is UpdateInfo selectedUpdateInfo)
            {
                await _controller.InstallVersionAsync(this, selectedUpdateInfo);
                UpdateControls();
            }
        }

        private void btnUninstall_Click(object sender, EventArgs e)
        {
            _controller.Uninstall(this);
            UpdateControls();
            cbRepository.Refresh();
        }

        private void btnLocalizationDisable_Click(object sender, EventArgs e)
        {
            _controller.ToggleLocalization(this);
            UpdateControls();
        }

        private void cbAllowPreReleaseVersions_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAllowPreReleaseVersions.Checked != _controller.CurrentInstallation.AllowPreRelease)
            {
                _controller.CurrentRepository.AllowPreReleases = cbAllowPreReleaseVersions.Checked;
                _controller.CurrentInstallation.AllowPreRelease = cbAllowPreReleaseVersions.Checked;
                Program.SaveAppSettings();
            }
        }

        private void cbCheckNewVersions_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCheckNewVersions.Checked != _controller.CurrentInstallation.MonitorForUpdates)
            {
                _controller.CurrentInstallation.MonitorForUpdates = cbCheckNewVersions.Checked;
                Program.SaveAppSettings();
                _controller.RepositoryManager.RunMonitors();
            }
        }

        private void cbRefreshTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRefreshTime.SelectedItem.ToString() != _controller.CurrentInstallation.MonitorRefreshTime.ToString())
            {
                _controller.CurrentInstallation.MonitorRefreshTime = int.Parse(cbRefreshTime.SelectedItem.ToString());
                Program.SaveAppSettings();
                _controller.RepositoryManager.RunMonitors();
            }
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            using var dialog = new ManageRepositoriesForm(_controller.RepositoryManager);
            dialog.ShowDialog(this);
            LocalizationForm_Load(sender, e);
        }

        private void cbLanguages_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if ((cbLanguages.SelectedItem is string currentLanguage) && !_controller.GameSettings.SaveCurrentLanguage(currentLanguage))
            {
                cbLanguages.SelectedItem = _controller.GameSettings.LanguageInfo.Current;
                /// TODO: Add dialog with error
            }
        }

        private void cbRepository_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0 && cbRepository.Items[e.Index] is ILocalizationRepository drawRepository)
            {
                bool isInstalled = _controller.IsRepositoryInstalled(drawRepository);
                using var brush = new SolidBrush(isInstalled ? e.ForeColor : Color.Gray);
                using var font = new Font(e.Font, isInstalled ? FontStyle.Bold : FontStyle.Regular);
                e.DrawBackground();
                e.Graphics.DrawString(drawRepository.Name, font, brush, e.Bounds);
                e.DrawFocusRectangle();
            }
        }

        private void cbVersions_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                var fontStyle = FontStyle.Italic;
                if (cbVersions.Items[e.Index] is UpdateInfo drawUpdateInfo)
                {
                    fontStyle = drawUpdateInfo.PreRelease ? FontStyle.Regular : FontStyle.Bold;
                }
                using var brush = new SolidBrush(e.ForeColor);
                using var font = new Font(e.Font, fontStyle);
                e.DrawBackground();
                e.Graphics.DrawString(cbVersions.Items[e.Index].ToString(), font, brush, e.Bounds);
                e.DrawFocusRectangle();
            }
        }

        private void SetCurrentLocalizationRepository(ILocalizationRepository localizationRepository)
        {
            if (_controller.SetCurrentRepository(localizationRepository))
            {
                UpdateAvailableVersions();
                UpdateControls();
            }
        }

        private void UpdateAvailableVersions()
        {
            var currentUpdateInfo = _controller.UpdateCurrentVersion();
            if (_controller.CurrentRepository.UpdateReleases != null)
            {
                btnInstall.Enabled = currentUpdateInfo != null;
                cbVersions.DataSource = _controller.CurrentRepository.UpdateReleases;
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
            switch (_controller.GetInstallationType())
            {
                case LocalizationInstallationType.None:
                    btnLocalizationDisable.Visible = false;
                    UpdateMissingLocalizationInfo();
                    break;
                case LocalizationInstallationType.Enabled:
                    btnLocalizationDisable.Visible = !string.IsNullOrEmpty(_controller.CurrentInstallation.InstalledVersion);
                    btnLocalizationDisable.Text = Resources.Localization_Button_Disable_localization;
                    UpdatePresentLocalizationInfo();
                    break;
                case LocalizationInstallationType.Disabled:
                    btnLocalizationDisable.Visible = !string.IsNullOrEmpty(_controller.CurrentInstallation.InstalledVersion);
                    btnLocalizationDisable.Text = Resources.Localization_Button_Enable_localization;
                    UpdatePresentLocalizationInfo();
                    break;
            }
            cbAllowPreReleaseVersions.Checked = _controller.CurrentInstallation.AllowPreRelease;
            // monitoring
            cbCheckNewVersions.Checked = _controller.CurrentInstallation.MonitorForUpdates;
            cbRefreshTime.SelectedItem = _controller.CurrentInstallation.MonitorRefreshTime.ToString();
            UpdateButtonsVisibility();
        }

        private void UpdateMissingLocalizationInfo()
        {
            var lastVersion = _controller.CurrentInstallation.LastVersion;
            lblSelectedVersion.Text = Resources.Localization_Latest_Version;
            tbCurrentVersion.Text = string.IsNullOrEmpty(lastVersion) ? "N/A" : lastVersion;            
            lblCurrentLanguage.Visible = false;
            cbLanguages.Visible = false;
        }

        private void UpdatePresentLocalizationInfo()
        {
            //Languages
            var installedVersion = _controller.CurrentInstallation.InstalledVersion;
            if (!string.IsNullOrEmpty(installedVersion))
            {
                lblSelectedVersion.Text = Resources.Localization_Installed_Version;
                tbCurrentVersion.Text = installedVersion;
                if (_controller.GameSettings.LanguageInfo.Languages.Any())
                {
                    cbLanguages.DataSource = _controller.GameSettings.LanguageInfo.Languages.ToList();
                    cbLanguages.SelectedItem = _controller.GameSettings.LanguageInfo.Current;
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
                UpdateMissingLocalizationInfo();
            }
        }

        private void UpdateButtonsVisibility()
        {
            if (_controller.CurrentInstallation.InstalledVersion != null && _controller.CurrentRepository.CurrentVersion != null &&
                string.Compare(_controller.CurrentInstallation.InstalledVersion, _controller.CurrentRepository.CurrentVersion, StringComparison.OrdinalIgnoreCase) == 0)
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
