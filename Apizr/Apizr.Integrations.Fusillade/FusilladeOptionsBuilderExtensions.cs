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

        public static IApizrResultRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
            WithPriority<TApizrRequestOptions, TApizrRequestOptionsBuilder>(
                this IApizrResultRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder> builder,
                Priority priority)
            where TApizrRequestOptions : IApizrResultRequestOptions
            where TApizrRequestOptionsBuilder :
            IApizrResultRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
            => builder.WithPriority((int) priority);

        public static IApizrResultRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
            WithPriority<TApizrRequestOptions, TApizrRequestOptionsBuilder>(
                this IApizrResultRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder> builder, int priority)
            where TApizrRequestOptions : IApizrResultRequestOptions
            where TApizrRequestOptionsBuilder :
            IApizrResultRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
        {
            builder.AddHandlerParameter(Constants.PriorityKey, priority);

            return builder;
        }
    }
}
