// Copied from https://github.com/BSiLabs/HttpTracer/blob/fdba9af621a005626bcad74de9651248e56b6872/src/HttpTracer/HttpMessageParts.cs

using System;

namespace Apizr.Logging
{
    [Flags]
    public enum HttpMessageParts
    {
        Unspecified = 0,
        None = 1,

        RequestBody = 2,
        RequestHeaders = 4,
        RequestCookies = 32,

        RequestAll = RequestBody | RequestHeaders | RequestCookies,

        ResponseBody = 8,
        ResponseHeaders = 16,

        ResponseAll = ResponseBody | ResponseHeaders,

        All = ResponseAll | RequestAll
    }
}
