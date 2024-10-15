using System;
using System.Collections.Generic;
using System.Linq;

namespace Apizr.Extending
{
    internal static class ObjectExtensions
    {
        internal static string ToString(this object source, string keyValueSeparator, string sequenceSeparator, params string[] includedProperties)
        {
            var props = new Dictionary<string, string>();
            if (source == null)
                throw new ArgumentException("Parameter source can not be null.");

            var type = source.GetType();
            var propsToInclude = includedProperties?.Length > 0
                ? type.GetProperties().Where(prop => includedProperties.Contains(prop.Name)).ToArray()
                : type.GetProperties();
            foreach (var prop in propsToInclude)
            {
                var val = prop.GetValue(source, []);
                var valStr = val?.ToString();
                if(!string.IsNullOrEmpty(valStr))
                    props.Add(prop.Name, valStr);
            }

            return props.ToString(keyValueSeparator, sequenceSeparator);
        }
	}
}
