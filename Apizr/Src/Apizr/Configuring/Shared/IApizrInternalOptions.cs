using System.Collections.Generic;

namespace Apizr.Configuring.Shared
{
    internal interface IApizrInternalOptions
    {
        IDictionary<string, object> ResilienceProperties { get; }
    }
}
