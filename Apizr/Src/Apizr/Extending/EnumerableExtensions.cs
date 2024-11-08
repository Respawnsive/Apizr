using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Apizr.Extending
{
    /// <summary>
    /// Some enumerable extensions
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Return the lowest value
        /// </summary>
        /// <param name="enumerable">The source enumerable</param>
        /// <returns></returns>
        public static LogLevel Low(this IEnumerable<LogLevel> enumerable)
        {
            return enumerable?.OrderBy(x => x).FirstOrDefault() ?? LogLevel.None;
        }

        /// <summary>
        /// Return the closest value to the middle
        /// </summary>
        /// <param name="enumerable">The source enumerable</param>
        /// <returns></returns>
        public static LogLevel Medium(this IEnumerable<LogLevel> enumerable)
        {
            var list = enumerable?.ToList();
            if (list == null || list.Count == 0)
                return LogLevel.None;
            if (list.Count == 1)
                return list.First();

            var mediumIndex = (list.Count - 1) / 2;
            return list.OrderBy(x => x).ElementAt(mediumIndex);
        }

        /// <summary>
        /// Return the highest value
        /// </summary>
        /// <param name="enumerable">The source enumerable</param>
        /// <returns></returns>
        public static LogLevel High(this IEnumerable<LogLevel> enumerable)
        {
            return enumerable?.OrderBy(x => x).LastOrDefault() ?? LogLevel.None;
        }
    }
}
