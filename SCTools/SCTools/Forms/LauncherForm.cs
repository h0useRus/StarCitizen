using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using NLog;
using NSW.StarCitizen.Tools.Controllers;
using NSW.StarCitizen.Tools.Controls;
using NSW.StarCitizen.Tools.Helpers;
using NSW.StarCitizen.Tools.Launcher;
using NSW.StarCitizen.Tools.Lib.Global;
using NSW.StarCitizen.Tools.Lib.Helpers;
using NSW.StarCitizen.Tools.Lib.Localization;
using NSW.StarCitizen.Tools.Properties;

namespace NSW.StarCitizen.Tools.Forms
{
    public partial class LauncherForm : FormEx, ILocalizedForm
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ISet<string> _profiles = new HashSet<string>();
        private readonly GameInfo _gameInfo;
        private readonly string _loginDataFilePath;
        private readonly string _profilesPath;

        public LauncherForm(GameInfo gameInfo)
        {
            _gameInfo = gameInfo;
            _loginDataFilePath = Path.Combine(_gameInfo.RootFolderPath, "loginData.json");
            _profilesPath = Path.Combine(_gameInfo.RootFolderPath, "profiles");
            InitializeComponent();
            UpdateLocalizedControls();
        }

        private void LauncherForm_Load(object sender, EventArgs e)
        {
            Program.ProcessManager.ProcessExited += GameProcessExited;
            LoadImportedProfiles();
            UpdateProfilesCombobox(GetActiveProfileName());
        }

        private void LauncherForm_FormClosed(object sender, FormClosedEventArgs e) => Program.ProcessManager.ProcessExited -= GameProcessExited;

        public void UpdateLocalizedControls()
        {
            Text = Resources.Launcher_Title + " - " + _gameInfo.Mode;
            lblProfile.Text = Resources.Launcher_GameProfile;
            btnImportProfile.Text = Resources.Launcher_ImportCurrentProfile;
            btnRemoveProfile.Text = Resources.Launcher_DeleteProfile;
        }

