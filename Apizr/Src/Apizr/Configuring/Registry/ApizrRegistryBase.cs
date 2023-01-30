using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Apizr.Requesting;

namespace Apizr.Configuring.Registry
{
    /// <summary>
    /// Registry options available for both static and extended registrations
    /// </summary>
    public abstract class ApizrRegistryBase : IApizrEnumerableRegistry, IApizrInternalEnumerableRegistry
    {
        internal readonly IDictionary<Type, Func<IApizrManager>> ConcurrentRegistry = new ConcurrentDictionary<Type, Func<IApizrManager>>();

        /// <summary>
        /// Get factory enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<Type, Func<IApizrManager>>> GetEnumerator() => ConcurrentRegistry.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #region Get

        /// <inheritdoc />
        public IApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>> GetCrudManagerFor<T>() where T : class
            => GetManagerFor<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>>();

        /// <inheritdoc />
        public IApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>> GetCrudManagerFor<T, TKey>()
            where T : class
            => GetManagerFor<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>();

        /// <inheritdoc />
        public IApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>> GetCrudManagerFor<T, TKey, TReadAllResult>()
            where T : class
            => GetManagerFor<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>();

        /// <inheritdoc />
        public IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> GetCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams>()
            where T : class
            => GetManagerFor<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>();

        /// <inheritdoc />
        public IApizrManager<TWebApi> GetManagerFor<TWebApi>()
            => GetManagerInternal<IApizrManager<TWebApi>>();

        /// <inheritdoc />
        public TApizrManager GetManagerInternal<TApizrManager>() where TApizrManager : IApizrManager
            => (TApizrManager)ConcurrentRegistry[typeof(TApizrManager)].Invoke();

        #endregion

        #region TryGet

        /// <inheritdoc />
        public bool TryGetCrudManagerFor<T>(out IApizrManager<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>> manager) where T : class
            => TryGetManagerFor(out manager);

        /// <inheritdoc />
        public bool TryGetCrudManagerFor<T, TKey>(out IApizrManager<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>> manager)
            where T : class
            => TryGetManagerFor(out manager);

        /// <inheritdoc />
        public bool TryGetCrudManagerFor<T, TKey, TReadAllResult>(out IApizrManager<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>> manager) where T : class
            => TryGetManagerFor(out manager);

        /// <inheritdoc />
        public bool TryGetCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams>(out IApizrManager<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>> manager) where T : class
            => TryGetManagerFor(out manager);

        /// <inheritdoc />
        public bool TryGetManagerFor<TWebApi>(out IApizrManager<TWebApi> manager)
            => TryGetManagerInternal(out manager);

        /// <inheritdoc />
        public bool TryGetManagerInternal<TApizrManager>(out TApizrManager manager) where TApizrManager : IApizrManager
        {
            if (!ConcurrentRegistry.TryGetValue(typeof(TApizrManager), out var managerFactory))
            {
                manager = default;
                return false;
            }

            manager = (TApizrManager)managerFactory.Invoke();
            return true;
        }

        #endregion

        #region Contains

        /// <inheritdoc />
        public int Count => ConcurrentRegistry.Count;

        /// <inheritdoc />
        public bool ContainsCrudManagerFor<T>() where T : class
            => ContainsManagerFor<ICrudApi<T, int, IEnumerable<T>, IDictionary<string, object>>>();

        /// <inheritdoc />
        public bool ContainsCrudManagerFor<T, TKey>() where T : class
            => ContainsManagerFor<ICrudApi<T, TKey, IEnumerable<T>, IDictionary<string, object>>>();

        /// <inheritdoc />
        public bool ContainsCrudManagerFor<T, TKey, TReadAllResult>() where T : class
            => ContainsManagerFor<ICrudApi<T, TKey, TReadAllResult, IDictionary<string, object>>>();

        /// <inheritdoc />
        public bool ContainsCrudManagerFor<T, TKey, TReadAllResult, TReadAllParams>() where T : class
            => ContainsManagerFor<ICrudApi<T, TKey, TReadAllResult, TReadAllParams>>();

        /// <inheritdoc />
        public bool ContainsManagerFor<TWebApi>()
            => ContainsManagerInternal<IApizrManager<TWebApi>>();

        /// <inheritdoc />
        public bool ContainsManagerInternal<TApizrManager>() where TApizrManager : IApizrManager
            => ConcurrentRegistry.ContainsKey(typeof(TApizrManager)); 

        #endregion
    }
}
