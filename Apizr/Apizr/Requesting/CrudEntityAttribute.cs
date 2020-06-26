using System;
using System.Collections.Generic;
using System.Reflection;

namespace Apizr.Requesting
{
    /// <summary>
    /// Tells Apizr to auto register an <see cref="IApizrManager{ICrudApi}"/> for this decorated entity (works only with IServiceCollection extensions registration)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CrudEntityAttribute : Attribute
    {
        /// <summary>
        /// Define this specific entity's base crud uri with key of type <see cref="int"/> and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="baseUri">This specific entity's base crud uri</param>
        public CrudEntityAttribute(string baseUri) : this(baseUri, typeof(int), typeof(IEnumerable<>))
        {
        }

        /// <summary>
        /// Define this specific entity's base crud uri and key type with "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="baseUri">This specific entity's base crud uri</param>
        /// <param name="keyType">This specific entity's crud key type</param>
        public CrudEntityAttribute(string baseUri, Type keyType) : this(baseUri, keyType, typeof(IEnumerable<>))
        {
        }

        /// <summary>
        /// Define this specific entity's base crud uri, key type and "ReadAll" query result type
        /// </summary>
        /// <param name="baseUri">This specific entity's base crud uri</param>
        /// <param name="keyType">This specific entity's crud key type</param>
        /// <param name="readAllResultType">The "ReadAll" query result type (you have to provide an <see cref="IPagedResult{T}"/> or <see cref="IEnumerable{T}"/> implementation type)</param>
        public CrudEntityAttribute(string baseUri, Type keyType, Type readAllResultType)
        {
            if (!keyType.GetTypeInfo().IsPrimitive)
                throw new ArgumentException($"{keyType.Name} is not primitive", nameof(keyType));

            if (!typeof(IEnumerable<>).IsAssignableFromGenericType(readAllResultType) && !typeof(IPagedResult<>).IsAssignableFromGenericType(readAllResultType))
                throw new ArgumentException($"{readAllResultType.Name} must inherit from {typeof(IEnumerable<>)} or {typeof(IPagedResult<>)}", nameof(readAllResultType));

            BaseUri = baseUri;
            KeyType = keyType;
            ReadAllResultType = readAllResultType;
        }

        /// <summary>
        /// This specific entity's base crud uri
        /// </summary>
        public string BaseUri { get; }

        /// <summary>
        /// This specific object's crud key type
        /// </summary>
        public Type KeyType { get; }

        /// <summary>
        /// "ReadAll" query result type
        /// </summary>
        public Type ReadAllResultType { get; set; }
    }
}
