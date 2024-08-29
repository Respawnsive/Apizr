using System;

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
        GetOrFetch
    }
}
