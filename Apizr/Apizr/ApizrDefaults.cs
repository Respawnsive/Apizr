using System;
using System.Collections.Generic;

namespace Apizr
{
    public class ApizrDefaults
    {
        public static readonly Type DefaultCrudKeyType = typeof(int);
        public static readonly Type DefaultCrudReadAllResultType = typeof(IEnumerable<>);
        public static readonly Type DefaultCrudReadAllParamsType = typeof(IDictionary<string, object>);
    }
}
