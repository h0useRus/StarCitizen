using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Adapters;
using NSW.StarCitizen.Tools.Global;
using NSW.StarCitizen.Tools.Helpers;
using NSW.StarCitizen.Tools.Properties;

namespace NSW.StarCitizen.Tools.Forms
{
    public partial class MainForm : Form
    {
        private bool _isGameFolderSet;
        private bool _holdUpdates;
        private string? _lastBrowsePath;

        public MainForm()
        {
            InitializeComponent();
            InitializeGeneral();
            InitializeLocalization();
        }

        #region Methods

        private void InitializeLocalization()
        {
            Text = niTray.Text = string.Format(Resources.AppName, Program.Version.ToString(3));
            if (!_isGameFolderSet)
                tbGamePath.Text = Resources.GamePath_Hint;
            if (Program.CurrentGame != null)
                UpdateGameModeInfo(Program.CurrentGame);
            lblGameMode.Text = Resources.Localization_Game_Mode;
            lblGameVersion.Text = Resources.Localization_Game_Version;
            lblLanguage.Text = Resources.Localization_Language_Text;
            cbGeneralRunMinimized.Text = Resources.Localization_RunMinimized_Text;
            cbGeneralRunWithWindows.Text = Resources.Localization_RunOnStartup_Text;
            lblMinutes.Text = Resources.Localization_AutomaticCheck_Measure;
            cbCheckNewVersions.Text = Resources.Localization_CheckForVersionEvery_Text;
            UpdateInstallButton();
        }

