using Apizr.Extending.Configuring;

[assembly: Apizr.Preserve]
namespace Apizr
{
    public static class OptionsBuilderExtensions
    {
        /// <summary>
        /// Use any registered IDistributedCache implementation
        /// </summary>
        /// <returns></returns>
        public static TBuilder WithDistributedCacheHandler<TBuilder, TCache>(this TBuilder builder) where TBuilder : IApizrExtendedOptionsBuilder
        {
            builder.WithCacheHandler<DistributedCacheHandler<TCache>>();

            return builder;
        }


        /// <summary>
        /// Use any registered IMemoryCache implementation
        /// </summary>
        /// <returns></returns>
        public static TBuilder WithInMemoryCacheHandler<TBuilder>(this TBuilder builder) where TBuilder : IApizrExtendedOptionsBuilder
        {
            builder.WithCacheHandler<InMemoryCacheHandler>();

            return builder;
        }
    }
}
