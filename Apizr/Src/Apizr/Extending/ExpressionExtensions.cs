using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Apizr.Extending
{
    internal static class ExpressionExtensions
    {
        internal static Action<TDeclaringType, TPropertyType> ToCompiledSetter<TDeclaringType, TPropertyType>(this Expression<Func<TDeclaringType, TPropertyType>> tokenProperty)
        {
            if (tokenProperty.Body is MemberExpression { Member: PropertyInfo { CanWrite: true } propertyInfo } && propertyInfo.GetSetMethod(true)?.IsPublic == true)
            {
                var propertyGetSetMethod = propertyInfo.GetSetMethod(true);
                if (propertyGetSetMethod?.IsPublic == true)
                {
                    var target = Expression.Parameter(typeof(TDeclaringType), "target");
                    var value = Expression.Parameter(typeof(TPropertyType), "value");
                    var assign = Expression.Call(target, propertyGetSetMethod, value);
                    return Expression.Lambda<Action<TDeclaringType, TPropertyType>>(assign, target, value).Compile();
                }
            }
            return null;
        }
    }
}
