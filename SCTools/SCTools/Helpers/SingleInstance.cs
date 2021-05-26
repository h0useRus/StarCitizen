using System;
using System.Threading;

namespace NSW.StarCitizen.Tools.Helpers
{
    public static class SingleInstance
    {
        public static readonly int WM_SHOWFIRSTINSTANCE = WinApi.RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", Program.ApplicationId);
        public static readonly IntPtr LP_NONE = IntPtr.Zero;
        public static readonly IntPtr LP_UPDATE_OUTDATED = (IntPtr)1;
        private static Mutex? _mutex;
        public static bool Start()
        {
            string mutexName = $"Local\\{Program.ApplicationId}";

            // if you want your app to be limited to a single instance
            // across ALL SESSIONS (multiple users & terminal services), then use the following line instead:
            // string mutexName = String.Format("Global\\{0}", ProgramInfo.AssemblyGuid);

            _mutex = new Mutex(true, mutexName, out var onlyInstance);
            return onlyInstance;
        }
        public static void ShowFirstInstance(bool updateOutdatedLocalization) =>
            WinApi.SendMessage((IntPtr)WinApi.HWND_BROADCAST, WM_SHOWFIRSTINSTANCE, IntPtr.Zero,
                updateOutdatedLocalization ? LP_UPDATE_OUTDATED : LP_NONE);
        public static void Stop() => _mutex?.ReleaseMutex();
    }
}