using System;
using System.Collections.Generic;
using System.Reflection;
using Apizr.Configuring.Proper;
using Apizr.Configuring.Registry;
using Apizr.Extending.Configuring.Proper;
using Apizr.Requesting;

namespace Apizr.Extending.Configuring.Registry
{
    public interface IApizrExtendedRegistryBuilder<out TApizrExtendedRegistry, out TApizrExtendedRegistryBuilder, out TApizrExtendedProperOptionsBuilder> : 
        IApizrRegistryBuilderBase<TApizrExtendedRegistry, TApizrExtendedRegistryBuilder, TApizrExtendedProperOptionsBuilder>
        where TApizrExtendedRegistry : IApizrEnumerableRegistry
        where TApizrExtendedRegistryBuilder : IApizrRegistryBuilderBase<TApizrExtendedRegistry, TApizrExtendedRegistryBuilder, TApizrExtendedProperOptionsBuilder>
        where TApizrExtendedProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase
    {
        #region Crud

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for <see cref="T"/> object type, 
        /// with key of type <see cref="TKey"/> (primitive) and "ReadAll" query result of type <see cref="TReadAllResult"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters type (inheriting from IDictionary{string,object} or be of class type)
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <typeparam name="TReadAllResult">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters</typeparam>
        /// <typeparam name="TApizrManager">A custom <see cref="IApizrManager{ICrudApi}"/> implementation</typeparam>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudFor<T, TKey, TReadAllResult, TReadAllParams, TApizrManager>(
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null)
            where T : class
            where TApizrManager : IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>;

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="crudedType"/> object type (class), 
        /// with key of type <see cref="int"/> and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <param name="crudedType">The object type to manage with crud api calls (class)</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudFor(Type crudedType,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="crudedType"/> object type (class), 
        /// with key of type <see cref="crudedKeyType"/> (primitive) and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="crudedType">The object type to manage with crud api calls (class)</param>
        /// <param name="crudedKeyType">The object key type (primitive)</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudFor(Type crudedType,
            Type crudedKeyType, Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="crudedType"/> object type (class), 
        /// with key of type <see cref="crudedKeyType"/> (primitive) and "ReadAll" query result of type <see cref="crudedReadAllResultType"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <param name="crudedType">The object type to manage with crud api calls (class)</param>
        /// <param name="crudedKeyType">The object key type (primitive)</param>
        /// <param name="crudedReadAllResultType">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudFor(Type crudedType,
            Type crudedKeyType, Type crudedReadAllResultType,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <see cref="crudedType"/> object type (class), 
        /// with key of type <see cref="crudedKeyType"/> (primitive) and "ReadAll" query result of type <see cref="crudedReadAllResultType"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters type (inheriting from IDictionary{string,object} or be of class type)
        /// </summary>
        /// <param name="crudedType">The object type to manage with crud api calls (class)</param>
        /// <param name="crudedKeyType">The object key type (primitive)</param>
        /// <param name="crudedReadAllResultType">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</param>
        /// <param name="crudedReadAllParamsType">ReadAll query parameters type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudFor(Type crudedType,
            Type crudedKeyType, Type crudedReadAllResultType, Type crudedReadAllParamsType,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for <see cref="crudedType"/> object type (class), 
        /// with key of type <see cref="crudedKeyType"/> (primitive) and "ReadAll" query result of type <see cref="crudedReadAllResultType"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters type (inheriting from IDictionary{string,object} or be of class type)
        /// </summary>
        /// <param name="crudedType">The object type to manage with crud api calls</param>
        /// <param name="crudedKeyType">The object key type</param>
        /// <param name="crudedReadAllResultType">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</param>
        /// <param name="crudedReadAllParamsType">ReadAll query parameters type</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudFor(Type crudedType,
            Type crudedKeyType, Type crudedReadAllResultType, Type crudedReadAllParamsType, Type apizrManagerType,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudFor(params Type[] assemblyMarkerTypes);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudFor(params Assembly[] assemblies);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudFor(Action<TApizrExtendedProperOptionsBuilder> optionsBuilder,
            params Type[] assemblyMarkerTypes);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudFor(Action<TApizrExtendedProperOptionsBuilder> optionsBuilder,
            params Assembly[] assemblies);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudFor(Type apizrManagerType, params Type[] assemblyMarkerTypes);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudFor(Type apizrManagerType, params Assembly[] assemblies);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudFor(Type apizrManagerType,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder, params Type[] assemblyMarkerTypes);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudFor(Type apizrManagerType,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder, params Assembly[] assemblies);

        #endregion

        #region General

        /// <summary>
        /// Register a custom <see cref="IApizrManager{TWebApi}"/>
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TApizrManager">A custom <see cref="IApizrManager{TWebApi}"/> implementation</typeparam>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddFor<TWebApi, TApizrManager>(
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register <see cref="IApizrManager{webApiType}"/>
        /// </summary>
        /// <param name="webApiType">The web api interface type to manage</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddFor(Type webApiType,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register a <see cref="IApizrManager{webApiType}"/> for each <see cref="WebApiAttribute"/> decorated interfaces
        /// </summary>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="WebApiAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddFor(Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null,
            params Type[] assemblyMarkerTypes);

        /// <summary>
        /// Register a <see cref="IApizrManager{webApiType}"/> for each <see cref="WebApiAttribute"/> decorated interfaces
        /// </summary>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="WebApiAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddFor(Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null,
            params Assembly[] assemblies);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{webApiType}"/> for each <see cref="WebApiAttribute"/> decorated interfaces
        /// </summary>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{webApiType}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="WebApiAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddFor(Type apizrManagerType,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null, params Type[] assemblyMarkerTypes);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{webApiType}"/> for each <see cref="WebApiAttribute"/> decorated interfaces
        /// </summary>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{webApiType}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="WebApiAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddFor(Type apizrManagerType,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null, params Assembly[] assemblies);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{webApiType}"/>
        /// </summary>
        /// <param name="webApiType">The web api interface type to manage</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{webApiType}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddFor(
            Type webApiType, Type apizrManagerType,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        #endregion
    }

    public interface IApizrExtendedRegistryBuilder : IApizrExtendedRegistryBuilder<IApizrExtendedRegistry, IApizrExtendedRegistryBuilder, IApizrExtendedProperOptionsBuilder>
    {
    }
}
