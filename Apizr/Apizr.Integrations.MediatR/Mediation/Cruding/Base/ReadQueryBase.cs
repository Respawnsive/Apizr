using System;
using Apizr.Configuring.Request;
using Apizr.Mediation.Querying;
using Polly;

namespace Apizr.Mediation.Cruding.Base
{
    /// <summary>
    /// The top level base mediation Read query
    /// </summary>
    /// <typeparam name="TResponse">The result entity type</typeparam>
    /// <typeparam name="TKey">The entity's crud key type</typeparam>
    public abstract class ReadQueryBase<TResponse, TKey> : MediationQueryBase<TResponse>
    {
        /// <summary>
        /// The top level base mediation Read query
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ReadQueryBase(TKey key, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(optionsBuilder)
        {
            Key = key;
        }

        /// <summary>
        /// The top level base mediation Read query
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ReadQueryBase(TKey key, int priority, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(priority, optionsBuilder)
        {
            Key = key;
        }

        /// <summary>
        /// The entity's crud key
        /// </summary>
        public TKey Key { get; }
    }

    /// <summary>
    /// The top level base mediation Read query
    /// </summary>
    /// <typeparam name="TResponse">The result entity type</typeparam>
    public abstract class ReadQueryBase<TResponse> : ReadQueryBase<TResponse, int>
    {
        /// <inheritdoc />
        protected ReadQueryBase(int key, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(key, optionsBuilder)
        {
        }

        /// <inheritdoc />
        protected ReadQueryBase(int key, int priority, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(key, priority, optionsBuilder)
        {
        }
    }
}
