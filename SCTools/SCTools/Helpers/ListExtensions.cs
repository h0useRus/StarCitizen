using System.Collections.Generic;

namespace NSW.StarCitizen.Tools.Helpers
{
    public static class ListExtensions
    {
        public static bool CanMoveStart<T>(this List<T> list, int index)
            => list.Count != 0 && index > 0 && index < list.Count;

        public static bool CanMoveEnd<T>(this List<T> list, int index)
            => list.Count != 0 && index >= 0 && index < list.Count - 1;

        public static bool MoveStart<T>(this List<T> list, int index)
        {
            if (CanMoveStart(list, index))
            {
                var value = list[index];
                list.RemoveAt(index);
                list.Insert(index - 1, value);
                return true;
            }
            return false;
        }

        public static bool MoveEnd<T>(this List<T> list, int index)
        {
            if (CanMoveEnd(list, index))
            {
                var value = list[index];
                list.RemoveAt(index);
                list.Insert(index + 1, value);
                return true;
            }
            return false;
        }
    }
}
