using System;
using Refit;

namespace Apizr.Caching
{
    /// <summary>
    /// Define the caching behaviour
    /// </summary>
    public enum CacheMode
    {
        /// <summary>
        /// Returns fresh data when request succeed (api data only)
        /// </summary>
        None,

        /// <summary>
        /// Returns fresh data when request succeed otherwise cached one if exist (api data first)
        /// </summary>
        FetchOrGet,

        /// <summary>
        /// Returns cached data if we get some otherwise fresh one if request succeed (cache data first)
        /// </summary>
        GetOrFetch,

        /// <summary>
        /// <para>Relies on one of the following response header presence, ordered by precedence (otherwise None):</para>
        /// <para>1. Cache-Control (GetOrFetch): Controls how Apizr should cache the data (e.g., max-age, no-store, immutable, etc.).</para>
        /// <para>2. Expires (GetOrFetch): Specifies a date after which Apizr should fetch api data again.</para>
        /// <para>3. ETag (FetchOrGet): Ask Apizr to use the If-None-Match header to check if the data has been modified and handle any 304 Not Modified response.</para>
        /// <para>4. Last-Modified (FetchOrGet): Ask Apizr to use the If-Modified-Since header to check if the resource has been modified and handle any 304 Not Modified response.</para>
        /// </summary>
        /// <remarks>REQUIRED: Works only with <see cref="IApiResponse{T}"/> result while designing api interface</remarks>
        SetByHeader
    }
}