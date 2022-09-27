using System;
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
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ReadQueryBase(TKey key, bool clearCache, Action<Exception> onException = null) : base(clearCache, onException)
        {
            Key = key;
        }

        /// <summary>
        /// The top level base mediation Read query
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ReadQueryBase(TKey key, int priority, bool clearCache, Action<Exception> onException = null) : base(priority, clearCache, onException)
        {
            Key = key;
        }

        /// <summary>
        /// The top level base mediation Read query
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ReadQueryBase(TKey key, Context context, bool clearCache, Action<Exception> onException = null) : base(context, clearCache, onException)
        {
            Key = key;
        }

        /// <summary>
        /// The top level base mediation Read query
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected ReadQueryBase(TKey key, int priority, Context context, bool clearCache, Action<Exception> onException = null) : base(priority, context, clearCache, onException)
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
        protected ReadQueryBase(int key, bool clearCache, Action<Exception> onException = null) : base(key, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ReadQueryBase(int key, int priority, bool clearCache, Action<Exception> onException = null) : base(key, priority, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ReadQueryBase(int key, Context context, bool clearCache, Action<Exception> onException = null) : base(key, context, clearCache, onException)
        {
        }

        /// <inheritdoc />
        protected ReadQueryBase(int key, int priority, Context context, bool clearCache, Action<Exception> onException = null) : base(key, priority, context, clearCache, onException)
        {
        }
    }
}
