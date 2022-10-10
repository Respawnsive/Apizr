using System;
using Apizr.Configuring.Request;
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
        /// <param name="optionsBuilder">Options provided to the request</param>
        public DeleteCommand(TKey key, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(key, optionsBuilder)
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
        /// <param name="optionsBuilder">Options provided to the request</param>
        public DeleteCommand(int key, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(key, optionsBuilder)
        {
        }
    }
}