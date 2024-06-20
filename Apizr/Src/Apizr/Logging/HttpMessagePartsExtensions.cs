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
                optionsBuilderInstance.IgnoreMessageParts(messageParts);
        else
            optionsBuilder += optionsBuilderInstance =>
                optionsBuilderInstance.IgnoreMessageParts(messageParts);

        return optionsBuilder;
    }

    public static T IgnoreMessageParts<T>(this
        T optionsBuilder, HttpMessageParts messageParts)
        where T : IApizrGlobalSharedOptionsBuilderBase
    {
        ((IApizrInternalOptionsBuilder)optionsBuilder).SetHandlerParameter(Constants.ApizrIgnoreMessagePartsKey, messageParts);
        return optionsBuilder;
    }

    public static HttpMessageParts IgnoreMessageParts(this HttpMessageParts messageParts, HttpMessageParts partsToIgnore) =>
        messageParts & ~partsToIgnore;
}