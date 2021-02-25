using System;
using System.Collections.Generic;
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

        public static void LoadFrom(this IEnumerable<ISettingControl> settingControls, CfgData cfgData)
        {
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
                    }
                }
                else
                {
                    control.ClearValue();
                }
            }
        }
    }
}
