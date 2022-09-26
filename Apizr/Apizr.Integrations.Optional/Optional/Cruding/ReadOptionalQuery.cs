using Apizr.Mediation.Cruding.Base;
using Optional;
using Polly;

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
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadOptionalQuery(TKey key, bool clearCache = false) : base(key, clearCache)
        {
        }

        /// <summary>
        /// The mediation Read optional query constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadOptionalQuery(TKey key, int priority, bool clearCache = false) : base(key, priority, clearCache)
        {
        }

        /// <summary>
        /// The mediation Read optional query constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadOptionalQuery(TKey key, Context context, bool clearCache = false) : base(key, context, clearCache)
        {
        }

        /// <summary>
        /// The mediation Read optional query constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadOptionalQuery(TKey key, int priority, Context context, bool clearCache = false) : base(key, priority, context, clearCache)
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
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadOptionalQuery(int key, bool clearCache = false) : base(key, clearCache)
        {
        }

        /// <summary>
        /// The mediation Read optional query constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadOptionalQuery(int key, int priority, bool clearCache = false) : base(key, priority, clearCache)
        {
        }

        /// <summary>
        /// The mediation Read optional query constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadOptionalQuery(int key, Context context, bool clearCache = false) : base(key, context, clearCache)
        {
        }

        /// <summary>
        /// The mediation Read optional query constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="priority">The execution priority to apply</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="clearCache">Asking to clear cache before sending</param>
        public ReadOptionalQuery(int key, int priority, Context context, bool clearCache = false) : base(key, priority, context, clearCache)
        {
        }
    }
}
