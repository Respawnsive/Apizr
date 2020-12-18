using System;
using System.Linq;
using Apizr.Logging;
using HttpTracer;

namespace Apizr
{
    public abstract class ApizrOptionsBase : IApizrOptionsBase
    {
        protected ApizrOptionsBase(Type webApiType,
            bool? isPriorityManagementEnabled,
            string[] assemblyPolicyRegistryKeys,
            string[] webApiPolicyRegistryKeys)
        {
            WebApiType = webApiType;
            IsPriorityManagementEnabled = isPriorityManagementEnabled ?? true;
            PolicyRegistryKeys =
                assemblyPolicyRegistryKeys?.Union(webApiPolicyRegistryKeys ?? Array.Empty<string>()).ToArray() ??
                webApiPolicyRegistryKeys ?? Array.Empty<string>();
        }

        public Type WebApiType { get; }
        public Uri BaseAddress { get; protected set; }
        public HttpMessageParts HttpTracerVerbosity { get; protected set; }
        public ApizrLogLevel ApizrVerbosity { get; protected set; }
        public bool IsPriorityManagementEnabled { get; set; }
        public string[] PolicyRegistryKeys { get; }
    }
}