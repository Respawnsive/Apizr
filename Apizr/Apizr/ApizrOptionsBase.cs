using System;
using System.Linq;
using Apizr.Logging;
using HttpTracer;

namespace Apizr
{
    public abstract class ApizrOptionsBase : IApizrOptionsBase
    {
        protected ApizrOptionsBase(Type webApiType,
            string[] assemblyPolicyRegistryKeys,
            string[] webApiPolicyRegistryKeys)
        {
            WebApiType = webApiType;
            PolicyRegistryKeys =
                assemblyPolicyRegistryKeys?.Union(webApiPolicyRegistryKeys ?? Array.Empty<string>()).ToArray() ??
                webApiPolicyRegistryKeys ?? Array.Empty<string>();
        }

        public Type WebApiType { get; }
        public Uri BaseAddress { get; protected set; }
        public HttpMessageParts HttpTracerVerbosity { get; protected set; }
        public ApizrLogLevel ApizrVerbosity { get; protected set; }
        public string[] PolicyRegistryKeys { get; }
    }
}