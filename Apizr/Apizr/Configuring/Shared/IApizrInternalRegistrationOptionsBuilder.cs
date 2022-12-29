using Apizr.Configuring.Manager;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using Apizr.Logging;

namespace Apizr.Configuring.Shared
{
    internal interface IApizrInternalRegistrationOptionsBuilder : IApizrInternalOptionsBuilder
    {
        void SetPrimaryHttpMessageHandler(
            Func<DelegatingHandler, ILogger, IApizrManagerOptionsBase, HttpMessageHandler> primaryHandlerFactory);
    }
}
