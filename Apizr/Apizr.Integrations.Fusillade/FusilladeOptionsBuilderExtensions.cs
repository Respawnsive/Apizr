using Apizr.Configuring.Common;

namespace Apizr
{
    public static class FusilladeOptionsBuilderExtensions
    {
        public static T WithPriorityManagement<T>(this T builder) 
            where T : IApizrGlobalCommonOptionsBuilderBase
        {
            builder.SetPrimaryHttpMessageHandler((innerHandler, logger, options) => new PriorityHttpMessageHandler(innerHandler, logger, options));

            return builder;
        }
    }
}
