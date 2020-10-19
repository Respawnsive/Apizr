using System;
using System.Linq;
using HttpTracer;

namespace Apizr
{
    public abstract class ApizrOptionsBase : IApizrOptionsBase
    {
        protected ApizrOptionsBase(Type webApiType, Uri baseAddress,
            HttpMessageParts? httpTracerVerbosity, bool? isPriorityManagementEnabled,
            string[] assemblyPolicyRegistryKeys,
            string[] webApiPolicyRegistryKeys)
        {
            WebApiType = webApiType;
            BaseAddress = baseAddress;
            HttpTracerVerbosity = httpTracerVerbosity ?? HttpMessageParts.None;
            IsPriorityManagementEnabled = isPriorityManagementEnabled ?? true;
            PolicyRegistryKeys =
                assemblyPolicyRegistryKeys?.Union(webApiPolicyRegistryKeys ?? Array.Empty<string>()).ToArray() ??
                webApiPolicyRegistryKeys ?? Array.Empty<string>();
        }

        public Type WebApiType { get; }
        public Uri BaseAddress { get; set; }
        public HttpMessageParts HttpTracerVerbosity { get; set; }
        public bool IsPriorityManagementEnabled { get; set; }
        public string[] PolicyRegistryKeys { get; }
    }
}