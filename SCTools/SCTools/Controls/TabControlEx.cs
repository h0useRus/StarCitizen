using System.Windows.Forms;
using NSW.StarCitizen.Tools.Helpers;

namespace NSW.StarCitizen.Tools.Controls
{
    public class TabControlEx : TabControl
    {
        private bool _inWndProc;

        protected override void WndProc(ref Message message)
        {
            if (message.Msg == WinApi.WM_MOUSEWHEEL && !_inWndProc)
            {
                _inWndProc = true;
                WinApi.SendMessage(SelectedTab.Handle, message.Msg, message.WParam, message.LParam);
                _inWndProc = false;
            }
            base.WndProc(ref message);
        }
    }
}
