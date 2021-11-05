using Apizr.Configuring;
using Apizr.Configuring.Common;
using Apizr.Configuring.Shared;

namespace Apizr
{
    public static class OptionsBuilderExtensions
    {
        public static T WithPriorityManagement<T>(this T builder) where T : IApizrCommonOptionsBuilderBase
        {
            builder.SetPrimaryHttpMessageHandler((innerHandler, logger, options) => new PriorityHttpMessageHandler(innerHandler, logger, options));

            return builder;
        }
    }
}
