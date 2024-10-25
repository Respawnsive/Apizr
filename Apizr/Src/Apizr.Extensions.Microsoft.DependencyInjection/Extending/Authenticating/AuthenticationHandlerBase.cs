using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
