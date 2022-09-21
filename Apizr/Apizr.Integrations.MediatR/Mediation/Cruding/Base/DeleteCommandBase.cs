using System;
using Apizr.Mediation.Commanding;
using MediatR;
using Polly;

namespace Apizr.Mediation.Cruding.Base
{
    /// <summary>
    /// The top level base mediation Delete command
    /// </summary>
    /// <typeparam name="T">The api entity type</typeparam>
    /// <typeparam name="TKey">The entity's crud key type</typeparam>
    /// <typeparam name="TResultData">The api result type</typeparam>
    public abstract class DeleteCommandBase<T, TKey, TResultData> : MediationCommandBase<TKey, TResultData>
    {
        /// <summary>
        /// The top level base mediation Delete command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected DeleteCommandBase(TKey key, Action<Exception> onException = null) : base(onException)
        {
            Key = key;
        }

        /// <summary>
        /// The top level base mediation Delete command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        protected DeleteCommandBase(TKey key, Context context, Action<Exception> onException = null) : base(context, onException)
        {
            Key = key;
        }

        /// <summary>
        /// The entity's crud key
        /// </summary>
        public TKey Key { get; }
    }
    
    /// <inheritdoc />
    public abstract class DeleteCommandBase<T, TResultData> : DeleteCommandBase<T, int, TResultData>
    {
        /// <inheritdoc />
        protected DeleteCommandBase(int key, Action<Exception> onException = null) : base(key, onException)
        {
        }

        /// <inheritdoc />
        protected DeleteCommandBase(int key, Context context, Action<Exception> onException = null) : base(key, context, onException)
        {
        }
    }

    /// <inheritdoc />
    public abstract class DeleteCommandBase<T> : DeleteCommandBase<T, Unit>
    {
        /// <inheritdoc />
        protected DeleteCommandBase(int key, Action<Exception> onException = null) : base(key, onException)
        {
        }

        /// <inheritdoc />
        protected DeleteCommandBase(int key, Context context, Action<Exception> onException = null) : base(key, context, onException)
        {
        }
    }
}