using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using NSW.StarCitizen.Tools.Controls;
using NSW.StarCitizen.Tools.Lib.Helpers;

namespace NSW.StarCitizen.Tools.Helpers
{
    public static class SettingControlListExtensions
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static CfgData ToCfgData(this IEnumerable<ISettingControl> settingControls, bool withComments = false)
        {
            var cfgData = new CfgData();
            foreach (var control in settingControls)
            {
                if (control.HasValue)
                {
                    if (withComments)
                        cfgData.AddCommentRow(control.Model.Name);
                    cfgData.AddOrUpdateRow(control.Model.Key, control.Value);
                }
            }
            return cfgData;
        }

        public static void ClearValues(this IEnumerable<ISettingControl> settingControls)
        {
            foreach (var control in settingControls)
            {
                control.ClearValue();
            }
        }

        public static bool UpdateByKey(this IEnumerable<ISettingControl> settingControls, string key, string value)
        {
            var setting = settingControls.FirstOrDefault(s => s.Model.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
            if (setting != null)
            {
                try
                {
                    setting.Value = value;
                }
                catch (Exception e)
                {
                    _logger.Warn(e, $"Invalid setting value {setting.Model.Key}={value}");
                    return false;
                }
                return true;
            }
            return false;
        }

        public static List<string> LoadFrom(this IEnumerable<ISettingControl> settingControls, CfgData cfgData)
        {
            var invalidSettings = new List<string>();
            foreach (var control in settingControls)
            {
                if (cfgData.TryGetValue(control.Model.Key, out var value) && value != null)
                {
                    try
                    {
                        control.Value = value;
                    }
                    catch (Exception e)
                    {
                        _logger.Warn(e, $"Invalid setting value {control.Model.Key}={value}. Reset to default");
                        // if value corrupted reset it to default
                        control.ClearValue();
                        invalidSettings.Add(control.Model.Key);
                    }
                }
                else
                {
                    control.ClearValue();
                }
            }
            return invalidSettings;
        }

        public static ISet<string> GetUnsupportedSettings(this IEnumerable<ISettingControl> settingControls, CfgData cfgData)
        {
            var unsupportedSettings = new HashSet<string>(cfgData.ToDictionary().Keys, StringComparer.OrdinalIgnoreCase);
            if (unsupportedSettings.Count != 0)
            {
                foreach (var control in settingControls)
                {
                    if (unsupportedSettings.Remove(control.Model.Key) && unsupportedSettings.Count == 0)
                    {
                        break;
                    }
                }
            }
            return unsupportedSettings;
        }
    }
}
