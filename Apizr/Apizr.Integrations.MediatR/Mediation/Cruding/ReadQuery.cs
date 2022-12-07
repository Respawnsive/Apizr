using System;
using Apizr.Configuring.Request;
using Apizr.Mediation.Cruding.Base;
using Polly;

namespace Apizr.Mediation.Cruding
{
    /// <summary>
    /// The mediation Read query
    /// </summary>
    /// <typeparam name="TResultData">The result entity type</typeparam>
    /// <typeparam name="TKey">The entity's crud key type</typeparam>
    public class ReadQuery<TResultData, TKey> : ReadQueryBase<TResultData, TKey, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {

        /// <summary>
        /// The mediation Read query constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ReadQuery(TKey key, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(key, optionsBuilder)
        {
        }
    }

    /// <summary>
    /// The mediation Read query
    /// </summary>
    /// <typeparam name="TResultData">The result entity type</typeparam>
    public class ReadQuery<TResultData> : ReadQueryBase<TResultData, IApizrRequestOptions, IApizrRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation Read query constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ReadQuery(int key, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(key, optionsBuilder)
        {
        }
    }
}
