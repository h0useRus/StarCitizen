using System;
using System.Threading;

namespace NSW.StarCitizen.Tools.Lib.Global
{
    public sealed class MutexWrapper : IDisposable
    {
        private readonly string _mutexName;
        private Mutex? _mutex;

        public MutexWrapper(string mutexName)
        {
            if (string.IsNullOrWhiteSpace(mutexName))
                throw new ArgumentException(nameof(mutexName));
            _mutexName = mutexName;
        }

        public bool TryAcquire()
        {
            if (_mutex != null)
                throw new InvalidOperationException("Mutex already acquired");
            var mutex = new Mutex(true, _mutexName, out var onlyInstance);
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
