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
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <param name="baseUri">This specific entity's base crud uri</param>
        public CrudEntityAttribute(string baseUri) : this(baseUri, typeof(int), typeof(IEnumerable<>), typeof(IDictionary<string, object>), null)
        {
        }

        /// <summary>
        /// Define this specific entity's base crud uri and key type with "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <param name="baseUri">This specific entity's base crud uri</param>
        /// <param name="keyType">This specific entity's crud key type</param>
        public CrudEntityAttribute(string baseUri, Type keyType) : this(baseUri, keyType, typeof(IEnumerable<>), typeof(IDictionary<string, object>), null)
        {
        }

        /// <summary>
        /// Define this specific entity's base crud uri, key type and "ReadAll" query result type
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <param name="baseUri">This specific entity's base crud uri</param>
        /// <param name="keyType">This specific entity's crud key type</param>
        /// <param name="readAllResultType">The "ReadAll" query result type (class or <see cref="IEnumerable{T}"/>)</param>
        public CrudEntityAttribute(string baseUri, Type keyType, Type readAllResultType) : this(baseUri, keyType, readAllResultType, typeof(IDictionary<string, object>), null)
        {
        }

        /// <summary>
        /// Define this specific entity's base crud uri, key type and "ReadAll" query result type
        /// </summary>
        /// <param name="baseUri">This specific entity's base crud uri</param>
        /// <param name="keyType">This specific entity's crud key type</param>
        /// <param name="readAllResultType">The "ReadAll" query result type (class or <see cref="IEnumerable{T}"/>)</param>
        /// <param name="readAllParamsType">ReadAll query parameters type (class or IDictionary{string,object})</param>
        public CrudEntityAttribute(string baseUri, Type keyType, Type readAllResultType, Type readAllParamsType) : this(
            baseUri, keyType, readAllResultType, readAllParamsType, null)
        {

        }

        /// <summary>
        /// Define this specific entity's base crud uri, key type and "ReadAll" query result type
        /// </summary>
        /// <param name="baseUri">This specific entity's base crud uri</param>
        /// <param name="keyType">This specific entity's crud key type</param>
        /// <param name="readAllResultType">The "ReadAll" query result type (class or <see cref="IEnumerable{T}"/>)</param>
        /// <param name="readAllParamsType">ReadAll query parameters type (class or IDictionary{string,object})</param>
        /// <param name="modelEntityType">[AutoMapper integration required] Model entity type mapped with this Api entity type (default: null = decorated api entity type)</param>
        public CrudEntityAttribute(string baseUri, Type keyType, Type readAllResultType, Type readAllParamsType, Type modelEntityType)
        {
            if (!keyType.GetTypeInfo().IsPrimitive)
                throw new ArgumentException($"{keyType.Name} is not primitive", nameof(keyType));

            if (!typeof(IEnumerable<>).IsAssignableFromGenericType(readAllResultType) && !readAllResultType.IsClass)
                throw new ArgumentException($"{readAllResultType.Name} must inherit from {typeof(IEnumerable<>)} or be of class type");

            if (!typeof(IDictionary<string, object>).IsAssignableFrom(readAllParamsType) &&
                !readAllParamsType.IsClass)
                throw new ArgumentException(
                    $"{readAllParamsType.Name} must inherit from {typeof(IDictionary<string, object>)} or be of class type", nameof(readAllParamsType));

            BaseUri = baseUri;
            KeyType = keyType;
            ReadAllResultType = readAllResultType;
            ReadAllParamsType = readAllParamsType;
            ModelEntityType = modelEntityType;
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
        public Type ModelEntityType { get; set; }
    }
}
