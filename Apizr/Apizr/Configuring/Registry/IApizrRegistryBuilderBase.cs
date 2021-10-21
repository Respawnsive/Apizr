using System;
using System.Collections.Generic;
using Apizr.Configuring.Proper;

namespace Apizr.Configuring.Registry
{
    public interface IApizrRegistryBuilderBase
    {
    }

    public interface IApizrRegistryBuilderBase<out TApizrRegistry, out TApizrRegistryBuilder, out TApizrProperOptionsBuilder> : IApizrRegistryBuilderBase
        where TApizrRegistry : IApizrRegistryBase
        where TApizrRegistryBuilder : IApizrRegistryBuilderBase<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder>
        where TApizrProperOptionsBuilder : IApizrProperOptionsBuilderBase
    {
        /// <summary>
        /// Apizr registry
        /// </summary>
        TApizrRegistry ApizrRegistry { get; }

        #region Crud

        /// <summary>
        /// Create a <see cref="ApizrManager{ICrudApi}"/> instance for <see cref="T"/> object type (class), 
        /// with key of type <see cref="int"/> and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <param name="optionsBuilder">The builder defining some api proper options</param>
        /// <returns></returns>
        TApizrRegistryBuilder AddCrudFor<T>(
            Action<TApizrProperOptionsBuilder> optionsBuilder = null)
            where T : class;

        /// <summary>
        /// Create a <see cref="ApizrManager{ICrudApi}"/> instance for <see cref="T"/> object type (class), 
        /// with key of type <see cref="TKey"/> (primitive) and "ReadAll" query result of type <see cref="IEnumerable{T}"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <param name="optionsBuilder">The builder defining some api proper options</param>
        /// <returns></returns>
        TApizrRegistryBuilder AddCrudFor<T, TKey>(
            Action<TApizrProperOptionsBuilder> optionsBuilder = null)
            where T : class;

        /// <summary>
        /// Create a <see cref="ApizrManager{ICrudApi}"/> instance for <see cref="T"/> object type (class), 
        /// with key of type <see cref="TKey"/> (primitive) and "ReadAll" query result of type <see cref="TReadAllResult"/>
        /// and ReadAll query parameters of type IDictionary{string,object}
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <typeparam name="TReadAllResult">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</typeparam>
        /// <param name="optionsBuilder">The builder defining some api proper options</param>
        /// <returns></returns>
        TApizrRegistryBuilder AddCrudFor<T, TKey,
            TReadAllResult>(
            Action<TApizrProperOptionsBuilder> optionsBuilder = null)
            where T : class;

        /// <summary>
        /// Create a <see cref="ApizrManager{ICrudApi}"/> instance for <see cref="T"/> object type (class), 
        /// with key of type <see cref="TKey"/> (primitive) and "ReadAll" query result of type <see cref="TReadAllResult"/>
        /// and ReadAll query parameters type (inheriting from IDictionary{string,object} or be of class type)
        /// </summary>
        /// <typeparam name="T">The object type to manage with crud api calls (class)</typeparam>
        /// <typeparam name="TKey">The object key type (primitive)</typeparam>
        /// <typeparam name="TReadAllResult">"ReadAll" query result type
        /// (should inherit from <see cref="IEnumerable{T}"/> or be of class type)</typeparam>
        /// <typeparam name="TReadAllParams">ReadAll query parameters</typeparam>
        /// <param name="optionsBuilder">The builder defining some api proper options</param>
        /// <returns></returns>
        TApizrRegistryBuilder AddCrudFor<T, TKey, TReadAllResult,
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
        TApizrRegistryBuilder AddFor<TWebApi>(
            Action<TApizrProperOptionsBuilder> optionsBuilder = null);

        #endregion
    }
}
