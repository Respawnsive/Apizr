// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Copied from https://github.com/dotnet/extensions/tree/release/3.1/src/HttpClientFactory/Polly/src but without any DI ref plus some adjustments

using System;
using System.Net.Http;
using Apizr.Configuring.Request;
using Polly;

namespace Apizr.Policing
{
    /// <summary>
    /// Extension methods for <see cref="HttpRequestMessage"/> Polly integration.
    /// </summary>
    public static class HttpRequestMessageApizrExtensions
    {
        /// <summary>
        /// Gets the <see cref="Context"/> associated with the provided <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
        /// <returns>The <see cref="Context"/> if set, otherwise <c>null</c>.</returns>
        /// <remarks>
        /// The <see cref="PolicyHttpMessageHandler"/> will attach a context to the <see cref="HttpResponseMessage"/> prior
        /// to executing a <see cref="Policy"/>, if one does not already exist. The <see cref="Context"/> will be provided
        /// to the policy for use inside the <see cref="Policy"/> and in other message handlers.
        /// </remarks>
        public static Context GetOrBuildApizrPolicyExecutionContext(this HttpRequestMessage request)
        {
            var context = request.GetApizrPolicyExecutionContext();
            if (context == null)
            {
                var interfaceType = (Type)request.Properties[Constants.InterfaceTypeKey];
                context = new Context(interfaceType.Name);
            }

            return context;
        }

        /// <summary>
        /// Gets the <see cref="Context"/> associated with the provided <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
        /// <returns>The <see cref="Context"/> if set, otherwise <c>null</c>.</returns>
        /// <remarks>
        /// The <see cref="PolicyHttpMessageHandler"/> will attach a context to the <see cref="HttpResponseMessage"/> prior
        /// to executing a <see cref="Policy"/>, if one does not already exist. The <see cref="Context"/> will be provided
        /// to the policy for use inside the <see cref="Policy"/> and in other message handlers.
        /// </remarks>
        public static Context GetApizrPolicyExecutionContext(this HttpRequestMessage request)
        {
            if (request.Properties.TryGetValue(Constants.PollyExecutionContextKey, out var contextProperty) &&
                contextProperty is Context context)
                return context;

            if (request.Properties.TryGetValue(Constants.ApizrRequestOptionsKey, out var optionsProperty) &&
                optionsProperty is IApizrRequestOptions optionsValue)
                return optionsValue.Context;

            return null;
        }

        /// <summary>
        /// Gets the <see cref="IApizrRequestOptions"/> associated with the provided <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
        /// <returns>The <see cref="IApizrRequestOptions"/> if set, otherwise <c>null</c>.</returns>
        public static IApizrRequestOptions GetApizrRequestOptions(this HttpRequestMessage request) =>
            request.Properties.TryGetValue(Constants.ApizrRequestOptionsKey, out var optionsProperty) &&
            optionsProperty is IApizrRequestOptions optionsValue
                ? optionsValue
                : null;

        /// <summary>
        /// Try to get the <see cref="IApizrRequestOptions"/> associated with the provided <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
        /// <param name="options">The <see cref="IApizrRequestOptions"/> if set, otherwise <c>null</c>.</param>
        /// <returns></returns>
        public static bool TryGetOptions(this HttpRequestMessage request, out IApizrRequestOptions options)
        {
            options = request.GetApizrRequestOptions();

            return options != null;
        }

        /// <summary>
        /// Sets the <see cref="Context"/> associated with the provided <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
        /// <param name="context">The <see cref="Context"/>, may be <c>null</c>.</param>
        /// <remarks>
        /// The <see cref="PolicyHttpMessageHandler"/> will attach a context to the <see cref="HttpResponseMessage"/> prior
        /// to executing a <see cref="Policy"/>, if one does not already exist. The <see cref="Context"/> will be provided
        /// to the policy for use inside the <see cref="Policy"/> and in other message handlers.
        /// </remarks>
        public static void SetApizrPolicyExecutionContext(this HttpRequestMessage request, Context context)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            request.Properties[Constants.PollyExecutionContextKey] = context;
        }
    }
}
