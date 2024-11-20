using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Authenticating;
using Apizr.Caching;
using Apizr.Caching.Attributes;
using Apizr.Configuring;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Request;
using Apizr.Configuring.Shared;
using Apizr.Configuring.Shared.Context;
using Apizr.Extending.Configuring.Shared;
using Apizr.Logging;
using Apizr.Resiliencing.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Extending.Configuring.Proper
{
    /// <summary>
    /// Builder options available at proper level for extended registrations
    /// </summary>
    public class ApizrExtendedProperOptionsBuilder : IApizrExtendedProperOptionsBuilder, IApizrInternalRegistrationOptionsBuilder
    {
        protected readonly ApizrExtendedProperOptions Options;

        internal ApizrExtendedProperOptionsBuilder(ApizrExtendedProperOptions properOptions)
        {
            Options = properOptions;
        }
        
        /// <inheritdoc />
        IApizrExtendedProperOptions IApizrExtendedProperOptionsBuilder.ApizrOptions => Options;

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithConfiguration(IConfiguration configuration)
            => WithConfiguration(configuration?.GetSection("Apizr"), null);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithConfiguration(IConfigurationSection configurationSection)
            => WithConfiguration(configurationSection, null);

        private IApizrExtendedProperOptionsBuilder WithConfiguration(IConfigurationSection configurationSection, string requestName)
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
        public IApizrExtendedProperOptionsBuilder WithBaseAddress(string baseAddress,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithBaseAddress(_ => baseAddress, strategy);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithBaseAddress(Func<IServiceProvider, string> baseAddressFactory,
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
        public IApizrExtendedProperOptionsBuilder WithBaseAddress(Uri baseAddress,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithBaseAddress(_ => baseAddress, strategy);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithBaseAddress(Func<IServiceProvider, Uri> baseAddressFactory,
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
        public IApizrExtendedProperOptionsBuilder WithBasePath(string basePath, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithBasePath(_ => basePath, strategy);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithBasePath(Func<IServiceProvider, string> basePathFactory,
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
        public IApizrExtendedProperOptionsBuilder WithHttpClientHandler(HttpClientHandler httpClientHandler)
            => WithHttpClientHandler(_ => httpClientHandler);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithHttpClientHandler(Func<IServiceProvider, HttpClientHandler> httpClientHandlerFactory)
        {
            Options.HttpClientHandlerFactory = httpClientHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder ConfigureHttpClientBuilder(
            Action<IHttpClientBuilder> httpClientBuilder, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Merge)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.HttpClientBuilder ??= httpClientBuilder;
                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.HttpClientBuilder = httpClientBuilder;
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.HttpClientBuilder == null)
                    {
                        Options.HttpClientBuilder = httpClientBuilder;
                    }
                    else
                    {
                        Options.HttpClientBuilder += httpClientBuilder.Invoke;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithDelegatingHandler<THandler>(
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add) where THandler : DelegatingHandler
            => WithDelegatingHandler((serviceProvider, _) => serviceProvider.GetRequiredService<THandler>());

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithDelegatingHandler<THandler>(Func<IServiceProvider, THandler> delegatingHandlerFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add) where THandler : DelegatingHandler
            => WithDelegatingHandler((serviceProvider, _) => delegatingHandlerFactory.Invoke(serviceProvider), strategy);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler<TAuthenticationHandler>(
            Func<IServiceProvider, IApizrManagerOptionsBase, TAuthenticationHandler> authenticationHandlerFactory)
            where TAuthenticationHandler : AuthenticationHandlerBase
            => WithDelegatingHandler(authenticationHandlerFactory);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler(Type authenticationHandlerType)
        {
            if (!authenticationHandlerType.IsOpenGeneric())
                throw new ArgumentException(
                    $"{authenticationHandlerType.Name} must be open generic");

            if (authenticationHandlerType.GetGenericArguments().Length != 1)
                throw new ArgumentException(
                    $"{authenticationHandlerType.Name} must define only one generic TWebApi argument");

            if (!typeof(AuthenticationHandlerBase).IsAssignableFrom(authenticationHandlerType))
                throw new ArgumentException(
                    $"{authenticationHandlerType.Name} must inherit from AuthenticationHandlerBase");

            return WithDelegatingHandler((serviceProvider, options) => (AuthenticationHandlerBase)serviceProvider.GetRequiredService(authenticationHandlerType.MakeGenericType(options.WebApiType)));
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler(
            Func<HttpRequestMessage, CancellationToken, Task<string>> getTokenFactory)
            => WithDelegatingHandler((_, options) =>
                new AuthenticationHandler(options, getTokenFactory));

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler(
            Func<HttpRequestMessage, CancellationToken, Task<string>> getTokenFactory,
            Func<HttpRequestMessage, string, CancellationToken, Task> setTokenFactory)
            => WithDelegatingHandler((_, options) =>
                new AuthenticationHandler(
                    options, getTokenFactory, setTokenFactory));

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler(
            Func<HttpRequestMessage, string, CancellationToken, Task<string>> refreshTokenFactory)
            => WithDelegatingHandler((_, options) =>
                new AuthenticationHandler(
                    options, refreshTokenFactory));

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler(
            Func<HttpRequestMessage, CancellationToken, Task<string>> getTokenFactory,
            Func<HttpRequestMessage, string, CancellationToken, Task> setTokenFactory,
            Func<HttpRequestMessage, string, CancellationToken, Task<string>> refreshTokenFactory)
            => WithDelegatingHandler((_, options) =>
                new AuthenticationHandler(
                    options, getTokenFactory, setTokenFactory, refreshTokenFactory));

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            Expression<Func<TSettingsService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression)
            => WithDelegatingHandler((serviceProvider, options) =>
            {
                var getTokenFactory = getTokenExpression.Compile();
                return new AuthenticationHandler(
                    
                    options,
                    (request, ct) => getTokenFactory.Invoke(serviceProvider.GetRequiredService<TSettingsService>(), request, ct));
            });

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            Expression<Func<TSettingsService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression,
            Expression<Func<TSettingsService, HttpRequestMessage, string, CancellationToken, Task>> setTokenExpression)
            => WithDelegatingHandler((serviceProvider, options) =>
            {
                var getTokenFactory = getTokenExpression.Compile();
                var setTokenFactory = setTokenExpression.Compile();
                return new AuthenticationHandler(
                    
                    options,
                    (request, ct) => getTokenFactory.Invoke(serviceProvider.GetRequiredService<TSettingsService>(), request, ct),
                    (request, token, ct) => setTokenFactory.Invoke(serviceProvider.GetRequiredService<TSettingsService>(), request, token, ct));
            });

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler<TTokenService>(
            Expression<Func<TTokenService, HttpRequestMessage, string, CancellationToken, Task<string>>>
                refreshTokenExpression)
            => WithDelegatingHandler((serviceProvider, options) =>
            {
                var refreshTokenFactory = refreshTokenExpression.Compile();
                return new AuthenticationHandler(
                    
                    options,
                    (request, token, ct) => refreshTokenFactory.Invoke(serviceProvider.GetRequiredService<TTokenService>(), request, token, ct));
            });

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(
            Expression<Func<TSettingsService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression,
            Expression<Func<TSettingsService, HttpRequestMessage, string, CancellationToken, Task>> setTokenExpression,
            Expression<Func<TTokenService, HttpRequestMessage, string, CancellationToken, Task<string>>>
                refreshTokenExpression)
            => WithDelegatingHandler((serviceProvider, options) =>
            {
                var getTokenFactory = getTokenExpression.Compile();
                var setTokenFactory = setTokenExpression.Compile();
                var refreshTokenFactory = refreshTokenExpression.Compile();
                return new AuthenticationHandler(
                    
                    options,
                    (request, ct) => getTokenFactory.Invoke(serviceProvider.GetRequiredService<TSettingsService>(), request, ct),
                    (request, token, ct) => setTokenFactory.Invoke(serviceProvider.GetRequiredService<TSettingsService>(), request, token, ct),
                    (request, token, ct) => refreshTokenFactory.Invoke(serviceProvider.GetRequiredService<TTokenService>(), request, token, ct));
            });

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler<TAuthService>(
            Expression<Func<TAuthService, HttpRequestMessage, CancellationToken, Task<string>>> getTokenExpression,
            Expression<Func<TAuthService, HttpRequestMessage, string, CancellationToken, Task>> setTokenExpression,
            Expression<Func<TAuthService, HttpRequestMessage, string, CancellationToken, Task<string>>>
                refreshTokenExpression)
            => WithDelegatingHandler((serviceProvider, options) =>
            {
                var getTokenFactory = getTokenExpression.Compile();
                var setTokenFactory = setTokenExpression.Compile();
                var refreshTokenFactory = refreshTokenExpression.Compile();
                return new AuthenticationHandler(
                    
                    options,
                    (request, ct) => getTokenFactory.Invoke(serviceProvider.GetRequiredService<TAuthService>(), request, ct),
                    (request, token, ct) => setTokenFactory.Invoke(serviceProvider.GetRequiredService<TAuthService>(), request, token, ct),
                    (request, token, ct) => refreshTokenFactory.Invoke(serviceProvider.GetRequiredService<TAuthService>(), request, token, ct));
            });

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler<TSettingsService>(
            Expression<Func<TSettingsService, string>> tokenPropertyExpression)
            => WithDelegatingHandler((serviceProvider, options) =>
            {
                var getTokenFactory = tokenPropertyExpression.Compile();
                var setTokenAction = tokenPropertyExpression.ToCompiledSetter();
                return new AuthenticationHandler(
                    
                    options,
                    (_, _) => Task.FromResult(getTokenFactory.Invoke(serviceProvider.GetRequiredService<TSettingsService>())),
                    (_, token, _) =>
                    {
                        setTokenAction?.Invoke(serviceProvider.GetRequiredService<TSettingsService>(), token);
                        return Task.CompletedTask;
                    });
            });

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler<TSettingsService, TTokenService>(
            Expression<Func<TSettingsService, string>> tokenPropertyExpression,
            Expression<Func<TTokenService, HttpRequestMessage, string, CancellationToken, Task<string>>>
                refreshTokenExpression)
            => WithDelegatingHandler((serviceProvider, options) =>
            {
                var getTokenFactory = tokenPropertyExpression.Compile();
                var setTokenAction = tokenPropertyExpression.ToCompiledSetter();
                var refreshTokenFactory = refreshTokenExpression.Compile();
                return new AuthenticationHandler(
                    
                    options,
                    (_, _) => Task.FromResult(getTokenFactory.Invoke(serviceProvider.GetRequiredService<TSettingsService>())),
                    (_, token, _) =>
                    {
                        setTokenAction?.Invoke(serviceProvider.GetRequiredService<TSettingsService>(), token);
                        return Task.CompletedTask;
                    },
                    (request, token, ct) => refreshTokenFactory.Invoke(serviceProvider.GetRequiredService<TTokenService>(), request, token, ct));
            });

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithAuthenticationHandler<TAuthService>(
            Expression<Func<TAuthService, string>> tokenPropertyExpression,
            Expression<Func<TAuthService, HttpRequestMessage, string, CancellationToken, Task<string>>>
                refreshTokenExpression)
            => WithDelegatingHandler((serviceProvider, options) =>
            {
                var getTokenFactory = tokenPropertyExpression.Compile();
                var setTokenAction = tokenPropertyExpression.ToCompiledSetter();
                var refreshTokenFactory = refreshTokenExpression.Compile();
                return new AuthenticationHandler(
                    
                    options,
                    (_, _) => Task.FromResult(getTokenFactory.Invoke(serviceProvider.GetRequiredService<TAuthService>())),
                    (_, token, _) =>
                    {
                        setTokenAction?.Invoke(serviceProvider.GetRequiredService<TAuthService>(), token);
                        return Task.CompletedTask;
                    },
                    (request, token, ct) => refreshTokenFactory.Invoke(serviceProvider.GetRequiredService<TAuthService>(), request, token, ct));
            });

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithHeaders(IList<string> headers,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add,
            ApizrRegistrationMode behavior = ApizrRegistrationMode.Set)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.Headers[behavior] ??= headers;
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.Headers.TryGetValue(behavior, out var value))
                    {
                        headers?.ToList().ForEach(header => value.Add(header));
                    }
                    else
                    {
                        Options.Headers[behavior] = headers;
                    }
                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.Headers[behavior] = headers;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithHeaders(Func<IServiceProvider, IList<string>> headersFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add,
            ApizrLifetimeScope scope = ApizrLifetimeScope.Api, 
            ApizrRegistrationMode mode = ApizrRegistrationMode.Set)
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.HeadersExtendedFactories[(mode, scope)] ??= serviceProvider => () => headersFactory(serviceProvider);
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.HeadersExtendedFactories.TryGetValue((mode, scope), out var previous))
                    {
                        Options.HeadersExtendedFactories[(mode, scope)] = serviceProvider => () => previous(serviceProvider).Invoke().Concat(headersFactory(serviceProvider)).ToList();
                    }
                    else
                    {
                        Options.HeadersExtendedFactories[(mode, scope)] = serviceProvider => () => headersFactory(serviceProvider);
                    }
                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.HeadersExtendedFactories[(mode, scope)] = serviceProvider => () => headersFactory(serviceProvider);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithHeaders<TSettingsService>(
            Expression<Func<TSettingsService, string>>[] headerProperties,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add,
            ApizrLifetimeScope scope = ApizrLifetimeScope.Api, ApizrRegistrationMode mode = ApizrRegistrationMode.Set)
        {
            var headersFactories = headerProperties.Select(exp => exp.Compile());

            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.HeadersExtendedFactories[(mode, scope)] ??= serviceProvider => () =>
                    {
                        var settingsService = serviceProvider.GetRequiredService<TSettingsService>();
                        return headersFactories.Select(headerFactory => headerFactory.Invoke(settingsService)).ToList();
                    };
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.HeadersExtendedFactories.TryGetValue((mode, scope), out var previous))
                    {
                        Options.HeadersExtendedFactories[(mode, scope)] = serviceProvider => () =>
                        {
                            var settingsService = serviceProvider.GetRequiredService<TSettingsService>();
                            return previous(serviceProvider).Invoke()
                                .Concat(headersFactories.Select(headerFactory => headerFactory.Invoke(settingsService)))
                                .ToList();
                        };
                    }
                    else
                    {
                        Options.HeadersExtendedFactories[(mode, scope)] = serviceProvider => () =>
                        {
                            var settingsService = serviceProvider.GetRequiredService<TSettingsService>();
                            return headersFactories.Select(headerFactory => headerFactory.Invoke(settingsService)).ToList();
                        };
                    }
                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.HeadersExtendedFactories[(mode, scope)] = serviceProvider => () =>
                    {
                        var settingsService = serviceProvider.GetRequiredService<TSettingsService>();
                        return headersFactories.Select(headerFactory => headerFactory.Invoke(settingsService)).ToList();
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithOperationTimeout(TimeSpan timeout)
            => WithOperationTimeout(_ => timeout);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithOperationTimeout(Func<IServiceProvider, TimeSpan> timeoutFactory)
        {
            Options.OperationTimeoutFactory = timeoutFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithRequestTimeout(TimeSpan timeout)
            => WithRequestTimeout(_ => timeout);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithRequestTimeout(Func<IServiceProvider, TimeSpan> timeoutFactory)
        {
            Options.RequestTimeoutFactory = timeoutFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithDelegatingHandler<THandler>(THandler delegatingHandler,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add) where THandler : DelegatingHandler
            => WithDelegatingHandler((_, _) => delegatingHandler, strategy);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithDelegatingHandler<THandler>(
            Func<IServiceProvider, IApizrManagerOptionsBase, THandler> delegatingHandlerFactory,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add) where THandler : DelegatingHandler
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.DelegatingHandlersExtendedFactories[typeof(THandler)] ??= delegatingHandlerFactory;
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    Options.DelegatingHandlersExtendedFactories[typeof(THandler)] = delegatingHandlerFactory;
                    break;
                case ApizrDuplicateStrategy.Replace:
                {
                    Options.DelegatingHandlersExtendedFactories.Clear();
                    Options.DelegatingHandlersExtendedFactories[typeof(THandler)] = delegatingHandlerFactory;
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithHttpMessageHandler<THandler>() where THandler : HttpMessageHandler
            => WithHttpMessageHandler((serviceProvider, _) => serviceProvider.GetRequiredService<THandler>());

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithHttpMessageHandler<THandler>(Func<IServiceProvider, THandler> httpMessageHandlerFactory) where THandler : HttpMessageHandler
            => WithHttpMessageHandler((serviceProvider, _) => httpMessageHandlerFactory.Invoke(serviceProvider));

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithHttpMessageHandler<THandler>(THandler httpMessageHandler) where THandler : HttpMessageHandler
            => WithHttpMessageHandler((_, _) => httpMessageHandler);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithHttpMessageHandler<THandler>(
            Func<IServiceProvider, IApizrManagerOptionsBase, THandler> httpMessageHandlerFactory) where THandler : HttpMessageHandler
        {
            if (typeof(DelegatingHandler).IsAssignableFrom(typeof(THandler)))
                return WithDelegatingHandler((serviceProvider, options) => httpMessageHandlerFactory.Invoke(serviceProvider, options) as DelegatingHandler);

            Options.HttpMessageHandlerFactory = httpMessageHandlerFactory;

            return this;
        }

        /// <inheritdoc />
        [Obsolete("Catching an exception by an Action is now replaced by a Func returning a handled boolean flag")]
        public IApizrExtendedProperOptionsBuilder WithExCatching(Action<ApizrException> onException,
            bool letThrowOnException = true, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(_ => new ApizrExceptionHandler(onException),
                letThrowOnException,
                strategy);

        /// <inheritdoc />
        [Obsolete("Catching an exception by an Action is now replaced by a Func returning a handled boolean flag")]
        public IApizrExtendedProperOptionsBuilder WithExCatching<TResult>(Action<ApizrException<TResult>> onException,
            bool letThrowOnException = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(
                _ => new ApizrExceptionHandler<TResult>(onException),
                letThrowOnException,
                strategy);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithExCatching(Func<ApizrException, bool> onException,
            bool letThrowOnHandledException = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(
                _ => new ApizrExceptionHandler(onException),
                letThrowOnHandledException,
                strategy);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithExCatching<TResult>(
            Func<ApizrException<TResult>, bool> onException,
            bool letThrowOnHandledException = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(
                _ => new ApizrExceptionHandler<TResult>(onException),
                letThrowOnHandledException,
                strategy);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithExCatching(Func<ApizrException, Task<bool>> onException,
            bool letThrowOnHandledException = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(_ => new ApizrExceptionHandler(onException), letThrowOnHandledException,
                strategy);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithExCatching<TResult>(
            Func<ApizrException<TResult>, Task<bool>> onException, bool letThrowOnHandledException = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(
                _ => new ApizrExceptionHandler<TResult>(onException),
                letThrowOnHandledException,
                strategy);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithExCatching(Func<IServiceProvider, ApizrException, bool> onException, bool letThrowOnHandledException = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(serviceProvider => new ApizrExceptionHandler(ex => onException(serviceProvider, ex)),
                letThrowOnHandledException,
                strategy);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithExCatching<TResult>(Func<IServiceProvider, ApizrException<TResult>, bool> onException, bool letThrowOnHandledException = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(serviceProvider =>
                new ApizrExceptionHandler<TResult>(ex => onException(serviceProvider, ex)),
                letThrowOnHandledException,
                strategy);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithExCatching(
            Func<IServiceProvider, ApizrException, Task<bool>> onException, bool letThrowOnHandledException = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(serviceProvider => new ApizrExceptionHandler(ex => onException(serviceProvider, ex)),
                letThrowOnHandledException,
                strategy);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithExCatching<TResult>(
            Func<IServiceProvider, ApizrException<TResult>, Task<bool>> onException,
            bool letThrowOnHandledException = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace)
            => WithExCatching(serviceProvider =>
                new ApizrExceptionHandler<TResult>(ex => onException(serviceProvider, ex)),
                letThrowOnHandledException,
                strategy);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithExCatching<THandler>(THandler exceptionHandler,
            bool letThrowOnHandledException = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace) where THandler : IApizrExceptionHandler
            => WithExCatching(_ => exceptionHandler, letThrowOnHandledException, strategy);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithExCatching<THandler>(Func<IServiceProvider, THandler> exceptionHandlerFactory,
            bool letThrowOnHandledException = true, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace) where THandler : IApizrExceptionHandler
        {
            switch (strategy)
            {
                case ApizrDuplicateStrategy.Ignore:
                    Options.ExceptionHandlersFactory ??= serviceProvider => [exceptionHandlerFactory(serviceProvider)];
                    break;
                case ApizrDuplicateStrategy.Replace:
                    Options.ExceptionHandlersFactory = serviceProvider => [exceptionHandlerFactory(serviceProvider)];
                    break;
                case ApizrDuplicateStrategy.Add:
                case ApizrDuplicateStrategy.Merge:
                    if (Options.ExceptionHandlersFactory == null)
                        Options.ExceptionHandlersFactory = serviceProvider => [exceptionHandlerFactory(serviceProvider)];
                    else
                    {
                        var previous = Options.ExceptionHandlersFactory;
                        Options.ExceptionHandlersFactory = serviceProvider =>
                        {
                            var exceptionHandlers = previous?.Invoke(serviceProvider) ?? [];
                            exceptionHandlers.Add(exceptionHandlerFactory(serviceProvider));
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
        public IApizrExtendedProperOptionsBuilder WithExCatching<THandler>(bool letThrowOnHandledException = true,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Replace) where THandler : IApizrExceptionHandler
            => WithExCatching(serviceProvider => serviceProvider.GetRequiredService<THandler>(),
                letThrowOnHandledException, strategy);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithHandlerParameter(string key, object value)
        {
            Options.HandlersParameters[key] = value;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithResilienceProperty<TValue>(ResiliencePropertyKey<TValue> key, TValue value)
            => WithResilienceProperty(key, _ => value);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithResilienceProperty<TValue>(ResiliencePropertyKey<TValue> key, Func<IServiceProvider, TValue> valueFactory)
        {
            ((IApizrExtendedSharedOptions)Options).ResiliencePropertiesExtendedFactories[key.Key] = serviceProvider => valueFactory(serviceProvider);

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithLogging(HttpTracerMode httpTracerMode = HttpTracerMode.Everything,
            HttpMessageParts trafficVerbosity = HttpMessageParts.All, params LogLevel[] logLevels)
            => WithLogging(_ => httpTracerMode, _ => trafficVerbosity,
                _ => logLevels?.Length > 0 ? logLevels : Constants.DefaultLogLevels);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithLogging(
            Func<IServiceProvider, (HttpTracerMode, HttpMessageParts, LogLevel[])> loggingConfigurationFactory)
            => WithLogging(serviceProvider => loggingConfigurationFactory.Invoke(serviceProvider).Item1,
                serviceProvider => loggingConfigurationFactory.Invoke(serviceProvider).Item2,
                serviceProvider => loggingConfigurationFactory.Invoke(serviceProvider).Item3);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithLogging(
            Func<IServiceProvider, HttpTracerMode> httpTracerModeFactory,
            Func<IServiceProvider, HttpMessageParts> trafficVerbosityFactory,
            Func<IServiceProvider, LogLevel[]> logLevelsFactory)
        {
            Options.HttpTracerModeFactory = httpTracerModeFactory;
            Options.TrafficVerbosityFactory = trafficVerbosityFactory;
            Options.LogLevelsFactory = logLevelsFactory;

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithResilienceContextOptions(Action<IApizrResilienceContextOptionsBuilder> contextOptionsBuilder)
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
        public IApizrExtendedProperOptionsBuilder WithLoggedHeadersRedactionNames(IEnumerable<string> redactedLoggedHeaderNames,
            ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add)
        {
            var sensitiveHeaders = new HashSet<string>(redactedLoggedHeaderNames, StringComparer.OrdinalIgnoreCase);

            return WithLoggedHeadersRedactionRule(header => sensitiveHeaders.Contains(header), strategy);
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithLoggedHeadersRedactionRule(
            Func<string, bool> shouldRedactHeaderValue, ApizrDuplicateStrategy strategy = ApizrDuplicateStrategy.Add)
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
        public IApizrExtendedProperOptionsBuilder WithResiliencePipelineKeys(string[] resiliencePipelineKeys,
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
        public IApizrExtendedProperOptionsBuilder WithCaching(CacheMode mode = CacheMode.FetchOrGet, TimeSpan? lifeSpan = null,
            bool shouldInvalidateOnError = false)
        {
            Options.CacheOptions[ApizrConfigurationSource.ProperOption] = new CacheAttribute(mode, lifeSpan, shouldInvalidateOnError);

            return this;
        }

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithRequestOptions(string requestName,
            Action<IApizrRequestOptionsBuilder> optionsBuilder,
            ApizrDuplicateStrategy duplicateStrategy = ApizrDuplicateStrategy.Add)
            => WithRequestOptions([requestName], optionsBuilder, duplicateStrategy);

        /// <inheritdoc />
        public IApizrExtendedProperOptionsBuilder WithRequestOptions(string[] requestNames,
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
            => WithDelegatingHandler((_, options) => handlerFactory.Invoke(
                options));

        #endregion
    }
}
