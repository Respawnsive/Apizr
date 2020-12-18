using System;
using System.Collections.Generic;
using System.Reflection;
using Apizr.Caching;
using Apizr.Extending;

namespace Apizr.Requesting
{
    /// <summary>
    /// Tells Apizr to auto register an <see cref="IApizrManager{ICrudApi}"/> for this decorated entity (works only with IServiceCollection extensions registration)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CrudEntityAttribute : Attribute
    {
        /// <summary>
        /// Define some crud api settings from this api entity
        /// </summary>
        /// <param name="baseUri">This specific api entity's base crud uri</param>
        /// <param name="keyType">This specific api entity's crud key type (default: null = typeof(int))</param>
        /// <param name="readAllResultType">The "ReadAll" query result type  (default: null = typeof(IEnumerable{}))</param>
        /// <param name="readAllParamsType">ReadAll query parameters type  (default: null = typeof(IDictionary{string, object}))</param>
        /// <param name="modelEntityType">Model entity type mapped with this api entity type (default: null = decorated api entity type)</param>
        public CrudEntityAttribute(string baseUri, Type keyType = null, Type readAllResultType = null, Type readAllParamsType = null, Type modelEntityType = null)
        {
            if (keyType != null && !keyType.GetTypeInfo().IsPrimitive)
                throw new ArgumentException($"{keyType.Name} is not primitive", nameof(keyType));

            if (readAllResultType != null && (!typeof(IEnumerable<>).IsAssignableFromGenericType(readAllResultType) && !readAllResultType.IsClass || !readAllResultType.IsGenericType))
                throw new ArgumentException($"{readAllResultType.Name} must inherit from {typeof(IEnumerable<>)} or be a generic class");

            if (readAllParamsType != null && !typeof(IDictionary<string, object>).IsAssignableFrom(readAllParamsType) && !readAllParamsType.IsClass)
                throw new ArgumentException($"{readAllParamsType.Name} must inherit from {typeof(IDictionary<string, object>)} or be a class", nameof(readAllParamsType));

            BaseUri = baseUri;
            KeyType = keyType ?? typeof(int);
            ReadAllResultType = readAllResultType ?? typeof(IEnumerable<>);
            ReadAllParamsType = readAllParamsType ?? typeof(IDictionary<string, object>);
            MappedEntityType = modelEntityType;
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

        /// <summary>
        /// "ReadAll" query parameters type
        /// </summary>
        public Type ReadAllParamsType { get; set; }

        /// <summary>
        /// Model entity type mapped with this Api entity type
        /// </summary>
        public Type MappedEntityType { get; set; }
    }
}
