using Apizr.Configuring.Request;
using Apizr.Mediation.Cruding.Base;
using MediatR;
using Optional;
using Polly;
using System;

namespace Apizr.Optional.Cruding
{
    /// <summary>
    /// The mediation Delete optional command
    /// </summary>
    /// <typeparam name="T">The api entity type</typeparam>
    /// <typeparam name="TKey">The entity's crud key type</typeparam>
    public class DeleteOptionalCommand<T, TKey> : DeleteCommandBase<T, TKey, Option<Unit, ApizrException>, IApizrUnitRequestOptions, IApizrUnitRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation Delete optional command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public DeleteOptionalCommand(TKey key, Action<IApizrUnitRequestOptionsBuilder> optionsBuilder = null) : base(key, optionsBuilder)
        {
        }
    }

    /// <summary>
    /// The mediation Delete optional command
    /// </summary>
    /// <typeparam name="T">The api entity type</typeparam>
    public class DeleteOptionalCommand<T> : DeleteCommandBase<T, Option<Unit, ApizrException>, IApizrUnitRequestOptions, IApizrUnitRequestOptionsBuilder>
    {
        /// <summary>
        /// The mediation Delete optional command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="optionsBuilder">Options provided to the request</param>
        public DeleteOptionalCommand(int key, Action<IApizrUnitRequestOptionsBuilder> optionsBuilder = null) : base(key, optionsBuilder)
        {
        }
    }
}