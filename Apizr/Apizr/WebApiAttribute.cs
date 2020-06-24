using System;
using System.Net;

namespace Apizr
{
    /// <summary>
    /// Define general web api settings (could be defined with options builder)
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class WebApiAttribute : Attribute
    {
        /// <summary>
        /// Define web api <see cref="BaseUri"/>
        /// </summary>
        /// <param name="baseUri">The web api base uri</param>
        public WebApiAttribute(string baseUri)
        {
            BaseUri = baseUri;
        }

        /// <summary>
        /// Define web api <see cref="BaseUri"/> and <see cref="DecompressionMethods"/>
        /// </summary>
        /// <param name="baseUri">The web api base uri</param>
        /// <param name="decompressionMethods">The web api base decompression methods</param>
        public WebApiAttribute(string baseUri, DecompressionMethods decompressionMethods)
        {
            BaseUri = baseUri;
            DecompressionMethods = decompressionMethods;
        }

        /// <summary>
        /// The web api base uri
        /// </summary>
        public string BaseUri { get; }

        /// <summary>
        /// The web api base decompression methods
        /// </summary>
        public DecompressionMethods? DecompressionMethods { get; }
    }
}
