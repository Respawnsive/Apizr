using System;

namespace Apizr
{
    /// <summary>
    /// Define general web api settings (could be defined with options builder)
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class WebApiAttribute : Attribute
    {
        /// <summary>
        /// Define web api with priority management and auto registration enabled, but without base uri (should be defined with options builder)
        /// </summary>
        public WebApiAttribute() : this(null, true)
        {
            
        }

        /// <summary>
        /// Define web api with a base uri and with priority management and auto registration enabled
        /// </summary>
        /// <param name="baseUri">The web api base uri</param>
        public WebApiAttribute(string baseUri) : this(baseUri, true)
        {
        }

        /// <summary>
        /// Define web api enabling or not priority management and auto registration, but without base uri (should be defined with options builder)
        /// </summary>
        /// <param name="isAutoRegistrable">Makes decorated interface registrable by assembly scanning</param>
        public WebApiAttribute(bool isAutoRegistrable) : this(null, isAutoRegistrable)
        {
        }

        /// <summary>
        /// Define web api with a base uri, enabling or not priority management and auto registration
        /// </summary>
        /// <param name="baseUri">The web api base uri</param>
        /// <param name="isAutoRegistrable">Makes decorated interface registrable by assembly scanning</param>
        public WebApiAttribute(string baseUri, bool isAutoRegistrable)
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
