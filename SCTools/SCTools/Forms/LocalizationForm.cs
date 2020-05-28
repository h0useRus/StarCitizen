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
        }

        private async void btnUpdateFiles_Click(object sender, System.EventArgs e)
        {
            if (_localization == null)
            {
                btnUpdateFiles.Enabled = false;
                _localization = await LocalizationService.Instance.GetLocalizationStatusAsync(_game);
                btnUpdateFiles.Enabled = true;

                if (_localization.Release == null)
                {
                    _localization = null;
                    lblVersion.Text = "Неудалось соединиться с сервером."
                                      + Environment.NewLine
                                      + "Попробуйте еще через несколько минут.";
                }

                btnUpdateFiles.Text = _localization.Status switch
                {
                    LocalizationStatus.NotInstalled => "Установить файлы локализации",
                    LocalizationStatus.Outdated => "Обновить файлы локализации",
                    LocalizationStatus.Actual => "Переустановить файлы локализации",
                };


                lblVersion.Text = $"Версия сервера: {_localization.Release.Name}";
                if (!string.IsNullOrWhiteSpace(SettingsService.Instance.AppSettings.Localization.LastVersion))
                    lblVersion.Text += Environment.NewLine +
                                       $"Ваша версия: {SettingsService.Instance.AppSettings.Localization.LastVersion}";

            }
            else
            {
                var fileName = await LocalizationService.Instance.DownloadAsync(_localization.Release);
                if (!string.IsNullOrWhiteSpace(fileName)
                    && LocalizationService.Instance.UnZipFile(_game.RootFolder.FullName, fileName))
                {
                    SettingsService.Instance.AppSettings.Localization.LastVersion = _localization.Release.Name;
                    SettingsService.Instance.SaveAppSettings();
                    lblVersion.Text =
                        $"Версия {SettingsService.Instance.AppSettings.Localization.LastVersion} успешно установлена.";
                    _localization = null;
                    btnUpdateFiles.Text = "Проверить обновления...";
                }
            }
        }

    }
}
