using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Apizr.Extending
{
    public static class EnumerableExtensions
    {
        public static LogLevel Low(this IEnumerable<LogLevel> enumerable)
        {
            return enumerable.OrderBy(x => x).First();
        }

        public static LogLevel Medium(this IEnumerable<LogLevel> enumerable)
        {
            var list = enumerable.ToList();
            if (list.Count < 2)
                return list.First();

            var mediumIndex = (list.Count - 1) / 2;
            return list.OrderBy(x => x).ElementAt(mediumIndex);
        }

        public static LogLevel High(this IEnumerable<LogLevel> enumerable)
        {
            return enumerable.OrderBy(x => x).Last();
        }
    }
}
