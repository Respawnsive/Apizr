// Copied from https://github.com/BSiLabs/HttpTracer/blob/fdba9af621a005626bcad74de9651248e56b6872/src/HttpTracer/HttpHandlerBuilder.cs
// but reshaped with Microsoft.Extensions.Logging and some more features

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Apizr.Configuring;
using Apizr.Configuring.Manager;
using Microsoft.Extensions.Logging;

namespace Apizr.Logging
{
    /// <summary>
    /// HttpHandler builder
    /// </summary>
    public class ExtendedHttpHandlerBuilder
    {
        private readonly IList<DelegatingHandler> _handlersList = new List<DelegatingHandler>();
        private readonly ExtendedHttpTracerHandler _rootHandler;
        private HttpMessageHandler _lastHandler;

        /// <summary>
        /// Underlying instance of the <typeparamref name="T:HttpTracer.HttpHandlerBuilder"/> class.
        /// </summary>
        public ExtendedHttpTracerHandler HttpTracerHandler => _rootHandler;

        /// <summary>
        /// Initializes a new instance of the <typeparamref name="T:ExtendedHttpTracerHandler"/> class.
        /// </summary>
        /// <param name="apizrOptions"></param>
        public ExtendedHttpHandlerBuilder(IApizrManagerOptionsBase apizrOptions) : this(new ExtendedHttpTracerHandler(null, apizrOptions)) { }

        /// <summary>
        /// Initializes a new instance of the <typeparamref name="T:ExtendedHttpTracerHandler"/> class.
        /// </summary>
        /// <param name="innerHandler">HttpClientHandler.</param>
        /// <param name="apizrOptions"></param>
        public ExtendedHttpHandlerBuilder(HttpClientHandler innerHandler, IApizrManagerOptionsBase apizrOptions) : this(new ExtendedHttpTracerHandler(innerHandler, apizrOptions)) { }

        /// <summary>
        /// Initializes a new instance of the <typeparamref name="T:ExtendedHttpTracerHandler"/> class.
        /// </summary>
        /// <param name="tracerHandler">Tracer handler.</param>
        public ExtendedHttpHandlerBuilder(ExtendedHttpTracerHandler tracerHandler)
        {
            _rootHandler = tracerHandler;
        }

        /// <summary>
        /// Adds a <see cref="HttpMessageHandler"/> to the chain of handlers.
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public ExtendedHttpHandlerBuilder AddHandler(HttpMessageHandler handler)
        {
            if (handler is ExtendedHttpTracerHandler) 
                throw new ArgumentException($"Can't add handler of type {nameof(HttpTracerHandler)}.");

            if (handler is DelegatingHandler delegatingHandler)
            {
                if (_handlersList.Any())
                    _handlersList.Last().InnerHandler = delegatingHandler;

                _handlersList.Add(delegatingHandler); 
            }
            else
            {
                _lastHandler = handler;
            }

            return this;
        }

        /// <summary>
        /// Adds <see cref="DelegatingHandler"/> as the last link of the chain.
        /// </summary>
        /// <returns></returns>
        public DelegatingHandler Build()
        {
            if(_lastHandler != null)
                _rootHandler.InnerHandler = _lastHandler;

            if (_handlersList.Any())
                _handlersList.Last().InnerHandler = _rootHandler;
            else
                return _rootHandler;

            return _handlersList.FirstOrDefault();
        }
    }
}
