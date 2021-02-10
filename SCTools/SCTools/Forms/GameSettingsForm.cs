using System;
using System.Collections.Generic;
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
        private IReadOnlyList<ISettingControl> _settingControls = new List<ISettingControl>();
        private ConfigData _configData;

        public GameSettingsForm(GameInfo gameInfo, ConfigData configData)
        {
            _gameInfo = gameInfo;
            _configData = configData;
            _gameSettings = new GameSettings(gameInfo);
            InitializeComponent();
            UpdateLocalizedControls();
        }

        private void GameSettingsForm_Load(object sender, EventArgs e) => LoadSettingControls(_configData);

        public async void UpdateLocalizedControls()
        {
            Text = Resources.GameSettings_Title + " - " + _gameInfo.Mode;
            btnResetAll.Text = Resources.GameSettings_Reset_All_Button;
            btnResetPage.Text = Resources.GameSettings_Reset_Page_Button;
            btnSave.Text = Resources.GameSettings_Save_Button;

            await LoadDatabaseAsync();
        }

        private void btnSave_Click(object sender, EventArgs e) => SaveGameSettings();

        private void btnResetAll_Click(object sender, EventArgs e) => ResetGameSettings();

        private void btnResetPage_Click(object sender, EventArgs e) => ResetAtPageSettings();

        private void cmGameSetting_Opened(object sender, EventArgs e)
        {
            miResetSelected.Enabled = cmGameSetting.SourceControl is ISettingControl setting && setting.HasValue;
            miResetSelected.Text = Resources.GameSettings_Reset_Selected_Button;
            miResetAll.Text = Resources.GameSettings_Reset_All_Button;
            miResetAtPage.Text = Resources.GameSettings_Reset_Page_Button;
            miChangedOnly.Text = Resources.GameSettings_Only_Changed_Button;
        }

        private void miResetSelected_Click(object sender, EventArgs e)
        {
            if (cmGameSetting.SourceControl is ISettingControl setting)
            {
                setting.ClearValue();
            }
        }

        private void miResetAtPage_Click(object sender, EventArgs e) => ResetAtPageSettings();

        private void miResetAll_Click(object sender, EventArgs e) => ResetGameSettings();

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
