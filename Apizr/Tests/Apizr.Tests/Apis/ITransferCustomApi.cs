using Apizr.Transferring.Requesting;
using System.Collections.Generic;

namespace Apizr.Tests.Apis
{
    [WebApi]
    public interface ITransferCustomApi<TUploadApiResultData> : ITransferApi<IDictionary<string, object>, TUploadApiResultData>
    { }
}
