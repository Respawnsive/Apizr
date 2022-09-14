using System;
using System.Collections;
using System.Collections.Generic;

namespace Apizr.Extending
{
    internal static class DictionaryExtensions
    {
        internal static string ToString(this IDictionary source, string keyValueSeparator, string sequenceSeparator)
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
