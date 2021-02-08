using System.Windows.Forms;

namespace NSW.StarCitizen.Tools.Controls
{
    public class AutoWidthCombobox : ComboBox
    {
        public AutoWidthCombobox() {}

        public new object DataSource
        {
            get => base.DataSource;
            set
            {
                base.DataSource = value;
                DetermineDropDownWidth();
            }
        }

        public new string DisplayMember
        {
            get => base.DisplayMember;
            set
            {
                base.DisplayMember = value;
                DetermineDropDownWidth();
            }
        }

        public new string ValueMember
        {
            get => base.ValueMember;
            set
            {
                base.ValueMember = value;
                DetermineDropDownWidth();
            }
        }

        private void DetermineDropDownWidth()
        {
            int widestStringInPixels = 0;
            foreach (object o in Items)
            {
                string toCheck;
                if (DisplayMember.CompareTo("") == 0)
                {
                    toCheck = o.ToString();
                }
                else
                {
                    var pinfo = o.GetType().GetProperty(DisplayMember);
                    toCheck = pinfo.GetValue(o, null).ToString();
                }
                if (TextRenderer.MeasureText(toCheck, Font).Width > widestStringInPixels)
                {
                    widestStringInPixels = TextRenderer.MeasureText(toCheck, Font).Width;
                }
            }
            Width = widestStringInPixels + 24;
            DropDownWidth = widestStringInPixels + 15;
        }
    }
}
