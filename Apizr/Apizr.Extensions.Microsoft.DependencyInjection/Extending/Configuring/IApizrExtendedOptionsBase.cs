using System.Net.Http;
using Apizr.Configuring;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;

namespace Apizr.Extending.Configuring
{
    public interface IApizrExtendedOptionsBase : IApizrExtendedCommonOptionsBase, IApizrExtendedProperOptionsBase, IApizrOptionsBase
    {
        HttpClientHandler HttpClientHandler { get; }
    }
}
