using Apizr.Mediation.Cruding.Base;
using MediatR;
using Optional;
using Polly;

namespace Apizr.Optional.Cruding
{
    /// <summary>
    /// The mediation Delete optional command
    /// </summary>
    /// <typeparam name="T">The api entity type</typeparam>
    /// <typeparam name="TKey">The entity's crud key type</typeparam>
    public class DeleteOptionalCommand<T, TKey> : DeleteCommandBase<T, TKey, Option<Unit, ApizrException>>
    {
        /// <summary>
        /// The mediation Delete optional command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        public DeleteOptionalCommand(TKey key) : base(key)
        {
        }

        /// <summary>
        /// The mediation Delete optional command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="context">The Polly context to pass through</param>
        public DeleteOptionalCommand(TKey key, Context context) : base(key, context)
        {
        }
    }

    /// <summary>
    /// The mediation Delete optional command
    /// </summary>
    /// <typeparam name="T">The api entity type</typeparam>
    public class DeleteOptionalCommand<T> : DeleteCommandBase<T, Option<Unit, ApizrException>>
    {
        /// <summary>
        /// The mediation Delete optional command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        public DeleteOptionalCommand(int key) : base(key)
        {
        }

        /// <summary>
        /// The mediation Delete optional command constructor
        /// </summary>
        /// <param name="key">The entity's crud key</param>
        /// <param name="context">The Polly context to pass through</param>
        public DeleteOptionalCommand(int key, Context context) : base(key, context)
        {
        }
    }
}