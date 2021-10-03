using System;
using System.Windows.Forms;

namespace NSW.StarCitizen.Tools.Helpers
{
    public sealed class AppMessageFilter : IMessageFilter
    {
        public int ShowFirstInstanceMsg { get; set; } = WinApi.WM_NULL;
        public Action? OnRestoreInstance { get; set; }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == ShowFirstInstanceMsg && ShowFirstInstanceMsg != WinApi.WM_NULL)
            {
                OnRestoreInstance?.Invoke();
                return true;
            }
            return false;
        }
    }
}
