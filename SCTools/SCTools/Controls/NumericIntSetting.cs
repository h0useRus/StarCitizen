using System;
using System.Drawing;
using System.Windows.Forms;
using Defter.StarCitizen.ConfigDB.Model;

namespace NSW.StarCitizen.Tools.Controls
{
    public partial class NumericIntSetting : UserControl, ISettingControl
    {
        public Control Control => this;
        public string Key => Setting.Key;
        public string Value
        {
            get => numControl.Value.ToString();
            set
            {
                numControl.Value = int.Parse(value);
                UpdateValueText();
            }
        }
        public bool HasValue
        {
            get
            {
                if (Setting.DefaultValue.HasValue)
                {
                    return numControl.Value != Setting.DefaultValue.Value;
                }
                return !string.IsNullOrEmpty(numControl.Text);
            }
        }
        public IntegerSetting Setting { get; }

        public NumericIntSetting(ToolTip toolTip, IntegerSetting setting)
        {
            Setting = setting;
            InitializeComponent();
            lblCaption.Text = setting.Name;
            numControl.Minimum = setting.MinValue;
            numControl.Maximum = setting.MaxValue;
            ClearValue();
            UpdateValueText();
            if (setting.Description != null)
            {
                toolTip.SetToolTip(lblCaption, setting.Description);
            }
        }

        public void ClearValue()
        {
            if (Setting.DefaultValue.HasValue)
            {
                numControl.Value = Setting.DefaultValue.Value;
            }
            else
            {
                numControl.ResetText();
            }
        }

        private void numControl_ValueChanged(object sender, EventArgs e) => UpdateValueText();

        private void numControl_TextChanged(object sender, EventArgs e) => UpdateValueText();

        private void UpdateValueText()
        {
            if ((!string.IsNullOrEmpty(numControl.Text) || Setting.DefaultValue.HasValue) &&
                Setting.Values.TryGetValue((int)numControl.Value, out var valueName) &&
                !numControl.Value.ToString().Equals(valueName))
            {
                lblValue.Text = valueName;
            }
            else
            {
                lblValue.Text = string.Empty;
            }
            BackColor = HasValue ? SystemColors.ControlDark : SystemColors.Control;
        }

        private void NumericIntSetting_DoubleClick(object sender, EventArgs e) => ClearValue();
    }
}
