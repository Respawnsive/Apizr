using System;
using Apizr.Configuring.Common;
using Apizr.Configuring.Shared;
using Fusillade;

namespace Apizr
{
    /// <summary>
    /// Fusillade options builder extensions
    /// </summary>
    public static class FusilladeOptionsBuilderExtensions
    {
        /// <summary>
        /// Tells Apizr to manage request priority
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static T WithPriority<T>(this T builder)
            where T : IApizrGlobalSharedRegistrationOptionsBuilderBase
        {
            builder.SetPrimaryHttpMessageHandler((innerHandler, logger, options) =>
                new PriorityHttpMessageHandler(innerHandler, logger, options));

            return builder;
        }

        /// <summary>
        /// Tells Apizr to manage request priority
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public static T WithPriority<T>(this T builder, Priority priority)
            where T : IApizrGlobalSharedOptionsBuilderBase
            => builder.WithPriority((int)priority);

        /// <summary>
        /// Tells Apizr to manage request priority
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public static T WithPriority<T>(this T builder, int priority)
            where T : IApizrGlobalSharedOptionsBuilderBase
        {
            if(builder is IApizrGlobalCommonOptionsBuilderBase commonBuilder)
                commonBuilder.SetPrimaryHttpMessageHandler((innerHandler, logger, options) =>
                    new PriorityHttpMessageHandler(innerHandler, logger, options));

            if (builder is IApizrVoidOptionsBuilderBase voidBuilder)
                voidBuilder.SetHandlerParameter(Constants.PriorityKey, priority);

            return builder;
        }
    }
}
