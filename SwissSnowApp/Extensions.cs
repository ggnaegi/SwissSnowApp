using System.Collections.Generic;
using System.Linq;

namespace SwissSnowApp
{
    public static class Extensions
    {
        /// <summary>
        /// Extension for splitting dtos list,
        /// since a queue message max size is set
        /// </summary>
        /// <param name="source"></param>
        /// <param name="subListSize"></param>
        /// <returns></returns>
        public static List<List<T>> Split<T>(this T[] source, int subListSize = 100)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / subListSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}
