using System.Drawing;
using System.Windows.Forms;
using Defter.StarCitizen.ConfigDB.Model;

namespace NSW.StarCitizen.Tools.Controls
{
    public partial class ComboboxFloatSetting : UserControl, ISettingControl
    {
        public Control Control => this;
        public BaseSetting Model => Setting;
        public string Value
        {
            get => SelectedValue.ToString();
            set => SelectedValue = float.Parse(value);
        }
        public bool HasValue
        {
            get
            {
                if (Setting.DefaultValue.HasValue)
                {
                    return SelectedValue != Setting.DefaultValue.Value;
                }
                return cbValue.SelectedValue != null;
            }
        }
        public FloatSetting Setting { get; }

        public override string Text
        {
            get => lblCaption.Text;
            set => lblCaption.Text = value;
        }

        public float SelectedValue
        {
            get => (float)cbValue.SelectedValue;
            set => cbValue.SelectedValue = value;
        }

        public ComboboxFloatSetting(ToolTip toolTip, FloatSetting setting)
        {
            Setting = setting;
            InitializeComponent();
            lblCaption.Text = setting.Name;
            cbValue.BindingContext = BindingContext;
            cbValue.DisplayMember = "Value";
            cbValue.ValueMember = "Key";
            cbValue.DataSource = new BindingSource(setting.Values, null);
            ClearValue();
            if (setting.Description != null)
            {
                toolTip.SetToolTip(lblCaption, setting.Description);
            }
        }

        public void ClearValue()
        {
            if (Setting.DefaultValue.HasValue)
            {
                cbValue.SelectedValue = Setting.DefaultValue.Value;
            }
            else
            {
                cbValue.SelectedIndex = -1;
                cbValue.SelectedItem = null;
            }
        }

        private void Combobox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            lblValue.Text = cbValue.SelectedValue != null ? cbValue.SelectedValue.ToString() : string.Empty;
            BackColor = HasValue ? SystemColors.ControlDark : SystemColors.Control;
        }

        private void ComboboxSetting_DoubleClick(object sender, System.EventArgs e) => ClearValue();
    }
}
