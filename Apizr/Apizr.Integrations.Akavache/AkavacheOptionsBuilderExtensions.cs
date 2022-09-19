using System;
using Akavache;
using Apizr.Configuring.Common;

namespace Apizr
{
    /// <summary>
    /// Akavache options builder extensions
    /// </summary>
    public static class AkavacheOptionsBuilderExtensions
    {
        /// <summary>
        /// Set Akavache as CacheHandler with LocalMachine blob cache and ApizrAkavacheCacheHandler name
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static TBuilder WithAkavacheCacheHandler<TBuilder>(this TBuilder builder)
            where TBuilder : IApizrGlobalCommonOptionsBuilderBase
        {
            builder.SetCacheHandlerFactory(() => new AkavacheCacheHandler());

            return builder;
        }

        /// <summary>
        /// Set Akavache as CacheHandler with your blob cache and ApizrAkavacheCacheHandler name
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <param name="builder"></param>
        /// <param name="blobCacheFactory">The blob cache factory of your choice</param>
        /// <returns></returns>
        public static TBuilder WithAkavacheCacheHandler<TBuilder>(this TBuilder builder, Func<IBlobCache> blobCacheFactory)
            where TBuilder : IApizrGlobalCommonOptionsBuilderBase
        {
            builder.SetCacheHandlerFactory(() => new AkavacheCacheHandler(blobCacheFactory));

            return builder;
        }

        /// <summary>
        /// Set Akavache as CacheHandler with LocalMachine blob cache and your provided name
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <param name="builder"></param>
        /// <param name="applicationName">The application name used by Akavache</param>
        /// <returns></returns>
        public static TBuilder WithAkavacheCacheHandler<TBuilder>(this TBuilder builder, string applicationName) 
            where TBuilder : IApizrGlobalCommonOptionsBuilderBase
        {
            builder.SetCacheHandlerFactory(() => new AkavacheCacheHandler(applicationName));

            return builder;
        }

        /// <summary>
        /// Set Akavache as CacheHandler with your blob cache and your provided name
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <param name="builder"></param>
        /// <param name="blobCacheFactory">The blob cache factory of your choice</param>
        /// <param name="applicationName">The application name used by Akavache</param>
        /// <returns></returns>
        public static TBuilder WithAkavacheCacheHandler<TBuilder>(this TBuilder builder, Func<IBlobCache> blobCacheFactory, string applicationName) 
            where TBuilder : IApizrGlobalCommonOptionsBuilderBase
        {
            builder.SetCacheHandlerFactory(() => new AkavacheCacheHandler(blobCacheFactory, applicationName));

            return builder;
        }
    }
}