        private void btnRunGame_Click(object sender, EventArgs e)
        {
            if (cbProfiles.SelectedItem is string selectedProfile)
            {
                if (Program.ProcessManager.IsProcessRunnnig(selectedProfile))
                {
                    Program.ProcessManager.StopProcess(selectedProfile);
                }
                else
                {
                    var localizationController = new LocalizationController(_gameInfo);
                    if (localizationController.GetInstallationType() == LocalizationInstallationType.None)
                    {
                        RtlAwareMessageBox.Show(this, Resources.Launcher_AskEnableLocalization_Text,
                            Resources.Launcher_AskEnableLocalization_Titile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (FileUtils.CopyFileNoThrow(GetProfileNamePath(selectedProfile), _loginDataFilePath, true))
                    {
                        btnRunGame.Enabled = false;
                        btnRunGame.ImageKey = "stop";
                        btnRemoveProfile.Enabled = false;
                        if (!Program.ProcessManager.LaunchProcess(this, _gameInfo, selectedProfile))
                        {
                            btnRemoveProfile.Enabled = true;
                            btnRunGame.ImageKey = "start";
                            RtlAwareMessageBox.Show(this, string.Format(Resources.Launcher_GameLaunchFailed_ErrorText, selectedProfile),
                                Resources.Launcher_GameLaunchFailed_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        btnRunGame.Enabled = true;
                        cbProfiles.Refresh();
                    }
                    else
                    {
                        RtlAwareMessageBox.Show(this, string.Format(Resources.Launcher_ProfileActivate_ErrorText, selectedProfile),
                            Resources.Launcher_ProfileActivate_ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnImportCurrentProfile_Click(object sender, EventArgs e)
        {
            var profileName = GetActiveProfileName();
            if (profileName != null)
            {
                if (FileUtils.CreateDirectoryNoThrow(_profilesPath) &&
                    FileUtils.CopyFileNoThrow(_loginDataFilePath, GetProfileNamePath(profileName), true))
                {
                    _profiles.Add(profileName);
                    UpdateProfilesCombobox(profileName);
                }
                else
                {
                    RtlAwareMessageBox.Show(this, Resources.Launcher_ProfileMissing_WarningText, Resources.Launcher_ProfileMissing_WarningTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                UpdateProfilesCombobox(null);
                RtlAwareMessageBox.Show(this, Resources.Launcher_ProfileMissing_WarningText, Resources.Launcher_ProfileMissing_WarningTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRemoveProfile_Click(object sender, EventArgs e)
        {
            if (cbProfiles.SelectedItem is string profileName)
            {
                if (RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentUICulture, Resources.Launcher_RemoveProfileConfirm_Text, profileName),
                        Resources.Localization_Warning_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    _profiles.Remove(profileName);
                    FileUtils.DeleteFileNoThrow(GetProfileNamePath(profileName));
                    UpdateProfilesCombobox(GetActiveProfileName());
                }
            }
        }

        private void cbProfiles_SelectedIndexChanged(object sender, EventArgs e) => UpdateButtons();

        private void cbProfiles_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                var fontStyle = FontStyle.Italic;
                if (cbProfiles.Items[e.Index] is string profile)
                {
                    fontStyle = Program.ProcessManager.IsProcessRunnnig(profile) ? FontStyle.Bold : FontStyle.Regular;
                }
                using var brush = new SolidBrush(e.ForeColor);
                using var font = new Font(e.Font, fontStyle);
                e.DrawBackground();
                e.Graphics.DrawString(cbProfiles.Items[e.Index].ToString(), font, brush, e.Bounds);
                e.DrawFocusRectangle();
            }
        }

        private void LoadImportedProfiles()
        {
            _profiles.Clear();
            var directory = new DirectoryInfo(_profilesPath);
            if (directory.Exists)
            {
                try
                {
                    foreach (var file in directory.EnumerateFiles("*.json", SearchOption.TopDirectoryOnly))
                    {
                        string profileName = Path.GetFileNameWithoutExtension(file.Name);
                        if (!string.IsNullOrWhiteSpace(profileName))
                        {
                            _profiles.Add(profileName);
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"Error during search profiles at path: {_profilesPath}");
                }
            }
        }

        private string? GetActiveProfileName() => JsonHelper.ReadFile<LoginDataUsername>(_loginDataFilePath)?.Username;

        private string GetProfileNamePath(string profileName) => Path.Combine(_profilesPath, profileName + ".json");

        private void UpdateButtons()
        {
            if (cbProfiles.SelectedItem is string selectedProfile)
            {
                bool isRunning = Program.ProcessManager.IsProcessRunnnig(selectedProfile);
                btnRunGame.ImageKey = isRunning ? "stop" : "start";
                btnRunGame.Enabled = true;
                btnRemoveProfile.Enabled = !isRunning;
            }
            else
            {
                btnRunGame.ImageKey = "start";
                btnRunGame.Enabled = false;
                btnRemoveProfile.Enabled = false;
            }
        }

        private void UpdateProfilesCombobox(string? selectedProfile)
        {
            cbProfiles.DataSource = null;
            cbProfiles.DataSource = _profiles.ToList();
            if (selectedProfile != null && _profiles.Contains(selectedProfile))
                cbProfiles.SelectedItem = selectedProfile;
            UpdateButtons();
        }

        private void GameProcessExited(object sender, ProcessExitedEventArgs e)
        {
            cbProfiles.Refresh();
            UpdateButtons();
            if (e.ExitCode != 0 && !e.Stopped)
            {
                RtlAwareMessageBox.Show(this, string.Format(Resources.Launcher_GameCrashed_ErrorText, e.ProfileName, e.ExitCode),
                        Resources.Launcher_GameCrashed_ErrorTitile, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public sealed class LoginDataUsername
    {
        [JsonProperty("username")]
        public string Username { get; }
        
        public LoginDataUsername(string username)
        {
            Username = username;
        }
    }
}
