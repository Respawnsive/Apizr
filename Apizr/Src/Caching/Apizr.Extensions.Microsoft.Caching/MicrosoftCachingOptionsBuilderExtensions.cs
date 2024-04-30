using Apizr.Configuring.Common;
using Apizr.Extending.Configuring.Common;

[assembly: Apizr.Preserve]
namespace Apizr
{
    /// <summary>
    /// Microsoft Caching options builder extensions
    /// </summary>
    public static class MicrosoftCachingOptionsBuilderExtensions
    {
        /// <summary>
        /// Use any registered IDistributedCache implementation
        /// </summary>
        /// <returns></returns>
        public static TBuilder WithDistributedCacheHandler<TBuilder, TCache>(this TBuilder builder) where TBuilder : IApizrExtendedCommonOptionsBuilderBase
        {
            builder.SetCacheHandlerType<DistributedCacheHandler<TCache>>();

            return builder;
        }


        /// <summary>
        /// Use any registered IMemoryCache implementation
        /// </summary>
        /// <returns></returns>
        public static TBuilder WithInMemoryCacheHandler<TBuilder>(this TBuilder builder) where TBuilder : IApizrExtendedCommonOptionsBuilderBase
        {
            builder.SetCacheHandlerType<InMemoryCacheHandler>();

            return builder;
        }
    }
}
