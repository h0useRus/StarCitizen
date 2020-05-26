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
                    tbGamePath.Text = GameService.Instance.GamePath.FullName;
                    cbGameModes.DataSource = GameService.Instance.GetModes();
                }
                else
                    MessageBox.Show(
                        "Выбран неправильный путь до Star Citizen!" + Environment.NewLine +
                        "Убедитесь, что вы выбрали ту же папку, что и при установке игры.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
