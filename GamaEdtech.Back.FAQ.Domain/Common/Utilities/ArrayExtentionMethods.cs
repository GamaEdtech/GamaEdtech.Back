namespace GamaEdtech.Back.FAQ.Domain.Common.Utilities
{
    public static class ArrayExtentionMethods
    {
        public static int[] GetStringIndex(this string[] source, string findString)
        {
            var findedArray = new List<int>();
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i]?.Split(':')?[0] == findString)
                {
                    findedArray.Add(i);
                }
            }
            return findedArray.ToArray();
        }
        public static T[] Slice<T>(this T[] source, int from, int to)
        {
            if (to < from) throw new ArgumentOutOfRangeException(nameof(to));

            int lenght = to - from + 1;
            T[] slice = new T[lenght];

            for (int i = from, j = 0; i <= to; i++, j++)
            {
                slice[j] = source[i];
            }

            return slice;
        }
    }
}
