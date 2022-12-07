using Apizr.Configuring.Shared;
using Polly;

namespace Apizr.Configuring.Request
{
    public abstract class ApizrRequestOptionsBase : ApizrGlobalSharedOptionsBase, IApizrRequestOptionsBase
    {
        /// <inheritdoc />
        protected ApizrRequestOptionsBase(IApizrGlobalSharedRegistrationOptionsBase sharedOptions) : base(sharedOptions)
        {
            Context = sharedOptions.ContextFactory?.Invoke();
        }

        /// <inheritdoc />
        public Context Context { get; internal set; }
    }
}
