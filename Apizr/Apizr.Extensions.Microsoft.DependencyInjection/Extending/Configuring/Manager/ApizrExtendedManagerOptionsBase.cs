using System;
using System.Collections.Generic;
using System.Net.Http;
using Apizr.Configuring.Common;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Proper;

namespace Apizr.Extending.Configuring.Manager
{
    /// <inheritdoc cref="IApizrExtendedManagerOptionsBase"/>
    public class ApizrExtendedManagerOptionsBase : ApizrManagerOptionsBase, IApizrExtendedManagerOptionsBase
    {
        public ApizrExtendedManagerOptionsBase(IApizrCommonOptionsBase commonOptions, IApizrProperOptionsBase properOptions) : base(commonOptions, properOptions)
        {
        }

        /// <inheritdoc />
        public HttpClientHandler HttpClientHandler { get; protected set; }

        /// <inheritdoc />
        public IList<Func<IServiceProvider, IApizrManagerOptionsBase, DelegatingHandler>> DelegatingHandlersExtendedFactories { get; protected set; }
    }
}
