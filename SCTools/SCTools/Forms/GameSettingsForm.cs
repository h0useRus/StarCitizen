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
            Text = "Star Citizen : Settings" + " - " + _gameInfo.Mode;

            await LoadDatabaseAsync();
        }

        private void btnSave_Click(object sender, EventArgs e) => SaveGameSettings();

        private void btnResetAll_Click(object sender, EventArgs e) => ResetGameSettings();

        private void btnResetPage_Click(object sender, EventArgs e)
        {
            foreach (var layoutPanel in tabCategories.SelectedTab.Controls.OfType<TableLayoutPanel>())
            {
                foreach (var settingControl in layoutPanel.Controls.OfType<ISettingControl>())
                {
                    settingControl.ClearValue();
                }
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
