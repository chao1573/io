using System.Collections.Generic;
namespace Common.Utils
{
    public static class ListExtension
    {
        public static void SwapRemoveAt<T>(this List<T> list, int index)
        {
            list[index] = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
        }

        public static bool SwapRemove<T>(this List<T> list, T value)
        {
            int index = list.IndexOf(value);
            if (index == -1)
                return false;
            
            SwapRemoveAt(list, index);
            return true;
        }
    }
}