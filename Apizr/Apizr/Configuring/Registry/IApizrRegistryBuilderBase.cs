using System;
using System.Collections.Generic;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;

namespace Apizr.Configuring.Registry
{
    /// <summary>
    /// Registry builder options available for both static and extended registrations
    /// </summary>
    public interface IApizrRegistryBuilderBase
    {
    }

    /// <summary>
    /// Registry builder options available for both static and extended registrations
    /// </summary>
    public interface IApizrRegistryBuilderBase<out TApizrRegistry, out TApizrRegistryBuilder, out TApizrProperOptionsBuilder, out TApizrCommonOptionsBuilder> : IApizrRegistryBuilderBase
        where TApizrRegistry : IApizrEnumerableRegistry
        where TApizrRegistryBuilder : IApizrRegistryBuilderBase<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
        where TApizrProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase
        where TApizrCommonOptionsBuilder : IApizrGlobalCommonOptionsBuilderBase
    {

        #region Registry

        /// <summary>
        /// Apizr registry
        /// </summary>
        TApizrRegistry ApizrRegistry { get; }

        /// <summary>
        /// Group registrations sharing specific common options
        /// </summary>
        /// <param name="registryGroupBuilder">The registry group</param>
        /// <param name="commonOptionsBuilder">The group common options</param>
        /// <returns></returns>
        TApizrRegistryBuilder AddGroup(Action<TApizrRegistryBuilder> registryGroupBuilder,
            Action<TApizrCommonOptionsBuilder> commonOptionsBuilder = null);

        #endregion

        #region Crud

        /// <summary>
        /// Create a <see cref="ApizrManager{ICrudApi}"/> instance for <typeparamref name="T"/> object type (class), 
        /// with key of type <see cref="int"/> and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <param name="optionsBuilder">The builder defining some api proper options</param>
        /// <returns></returns>
        TApizrRegistryBuilder AddCrudManagerFor<T>(
            Action<TApizrProperOptionsBuilder> optionsBuilder = null)
            where T : class;

        /// <summary>
        /// Create a <see cref="ApizrManager{ICrudApi}"/> instance for <typeparamref name="T"/> object type (class), 
        /// with key of type <typeparamref name="TKey"/> (primitive) and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <param name="optionsBuilder">The builder defining some api proper options</param>
        /// <returns></returns>
        TApizrRegistryBuilder AddCrudManagerFor<T, TKey>(
            Action<TApizrProperOptionsBuilder> optionsBuilder = null)
            where T : class;

        /// <summary>
        /// Create a <see cref="ApizrManager{ICrudApi}"/> instance for <typeparamref name="T"/> object type (class), 
        /// with key of type <typeparamref name="TKey"/> (primitive) and "ReadAll" query result of type <typeparamref name="TReadAllResult"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <typeparam name="TReadAllResult">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</typeparam>
        /// <param name="optionsBuilder">The builder defining some api proper options</param>
        /// <returns></returns>
        TApizrRegistryBuilder AddCrudManagerFor<T, TKey,
            TReadAllResult>(
            Action<TApizrProperOptionsBuilder> optionsBuilder = null)
            where T : class;

        /// <summary>
        /// Create a <see cref="ApizrManager{ICrudApi}"/> instance for <typeparamref name="T"/> object type (class), 
        /// with key of type <typeparamref name="TKey"/> (primitive) and "ReadAll" query result of type <typeparamref name="TReadAllResult"/>
        /// and ReadAll query parameters type (inheriting from IDictionary{string,object} or be of class type)
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <typeparam name="TReadAllResult">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters</typeparam>
        /// <param name="optionsBuilder">The builder defining some api proper options</param>
        /// <returns></returns>
        TApizrRegistryBuilder AddCrudManagerFor<T, TKey, TReadAllResult,
            TReadAllParams>(
            Action<TApizrProperOptionsBuilder> optionsBuilder = null)
            where T : class;

        #endregion

        #region General
        
        /// <summary>
        /// Create a <see cref="ApizrManager{TWebApi}"/> instance
        /// </summary>
        /// <typeparam name="TWebApi">The web api interface to manage</typeparam>
        /// <param name="optionsBuilder">The builder defining some api proper options</param>
        /// <returns></returns>
        TApizrRegistryBuilder AddManagerFor<TWebApi>(
            Action<TApizrProperOptionsBuilder> optionsBuilder = null);

        #endregion
    }
}
