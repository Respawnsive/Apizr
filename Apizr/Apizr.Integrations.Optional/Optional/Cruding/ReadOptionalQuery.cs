using Apizr.Configuring.Request;
using Apizr.Mediation.Cruding.Base;
using Optional;
using Polly;
using System;

namespace Apizr.Optional.Cruding
{
    /// <summary>
    /// The mediation Read optional query
    /// </summary>
    /// <typeparam name="TResultData">The result entity type</typeparam>
    /// <typeparam name="TKey">The entity's crud key type</typeparam>
    public class ReadOptionalQuery<TResultData, TKey> : ReadQueryBase<Option<TResultData, ApizrException<TResultData>>, TKey>
    {
        /// <summary>
        /// The mediation Read optional query constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ReadOptionalQuery(TKey key, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(key, optionsBuilder)
        {
        }

        /// <summary>
        /// The mediation Read optional query constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ReadOptionalQuery(TKey key, int priority, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(key, priority, optionsBuilder)
        {
        }
    }

    /// <summary>
    /// The mediation Read optional query
    /// </summary>
    /// <typeparam name="TResultData">The result entity type</typeparam>
    public class ReadOptionalQuery<TResultData> : ReadQueryBase<Option<TResultData, ApizrException<TResultData>>>
    {
        /// <summary>
        /// The mediation Read optional query constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ReadOptionalQuery(int key, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(key, optionsBuilder)
        {
        }

        /// <summary>
        /// The mediation Read optional query constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public ReadOptionalQuery(int key, int priority, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(key, priority, optionsBuilder)
        {
        }
    }
}
