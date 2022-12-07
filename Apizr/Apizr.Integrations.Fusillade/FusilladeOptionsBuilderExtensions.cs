using System.Threading.Tasks;
using Apizr.Configuring.Common;
using Apizr.Configuring.Request;
using Fusillade;

namespace Apizr
{
    /// <summary>
    /// Fusillade options builder extensions
    /// </summary>
    public static class FusilladeOptionsBuilderExtensions
    {
        /// <summary>
        /// Tells Apizr to manage request priorities
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static T WithPriorityManagement<T>(this T builder) 
            where T : IApizrGlobalCommonOptionsBuilderBase
        {
            builder.SetPrimaryHttpMessageHandler((innerHandler, logger, options) => new PriorityHttpMessageHandler(innerHandler, logger, options));

            return builder;
        }

        public static IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
            WithPriority<TApizrRequestOptions, TApizrRequestOptionsBuilder>(
                this IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder> builder,
                Priority priority)
            where TApizrRequestOptions : IApizrRequestOptions
            where TApizrRequestOptionsBuilder :
            IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
            => builder.WithPriority((int) priority);

        public static IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
            WithPriority<TApizrRequestOptions, TApizrRequestOptionsBuilder>(
                this IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder> builder, int priority)
            where TApizrRequestOptions : IApizrRequestOptions
            where TApizrRequestOptionsBuilder :
            IApizrRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
        {
            builder.WithHandlerParameter(Constants.PriorityKey, priority);

            return builder;
        }
    }
}
