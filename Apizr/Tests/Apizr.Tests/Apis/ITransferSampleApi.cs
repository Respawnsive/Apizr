using Apizr.Configuring;
using Apizr.Logging.Attributes;
using Apizr.Logging;
using Apizr.Transferring.Requesting;
using Microsoft.Extensions.Logging;

namespace Apizr.Tests.Apis
{
    [BaseAddress("http://speedtest.ftp.otenet.gr/files"), Log(HttpMessageParts.None, LogLevel.None)]
    public interface ITransferSampleApi : ITransferApi
    { }
}
