﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Authenticating;
using Apizr.Caching;
using Apizr.Caching.Attributes;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Request;
using Apizr.Configuring.Shared;
using Apizr.Configuring.Shared.Context;
using Apizr.Extending;
using Apizr.Logging;
using Apizr.Resiliencing.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Configuring.Proper
{
    /// <summary>
    /// Builder options available at proper level for static registrations
    /// </summary>
    public class ApizrProperOptionsBuilder : IApizrProperOptionsBuilder, IApizrInternalRegistrationOptionsBuilder
    {
        /// <summary>
        /// The proper options
        /// </summary>
        protected readonly ApizrProperOptions Options;

        internal ApizrProperOptionsBuilder(ApizrProperOptions properOptions)
        {
            Options = properOptions;
        }

        /// <inheritdoc />
        IApizrProperOptions IApizrProperOptionsBuilder.ApizrOptions => Options;

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithConfiguration(IConfiguration configuration)
            => WithConfiguration(configuration?.GetSection("Apizr"), null);

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithConfiguration(IConfigurationSection configurationSection)
            => WithConfiguration(configurationSection, null);

        private IApizrProperOptionsBuilder WithConfiguration(IConfigurationSection configurationSection, string requestName)
        {
            if (configurationSection is not null)
            {
                var isRequestOptions = !string.IsNullOrWhiteSpace(requestName);
                var apiName = Options.CrudApiEntityType?.Name ?? Options.WebApiType.Name;
                var configs = configurationSection.GetChildren().Where(config =>
                    config.Key == "Apizr" || 
                    config.Key == "ProperOptions" || config.Path.Contains($"Apizr:ProperOptions:{apiName}") ||
                    config.Key == "RequestOptions" || config.Path.Contains($"Apizr:ProperOptions:{apiName}:RequestOptions") ||
                    Constants.ConfigurableSettings.Contains(config.Key));
                foreach (var config in configs)
                {
                    switch (config.Key)
                    {
                        case "BaseAddress":
                            WithBaseAddress(config.Value);
                            break;
                        case "BasePath":
                            WithBasePath(config.Value);
                            break;
                        case "OperationTimeout":
                            if (isRequestOptions)
                                WithRequestOptions(requestName, requestOptions => requestOptions.WithOperationTimeout(TimeSpan.Parse(config.Value!)));
                            else
                                WithOperationTimeout(TimeSpan.Parse(config.Value!));
                            break;
                        case "RequestTimeout":
                            if (isRequestOptions)
                                WithRequestOptions(requestName, requestOptions => requestOptions.WithRequestTimeout(TimeSpan.Parse(config.Value!)));
                            else
                                WithRequestTimeout(TimeSpan.Parse(config.Value!));
                            break;
                        case "Logging":
                            {
                                var logSection = config.GetChildren().ToList();
                                var httpTracerModeValue = logSection.FirstOrDefault(c => c.Key == "HttpTracerMode")?.Value;
                                var httpTracerMode = !string.IsNullOrEmpty(httpTracerModeValue) ? (HttpTracerMode)Enum.Parse(typeof(HttpTracerMode), httpTracerModeValue) : Constants.DefaultHttpTracerMode;
                                var trafficVerbosityValue = logSection.FirstOrDefault(c => c.Key == "TrafficVerbosity")?.Value;
                                var trafficVerbosity = !string.IsNullOrEmpty(trafficVerbosityValue) ? (HttpMessageParts)Enum.Parse(typeof(HttpMessageParts), trafficVerbosityValue) : Constants.DefaultTrafficVerbosity;
                                var logLevelsValue = logSection.FirstOrDefault(c => c.Key == "LogLevels");
                                var logLevels = logLevelsValue?.GetChildren().Select(c => (LogLevel)Enum.Parse(typeof(LogLevel), c.Value!)).ToArray() ?? Constants.DefaultLogLevels;

                                if (isRequestOptions)
                                    WithRequestOptions(requestName, requestOptions => requestOptions.WithLogging(httpTracerMode, trafficVerbosity, logLevels));
                                else
                                    WithLogging(httpTracerMode, trafficVerbosity, logLevels);

                                break;
                            }
                        case "Headers":
                            if (isRequestOptions)
                                WithRequestOptions(requestName, requestOptions => requestOptions.WithHeaders(config.GetChildren().Select(c => c.Value!).ToList()));
                            else
                                WithHeaders(config.GetChildren().Select(c => c.Value!).ToList());
                            break;
                        case "LoggedHeadersRedactionNames":
                            if (isRequestOptions)
                                WithRequestOptions(requestName, requestOptions => requestOptions.WithLoggedHeadersRedactionNames(config.GetChildren().Select(c => c.Value!).ToList()));
                            else
                                WithLoggedHeadersRedactionNames(config.GetChildren().Select(c => c.Value!).ToList());
                            break;
                        case "ContinueOnCapturedContext":
                            WithResilienceContextOptions(options => options.ContinueOnCapturedContext(bool.Parse(config.Value!)));
                            break;
                        case "ReturnContextToPoolOnComplete":
                            WithResilienceContextOptions(options => options.ReturnToPoolOnComplete(bool.Parse(config.Value!)));
                            break;
                        case "ResiliencePipelineKeys":
                            if (isRequestOptions)
                                WithRequestOptions(requestName, requestOptions => requestOptions.WithResiliencePipelineKeys(config.GetChildren().Select(c => c.Value!).ToArray()));
                            else
                                WithResiliencePipelineKeys(config.GetChildren().Select(c => c.Value!).ToArray());
                            break;
                        case "ResiliencePipelineOptions":
                            {
                                var resiliencePipelineOptions = config.GetChildren().ToList();
                                foreach (var resiliencePipelineOption in resiliencePipelineOptions)
                                {
                                    if (!ApizrRequestMethod.TryParse(resiliencePipelineOption.Key, out var requestMethod))
                                        continue;

                                    var resiliencePipelineKeys = resiliencePipelineOption.GetChildren().Select(c => c.Value!).ToArray();
                                    WithResiliencePipelineKeys(resiliencePipelineKeys, [requestMethod]);
                                }

                                break;
                            }
                        case "Caching":
                            {
                                var cacheSection = config.GetChildren().ToList();
                                var modeValue = cacheSection.FirstOrDefault(c => c.Key == "Mode")?.Value;
                                var mode = !string.IsNullOrEmpty(modeValue) ? (CacheMode)Enum.Parse(typeof(CacheMode), modeValue) : CacheMode.FetchOrGet;
                                var lifeSpanValue = cacheSection.FirstOrDefault(c => c.Key == "LifeSpan")?.Value;
                                var lifeSpan = !string.IsNullOrEmpty(lifeSpanValue) ? TimeSpan.Parse(lifeSpanValue) : TimeSpan.Zero;
                                var shouldInvalidateOnErrorValue = cacheSection.FirstOrDefault(c => c.Key == "ShouldInvalidateOnError")?.Value;
                                var shouldInvalidateOnError = !string.IsNullOrEmpty(shouldInvalidateOnErrorValue) && bool.Parse(shouldInvalidateOnErrorValue);

                                if (isRequestOptions)
                                    WithRequestOptions(requestName, requestOptions => requestOptions.WithCaching(mode, lifeSpan, shouldInvalidateOnError));
                                else
                                    WithCaching(mode, lifeSpan, shouldInvalidateOnError);

                                break;
                            }
                        case "Priority":
                            WithHandlerParameter(Constants.PriorityKey, config.Value);
                            break;
                        default:
                            {
                                if (config.GetChildren().Any())
                                {
                                    if (string.IsNullOrWhiteSpace(requestName))
                                        requestName = Options.RequestNames.Contains(config.Key) ? config.Key : null;

                                    WithConfiguration(config, requestName);
                                }

                                break;
                            }
                    }
                }
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithBaseAddress(string baseAddress, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithBaseAddress(() => baseAddress, strategy);

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithBaseAddress(Func<string> baseAddressFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.BaseAddressFactory ??= baseAddressFactory;
                    break;
                default:
                    Options.BaseAddressFactory = baseAddressFactory;
                    break;
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithBaseAddress(Uri baseAddress, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithBaseAddress(() => baseAddress, strategy);

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithBaseAddress(Func<Uri> baseAddressFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.BaseUriFactory ??= baseAddressFactory;
                    break;
                default:
                    Options.BaseUriFactory = baseAddressFactory;
                    break;
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithBasePath(string basePath, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithBasePath(() => basePath, strategy);

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithBasePath(Func<string> basePathFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.BasePathFactory ??= basePathFactory;
                    break;
                default:
                    Options.BasePathFactory = basePathFactory;
                    break;
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithHttpClientHandler(HttpClientHandler httpClientHandler)
            => WithHttpClientHandler(() => httpClientHandler);

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithHttpClientHandler(Func<HttpClientHandler> httpClientHandlerFactory)
        {
            Options.HttpClientHandlerFactory = httpClientHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrProperOptionsBuilder ConfigureHttpClient(Action<HttpClient> configureHttpClient,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Merge)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.HttpClientConfigurationBuilder ??= configureHttpClient;
                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.HttpClientConfigurationBuilder = configureHttpClient;
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.HttpClientConfigurationBuilder == null)
                    {
                        Options.HttpClientConfigurationBuilder = configureHttpClient;
                    }
                    else
                    {
                        Options.HttpClientConfigurationBuilder += configureHttpClient.Invoke;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(
            Func<ILogger, IApizrManagerOptionsBase, TAuthenticationHandler> authenticationHandlerFactory)
            where TAuthenticationHandler : AuthenticationHandlerBase
            => WithDelegatingHandler(authenticationHandlerFactory);

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithAuthenticationHandler(
            Func<HttpRequestMessage, CancellationToken, Task<string>> getTokenFactory)
            => WithDelegatingHandler((logger, options) =>
                new AuthenticationHandler(logger, options, getTokenFactory));

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithAuthenticationHandler(
            Func<HttpRequestMessage, CancellationToken, Task<string>> getTokenFactory,
            Func<HttpRequestMessage, string, CancellationToken, Task> setTokenFactory)
            => WithDelegatingHandler((logger, options) =>
                new AuthenticationHandler(logger, options, getTokenFactory, setTokenFactory));

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithAuthenticationHandler(
            Func<HttpRequestMessage, string, CancellationToken, Task<string>> refreshTokenFactory)
            => WithDelegatingHandler((logger, options) =>
                new AuthenticationHandler(logger, options, refreshTokenFactory));

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithAuthenticationHandler(
            Func<HttpRequestMessage, CancellationToken, Task<string>> getTokenFactory,
            Func<HttpRequestMessage, string, CancellationToken, Task> setTokenFactory,
            Func<HttpRequestMessage, string, CancellationToken, Task<string>> refreshTokenFactory)
            => WithDelegatingHandler((logger, options) =>
                new AuthenticationHandler(logger, options, getTokenFactory, setTokenFactory, refreshTokenFactory));

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            TSettingsService settingsService,
            Expression<Func<TSettingsService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression)
            => WithDelegatingHandler((logger, options) =>
            {
                var getTokenFactory = getTokenExpression.Compile();
                return new AuthenticationHandler(logger,
                    options,
                    (request, ct) => getTokenFactory.Invoke(settingsService, request, ct));
            });

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            TSettingsService settingsService,
            Expression<Func<TSettingsService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression,
            Expression<Func<TSettingsService, HttpRequestMessage, string, CancellationToken, Task>> setTokenExpression)
            => WithDelegatingHandler((logger, options) =>
            {
                var getTokenFactory = getTokenExpression.Compile();
                var setTokenFactory = setTokenExpression.Compile();
                return new AuthenticationHandler(logger,
                    options,
                    (request, ct) => getTokenFactory.Invoke(settingsService, request, ct),
                    (request, token, ct) => setTokenFactory.Invoke(settingsService, request, token, ct));
            });

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithAuthenticationHandler<TTokenService>(TTokenService tokenService,
            Expression<Func<TTokenService, HttpRequestMessage, string, CancellationToken, Task<string>>>
                refreshTokenExpression)
            => WithDelegatingHandler((logger, options) =>
            {
                var refreshTokenFactory = refreshTokenExpression.Compile();
                return new AuthenticationHandler(logger,
                    options,
                    (request, token, ct) => refreshTokenFactory.Invoke(tokenService, request, token, ct));
            });

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(
            TSettingsService settingsService,
            Expression<Func<TSettingsService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression,
            Expression<Func<TSettingsService, HttpRequestMessage, string, CancellationToken, Task>> setTokenExpression,
            TTokenService tokenService,
            Expression<Func<TTokenService, HttpRequestMessage, string, CancellationToken, Task<string>>>
                refreshTokenExpression)
            => WithDelegatingHandler((logger, options) =>
            {
                var getTokenFactory = getTokenExpression.Compile();
                var setTokenFactory = setTokenExpression.Compile();
                var refreshTokenFactory = refreshTokenExpression.Compile();
                return new AuthenticationHandler(logger,
                    options,
                    (request, ct) => getTokenFactory.Invoke(settingsService, request, ct),
                    (request, token, ct) => setTokenFactory.Invoke(settingsService, request, token, ct),
                    (request, token, ct) => refreshTokenFactory.Invoke(tokenService, request, token, ct));
            });

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithAuthenticationHandler<TAuthService>(TAuthService authService,
            Expression<Func<TAuthService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression,
            Expression<Func<TAuthService, HttpRequestMessage, string, CancellationToken, Task>> setTokenExpression,
            Expression<Func<TAuthService, HttpRequestMessage, string, CancellationToken, Task<string>>>
                refreshTokenExpression)
            => WithDelegatingHandler((logger, options) =>
            {
                var getTokenFactory = getTokenExpression.Compile();
                var setTokenFactory = setTokenExpression.Compile();
                var refreshTokenFactory = refreshTokenExpression.Compile();
                return new AuthenticationHandler(logger,
                    options,
                    (request, ct) => getTokenFactory.Invoke(authService, request, ct),
                    (request, token, ct) => setTokenFactory.Invoke(authService, request, token, ct),
                    (request, token, ct) => refreshTokenFactory.Invoke(authService, request, token, ct));
            });

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            TSettingsService settingsService,
            Expression<Func<TSettingsService, string>> tokenPropertyExpression)
            => WithDelegatingHandler((logger, options) =>
            {
                var getTokenFactory = tokenPropertyExpression.Compile();
                var setTokenAction = tokenPropertyExpression.ToCompiledSetter();
                return new AuthenticationHandler(logger,
                    options,
                    (_, _) => Task.FromResult(getTokenFactory.Invoke(settingsService)),
                    (_, token, _) =>
                    {
                        setTokenAction?.Invoke(settingsService, token);
                        return Task.CompletedTask;
                    });
            });

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(
            TSettingsService settingsService,
            Expression<Func<TSettingsService, string>> tokenPropertyExpression,
            TTokenService tokenService,
            Expression<Func<TTokenService, HttpRequestMessage, string, CancellationToken, Task<string>>>
                refreshTokenExpression)
            => WithDelegatingHandler((logger, options) =>
            {
                var getTokenFactory = tokenPropertyExpression.Compile();
                var setTokenAction = tokenPropertyExpression.ToCompiledSetter();
                var refreshTokenFactory = refreshTokenExpression.Compile();
                return new AuthenticationHandler(logger,
                    options,
                    (_, _) => Task.FromResult(getTokenFactory.Invoke(settingsService)),
                    (_, token, _) =>
                    {
                        setTokenAction?.Invoke(settingsService, token);
                        return Task.CompletedTask;
                    },
                    (request, token, ct) => refreshTokenFactory.Invoke(tokenService, request, token, ct));
            });

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithAuthenticationHandler<TAuthService>(
            TAuthService authService,
            Expression<Func<TAuthService, string>> tokenPropertyExpression,
            Expression<Func<TAuthService, HttpRequestMessage, string, CancellationToken, Task<string>>> refreshTokenExpression)
            => WithDelegatingHandler((logger, options) =>
            {
                var getTokenFactory = tokenPropertyExpression.Compile();
                var setTokenAction = tokenPropertyExpression.ToCompiledSetter();
                var refreshTokenFactory = refreshTokenExpression.Compile();
                return new AuthenticationHandler(logger,
                    options,
                    (_, _) => Task.FromResult(getTokenFactory.Invoke(authService)),
                    (_, token, _) =>
                    {
                        setTokenAction?.Invoke(authService, token);
                        return Task.CompletedTask;
                    },
                    (request, token, ct) => refreshTokenFactory.Invoke(authService, request, token, ct));
            });

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            Func<TSettingsService> settingsServiceFactory,
            Expression<Func<TSettingsService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression)
            => WithDelegatingHandler((logger, options) =>
            {
                var getTokenFactory = getTokenExpression.Compile();
                return new AuthenticationHandler(logger,
                    options,
                    (request, ct) => getTokenFactory.Invoke(settingsServiceFactory.Invoke(), request, ct));
            });

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            Func<TSettingsService> settingsServiceFactory,
            Expression<Func<TSettingsService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression,
            Expression<Func<TSettingsService, HttpRequestMessage, string, CancellationToken, Task>> setTokenExpression)
            => WithDelegatingHandler((logger, options) =>
            {
                var getTokenFactory = getTokenExpression.Compile();
                var setTokenFactory = setTokenExpression.Compile();
                return new AuthenticationHandler(logger,
                    options,
                    (request, ct) => getTokenFactory.Invoke(settingsServiceFactory.Invoke(), request, ct),
                    (request, token, ct) => setTokenFactory.Invoke(settingsServiceFactory.Invoke(), request, token, ct));
            });

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithAuthenticationHandler<TTokenService>(
            Func<TTokenService> tokenServiceFactory,
            Expression<Func<TTokenService, HttpRequestMessage, string, CancellationToken, Task<string>>>
                refreshTokenExpression)
            => WithDelegatingHandler((logger, options) =>
            {
                var refreshTokenFactory = refreshTokenExpression.Compile();
                return new AuthenticationHandler(logger,
                    options,
                    (request, token, ct) => refreshTokenFactory.Invoke(tokenServiceFactory.Invoke(), request, token, ct));
            });

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(
            Func<TSettingsService> settingsServiceFactory,
            Expression<Func<TSettingsService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression,
            Expression<Func<TSettingsService, HttpRequestMessage, string, CancellationToken, Task>> setTokenExpression,
            Func<TTokenService> tokenServiceFactory,
            Expression<Func<TTokenService, HttpRequestMessage, string, CancellationToken, Task<string>>>
                refreshTokenExpression)
            => WithDelegatingHandler((logger, options) =>
            {
                var getTokenFactory = getTokenExpression.Compile();
                var setTokenFactory = setTokenExpression.Compile();
                var refreshTokenFactory = refreshTokenExpression.Compile();
                return new AuthenticationHandler(logger,
                    options,
                    (request, ct) => getTokenFactory.Invoke(settingsServiceFactory.Invoke(), request, ct),
                    (request, token, ct) => setTokenFactory.Invoke(settingsServiceFactory.Invoke(), request, token, ct),
                    (request, token, ct) => refreshTokenFactory.Invoke(tokenServiceFactory.Invoke(), request, token, ct));
            });

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithAuthenticationHandler<TAuthService>(Func<TAuthService> authServiceFactory,
            Expression<Func<TAuthService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression,
            Expression<Func<TAuthService, HttpRequestMessage, string, CancellationToken, Task>> setTokenExpression,
            Expression<Func<TAuthService, HttpRequestMessage, string, CancellationToken, Task<string>>>
                refreshTokenExpression)
            => WithDelegatingHandler((logger, options) =>
            {
                var getTokenFactory = getTokenExpression.Compile();
                var setTokenFactory = setTokenExpression.Compile();
                var refreshTokenFactory = refreshTokenExpression.Compile();
                return new AuthenticationHandler(logger,
                    options,
                    (request, ct) => getTokenFactory.Invoke(authServiceFactory.Invoke(), request, ct),
                    (request, token, ct) => setTokenFactory.Invoke(authServiceFactory.Invoke(), request, token, ct),
                    (request, token, ct) => refreshTokenFactory.Invoke(authServiceFactory.Invoke(), request, token, ct));
            });

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            Func<TSettingsService> settingsServiceFactory,
            Expression<Func<TSettingsService, string>> tokenPropertyExpression)
            => WithDelegatingHandler((logger, options) =>
            {
                var getTokenFactory = tokenPropertyExpression.Compile();
                var setTokenAction = tokenPropertyExpression.ToCompiledSetter();
                return new AuthenticationHandler(logger,
                    options,
                    (_, _) => Task.FromResult(getTokenFactory.Invoke(settingsServiceFactory.Invoke())),
                    (_, token, _) =>
                    {
                        setTokenAction?.Invoke(settingsServiceFactory.Invoke(), token);
                        return Task.CompletedTask;
                    });
            });

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(
            Func<TSettingsService> settingsServiceFactory,
            Expression<Func<TSettingsService, string>> tokenPropertyExpression,
            Func<TTokenService> tokenServiceFactory,
            Expression<Func<TTokenService, HttpRequestMessage, string, CancellationToken, Task<string>>>
                refreshTokenExpression)
            => WithDelegatingHandler((logger, options) =>
            {
                var getTokenFactory = tokenPropertyExpression.Compile();
                var setTokenAction = tokenPropertyExpression.ToCompiledSetter();
                var refreshTokenFactory = refreshTokenExpression.Compile();
                return new AuthenticationHandler(logger,
                    options,
                    (_, _) => Task.FromResult(getTokenFactory.Invoke(settingsServiceFactory.Invoke())),
                    (_, token, _) =>
                    {
                        setTokenAction?.Invoke(settingsServiceFactory.Invoke(), token);
                        return Task.CompletedTask;
                    },
                    (request, token, ct) => refreshTokenFactory.Invoke(tokenServiceFactory.Invoke(), request, token, ct));
            });

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithAuthenticationHandler<TAuthService>(Func<TAuthService> authServiceFactory,
            Expression<Func<TAuthService, string>> tokenPropertyExpression,
            Expression<Func<TAuthService, HttpRequestMessage, string, CancellationToken, Task<string>>>
                refreshTokenExpression)
            => WithDelegatingHandler((logger, options) =>
            {
                var getTokenFactory = tokenPropertyExpression.Compile();
                var setTokenAction = tokenPropertyExpression.ToCompiledSetter();
                var refreshTokenFactory = refreshTokenExpression.Compile();
                return new AuthenticationHandler(logger,
                    options,
                    (_, _) => Task.FromResult(getTokenFactory.Invoke(authServiceFactory.Invoke())),
                    (_, token, _) =>
                    {
                        setTokenAction?.Invoke(authServiceFactory.Invoke(), token);
                        return Task.CompletedTask;
                    },
                    (request, token, ct) => refreshTokenFactory.Invoke(authServiceFactory.Invoke(), request, token, ct));
            });

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithDelegatingHandler<THandler>(THandler delegatingHandler,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add) where THandler : DelegatingHandler
            => WithDelegatingHandler((_, _) => delegatingHandler, strategy);

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithDelegatingHandler<THandler>(
            Func<ILogger, THandler> delegatingHandlerFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add) where THandler : DelegatingHandler
            => WithDelegatingHandler((logger, _) => delegatingHandlerFactory(logger), strategy);

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithDelegatingHandler<THandler>(
            Func<ILogger, IApizrManagerOptionsBase, THandler> delegatingHandlerFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add) where THandler : DelegatingHandler
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.DelegatingHandlersFactories[typeof(THandler)] ??= delegatingHandlerFactory;
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    Options.DelegatingHandlersFactories[typeof(THandler)] = delegatingHandlerFactory;
                    break;
                case ApizrDuplicateStrategy.Replace:
                    {
                        Options.DelegatingHandlersFactories.Clear();
                        Options.DelegatingHandlersFactories[typeof(THandler)] = delegatingHandlerFactory;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithHttpMessageHandler<THandler>(THandler httpMessageHandler) where THandler : HttpMessageHandler
            => WithHttpMessageHandler((_, _) => httpMessageHandler);

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithHttpMessageHandler<THandler>(Func<ILogger, THandler> httpMessageHandlerFactory) where THandler : HttpMessageHandler
            => WithHttpMessageHandler((logger, _) => httpMessageHandlerFactory(logger));

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithHttpMessageHandler<THandler>(Func<ILogger, IApizrManagerOptionsBase, THandler> httpMessageHandlerFactory) where THandler : HttpMessageHandler
        {
            if (httpMessageHandlerFactory is DelegatingHandler delegatingHandler)
                return WithDelegatingHandler(delegatingHandler);

            Options.HttpMessageHandlerFactory = httpMessageHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        [Obsolete("Catching an exception by an Action is now replaced by a Func returning a handled boolean flag")]
        public IApizrProperOptionsBuilder WithExCatching(Action<ApizrException> onException,
            bool letThrowOnException = true, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(() => new ApizrExceptionHandler(onException),
                letThrowOnException,
                strategy);

        /// <inheritdoc />
        [Obsolete("Catching an exception by an Action is now replaced by a Func returning a handled boolean flag")]
        public IApizrProperOptionsBuilder WithExCatching<TResult>(Action<ApizrException<TResult>> onException,
            bool letThrowOnException = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(
                () => new ApizrExceptionHandler<TResult>(onException),
                letThrowOnException,
                strategy);

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithExCatching(Func<ApizrException, bool> onException, bool letThrowOnHandledException = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(
                () => new ApizrExceptionHandler(onException),
                letThrowOnHandledException,
                strategy);

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithExCatching<TResult>(Func<ApizrException<TResult>, bool> onException,
            bool letThrowOnHandledException = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(
                () => new ApizrExceptionHandler<TResult>(onException),
                letThrowOnHandledException,
                strategy);

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithExCatching(Func<ApizrException, Task<bool>> onException, bool letThrowOnHandledException = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(new ApizrExceptionHandler(onException), letThrowOnHandledException,
                strategy);

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithExCatching<THandler>(THandler exceptionHandler, bool letThrowOnHandledException = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace) where THandler : IApizrExceptionHandler
            => WithExCatching(
                () => exceptionHandler,
                letThrowOnHandledException,
                strategy);

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithExCatching<TResult>(Func<ApizrException<TResult>, Task<bool>> onException, bool letThrowOnHandledException = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(
                () => new ApizrExceptionHandler<TResult>(onException),
                letThrowOnHandledException,
                strategy);

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithExCatching<THandler>(Func<THandler> exceptionHandlerFactory,
            bool letThrowOnHandledException = true, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace) where THandler : IApizrExceptionHandler
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.ExceptionHandlersFactory ??= () => [exceptionHandlerFactory()];
                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.ExceptionHandlersFactory = () => [exceptionHandlerFactory()];
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.ExceptionHandlersFactory == null)
                        Options.ExceptionHandlersFactory = () => [exceptionHandlerFactory()];
                    else
                    {
                        var previous = Options.ExceptionHandlersFactory;
                        Options.ExceptionHandlersFactory = () =>
                        {
                            var exceptionHandlers = previous?.Invoke() ?? [];
                            exceptionHandlers.Add(exceptionHandlerFactory());
                            return exceptionHandlers;
                        };
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            Options.LetThrowOnHandledException = letThrowOnHandledException;

            return this;
        }

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithHandlerParameter(string key, object value)
        {
            Options.HandlersParameters[key] = value;

            return this;
        }

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithLogging(HttpTracerMode httpTracerMode = HttpTracerMode.Everything,
            HttpMessageParts trafficVerbosity = HttpMessageParts.All, params LogLevel[] logLevels)
            => WithLogging(() => httpTracerMode, () => trafficVerbosity, () => logLevels);

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithLogging(Func<HttpTracerMode> httpTracerModeFactory,
            Func<HttpMessageParts> trafficVerbosityFactory, Func<LogLevel[]> logLevelsFactory)
        {
            Options.HttpTracerModeFactory = httpTracerModeFactory;
            Options.TrafficVerbosityFactory = trafficVerbosityFactory;
            Options.LogLevelsFactory = logLevelsFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithLogging(
            Func<(HttpTracerMode, HttpMessageParts, LogLevel[])> loggingConfigurationFactory)
            => WithLogging(() => loggingConfigurationFactory.Invoke().Item1,
                () => loggingConfigurationFactory.Invoke().Item2,
                () => loggingConfigurationFactory.Invoke().Item3);

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithHeaders(IList<string> headers,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add,
            ApizrRegistrationMode mode = ApizrRegistrationMode.Set)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.Headers[mode] ??= headers;
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.Headers.TryGetValue(mode, out var value))
                    {
                        headers?.ToList().ForEach(header => value.Add(header));
                    }
                    else
                    {
                        Options.Headers[mode] = headers;
                    }
                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.Headers[mode] = headers;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithHeaders(Func<IList<string>> headersFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add,
            ApizrLifetimeScope scope = ApizrLifetimeScope.Api,
            ApizrRegistrationMode mode = ApizrRegistrationMode.Set)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.HeadersFactories[(mode, scope)] ??= headersFactory;
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.HeadersFactories.TryGetValue((mode, scope), out var previous))
                    {
                        Options.HeadersFactories[(mode, scope)] = () => previous().Concat(headersFactory()).ToList();
                    }
                    else
                    {
                        Options.HeadersFactories[(mode, scope)] = headersFactory;
                    }
                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.HeadersFactories[(mode, scope)] = headersFactory;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithHeaders<TSettingsService>(TSettingsService settingsService,
            Expression<Func<TSettingsService, string>>[] headerProperties,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add,
            ApizrLifetimeScope scope = ApizrLifetimeScope.Api,
            ApizrRegistrationMode mode = ApizrRegistrationMode.Set)
            => WithHeaders(() => settingsService, headerProperties, strategy, scope, mode);

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithHeaders<TSettingsService>(Func<TSettingsService> settingsServiceFactory,
            Expression<Func<TSettingsService, string>>[] headerProperties,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add,
            ApizrLifetimeScope scope = ApizrLifetimeScope.Api, 
            ApizrRegistrationMode mode = ApizrRegistrationMode.Set)
        {
            var settingsService = settingsServiceFactory.Invoke();
            var headersFactories = headerProperties.Select(exp => exp.Compile());

            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.HeadersFactories[(mode, scope)] ??= () => headersFactories
                        .Select(headerFactory => headerFactory.Invoke(settingsService))
                        .ToList();
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.HeadersFactories.TryGetValue((mode, scope), out var previous))
                    {
                        Options.HeadersFactories[(mode, scope)] = () => previous()
                            .Concat(headersFactories.Select(headerFactory => headerFactory.Invoke(settingsService)))
                            .ToList();
                    }
                    else
                    {
                        Options.HeadersFactories[(mode, scope)] = () => headersFactories
                            .Select(headerFactory => headerFactory.Invoke(settingsService))
                            .ToList();
                    }
                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.HeadersFactories[(mode, scope)] = () => headersFactories
                        .Select(headerFactory => headerFactory.Invoke(settingsService))
                        .ToList();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithOperationTimeout(TimeSpan timeout)
            => WithOperationTimeout(() => timeout);

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithOperationTimeout(Func<TimeSpan> timeoutFactory)
        {
            Options.OperationTimeoutFactory = timeoutFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithRequestTimeout(TimeSpan timeout)
            => WithRequestTimeout(() => timeout);

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithRequestTimeout(Func<TimeSpan> timeoutFactory)
        {
            Options.RequestTimeoutFactory = timeoutFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithResilienceProperty<TValue>(ResiliencePropertyKey<TValue> key, TValue value)
            => WithResilienceProperty(key, () => value);

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithResilienceProperty<TValue>(ResiliencePropertyKey<TValue> key, Func<TValue> valueFactory)
        {
            ((IApizrGlobalSharedOptionsBase)Options).ResiliencePropertiesFactories[key.Key] = () => valueFactory();

            return this;
        }

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithResilienceContextOptions(Action<IApizrResilienceContextOptionsBuilder> contextOptionsBuilder)
        {
            var options = Options as IApizrGlobalSharedOptionsBase;
            if (options.ContextOptionsBuilder == null)
            {
                options.ContextOptionsBuilder = contextOptionsBuilder;
            }
            else
            {
                options.ContextOptionsBuilder += contextOptionsBuilder.Invoke;
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithLoggedHeadersRedactionNames(IEnumerable<string> redactedLoggedHeaderNames,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add)
        {
            var sensitiveHeaders = new HashSet<string>(redactedLoggedHeaderNames, StringComparer.OrdinalIgnoreCase);

            return WithLoggedHeadersRedactionRule(header => sensitiveHeaders.Contains(header), strategy);
        }

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithLoggedHeadersRedactionRule(Func<string, bool> shouldRedactHeaderValue,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.ShouldRedactHeaderValue ??= shouldRedactHeaderValue;
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.ShouldRedactHeaderValue == null)
                    {
                        Options.ShouldRedactHeaderValue = shouldRedactHeaderValue;
                    }
                    else
                    {
                        var previous = Options.ShouldRedactHeaderValue;
                        Options.ShouldRedactHeaderValue = header => previous(header) || shouldRedactHeaderValue(header);
                    }

                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.ShouldRedactHeaderValue = shouldRedactHeaderValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithResiliencePipelineKeys(string[] resiliencePipelineKeys,
            IEnumerable<ApizrRequestMethod> methodScope = null,
            ApizrDuplicateStrategy duplicateStrategy = ApizrDuplicateStrategy.Add)
        {
            methodScope ??= [ApizrRequestMethod.All];

            switch (duplicateStrategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    if (Options.ResiliencePipelineOptions.Count == 0)
                        Options.ResiliencePipelineOptions[ApizrConfigurationSource.ProperOption] = methodScope
                            .Select(method => new ResiliencePipelineAttribute(resiliencePipelineKeys) { RequestMethod = method })
                            .Cast<ResiliencePipelineAttributeBase>()
                            .ToArray();
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.ResiliencePipelineOptions.TryGetValue(ApizrConfigurationSource.ProperOption, out var attributes))
                    {
                        foreach (var method in methodScope)
                        {
                            var attribute = attributes.FirstOrDefault(attribute => attribute.RequestMethod == method);
                            if (attribute != null)
                                attribute.RegistryKeys = attribute.RegistryKeys.Union(resiliencePipelineKeys).ToArray();
                            else
                                attributes = attributes.Concat([new ResiliencePipelineAttribute(resiliencePipelineKeys) { RequestMethod = method }]).ToArray();
                        }

                        Options.ResiliencePipelineOptions[ApizrConfigurationSource.ProperOption] = attributes;
                    }
                    else
                    {
                        Options.ResiliencePipelineOptions[ApizrConfigurationSource.ProperOption] = methodScope
                            .Select(method => new ResiliencePipelineAttribute(resiliencePipelineKeys) { RequestMethod = method })
                            .Cast<ResiliencePipelineAttributeBase>()
                            .ToArray();
                    }

                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.ResiliencePipelineOptions.Clear();
                    Options.ResiliencePipelineOptions[ApizrConfigurationSource.ProperOption] = methodScope
                        .Select(method => new ResiliencePipelineAttribute(resiliencePipelineKeys) { RequestMethod = method })
                        .Cast<ResiliencePipelineAttributeBase>()
                        .ToArray();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(duplicateStrategy), duplicateStrategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithCaching(CacheMode mode = CacheMode.FetchOrGet, TimeSpan? lifeSpan = null,
            bool shouldInvalidateOnError = false)
        {
            Options.CacheOptions[ApizrConfigurationSource.ProperOption] = new CacheAttribute(mode, lifeSpan, shouldInvalidateOnError);

            return this;
        }

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithRequestOptions(string requestName,
            Action<IApizrRequestOptionsBuilder> optionsBuilder,
            ApizrDuplicateStrategy duplicateStrategy = ApizrDuplicateStrategy.Add)
            => WithRequestOptions([requestName], optionsBuilder, duplicateStrategy);

        /// <inheritdoc />
        public IApizrProperOptionsBuilder WithRequestOptions(string[] requestNames, 
            Action<IApizrRequestOptionsBuilder> optionsBuilder,
            ApizrDuplicateStrategy duplicateStrategy = ApizrDuplicateStrategy.Add)
        {
            foreach (var requestName in requestNames)
            {
                if (Options.RequestOptionsBuilders.ContainsKey(requestName))
                {
                    switch (duplicateStrategy)
                    {
                        case ApizrDuplicateStrategy.Ignore:
                            // Skip request configuration
                            break;
                        case ApizrDuplicateStrategy.Replace:
                            Options.RequestOptionsBuilders[requestName] = optionsBuilder;
                            break;
                        case ApizrDuplicateStrategy.Add:
                        case ApizrDuplicateStrategy.Merge:
                            Options.RequestOptionsBuilders[requestName] += optionsBuilder;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(duplicateStrategy), duplicateStrategy, null);
                    }
                }
                else
                {
                    Options.RequestOptionsBuilders[requestName] = optionsBuilder;
                }
            }

            return this;
        }

        #region Internal

        void IApizrInternalOptionsBuilder.SetHandlerParameter(string key, object value) => WithHandlerParameter(key, value);

        void IApizrInternalRegistrationOptionsBuilder.SetPrimaryHttpMessageHandler(Func<DelegatingHandler, ILogger, IApizrManagerOptionsBase, HttpMessageHandler> primaryHandlerFactory)
        {
            Options.PrimaryHandlerFactory = primaryHandlerFactory;
        }

        /// <inheritdoc />
        void IApizrInternalRegistrationOptionsBuilder.AddDelegatingHandler<THandler>(Func<IApizrManagerOptionsBase, THandler> handlerFactory) 
            => WithDelegatingHandler((_, opt) => handlerFactory.Invoke(opt));

        #endregion
    }
}
