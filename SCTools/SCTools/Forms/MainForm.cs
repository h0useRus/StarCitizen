using System;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Services;

namespace NSW.StarCitizen.Tools.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitVisuals();
        }

        private void btnGamePath_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog
            {
                Description = "Выберите установочную папку Star Citizen.",
                RootFolder = Environment.SpecialFolder.MyComputer,
                ShowNewFolderButton = false
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (GameService.Instance.SetFolder(dlg.SelectedPath))
                {
                    tbGamePath.TextAlign = HorizontalAlignment.Left;
                    tbGamePath.Text = GameService.Instance.GamePath.FullName.ToUpper();
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
                tbGameMode.Text = gameInfo.Mode == GameMode.LIVE
                    ? "Постоянная Вселенная"
                    : "Постоянная Тестовая Вселенная";

                tbGameVersion.Text = gameInfo.ExeVersion;
                gbGameInfo.Visible = gbButtonMenu.Visible = true;
            }
        }
    }
}
