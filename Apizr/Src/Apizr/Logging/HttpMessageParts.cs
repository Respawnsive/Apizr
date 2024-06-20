// Copied from https://github.com/BSiLabs/HttpTracer/blob/fdba9af621a005626bcad74de9651248e56b6872/src/HttpTracer/HttpMessageParts.cs

using System;

namespace Apizr.Logging
{
    /// <summary>
    /// Http message parts to log
    /// </summary>
    [Flags]
    public enum HttpMessageParts
    {
        /// <summary>
        /// Unspecified
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        Unspecified = 1,

        /// <summary>
        /// Logs no parts
        /// </summary>
        None = 2,

        /// <summary>
        /// Logs request body only
        /// </summary>
        RequestBody = 4,

        /// <summary>
        /// Logs request headers only
        /// </summary>
        RequestHeaders = 8,

        /// <summary>
        /// Logs request cookies only
        /// </summary>
        RequestCookies = 64,

        /// <summary>
        /// Logs request headers and cookies only
        /// </summary>
        RequestAllButBody = RequestHeaders | RequestCookies,

        /// <summary>
        /// Logs request body, headers and cookies only
        /// </summary>
        RequestAll = RequestBody | RequestHeaders | RequestCookies,
        
        /// <summary>
        /// Logs response body only
        /// </summary>
        ResponseBody = 16,

        /// <summary>
        /// Logs response headers only
        /// </summary>
        ResponseHeaders = 32,

        /// <summary>
        /// Logs response body and headers only
        /// </summary>
        ResponseAll = ResponseBody | ResponseHeaders,

        /// <summary>
        /// Logs response body and headers only
        /// </summary>
        HeadersOnly = RequestHeaders | ResponseHeaders,

        /// <summary>
        /// Logs all parts but request body
        /// </summary>
        AllButRequestBody = RequestAllButBody | ResponseAll,

        /// <summary>
        /// Logs all parts but response body
        /// </summary>
        AllButResponseBody = RequestAll | ResponseHeaders,

        /// <summary>
        /// Logs all parts but bodies
        /// </summary>
        AllButBodies = RequestAllButBody | ResponseHeaders,

        /// <summary>
        /// Logs all parts
        /// </summary>
        All = RequestAll | ResponseAll
    }
}
