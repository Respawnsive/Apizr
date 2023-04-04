using Apizr.Configuring.Common;
using Mapster;
using MapsterMapper;

namespace Apizr
{
    /// <summary>
    /// Mapster options builder extensions
    /// </summary>
    public static class MapsterOptionsBuilderExtensions
    {
        /// <summary>
        /// Set Mapster as MappingHandler
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <param name="builder"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static TBuilder WithMapsterMappingHandler<TBuilder>(this TBuilder builder, IMapper mapper)
            where TBuilder : IApizrCommonOptionsBuilderBase
        {
            builder.SetMappingHandlerFactory(() => new MapsterMappingHandler(mapper));

            return builder;
        }


        /// <summary>
        /// Set Mapster as MappingHandler
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static TBuilder WithMapsterMappingHandler<TBuilder>(this TBuilder builder)
            where TBuilder : IApizrExtendedCommonOptionsBuilderBase
        {
            builder.SetMappingHandlerType<MapsterMappingHandler>();

            return builder;
        }
    }
}
