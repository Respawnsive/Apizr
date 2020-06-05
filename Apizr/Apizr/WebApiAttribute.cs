using System;
using System.Net;

namespace Apizr
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class WebApiAttribute : Attribute
    {
        public WebApiAttribute(string baseUri)
        {
            BaseUri = baseUri;
        }
        public WebApiAttribute(string baseUri, DecompressionMethods decompressionMethods)
        {
            BaseUri = baseUri;
            DecompressionMethods = decompressionMethods;
        }

        public string BaseUri { get; }
        public DecompressionMethods? DecompressionMethods { get; }
    }
}
