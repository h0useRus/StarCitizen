using System;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Services;

namespace NSW.StarCitizen.Tools.Forms
{
    public partial class MainForm : Form
    {
        private GameInfo _current;
        public MainForm()
        {
            InitializeComponent();
            niTray.Text = Text;
            InitVisuals();
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

        private void InitVisuals()
        {
            gbGameInfo.Visible = gbButtonMenu.Visible = false;
            tbGamePath.Text = "Нажмите здесь, чтобы выбрать путь до папки Star Citizen";
            tbGamePath.TextAlign = HorizontalAlignment.Center;
            cbGameModes.DataSource = null;
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

        private void btnLocalization_Click(object sender, EventArgs e)
        {
            using var dlg =  new LocalizationForm();
            dlg.ShowDialog(this, _current);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SettingsService.Instance.AppSettings.GameFolder)
            && GameService.Instance.SetFolder(SettingsService.Instance.AppSettings.GameFolder))
            {
                tbGamePath.Text = SettingsService.Instance.AppSettings.GameFolder.ToUpper();
                tbGamePath.TextAlign = HorizontalAlignment.Left;
                cbGameModes.DataSource = GameService.Instance.GetModes();
            }
        }
    }
}
