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
        /// Define web api with priority management and auto registration enabled, but without base uri (should be defined with options builder)
        /// </summary>
        public WebApiAttribute() : this(null, true, true)
        {
            
        }

        /// <summary>
        /// Define web api with a base uri and with priority management and auto registration enabled
        /// </summary>
        /// <param name="baseUri">The web api base uri</param>
        public WebApiAttribute(string baseUri) : this(baseUri, true, true)
        {
        }

        /// <summary>
        /// Define web api with auto registration enabled, enabling or not priority management,
        /// but without base uri (should be defined with options builder)
        /// </summary>
        /// <param name="isPriorityManagementEnabled">Enable Fusillade priority management</param>
        public WebApiAttribute(bool isPriorityManagementEnabled) : this(null, isPriorityManagementEnabled, true)
        {
        }

        /// <summary>
        /// Define web api enabling or not priority management and auto registration, but without base uri (should be defined with options builder)
        /// </summary>
        /// <param name="isPriorityManagementEnabled">Enable Fusillade priority management</param>
        /// <param name="isAutoRegistrable">Makes decorated interface registrable by assembly scanning</param>
        public WebApiAttribute(bool isPriorityManagementEnabled, bool isAutoRegistrable) : this(null, isPriorityManagementEnabled, isAutoRegistrable)
        {
        }

        /// <summary>
        /// Define web api with a base uri and auto registration enabled, enabling or not priority management
        /// </summary>
        /// <param name="baseUri">The web api base uri</param>
        /// <param name="isPriorityManagementEnabled">Enable Fusillade priority management</param>
        public WebApiAttribute(string baseUri, bool isPriorityManagementEnabled) : this(baseUri, isPriorityManagementEnabled, true)
        {
        }

        /// <summary>
        /// Define web api with a base uri, enabling or not priority management and auto registration
        /// </summary>
        /// <param name="baseUri">The web api base uri</param>
        /// <param name="isPriorityManagementEnabled">Enable Fusillade priority management</param>
        /// <param name="isAutoRegistrable">Makes decorated interface registrable by assembly scanning</param>
        public WebApiAttribute(string baseUri, bool isPriorityManagementEnabled, bool isAutoRegistrable)
        {
            BaseUri = baseUri;
            IsPriorityManagementEnabled = isPriorityManagementEnabled;
            IsAutoRegistrable = isAutoRegistrable;
        }

        /// <summary>
        /// The web api base uri
        /// </summary>
        public string BaseUri { get; }

        /// <summary>
        /// Fusillade priority management activation
        /// </summary>
        public bool IsPriorityManagementEnabled { get; }

        /// <summary>
        /// Makes decorated interface registrable by assembly scanning
        /// </summary>
        public bool IsAutoRegistrable { get; }
    }
}
