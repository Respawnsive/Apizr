using System;
using System.Collections.Generic;
using System.Reflection;
using Apizr.Configuring;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Configuring.Registry;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;
using Apizr.Requesting;
using Apizr.Requesting.Attributes;

namespace Apizr.Extending.Configuring.Registry
{
    /// <summary>
    /// Registry builder options available for extended registrations
    /// </summary>
    public interface IApizrExtendedRegistryBuilder<out TApizrExtendedRegistry, out TApizrExtendedRegistryBuilder, out TApizrExtendedProperOptionsBuilder, out TApizrExtendedCommonOptionsBuilder> : 
        IApizrRegistryBuilderBase<TApizrExtendedRegistry, TApizrExtendedRegistryBuilder, TApizrExtendedProperOptionsBuilder, TApizrExtendedCommonOptionsBuilder>
        where TApizrExtendedRegistry : IApizrEnumerableRegistry
        where TApizrExtendedRegistryBuilder : IApizrRegistryBuilderBase<TApizrExtendedRegistry, TApizrExtendedRegistryBuilder, TApizrExtendedProperOptionsBuilder, TApizrExtendedCommonOptionsBuilder>
        where TApizrExtendedProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase
        where TApizrExtendedCommonOptionsBuilder : IApizrGlobalCommonOptionsBuilderBase
    {
        #region Crud

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for <typeparamref name="T"/> object type, 
        /// with key of type <typeparamref name="TKey"/> (primitive) and "ReadAll" query result of type <typeparamref name="TReadAllResult"/>
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
        TApizrExtendedRegistryBuilder AddCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams, TApizrManager>(
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null)
            where T : class
            where TApizrManager : IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>;

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <paramref name="apiEntityType"/> object type (class), 
        /// with key of type <see cref="int"/> and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <param name="apiEntityType">The object type to manage with crud api calls (class)</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudManagerFor(Type apiEntityType,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <paramref name="apiEntityType"/> object type (class), 
        /// with key of type <paramref name="apiEntityKeyType"/> (primitive) and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="apiEntityType">The object type to manage with crud api calls (class)</param>
        /// <param name="apiEntityKeyType">The object key type (primitive)</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudManagerFor(Type apiEntityType,
            Type apiEntityKeyType, Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <paramref name="apiEntityType"/> object type (class), 
        /// with key of type <paramref name="apiEntityKeyType"/> (primitive) and "ReadAll" query result of type <paramref name="apiEntityReadAllResultType"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <param name="apiEntityType">The object type to manage with crud api calls (class)</param>
        /// <param name="apiEntityKeyType">The object key type (primitive)</param>
        /// <param name="apiEntityReadAllResultType">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudManagerFor(Type apiEntityType,
            Type apiEntityKeyType, Type apiEntityReadAllResultType,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <paramref name="apiEntityType"/> object type (class), 
        /// with key of type <paramref name="apiEntityKeyType"/> (primitive) and "ReadAll" query result of type <paramref name="apiEntityReadAllResultType"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters type (inheriting from IDictionary{string,object} or be of class type)
        /// </summary>
        /// <param name="apiEntityType">The object type to manage with crud api calls (class)</param>
        /// <param name="apiEntityKeyType">The object key type (primitive)</param>
        /// <param name="apiEntityReadAllResultType">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</param>
        /// <param name="apiEntityReadAllParamsType">ReadAll query parameters type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudManagerFor(Type apiEntityType,
            Type apiEntityKeyType, Type apiEntityReadAllResultType, Type apiEntityReadAllParamsType,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for <paramref name="apiEntityType"/> object type (class), 
        /// with key of type <paramref name="apiEntityKeyType"/> (primitive) and "ReadAll" query result of type <paramref name="apiEntityReadAllResultType"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters type (inheriting from IDictionary{string,object} or be of class type)
        /// </summary>
        /// <param name="apiEntityType">The object type to manage with crud api calls</param>
        /// <param name="apiEntityKeyType">The object key type</param>
        /// <param name="apiEntityReadAllResultType">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</param>
        /// <param name="apiEntityReadAllParamsType">ReadAll query parameters type</param>
        /// <param name="apizrManagerImplementationType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudManagerFor(Type apiEntityType,
            Type apiEntityKeyType, Type apiEntityReadAllResultType, Type apiEntityReadAllParamsType, Type apizrManagerImplementationType,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudManagerFor(Type[] assemblyMarkerTypes, Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudManagerFor(Assembly[] assemblies, Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="apizrManagerImplementationType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudManagerFor(Type apizrManagerImplementationType, Type[] assemblyMarkerTypes,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="apizrManagerImplementationType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudManagerFor(Type apizrManagerImplementationType, Assembly[] assemblies,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        #endregion

        #region General

        /// <summary>
        /// Register a custom <see cref="IApizrManager{TWebApi}"/>
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <typeparam name="TApizrManager">A custom <see cref="IApizrManager{TWebApi}"/> implementation</typeparam>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddManagerFor<TWebApi, TApizrManager>(
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register <see cref="IApizrManager{webApiType}"/>
        /// </summary>
        /// <param name="webApiType">The web api interface type to manage</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddManagerFor(Type webApiType,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register a <see cref="IApizrManager{webApiType}"/> for each <see cref="BaseAddressAttribute"/> decorated interfaces
        /// </summary>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="BaseAddressAttribute"/></param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddManagerFor(Type[] assemblyMarkerTypes, Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register a <see cref="IApizrManager{webApiType}"/> for each <see cref="BaseAddressAttribute"/> decorated interfaces
        /// </summary>
        /// <param name="assemblies">Any assembly to scan for <see cref="BaseAddressAttribute"/></param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddManagerFor(Assembly[] assemblies, Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{webApiType}"/> for each <see cref="BaseAddressAttribute"/> decorated interfaces
        /// </summary>
        /// <param name="apizrManagerImplementationType">A custom <see cref="IApizrManager{webApiType}"/> implementation type</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="BaseAddressAttribute"/></param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddManagerFor(Type apizrManagerImplementationType, Type[] assemblyMarkerTypes,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{webApiType}"/> for each <see cref="BaseAddressAttribute"/> decorated interfaces
        /// </summary>
        /// <param name="apizrManagerImplementationType">A custom <see cref="IApizrManager{webApiType}"/> implementation type</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="BaseAddressAttribute"/></param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddManagerFor(Type apizrManagerImplementationType, Assembly[] assemblies,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{webApiType}"/>
        /// </summary>
        /// <param name="webApiType">The web api interface type to manage</param>
        /// <param name="apizrManagerImplementationType">A custom <see cref="IApizrManager{webApiType}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddManagerFor(
            Type webApiType, Type apizrManagerImplementationType,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        #endregion
    }

    /// <summary>
    /// Registry builder options available for extended registrations
    /// </summary>
    public interface IApizrExtendedRegistryBuilder : IApizrExtendedRegistryBuilder<IApizrExtendedRegistry, IApizrExtendedRegistryBuilder, IApizrExtendedProperOptionsBuilder, IApizrExtendedCommonOptionsBuilder>
    {
    }
}
