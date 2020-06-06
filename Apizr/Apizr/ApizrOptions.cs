using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using HttpTracer;
using Refit;

namespace Apizr
{
    public class ApizrOptions : IApizrOptions
    {
        public ApizrOptions(Type webApiType, Uri baseAddress, DecompressionMethods? decompressionMethods, HttpMessageParts? httpTracerVerbosity, string[] assemblyPolicyRegistryKeys, string[] webApiPolicyRegistryKeys)
        {
            WebApiType = webApiType;
            BaseAddress = baseAddress;
            DecompressionMethods = decompressionMethods ?? DecompressionMethods.None;
            HttpTracerVerbosity = httpTracerVerbosity ?? HttpMessageParts.None;
            PolicyRegistryKeys = assemblyPolicyRegistryKeys?.Union(webApiPolicyRegistryKeys ?? Array.Empty<string>()).ToArray() ?? webApiPolicyRegistryKeys ?? Array.Empty<string>();
            RefitSettingsFactory = () => new RefitSettings();
            DelegatingHandlersFactories = new List<Func<DelegatingHandler>>();
        }

        public Type WebApiType { get; }
        public Uri BaseAddress { get; }
        public DecompressionMethods DecompressionMethods { get; }
        public HttpMessageParts HttpTracerVerbosity { get; }
        public string[] PolicyRegistryKeys { get; }
        public Func<RefitSettings> RefitSettingsFactory { get; internal set; }
        public IList<Func<DelegatingHandler>> DelegatingHandlersFactories { get; }
    }
}