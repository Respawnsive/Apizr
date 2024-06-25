using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using System.Collections.Generic;
using System;
using Apizr.Configuring.Shared;

namespace Apizr.Configuring.Manager
{
    /// <summary>
    /// Options available for both static and extended registrations
    /// </summary>
    public interface IApizrManagerOptionsBase : IApizrCommonOptionsBase, IApizrProperOptionsBase
    {
    }
}