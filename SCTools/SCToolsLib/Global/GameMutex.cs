using System;

namespace NSW.StarCitizen.Tools.Lib.Global
{
    public sealed class GameMutex : IDisposable
    {
        private const string GameMutexName = "StarCitizenApplication";
        private readonly MutexWrapper _gameMutex = new MutexWrapper(GameMutexName);

        public bool TryAcquire()
        {
            try
            {
                if (_gameMutex.TryAcquire())
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

        public void Release() => _gameMutex.Release();

        public void Dispose() => Release();
    }
}
