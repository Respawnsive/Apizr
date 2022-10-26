using System;
using Apizr.Configuring.Request;
using Apizr.Mapping;

namespace Apizr.Mediation.Requesting.Handling.Base
{
    /// <summary>
    /// The top level base request handler
    /// </summary>
    public abstract class RequestHandlerBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptionsBase
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilderBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
    }
}
