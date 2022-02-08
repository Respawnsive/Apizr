using Apizr.Configuring.Common;
using AutoMapper;

namespace Apizr
{
    public static class AutoMapperOptionsBuilderExtensions
    {
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
            builder.SetMappingHandlerFactory(() => new AutoMapperMappingHandler(configuration.CreateMapper()));

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
