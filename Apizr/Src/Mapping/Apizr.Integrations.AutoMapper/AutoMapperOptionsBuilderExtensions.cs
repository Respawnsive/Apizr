using Apizr.Configuring.Common;
using AutoMapper;

namespace Apizr
{
    /// <summary>
    /// AutoMapper options builder extensions
    /// </summary>
    public static class AutoMapperOptionsBuilderExtensions
    {
        /// <summary>
        /// Set AutoMapper as MappingHandler
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <param name="builder"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static TBuilder WithAutoMapperMappingHandler<TBuilder>(this TBuilder builder, IMapper mapper)
            where TBuilder : IApizrCommonOptionsBuilderBase
        {
            builder.SetMappingHandlerInternalFactory(() => new AutoMapperMappingHandler(mapper));

            return builder;
        }

        /// <summary>
        /// Set AutoMapper as MappingHandler
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static TBuilder WithAutoMapperMappingHandler<TBuilder>(this TBuilder builder, IConfigurationProvider configuration)
            where TBuilder : IApizrCommonOptionsBuilderBase
        {
            builder.SetMappingHandlerInternalFactory(() => new AutoMapperMappingHandler(configuration.CreateMapper()));

            return builder;
        }


        /// <summary>
        /// Set AutoMapper as MappingHandler
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static TBuilder WithAutoMapperMappingHandler<TBuilder>(this TBuilder builder)
            where TBuilder : IApizrExtendedCommonOptionsBuilderBase
        {
            builder.SetMappingHandlerType<AutoMapperMappingHandler>();

            return builder;
        }
    }
}
