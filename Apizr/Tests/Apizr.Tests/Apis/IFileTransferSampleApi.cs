﻿using Apizr.Logging.Attributes;
using Apizr.Logging;
using Apizr.Transferring.Requesting;
using Microsoft.Extensions.Logging;

namespace Apizr.Tests.Apis
{
    [WebApi("http://speedtest.ftp.otenet.gr/files"), Log(HttpMessageParts.None, LogLevel.None)]
    public interface IFileTransferSampleApi : IFileTransferApi
    { }
}
