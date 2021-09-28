using System;
using System.Collections;

namespace NSW.StarCitizen.Tools.Lib.Helpers
{
    public static class DisposableUtils
    {
        public static void Dispose<T>(T obj)
        {
            if (obj != null)
            {
                if (obj is IDisposable disposableObj)
                {
                    disposableObj.Dispose();
                }
                else if (obj is IEnumerable enumerableObj)
                {
                    foreach (var elementObj in enumerableObj)
                    {
                        Dispose(elementObj);
                    }
                }
            }
        }
    }

    public sealed class DynamicDisposable<T> : IDisposable
    {
        public T Object { get; }

        public DynamicDisposable(T obj)
        {
            Object = obj;
        }

        public void Dispose()
        {
            DisposableUtils.Dispose(Object);
            GC.SuppressFinalize(this);
        }
    }
}
