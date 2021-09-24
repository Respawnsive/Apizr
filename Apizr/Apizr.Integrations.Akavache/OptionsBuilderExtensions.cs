using Akavache;

namespace Apizr
{
    public static partial class OptionsBuilderExtensions
    {
        public static TBuilder WithCacheHandler<TBuilder, THandler>(this TBuilder builder, IBlobCache blobCache)
            where TBuilder : IApizrOptionsBuilderBase where THandler : AkavacheCacheHandler
        {
            builder.SetCacheHandlerFactory(() => new AkavacheCacheHandler(blobCache));

            return builder;
        }

        public static TBuilder WithCacheHandler<TBuilder, THandler>(this TBuilder builder, string applicationName) where TBuilder : IApizrOptionsBuilderBase where THandler : AkavacheCacheHandler
        {
            builder.SetCacheHandlerFactory(() => new AkavacheCacheHandler(applicationName));

            return builder;
        }

        public static TBuilder WithCacheHandler<TBuilder, THandler>(this TBuilder builder, IBlobCache blobCache, string applicationName) where TBuilder : IApizrOptionsBuilderBase where THandler : AkavacheCacheHandler
        {
            builder.SetCacheHandlerFactory(() => new AkavacheCacheHandler(blobCache, applicationName));

            return builder;
        }
    }
}
