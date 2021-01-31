using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Windows.Forms;
using NSW.StarCitizen.Tools.Adapters;
using NSW.StarCitizen.Tools.Controllers;
using NSW.StarCitizen.Tools.Global;
using NSW.StarCitizen.Tools.Helpers;
using NSW.StarCitizen.Tools.Localization;
using NSW.StarCitizen.Tools.Properties;

namespace NSW.StarCitizen.Tools.Forms
{
    public partial class MainForm : Form, ILocalizedForm
    {
        private bool _isGameFolderSet;
        private bool _holdUpdates;
        private string? _lastBrowsePath;
        private List<GameMode>? _gameModes;

        public MainForm()
        {
            InitializeComponent();
            InitializeGeneral();
            UpdateLocalizedControls();
        }

        #region Methods

        public void UpdateLocalizedControls()
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
            // menu localization
            miExitApp.Text = Resources.Localization_QuitApp_Text;
            miSettings.Text = Resources.Localization_Settings_Text;
            miRunMinimized.Text = Resources.Localization_RunMinimized_Text;
            miRunOnStartup.Text = Resources.Localization_RunOnStartup_Text;
            miRunTopMost.Text = Resources.Localization_AlwaysOnTop_Text;
            miUseHttpProxy.Text = Resources.Localization_UseHttpProxy_Text;
            miTools.Text = Resources.Tools_Title;
            miMoveLiveToPtu.Text = Resources.Tools_Move_LIVE_PTU;
            miMovePtuToLive.Text = Resources.Tools_Move_PTU_LIVE;
            UpdateAppInstallButton();
        }

        private void InitializeGeneral()
        {
            Program.Updater.Notification += (sender, s) =>
            {
                niTray.ShowBalloonTip(5000, s.Item2, s.Item1, ToolTipIcon.Info);
            };
            foreach (var manager in Program.RepositoryManagers.Values)
            {
                manager.Notification += (sender, s) =>
                {
                    niTray.ShowBalloonTip(5000, s.Item2, s.Item1, ToolTipIcon.Info);
                };
            }
            TopMost = Program.Settings.TopMostWindow;
            InitLanguageCombobox(cbLanguage);
            cbRefreshTime.SelectedItem = Program.Settings.Update.MonitorRefreshTime.ToString();
            _holdUpdates = true;
            cbGeneralRunMinimized.Checked = Program.Settings.RunMinimized;
            cbGeneralRunWithWindows.Checked = Program.Settings.RunWithWindows;
            cbCheckNewVersions.Checked = Program.Settings.Update.MonitorUpdates;
            _holdUpdates = false;
        }

        private void Minimize()
        {
            ShowInTaskbar = false;
            Hide();
        }

        private void Restore()
        {
            ShowInTaskbar = true;
            Show();
            WindowState = FormWindowState.Normal;
            WinApi.ShowToFront(Handle);
        }

        #endregion

        #region Events

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetGameFolder(Program.Settings.GameFolder);

