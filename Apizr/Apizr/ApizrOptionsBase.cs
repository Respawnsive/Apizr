﻿using System;
using System.Linq;
using System.Net;
using HttpTracer;

namespace Apizr
{
    public abstract class ApizrOptionsBase : IApizrOptionsBase
    {
        protected ApizrOptionsBase(Type webApiType, Uri baseAddress, DecompressionMethods? decompressionMethods,
            HttpMessageParts? httpTracerVerbosity, string[] assemblyPolicyRegistryKeys,
            string[] webApiPolicyRegistryKeys)
        {
            WebApiType = webApiType;
            BaseAddress = baseAddress;
            DecompressionMethods = decompressionMethods ?? DecompressionMethods.None;
            HttpTracerVerbosity = httpTracerVerbosity ?? HttpMessageParts.None;
            PolicyRegistryKeys =
                assemblyPolicyRegistryKeys?.Union(webApiPolicyRegistryKeys ?? Array.Empty<string>()).ToArray() ??
                webApiPolicyRegistryKeys ?? Array.Empty<string>();
        }

        public Type WebApiType { get; }
        public Uri BaseAddress { get; }
        public DecompressionMethods DecompressionMethods { get; }
        public HttpMessageParts HttpTracerVerbosity { get; }
        public string[] PolicyRegistryKeys { get; }
    }
}