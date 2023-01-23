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
            => (IApizrManager<TWebApi>)ConcurrentRegistry[typeof(IApizrManager<TWebApi>)].Invoke();

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
        {
            if (!ConcurrentRegistry.TryGetValue(typeof(IApizrManager<TWebApi>), out var managerFactory))
            {
                manager = default;
                return false;
            }

            manager = (IApizrManager<TWebApi>)managerFactory.Invoke();
            return true;
        }

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
            => ConcurrentRegistry.ContainsKey(typeof(IApizrManager<TWebApi>));

        #region Internal

        /// <inheritdoc />
        public TWrappedManager GetWrappedManager<TWrappedManager>()
            => (TWrappedManager)ConcurrentRegistry[typeof(TWrappedManager)].Invoke();

        /// <inheritdoc />
        public bool TryGetWrappedManager<TWrappedManager>(out TWrappedManager manager)
        {
            if (!ConcurrentRegistry.TryGetValue(typeof(TWrappedManager), out var managerFactory))
            {
                manager = default;
                return false;
            }

            manager = (TWrappedManager)managerFactory.Invoke();
            return true;
        }

        #endregion
    }
}
