using System;
using System.Threading;

namespace NSW.StarCitizen.Tools.Lib.Global
{
    public sealed class GameMutex : IDisposable
    {
        private const string MutexName = "StarCitizenApplication";
        private Mutex? _mutex;

        public bool TryAcquire()
        {
            if (_mutex != null)
                throw new InvalidOperationException("Game mutex already acquired");
            var mutex = new Mutex(true, MutexName, out var onlyInstance);
            if (!onlyInstance)
            {
                mutex.Dispose();
                return false;
            }
            _mutex = mutex;
            return true;
        }

        public void Release()
        {
            if (_mutex != null)
            {
                _mutex.ReleaseMutex();
                _mutex.Dispose();
                _mutex = null;
            }
        }

        public void Dispose() => Release();
    }
}
