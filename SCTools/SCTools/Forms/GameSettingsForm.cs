using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Defter.StarCitizen.ConfigDB.Model;
using NSW.StarCitizen.Tools.Controllers;
using NSW.StarCitizen.Tools.Controls;
using NSW.StarCitizen.Tools.Helpers;
using NSW.StarCitizen.Tools.Lib.Global;
using NSW.StarCitizen.Tools.Lib.Helpers;
using NSW.StarCitizen.Tools.Lib.Localization;
using NSW.StarCitizen.Tools.Properties;
using NSW.StarCitizen.Tools.Repository;

namespace NSW.StarCitizen.Tools.Forms
{
    public partial class GameSettingsForm : Form, ILocalizedForm
    {
        private readonly GameInfo _gameInfo;
        private readonly GameSettings _gameSettings;
        private readonly ProfileManager _profileManager = new ProfileManager();
        private IReadOnlyList<ISettingControl> _settingControls = new List<ISettingControl>();
        private string? _currentProfileName;
        private string? _appliedProfileName;
        private ConfigData _configData;

        public GameSettingsForm(GameInfo gameInfo, ConfigData configData)
        {
            _gameInfo = gameInfo;
            _configData = configData;
            _gameSettings = new GameSettings(gameInfo);
            InitializeComponent();
            UpdateLocalizedControlsOnly();
        }

        private void GameSettingsForm_Load(object sender, EventArgs e)
        {
#if DEBUG
            lblReportIssues.Visible = false;
            lblSettingsDbRepoUrl.Text = ConfigDataRepository.SourceSettings.DatabasePath;
#else
            lblReportIssues.Visible = true;
            lblSettingsDbRepoUrl.Text = ConfigDataRepository.SourceSettings.RepositoryUrl;
#endif
            _profileManager.Load();

            cbProfiles.BindingContext = BindingContext;
            SetProfilesList(_profileManager.Profiles.Keys);
            cbProfiles.SelectedIndex = -1;
            btnRenameProfile.Enabled = false;
            btnDeleteProfile.Enabled = false;
            _currentProfileName = null;

            LoadSettingControls(_configData);

            var cfgData = _gameSettings.Load();
            cfgData.RemoveRow(GameConstants.CurrentLanguageKey);
            LoadGameSettings(cfgData);
            SelectProfileByData(cfgData);
        }

        private void GameSettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_currentProfileName != null && AskAndSaveProfileChanges(_currentProfileName) == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        public async void UpdateLocalizedControls()
        {
            if (_currentProfileName != null)
            {
                AskAndSaveProfileChanges(_currentProfileName, withCancel:false);
            }
            UpdateLocalizedControlsOnly();
            await LoadDatabaseAsync();
        }

        private void UpdateLocalizedControlsOnly()
        {
            Text = Resources.GameSettings_Title + " - " + _gameInfo.Mode;
            lblProfile.Text = Resources.GameSettings_Profile_Text;
            lblReportIssues.Text = Resources.GameSettings_FoundAnyIssue_Text;
            btnNewProfile.Text = Resources.GameSettings_ProfileNew_Button;
            btnRenameProfile.Text = Resources.GameSettings_ProfileRename_Button;
            btnDeleteProfile.Text = Resources.GameSettings_ProfileDelete_Button;
            btnResetAll.Text = Resources.GameSettings_Reset_All_Button;
            btnResetPage.Text = Resources.GameSettings_Reset_Page_Button;
            btnSave.Text = Resources.GameSettings_Save_Button;
        }

        private void lblSettingsDbRepoUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(lblSettingsDbRepoUrl.Text);
            lblSettingsDbRepoUrl.LinkVisited = true;
        }

