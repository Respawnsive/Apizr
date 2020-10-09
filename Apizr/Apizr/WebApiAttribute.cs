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
        /// Define web api <see cref="BaseUri"/> and <see cref="DecompressionMethods"/>
        /// </summary>
        /// <param name="baseUri">The web api base uri</param>
        /// <param name="isAutoRegistrable">Makes decorated interface registrable by assembly scanning (default: true)</param>
        public WebApiAttribute(string baseUri, bool isAutoRegistrable = true)
        {
            BaseUri = baseUri;
            IsAutoRegistrable = isAutoRegistrable;
        }

        /// <summary>
        /// The web api base uri
        /// </summary>
        public string BaseUri { get; }

        /// <summary>
        /// Makes decorated interface registrable by assembly scanning
        /// </summary>
        public bool IsAutoRegistrable { get; }
    }
}
