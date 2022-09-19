using Apizr.Configuring.Common;

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
    }
}
