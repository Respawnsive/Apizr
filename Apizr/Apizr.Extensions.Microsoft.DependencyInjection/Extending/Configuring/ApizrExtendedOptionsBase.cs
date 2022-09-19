using System;
using System.Collections.Generic;
using System.Net.Http;
using Apizr.Configuring;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;

namespace Apizr.Extending.Configuring
{
    /// <inheritdoc cref="IApizrExtendedOptionsBase"/>
    public class ApizrExtendedOptionsBase : ApizrOptionsBase, IApizrExtendedOptionsBase
    {
        public ApizrExtendedOptionsBase(IApizrCommonOptionsBase commonOptions, IApizrProperOptionsBase properOptions) : base(commonOptions, properOptions)
        {
        }

        /// <inheritdoc />
        public HttpClientHandler HttpClientHandler { get; protected set; }

        /// <inheritdoc />
        public IList<Func<IServiceProvider, IApizrOptionsBase, DelegatingHandler>> DelegatingHandlersExtendedFactories { get; protected set; }
    }
}
