using System.Collections.Generic;
using System;
using System.Net.Http;
using Apizr.Configuring.Manager;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;

namespace Apizr.Extending.Configuring.Manager
{
    /// <summary>
    /// Options available for both static and extended registrations
    /// </summary>
    public interface IApizrExtendedManagerOptionsBase : IApizrExtendedCommonOptionsBase, IApizrExtendedProperOptionsBase, IApizrManagerOptionsBase
    {
        HttpClientHandler HttpClientHandler { get; }
    }
}
