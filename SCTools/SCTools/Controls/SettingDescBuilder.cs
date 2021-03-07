using System.Linq;
using System.Text;
using Defter.StarCitizen.ConfigDB.Model;
using NSW.StarCitizen.Tools.Properties;

namespace NSW.StarCitizen.Tools.Controls
{
    public static class SettingDescBuilder
    {
        public static string Build(BooleanSetting setting)
        {
            StringBuilder builder = new StringBuilder();
            if (setting.Description != null)
            {
                builder.AppendLine(setting.Description);
                builder.AppendLine();
            }
            builder.AppendLine(setting.Key);
            if (setting.DefaultValue.HasValue)
            {
                builder.AppendLine();
                var realDefaultValue = setting.DefaultValue.Value ? "1" : "0";
                builder.AppendLine($"{Resources.GameSettings_DefaultValue_Text}: {realDefaultValue}");
            }
            return builder.ToString();
        }

        public static string Build(IntegerSetting setting)
        {
            StringBuilder builder = new StringBuilder();
            if (setting.Description != null)
            {
                builder.AppendLine(setting.Description);
                builder.AppendLine();
            }
            builder.AppendLine(setting.Key);
            builder.AppendLine();
            if (setting.DefaultValue.HasValue)
            {
                builder.AppendLine($"{Resources.GameSettings_DefaultValue_Text}: {setting.DefaultValue.Value}");
            }
            builder.AppendLine($"{Resources.GameSettings_MinValue_Text}: {setting.MinValue}");
            builder.AppendLine($"{Resources.GameSettings_MaxValue_Text}: {setting.MaxValue}");
            if (setting.Step.HasValue)
            {
                builder.AppendLine($"{Resources.GameSettings_Step_Text}: {setting.Step.Value}");
            }
            if (setting.LabeledValues.Any())
            {
                builder.AppendLine();
                builder.AppendLine($"{Resources.GameSettings_Values_Text}:");
                foreach (var pair in setting.LabeledValues)
                {
                    builder.AppendLine($"{pair.Value} - {pair.Key}");
                }
            }
            return builder.ToString();
        }

        public static string Build(FloatSetting setting)
        {
            StringBuilder builder = new StringBuilder();
            if (setting.Description != null)
            {
                builder.AppendLine(setting.Description);
                builder.AppendLine();
            }
            builder.AppendLine(setting.Key);
            builder.AppendLine();
            if (setting.DefaultValue.HasValue)
            {
                builder.AppendLine($"{Resources.GameSettings_DefaultValue_Text}: {setting.DefaultValue.Value}");
            }
            builder.AppendLine($"{Resources.GameSettings_MinValue_Text}: {setting.MinValue}");
            builder.AppendLine($"{Resources.GameSettings_MaxValue_Text}: {setting.MaxValue}");
            if (setting.Step.HasValue)
            {
                builder.AppendLine($"{Resources.GameSettings_Step_Text}: {setting.Step.Value}");
            }
            if (setting.LabeledValues.Any())
            {
                builder.AppendLine();
                builder.AppendLine($"{Resources.GameSettings_Values_Text}:");
                foreach (var pair in setting.LabeledValues)
                {
                    builder.AppendLine($"{pair.Value} - {pair.Key}");
                }
            }
            return builder.ToString();
        }
    }
}
