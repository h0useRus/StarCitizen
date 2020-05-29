using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Services;

namespace NSW.StarCitizen.Tools.Forms
{
    public partial class LocalizationForm : Form
    {
        private PatchInfo _current;
        private GameInfo _game;
        private LocalizationInfo _localization;

        public LocalizationForm()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(IWin32Window owner, GameInfo gameInfo)
            => Init(gameInfo) ? ShowDialog(owner) : DialogResult.Cancel;

        private bool Init(GameInfo gameInfo)
        {
            _game = gameInfo;
            _current = LocalizationService.Instance.GetPatchSupport(gameInfo);
            UpdateControls();
            return true;
        }

        private void btnLocalization_Click(object sender, System.EventArgs e)
        {
            _current = LocalizationService.Instance.Patch(_current);
            UpdateControls();
        }

        private void UpdateControls()
        {
            btnLocalization.Visible = _current.Status != PatchStatus.NotSupported;
            btnLocalization.Text = _current.Status == PatchStatus.Original
                ? "Включить поддержку локализации"
                : "Отключить поддержку локализации";

            if (string.IsNullOrWhiteSpace(SettingsService.Instance.AppSettings.Localization.LastVersion))
            {
                tbCurrentVersion.Text = LocalizationService.Instance.IsLocalizationInstalled(_game)
                    ? "Нет информации"
                    : "Локализацция не установлена";
            }
            else
            {
                tbCurrentVersion.Text = SettingsService.Instance.AppSettings.Localization.LastVersion;
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
                    SettingsService.Instance.AppSettings.Localization.LastVersion = _localization.Release.Name;
                    SettingsService.Instance.SaveAppSettings();
                    UpdateControls();
                }
            }
            btnInstall.Enabled = true;
        }
    }
}
