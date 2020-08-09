using System;
using System.Collections.Generic;

namespace Utils
{
    public static class Shuffle
    {
        private static readonly Random Random = new Random();
        public static void ShuffleList<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = Random.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }


}
