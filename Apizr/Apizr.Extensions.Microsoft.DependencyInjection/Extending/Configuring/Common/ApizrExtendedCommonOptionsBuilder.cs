using System;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Logging;
using Apizr.Mapping;
using Microsoft.Extensions.Logging;
using Refit;

namespace Apizr.Extending.Configuring.Common
{
    public class ApizrExtendedCommonOptionsBuilder : IApizrExtendedCommonOptionsBuilder
    {
        protected readonly ApizrExtendedCommonOptions Options;

        internal ApizrExtendedCommonOptionsBuilder(ApizrExtendedCommonOptions commonOptions)
        {
            Options = commonOptions;
        }

        public IApizrExtendedCommonOptions ApizrOptions => Options;

        public IApizrExtendedCommonOptionsBuilder WithAuthenticationHandler(Func<HttpRequestMessage, Task<string>> refreshTokenFactory)
        {
            throw new NotImplementedException();
        }

        public IApizrExtendedCommonOptionsBuilder AddDelegatingHandler(DelegatingHandler delegatingHandler)
        {
            throw new NotImplementedException();
        }

        public IApizrExtendedCommonOptionsBuilder WithLogging(HttpTracerMode httpTracerMode = HttpTracerMode.Everything,
            HttpMessageParts trafficVerbosity = HttpMessageParts.All, LogLevel logLevel = LogLevel.Information)
        {
            throw new NotImplementedException();
        }

        public IApizrExtendedCommonOptionsBuilder WithRefitSettings(RefitSettings refitSettings)
        {
            throw new NotImplementedException();
        }

        public IApizrExtendedCommonOptionsBuilder WithConnectivityHandler(IConnectivityHandler connectivityHandler)
        {
            throw new NotImplementedException();
        }

        public IApizrExtendedCommonOptionsBuilder WithConnectivityHandler(Func<bool> connectivityCheckingFunction)
        {
            throw new NotImplementedException();
        }

        public IApizrExtendedCommonOptionsBuilder WithCacheHandler(ICacheHandler cacheHandler)
        {
            throw new NotImplementedException();
        }

        public IApizrExtendedCommonOptionsBuilder WithLoggerFactory(ILoggerFactory loggerFactory)
        {
            throw new NotImplementedException();
        }

        public IApizrExtendedCommonOptionsBuilder WithMappingHandler(IMappingHandler mappingHandler)
        {
            throw new NotImplementedException();
        }
    }
}
