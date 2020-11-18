using System;
using System.Linq;
using Apizr.Logging;
using HttpTracer;

namespace Apizr
{
    public abstract class ApizrOptionsBase : IApizrOptionsBase
    {
        protected ApizrOptionsBase(Type webApiType,
            HttpMessageParts? httpTracerVerbosity, 
            ApizrLogLevel? apizrVerbosity, 
            bool? isPriorityManagementEnabled,
            string[] assemblyPolicyRegistryKeys,
            string[] webApiPolicyRegistryKeys)
        {
            WebApiType = webApiType;
            HttpTracerVerbosity = httpTracerVerbosity ?? HttpMessageParts.None;
            ApizrVerbosity = apizrVerbosity ?? ApizrLogLevel.None;
            IsPriorityManagementEnabled = isPriorityManagementEnabled ?? true;
            PolicyRegistryKeys =
                assemblyPolicyRegistryKeys?.Union(webApiPolicyRegistryKeys ?? Array.Empty<string>()).ToArray() ??
                webApiPolicyRegistryKeys ?? Array.Empty<string>();
        }

        public Type WebApiType { get; }
        public HttpMessageParts HttpTracerVerbosity { get; set; }
        public ApizrLogLevel ApizrVerbosity { get; set; }
        public bool IsPriorityManagementEnabled { get; set; }
        public string[] PolicyRegistryKeys { get; }
    }
}