        private void cbProfiles_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbProfiles.SelectedItem is string profileName)
            {
                if (_currentProfileName != null && !profileName.Equals(_currentProfileName) &&
                    AskAndSaveProfileChanges(_currentProfileName) == DialogResult.Cancel)
                {
                    cbProfiles.SelectedItem = _currentProfileName;
                    return;
                }
                if (!_profileManager.Profiles.TryGetValue(profileName, out var profileData))
                {
                    cbProfiles.SelectedItem = _currentProfileName;
                    return;
                }
                _currentProfileName = profileName;
                LoadGameSettings(profileData);
                UpdateSettingsVisibility();
                btnRenameProfile.Enabled = true;
                btnDeleteProfile.Enabled = true;
            }
        }

        private void cbProfiles_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                var profileName = cbProfiles.Items[e.Index].ToString();
                var fontStyle = profileName.Equals(_appliedProfileName) ? FontStyle.Bold : FontStyle.Regular;
                using var brush = new SolidBrush(e.ForeColor);
                using var font = new Font(e.Font, fontStyle);
                e.DrawBackground();
                e.Graphics.DrawString(profileName, font, brush, e.Bounds);
                e.DrawFocusRectangle();
            }
        }

        private void btnNewProfile_Click(object sender, EventArgs e)
        {
            using var promptDlg = new PromptForm(PromptForm.PromptType.CreateProfile, IsValidAndAvailableProfileName)
            {
                MaxValueLength = 32
            };
            if (promptDlg.ShowDialog(this) == DialogResult.OK)
            {
                var profileName = promptDlg.Value.Trim();
                if (_profileManager.CreateProfile(profileName, GetCurrentConfigData()))
                {
                    UpdateProfiles(profileName);
                }
                else
                {
                    MessageBox.Show(this, Resources.GameSettings_ProfileError_Text,
                        Resources.GameSettings_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRenameProfile_Click(object sender, EventArgs e)
        {
            if (cbProfiles.SelectedItem is string profileName)
            {
                using var promptDlg = new PromptForm(PromptForm.PromptType.RenameProfile, IsValidAndAvailableProfileName)
                {
                    Value = profileName,
                    MaxValueLength = 32
                };
                if (promptDlg.ShowDialog(this) == DialogResult.OK)
                {
                    var newProfileName = promptDlg.Value.Trim();
                    if (_profileManager.RenameProfile(profileName, newProfileName))
                    {
                        if (_appliedProfileName != null && _appliedProfileName.Equals(profileName))
                        {
                            _appliedProfileName = newProfileName;
                        }
                        UpdateProfiles(newProfileName);
                    }
                    else
                    {
                        MessageBox.Show(this, Resources.GameSettings_ProfileError_Text,
                            Resources.GameSettings_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnDeleteProfile_Click(object sender, EventArgs e)
        {
            if (cbProfiles.SelectedItem is string profileName)
            {
                _profileManager.DeleteProfile(profileName);
                if (_appliedProfileName != null && _appliedProfileName.Equals(profileName))
                {
                    _appliedProfileName = null;
                }
                UpdateProfiles(null);
            }
        }

        private void UpdateProfiles(string? selectValue)
        {
            SetProfilesList(_profileManager.Profiles.Keys);
            if (_profileManager.Profiles.Count > 0)
            {
                if (selectValue != null)
                {
                    cbProfiles.SelectedItem = selectValue;
                    _currentProfileName = selectValue;
                }
                else
                {
                    cbProfiles.SelectedIndex = -1;
                    _currentProfileName = null;
                }
            }
            else
            {
                _currentProfileName = null;
            }
            btnRenameProfile.Enabled = _currentProfileName != null;
            btnDeleteProfile.Enabled = _currentProfileName != null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string? selectedProfile = null;
            if (cbProfiles.SelectedItem is string profileName)
            {
                var result = AskAndSaveProfileChanges(profileName);
                if (result == DialogResult.Cancel)
                {
                    return;
                }
                if (result == DialogResult.Yes)
                {
                    selectedProfile = profileName;
                }
            }
            if (_gameSettings.SaveConfig(GetCurrentConfigData()))
            {
                _appliedProfileName = selectedProfile;
                if (selectedProfile == null)
                {
                    UpdateProfiles(null);
                }
            }
            else
            {
                MessageBox.Show(this, Resources.GameSettings_SettingApplyError_Text,
                    Resources.GameSettings_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cbProfiles.Refresh();
        }

        private void btnResetAll_Click(object sender, EventArgs e) => ResetGameSettings();

        private void btnResetPage_Click(object sender, EventArgs e) => ResetAtPageSettings();

        private void cmGameSetting_Opened(object sender, EventArgs e)
        {
            bool settingWithValue = cmGameSetting.SourceControl is ISettingControl setting && setting.HasValue;
            miResetSetting.Enabled = settingWithValue;
            miResetSetting.Text = Resources.GameSettings_Reset_Setting_Button;
            miCopySetting.Enabled = settingWithValue;
            miCopySetting.Text = Resources.GameSettings_Copy_Setting_Button;
            miCopyAllSettings.Text = Resources.GameSettings_Copy_All_Settings_Button;
            miChangedOnly.Text = Resources.GameSettings_Show_Only_Changed_Button;
        }

        private void miResetSetting_Click(object sender, EventArgs e)
        {
            if (cmGameSetting.SourceControl is ISettingControl setting)
            {
                setting.ClearValue();
            }
        }

        private void miCopySetting_Click(object sender, EventArgs e)
        {
            if (cmGameSetting.SourceControl is ISettingControl setting)
            {
                var cfgData = new CfgData();
                cfgData.AddCommentRow(setting.Model.Name);
                cfgData.AddOrUpdateRow(setting.Model.Key, setting.Value);
                Clipboard.SetText(cfgData.ToString());
            }
        }

        private void miCopyAllSettings_Click(object sender, EventArgs e) =>
            Clipboard.SetText(_settingControls.ToCfgData(withComments:true).ToString());

        private void miChangedOnly_CheckedChanged(object sender, EventArgs e) => UpdateSettingsVisibility();

        private void toolTip_Popup(object sender, PopupEventArgs e)
        {
            if (e.AssociatedControl.Parent is ISettingControl setting)
            {
                toolTip.ToolTipTitle = setting.Model.Name;
            }
        }

        private async Task LoadDatabaseAsync()
        {
            var configDataLoadController = new ConfigDataLoadController(ConfigDataRepository.Loader);
            var configData = await configDataLoadController.LoadDatabaseAsync(this, Program.Settings.Language);
            if (configData != null)
            {
                _configData = configData;
                LoadSettingControls(configData);
            }
        }

        private void LoadSettingControls(ConfigData configData)
        {
            var settingControls = new List<ISettingControl>();
            tabCategories.SuspendLayout();
            tabCategories.Hide();
            tabCategories.TabPages.Clear();
            foreach (var category in configData.SettingCategories.Values)
            {
                tabCategories.TabPages.Add(SettingCategoryTabPage.Create(category, toolTip,
                    cmGameSetting, c => settingControls.Add(c)));
            }
            tabCategories.ResumeLayout();
            tabCategories.Show();
            foreach (var setting in _settingControls)
            {
                if (setting.HasValue)
                {
                    settingControls.UpdateByKey(setting.Model.Key, setting.Value);
                }
            }
            DisposableUtils.Dispose(_settingControls);
            _settingControls = settingControls;
            bool anyDataAvailable = settingControls.Count != 0;
            btnSave.Enabled = anyDataAvailable;
            btnResetAll.Enabled = anyDataAvailable;
            btnResetPage.Enabled = anyDataAvailable;
        }

        private CfgData GetCurrentConfigData() => _settingControls.ToCfgData();

        private void LoadGameSettings(CfgData cfgData)
        {
            var invalidSettings = _settingControls.LoadFrom(cfgData);
            invalidSettings.AddRange(_settingControls.GetUnsupportedSettings(cfgData));
            if (invalidSettings.Count != 0)
            {
                MessageBox.Show(this, $"{Resources.GameSettings_InvalidSettingFound_Text}\n{string.Join(",\n", invalidSettings)}",
                    Resources.GameSettings_InvalidSettingFound_Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ResetAtPageSettings() => SettingCategoryTabPage.GetSettings(tabCategories.SelectedTab).ClearValues();

        private void ResetGameSettings() => _settingControls.ClearValues();

        private void UpdateSettingsVisibility()
        {
            foreach (var setting in _settingControls)
            {
                setting.Control.Visible = !miChangedOnly.Checked || setting.HasValue;
            }
        }

        private void SetProfilesList(IEnumerable<string> profiles)
        {
            var profilesSource = cbProfiles.DataSource;
            if (profiles.Any())
                cbProfiles.DataSource = new BindingSource(profiles, null);
            else
                cbProfiles.DataSource = null;
            DisposableUtils.Dispose(profilesSource);
        }

        private bool IsValidAndAvailableProfileName(string value) =>
            ProfileManager.IsValidProfileName(value) && !_profileManager.Profiles.ContainsKey(value.Trim());

        private void SelectProfileByData(CfgData cfgData)
        {
            foreach (var profileEntry in _profileManager.Profiles)
            {
                if (profileEntry.Value.Equals(cfgData))
                {
                    _appliedProfileName = profileEntry.Key;
                    UpdateProfiles(profileEntry.Key);
                    break;
                }
            }
        }

        private DialogResult AskAndSaveProfileChanges(string profileName, bool withCancel = true)
        {
            var currentConfig = GetCurrentConfigData();
            if (_profileManager.Profiles.TryGetValue(profileName, out var profileData) &&
                !currentConfig.Equals(profileData))
            {
                var dialogResult = MessageBox.Show(this, string.Format(Resources.GameSettings_ProfileAskSave_Text, profileName),
                    Resources.GameSettings_ProfileAskSave_Title, withCancel ? MessageBoxButtons.YesNoCancel : MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, withCancel ? MessageBoxDefaultButton.Button3 : MessageBoxDefaultButton.Button2);
                switch (dialogResult)
                {
                    case DialogResult.Yes:
                        if (!_profileManager.SaveProfile(profileName, currentConfig))
                        {
                            MessageBox.Show(this, Resources.GameSettings_ProfileError_Text,
                                Resources.GameSettings_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return DialogResult.Cancel;
                        }
                        return DialogResult.Yes;
                    case DialogResult.No:
                        return DialogResult.No;
                    case DialogResult.Cancel:
                    default:
                        return DialogResult.Cancel;
                }
            }
            return DialogResult.Yes;
        }
    }
}
