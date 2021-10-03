using System;
using System.Threading;

namespace NSW.StarCitizen.Tools.Helpers
{
    public sealed class SingleInstance : IDisposable
    {
        private readonly string _applicationId;
        private readonly int _showFirstInstanceMsg;
        private Mutex? _mutex;

        public int ShowFirstInstanceMsg => _showFirstInstanceMsg;

        public SingleInstance(string applicationId)
        {
            _applicationId = applicationId;
            _showFirstInstanceMsg = WinApi.RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", applicationId);
#if DEBUG
            if (_showFirstInstanceMsg == WinApi.WM_NULL)
                throw new InvalidOperationException("Unable create single instance show window message");
#endif
        }

        public void Dispose()
        {
            if (_mutex != null)
            {
                _mutex.Dispose();
                _mutex = null;
            }
        }

        public bool Start(bool acrossAllUsers)
        {
            if (_mutex != null)
                throw new InvalidOperationException("Can't start single instance twice");
            string mutexName = acrossAllUsers ? $"Global\\{_applicationId}" : $"Local\\{_applicationId}";
            _mutex = new Mutex(true, mutexName, out var onlyInstance);
            return onlyInstance;
        }

        public bool ShowFirstInstance()
        {
            if (_showFirstInstanceMsg != WinApi.WM_NULL)
            {
                return WinApi.PostMessage((IntPtr)WinApi.HWND_BROADCAST, _showFirstInstanceMsg, IntPtr.Zero, IntPtr.Zero);
            }
            return false;
        }

        public void Stop()
        {
            if (_mutex != null)
            {
                _mutex.ReleaseMutex();
                _mutex.Dispose();
                _mutex = null;
            }
        }
    }
}