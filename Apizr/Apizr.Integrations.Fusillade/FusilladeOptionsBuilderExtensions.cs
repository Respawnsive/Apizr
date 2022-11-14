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

        public static T WithPriority<T>(this T builder, Priority priority)
            where T : IApizrResultRequestOptionsBuilderBase
            => builder.WithPriority((int) priority);

        public static T WithPriority<T>(this T builder, int priority)
            where T : IApizrResultRequestOptionsBuilderBase
        {
            builder.ApizrOptions.HandlersParameters[Constants.PriorityKey] = priority;

            return builder;
        }
    }
}
