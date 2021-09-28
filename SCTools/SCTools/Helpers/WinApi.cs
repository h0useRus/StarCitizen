using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;

namespace NSW.StarCitizen.Tools.Helpers
{
    public static class WinApi
    {
        public const int HWND_BROADCAST = 0xffff;
        public const int SW_SHOWNORMAL = 1;
        public const int WM_MOUSEWHEEL = 0x20a;

        public static int RegisterWindowMessage(string format, params object[] args)
        {
            var message = string.Format(CultureInfo.InvariantCulture, format, args);
            return UnsafeNativeMethods.RegisterWindowMessage(message);
        }

        public static bool SendMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam)
            => UnsafeNativeMethods.SendMessage(hwnd, msg, wparam, lparam) != IntPtr.Zero;

        public static void ShowToFront(IntPtr window)
        {
            UnsafeNativeMethods.SetForegroundWindow(window);
            UnsafeNativeMethods.BringWindowToTop(window);
        }

        public static bool SendControlMessage(Control control, ref Message message, ref bool isProcessing)
        {
            if (!isProcessing)
            {
                try
                {
                    isProcessing = true;
                    return UnsafeNativeMethods.SendMessage(control.Handle, message.Msg, message.WParam, message.LParam) != IntPtr.Zero;
                }
                finally
                {
                    isProcessing = false;
                }
            }
            return false;
        }
    }

    [SuppressUnmanagedCodeSecurity]
    internal static class UnsafeNativeMethods
    {
        [DllImport("user32", CharSet = CharSet.Unicode)]
        internal static extern int RegisterWindowMessage(string message);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32")]
        internal static extern IntPtr SendMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BringWindowToTop(IntPtr hWnd);
    }
}