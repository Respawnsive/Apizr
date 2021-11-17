using System;
using System.Runtime.CompilerServices;
using Apizr.Configuring.Common;
using Apizr.Mapping;

[assembly: InternalsVisibleTo("Apizr.Integrations.AutoMapper")]
namespace Apizr
{
    internal static class InternalExtensions
    {
        #region MappingHandler

        private static Func<IServiceProvider, IMappingHandler> _mappingHandlerFactory;

        internal static void SetMappingHandlerFactory(this IApizrCommonOptionsBuilderBase builder,
            Func<IServiceProvider, IMappingHandler> mappingHandlerFactory)
        {
            _mappingHandlerFactory = mappingHandlerFactory;
        }

        internal static Func<IServiceProvider, IMappingHandler> GetMappingHanderFactory(this IApizrCommonOptionsBase builder) =>
            _mappingHandlerFactory; 

        #endregion
    }
}
