using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Defter.StarCitizen.ConfigDB.Model;
using NLog;

namespace NSW.StarCitizen.Tools.Controls
{
    public static class SettingCategoryTabPage
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static TabPage Create(SettingCategory category, ToolTip toolTip, ContextMenuStrip contextMenu,
            Action<ISettingControl> settingControlFunc)
        {
            var tabPage = new TabPage(category.Name)
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
                    settingControl.Control.ContextMenuStrip = contextMenu;
                    layout.Controls.Add(settingControl.Control);
                    settingControlFunc.Invoke(settingControl);
                }
            }
            tabPage.Controls.Add(layout);
            return tabPage;
        }

        public static IEnumerable<ISettingControl> GetSettings(TabPage tabPage)
        {
            foreach (var layoutPanel in tabPage.Controls.OfType<TableLayoutPanel>())
            {
                foreach (var control in layoutPanel.Controls)
                {
                    if (control is ISettingControl settingControl)
                    {
                        yield return settingControl;
                    }
                }
            }
        }

        public static bool IsHasChangedSettings(TabPage tabPage)
        {
            foreach (var layoutPanel in tabPage.Controls.OfType<TableLayoutPanel>())
            {
                if (layoutPanel.Controls.OfType<ISettingControl>().Any(s => s.HasValue))
                {
                    return true;
                }
            }
            return false;
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
                    return new ComboboxIntSetting(toolTip, integerSetting);
                }
                if (setting is FloatSetting floatSetting)
                {
                    if (floatSetting.Range)
                    {
                        return new NumericFloatSetting(toolTip, floatSetting);
                    }
                    return new ComboboxFloatSetting(toolTip, floatSetting);
                }
            }
            catch (Exception e)
            {
                _logger.Warn(e, $"Ignore invalid setting: {setting.Key}");
                return null;
            }
            _logger.Info($"Ignore not supported setting: {setting.Key}");
            return null;
        }
    }
}
