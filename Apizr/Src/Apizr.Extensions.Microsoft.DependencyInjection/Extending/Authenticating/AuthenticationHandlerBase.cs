using Apizr.Authenticating;
using Apizr.Configuring.Manager;
using Microsoft.Extensions.Logging;

namespace Apizr.Extending.Authenticating
{
    public abstract class AuthenticationHandlerBase<TWebApi> : AuthenticationHandlerBase
    {
        protected AuthenticationHandlerBase(ILogger logger, IApizrManagerOptions<TWebApi> apizrOptions) : base(logger, apizrOptions)
        {
        }
    }
}
