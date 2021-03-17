using System.Windows.Forms;
using Defter.StarCitizen.ConfigDB.Model;

namespace NSW.StarCitizen.Tools.Controls
{
    public interface ISettingControl
    {
        public Control Control { get; }
        public BaseSetting Model { get; }
        public string Value { get; set; }
        public bool HasValue { get; }
        public void ClearValue();
    }
}
