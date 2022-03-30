using System;
using System.Collections.Generic;
using System.Net.Http;
using Apizr.Configuring;
using Apizr.Configuring.Shared;

namespace Apizr.Extending.Configuring.Shared
{
    public interface IApizrExtendedSharedOptionsBase : IApizrSharedOptionsBase
    {
        /// <summary>
        /// Delegating handlers factories
        /// </summary>
        IList<Func<IServiceProvider, IApizrOptionsBase, DelegatingHandler>> DelegatingHandlersExtendedFactories { get; }
    }
}
