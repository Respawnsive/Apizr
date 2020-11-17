using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apizr.Extending
{
    public static class DictionaryExtensions
    {
        public static string ToString(this IDictionary<string, string> source, string keyValueSeparator, string sequenceSeparator)
        {
            if (source == null)
                throw new ArgumentException("Parameter source can not be null.");

            var pairs = source.Select(x => $"{x.Key}{keyValueSeparator}{x.Value}");

            return string.Join(sequenceSeparator, pairs);
        }
    }
}
