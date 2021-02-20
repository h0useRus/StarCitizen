using System.Windows.Forms;
using NSW.StarCitizen.Tools.Helpers;

namespace NSW.StarCitizen.Tools.Controls
{
    public class TabControlEx : TabControl
    {
        private bool _inWndProc;

        protected override void WndProc(ref Message message)
        {
            if (message.Msg == WinApi.WM_MOUSEWHEEL)
            {
                WinApi.SendControlMessage(SelectedTab, ref message, ref _inWndProc);
            }
            base.WndProc(ref message);
        }
    }
}
