﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Apizr.Caching
{
    /// <summary>
    /// The cache handler method mapping interface
    /// Implement it to provide some caching features to Apizr
    /// </summary>
    public interface ICacheHandler
    {
        /// <summary>
        /// Map Apizr cache saving method to your cache handler method
        /// </summary>
        /// <param name="key">The key to cache at</param>
        /// <param name="value">The value to cache</param>
        /// <param name="lifeSpan">The optional life span</param>
        /// <param name="cancellationToken">An optional cancellation token</param>
        /// <returns></returns>
        Task SetAsync(string key, object value, TimeSpan? lifeSpan = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Map Apizr cache getting method to your cache handler method
        /// </summary>
        /// <typeparam name="T">The expected value type</typeparam>
        /// <param name="key">The key to get from</param>
        /// <param name="cancellationToken">An optional cancellation token</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Map Apizr cache removing method to your cache handler method
        /// </summary>
        /// <param name="key">The key to remove from</param>
        /// <param name="cancellationToken">An optional cancellation token</param>
        /// <returns></returns>
        Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Map Apizr cache clearing method to your cache handler method
        /// </summary>
        /// <param name="cancellationToken">An optional cancellation token</param>
        /// <returns></returns>
        Task ClearAsync(CancellationToken cancellationToken = default);
    }
}
