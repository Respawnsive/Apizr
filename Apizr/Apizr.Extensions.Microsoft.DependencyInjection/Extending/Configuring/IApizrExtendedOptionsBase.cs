using System.Net.Http;
using Apizr.Configuring;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;

namespace Apizr.Extending.Configuring
{
    /// <summary>
    /// Options available for both static and extended registrations
    /// </summary>
    public interface IApizrExtendedOptionsBase : IApizrExtendedCommonOptionsBase, IApizrExtendedProperOptionsBase, IApizrOptionsBase
    {
        HttpClientHandler HttpClientHandler { get; }
    }
}
