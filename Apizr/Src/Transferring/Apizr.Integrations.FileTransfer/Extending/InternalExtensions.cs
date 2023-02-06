using Apizr.Configuring.Request;
using Apizr.Helping;

namespace Apizr.Extending
{
    internal static class InternalExtensions
    {
        internal static string GetDynamicPathOrDefault(this IApizrRequestOptions options) =>
            options.HandlersParameters.TryGetValue(Constants.ApizrDynamicPathKey, out var endingPathObject)
                ? endingPathObject?.ToString()
                : null;

        internal static string GetDynamicPathOrDefault(this IApizrRequestOptions options, string lastPath) =>
            options.HandlersParameters.TryGetValue(Constants.ApizrDynamicPathKey, out var endingPathObject)
                ? string.IsNullOrEmpty(endingPathObject?.ToString())
                    ? lastPath
                    : UrlHelper.Combine(endingPathObject.ToString(), lastPath)
                : lastPath;
    }
}
