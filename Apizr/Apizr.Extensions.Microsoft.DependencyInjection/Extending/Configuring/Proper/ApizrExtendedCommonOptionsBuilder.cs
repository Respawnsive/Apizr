using System;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Logging;
using Microsoft.Extensions.Logging;
using Polly.Registry;

namespace Apizr.Extending.Configuring.Proper
{
    public class ApizrExtendedProperOptionsBuilder : IApizrExtendedProperOptionsBuilder
    {
        protected readonly ApizrExtendedProperOptions Options;

        internal ApizrExtendedProperOptionsBuilder(ApizrExtendedProperOptions properOptions)
        {
            Options = properOptions;
        }

        public IApizrExtendedProperOptions ApizrOptions => Options;

        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler(Func<HttpRequestMessage, Task<string>> refreshTokenFactory)
        {
            throw new NotImplementedException();
        }

        public IApizrExtendedProperOptionsBuilder AddDelegatingHandler(DelegatingHandler delegatingHandler)
        {
            throw new NotImplementedException();
        }

        public IApizrExtendedProperOptionsBuilder WithLogging(HttpTracerMode httpTracerMode = HttpTracerMode.Everything,
            HttpMessageParts trafficVerbosity = HttpMessageParts.All, LogLevel logLevel = LogLevel.Information)
        {
            throw new NotImplementedException();
        }

        public IApizrExtendedProperOptionsBuilder WithBaseAddress(string baseAddress)
        {
            throw new NotImplementedException();
        }

        public IApizrExtendedProperOptionsBuilder WithBaseAddress(Uri baseAddress)
        {
            throw new NotImplementedException();
        }

        public IApizrExtendedProperOptionsBuilder WithPolicyRegistry(IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            throw new NotImplementedException();
        }
    }
}
