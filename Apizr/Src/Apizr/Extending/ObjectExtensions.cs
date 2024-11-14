using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Apizr.Extending
{
    internal static class ObjectExtensions
    {
        internal static string ToString(this object source, string keyValueSeparator, string sequenceSeparator, params string[] includedProperties)
        {
            if (source == null)
                throw new ArgumentException("Parameter source can not be null.");

            var includeAllProperties = includedProperties == null || includedProperties.Length == 0;
            var includedPropertiesSet = new HashSet<string>(includedProperties ?? [], StringComparer.OrdinalIgnoreCase);
            var pairs = new List<string>();

            if (source is IDictionary sourceDictionary)
            {
                foreach (DictionaryEntry entry in sourceDictionary)
                {
                    if(includeAllProperties || includedPropertiesSet.Contains(entry.Key.ToString()))
                        pairs.Add($"{entry.Key}{keyValueSeparator}{entry.Value}");
                }
            }
            else
            {
                pairs.AddRange(source.GetType()
                    .GetProperties()
                    .Where(prop => includeAllProperties || includedPropertiesSet.Contains(prop.Name))
                    .Select(prop => $"{prop.Name}{keyValueSeparator}{prop.GetValue(source)}"));
            }

            return string.Join(sequenceSeparator, pairs);
        }
    }
}
