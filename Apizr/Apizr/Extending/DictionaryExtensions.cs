using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apizr.Extending
{
    public static class DictionaryExtensions
    {
        public static string ToString(this IDictionary source, string keyValueSeparator, string sequenceSeparator)
        {
            if (source == null)
                throw new ArgumentException("Parameter source can not be null.");

            var pairs = new List<string>();
            foreach (DictionaryEntry entry in source) 
                pairs.Add($"{entry.Key}{keyValueSeparator}{entry.Value}");

            return string.Join(sequenceSeparator, pairs);
        }
    }
}
