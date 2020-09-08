using System;
using System.Collections.Generic;
using System.Linq;

namespace localtour.Helpers
{
    public static class DistinctHelper
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
                                this IEnumerable<TSource> source,
                                Func<TSource, TKey> keySelector)
        {
            return source.GroupBy(keySelector).Select(i => i.First());
        }
    }
}
