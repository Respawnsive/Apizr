using System;
using System.Linq;
using System.Reflection;

namespace Apizr.Extending
{
    public static class TypeExtensions
    {
        public static bool IsAssignableFromGenericType(this Type genericType, Type givenType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
                return true;

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            return givenType.BaseType != null && IsAssignableFromGenericType(genericType, givenType.BaseType);
        }

        public static Type MakeGenericTypeIfNeeded(this Type type, params Type[] typeArguments)
        {
            if (!type.IsOpenGeneric())
                return type;

            return type.MakeGenericType(typeArguments);
        }

        public static bool IsOpenGeneric(this Type type)
        {
            return type.GetTypeInfo().IsGenericTypeDefinition || type.GetTypeInfo().ContainsGenericParameters;
        }

        public static string GetFriendlyName(this Type type)
        {
            if (type == typeof(int))
                return "int";
            else if (type == typeof(short))
                return "short";
            else if (type == typeof(byte))
                return "byte";
            else if (type == typeof(bool))
                return "bool";
            else if (type == typeof(long))
                return "long";
            else if (type == typeof(float))
                return "float";
            else if (type == typeof(double))
                return "double";
            else if (type == typeof(decimal))
                return "decimal";
            else if (type == typeof(string))
                return "string";
            else if (type.IsGenericType)
                return type.Name.Split('`')[0] + "<" + string.Join(", ", type.GetGenericArguments().Select(GetFriendlyName).ToArray()) + ">";
            else
                return type.Name;
        }
    }
}
