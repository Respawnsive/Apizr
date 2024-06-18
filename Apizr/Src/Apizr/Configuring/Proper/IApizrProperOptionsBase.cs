using System;
using System.Reflection;
using Apizr.Configuring.Shared;
using Microsoft.Extensions.Logging;

namespace Apizr.Configuring.Proper
{
    /// <summary>
    /// Options available at proper level for both static and extended registrations
    /// </summary>
    public interface IApizrProperOptionsBase : IApizrSharedRegistrationOptionsBase
    {
        /// <summary>
        /// Web api interface type
        /// </summary>
        Type WebApiType { get; }

        /// <summary>
        /// Crud model type if any
        /// </summary>
        Type CrudModelType { get; }

        /// <summary>
        /// Web api interface or Crud model class type info
        /// </summary>
        TypeInfo TypeInfo { get; }

        /// <summary>
        /// True if it's a CRUD api
        /// </summary>
        bool IsCrudApi { get; }

        /// <summary>
        /// The logger instance
        /// </summary>
        ILogger Logger { get; }
    }
}
