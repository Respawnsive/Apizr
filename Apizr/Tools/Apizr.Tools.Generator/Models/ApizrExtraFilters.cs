using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DotLiquid;

namespace Apizr.Tools.Generator.Models
{
     public static class ApizrExtraFilters
    {
        private static readonly IEqualityComparer<object> Comparer = new ValueEqualityComparer();

        public static IEnumerable<object> XSelectPrefix(object any, string prefix)
        {
            return ToEnum(any)
                .Select(ToDict)
                .Select(x => x.ToDictionary(kvp => prefix + kvp.Key, kvp => kvp.Value));
        }

        public static IEnumerable<object> XValueArray(object any, string keyName = null)
        {
            var dict = ToDict(any);
            if (string.IsNullOrWhiteSpace(keyName))
            {
                return dict.Values;
            }

            return dict.Select(kvp => {
                // Create new object, rather than changing existing one
                var hash = new Hash();
                hash.Merge(ToDict(kvp.Value));
                hash[keyName] = kvp.Key;
                return hash;
            });
        }

        public static IEnumerable<object> XSelectMany(object any, string key)
        {
            return ToEnum(any).SelectMany(
                outer => ToEnum(GetFieldVal(outer, key)),
                MergeDicts
            );
        }

        public static IEnumerable<object> Where(object any, string key, object value)
        {
            return ToEnum(any)
                .Where(el => Comparer.Equals(GetFieldVal(el, key), value));
        }

        public static IEnumerable<IDictionary<string,object>> XJoin(object outerAny, object innerAny, string outerKey, string innerKey = null)
        {
            innerKey ??= outerKey;
            var outerEnum = ToEnum(outerAny);
            var innerEnum = ToEnum(innerAny);
            return outerEnum.Join(
                innerEnum,
                outer => GetFieldVal(outer, outerKey),
                inner => GetFieldVal(inner, innerKey),
                MergeDicts,
                Comparer
            );
        }

        private class ValueEqualityComparer : IEqualityComparer<object>
        {
            bool IEqualityComparer<object>.Equals(object left, object right)
            {
                // This is the same as DotLiquid.Condition.EqualVariables
                if (left != null && right != null && left.GetType() != right.GetType())
                {
                    try
                    {
                        right = Convert.ChangeType(right, left.GetType());
                    }
                    catch (Exception)
                    {
                    }
                }

                return Equals(left, right);
            }

            int IEqualityComparer<object>.GetHashCode(object obj) => obj.GetHashCode();
        }

        private static object GetFieldVal(object any, string key)
        {
            if (any is IDictionary<string, object> dict && dict.TryGetValue(key, out var val))
            {
                return val;
            }

            return null;
        }

        private static IEnumerable<object> ToEnum(object any)
        {
            return any switch {
                null => new object[0],
                string _ => new [] {any},
                IDictionary<string, object> dict => dict.Values,
                IEnumerable<object> en => en,
                IEnumerable en => en.Cast<object>(),
                _ => new[] {any}
            };
        }

        private static IDictionary<string, object> ToDict(object any)
        {
            return any switch {
                null => new Hash(),
                IDictionary<string, object> dict => dict,
                _ => new Hash()
            };
        }

        private static IDictionary<string, object> MergeDicts(object outer, object inner)
        {
            var outerDict = ToDict(outer);
            var innerDict = ToDict(inner);

            var outDict = new Hash();
            outDict.Merge(innerDict);
            outDict.Merge(outerDict);
            return outDict;
        }
    }
}