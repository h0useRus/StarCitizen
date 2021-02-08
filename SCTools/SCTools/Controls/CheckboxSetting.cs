using System.Drawing;
using System.Windows.Forms;
using Defter.StarCitizen.ConfigDB.Model;

namespace NSW.StarCitizen.Tools.Controls
{
    public partial class CheckboxSetting : UserControl, ISettingControl
    {
        public Control Control => this;
        public string Key => Setting.Key;
        public string Value
        {
            get => Checked ? "1" : "0";
            set => Checked = !value.Equals("0");
        }
        public bool HasValue => cbValue.CheckState != GetCheckStateFromValue(Setting.DefaultValue);
        public BooleanSetting Setting { get; }

        public override string Text
        {
            get => lblCaption.Text;
            set => lblCaption.Text = value;
        }

        public bool Checked
        {
            get => cbValue.Checked;
            set => cbValue.Checked = value;
        }

        public CheckboxSetting(ToolTip toolTip, BooleanSetting setting)
        {
            Setting = setting;
            InitializeComponent();
            lblCaption.Text = setting.Name;
            cbValue.ThreeState = !setting.DefaultValue.HasValue;
            cbValue.CheckState = GetCheckStateFromValue(setting.DefaultValue);
            if (setting.Description != null)
            {
                toolTip.SetToolTip(lblCaption, setting.Description);
            }
        }

        public void ClearValue() => cbValue.CheckState = GetCheckStateFromValue(Setting.DefaultValue);

        private void cbValue_CheckStateChanged(object sender, System.EventArgs e) =>
            BackColor = HasValue ? SystemColors.ControlDark : SystemColors.Control;

        private static CheckState GetCheckStateFromValue(bool? value)
        {
            if (value.HasValue)
            {
                return value.Value ? CheckState.Checked : CheckState.Unchecked;
            }
            return CheckState.Indeterminate;
        }

        private void CheckboxSetting_DoubleClick(object sender, System.EventArgs e) => ClearValue();
    }
}
