using System;
using Apizr.Logging;
using Microsoft.Extensions.Logging;

namespace Apizr.Configuring.Shared
{
    /// <summary>
    /// Options available at both common and proper level for both static and extended registrations
    /// </summary>
    public interface IApizrSharedRegistrationOptionsBase : IApizrGlobalSharedRegistrationOptionsBase
    {
    }
}
