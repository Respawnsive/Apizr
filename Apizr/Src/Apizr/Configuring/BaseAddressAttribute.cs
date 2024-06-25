using System;

namespace Apizr.Configuring
{
    /// <summary>
    /// Set a base absolute address or relative path (could be defined with options builder)
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class BaseAddressAttribute : Attribute
    {
        /// <summary>
        /// Set a base absolute address or relative path (if path, base address has to be defined fluently)
        /// </summary>
        /// <param name="baseAddressOrPath">The web api base absolute address or relative path</param>
        public BaseAddressAttribute(string baseAddressOrPath)
        {
            BaseAddressOrPath = baseAddressOrPath;
        }

        /// <summary>
        /// The web api base absolute address or relative path
        /// </summary>
        public string BaseAddressOrPath { get; }
    }
}
