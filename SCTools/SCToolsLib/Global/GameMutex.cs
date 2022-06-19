using System;

namespace NSW.StarCitizen.Tools.Lib.Global
{
    public sealed class GameMutex : IDisposable
    {
        private const string GameMutexName = "StarCitizenApplication";
        private const string CoreMutexName = "StarCitizenModdingCore";
        private readonly MutexWrapper _gameMutex = new MutexWrapper(GameMutexName);
        private readonly MutexWrapper _coreMutex = new MutexWrapper(CoreMutexName);

        public bool TryAcquire()
        {
            try
            {
                if (_gameMutex.TryAcquire() &&
                    _coreMutex.TryAcquire())
                {
                    return true;
                }
            }
            catch
            {
                Release();
                throw;
            }
            Release();
            return false;
        }

        public void Release()
        {
            _gameMutex.Release();
            _coreMutex.Release();
        }

        public void Dispose() => Release();
    }
}
