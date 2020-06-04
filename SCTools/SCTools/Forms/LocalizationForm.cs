using System;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Services;

namespace NSW.StarCitizen.Tools.Forms
{
    public partial class LocalizationForm : Form
    {
        private GameInfo _game;
        private LocalizationInfo _localization;
        private LanguagesInfo _languages;
        private bool holdUpdates = false;
        public LocalizationForm()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(IWin32Window owner, GameInfo gameInfo)
            => Init(gameInfo) ? ShowDialog(owner) : DialogResult.Cancel;

        private bool Init(GameInfo gameInfo)
        {
            _game = gameInfo;
            UpdateControls();
            // rollback localization
            var patch = LocalizationService.Instance.GetPatchSupport(gameInfo);
            if (patch.Status == PatchStatus.Patched)
            {
                LocalizationService.Instance.Patch(patch);
            }

            return true;
        }

        

        private void UpdateControls()
        {
            holdUpdates = true;

            if (string.IsNullOrWhiteSpace(LocalizationService.Instance.GetSettings(_game.Mode).LastVersion))
            {
                tbCurrentVersion.Text = LocalizationService.Instance.IsLocalizationInstalled(_game)
                    ? "Нет информации"
                    : "Локализацция не установлена";
            }
            else
            {
                tbCurrentVersion.Text = LocalizationService.Instance.GetSettings(_game.Mode).LastVersion;
            }

            btnInstall.Enabled = false;
            if (_localization == null)
            {
                tbServerVersion.Text = "Нажмите 'Обновить'";
            }
            else if(_localization.Release == null)
            {
                tbServerVersion.Text = "Ошибка связи, нажмите 'Обновить' ещё раз";
            }
            else
            {
                tbServerVersion.Text = _localization.Release.Name;
                btnInstall.Enabled = true;
            }

            _languages = LocalizationService.Instance.GetLanguagesConfiguration(_game);
            if (_languages.Languages.Count > 0)
            {
                cbCurrentLanguage.DataSource = _languages.Languages;
                cbCurrentLanguage.SelectedItem = _languages.Current;
                lblCurrentLanguage.Visible = cbCurrentLanguage.Visible = true;
            }
            else
            {
                lblCurrentLanguage.Visible = cbCurrentLanguage.Visible = false;
            }

            cbLocalizationCheckNewVersions.Checked =
                SettingsService.Instance.AppSettings.Localization.MonitorForUpdates;

            cbLocalizationRefreshTime.SelectedItem =
                SettingsService.Instance.AppSettings.Localization.MonitorRefreshTime.ToString();

            holdUpdates = false;
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            btnRefresh.Enabled = false;
            _localization = await LocalizationService.Instance.GetLocalizationStatusAsync(_game);
            UpdateControls();
            btnRefresh.Enabled = true;
        }

        private async void btnInstall_Click(object sender, EventArgs e)
        {
            btnInstall.Enabled = false;
            if (_localization?.Release != null)
            {
                var fileName = await LocalizationService.Instance.DownloadAsync(_localization.Release);
                if (!string.IsNullOrWhiteSpace(fileName)
                    && LocalizationService.Instance.UnZipFile(_game.RootFolder.FullName, fileName))
                {
                    LocalizationService.Instance.UpdateLastPatchVersion(_game.Mode, _localization.Release.Name);
                    UpdateControls();
                }
            }
            btnInstall.Enabled = true;
        }

        private void cbCurrentLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(holdUpdates) return;
            if (cbCurrentLanguage.SelectedItem is string selected)
            {
                cbCurrentLanguage.Enabled = false;
                _languages.New = selected;
                _languages = LocalizationService.Instance.UpdateLanguage(_game, _languages);
                cbCurrentLanguage.Enabled = true;
            }
        }

        private void cbLocalizationCheckNewVersions_CheckedChanged(object sender, EventArgs e)
        {
            if(holdUpdates) return;
            SettingsService.Instance.AppSettings.Localization.MonitorForUpdates =
                cbLocalizationCheckNewVersions.Checked;
            SettingsService.Instance.SaveAppSettings();
            RefreshMonitor();
        }

        private void RefreshMonitor()
        {
            if (SettingsService.Instance.AppSettings.Localization.MonitorForUpdates)
                LocalizationService.Instance.MonitorStart();
            else
                LocalizationService.Instance.MonitorStop();
        }

        private void cbLocalizationRefreshTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (holdUpdates) return;
            SettingsService.Instance.AppSettings.Localization.MonitorRefreshTime =
                int.TryParse(cbLocalizationRefreshTime.SelectedItem.ToString(), out var result) ? result : 5;
            SettingsService.Instance.SaveAppSettings();
            RefreshMonitor();
        }
    }
}
