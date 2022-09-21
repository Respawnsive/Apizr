using System;
using Apizr.Mediation.Cruding.Base;
using Polly;

namespace Apizr.Mediation.Cruding
{
    /// <summary>
    /// The mediation Read query
    /// </summary>
    /// <typeparam name="TResultData">The result entity type</typeparam>
    /// <typeparam name="TKey">The entity's crud key type</typeparam>
    public class ReadQuery<TResultData, TKey> : ReadQueryBase<TResultData, TKey>
    {
        /// <summary>
        /// The mediation Read query constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadQuery(TKey key, bool clearCache = false, Action<Exception> onException = null) : base(key, clearCache, onException)
        {
        }

        /// <summary>
        /// The mediation Read query constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadQuery(TKey key, int priority, bool clearCache = false, Action<Exception> onException = null) : base(key, priority, clearCache, onException)
        {
        }

        /// <summary>
        /// The mediation Read query constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadQuery(TKey key, Context context, bool clearCache = false, Action<Exception> onException = null) : base(key, context, clearCache, onException)
        {
        }

        /// <summary>
        /// The mediation Read query constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadQuery(TKey key, int priority, Context context, bool clearCache = false, Action<Exception> onException = null) : base(key, priority, context, clearCache, onException)
        {
        }
    }

    /// <summary>
    /// The mediation Read query
    /// </summary>
    /// <typeparam name="TResultData">The result entity type</typeparam>
    public class ReadQuery<TResultData> : ReadQueryBase<TResultData>
    {
        /// <summary>
        /// The mediation Read query constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadQuery(int key, bool clearCache = false, Action<Exception> onException = null) : base(key, clearCache, onException)
        {
        }

        /// <summary>
        /// The mediation Read query constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadQuery(int key, int priority, bool clearCache = false, Action<Exception> onException = null) : base(key, priority, clearCache, onException)
        {
        }

        /// <summary>
        /// The mediation Read query constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadQuery(int key, Context context, bool clearCache = false, Action<Exception> onException = null) : base(key, context, clearCache, onException)
        {
        }

        /// <summary>
        /// The mediation Read query constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public ReadQuery(int key, int priority, Context context, bool clearCache = false, Action<Exception> onException = null) : base(key, priority, context, clearCache, onException)
        {
        }
    }
}
