namespace Apizr.Caching
{
    public enum CacheMode
    {
        /// <summary>
        /// Returns fresh data when request succeed (no cache)
        /// </summary>
        None,

        /// <summary>
        /// Returns fresh data when request succeed otherwise cached one if exist (dynamic data)
        /// </summary>
        GetAndFetch,

        /// <summary>
        /// Returns cached data if we get some otherwise fresh one if request succeed (static data)
        /// </summary>
        GetOrFetch
    }
}
