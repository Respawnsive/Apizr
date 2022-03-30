using System;
using System.Collections.Generic;

namespace Apizr.Extending
{
    public static class ObjectExtensions
    {
        public static string ToString(this object source, string keyValueSeparator, string sequenceSeparator)
        {
            var props = new Dictionary<string, string>();
            if (source == null)
                throw new ArgumentException("Parameter source can not be null.");

            var type = source.GetType();
            foreach (var prop in type.GetProperties())
            {
                var val = prop.GetValue(source, new object[] { });
                var valStr = val?.ToString();
                if(!string.IsNullOrEmpty(valStr))
                    props.Add(prop.Name, valStr);
            }

            return props.ToString(keyValueSeparator, sequenceSeparator);
        }
	}
}
