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
        /// Define a web api without any base absolute address or relative path (has to be defined fluently) but discoverable for auto registration
        /// </summary>
        public WebApiAttribute() : this(null, true)
        {
            
        }

        /// <summary>
        /// Define a web api with a base absolute address or relative path (if path, base address has to be defined fluently) and discoverable for auto registration
        /// </summary>
        /// <param name="baseAddressOrPath">The web api base absolute address or relative path</param>
        public WebApiAttribute(string baseAddressOrPath) : this(baseAddressOrPath, true)
        {
        }

        /// <summary>
        /// Define a web api without any base absolute address or relative path (has to be defined fluently) and make it discoverable for auto registration or not
        /// </summary>
        /// <param name="isAutoRegistrable">Makes decorated interface registrable by assembly scanning</param>
        public WebApiAttribute(bool isAutoRegistrable) : this(null, isAutoRegistrable)
        {
        }

        /// <summary>
        /// Define a web api with a base absolute address or relative path (if path, base address has to be defined fluently) and make it discoverable for auto registration or not
        /// </summary>
        /// <param name="baseAddressOrPath">The web api base absolute address or relative path</param>
        /// <param name="isAutoRegistrable">Makes decorated interface registrable by assembly scanning</param>
        public WebApiAttribute(string baseAddressOrPath, bool isAutoRegistrable)
        {
            BaseAddressOrPath = baseAddressOrPath;
            IsAutoRegistrable = isAutoRegistrable;
        }

        /// <summary>
        /// The web api base absolute address or relative path
        /// </summary>
        public string BaseAddressOrPath { get; }

        /// <summary>
        /// Makes decorated interface registrable by assembly scanning
        /// </summary>
        public bool IsAutoRegistrable { get; }
    }
}
