using System;
using System.Collections.Generic;
using System.Reflection;
using Apizr.Configuring.Request;
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
        /// Crud api entity type if any
        /// </summary>
        Type CrudApiEntityType { get; }

        /// <summary>
        /// Web api interface or Crud api entity class type info
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

        /// <summary>
        /// The request options builders
        /// </summary>
        IDictionary<string, Action<IApizrRequestOptionsBuilder>> RequestOptionsBuilders { get; }
    }
}
