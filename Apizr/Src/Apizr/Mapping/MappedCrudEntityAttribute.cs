using System;
using Apizr.Requesting;
using Apizr.Requesting.Attributes;

namespace Apizr.Mapping
{
    /// <summary>
    /// Tells Apizr to auto register an <see cref="IApizrManager{ICrudApi}"/> for the referenced api entity
    /// and mapped to this decorated model entity (works only with IServiceCollection extensions registration)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MappedCrudEntityAttribute : CrudEntityAttribute
    {
        /// <summary>
        /// Define some crud api settings from this mapped model entity
        /// </summary>
        /// <param name="apiEntityBaseUri">The mapped api entity's base crud uri</param>
        /// <param name="apiEntityType">The mapped api entity type</param>
        /// <param name="apiEntityKeyType">The mapped api entity's crud key type (default: null = typeof(int))</param>
        /// <param name="apiEntityReadAllResultType">The mapped api entity "ReadAll" query result type  (default: null = typeof(IEnumerable{}))</param>
        /// <param name="apiEntityReadAllParamsType">The mapped api entity ReadAll query parameters type  (default: null = typeof(IDictionary{string, object}))</param>
        public MappedCrudEntityAttribute(string apiEntityBaseUri, Type apiEntityType, Type apiEntityKeyType = null, Type apiEntityReadAllResultType = null, Type apiEntityReadAllParamsType = null) : base(apiEntityBaseUri, apiEntityKeyType, apiEntityReadAllResultType, apiEntityReadAllParamsType, apiEntityType)
        {
        }
    }

    /// <summary>
    /// Tells Apizr to auto register an <see cref="IApizrManager{ICrudApi}"/> for the referenced api entity
    /// and mapped to this decorated model entity (works only with IServiceCollection extensions registration)
    /// </summary>
    /// <typeparam name="TApiEntity">The mapped api entity type</typeparam>
    [AttributeUsage(AttributeTargets.Class)]
    public class MappedCrudEntityAttribute<TApiEntity> : MappedCrudEntityAttribute
    {
        /// <inheritdoc />
        public MappedCrudEntityAttribute(string apiEntityBaseUri) : base(apiEntityBaseUri, typeof(TApiEntity))
        {
        }
    }

    /// <summary>
    /// Tells Apizr to auto register an <see cref="IApizrManager{ICrudApi}"/> for the referenced api entity
    /// and mapped to this decorated model entity (works only with IServiceCollection extensions registration)
    /// </summary>
    /// <typeparam name="TApiEntity">The mapped api entity type</typeparam>
    /// <typeparam name="TApiEntityKey">The mapped api entity's crud key type (default: null = typeof(int))</typeparam>
    [AttributeUsage(AttributeTargets.Class)]
    public class MappedCrudEntityAttribute<TApiEntity, TApiEntityKey> : MappedCrudEntityAttribute
    {
        /// <inheritdoc />
        public MappedCrudEntityAttribute(string apiEntityBaseUri) : base(apiEntityBaseUri, typeof(TApiEntity), typeof(TApiEntityKey))
        {
        }
    }

    /// <summary>
    /// Tells Apizr to auto register an <see cref="IApizrManager{ICrudApi}"/> for the referenced api entity
    /// and mapped to this decorated model entity (works only with IServiceCollection extensions registration)
    /// </summary>
    /// <typeparam name="TApiEntity">The mapped api entity type</typeparam>
    /// <typeparam name="TApiEntityKey">The mapped api entity's crud key type (default: null = typeof(int))</typeparam>
    /// <typeparam name="TApiEntityReadAllResult">The mapped api entity "ReadAll" query result type  (default: null = typeof(IEnumerable{}))</typeparam>
    [AttributeUsage(AttributeTargets.Class)]
    public class MappedCrudEntityAttribute<TApiEntity, TApiEntityKey, TApiEntityReadAllResult> : MappedCrudEntityAttribute
    {
        /// <inheritdoc />
        public MappedCrudEntityAttribute(string apiEntityBaseUri) : base(apiEntityBaseUri, typeof(TApiEntity), typeof(TApiEntityKey), typeof(TApiEntityReadAllResult))
        {
        }
    }

    /// <summary>
    /// Tells Apizr to auto register an <see cref="IApizrManager{ICrudApi}"/> for the referenced api entity
    /// and mapped to this decorated model entity (works only with IServiceCollection extensions registration)
    /// </summary>
    /// <typeparam name="TApiEntity">The mapped api entity type</typeparam>
    /// <typeparam name="TApiEntityKey">The mapped api entity's crud key type (default: null = typeof(int))</typeparam>
    /// <typeparam name="TApiEntityReadAllResult">The mapped api entity "ReadAll" query result type  (default: null = typeof(IEnumerable{}))</typeparam>
    /// <typeparam name="TApiEntityReadAllParams">The mapped api entity ReadAll query parameters type  (default: null = typeof(IDictionary{string, object}))</typeparam>
    [AttributeUsage(AttributeTargets.Class)]
    public class MappedCrudEntityAttribute<TApiEntity, TApiEntityKey, TApiEntityReadAllResult, TApiEntityReadAllParams> : MappedCrudEntityAttribute
    {
        /// <inheritdoc />
        public MappedCrudEntityAttribute(string apiEntityBaseUri) : base(apiEntityBaseUri, typeof(TApiEntity), typeof(TApiEntityKey), typeof(TApiEntityReadAllResult), typeof(TApiEntityReadAllParams))
        {
        }
    }
}
