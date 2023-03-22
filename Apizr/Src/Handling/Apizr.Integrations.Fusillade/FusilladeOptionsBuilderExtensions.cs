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
        [Obsolete("Use WithPriority instead")]
        public static T WithPriorityManagement<T>(this T builder)
            where T : IApizrGlobalCommonOptionsBuilderBase
        {
            if (builder is IApizrGlobalSharedRegistrationOptionsBuilderBase registrationBuilder)
                registrationBuilder.WithPriority();

            return builder;
        }

        /// <summary>
        /// Enables priority management with Apizr
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static T WithPriority<T>(this T builder)
            where T : IApizrGlobalSharedRegistrationOptionsBuilderBase
        {
            if (builder is IApizrInternalRegistrationOptionsBuilder registrationBuilder)
                registrationBuilder.SetPrimaryHttpMessageHandler((innerHandler, logger, options) =>
                    new PriorityHttpMessageHandler(innerHandler, logger, options));

            return builder;
        }

        /// <summary>
        /// Tells Apizr to manage request with a priority
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="priority">The priority to manage the request with</param>
        /// <returns></returns>
        public static T WithPriority<T>(this T builder, Priority priority)
            where T : IApizrGlobalSharedOptionsBuilderBase
            => builder.WithPriority((int)priority);

        /// <summary>
        /// Tells Apizr to manage request with a priority
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="priority">The priority to manage the request with</param>
        /// <returns></returns>
        public static T WithPriority<T>(this T builder, int priority)
            where T : IApizrGlobalSharedOptionsBuilderBase
        {
            if (builder is IApizrInternalRegistrationOptionsBuilder registrationBuilder)
                registrationBuilder.SetPrimaryHttpMessageHandler((innerHandler, logger, options) =>
                    new PriorityHttpMessageHandler(innerHandler, logger, options));

            if (builder is IApizrInternalOptionsBuilder voidBuilder)
                voidBuilder.SetHandlerParameter(Constants.PriorityKey, priority);

            return builder;
        }
    }
}
