using System;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Services;

namespace NSW.StarCitizen.Tools.Forms
{
    public partial class MainForm : Form
    {
        private GameInfo _current;
        private bool _stopGeneral;


        public MainForm()
        {
            InitializeComponent();
            InitVisuals();
            InitGeneral();
            LocalizationService.Instance.RegisterNotification(niTray);
        }

        #region Methods

        private void InitVisuals()
        {
            niTray.Text = Text;
            gbGameInfo.Visible = gbButtonMenu.Visible = false;
            tbGamePath.Text = "Нажмите здесь, чтобы выбрать путь до папки Star Citizen";
            tbGamePath.TextAlign = HorizontalAlignment.Center;
            cbGameModes.DataSource = null;
        }
        private void InitGeneral()
        {
            _stopGeneral = true;
            cbGeneralRunMinimized.Checked = SettingsService.Instance.AppSettings.RunMinimized;
            cbGeneralRunWithWindows.Checked = SettingsService.Instance.AppSettings.RunWithWindows;
            _stopGeneral = false;
        }

        private void Minimize()
        {
            Hide();
            ShowInTaskbar = false;
        }
        private void Maximize()
        {
            Show();
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
        }

        #endregion

        #region Events

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SettingsService.Instance.AppSettings.GameFolder)
                && GameService.Instance.SetFolder(SettingsService.Instance.AppSettings.GameFolder))
            {
                tbGamePath.Text = SettingsService.Instance.AppSettings.GameFolder.ToUpper();
                tbGamePath.TextAlign = HorizontalAlignment.Left;
                cbGameModes.DataSource = GameService.Instance.GetModes();
            }

            if (SettingsService.Instance.AppSettings.Localization.MonitorForUpdates)
            {
                LocalizationService.Instance.MonitorStart();
            }

            if (SettingsService.Instance.AppSettings.RunMinimized)
                Minimize();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if(WindowState==FormWindowState.Minimized)
                Maximize();
            else
                Minimize();
        }
        private void niTray_MouseClick(object sender, MouseEventArgs e)
        {
            if (Visible)
                Minimize();
            else
                Maximize();
        }
        private void btnGamePath_Click(object sender, EventArgs e)
        {
            using var dlg = new FolderBrowserDialog
            {
                Description = "Выберите установочную папку Star Citizen.",
                RootFolder = Environment.SpecialFolder.MyComputer,
                ShowNewFolderButton = false,
                SelectedPath = tbGamePath.TextAlign == HorizontalAlignment.Left ? tbGamePath.Text : null
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (GameService.Instance.SetFolder(dlg.SelectedPath))
                {
                    SettingsService.Instance.AppSettings.GameFolder =
                        tbGamePath.Text = GameService.Instance.GamePath.FullName.ToUpper();
                    SettingsService.Instance.SaveAppSettings();

                    tbGamePath.TextAlign = HorizontalAlignment.Left;
                    cbGameModes.DataSource = GameService.Instance.GetModes();
                }
                else
                {
                    InitVisuals();
                    MessageBox.Show(
                        "Выбран неправильный путь до Star Citizen!" + Environment.NewLine +
                        "Убедитесь, что вы выбрали ту же папку, что и при установке игры.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnLocalization_Click(object sender, EventArgs e)
        {
            if (_current == null)
                return;

            using var dlg = new LocalizationForm();
            dlg.ShowDialog(this, _current);
        }
        private void cbGameModes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbGameModes.SelectedItem is GameInfo gameInfo)
            {
                _current = gameInfo;
                tbGameMode.Text = gameInfo.Mode == GameMode.LIVE
                    ? "Постоянная Вселенная"
                    : "Постоянная Тестовая Вселенная";

                btnLocalization.Text = $"Локализация {gameInfo.Mode}";
                tbGameVersion.Text = gameInfo.ExeVersion;
                gbGameInfo.Visible = gbButtonMenu.Visible = true;
            }
        }
        private void cbGeneralRunWithWindows_CheckedChanged(object sender, EventArgs e)
        {
            if (_stopGeneral)
                return;
            SettingsService.Instance.AppSettings.RunWithWindows = cbGeneralRunWithWindows.Checked;
        }
        private void cbGeneralRunMinimized_CheckedChanged(object sender, EventArgs e)
        {
            if (_stopGeneral)
                return;
            SettingsService.Instance.AppSettings.RunMinimized = cbGeneralRunMinimized.Checked;
            SettingsService.Instance.SaveAppSettings();
        }
        #endregion
    }
}
