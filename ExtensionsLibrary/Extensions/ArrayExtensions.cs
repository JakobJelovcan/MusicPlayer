using ExtensionsLibrary.Exceptions;

namespace ExtensionsLibrary.Extensions
{
    public static class ArrayExtensions
    {
        public static int IndexOf<T>(this T[] array, T[] target, int startIndex)
        {
            if (target.Length < 1 || array.Length < target.Length + startIndex) throw new ArraySizeException();
            int matches;
            for (int i = startIndex; i < array.Length - target.Length; i++)
            {
                matches = 0;
                for (int j = 0; j < target.Length; j++)
                {
                    if (array[i + j]?.Equals(target[j]) == true) matches++;
                    else break;
                }
                if (matches == target.Length) return i;
            }
            return -1;
        }

        public static int IndexOf<T>(this T[] array, T[] target, int startIndex, int count)
        {
            if (target.Length < 1 || array.Length < target.Length + startIndex) throw new ArraySizeException();
            int matches;
            for (int i = startIndex; i < startIndex + count - target.Length; i++)
            {
                matches = 0;
                for (int j = 0; j < target.Length; j++)
                {
                    if (array[i + j]?.Equals(target[j]) == true) matches++;
                    else break;
                }
                if (matches == target.Length) return i;
            }
            return startIndex + count;
        }
    }
}