        private void InitializeGeneral()
        {
            Program.Updater.Notification += (sender, s) =>
            {
                niTray.ShowBalloonTip(5000, s.Item2, s.Item1, ToolTipIcon.Info);
            };
            Program.RepositoryManager.Notification += (sender, s) =>
            {
                niTray.ShowBalloonTip(5000, s.Item2, s.Item1, ToolTipIcon.Info);
            };
            cbLanguage.DataSource = new BindingSource(GetSupportedLanguages(), null);
            cbLanguage.DisplayMember = "Value";
            cbLanguage.ValueMember = "Key";
            cbLanguage.SelectedValue = Program.Settings.Language;
            cbRefreshTime.SelectedItem = Program.Settings.Update.MonitorRefreshTime.ToString();
            _holdUpdates = true;
            cbGeneralRunMinimized.Checked = Program.Settings.RunMinimized;
            cbGeneralRunWithWindows.Checked = Program.Settings.RunWithWindows;
            cbCheckNewVersions.Checked = Program.Settings.Update.MonitorUpdates;
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

        private void Restore()
        {
            Maximize();
            WinApi.ShowToFront(Handle);
        }

        #endregion

        #region Events

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetGameFolder(Program.Settings.GameFolder);

            if (Program.Settings.RunMinimized)
                Minimize();

            Program.RepositoryManager.RunMonitors();
            if (Program.Settings.Update.MonitorUpdates)
                Program.Updater.MonitorStart(Program.Settings.Update.MonitorRefreshTime);
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
                SelectedPath = _lastBrowsePath ?? (_isGameFolderSet ? tbGamePath.Text : string.Empty)
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _lastBrowsePath = dlg.SelectedPath;
                if (!SetGameFolder(dlg.SelectedPath))
                {
                    MessageBox.Show(Resources.GamePath_Error_Text, Resources.GamePath_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLocalization_Click(object sender, EventArgs e)
        {
            if (Program.CurrentGame == null)
                return;

            using var dlg = new LocalizationForm(Program.CurrentGame);
            dlg.ShowDialog(this);
        }

        private void cbGameModes_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbGameModes.SelectedItem is GameInfo gameInfo)
            {
                SetGameModeInfo(gameInfo);
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

        private void cbLanguage_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbLanguage.SelectedValue is string language)
            {
                Program.Settings.Language = language;
                Program.SaveAppSettings();
                InitializeLocalization();
            }
        }

        private async void btnAppUpdate_Click(object sender, EventArgs e)
        {
            if (Program.Updater.GetScheduledUpdateInfo() != null)
            {
                if (Program.InstallScheduledUpdate())
                    Close();
                UpdateInstallButton();
                return;
            }
            using var progressDlg = new ProgressForm();
            try
            {
                Enabled = false;
                Cursor.Current = Cursors.WaitCursor;
                progressDlg.Text = Resources.Localization_ApplicationUpdate_Title;
                var checkForUpdateDialogAdapter = new CheckForUpdateDialogAdapter(progressDlg);
                progressDlg.Show(this);
                var availableUpdate = await Program.Updater.CheckForUpdateVersionAsync(progressDlg.CancelToken);
                progressDlg.CurrentTaskProgress = 1.0f;
                if (availableUpdate == null)
                {
                    progressDlg.Hide();
                    MessageBox.Show(this, Resources.Localization_NoUpdatesFound_Text, Resources.Localization_CheckForUpdate_Title,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                var dialogResult = MessageBox.Show(string.Format(Resources.Localization_UpdateAvailableDownloadAsk_Text, availableUpdate.GetVersion()),
                        Resources.Localization_CheckForUpdate_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    var downloadDialogAdapter = new DownloadProgressDialogAdapter(progressDlg);
                    var filePath = await Program.Updater.DownloadVersionAsync(availableUpdate, progressDlg.CancelToken, downloadDialogAdapter);
                    Program.Updater.ScheduleInstallUpdate(availableUpdate, filePath);
                }
            }
            catch
            {
                if (!progressDlg.IsCanceledByUser)
                {
                    progressDlg.Hide();
                    MessageBox.Show(this, Resources.Localization_Download_ErrorText,
                        Resources.Localization_Download_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                Enabled = true;
                progressDlg.Hide();
                UpdateInstallButton();
            }
        }

        private void cbCheckNewVersions_CheckedChanged(object sender, EventArgs e)
        {
            if (_holdUpdates) return;
            Program.Settings.Update.MonitorUpdates = cbCheckNewVersions.Checked;
            if (Program.Settings.Update.MonitorUpdates)
                Program.Updater.MonitorStart(Program.Settings.Update.MonitorRefreshTime);
            else
                Program.Updater.MonitorStop();
            Program.SaveAppSettings();
        }

        private void cbRefreshTime_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Program.Settings.Update.MonitorRefreshTime = int.Parse(cbRefreshTime.SelectedItem.ToString());
            if (Program.Settings.Update.MonitorUpdates)
                Program.Updater.MonitorStart(Program.Settings.Update.MonitorRefreshTime);
            Program.SaveAppSettings();
        }

        #endregion

        protected override void WndProc(ref Message message)
        {
            if (message.Msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
            {
                Restore();
            }
            base.WndProc(ref message);
        }

        private bool SetGameFolder(string? path)
        {
            if (Program.SetGameFolder(path))
            {
                _isGameFolderSet = true;
                gbGameInfo.Visible = true;
                gbButtonMenu.Visible = true;
                cbGameModes.Visible = true;
                tbGamePath.Text = path.ToUpper();
                tbGamePath.TextAlign = HorizontalAlignment.Left;
                var gameModes = Program.GetGameModes();
                cbGameModes.DataSource = gameModes;
                SetGameModeInfo(gameModes.FirstOrDefault());
                return true;
            }
            _isGameFolderSet = false;
            gbGameInfo.Visible = false;
            gbButtonMenu.Visible = false;
            cbGameModes.Visible = false;
            tbGamePath.Text = Resources.GamePath_Hint;
            tbGamePath.TextAlign = HorizontalAlignment.Center;
            cbGameModes.DataSource = null;
            Program.CurrentGame = null;
            return false;
        }

        private void SetGameModeInfo(GameInfo gameInfo)
        {
            if (Program.CurrentGame != gameInfo)
            {
                Program.CurrentGame = gameInfo;
                UpdateGameModeInfo(gameInfo);
            }
        }

        private void UpdateGameModeInfo(GameInfo gameInfo)
        {
            tbGameMode.Text = gameInfo.Mode == GameMode.LIVE
                    ? Resources.GameMode_LIVE
                    : Resources.GameMode_PTU;
            btnLocalization.Text = string.Format(Resources.LocalizationButton_Text, gameInfo.Mode);
            tbGameVersion.Text = gameInfo.ExeVersion;
        }

        private void UpdateInstallButton()
        {
            var scheduledUpdateInfo = Program.Updater.GetScheduledUpdateInfo();
            if (scheduledUpdateInfo != null)
                btnAppUpdate.Text = string.Format(Resources.Localization_InstallUpdateVer_Text, scheduledUpdateInfo.GetVersion());
            else
                btnAppUpdate.Text = Resources.Localization_CheckForUpdates_Text;
        }

        private static Dictionary<string, string> GetSupportedLanguages()
        {
            var languages = new Dictionary<string, string> {
                { "en-US", "english" }
            };
            var neutralCultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures)
                .Where(c => Directory.Exists(c.TwoLetterISOLanguageName));
            foreach (var neutralCulture in neutralCultures)
            {
                var culture = CultureInfo.CreateSpecificCulture(neutralCulture.Name);
                languages.Add(culture.Name, neutralCulture.NativeName);
            }
            return languages;
        }
    }
}
