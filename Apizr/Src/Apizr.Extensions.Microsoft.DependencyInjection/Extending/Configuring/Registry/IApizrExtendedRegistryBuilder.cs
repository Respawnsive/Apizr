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
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <paramref name="crudedType"/> object type (class), 
        /// with key of type <see cref="int"/> and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <param name="crudedType">The object type to manage with crud api calls (class)</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudManagerFor(Type crudedType,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <paramref name="crudedType"/> object type (class), 
        /// with key of type <paramref name="crudedKeyType"/> (primitive) and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="crudedType">The object type to manage with crud api calls (class)</param>
        /// <param name="crudedKeyType">The object key type (primitive)</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudManagerFor(Type crudedType,
            Type crudedKeyType, Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <paramref name="crudedType"/> object type (class), 
        /// with key of type <paramref name="crudedKeyType"/> (primitive) and "ReadAll" query result of type <paramref name="crudedReadAllResultType"/>
        /// (inheriting from <see cref="IEnumerable{T}"/> or be of class type)
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <param name="crudedType">The object type to manage with crud api calls (class)</param>
        /// <param name="crudedKeyType">The object key type (primitive)</param>
        /// <param name="crudedReadAllResultType">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudManagerFor(Type crudedType,
            Type crudedKeyType, Type crudedReadAllResultType,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for <paramref name="crudedType"/> object type (class), 
        /// with key of type <paramref name="crudedKeyType"/> (primitive) and "ReadAll" query result of type <paramref name="crudedReadAllResultType"/>
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
        TApizrExtendedRegistryBuilder AddCrudManagerFor(Type crudedType,
            Type crudedKeyType, Type crudedReadAllResultType, Type crudedReadAllParamsType,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for <paramref name="crudedType"/> object type (class), 
        /// with key of type <paramref name="crudedKeyType"/> (primitive) and "ReadAll" query result of type <paramref name="crudedReadAllResultType"/>
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
        TApizrExtendedRegistryBuilder AddCrudManagerFor(Type crudedType,
            Type crudedKeyType, Type crudedReadAllResultType, Type crudedReadAllParamsType, Type apizrManagerType,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudManagerFor(params Type[] assemblyMarkerTypes);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudManagerFor(params Assembly[] assemblies);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudManagerFor(Action<TApizrExtendedProperOptionsBuilder> optionsBuilder,
            params Type[] assemblyMarkerTypes);

        /// <summary>
        /// Register <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudManagerFor(Action<TApizrExtendedProperOptionsBuilder> optionsBuilder,
            params Assembly[] assemblies);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudManagerFor(Type apizrManagerType, params Type[] assemblyMarkerTypes);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudManagerFor(Type apizrManagerType, params Assembly[] assemblies);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudManagerFor(Type apizrManagerType,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder, params Type[] assemblyMarkerTypes);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{ICrudApi}"/> for each <see cref="CrudEntityAttribute"/> decorated classes
        /// </summary>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{ICrudApi}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="CrudEntityAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddCrudManagerFor(Type apizrManagerType,
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
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="BaseAddressAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddManagerFor(Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null,
            params Type[] assemblyMarkerTypes);

        /// <summary>
        /// Register a <see cref="IApizrManager{webApiType}"/> for each <see cref="BaseAddressAttribute"/> decorated interfaces
        /// </summary>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="BaseAddressAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddManagerFor(Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null,
            params Assembly[] assemblies);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{webApiType}"/> for each <see cref="BaseAddressAttribute"/> decorated interfaces
        /// </summary>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{webApiType}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblyMarkerTypes">Any type contained in assembly to scan for <see cref="BaseAddressAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddManagerFor(Type apizrManagerType,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null, params Type[] assemblyMarkerTypes);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{webApiType}"/> for each <see cref="BaseAddressAttribute"/> decorated interfaces
        /// </summary>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{webApiType}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <param name="assemblies">Any assembly to scan for <see cref="BaseAddressAttribute"/></param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddManagerFor(Type apizrManagerType,
            Action<TApizrExtendedProperOptionsBuilder> optionsBuilder = null, params Assembly[] assemblies);

        /// <summary>
        /// Register a custom <see cref="IApizrManager{webApiType}"/>
        /// </summary>
        /// <param name="webApiType">The web api interface type to manage</param>
        /// <param name="apizrManagerType">A custom <see cref="IApizrManager{webApiType}"/> implementation type</param>
        /// <param name="optionsBuilder">The builder defining specific Apizr options</param>
        /// <returns></returns>
        TApizrExtendedRegistryBuilder AddManagerFor(
            Type webApiType, Type apizrManagerType,
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