            if (Program.Settings.Update.MonitorUpdates)
                Program.Updater.MonitorStart(Program.Settings.Update.MonitorRefreshTime);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (Program.Settings.RunMinimized)
                Minimize();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Minimize();
            }
        }

        private void niTray_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Left))
            {
                if (ShowInTaskbar)
                {
                    if (CanFocus)
                        Minimize();
                }
                else
                    Restore();
            }
        }

        private void niTray_BalloonTipClicked(object sender, EventArgs e) => Restore();

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
                string? gamePath = Program.SearchGameFolder(_lastBrowsePath);
                if (!SetGameFolder(gamePath))
                {
                    MessageBox.Show(this, Resources.GamePath_Error_Text, Resources.GamePath_Error_Title,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (string.Compare(Program.Settings.GameFolder, gamePath, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    Program.Settings.GameFolder = gamePath;
                    Program.SaveAppSettings();
                }
            }
        }

        private void btnLocalization_Click(object sender, EventArgs e)
        {
            if (Program.CurrentGame != null)
            {
                using var dlg = new LocalizationForm(Program.CurrentGame);
                dlg.ShowDialog(this);
                SetGameFolder(Program.Settings.GameFolder);
            }
        }

        private async void btnUpdateLocalization_Click(object sender, EventArgs e)
        {
            if (Program.CurrentGame == null) return;
            var controller = new LocalizationController(Program.CurrentGame);
            controller.Load();
            var installedVersion = controller.CurrentInstallation.InstalledVersion;
            if (installedVersion != null && await controller.RefreshVersionsAsync(this))
            {
                var availableUpdate = controller.CurrentRepository.LatestUpdateInfo;
                if (availableUpdate != null &&
                    string.Compare(installedVersion, availableUpdate.GetVersion(),
                        StringComparison.OrdinalIgnoreCase) != 0)
                {
                    var dialogResult = MessageBox.Show(this,
                        string.Format(Resources.Localization_UpdateAvailableInstallAsk_Text,
                            $"\n{controller.CurrentRepository.Name} - {availableUpdate.GetVersion()}"),
                        Resources.Localization_CheckForUpdate_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes &&
                        await controller.InstallVersionAsync(this, availableUpdate))
                    {
                        controller.CurrentRepository.SetCurrentVersion(availableUpdate.GetVersion());
                    }
                }
                else
                {
                    MessageBox.Show(this, Resources.Localization_NoUpdatesFound_Text,
                        Resources.Localization_CheckForUpdate_Title,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
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
                Program.UpdateUiLanguage();
            }
        }

        private async void btnAppUpdate_Click(object sender, EventArgs e)
        {
            if (Program.Updater.GetScheduledUpdateInfo() != null)
            {
                if (Program.InstallScheduledUpdate())
                    Close();
                UpdateAppInstallButton();
                return;
            }
            using var progressDlg = new ProgressForm();
            try
            {
                Enabled = false;
                Cursor.Current = Cursors.WaitCursor;
                progressDlg.BindAdapter(new CheckForUpdateDialogAdapter());
                progressDlg.Show(this);
                var availableUpdate = await Program.Updater.CheckForUpdateVersionAsync(progressDlg.CancelToken);
                progressDlg.CurrentTaskProgress = 1.0f;
                if (availableUpdate == null)
                {
                    progressDlg.Hide();
                    MessageBox.Show(this, Resources.Application_NoUpdatesFound_Text,
                        Resources.Application_CheckForUpdate_Title,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                var dialogResult = MessageBox.Show(this,
                    string.Format(Resources.Application_UpdateAvailableDownloadAsk_Text, availableUpdate.GetVersion()),
                    Resources.Application_CheckForUpdate_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    var downloadDialogAdapter = new DownloadProgressDialogAdapter(null);
                    progressDlg.BindAdapter(downloadDialogAdapter);
                    var filePath = await Program.Updater.DownloadVersionAsync(availableUpdate, progressDlg.CancelToken,
                        downloadDialogAdapter);
                    Program.Updater.ScheduleInstallUpdate(availableUpdate, filePath);
                }
            }
            catch (Exception exception)
            {
                if (!progressDlg.IsCanceledByUser)
                {
                    progressDlg.Hide();
                    if (exception is HttpRequestException)
                    {
                        MessageBox.Show(this, Resources.Localization_Download_ErrorText + '\n' + exception.Message,
                            Resources.Localization_Download_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(this, Resources.Localization_Download_ErrorText,
                            Resources.Localization_Download_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                Enabled = true;
                progressDlg.Hide();
                UpdateAppInstallButton();
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

        private void cmTrayMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            miRunMinimized.Checked = Program.Settings.RunMinimized;
            miRunOnStartup.Checked = Program.Settings.RunWithWindows;
            miRunTopMost.Checked = Program.Settings.TopMostWindow;
            miUseHttpProxy.Checked = Program.Settings.UseHttpProxy;
            if (Program.Settings.GameFolder != null && _gameModes != null)
            {
                miMoveLiveToPtu.Enabled = _gameModes.Contains(GameMode.LIVE) && !_gameModes.Contains(GameMode.PTU);
                miMovePtuToLive.Enabled = _gameModes.Contains(GameMode.PTU) && !_gameModes.Contains(GameMode.LIVE);
            }
            else
            {
                miMoveLiveToPtu.Enabled = false;
                miMovePtuToLive.Enabled = false;
            }
            if (cbMenuLanguage.ComboBox != null)
            {
                _holdUpdates = true;
                InitLanguageCombobox(cbMenuLanguage.ComboBox);
                _holdUpdates = false;
            }
        }

        private void miAppName_Click(object sender, EventArgs e) => Restore();

        private void miExitApp_Click(object sender, EventArgs e)
        {
            if (CanFocus || !ShowInTaskbar)
                Close();
            else
                Restore();
        }

        private void miRunMinimized_Click(object sender, EventArgs e) => cbGeneralRunMinimized.Checked = miRunMinimized.Checked;

        private void miRunOnStartup_Click(object sender, EventArgs e) => cbGeneralRunWithWindows.Checked = miRunOnStartup.Checked;

        private void miRunTopMost_Click(object sender, EventArgs e)
        {
            TopMost = miRunTopMost.Checked;
            Program.Settings.TopMostWindow = miRunTopMost.Checked;
            Program.SaveAppSettings();
        }

        private void miUseHttpProxy_Click(object sender, EventArgs e)
        {
            Program.Settings.UseHttpProxy = miUseHttpProxy.Checked;
            Program.SaveAppSettings();
            // TODO: need some dialog about need restart to apply this setting
        }

        private void cbMenuLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_holdUpdates) return;
            if (cbMenuLanguage.ComboBox != null && cbMenuLanguage.ComboBox.SelectedValue is string language)
            {
                cbLanguage.SelectedValue = language;
                Program.Settings.Language = language;
                Program.SaveAppSettings();
                Program.UpdateUiLanguage();
            }
        }

        private void miMoveLiveToPtu_Click(object sender, EventArgs e)
        {
            if (Program.Settings.GameFolder == null) return;
            var controller = new GameModesController(Program.Settings.GameFolder, Program.CurrentGame);
            if (controller.MoveGameMode(this, GameMode.LIVE, GameMode.PTU))
            {
                SetGameFolder(Program.Settings.GameFolder);
            }
        }

        private void miMovePtuToLive_Click(object sender, EventArgs e)
        {
            if (Program.Settings.GameFolder == null) return;
            var controller = new GameModesController(Program.Settings.GameFolder, Program.CurrentGame);
            if (controller.MoveGameMode(this, GameMode.PTU, GameMode.LIVE))
            {
                SetGameFolder(Program.Settings.GameFolder);
            }
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
            if (path != null)
            {
                var gameModes = Program.GetGameModes(path);
                foreach (var gameMode in gameModes)
                {
                    _isGameFolderSet = true;
                    gbGameInfo.Visible = true;
                    gbButtonMenu.Visible = true;
                    cbGameModes.Visible = true;
                    tbGamePath.Text = path.ToUpper();
                    tbGamePath.TextAlign = HorizontalAlignment.Left;
                    cbGameModes.DataSource = gameModes;
                    _gameModes = gameModes.Select(i => i.Mode).ToList();
                    SetGameModeInfo(gameMode);
                    Program.RunRepositoryMonitors(gameModes);
                    return true;
                }
            }
            _isGameFolderSet = false;
            gbGameInfo.Visible = false;
            gbButtonMenu.Visible = false;
            cbGameModes.Visible = false;
            tbGamePath.Text = Resources.GamePath_Hint;
            tbGamePath.TextAlign = HorizontalAlignment.Center;
            cbGameModes.DataSource = null;
            _gameModes = null;
            Program.CurrentGame = null;
            Program.StopRepositoryMonitors();
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
            btnUpdateLocalization.Text = Resources.Localization_CheckForUpdates_Text;
            var controller = new LocalizationController(gameInfo);
            btnUpdateLocalization.Visible = controller.CurrentInstallation.InstalledVersion != null &&
                                            controller.GetInstallationType() != LocalizationInstallationType.None;
        }

        private void UpdateAppInstallButton()
        {
            var scheduledUpdateInfo = Program.Updater.GetScheduledUpdateInfo();
            btnAppUpdate.Text = scheduledUpdateInfo != null
                ? string.Format(Resources.Localization_InstallUpdateVer_Text, scheduledUpdateInfo.GetVersion())
                : Resources.Application_CheckForUpdates_Text;
        }

        private void InitLanguageCombobox(ComboBox combobox)
        {
            combobox.BindingContext = BindingContext;
            combobox.DataSource = new BindingSource(Program.GetSupportedUiLanguages(), null);
            combobox.DisplayMember = "Value";
            combobox.ValueMember = "Key";
            combobox.SelectedValue = Program.Settings.Language;
        }
    }
}
