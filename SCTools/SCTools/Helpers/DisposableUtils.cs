using System;
using System.Collections;

namespace NSW.StarCitizen.Tools.Helpers
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

    public class DynamicDisposable<T> : IDisposable
    {
        public T Object { get; }

        public static DynamicDisposable<T> Create(T obj) => (obj != null) ? new DynamicDisposable<T>(obj) : null;

        public DynamicDisposable(T obj)
        {
            Object = obj;
        }

        public void Dispose() => DisposableUtils.Dispose(Object);
    }
}
