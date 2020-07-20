using System;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Global;
using NSW.StarCitizen.Tools.Properties;

namespace NSW.StarCitizen.Tools.Forms
{
    public partial class MainForm : Form
    {
        private bool _holdUpdates;

        public MainForm()
        {
            InitializeComponent();
            InitVisuals();
            InitGeneral();
        }

        #region Methods

        private void InitVisuals()
        {
            niTray.Text = Text = Resources.AppName;
            Program.Notification += (sender, s) =>
            {
                niTray.ShowBalloonTip(5000, s.Item2, s.Item1, ToolTipIcon.Info);
            };

            gbGameInfo.Visible = gbButtonMenu.Visible = false;
            tbGamePath.Text = Resources.GamePath_Hint;
            tbGamePath.TextAlign = HorizontalAlignment.Center;
            cbGameModes.DataSource = null;
        }

        private void InitGeneral()
        {
            _holdUpdates = true;
            cbGeneralRunMinimized.Checked = Program.Settings.RunMinimized;
            cbGeneralRunWithWindows.Checked = Program.Settings.RunWithWindows;
            _holdUpdates = false;
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
            

            if (Program.SetGameFolder(Program.Settings.GameFolder))
            {
                tbGamePath.Text = Program.Settings.GameFolder.ToUpper();
                tbGamePath.TextAlign = HorizontalAlignment.Left;
                cbGameModes.DataSource = Program.GetGameModes();
            }

            if (Program.Settings.RunMinimized)
                Minimize();

            Program.RunMonitors();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
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
                Description = Resources.GamePath_Description,
                RootFolder = Environment.SpecialFolder.MyComputer,
                ShowNewFolderButton = false,
                SelectedPath = tbGamePath.TextAlign == HorizontalAlignment.Left ? tbGamePath.Text : null
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (Program.SetGameFolder(dlg.SelectedPath))
                {
                    tbGamePath.TextAlign = HorizontalAlignment.Left;
                    cbGameModes.DataSource = Program.GetGameModes();
                }
                else
                {
                    InitVisuals();
                    MessageBox.Show(Resources.GamePath_Error_Text, Resources.GamePath_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnLocalization_Click(object sender, EventArgs e)
        {
            if (Program.CurrentGame == null)
                return;

            using var dlg = new LocalizationForm();
            dlg.ShowDialog(this);
        }
        private void cbGameModes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbGameModes.SelectedItem is GameInfo gameInfo)
            {
                Program.CurrentGame = gameInfo;
                tbGameMode.Text = gameInfo.Mode == GameMode.LIVE
                    ? Resources.GameMode_LIVE
                    : Resources.GameMode_PTU;

                btnLocalization.Text = string.Format(Resources.LocalizationButton_Text, gameInfo.Mode);
                tbGameVersion.Text = gameInfo.ExeVersion;
                gbGameInfo.Visible = gbButtonMenu.Visible = true;
            }
        }
        private void cbGeneralRunWithWindows_CheckedChanged(object sender, EventArgs e)
        {
            if (_holdUpdates) return;
            Program.Settings.RunWithWindows = cbGeneralRunWithWindows.Checked;
        }
        private void cbGeneralRunMinimized_CheckedChanged(object sender, EventArgs e)
        {
            if (_holdUpdates) return;
            Program.Settings.RunMinimized = cbGeneralRunMinimized.Checked;
            Program.SaveAppSettings();
        }
        #endregion
    }
}
