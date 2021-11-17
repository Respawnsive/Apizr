using System;
using Apizr.Configuring.Common;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Apizr
{
    public static class OptionsBuilderExtensions
    {
        /// <summary>
        /// Set AutoMapper as MappingHandler
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static TBuilder WithAutoMapperMappingHandler<TBuilder>(this TBuilder builder)
            where TBuilder : IApizrCommonOptionsBuilderBase
        {
            builder.SetMappingHandlerFactory(serviceProvider => new AutoMapperMappingHandler(serviceProvider.GetRequiredService<IMapper>()));

            return builder;
        }
    }
}
