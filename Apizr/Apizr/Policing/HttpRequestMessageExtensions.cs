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
    public static class HttpRequestMessageExtensions
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
        public static Context GetOrBuildPolicyExecutionContext(this HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var context = request.GetPolicyExecutionContext();
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
        public static Context GetPolicyExecutionContext(this HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            Context context = null;
            if (request.Properties.TryGetValue(Constants.PollyExecutionContextKey, out var contextProperty) && contextProperty is Context contextValue)
            {
                context = contextValue;
            }
            else if (request.Properties.TryGetValue(Constants.ApizrRequestOptionsKey, out var optionsProperty) && optionsProperty is IApizrRequestOptions optionsValue)
            {
                context = optionsValue.Context;
            }
            return context;
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
        public static void SetPolicyExecutionContext(this HttpRequestMessage request, Context context)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            request.Properties[Constants.PollyExecutionContextKey] = context;
        }
    }
}
