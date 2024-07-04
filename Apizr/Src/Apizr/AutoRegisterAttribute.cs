using Apizr.Extending;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Apizr.Configuring;

namespace Apizr
{
    /// <summary>
    /// Tells Apizr to auto register a Manager for the provided api (works only with IServiceCollection extensions registration)
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class AutoRegisterAttribute : BaseAddressAttribute
    {
        /// <summary>
        /// Auto register a manager for the provided api
        /// </summary>
        /// <param name="webApiType">The web api interface type to manage</param>
        /// <param name="baseAddressOrPath">The web api base absolute address or relative path</param>
        public AutoRegisterAttribute(Type webApiType, string baseAddressOrPath) : base(baseAddressOrPath)
        {
            if (webApiType == null || !webApiType.GetTypeInfo().IsInterface)
                throw new ArgumentException("WebApi type must be an interface", nameof(webApiType));

            WebApiType = webApiType;
        }

        public Type WebApiType { get; }
    }

    public class AutoRegisterAttribute<TWebApi>(string baseAddressOrPath) : AutoRegisterAttribute(typeof(TWebApi), baseAddressOrPath);
}
