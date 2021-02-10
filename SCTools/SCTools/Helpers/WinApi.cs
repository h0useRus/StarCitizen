using System;
using System.Runtime.InteropServices;

namespace NSW.StarCitizen.Tools.Helpers
{
    public static class WinApi
    {
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);

        public static int RegisterWindowMessage(string format, params object[] args)
        {
            var message = string.Format(format, args);
            return RegisterWindowMessage(message);
        }

        public const int HWND_BROADCAST = 0xffff;
        public const int SW_SHOWNORMAL = 1;
        public const int WM_MOUSEWHEEL = 0x20a;

        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32")]
        public static extern bool SendMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32")]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        public static void ShowToFront(IntPtr window)
        {
            SetForegroundWindow(window);
            BringWindowToTop(window);
        }
    }
}