using System.Windows.Forms;

namespace NSW.StarCitizen.Tools.Controls
{
    public interface ISettingControl
    {
        public Control Control { get; }
        public string Key { get; }
        public string Value { get; set; }
        public bool HasValue { get; }
        public void ClearValue();
    }
}
