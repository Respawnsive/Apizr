﻿using Apizr.Configuring.Manager;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;

namespace Apizr.Extending.Configuring.Manager
{
    /// <summary>
    /// Options available for extended registrations
    /// </summary>
    public interface IApizrExtendedManagerOptions : IApizrExtendedManagerOptionsBase, IApizrExtendedCommonOptions, IApizrExtendedProperOptions
    {
    }

    /// <summary>
    /// Options available for extended registrations
    /// </summary>
    public interface IApizrExtendedManagerOptions<TWebApi> : IApizrManagerOptions<TWebApi>, IApizrExtendedManagerOptionsBase
    {

    }
}
