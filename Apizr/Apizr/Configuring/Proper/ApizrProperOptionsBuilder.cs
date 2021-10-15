using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Authenticating;
using Apizr.Caching;
using Apizr.Logging;
using Microsoft.Extensions.Logging;
using Polly.Registry;

namespace Apizr.Configuring.Proper
{
    public class ApizrProperOptionsBuilder : IApizrProperOptionsBuilder
    {
        protected readonly ApizrProperOptions Options;

        internal ApizrProperOptionsBuilder(ApizrProperOptions properOptions)
        {
            Options = properOptions;
        }

        public IApizrProperOptions ApizrOptions => Options;

        public IApizrProperOptionsBuilder WithBaseAddress(string baseAddress)
        {
            if (Uri.TryCreate(baseAddress, UriKind.RelativeOrAbsolute, out var baseUri))
                Options.BaseAddressFactory = () => baseUri;

            return this;
        }

        public IApizrProperOptionsBuilder WithBaseAddress(Uri baseAddress)
        {
            Options.BaseAddressFactory = () => baseAddress;

            return this;
        }

        public IApizrProperOptionsBuilder WithBaseAddress(Func<string> baseAddressFactory)
        {
            Options.BaseAddressFactory = () =>
                Uri.TryCreate(baseAddressFactory.Invoke(), UriKind.RelativeOrAbsolute, out var baseUri)
                    ? baseUri
                    : null;

            return this;
        }

        public IApizrProperOptionsBuilder WithBaseAddress(Func<Uri> baseAddressFactory)
        {
            Options.BaseAddressFactory = baseAddressFactory;

            return this;
        }

        public IApizrProperOptionsBuilder WithHttpClientHandler(HttpClientHandler httpClientHandler)
            => WithHttpClientHandler(() => httpClientHandler);

        public IApizrProperOptionsBuilder WithHttpClientHandler(Func<HttpClientHandler> httpClientHandlerFactory)
        {
            Options.HttpClientHandlerFactory = httpClientHandlerFactory;

            return this;
        }

        public IApizrProperOptionsBuilder WithAuthenticationHandler(Func<HttpRequestMessage, Task<string>> refreshTokenFactory)
        {
            var authenticationHandler = new Func<ILogger, IApizrOptionsBase, DelegatingHandler>((logger, options) =>
                new AuthenticationHandler(logger, options, refreshTokenFactory));
            Options.DelegatingHandlersFactories.Add(authenticationHandler);

            return this;
        }

        public IApizrProperOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(Func<ILogger, IApizrOptionsBase, TAuthenticationHandler> authenticationHandlerFactory) where TAuthenticationHandler : AuthenticationHandlerBase
        {
            Options.DelegatingHandlersFactories.Add(authenticationHandlerFactory);

            return this;
        }

        public IApizrProperOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(TSettingsService settingsService,
            Expression<Func<TSettingsService, string>> tokenProperty, TTokenService tokenService, Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod)
            => WithAuthenticationHandler(() => settingsService, tokenProperty, () => tokenService, refreshTokenMethod);

        public IApizrProperOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(Func<TSettingsService> settingsServiceFactory,
            Expression<Func<TSettingsService, string>> tokenProperty, Func<TTokenService> tokenServiceFactory, Expression<Func<TTokenService, HttpRequestMessage, Task<string>>> refreshTokenMethod)
        {
            var authenticationHandler = new Func<ILogger, IApizrOptionsBase, DelegatingHandler>((logHger, options) =>
                new AuthenticationHandler<TSettingsService, TTokenService>(logHger,
                    options,
                    settingsServiceFactory, tokenProperty,
                    tokenServiceFactory, refreshTokenMethod));
            Options.DelegatingHandlersFactories.Add(authenticationHandler);

            return this;
        }

        public IApizrProperOptionsBuilder WithAuthenticationHandler<TSettingsService>(TSettingsService settingsService,
            Expression<Func<TSettingsService, string>> tokenProperty, Func<HttpRequestMessage, Task<string>> refreshTokenFactory)
            => WithAuthenticationHandler(() => settingsService, tokenProperty, refreshTokenFactory);

        public IApizrProperOptionsBuilder WithAuthenticationHandler<TSettingsService>(Func<TSettingsService> settingsServiceFactory,
            Expression<Func<TSettingsService, string>> tokenProperty, Func<HttpRequestMessage, Task<string>> refreshTokenFactory)
        {
            var authenticationHandler = new Func<ILogger, IApizrOptionsBase, DelegatingHandler>((logger, options) =>
                new AuthenticationHandler<TSettingsService>(logger, options, settingsServiceFactory, tokenProperty, refreshTokenFactory));
            Options.DelegatingHandlersFactories.Add(authenticationHandler);

            return this;
        }

        public IApizrProperOptionsBuilder AddDelegatingHandler(DelegatingHandler delegatingHandler)
            => AddDelegatingHandler(_ => delegatingHandler);

        public IApizrProperOptionsBuilder AddDelegatingHandler(Func<ILogger, DelegatingHandler> delegatingHandlerFactory)
            => AddDelegatingHandler((logger, _) => delegatingHandlerFactory(logger));

        public IApizrProperOptionsBuilder AddDelegatingHandler(Func<ILogger, IApizrOptionsBase, DelegatingHandler> delegatingHandlerFactory)
        {
            Options.DelegatingHandlersFactories.Add(delegatingHandlerFactory);

            return this;
        }

        public IApizrProperOptionsBuilder WithLogging(HttpTracerMode httpTracerMode = HttpTracerMode.Everything,
            HttpMessageParts trafficVerbosity = HttpMessageParts.All, LogLevel logLevel = LogLevel.Information)
            => WithLogging(() => httpTracerMode, () => trafficVerbosity, () => logLevel);

        public IApizrProperOptionsBuilder WithLogging(Func<HttpTracerMode> httpTracerModeFactory, Func<HttpMessageParts> trafficVerbosityFactory, Func<LogLevel> logLevelFactory)
        {
            Options.HttpTracerModeFactory = httpTracerModeFactory;
            Options.TrafficVerbosityFactory = trafficVerbosityFactory;
            Options.LogLevelFactory = logLevelFactory;

            return this;
        }
    }
}
