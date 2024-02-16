// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Copied from https://github.com/dotnet/extensions/blob/main/src/Libraries/Microsoft.Extensions.Http.Resilience/Resilience but without any DI ref plus some adjustments

using System;
using System.Net.Http;
using System.Threading;
using Apizr.Configuring.Request;
using Polly;

namespace Apizr.Resiliencing
{
    /// <summary>
    /// Extension methods for <see cref="HttpRequestMessage"/> Polly integration.
    /// </summary>
    public static class HttpRequestMessageApizrExtensions
    {
        /// <summary>
        /// Gets the <see cref="ResilienceContext"/> associated with the provided <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The <see cref="ResilienceContext"/> if set, otherwise <c>null</c>.</returns>
        /// <remarks>
        /// The <see cref="ResilienceHttpMessageHandler"/> will attach a context to the <see cref="HttpResponseMessage"/> prior
        /// to executing a <see cref="ResiliencePipeline"/>, if one does not already exist. The <see cref="ResilienceContext"/> will be provided
        /// to the resilience pipeline for use inside the <see cref="ResilienceContext"/> and in other message handlers.
        /// </remarks>
        public static ResilienceContext GetOrBuildApizrResilienceContext(this HttpRequestMessage request, CancellationToken cancellationToken)
        {
#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(request);

            var context = request.GetApizrResilienceContext();
            if (context == null && 
                request.Options.TryGetValue(Constants.InterfaceTypeOptionsKey, out var interfaceType))
                context = ResilienceContextPool.Shared.Get(interfaceType.Name, cancellationToken);
#else
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var context = request.GetApizrResilienceContext();
            if (context == null && 
                request.Properties.TryGetValue(Constants.InterfaceTypeKey, out var interfaceTypeProperty) && 
                interfaceTypeProperty is Type interfaceType)
            {
                context = ResilienceContextPool.Shared.Get(interfaceType.Name, cancellationToken);
            }
#endif

            return context;
        }

        /// <summary>
        /// Gets the <see cref="Context"/> associated with the provided <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
        /// <returns>The <see cref="Context"/> if set, otherwise <c>null</c>.</returns>
        /// <remarks>
        /// The <see cref="ResilienceHttpMessageHandler"/> will attach a context to the <see cref="HttpResponseMessage"/> prior
        /// to executing a <see cref="ResiliencePipeline"/>, if one does not already exist. The <see cref="Context"/> will be provided
        /// to the Resilience Pipeline for use inside the <see cref="ResiliencePipeline"/> and in other message handlers.
        /// </remarks>
        public static ResilienceContext GetApizrResilienceContext(this HttpRequestMessage request)
        {
#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(request);

            if (request.Options.TryGetValue(Constants.ResilienceContextOptionsKey, out var context))
                return context;

            if (request.Options.TryGetValue(Constants.ApizrRequestOptionsOptionsKey, out var apizrOptions))
                return apizrOptions.ResilienceContext;
#else
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Properties.TryGetValue(Constants.ResilienceContextKey, out var contextRaw) && 
                contextRaw is ResilienceContext context)
                return context;

            if (request.Properties.TryGetValue(Constants.ApizrRequestOptionsKey, out var optionsProperty) &&
                optionsProperty is IApizrRequestOptions optionsValue)
                return optionsValue.ResilienceContext;
#endif

            return null;
        }

        /// <summary>
        /// Gets the <see cref="IApizrRequestOptions"/> associated with the provided <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
        /// <returns>The <see cref="IApizrRequestOptions"/> if set, otherwise <c>null</c>.</returns>
        public static IApizrRequestOptions GetApizrRequestOptions(this HttpRequestMessage request)
        {
#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(request);

            if (request.Options.TryGetValue(Constants.ApizrRequestOptionsOptionsKey, out var apizrOptions))
                return apizrOptions;
#else
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Properties.TryGetValue(Constants.ApizrRequestOptionsKey, out var optionsProperty) &&
                optionsProperty is IApizrRequestOptions optionsValue)
                return optionsValue;
#endif

            return null;
        }

        /// <summary>
        /// Try to get the <see cref="IApizrRequestOptions"/> associated with the provided <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
        /// <param name="options">The <see cref="IApizrRequestOptions"/> if set, otherwise <c>null</c>.</param>
        /// <returns></returns>
        public static bool TryGetOptions(this HttpRequestMessage request, out IApizrRequestOptions options)
        {
            options = request?.GetApizrRequestOptions();

            return options != null;
        }

        /// <summary>
        /// Sets the <see cref="ResilienceContext"/> associated with the provided <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
        /// <param name="context">The <see cref="ResilienceContext"/>, may be <c>null</c>.</param>
        /// <remarks>
        /// The <see cref="ResilienceHttpMessageHandler"/> will attach a context to the <see cref="HttpResponseMessage"/> prior
        /// to executing a <see cref="ResiliencePipeline"/>, if one does not already exist. The <see cref="ResilienceContext"/> will be provided
        /// to the strategy for use inside the <see cref="ResiliencePipeline"/> and in other message handlers.
        /// </remarks>
        public static void SetApizrResilienceContext(this HttpRequestMessage request, ResilienceContext context)
        {
#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(request);

            request.Options.Set(Constants.ResilienceContextOptionsKey, context);
#else
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            request.Properties[Constants.ResilienceContextKey] = context;
#endif
        }
    }
}
