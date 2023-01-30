// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Copied from https://github.com/aspnet/AspNetWebStack/tree/main/src/System.Net.Http.Formatting but without any external ref and adjusted to Apizr needs

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Extending;

namespace Apizr.Progressing
{
    /// <summary>
    /// The <see cref="ApizrProgressHandler"/> provides a mechanism for getting progress event notifications
    /// when sending and receiving data in connection with exchanging HTTP requests and responses.
    /// </summary>
    public class ApizrProgressHandler : DelegatingHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApizrProgressHandler"/> class.
        /// </summary>
        public ApizrProgressHandler()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApizrProgressHandler"/> class.
        /// </summary>
        /// <param name="innerHandler">The inner handler to which this handler submits requests.</param>
        public ApizrProgressHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var progress = request.GetApizrProgress();
            if(progress != null)
                AddRequestProgress(request, progress);

            var response = await base.SendAsync(request, cancellationToken);

            if (progress != null && response is {Content: { }})
            {
                cancellationToken.ThrowIfCancellationRequested();
                await AddResponseProgressAsync(request, response, progress);
            }

            return response;
        }

        private static void AddRequestProgress(HttpRequestMessage request, IApizrProgress progress)
        {
            if (progress == null || request is not {Content: { }}) 
                return;
            
            request.Content = new ApizrProgressContent(request.Content, progress, request);
        }

        private static async Task AddResponseProgressAsync(HttpRequestMessage request, HttpResponseMessage response,
            IApizrProgress progress)
        {
            var stream = await response.Content.ReadAsStreamAsync();
            var progressStream = new ApizrProgressStream(stream, progress, request, response);
            HttpContent progressContent = new StreamContent(progressStream);
            response.Content.Headers.CopyTo(progressContent.Headers);
            response.Content = progressContent;
        }
    }
}