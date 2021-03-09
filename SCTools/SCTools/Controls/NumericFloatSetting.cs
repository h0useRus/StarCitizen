using System;
using System.Drawing;
using System.Windows.Forms;
using Defter.StarCitizen.ConfigDB.Model;

namespace NSW.StarCitizen.Tools.Controls
{
    public partial class NumericFloatSetting : UserControl, ISettingControl
    {
        public Control Control => this;
        public BaseSetting Model => Setting;
        public string Value
        {
            get => numControl.Value.ToString();
            set
            {
                numControl.Value = decimal.Parse(value);
                UpdateValueText();
            }
        }
        public bool HasValue
        {
            get
            {
                if (Setting.DefaultValue.HasValue)
                {
                    return (float)numControl.Value != Setting.DefaultValue.Value;
                }
                return !string.IsNullOrEmpty(numControl.Text);
            }
        }
        public FloatSetting Setting { get; }

        public NumericFloatSetting(ToolTip toolTip, FloatSetting setting)
        {
            Setting = setting;
            InitializeComponent();
            lblCaption.Text = setting.Name;
            numControl.Minimum = (decimal)setting.MinValue;
            numControl.Maximum = (decimal)setting.MaxValue;
            if (setting.Step.HasValue)
            {
                InitStep(numControl, setting.Step.Value);
            }
            else
            {
                InitStep(numControl, Math.Min((setting.MaxValue - setting.MinValue) / 100.0f, 1.0f));
            }
            ClearValue();
            UpdateValueText();
            toolTip.SetToolTip(lblCaption, SettingDescBuilder.Build(setting));
        }

        public void ClearValue()
        {
            if (Setting.DefaultValue.HasValue)
            {
                numControl.Value = (decimal)Setting.DefaultValue.Value;
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
                Setting.Values.TryGetValue((float)numControl.Value, out var valueName) && valueName != null)
            {
                lblValue.Text = valueName;
            }
            else
            {
                lblValue.Text = string.Empty;
            }
            BackColor = HasValue ? SystemColors.ControlDark : SystemColors.Control;
        }

        private void NumericFloatSetting_DoubleClick(object sender, EventArgs e) => ClearValue();

        private static void InitStep(NumericUpDown numControl, float increment)
        {
            numControl.Increment = (decimal)increment;
            int decimalPlaces = 0;
            if (increment > 0)
            {
                for (float value = 1.0f; increment < value; value /= 10.0f)
                {
                    decimalPlaces++;
                }
            }
            numControl.DecimalPlaces = decimalPlaces;
        }
    }
}
