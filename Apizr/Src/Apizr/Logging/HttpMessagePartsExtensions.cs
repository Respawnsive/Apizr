namespace Apizr.Logging;

public static class HttpMessagePartsExtensions
{
    public static bool HasRequestFlags(this HttpMessageParts parts) =>
        parts.HasFlag(HttpMessageParts.RequestHeaders) || 
        parts.HasFlag(HttpMessageParts.RequestBody) ||
        parts.HasFlag(HttpMessageParts.RequestCookies);

    public static bool HasResponseFlags(this HttpMessageParts parts) =>
        parts.HasFlag(HttpMessageParts.ResponseHeaders) ||
        parts.HasFlag(HttpMessageParts.ResponseBody);
}