using System;
using Apizr.Configuring.Shared;

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

    public static Action<T> IgnoreMessageParts<T>(this
        Action<T> optionsBuilder, HttpMessageParts messageParts)
        where T : IApizrGlobalSharedOptionsBuilderBase
    {
        if (optionsBuilder == null)
            optionsBuilder = optionsBuilderInstance =>
                ((IApizrInternalOptionsBuilder)optionsBuilderInstance).SetHandlerParameter(Constants.ApizrIgnoreMessagePartsKey, messageParts);
        else
            optionsBuilder += optionsBuilderInstance =>
                ((IApizrInternalOptionsBuilder)optionsBuilderInstance).SetHandlerParameter(Constants.ApizrIgnoreMessagePartsKey, messageParts);

        return optionsBuilder;
    }
}