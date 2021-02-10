using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private IReadOnlyList<ISettingControl> _settingControls = new List<ISettingControl>();
        private ConfigData _configData;

        public GameSettingsForm(GameInfo gameInfo, ConfigData configData)
        {
            _gameInfo = gameInfo;
            _configData = configData;
            _gameSettings = new GameSettings(gameInfo);
            InitializeComponent();
            UpdateLocalizedControlsOnly();
        }

        private void GameSettingsForm_Load(object sender, EventArgs e) => LoadSettingControls(_configData);

        public async void UpdateLocalizedControls()
        {
            UpdateLocalizedControlsOnly();
            await LoadDatabaseAsync();
        }

        private void UpdateLocalizedControlsOnly()
        {
            Text = Resources.GameSettings_Title + " - " + _gameInfo.Mode;
            btnResetAll.Text = Resources.GameSettings_Reset_All_Button;
            btnResetPage.Text = Resources.GameSettings_Reset_Page_Button;
            btnSave.Text = Resources.GameSettings_Save_Button;
        }

        private void btnSave_Click(object sender, EventArgs e) => SaveGameSettings();

        private void btnResetAll_Click(object sender, EventArgs e) => ResetGameSettings();

        private void btnResetPage_Click(object sender, EventArgs e) => ResetAtPageSettings();

        private void cmGameSetting_Opened(object sender, EventArgs e)
        {
            miResetSetting.Enabled = cmGameSetting.SourceControl is ISettingControl setting && setting.HasValue;
            miResetSetting.Text = Resources.GameSettings_Reset_Setting_Button;
            miCopySetting.Enabled = cmGameSetting.SourceControl is ISettingControl;
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
                Clipboard.SetText($"{setting.Key}={setting.Value}");
            }
        }

        private void miCopyAllSettings_Click(object sender, EventArgs e)
        {
            StringBuilder content = new StringBuilder();
            foreach (var setting in _settingControls)
            {
                if (setting.HasValue)
                {
                    content.AppendLine($"{setting.Key}={setting.Value}");
                }
            }
            Clipboard.SetText(content.ToString());
        }

        private void miChangedOnly_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var setting in _settingControls)
            {
                setting.Control.Visible = !miChangedOnly.Checked || setting.HasValue;
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
            tabCategories.SuspendLayout();
            tabCategories.TabPages.Clear();
            var settingControls = new List<ISettingControl>();
            foreach (var category in configData.SettingCategories.Values)
            {
                TabPage newPage = new TabPage(category.Name)
                {
                    AutoScroll = true
                };
                TableLayoutPanel layout = new TableLayoutPanel
                {
                    Dock = DockStyle.Top,
                    ColumnCount = 1,
                    AutoSize = true
                };
                foreach (var setting in category.Settings.Values)
                {
                    var settingControl = CreateSettingControl(toolTip, setting);
                    if (settingControl != null)
                    {
                        settingControl.Control.Dock = DockStyle.Fill;
                        settingControl.Control.ContextMenuStrip = cmGameSetting;
                        settingControls.Add(settingControl);
                        layout.Controls.Add(settingControl.Control);
                    }
                }
                newPage.Controls.Add(layout);
                tabCategories.TabPages.Add(newPage);
            }
            tabCategories.ResumeLayout();
            _settingControls = settingControls;
            bool anyDataAvailable = settingControls.Count != 0;
            btnSave.Enabled = anyDataAvailable;
            btnResetAll.Enabled = anyDataAvailable;
            btnResetPage.Enabled = anyDataAvailable;
            LoadGameSettings();
        }

        private void ResetAtPageSettings()
        {
            foreach (var layoutPanel in tabCategories.SelectedTab.Controls.OfType<TableLayoutPanel>())
            {
                foreach (var settingControl in layoutPanel.Controls.OfType<ISettingControl>())
                {
                    settingControl.ClearValue();
                }
            }
        }

        private void ResetGameSettings()
        {
            foreach (var control in _settingControls)
            {
                control.ClearValue();
            }
        }

        private void LoadGameSettings()
        {
            var cfgData = _gameSettings.Load();
            foreach (var control in _settingControls)
            {
                if (cfgData.TryGetValue(control.Key, out var value) && value != null)
                {
                    try
                    {
                        control.Value = value;
                    }
                    catch
                    {
                        // if value corrupted reset it to default
                        control.ClearValue();
                    }
                }
                else
                {
                    control.ClearValue();
                }
            }
        }

        private bool SaveGameSettings()
        {
            var cfgData = new CfgData();
            foreach (var control in _settingControls)
            {
                if (control.HasValue)
                {
                    cfgData.AddOrUpdateRow(control.Key, control.Value);
                }
            }
            return _gameSettings.SaveConfig(cfgData);
        }

        private static ISettingControl? CreateSettingControl(ToolTip toolTip, BaseSetting setting)
        {
            try
            {
                if (setting is BooleanSetting booleanSetting)
                {
                    return new CheckboxSetting(toolTip, booleanSetting);
                }
                if (setting is IntegerSetting integerSetting)
                {
                    if (integerSetting.Range)
                    {
                        return new NumericIntSetting(toolTip, integerSetting);
                    }
                    return new ComboboxSetting(toolTip, integerSetting);
                }
                if (setting is FloatSetting floatSetting)
                {
                    return new NumericFloatSetting(toolTip, floatSetting);
                }
            }
            catch
            {
                // skip invalid settings
            }
            return null;
        }
    }
}
