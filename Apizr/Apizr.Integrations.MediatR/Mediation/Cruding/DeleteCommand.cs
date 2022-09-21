using System;
using Apizr.Mediation.Cruding.Base;
using MediatR;
using Polly;

namespace Apizr.Mediation.Cruding
{
    /// <summary>
    /// The mediation Delete command
    /// </summary>
    /// <typeparam name="T">The api entity type</typeparam>
    /// <typeparam name="TKey">The entity's crud key type</typeparam>
    public class DeleteCommand<T, TKey> : DeleteCommandBase<T, TKey, Unit>
    {
        /// <summary>
        /// The mediation Delete command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public DeleteCommand(TKey key, Action<Exception> onException = null) : base(key, onException)
        {
        }

        /// <summary>
        /// The mediation Delete command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public DeleteCommand(TKey key, Context context, Action<Exception> onException = null) : base(key, context, onException)
        {
        }
    }

    /// <summary>
    /// The mediation Delete command
    /// </summary>
    /// <typeparam name="T">The api entity type</typeparam>
    public class DeleteCommand<T> : DeleteCommandBase<T>
    {
        /// <summary>
        /// The mediation Delete command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public DeleteCommand(int key, Action<Exception> onException = null) : base(key, onException)
        {
        }

        /// <summary>
        /// The mediation Delete command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="context">The Polly context to pass through</param>
        /// <param name="onException">Action to execute when an exception occurs</param>
        public DeleteCommand(int key, Context context, Action<Exception> onException = null) : base(key, context, onException)
        {
        }
    }
}