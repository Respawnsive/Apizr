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
    /// <typeparam name="TApizrRequestOptions">Options provided to the request</typeparam>
    /// <typeparam name="TApizrRequestOptionsBuilder">The request options builder</typeparam>
    public abstract class ReadQueryBase<TResponse, TKey, TApizrRequestOptions, TApizrRequestOptionsBuilder> : MediationQueryBase<TResponse, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The top level base mediation Read query
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        protected ReadQueryBase(TKey key, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(optionsBuilder)
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
    /// <typeparam name="TApizrRequestOptions">Options provided to the request</typeparam>
    /// <typeparam name="TApizrRequestOptionsBuilder">The request options builder</typeparam>
    public abstract class ReadQueryBase<TResponse, TApizrRequestOptions, TApizrRequestOptionsBuilder> : ReadQueryBase<TResponse, int, TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptions
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        /// <inheritdoc />
        protected ReadQueryBase(int key, Action<TApizrRequestOptionsBuilder> optionsBuilder = null) : base(key, optionsBuilder)
        {
        }
    }
}
