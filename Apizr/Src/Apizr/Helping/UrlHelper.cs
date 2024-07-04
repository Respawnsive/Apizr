using System;
using System.Linq;

namespace Apizr.Helping
{
    /// <summary>
    /// Some url helpers
    /// </summary>
    public static class UrlHelper
    {
        /// <summary>
        /// Combines the url base and the relative url into one, consolidating the '/' between them
        /// </summary>
        /// <param name="baseUrl">Base url that will be combined</param>
        /// <param name="relativePath">The relative path to combine</param>
        /// <returns>The merged url</returns>
        public static string Combine(string baseUrl, string relativePath)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                return null;

            if (string.IsNullOrWhiteSpace(relativePath))
                return baseUrl;

            baseUrl = baseUrl.TrimEnd('/');
            relativePath = relativePath.TrimStart('/');

            return $"{baseUrl}/{relativePath}";
        }

        /// <summary>
        /// Combines the url base and the array of relatives urls into one, consolidating the '/' between them
        /// </summary>
        /// <param name="baseUrl">Base url that will be combined</param>
        /// <param name="relativePaths">The array of relative paths to combine</param>
        /// <returns>The merged url</returns>
        public static string Combine(string baseUrl, params string[] relativePaths)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                return null;

            if (relativePaths.Length == 0)
                return baseUrl;

            var currentUrl = Combine(baseUrl, relativePaths[0]);

            return Combine(currentUrl, relativePaths.Skip(1).ToArray());
        }
    }
}
