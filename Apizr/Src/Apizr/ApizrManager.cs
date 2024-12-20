﻿// Largely inspired by Refit.Insane.PowerPack, but with many more features

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Caching;
using Apizr.Caching.Attributes;
using Apizr.Cancelling.Attributes;
using Apizr.Cancelling.Attributes.Operation;
using Apizr.Cancelling.Attributes.Request;
using Apizr.Configuring;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Proper;
using Apizr.Configuring.Request;
using Apizr.Configuring.Shared;
using Apizr.Configuring.Shared.Context;
using Apizr.Connecting;
using Apizr.Extending;
using Apizr.Logging;
using Apizr.Logging.Attributes;
using Apizr.Mapping;
using Apizr.Requesting.Attributes;
using Apizr.Resiliencing;
using Apizr.Resiliencing.Attributes;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;
using Refit;

namespace Apizr
{
    public abstract class ApizrManager : IApizrManager
    {
        internal static IApizrRequestOptionsBuilder CreateRequestOptionsBuilder(
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => CreateRequestOptionsBuilder(null, optionsBuilder);

        internal static IApizrRequestOptionsBuilder CreateRequestOptionsBuilder(
            IApizrGlobalSharedRegistrationOptionsBase baseOptions,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null,
            string requestName = null,
            LogAttributeBase requestLogAttribute = null,
            IList<HandlerParameterAttribute> requestHandlerParameterAttributes = null,
            TimeoutAttributeBase operationTimeoutAttribute = null,
            TimeoutAttributeBase requestTimeoutAttribute = null,
            ResiliencePipelineAttributeBase resiliencePipelineAttribute = null,
            CacheAttributeBase requestCacheAttribute = null,
            ApizrRequestMethod requestMethod = null)
        {
            // Create base request options from parent options
            var requestOptions = new ApizrRequestOptions(baseOptions,
                requestHandlerParameterAttributes?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ?? [],
                requestLogAttribute?.HttpTracerMode,
                requestLogAttribute?.TrafficVerbosity,
                operationTimeoutAttribute?.Timeout,
                requestTimeoutAttribute?.Timeout,
                resiliencePipelineAttribute,
                requestCacheAttribute,
                requestMethod,
                requestLogAttribute?.LogLevels);

            // Create request options builder with request options
            var builder = new ApizrRequestOptionsBuilder(requestOptions) as IApizrRequestOptionsBuilder;

            // Only once with full options building
            if (baseOptions != null)
            {
                // Apply registered request options if any
                var properOptionsBase = baseOptions as IApizrProperOptionsBase;
                if(!string.IsNullOrWhiteSpace(requestName) && properOptionsBase?.RequestOptionsBuilders.TryGetValue(requestName!, out var requestOptionsBuilderBase) == true)
                    requestOptionsBuilderBase.Invoke(builder);

                // Refresh request scoped headers if any
                if (baseOptions.HeadersFactories?.TryGetValue((ApizrRegistrationMode.Set, ApizrLifetimeScope.Request), out var setFactory) == true)
                {
                    // Set refreshed headers right the way
                    var setHeaders = setFactory?.Invoke()?.ToArray();
                    if (setHeaders?.Length > 0)
                        builder.WithHeaders(setHeaders, ApizrRegistrationMode.Set);
                }

                if (baseOptions.HeadersFactories?.TryGetValue((ApizrRegistrationMode.Store, ApizrLifetimeScope.Request), out var storeFactory) == true)
                {
                    // Store refreshed headers for further attribute key match use
                    var storeHeaders = storeFactory?.Invoke()?.ToArray();
                    if (storeHeaders?.Length > 0)
                        builder.WithHeaders(storeHeaders, ApizrRegistrationMode.Store);
                } 
            }

            // Apply latest request options if any
            optionsBuilder?.Invoke(builder);

            // Only once with full options building
            if (baseOptions != null)
            {
                // Check for header values redaction
                var redactHeaders = new List<string>();
                foreach (var header in builder.ApizrOptions.Headers)
                    if (HttpRequestMessageExtensions.TryGetHeaderKeyValue(header, out var key, out var value) && value.StartsWith("*") && value.EndsWith("*"))
                        redactHeaders.Add(key);

                // Apply redacted headers if any
                if (redactHeaders.Count > 0)
                    builder.WithLoggedHeadersRedactionNames(redactHeaders);

                // Apply context options if any
                if (builder.ApizrOptions.ContextOptionsBuilder != null)
                {
                    var contextOptions = new ApizrResilienceContextOptions();
                    var contextOptionsBuilder = new ApizrResilienceContextOptionsBuilder(contextOptions) as IApizrResilienceContextOptionsBuilder;
                    builder.ApizrOptions.ContextOptionsBuilder.Invoke(contextOptionsBuilder);
                    builder.WithResilienceContextOptions(contextOptionsBuilder.ResilienceContextOptions);
                }

                // Set final cache configuration if any
                if(builder.ApizrOptions.CacheOptions.Count > 0)
                    builder.ApizrOptions.CacheOptions[ApizrConfigurationSource.FinalConfiguration] = builder.ApizrOptions.CacheOptions
                        .OrderBy(x => x.Key)
                        .LastOrDefault(x => x.Key != ApizrConfigurationSource.FinalConfiguration)
                        .Value;

                // Filter out all resilience pipeline keys that are not related to the current request method
                var filteredPipelineOptions = builder.ApizrOptions.ResiliencePipelineOptions
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Where(attribute => attribute.RequestMethod >= requestMethod).ToArray()) // Remove all attributes with a lower request method
                    .Where(kvp => kvp.Value.Length > 0) // Ignore keys with empty attribute values
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value); // Return the filtered dictionary
                builder.WithResiliencePipelineOptions(filteredPipelineOptions);
            }

            // Return the builder
            return builder;
        }
    }

    /// <summary>
    /// The manager
    /// </summary>
    public class ApizrManager<TWebApi> : ApizrManager, IApizrManager<TWebApi>
    {
        #region Fields

        private readonly ILazyFactory<TWebApi> _lazyWebApi;
        private readonly IConnectivityHandler _connectivityHandler;
        private readonly ICacheHandler _cacheHandler;
        private readonly IMappingHandler _mappingHandler;
        private readonly ILazyFactory<ResiliencePipelineRegistry<string>> _lazyResiliencePipelineRegistry;
        private readonly string _webApiFriendlyName;
        private readonly IApizrManagerOptions<TWebApi> _apizrOptions;

        private readonly ConcurrentDictionary<MethodDetails, string> _cacheKeyMethodsSet;
        private readonly ConcurrentDictionary<MethodDetails, List<string>> _headersMethodsSet;
        private readonly ConcurrentDictionary<MethodDetails, CacheAttributeBase> _cachingMethodsSet;
        private readonly ConcurrentDictionary<MethodDetails, LogAttributeBase> _loggingMethodsSet;
        private readonly ConcurrentDictionary<MethodDetails, IList<HandlerParameterAttribute>> _handlerParameterMethodsSet;
        private readonly ConcurrentDictionary<MethodDetails, TimeoutAttributeBase> _operationTimeoutMethodsSet;
        private readonly ConcurrentDictionary<MethodDetails, TimeoutAttributeBase> _requestTimeoutMethodsSet;
        private readonly ConcurrentDictionary<MethodDetails, ResiliencePipelineAttributeBase> _resilienceMethodsSet;

        #endregion

        /// <summary>
        /// Apizr manager constructor
        /// </summary>
        /// <param name="lazyWebApi">The managed web api</param>
        /// <param name="connectivityHandler">The connectivity handler</param>
        /// <param name="cacheHandler">The cache handler</param>
        /// <param name="mappingHandler">The mapping handler</param>
        /// <param name="lazyResiliencePipelineRegistry">The resilience pipeline registry</param>
        /// <param name="apizrOptions">The web api dedicated options</param>
        public ApizrManager(ILazyFactory<TWebApi> lazyWebApi, 
            IConnectivityHandler connectivityHandler,
            ICacheHandler cacheHandler, 
            IMappingHandler mappingHandler, 
            ILazyFactory<ResiliencePipelineRegistry<string>> lazyResiliencePipelineRegistry,
            IApizrManagerOptions<TWebApi> apizrOptions)
        {
            _lazyWebApi = lazyWebApi;
            _connectivityHandler = connectivityHandler;
            _cacheHandler = cacheHandler;
            _mappingHandler = mappingHandler;
            _lazyResiliencePipelineRegistry = lazyResiliencePipelineRegistry;
            _webApiFriendlyName = typeof(TWebApi).GetFriendlyName();
            _apizrOptions = apizrOptions;

            _headersMethodsSet = new ConcurrentDictionary<MethodDetails, List<string>>();
            _cacheKeyMethodsSet = new ConcurrentDictionary<MethodDetails, string>();
            _cachingMethodsSet = new ConcurrentDictionary<MethodDetails, CacheAttributeBase>();
            _loggingMethodsSet = new ConcurrentDictionary<MethodDetails, LogAttributeBase>();
            _handlerParameterMethodsSet = new ConcurrentDictionary<MethodDetails, IList<HandlerParameterAttribute>>();
            _operationTimeoutMethodsSet = new ConcurrentDictionary<MethodDetails, TimeoutAttributeBase>();
            _requestTimeoutMethodsSet = new ConcurrentDictionary<MethodDetails, TimeoutAttributeBase>();
            _resilienceMethodsSet = new ConcurrentDictionary<MethodDetails, ResiliencePipelineAttributeBase>();
        }

        #region Implementation

        /// <inheritdoc />
        public TWebApi Api => _lazyWebApi.Value;

        /// <inheritdoc />
        public IApizrManagerOptionsBase Options => _apizrOptions;

        #region ExecuteAsync

        #region Task

        /// <inheritdoc />
        public virtual Task ExecuteAsync(
            Expression<Func<TWebApi, Task>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => ExecuteAsync((_, webApi) => executeApiMethod.Compile().Invoke(webApi),
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public virtual async Task ExecuteAsync(
            Expression<Func<IApizrRequestOptions, TWebApi, Task>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        {
            var webApi = _lazyWebApi.Value;
            var requestOnlyOptionsBuilder = CreateRequestOptionsBuilder(optionsBuilder);
            var originalExpression = requestOnlyOptionsBuilder.ApizrOptions.OriginalExpression ?? executeApiMethod;
            var methodDetails = GetMethodDetails(originalExpression);
            var requestRedactHeaders = GetRequestHeadersAttributeRedactFactory(methodDetails);
            var requestLogAttribute = GetRequestLogAttribute(methodDetails);
            var requestHandlerParameterAttributes = GetRequestHandlerParameterAttributes(methodDetails);
            var operationTimeoutAttribute = GetOperationTimeoutAttribute(methodDetails);
            var requestTimeoutAttribute = GetRequestTimeoutAttribute(methodDetails);
            var resiliencePipelineAttribute = GetMethodResiliencePipelineAttribute(methodDetails);
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, methodDetails.MethodInfo.Name, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute, resiliencePipelineAttribute);

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            ResilienceContext resilienceContext = null;
            try
            {
                if (!_connectivityHandler.IsConnected())
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                        $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                    throw new IOException("Connectivity check failed");
                }

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                var resiliencePipeline = GetMethodResiliencePipeline(methodDetails, requestOptionsBuilder.ApizrOptions);
                resilienceContext = ResilienceContextPool.Shared.Get(methodDetails.MethodInfo.Name,
                    requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ContinueOnCapturedContext,
                    requestOptionsBuilder.ApizrOptions.CancellationToken);
                if (requestOptionsBuilder.ApizrOptions is IApizrGlobalSharedOptionsBase internalOptions &&
                    internalOptions.ResiliencePropertiesFactories.Any())
                    resilienceContext.Properties.SetProperties(internalOptions.ResiliencePropertiesFactories.ToDictionary(kvp => kvp.Key, kvp => kvp.Value()), out _);
                resilienceContext.WithLogger(_apizrOptions.Logger, requestOptionsBuilder.ApizrOptions.LogLevels,
                    requestOptionsBuilder.ApizrOptions.TrafficVerbosity,
                    requestOptionsBuilder.ApizrOptions.HttpTracerMode);
                requestOptionsBuilder.WithContext(resilienceContext);

                if (requestRedactHeaders.Count > 0)
                    requestOptionsBuilder.WithLoggedHeadersRedactionNames(requestRedactHeaders);

                if (requestOptionsBuilder.ApizrOptions.ShouldRedactHeaderValue != null)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Applying some header redaction rules to the request");

                requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Executing the request");

                await resiliencePipeline.ExecuteAsync(
                    async _ => await executeApiMethod.Compile().Invoke(requestOptionsBuilder.ApizrOptions, webApi).ConfigureAwait(resilienceContext.ContinueOnCapturedContext),
                    requestOptionsBuilder).ConfigureAwait(resilienceContext.ContinueOnCapturedContext);
            }
            catch (Exception e)
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                    $"{methodDetails.MethodInfo.Name}: Request threw an exception with message {e.Message}");
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                    $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException)} with InnerException");

                var ex = new ApizrException(e);
                if (requestOptionsBuilder.ApizrOptions.ExceptionHandlers.Count == 0)
                    throw ex;

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: {nameof(ApizrException)} might be handled by callback");

                foreach (var exceptionHandler in requestOptionsBuilder.ApizrOptions.ExceptionHandlers)
                {
                    var exceptionHandled = await exceptionHandler.HandleAsync(ex).ConfigureAwait(false);
                    if (exceptionHandled) 
                        ex.Handled = true;
                }

                if (!ex.Handled || requestOptionsBuilder.ApizrOptions.LetThrowOnHandledException)
                    throw ex;
            }
            finally
            {
                if(resilienceContext != null &&
                   requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ReturnToPoolOnComplete != false)
                    ResilienceContextPool.Shared.Return(resilienceContext);
            }
        }

        /// <inheritdoc />
        public virtual Task ExecuteAsync<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => ExecuteAsync<TModelData, TApiData>(
                (_, webApi, apiData) => executeApiMethod.Compile().Invoke(webApi, apiData),
                modelData,
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public virtual async Task ExecuteAsync<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        {
            var webApi = _lazyWebApi.Value;
            var requestOnlyOptionsBuilder = CreateRequestOptionsBuilder(optionsBuilder);
            var originalExpression = requestOnlyOptionsBuilder.ApizrOptions.OriginalExpression ?? executeApiMethod;
            var methodDetails = GetMethodDetails(originalExpression);
            var requestRedactHeaders = GetRequestHeadersAttributeRedactFactory(methodDetails);
            var requestLogAttribute = GetRequestLogAttribute(methodDetails);
            var requestHandlerParameterAttributes = GetRequestHandlerParameterAttributes(methodDetails);
            var operationTimeoutAttribute = GetOperationTimeoutAttribute(methodDetails);
            var requestTimeoutAttribute = GetRequestTimeoutAttribute(methodDetails);
            var resiliencePipelineAttribute = GetMethodResiliencePipelineAttribute(methodDetails);
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, methodDetails.MethodInfo.Name, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute, resiliencePipelineAttribute);

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            ResilienceContext resilienceContext = null;
            try
            {
                requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                if (!_connectivityHandler.IsConnected())
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                        $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                    throw new IOException("Connectivity check failed");
                }

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                var resiliencePipeline = GetMethodResiliencePipeline(methodDetails, requestOptionsBuilder.ApizrOptions);
                resilienceContext = ResilienceContextPool.Shared.Get(methodDetails.MethodInfo.Name,
                    requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ContinueOnCapturedContext,
                    requestOptionsBuilder.ApizrOptions.CancellationToken);
                if (requestOptionsBuilder.ApizrOptions is IApizrGlobalSharedOptionsBase internalOptions &&
                    internalOptions.ResiliencePropertiesFactories.Any())
                    resilienceContext.Properties.SetProperties(internalOptions.ResiliencePropertiesFactories.ToDictionary(kvp => kvp.Key, kvp => kvp.Value()), out _);
                resilienceContext.WithLogger(_apizrOptions.Logger, requestOptionsBuilder.ApizrOptions.LogLevels,
                    requestOptionsBuilder.ApizrOptions.TrafficVerbosity,
                    requestOptionsBuilder.ApizrOptions.HttpTracerMode);
                requestOptionsBuilder.WithContext(resilienceContext);

                if (requestRedactHeaders.Count > 0)
                    requestOptionsBuilder.WithLoggedHeadersRedactionNames(requestRedactHeaders);

                if (requestOptionsBuilder.ApizrOptions.ShouldRedactHeaderValue != null)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Applying some header redaction rules to the request");

                var apiData = Map<TModelData, TApiData>(modelData);
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: {typeof(TModelData).Name} mapped to {typeof(TApiData).Name}");

                requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Executing the request");

                await resiliencePipeline.ExecuteAsync(
                    async options => await executeApiMethod.Compile().Invoke(options, webApi, apiData).ConfigureAwait(resilienceContext.ContinueOnCapturedContext),
                    requestOptionsBuilder).ConfigureAwait(resilienceContext.ContinueOnCapturedContext);
            }
            catch (Exception e)
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                    $"{methodDetails.MethodInfo.Name}: Request threw an exception with message {e.Message}");
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                    $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException)} with InnerException");

                var ex = new ApizrException(e);
                if (requestOptionsBuilder.ApizrOptions.ExceptionHandlers.Count == 0)
                    throw ex;

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: {nameof(ApizrException)} might be handled by callback");

                foreach (var exceptionHandler in requestOptionsBuilder.ApizrOptions.ExceptionHandlers)
                {
                    var exceptionHandled = await exceptionHandler.HandleAsync(ex).ConfigureAwait(false);
                    if (exceptionHandled)
                        ex.Handled = true;
                }

                if (!ex.Handled || requestOptionsBuilder.ApizrOptions.LetThrowOnHandledException)
                    throw ex;
            }
            finally
            {
                if (resilienceContext != null &&
                    requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ReturnToPoolOnComplete != false)
                    ResilienceContextPool.Shared.Return(resilienceContext);
            }
        }

        #endregion

        #region Task<T>

        /// <inheritdoc />
        public virtual Task<IApizrResponse> ExecuteAsync(Expression<Func<TWebApi, Task<IApiResponse>>> executeApiMethod, 
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => ExecuteAsync((_, api) => executeApiMethod.Compile().Invoke(api),
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public virtual Task<TApiData> ExecuteAsync<TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => ExecuteAsync((_, api) => executeApiMethod.Compile().Invoke(api),
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public virtual Task<IApizrResponse<TApiData>> ExecuteAsync<TApiData>(
            Expression<Func<TWebApi, Task<ApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => ExecuteAsync((_, api) => executeApiMethod.Compile().Invoke(api)
                    .ContinueWith(task => (IApiResponse<TApiData>) task.Result),
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public virtual Task<IApizrResponse<TApiData>> ExecuteAsync<TApiData>(
            Expression<Func<TWebApi, Task<IApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => ExecuteAsync((_, api) => executeApiMethod.Compile().Invoke(api),
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public virtual async Task<IApizrResponse> ExecuteAsync(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<IApiResponse>>> executeApiMethod, 
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        {
            var webApi = _lazyWebApi.Value;
            var requestOnlyOptionsBuilder = CreateRequestOptionsBuilder(optionsBuilder);
            var originalExpression = requestOnlyOptionsBuilder.ApizrOptions.OriginalExpression ?? executeApiMethod;
            var methodDetails = GetMethodDetails<IApiResponse>(originalExpression);
            var requestRedactHeaders = GetRequestHeadersAttributeRedactFactory(methodDetails);
            var requestLogAttribute = GetRequestLogAttribute(methodDetails);
            var requestHandlerParameterAttributes = GetRequestHandlerParameterAttributes(methodDetails);
            var operationTimeoutAttribute = GetOperationTimeoutAttribute(methodDetails);
            var requestTimeoutAttribute = GetRequestTimeoutAttribute(methodDetails);
            var resiliencePipelineAttribute = GetMethodResiliencePipelineAttribute(methodDetails);
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, methodDetails.MethodInfo.Name, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute, resiliencePipelineAttribute);

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            ApizrResponse response = default;
            ResilienceContext resilienceContext = null;

            try
            {
                requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                if (!_connectivityHandler.IsConnected())
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                        $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                    throw new IOException("Connectivity check failed");
                }

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                var resiliencePipeline = GetMethodResiliencePipeline<IApiResponse>(methodDetails, requestOptionsBuilder.ApizrOptions);
                resilienceContext = ResilienceContextPool.Shared.Get(methodDetails.MethodInfo.Name,
                    requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ContinueOnCapturedContext,
                    requestOptionsBuilder.ApizrOptions.CancellationToken);
                if (requestOptionsBuilder.ApizrOptions is IApizrGlobalSharedOptionsBase internalOptions &&
                    internalOptions.ResiliencePropertiesFactories.Any())
                    resilienceContext.Properties.SetProperties(internalOptions.ResiliencePropertiesFactories.ToDictionary(kvp => kvp.Key, kvp => kvp.Value()), out _);
                resilienceContext.WithLogger(_apizrOptions.Logger, requestOptionsBuilder.ApizrOptions.LogLevels,
                    requestOptionsBuilder.ApizrOptions.TrafficVerbosity,
                    requestOptionsBuilder.ApizrOptions.HttpTracerMode);
                requestOptionsBuilder.WithContext(resilienceContext);

                if (requestRedactHeaders.Count > 0)
                    requestOptionsBuilder.WithLoggedHeadersRedactionNames(requestRedactHeaders);

                if (requestOptionsBuilder.ApizrOptions.ShouldRedactHeaderValue != null)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Applying some header redaction rules to the request");

                requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Executing the request");

                var apiResponse = await resiliencePipeline.ExecuteAsync(
                    async options => await executeApiMethod.Compile().Invoke(options, webApi).ConfigureAwait(resilienceContext.ContinueOnCapturedContext),
                    requestOptionsBuilder).ConfigureAwait(resilienceContext.ContinueOnCapturedContext);

                if (apiResponse.IsSuccessStatusCode && apiResponse.Error == null)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Request succeed!");
                else
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Request failed!");

                response = apiResponse.Error == null ?
                    new ApizrResponse(apiResponse) :
                    new ApizrResponse(apiResponse, new ApizrException(apiResponse.Error));
            }
            catch (Exception e)
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                    $"{methodDetails.MethodInfo.Name}: Request threw an exception with message {e.Message}");

                response = new ApizrResponse(new ApizrException(e));
            }
            finally
            {
                if (resilienceContext != null &&
                    requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ReturnToPoolOnComplete != false)
                    ResilienceContextPool.Shared.Return(resilienceContext);
            }

            if (response.Exception != null && requestOptionsBuilder.ApizrOptions.ExceptionHandlers.Count > 0)
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Exception might be handled by callback");

                foreach (var exceptionHandler in requestOptionsBuilder.ApizrOptions.ExceptionHandlers)
                {
                    var exceptionHandled = await exceptionHandler.HandleAsync(response.Exception).ConfigureAwait(false);
                    if (exceptionHandled)
                        response.Exception.Handled = true;
                }
            }

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Returning result");

            return response;
        }

        /// <inheritdoc />
        public virtual async Task<TApiData> ExecuteAsync<TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        {
            var webApi = _lazyWebApi.Value;
            var requestOnlyOptionsBuilder = CreateRequestOptionsBuilder(optionsBuilder);
            var originalExpression = requestOnlyOptionsBuilder.ApizrOptions.OriginalExpression ?? executeApiMethod;
            var methodDetails = GetMethodDetails<TApiData>(originalExpression);
            var requestRedactHeaders = GetRequestHeadersAttributeRedactFactory(methodDetails);
            var requestLogAttribute = GetRequestLogAttribute(methodDetails);
            var requestHandlerParameterAttributes = GetRequestHandlerParameterAttributes(methodDetails);
            var operationTimeoutAttribute = GetOperationTimeoutAttribute(methodDetails);
            var requestTimeoutAttribute = GetRequestTimeoutAttribute(methodDetails);
            var resiliencePipelineAttribute = GetMethodResiliencePipelineAttribute(methodDetails);
            var requestCacheAttribute = GetMethodCacheAttribute(methodDetails);
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, methodDetails.MethodInfo.Name, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute,
                resiliencePipelineAttribute, requestCacheAttribute, methodDetails.RequestMethod);

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiData result = default;

            if (ShouldCache<TApiData>(methodDetails, originalExpression, requestOptionsBuilder.ApizrOptions, out var cacheAttribute))
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheAttribute.CacheKey}");

                result = await _cacheHandler.GetAsync<TApiData>(cacheAttribute.CacheKey,
                    requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                if (!Equals(result, default))
                {
                    if (requestOptionsBuilder.ApizrOptions.ClearCache)
                    {
                        await _cacheHandler.RemoveAsync(cacheAttribute.CacheKey, requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                        result = default;
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Cached data cleared for this cache key");
                    }
                    else
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");
                    }
                }
            }

            if (Equals(result, default) || cacheAttribute?.FinalMode != CacheMode.GetOrFetch)
            {
                ResilienceContext resilienceContext = null;
                ApizrException<TApiData> ex = null;
                try
                {
                    requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }

                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var resiliencePipeline = GetMethodResiliencePipeline<TApiData>(methodDetails, requestOptionsBuilder.ApizrOptions);
                    resilienceContext = ResilienceContextPool.Shared.Get(methodDetails.MethodInfo.Name,
                        requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ContinueOnCapturedContext,
                        requestOptionsBuilder.ApizrOptions.CancellationToken);
                    if (requestOptionsBuilder.ApizrOptions is IApizrGlobalSharedOptionsBase internalOptions &&
                        internalOptions.ResiliencePropertiesFactories.Any())
                        resilienceContext.Properties.SetProperties(internalOptions.ResiliencePropertiesFactories.ToDictionary(kvp => kvp.Key, kvp => kvp.Value()), out _);
                    resilienceContext.WithLogger(_apizrOptions.Logger, requestOptionsBuilder.ApizrOptions.LogLevels,
                        requestOptionsBuilder.ApizrOptions.TrafficVerbosity,
                        requestOptionsBuilder.ApizrOptions.HttpTracerMode);
                    requestOptionsBuilder.WithContext(resilienceContext);

                    if (requestRedactHeaders.Count > 0)
                        requestOptionsBuilder.WithLoggedHeadersRedactionNames(requestRedactHeaders);

                    if (requestOptionsBuilder.ApizrOptions.ShouldRedactHeaderValue != null)
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Applying some header redaction rules to the request");

                    requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Executing the request");

                    result = await resiliencePipeline.ExecuteAsync(
                        async options => await executeApiMethod.Compile().Invoke(options, webApi).ConfigureAwait(resilienceContext.ContinueOnCapturedContext),
                        requestOptionsBuilder).ConfigureAwait(resilienceContext.ContinueOnCapturedContext);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                        $"{methodDetails.MethodInfo.Name}: Request threw an exception with message {e.Message}");

                    ex = new ApizrException<TApiData>(e, result);
                    if (requestOptionsBuilder.ApizrOptions.ExceptionHandlers.Count == 0)
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                            Equals(result, default(TApiData))
                                ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TApiData>)} with InnerException but no cached result"
                                : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TApiData>)} with InnerException and cached result");

                        throw ex;
                    }

                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        Equals(result, default(TApiData))
                            ? $"{methodDetails.MethodInfo.Name}: {nameof(ApizrException<TApiData>)} might be handled by callback with InnerException but no cached result"
                            : $"{methodDetails.MethodInfo.Name}: {nameof(ApizrException<TApiData>)} might be handled by callback with InnerException and cached result");

                    foreach (var exceptionHandler in requestOptionsBuilder.ApizrOptions.ExceptionHandlers)
                    {
                        var exceptionHandled = await exceptionHandler.HandleAsync(ex).ConfigureAwait(false);
                        if (exceptionHandled)
                            ex.Handled = true;
                    }

                    if (!ex.Handled || requestOptionsBuilder.ApizrOptions.LetThrowOnHandledException)
                        throw ex;
                }
                finally
                {
                    if (resilienceContext != null &&
                        requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ReturnToPoolOnComplete != false)
                        ResilienceContextPool.Shared.Return(resilienceContext);
                }

                if (ex == null &&
                    result != null &&
                    cacheAttribute != null &&
                    !string.IsNullOrWhiteSpace(cacheAttribute.CacheKey) &&
                    cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheAttribute.CacheKey, result, cacheAttribute.LifeSpan,
                        requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                }
            }

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Returning result");

            return result;
        }

        /// <inheritdoc />
        public virtual Task<IApizrResponse<TApiData>> ExecuteAsync<TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<ApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => ExecuteAsync(
                (opt, api) => executeApiMethod.Compile().Invoke(opt, api)
                    .ContinueWith(task => (IApiResponse<TApiData>) task.Result),
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public virtual async Task<IApizrResponse<TApiData>> ExecuteAsync<TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<IApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        {
            var webApi = _lazyWebApi.Value;
            var requestOnlyOptionsBuilder = CreateRequestOptionsBuilder(optionsBuilder);
            var originalExpression = requestOnlyOptionsBuilder.ApizrOptions.OriginalExpression ?? executeApiMethod;
            var methodDetails = GetMethodDetails<TApiData>(originalExpression);
            var requestRedactHeaders = GetRequestHeadersAttributeRedactFactory(methodDetails);
            var requestLogAttribute = GetRequestLogAttribute(methodDetails);
            var requestHandlerParameterAttributes = GetRequestHandlerParameterAttributes(methodDetails);
            var operationTimeoutAttribute = GetOperationTimeoutAttribute(methodDetails);
            var requestTimeoutAttribute = GetRequestTimeoutAttribute(methodDetails);
            var resiliencePipelineAttribute = GetMethodResiliencePipelineAttribute(methodDetails);
            var requestCacheAttribute = GetMethodCacheAttribute(methodDetails);
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, methodDetails.MethodInfo.Name, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute,
                resiliencePipelineAttribute, requestCacheAttribute);

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiData cachedResult = default;
            ApizrResponse<TApiData> response = default;

            if (ShouldCache<TApiData>(methodDetails, originalExpression, requestOptionsBuilder.ApizrOptions, out var cacheAttribute))
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheAttribute.CacheKey}");

                cachedResult = await _cacheHandler.GetAsync<TApiData>(cacheAttribute.CacheKey,
                    requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                if (!Equals(cachedResult, default))
                {
                    if (requestOptionsBuilder.ApizrOptions.ClearCache)
                    {
                        await _cacheHandler.RemoveAsync(cacheAttribute.CacheKey, requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                        cachedResult = default;
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Cached data cleared for this cache key");
                    }
                    else
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");

                        if (cacheAttribute.Mode == CacheMode.SetByHeader)
                        {
                            var finalCacheMode = await _cacheHandler.GetAsync<CacheMode>(
                                cacheAttribute.FinalModeCacheKey,
                                requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                            if (finalCacheMode != CacheMode.None)
                            {
                                cacheAttribute.FinalMode = finalCacheMode;
                                if (finalCacheMode == CacheMode.FetchOrGet)
                                {
                                    var cacheHeaders = await _cacheHandler.GetAsync<string[]>(
                                        cacheAttribute.HeadersCacheKey,
                                        requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                                    if (cacheHeaders?.Length > 0)
                                        cacheAttribute.Headers = cacheHeaders;
                                }
                            }
                        }
                    }
                }
            }

            if (!Equals(cachedResult, default) && cacheAttribute?.FinalMode == CacheMode.GetOrFetch)
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Using cached data for the response");

                response = new ApizrResponse<TApiData>(cachedResult, ApizrResponseDataSource.Cache);
            }
            else
            {
                ResilienceContext resilienceContext = null;
                try
                {
                    requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }

                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var resiliencePipeline = GetMethodResiliencePipeline<IApiResponse<TApiData>>(methodDetails, requestOptionsBuilder.ApizrOptions);
                    resilienceContext = ResilienceContextPool.Shared.Get(methodDetails.MethodInfo.Name,
                        requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ContinueOnCapturedContext,
                        requestOptionsBuilder.ApizrOptions.CancellationToken);
                    if (requestOptionsBuilder.ApizrOptions is IApizrGlobalSharedOptionsBase internalOptions &&
                        internalOptions.ResiliencePropertiesFactories.Any())
                        resilienceContext.Properties.SetProperties(
                            internalOptions.ResiliencePropertiesFactories.ToDictionary(kvp => kvp.Key,
                                kvp => kvp.Value()), out _);
                    resilienceContext.WithLogger(_apizrOptions.Logger, requestOptionsBuilder.ApizrOptions.LogLevels,
                        requestOptionsBuilder.ApizrOptions.TrafficVerbosity,
                        requestOptionsBuilder.ApizrOptions.HttpTracerMode);
                    requestOptionsBuilder.WithContext(resilienceContext);

                    if (requestRedactHeaders.Count > 0)
                        requestOptionsBuilder.WithLoggedHeadersRedactionNames(requestRedactHeaders);

                    if (requestOptionsBuilder.ApizrOptions.ShouldRedactHeaderValue != null)
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Applying some header redaction rules to the request");

                    if (cacheAttribute?.Headers.Count > 0)
                        requestOptionsBuilder.WithHeaders(cacheAttribute.Headers.ToList());

                    requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Executing the request");

                    var apiResponse = await resiliencePipeline.ExecuteAsync(
                        async options => await executeApiMethod.Compile().Invoke(options, webApi).ConfigureAwait(resilienceContext.ContinueOnCapturedContext),
                        requestOptionsBuilder).ConfigureAwait(resilienceContext.ContinueOnCapturedContext);

                    var apiResult = apiResponse.Content;

                    if (apiResponse.IsSuccessStatusCode && apiResponse.Error == null)
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Request succeed! Using request data for the response");

                        response = new ApizrResponse<TApiData>(apiResponse, apiResult,
                            ApizrResponseDataSource.Request);
                    }
                    else if (!Equals(cachedResult, default(TApiData)))
                    {
                        if (apiResponse.StatusCode == HttpStatusCode.NotModified && cacheAttribute is
                            { Mode: CacheMode.SetByHeader, FinalMode: CacheMode.FetchOrGet })
                        {
                            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                                $"{methodDetails.MethodInfo.Name}: No modified data! Using cached data for the response");

                            response = new ApizrResponse<TApiData>(apiResponse, cachedResult,
                                ApizrResponseDataSource.Cache);
                        }
                        else
                        {
                            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                                $"{methodDetails.MethodInfo.Name}: Request failed! Using cached data for the response");

                            response = new ApizrResponse<TApiData>(apiResponse,
                                new ApizrException<TApiData>(apiResponse.Error, cachedResult));
                        }
                    }
                    else
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Request failed! No cached data to use for the response");

                        response = new ApizrResponse<TApiData>(apiResponse,
                            new ApizrException<TApiData>(apiResponse.Error));
                    }
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                        $"{methodDetails.MethodInfo.Name}: Request threw an exception with message {e.Message}");

                    response = new ApizrResponse<TApiData>(new ApizrException<TApiData>(e, cachedResult));
                }
                finally
                {
                    if (resilienceContext != null &&
                        requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ReturnToPoolOnComplete != false)
                        ResilienceContextPool.Shared.Return(resilienceContext);
                }

                if (response.Exception != null && requestOptionsBuilder.ApizrOptions.ExceptionHandlers.Count > 0)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        Equals(cachedResult, default(TApiData))
                            ? $"{methodDetails.MethodInfo.Name}: {nameof(ApizrException<TApiData>)} might be handled by callback with InnerException but no cached result"
                            : $"{methodDetails.MethodInfo.Name}: {nameof(ApizrException<TApiData>)} might be handled by callback with InnerException and cached result");

                    foreach (var exceptionHandler in requestOptionsBuilder.ApizrOptions.ExceptionHandlers)
                    {
                        var exceptionHandled = await exceptionHandler.HandleAsync(response.Exception).ConfigureAwait(false);
                        if (exceptionHandled)
                            response.Exception.Handled = true;
                    }
                }

                if (response.Exception == null && 
                    response.Result != null && 
                    _cacheHandler != null &&
                    cacheAttribute != null &&
                    !string.IsNullOrWhiteSpace(cacheAttribute.CacheKey) &&
                    cacheAttribute.Mode != CacheMode.None)
                {
                    var cacheEntries = GetCacheEntries(response, cacheAttribute, out var cacheLifeSpan);
                    if (cacheLifeSpan != TimeSpan.Zero)
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                                        $"{methodDetails.MethodInfo.Name}: Caching result");

                        foreach (var cacheEntry in cacheEntries)
                        {
                            await _cacheHandler.SetAsync(cacheEntry.Key, cacheEntry.Value, cacheLifeSpan,
                                                requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);  
                        }
                    }
                }
            }

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Returning response");

            return response;
        }

        /// <inheritdoc />
        public virtual Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => ExecuteAsync<TModelData, TApiData>(
                (_, api) => executeApiMethod.Compile().Invoke(api),
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public virtual Task<IApizrResponse<TModelData>> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<TWebApi, Task<ApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => ExecuteAsync<TModelData, TApiData>(
                (_, api) => executeApiMethod.Compile().Invoke(api)
                    .ContinueWith(task => (IApiResponse<TApiData>) task.Result),
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public virtual Task<IApizrResponse<TModelData>> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<TWebApi, Task<IApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => ExecuteAsync<TModelData, TApiData>(
                (_, api) => executeApiMethod.Compile().Invoke(api),
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public virtual async Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        {
            var webApi = _lazyWebApi.Value;
            var requestOnlyOptionsBuilder = CreateRequestOptionsBuilder(optionsBuilder);
            var originalExpression = requestOnlyOptionsBuilder.ApizrOptions.OriginalExpression ?? executeApiMethod;
            var methodDetails = GetMethodDetails<TApiData>(originalExpression);
            var requestRedactHeaders = GetRequestHeadersAttributeRedactFactory(methodDetails);
            var requestLogAttribute = GetRequestLogAttribute(methodDetails);
            var requestHandlerParameterAttributes = GetRequestHandlerParameterAttributes(methodDetails);
            var operationTimeoutAttribute = GetOperationTimeoutAttribute(methodDetails);
            var requestTimeoutAttribute = GetRequestTimeoutAttribute(methodDetails);
            var resiliencePipelineAttribute = GetMethodResiliencePipelineAttribute(methodDetails);
            var requestCacheAttribute = GetMethodCacheAttribute(methodDetails);
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, methodDetails.MethodInfo.Name, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute,
                resiliencePipelineAttribute, requestCacheAttribute);

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiData result = default;

            if (ShouldCache<TApiData>(methodDetails, originalExpression, requestOptionsBuilder.ApizrOptions, out var cacheAttribute))
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheAttribute.CacheKey}");

                result = await _cacheHandler.GetAsync<TApiData>(cacheAttribute.CacheKey,
                    requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                if (!Equals(result, default))
                {
                    if (requestOptionsBuilder.ApizrOptions.ClearCache)
                    {
                        await _cacheHandler.RemoveAsync(cacheAttribute.CacheKey, requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                        result = default;
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Cached data cleared for this cache key");
                    }
                    else
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");
                    }
                }
            }

            if (Equals(result, default) || cacheAttribute?.FinalMode != CacheMode.GetOrFetch)
            {
                ResilienceContext resilienceContext = null;
                ApizrException<TModelData> ex = null;
                try
                {
                    requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }

                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var resiliencePipeline = GetMethodResiliencePipeline<TApiData>(methodDetails, requestOptionsBuilder.ApizrOptions);
                    resilienceContext = ResilienceContextPool.Shared.Get(methodDetails.MethodInfo.Name,
                        requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ContinueOnCapturedContext,
                        requestOptionsBuilder.ApizrOptions.CancellationToken);
                    if (requestOptionsBuilder.ApizrOptions is IApizrGlobalSharedOptionsBase internalOptions &&
                        internalOptions.ResiliencePropertiesFactories.Any())
                        resilienceContext.Properties.SetProperties(internalOptions.ResiliencePropertiesFactories.ToDictionary(kvp => kvp.Key, kvp => kvp.Value()), out _);
                    resilienceContext.WithLogger(_apizrOptions.Logger, requestOptionsBuilder.ApizrOptions.LogLevels,
                        requestOptionsBuilder.ApizrOptions.TrafficVerbosity,
                        requestOptionsBuilder.ApizrOptions.HttpTracerMode);
                    requestOptionsBuilder.WithContext(resilienceContext);

                    if (requestRedactHeaders.Count > 0)
                        requestOptionsBuilder.WithLoggedHeadersRedactionNames(requestRedactHeaders);

                    if (requestOptionsBuilder.ApizrOptions.ShouldRedactHeaderValue != null)
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Applying some header redaction rules to the request");

                    requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Executing the request");

                    result = await resiliencePipeline.ExecuteAsync(
                        async options => await executeApiMethod.Compile().Invoke(options, webApi).ConfigureAwait(resilienceContext.ContinueOnCapturedContext),
                        requestOptionsBuilder).ConfigureAwait(resilienceContext.ContinueOnCapturedContext);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                        $"{methodDetails.MethodInfo.Name}: Request threw an exception with message {e.Message}");

                    ex = new ApizrException<TModelData>(e, Map<TApiData, TModelData>(result));
                    if (requestOptionsBuilder.ApizrOptions.ExceptionHandlers.Count == 0)
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                            Equals(result, default(TApiData))
                                ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelData>)} with InnerException but no cached result"
                                : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelData>)} with InnerException and cached result");

                        throw ex;
                    }

                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        Equals(result, default(TApiData))
                            ? $"{methodDetails.MethodInfo.Name}: {nameof(ApizrException<TModelData>)} might be handled by callback with InnerException but no cached result"
                            : $"{methodDetails.MethodInfo.Name}: {nameof(ApizrException<TModelData>)} might be handled by callback with InnerException and cached result");

                    foreach (var exceptionHandler in requestOptionsBuilder.ApizrOptions.ExceptionHandlers)
                    {
                        var exceptionHandled = await exceptionHandler.HandleAsync(ex).ConfigureAwait(false);
                        if (exceptionHandled)
                            ex.Handled = true;
                    }

                    if (!ex.Handled || requestOptionsBuilder.ApizrOptions.LetThrowOnHandledException)
                        throw ex;
                }
                finally
                {
                    if (resilienceContext != null &&
                        requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ReturnToPoolOnComplete != false)
                        ResilienceContextPool.Shared.Return(resilienceContext);
                }

                if (ex == null && 
                    result != null && 
                    _cacheHandler != null &&
                    cacheAttribute != null &&
                    !string.IsNullOrWhiteSpace(cacheAttribute.CacheKey) &&
                    cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheAttribute.CacheKey, result, cacheAttribute.LifeSpan,
                        requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                }
            }

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Returning mapped result from {typeof(TApiData).Name} to {typeof(TModelData).Name}");

            return Map<TApiData, TModelData>(result);
        }

        /// <inheritdoc />
        public virtual Task<IApizrResponse<TModelData>> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<ApiResponse<TApiData>>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => ExecuteAsync<TModelData, TApiData>(
                (opt, api) => executeApiMethod.Compile().Invoke(opt, api)
                    .ContinueWith(task => (IApiResponse<TApiData>) task.Result),
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public virtual async Task<IApizrResponse<TModelData>> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<IApiResponse<TApiData>>>> executeApiMethod, 
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        {
            var webApi = _lazyWebApi.Value;
            var requestOnlyOptionsBuilder = CreateRequestOptionsBuilder(optionsBuilder);
            var originalExpression = requestOnlyOptionsBuilder.ApizrOptions.OriginalExpression ?? executeApiMethod;
            var methodDetails = GetMethodDetails<TApiData>(originalExpression);
            var requestRedactHeaders = GetRequestHeadersAttributeRedactFactory(methodDetails);
            var requestLogAttribute = GetRequestLogAttribute(methodDetails);
            var requestHandlerParameterAttributes = GetRequestHandlerParameterAttributes(methodDetails);
            var operationTimeoutAttribute = GetOperationTimeoutAttribute(methodDetails);
            var requestTimeoutAttribute = GetRequestTimeoutAttribute(methodDetails);
            var resiliencePipelineAttribute = GetMethodResiliencePipelineAttribute(methodDetails);
            var requestCacheAttribute = GetMethodCacheAttribute(methodDetails);
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, methodDetails.MethodInfo.Name, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute,
                resiliencePipelineAttribute, requestCacheAttribute);

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiData cachedResult = default;
            TApiData apiResult = default;
            ApizrResponse<TModelData> response = default;

            if (ShouldCache<TApiData>(methodDetails, originalExpression, requestOptionsBuilder.ApizrOptions, out var cacheAttribute))
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheAttribute.CacheKey}");

                cachedResult = await _cacheHandler.GetAsync<TApiData>(cacheAttribute.CacheKey,
                    requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                if (!Equals(cachedResult, default))
                {
                    if (requestOptionsBuilder.ApizrOptions.ClearCache)
                    {
                        await _cacheHandler.RemoveAsync(cacheAttribute.CacheKey, requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                        cachedResult = default;
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Cached data cleared for this cache key");
                    }
                    else
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");

                        if (cacheAttribute.Mode == CacheMode.SetByHeader)
                        {
                            var finalCacheMode = await _cacheHandler.GetAsync<CacheMode>(
                                cacheAttribute.FinalModeCacheKey,
                                requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                            if (finalCacheMode != CacheMode.None)
                            {
                                cacheAttribute.FinalMode = finalCacheMode;
                                if (finalCacheMode == CacheMode.FetchOrGet)
                                {
                                    var cacheHeaders = await _cacheHandler.GetAsync<string[]>(
                                        cacheAttribute.HeadersCacheKey,
                                        requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                                    if (cacheHeaders?.Length > 0)
                                        cacheAttribute.Headers = cacheHeaders;
                                }
                            }
                        }
                    }
                }
            }

            if (!Equals(cachedResult, default) && cacheAttribute?.FinalMode == CacheMode.GetOrFetch)
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Using cached data for the response");

                response = new ApizrResponse<TModelData>(Map<TApiData, TModelData>(cachedResult), ApizrResponseDataSource.Cache);
            }
            else
            {
                ResilienceContext resilienceContext = null;
                try
                {
                    requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }

                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var resiliencePipeline = GetMethodResiliencePipeline<IApiResponse<TApiData>>(methodDetails, requestOptionsBuilder.ApizrOptions);
                    resilienceContext = ResilienceContextPool.Shared.Get(methodDetails.MethodInfo.Name,
                        requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ContinueOnCapturedContext,
                        requestOptionsBuilder.ApizrOptions.CancellationToken);
                    if (requestOptionsBuilder.ApizrOptions is IApizrGlobalSharedOptionsBase internalOptions &&
                        internalOptions.ResiliencePropertiesFactories.Any())
                        resilienceContext.Properties.SetProperties(
                            internalOptions.ResiliencePropertiesFactories.ToDictionary(kvp => kvp.Key,
                                kvp => kvp.Value()), out _);
                    resilienceContext.WithLogger(_apizrOptions.Logger, requestOptionsBuilder.ApizrOptions.LogLevels,
                        requestOptionsBuilder.ApizrOptions.TrafficVerbosity,
                        requestOptionsBuilder.ApizrOptions.HttpTracerMode);
                    requestOptionsBuilder.WithContext(resilienceContext);

                    if (requestRedactHeaders.Count > 0)
                        requestOptionsBuilder.WithLoggedHeadersRedactionNames(requestRedactHeaders);

                    if (requestOptionsBuilder.ApizrOptions.ShouldRedactHeaderValue != null)
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Applying some header redaction rules to the request");

                    requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Executing the request");

                    var apiResponse = await resiliencePipeline.ExecuteAsync(
                        async options => await executeApiMethod.Compile().Invoke(options, webApi).ConfigureAwait(resilienceContext.ContinueOnCapturedContext),
                        requestOptionsBuilder).ConfigureAwait(resilienceContext.ContinueOnCapturedContext);

                    apiResult = apiResponse.Content;

                    if (apiResponse.IsSuccessStatusCode && apiResponse.Error == null)
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Request succeed! Using request data for the response");

                        response = new ApizrResponse<TModelData>(apiResponse, Map<TApiData, TModelData>(apiResult),
                            ApizrResponseDataSource.Request);
                    }
                    else if (!Equals(cachedResult, default(TApiData)))
                    {
                        if (apiResponse.StatusCode == HttpStatusCode.NotModified && cacheAttribute is
                            { Mode: CacheMode.SetByHeader, FinalMode: CacheMode.FetchOrGet })
                        {
                            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                                $"{methodDetails.MethodInfo.Name}: No modified data! Using cached data for the response");

                            response = new ApizrResponse<TModelData>(apiResponse,
                                Map<TApiData, TModelData>(cachedResult),
                                ApizrResponseDataSource.Cache);
                        }
                        else
                        {
                            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                                $"{methodDetails.MethodInfo.Name}: Request failed! Using cached data for the response");

                            response = new ApizrResponse<TModelData>(apiResponse,
                                new ApizrException<TModelData>(apiResponse.Error,
                                    Map<TApiData, TModelData>(cachedResult)));
                        }
                    }
                    else
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Request failed! No cached data to use for the response");

                        response = new ApizrResponse<TModelData>(apiResponse,
                            new ApizrException<TModelData>(apiResponse.Error));
                    }
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                        $"{methodDetails.MethodInfo.Name}: Request threw an exception with message {e.Message}");

                    response = new ApizrResponse<TModelData>(new ApizrException<TModelData>(e, Map<TApiData, TModelData>(cachedResult)));
                }
                finally
                {
                    if (resilienceContext != null &&
                        requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ReturnToPoolOnComplete != false)
                        ResilienceContextPool.Shared.Return(resilienceContext);
                }

                if (response.Exception != null && requestOptionsBuilder.ApizrOptions.ExceptionHandlers.Count > 0)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        Equals(cachedResult, default(TApiData))
                            ? $"{methodDetails.MethodInfo.Name}: {nameof(ApizrException<TModelData>)} might be handled by callback with InnerException but no cached result"
                            : $"{methodDetails.MethodInfo.Name}: {nameof(ApizrException<TModelData>)} might be handled by callback with InnerException and cached result");

                    foreach (var exceptionHandler in requestOptionsBuilder.ApizrOptions.ExceptionHandlers)
                    {
                        var exceptionHandled = await exceptionHandler.HandleAsync(response.Exception).ConfigureAwait(false);
                        if (exceptionHandled)
                            response.Exception.Handled = true;
                    }
                }

                if (response.Exception == null &&
                    _cacheHandler != null &&
                    cacheAttribute != null &&
                    !string.IsNullOrWhiteSpace(cacheAttribute.CacheKey) &&
                    cacheAttribute.Mode != CacheMode.None &&
                    !Equals(apiResult, default(TApiData)))
                {
                    var cacheEntries = GetCacheEntries(response, apiResult, cacheAttribute, out var cacheLifeSpan);
                    if (cacheLifeSpan != TimeSpan.Zero)
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Caching result");

                        foreach (var cacheEntry in cacheEntries)
                        {
                            await _cacheHandler.SetAsync(cacheEntry.Key, cacheEntry.Value, cacheLifeSpan,
                                requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                        }
                    }
                }
            }

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Returning mapped result from {typeof(TApiData).Name} to {typeof(TModelData).Name}");

            return response;
        }

        /// <inheritdoc />
        public virtual Task<IApizrResponse> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<IApiResponse>>> executeApiMethod, TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => ExecuteAsync<TModelData, TApiData>(
                (_, api, apiData) => executeApiMethod.Compile().Invoke(api, apiData), modelData,
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public virtual Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, 
            TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => ExecuteAsync<TModelData, TApiData>(
                (_, api, apiData) => executeApiMethod.Compile().Invoke(api, apiData), modelData,
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public virtual Task<IApizrResponse<TModelData>> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<ApiResponse<TApiData>>>> executeApiMethod, 
            TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => ExecuteAsync<TModelData, TApiData>(
                (_, api, apiData) => executeApiMethod.Compile().Invoke(api, apiData)
                    .ContinueWith(task => (IApiResponse<TApiData>) task.Result), modelData,
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public virtual Task<IApizrResponse<TModelData>> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<IApiResponse<TApiData>>>> executeApiMethod, 
            TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => ExecuteAsync<TModelData, TApiData>(
                (_, api, apiData) => executeApiMethod.Compile().Invoke(api, apiData), modelData,
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public virtual async Task<IApizrResponse> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<IApiResponse>>> executeApiMethod, TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        {
            var webApi = _lazyWebApi.Value;
            var requestOnlyOptionsBuilder = CreateRequestOptionsBuilder(optionsBuilder);
            var originalExpression = requestOnlyOptionsBuilder.ApizrOptions.OriginalExpression ?? executeApiMethod;
            var methodDetails = GetMethodDetails<IApiResponse>(originalExpression);
            var requestRedactHeaders = GetRequestHeadersAttributeRedactFactory(methodDetails);
            var requestLogAttribute = GetRequestLogAttribute(methodDetails);
            var requestHandlerParameterAttributes = GetRequestHandlerParameterAttributes(methodDetails);
            var operationTimeoutAttribute = GetOperationTimeoutAttribute(methodDetails);
            var requestTimeoutAttribute = GetRequestTimeoutAttribute(methodDetails);
            var resiliencePipelineAttribute = GetMethodResiliencePipelineAttribute(methodDetails);
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, methodDetails.MethodInfo.Name, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute, resiliencePipelineAttribute);

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            ApizrResponse response = default;
            ResilienceContext resilienceContext = null;

            try
            {
                requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                if (!_connectivityHandler.IsConnected())
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                        $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                    throw new IOException("Connectivity check failed");
                }

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                var resiliencePipeline = GetMethodResiliencePipeline<IApiResponse>(methodDetails, requestOptionsBuilder.ApizrOptions);
                resilienceContext = ResilienceContextPool.Shared.Get(methodDetails.MethodInfo.Name,
                    requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ContinueOnCapturedContext,
                    requestOptionsBuilder.ApizrOptions.CancellationToken);
                if (requestOptionsBuilder.ApizrOptions is IApizrGlobalSharedOptionsBase internalOptions &&
                    internalOptions.ResiliencePropertiesFactories.Any())
                    resilienceContext.Properties.SetProperties(internalOptions.ResiliencePropertiesFactories.ToDictionary(kvp => kvp.Key, kvp => kvp.Value()), out _);
                resilienceContext.WithLogger(_apizrOptions.Logger, requestOptionsBuilder.ApizrOptions.LogLevels,
                    requestOptionsBuilder.ApizrOptions.TrafficVerbosity,
                    requestOptionsBuilder.ApizrOptions.HttpTracerMode);
                requestOptionsBuilder.WithContext(resilienceContext);

                if (requestRedactHeaders.Count > 0)
                    requestOptionsBuilder.WithLoggedHeadersRedactionNames(requestRedactHeaders);

                if (requestOptionsBuilder.ApizrOptions.ShouldRedactHeaderValue != null)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Applying some header redaction rules to the request");

                var apiData = Map<TModelData, TApiData>(modelData);
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: {typeof(TModelData).Name} mapped to {typeof(TApiData).Name}");

                requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Executing the request");

                var apiResponse = await resiliencePipeline.ExecuteAsync(
                    async options => await executeApiMethod.Compile().Invoke(options, webApi, apiData).ConfigureAwait(resilienceContext.ContinueOnCapturedContext),
                    requestOptionsBuilder).ConfigureAwait(resilienceContext.ContinueOnCapturedContext);

                if (apiResponse.IsSuccessStatusCode && apiResponse.Error == null)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Request succeed!");
                else
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Request failed!");

                response = apiResponse.Error == null ?
                    new ApizrResponse(apiResponse) :
                    new ApizrResponse(apiResponse, new ApizrException(apiResponse.Error));
            }
            catch (Exception e)
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                    $"{methodDetails.MethodInfo.Name}: Request threw an exception with message {e.Message}");

                response = new ApizrResponse(new ApizrException(e));
            }
            finally
            {
                if (resilienceContext != null &&
                    requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ReturnToPoolOnComplete != false)
                    ResilienceContextPool.Shared.Return(resilienceContext);
            }

            if (response.Exception != null && requestOptionsBuilder.ApizrOptions.ExceptionHandlers.Count > 0)
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: {nameof(ApizrException)} might be handled by callback");

                foreach (var exceptionHandler in requestOptionsBuilder.ApizrOptions.ExceptionHandlers)
                {
                    var exceptionHandled = await exceptionHandler.HandleAsync(response.Exception).ConfigureAwait(false);
                    if (exceptionHandled)
                        response.Exception.Handled = true;
                }
            }

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Returning mapped result from {typeof(TApiData).Name} to {typeof(TModelData).Name}");

            return response;
        }

        /// <inheritdoc />
        public virtual async Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        {
            var webApi = _lazyWebApi.Value;
            var requestOnlyOptionsBuilder = CreateRequestOptionsBuilder(optionsBuilder);
            var originalExpression = requestOnlyOptionsBuilder.ApizrOptions.OriginalExpression ?? executeApiMethod;
            var methodDetails = GetMethodDetails<TApiData>(originalExpression);
            var requestRedactHeaders = GetRequestHeadersAttributeRedactFactory(methodDetails);
            var requestLogAttribute = GetRequestLogAttribute(methodDetails);
            var requestHandlerParameterAttributes = GetRequestHandlerParameterAttributes(methodDetails);
            var operationTimeoutAttribute = GetOperationTimeoutAttribute(methodDetails);
            var requestTimeoutAttribute = GetRequestTimeoutAttribute(methodDetails);
            var resiliencePipelineAttribute = GetMethodResiliencePipelineAttribute(methodDetails);
            var requestCacheAttribute = GetMethodCacheAttribute(methodDetails);
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, methodDetails.MethodInfo.Name, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute,
                resiliencePipelineAttribute, requestCacheAttribute);

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiData result = default;

            if (ShouldCache<TApiData>(methodDetails, originalExpression, requestOptionsBuilder.ApizrOptions, out var cacheAttribute))
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheAttribute.CacheKey}");

                result = await _cacheHandler.GetAsync<TApiData>(cacheAttribute.CacheKey,
                    requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                if (!Equals(result, default))
                {
                    if (requestOptionsBuilder.ApizrOptions.ClearCache)
                    {
                        await _cacheHandler.RemoveAsync(cacheAttribute.CacheKey, requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                        result = default;
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Cached data cleared for this cache key");
                    }
                    else
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");
                    }
                }
            }

            if (Equals(result, default) || cacheAttribute?.FinalMode != CacheMode.GetOrFetch)
            {
                ResilienceContext resilienceContext = null;
                ApizrException<TApiData> ex = null;
                try
                {
                    requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }
                    else
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var resiliencePipeline = GetMethodResiliencePipeline<TApiData>(methodDetails, requestOptionsBuilder.ApizrOptions);
                    resilienceContext = ResilienceContextPool.Shared.Get(methodDetails.MethodInfo.Name,
                        requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ContinueOnCapturedContext,
                        requestOptionsBuilder.ApizrOptions.CancellationToken);
                    if (requestOptionsBuilder.ApizrOptions is IApizrGlobalSharedOptionsBase internalOptions &&
                        internalOptions.ResiliencePropertiesFactories.Any())
                        resilienceContext.Properties.SetProperties(internalOptions.ResiliencePropertiesFactories.ToDictionary(kvp => kvp.Key, kvp => kvp.Value()), out _);
                    resilienceContext.WithLogger(_apizrOptions.Logger, requestOptionsBuilder.ApizrOptions.LogLevels,
                        requestOptionsBuilder.ApizrOptions.TrafficVerbosity,
                        requestOptionsBuilder.ApizrOptions.HttpTracerMode);
                    requestOptionsBuilder.WithContext(resilienceContext);

                    if (requestRedactHeaders.Count > 0)
                        requestOptionsBuilder.WithLoggedHeadersRedactionNames(requestRedactHeaders);

                    if (requestOptionsBuilder.ApizrOptions.ShouldRedactHeaderValue != null)
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Applying some header redaction rules to the request");

                    var apiData = Map<TModelData, TApiData>(modelData);
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: {typeof(TModelData).Name} mapped to {typeof(TApiData).Name}");

                    requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Executing the request");

                    result = await resiliencePipeline.ExecuteAsync(
                        async options => await executeApiMethod.Compile().Invoke(options, webApi, apiData).ConfigureAwait(resilienceContext.ContinueOnCapturedContext),
                        requestOptionsBuilder).ConfigureAwait(resilienceContext.ContinueOnCapturedContext);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                        $"{methodDetails.MethodInfo.Name}: Request threw an exception with message {e.Message}");

                    ex = new ApizrException<TApiData>(e, result);
                    if (requestOptionsBuilder.ApizrOptions.ExceptionHandlers.Count == 0)
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                            Equals(result, default(TApiData))
                                ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TApiData>)} with InnerException but no cached result"
                                : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TApiData>)} with InnerException and cached result");

                        throw ex;
                    }

                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        Equals(result, default(TApiData))
                            ? $"{methodDetails.MethodInfo.Name}: {nameof(ApizrException<TApiData>)} might be handled by callback with InnerException but no cached result"
                            : $"{methodDetails.MethodInfo.Name}: {nameof(ApizrException<TApiData>)} might be handled by callback with InnerException and cached result");

                    foreach (var exceptionHandler in requestOptionsBuilder.ApizrOptions.ExceptionHandlers)
                    {
                        var exceptionHandled = await exceptionHandler.HandleAsync(ex).ConfigureAwait(false);
                        if (exceptionHandled)
                            ex.Handled = true;
                    }

                    if (!ex.Handled || requestOptionsBuilder.ApizrOptions.LetThrowOnHandledException)
                        throw ex;
                }
                finally
                {
                    if (resilienceContext != null &&
                        requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ReturnToPoolOnComplete != false)
                        ResilienceContextPool.Shared.Return(resilienceContext);
                }

                if (ex == null && 
                    result != null && 
                    _cacheHandler != null &&
                    cacheAttribute != null &&
                    !string.IsNullOrWhiteSpace(cacheAttribute.CacheKey) &&
                    cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheAttribute.CacheKey, result, cacheAttribute.LifeSpan,
                        requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                }
            }

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Returning mapped result from {typeof(TApiData).Name} to {typeof(TModelData).Name}");

            return Map<TApiData, TModelData>(result);
        }

        /// <inheritdoc />
        public virtual Task<IApizrResponse<TModelData>> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<ApiResponse<TApiData>>>> executeApiMethod,
            TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => ExecuteAsync<TModelData, TApiData>(
                (opt, api, apiData) => executeApiMethod.Compile().Invoke(opt, api, apiData)
                    .ContinueWith(task => (IApiResponse<TApiData>) task.Result), modelData,
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public virtual async Task<IApizrResponse<TModelData>> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiData, Task<IApiResponse<TApiData>>>> executeApiMethod, 
            TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        {
            var webApi = _lazyWebApi.Value;
            var requestOnlyOptionsBuilder = CreateRequestOptionsBuilder(optionsBuilder);
            var originalExpression = requestOnlyOptionsBuilder.ApizrOptions.OriginalExpression ?? executeApiMethod;
            var methodDetails = GetMethodDetails<TApiData>(originalExpression);
            var requestRedactHeaders = GetRequestHeadersAttributeRedactFactory(methodDetails);
            var requestLogAttribute = GetRequestLogAttribute(methodDetails);
            var requestHandlerParameterAttributes = GetRequestHandlerParameterAttributes(methodDetails);
            var operationTimeoutAttribute = GetOperationTimeoutAttribute(methodDetails);
            var requestTimeoutAttribute = GetRequestTimeoutAttribute(methodDetails);
            var resiliencePipelineAttribute = GetMethodResiliencePipelineAttribute(methodDetails);
            var requestCacheAttribute = GetMethodCacheAttribute(methodDetails);
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, methodDetails.MethodInfo.Name, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute,
                resiliencePipelineAttribute, requestCacheAttribute);

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiData cachedResult = default;
            TApiData apiResult = default;
            ApizrResponse<TModelData> response = default;

            if (ShouldCache<TApiData>(methodDetails, originalExpression, requestOptionsBuilder.ApizrOptions, out var cacheAttribute))
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheAttribute.CacheKey}");

                cachedResult = await _cacheHandler.GetAsync<TApiData>(cacheAttribute.CacheKey,
                    requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                if (!Equals(cachedResult, default))
                {
                    if (requestOptionsBuilder.ApizrOptions.ClearCache)
                    {
                        await _cacheHandler.RemoveAsync(cacheAttribute.CacheKey, requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                        cachedResult = default;
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Cached data cleared for this cache key");
                    }
                    else
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");

                        if (cacheAttribute.Mode == CacheMode.SetByHeader)
                        {
                            var finalCacheMode = await _cacheHandler.GetAsync<CacheMode>(
                                cacheAttribute.FinalModeCacheKey,
                                requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                            if (finalCacheMode != CacheMode.None)
                            {
                                cacheAttribute.FinalMode = finalCacheMode;
                                if (finalCacheMode == CacheMode.FetchOrGet)
                                {
                                    var cacheHeaders = await _cacheHandler.GetAsync<string[]>(
                                        cacheAttribute.HeadersCacheKey,
                                        requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                                    if (cacheHeaders?.Length > 0)
                                        cacheAttribute.Headers = cacheHeaders;
                                }
                            }
                        }
                    }
                }
            }

            if (!Equals(cachedResult, default) && cacheAttribute?.FinalMode == CacheMode.GetOrFetch)
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Using cached data for the response");

                response = new ApizrResponse<TModelData>(Map<TApiData, TModelData>(cachedResult), ApizrResponseDataSource.Cache);
            }
            else
            {
                ResilienceContext resilienceContext = null;
                try
                {
                    requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }
                    else
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var resiliencePipeline = GetMethodResiliencePipeline<IApiResponse<TApiData>>(methodDetails, requestOptionsBuilder.ApizrOptions);
                    resilienceContext = ResilienceContextPool.Shared.Get(methodDetails.MethodInfo.Name,
                        requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ContinueOnCapturedContext,
                        requestOptionsBuilder.ApizrOptions.CancellationToken);
                    if (requestOptionsBuilder.ApizrOptions is IApizrGlobalSharedOptionsBase internalOptions &&
                        internalOptions.ResiliencePropertiesFactories.Any())
                        resilienceContext.Properties.SetProperties(
                            internalOptions.ResiliencePropertiesFactories.ToDictionary(kvp => kvp.Key,
                                kvp => kvp.Value()), out _);
                    resilienceContext.WithLogger(_apizrOptions.Logger, requestOptionsBuilder.ApizrOptions.LogLevels,
                        requestOptionsBuilder.ApizrOptions.TrafficVerbosity,
                        requestOptionsBuilder.ApizrOptions.HttpTracerMode);
                    requestOptionsBuilder.WithContext(resilienceContext);

                    if (requestRedactHeaders.Count > 0)
                        requestOptionsBuilder.WithLoggedHeadersRedactionNames(requestRedactHeaders);

                    if (requestOptionsBuilder.ApizrOptions.ShouldRedactHeaderValue != null)
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Applying some header redaction rules to the request");

                    var apiData = Map<TModelData, TApiData>(modelData);
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: {typeof(TModelData).Name} mapped to {typeof(TApiData).Name}");

                    requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Executing the request");

                    var apiResponse = await resiliencePipeline.ExecuteAsync(
                        async options => await executeApiMethod.Compile().Invoke(options, webApi, apiData).ConfigureAwait(resilienceContext.ContinueOnCapturedContext),
                        requestOptionsBuilder).ConfigureAwait(resilienceContext.ContinueOnCapturedContext);

                    apiResult = apiResponse.Content;

                    if (apiResponse.IsSuccessStatusCode && apiResponse.Error == null)
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Request succeed! Using request data for the response");

                        response = new ApizrResponse<TModelData>(apiResponse, Map<TApiData, TModelData>(apiResult),
                            ApizrResponseDataSource.Request);
                    }
                    else if (!Equals(cachedResult, default(TApiData)))
                    {
                        if (apiResponse.StatusCode == HttpStatusCode.NotModified && cacheAttribute is
                            { Mode: CacheMode.SetByHeader, FinalMode: CacheMode.FetchOrGet })
                        {
                            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                                $"{methodDetails.MethodInfo.Name}: No modified data! Using cached data for the response");

                            response = new ApizrResponse<TModelData>(apiResponse,
                                Map<TApiData, TModelData>(cachedResult),
                                ApizrResponseDataSource.Cache);
                        }
                        else
                        {
                            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                                $"{methodDetails.MethodInfo.Name}: Request failed! Using cached data for the response");

                            response = new ApizrResponse<TModelData>(apiResponse,
                                new ApizrException<TModelData>(apiResponse.Error,
                                    Map<TApiData, TModelData>(cachedResult)));
                        }
                    }
                    else
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Request failed! No cached data to use for the response");

                        response = new ApizrResponse<TModelData>(apiResponse,
                            new ApizrException<TModelData>(apiResponse.Error));
                    }
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                        $"{methodDetails.MethodInfo.Name}: Request threw an exception with message {e.Message}");

                    response = new ApizrResponse<TModelData>(new ApizrException<TModelData>(e, Map<TApiData, TModelData>(cachedResult)));
                }
                finally
                {
                    if (resilienceContext != null &&
                        requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ReturnToPoolOnComplete != false)
                        ResilienceContextPool.Shared.Return(resilienceContext);
                }

                if (response.Exception != null && requestOptionsBuilder.ApizrOptions.ExceptionHandlers.Count > 0)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        Equals(cachedResult, default(TApiData))
                            ? $"{methodDetails.MethodInfo.Name}: {nameof(ApizrException<TModelData>)} might be handled by callback with InnerException but no cached result"
                            : $"{methodDetails.MethodInfo.Name}: {nameof(ApizrException<TModelData>)} might be handled by callback with InnerException and cached result");

                    foreach (var exceptionHandler in requestOptionsBuilder.ApizrOptions.ExceptionHandlers)
                    {
                        var exceptionHandled = await exceptionHandler.HandleAsync(response.Exception).ConfigureAwait(false);
                        if (exceptionHandled)
                            response.Exception.Handled = true;
                    }
                }

                if (response.Exception == null && 
                    _cacheHandler != null &&
                    cacheAttribute != null &&
                    !string.IsNullOrWhiteSpace(cacheAttribute.CacheKey) &&
                    cacheAttribute.Mode != CacheMode.None &&
                    !Equals(apiResult, default(TApiData)))
                {
                    var cacheEntries = GetCacheEntries(response, apiResult, cacheAttribute, out var cacheLifeSpan);
                    if (cacheLifeSpan != TimeSpan.Zero)
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Caching result");

                        foreach (var cacheEntry in cacheEntries)
                        {
                            await _cacheHandler.SetAsync(cacheEntry.Key, cacheEntry.Value, cacheLifeSpan,
                                requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                        }
                    }
                }
            }

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Returning mapped result from {typeof(TApiData).Name} to {typeof(TModelData).Name}");

            return response;
        }

        /// <inheritdoc />
        public virtual Task<TModelResultData> ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (_, api, apiData) => executeApiMethod.Compile().Invoke(api, apiData), modelRequestData,
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public virtual Task<IApizrResponse<TModelResultData>> ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<ApiResponse<TApiResultData>>>> executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (_, api, apiData) => executeApiMethod.Compile().Invoke(api, apiData)
                    .ContinueWith(task => (IApiResponse<TApiResultData>) task.Result), modelRequestData,
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public virtual Task<IApizrResponse<TModelResultData>> ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<IApiResponse<TApiResultData>>>> executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (_, api, apiData) => executeApiMethod.Compile().Invoke(api, apiData), modelRequestData,
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public virtual async Task<TModelResultData> ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        {
            var webApi = _lazyWebApi.Value;
            var requestOnlyOptionsBuilder = CreateRequestOptionsBuilder(optionsBuilder);
            var originalExpression = requestOnlyOptionsBuilder.ApizrOptions.OriginalExpression ?? executeApiMethod;
            var methodDetails = GetMethodDetails<TApiResultData>(originalExpression);
            var requestRedactHeaders = GetRequestHeadersAttributeRedactFactory(methodDetails);
            var requestLogAttribute = GetRequestLogAttribute(methodDetails);
            var requestHandlerParameterAttributes = GetRequestHandlerParameterAttributes(methodDetails);
            var operationTimeoutAttribute = GetOperationTimeoutAttribute(methodDetails);
            var requestTimeoutAttribute = GetRequestTimeoutAttribute(methodDetails);
            var resiliencePipelineAttribute = GetMethodResiliencePipelineAttribute(methodDetails);
            var requestCacheAttribute = GetMethodCacheAttribute(methodDetails);
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, methodDetails.MethodInfo.Name, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute,
                resiliencePipelineAttribute, requestCacheAttribute);

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiResultData result = default;

            if (ShouldCache<TApiResultData>(methodDetails, originalExpression, requestOptionsBuilder.ApizrOptions, out var cacheAttribute))
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheAttribute.CacheKey}");

                result = await _cacheHandler.GetAsync<TApiResultData>(cacheAttribute.CacheKey,
                    requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                if (!Equals(result, default))
                {
                    if (requestOptionsBuilder.ApizrOptions.ClearCache)
                    {
                        await _cacheHandler.RemoveAsync(cacheAttribute.CacheKey, requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                        result = default;
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Cached data cleared for this cache key");
                    }
                    else
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");
                    }
                }
            }

            if (Equals(result, default) || cacheAttribute?.FinalMode != CacheMode.GetOrFetch)
            {
                ResilienceContext resilienceContext = null;
                ApizrException<TModelResultData> ex = null;
                try
                {
                    requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }

                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var resiliencePipeline = GetMethodResiliencePipeline<TApiResultData>(methodDetails, requestOptionsBuilder.ApizrOptions);
                    resilienceContext = ResilienceContextPool.Shared.Get(methodDetails.MethodInfo.Name,
                        requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ContinueOnCapturedContext,
                        requestOptionsBuilder.ApizrOptions.CancellationToken);
                    if (requestOptionsBuilder.ApizrOptions is IApizrGlobalSharedOptionsBase internalOptions &&
                        internalOptions.ResiliencePropertiesFactories.Any())
                        resilienceContext.Properties.SetProperties(internalOptions.ResiliencePropertiesFactories.ToDictionary(kvp => kvp.Key, kvp => kvp.Value()), out _);
                    resilienceContext.WithLogger(_apizrOptions.Logger, requestOptionsBuilder.ApizrOptions.LogLevels,
                        requestOptionsBuilder.ApizrOptions.TrafficVerbosity,
                        requestOptionsBuilder.ApizrOptions.HttpTracerMode);
                    requestOptionsBuilder.WithContext(resilienceContext);

                    if (requestRedactHeaders.Count > 0)
                        requestOptionsBuilder.WithLoggedHeadersRedactionNames(requestRedactHeaders);

                    if (requestOptionsBuilder.ApizrOptions.ShouldRedactHeaderValue != null)
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Applying some header redaction rules to the request");

                    var apiRequestData = Map<TModelRequestData, TApiRequestData>(modelRequestData);
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: {typeof(TModelRequestData).Name} mapped to {typeof(TApiRequestData).Name}");

                    requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Executing the request");

                    result = await resiliencePipeline.ExecuteAsync(
                        async options => await executeApiMethod.Compile().Invoke(options, webApi, apiRequestData).ConfigureAwait(resilienceContext.ContinueOnCapturedContext),
                        requestOptionsBuilder).ConfigureAwait(resilienceContext.ContinueOnCapturedContext);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                        $"{methodDetails.MethodInfo.Name}: Request threw an exception with message {e.Message}");

                    ex = new ApizrException<TModelResultData>(e, Map<TApiResultData, TModelResultData>(result));
                    if (requestOptionsBuilder.ApizrOptions.ExceptionHandlers.Count == 0)
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                            Equals(result, default(TApiResultData))
                                ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelResultData>)} with InnerException but no cached result"
                                : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelResultData>)} with InnerException and cached result");

                        throw ex;
                    }

                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        Equals(result, default(TApiResultData))
                            ? $"{methodDetails.MethodInfo.Name}: {nameof(ApizrException<TModelResultData>)} might be handled by callback with InnerException but no cached result"
                            : $"{methodDetails.MethodInfo.Name}: {nameof(ApizrException<TModelResultData>)} might be handled by callback with InnerException and cached result");

                    foreach (var exceptionHandler in requestOptionsBuilder.ApizrOptions.ExceptionHandlers)
                    {
                        var exceptionHandled = await exceptionHandler.HandleAsync(ex).ConfigureAwait(false);
                        if (exceptionHandled)
                            ex.Handled = true;
                    }

                    if (!ex.Handled || requestOptionsBuilder.ApizrOptions.LetThrowOnHandledException)
                        throw ex;
                }
                finally
                {
                    if (resilienceContext != null &&
                        requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ReturnToPoolOnComplete != false)
                        ResilienceContextPool.Shared.Return(resilienceContext);
                }

                if (ex == null && 
                    result != null && 
                    _cacheHandler != null &&
                    cacheAttribute != null &&
                    !string.IsNullOrWhiteSpace(cacheAttribute.CacheKey) &&
                    cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheAttribute.CacheKey, result, cacheAttribute.LifeSpan,
                        requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                }
            }

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Returning mapped result from {typeof(TApiResultData).Name} to {typeof(TModelResultData).Name}");

            return Map<TApiResultData, TModelResultData>(result);
        }

        /// <inheritdoc />
        public virtual Task<IApizrResponse<TModelResultData>> ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiRequestData, Task<ApiResponse<TApiResultData>>>> executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
                (opt, api, apiData) => executeApiMethod.Compile().Invoke(opt, api, apiData)
                    .ContinueWith(task => (IApiResponse<TApiResultData>)task.Result), modelRequestData,
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public virtual async Task<IApizrResponse<TModelResultData>> ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData, TModelRequestData>(
            Expression<Func<IApizrRequestOptions, TWebApi, TApiRequestData, Task<IApiResponse<TApiResultData>>>> executeApiMethod,
            TModelRequestData modelRequestData, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        {
            var webApi = _lazyWebApi.Value;
            var requestOnlyOptionsBuilder = CreateRequestOptionsBuilder(optionsBuilder);
            var originalExpression = requestOnlyOptionsBuilder.ApizrOptions.OriginalExpression ?? executeApiMethod;
            var methodDetails = GetMethodDetails<TApiResultData>(originalExpression);
            var requestRedactHeaders = GetRequestHeadersAttributeRedactFactory(methodDetails);
            var requestLogAttribute = GetRequestLogAttribute(methodDetails);
            var requestHandlerParameterAttributes = GetRequestHandlerParameterAttributes(methodDetails);
            var operationTimeoutAttribute = GetOperationTimeoutAttribute(methodDetails);
            var requestTimeoutAttribute = GetRequestTimeoutAttribute(methodDetails);
            var resiliencePipelineAttribute = GetMethodResiliencePipelineAttribute(methodDetails);
            var requestCacheAttribute = GetMethodCacheAttribute(methodDetails);
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, methodDetails.MethodInfo.Name, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute,
                resiliencePipelineAttribute, requestCacheAttribute);

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiResultData cachedResult = default;
            TApiResultData apiResult = default;
            ApizrResponse<TModelResultData> response = default;

            if (ShouldCache<TApiResultData>(methodDetails, originalExpression, requestOptionsBuilder.ApizrOptions, out var cacheAttribute))
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheAttribute.CacheKey}");

                cachedResult = await _cacheHandler.GetAsync<TApiResultData>(cacheAttribute.CacheKey,
                    requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                if (!Equals(cachedResult, default))
                {
                    if (requestOptionsBuilder.ApizrOptions.ClearCache)
                    {
                        await _cacheHandler.RemoveAsync(cacheAttribute.CacheKey, requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                        cachedResult = default;
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Cached data cleared for this cache key");
                    }
                    else
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");

                        if (cacheAttribute.Mode == CacheMode.SetByHeader)
                        {
                            var finalCacheMode = await _cacheHandler.GetAsync<CacheMode>(
                                cacheAttribute.FinalModeCacheKey,
                                requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                            if (finalCacheMode != CacheMode.None)
                            {
                                cacheAttribute.FinalMode = finalCacheMode;
                                if (finalCacheMode == CacheMode.FetchOrGet)
                                {
                                    var cacheHeaders = await _cacheHandler.GetAsync<string[]>(
                                        cacheAttribute.HeadersCacheKey,
                                        requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                                    if (cacheHeaders?.Length > 0)
                                        cacheAttribute.Headers = cacheHeaders;
                                }
                            }
                        }
                    }
                }
            }

            if (!Equals(cachedResult, default) && cacheAttribute?.FinalMode == CacheMode.GetOrFetch)
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Using cached data for the response");

                response = new ApizrResponse<TModelResultData>(Map<TApiResultData, TModelResultData>(cachedResult), ApizrResponseDataSource.Cache);
            }
            else
            {
                ResilienceContext resilienceContext = null;
                try
                {
                    requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }

                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var resiliencePipeline = GetMethodResiliencePipeline<IApiResponse<TApiResultData>>(methodDetails, requestOptionsBuilder.ApizrOptions);
                    resilienceContext = ResilienceContextPool.Shared.Get(methodDetails.MethodInfo.Name,
                        requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ContinueOnCapturedContext,
                        requestOptionsBuilder.ApizrOptions.CancellationToken);
                    if (requestOptionsBuilder.ApizrOptions is IApizrGlobalSharedOptionsBase internalOptions &&
                        internalOptions.ResiliencePropertiesFactories.Any())
                        resilienceContext.Properties.SetProperties(
                            internalOptions.ResiliencePropertiesFactories.ToDictionary(kvp => kvp.Key,
                                kvp => kvp.Value()), out _);
                    resilienceContext.WithLogger(_apizrOptions.Logger, requestOptionsBuilder.ApizrOptions.LogLevels,
                        requestOptionsBuilder.ApizrOptions.TrafficVerbosity,
                        requestOptionsBuilder.ApizrOptions.HttpTracerMode);
                    requestOptionsBuilder.WithContext(resilienceContext);

                    if (requestRedactHeaders.Count > 0)
                        requestOptionsBuilder.WithLoggedHeadersRedactionNames(requestRedactHeaders);

                    if (requestOptionsBuilder.ApizrOptions.ShouldRedactHeaderValue != null)
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Applying some header redaction rules to the request");

                    var apiRequestData = Map<TModelRequestData, TApiRequestData>(modelRequestData);
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: {typeof(TModelRequestData).Name} mapped to {typeof(TApiRequestData).Name}");

                    requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Executing the request");

                    var apiResponse = await resiliencePipeline.ExecuteAsync(
                        async options => await executeApiMethod.Compile().Invoke(options, webApi, apiRequestData)
                            .ConfigureAwait(resilienceContext.ContinueOnCapturedContext),
                        requestOptionsBuilder).ConfigureAwait(resilienceContext.ContinueOnCapturedContext);

                    apiResult = apiResponse.Content;

                    if (apiResponse.IsSuccessStatusCode && apiResponse.Error == null)
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Request succeed! Using request data for the response");

                        response = new ApizrResponse<TModelResultData>(apiResponse, Map<TApiResultData, TModelResultData>(apiResult),
                            ApizrResponseDataSource.Request);
                    }
                    else if (!Equals(cachedResult, default(TApiResultData)))
                    {
                        if (apiResponse.StatusCode == HttpStatusCode.NotModified && cacheAttribute is
                                { Mode: CacheMode.SetByHeader, FinalMode: CacheMode.FetchOrGet })
                        {
                            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                                $"{methodDetails.MethodInfo.Name}: No modified data! Using cached data for the response");

                            response = new ApizrResponse<TModelResultData>(apiResponse,
                                Map<TApiResultData, TModelResultData>(cachedResult),
                                ApizrResponseDataSource.Cache);
                        }
                        else
                        {
                            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                                $"{methodDetails.MethodInfo.Name}: Request failed! Using cached data for the response");

                            response = new ApizrResponse<TModelResultData>(apiResponse,
                                new ApizrException<TModelResultData>(apiResponse.Error,
                                    Map<TApiResultData, TModelResultData>(cachedResult)));
                        }
                    }
                    else
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Request failed! No cached data to use for the response");

                        response = new ApizrResponse<TModelResultData>(apiResponse,
                            new ApizrException<TModelResultData>(apiResponse.Error));
                    }
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                        $"{methodDetails.MethodInfo.Name}: Request threw an exception with message {e.Message}");

                    response = new ApizrResponse<TModelResultData>(new ApizrException<TModelResultData>(e, Map<TApiResultData, TModelResultData>(cachedResult)));
                }
                finally
                {
                    if (resilienceContext != null &&
                        requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ReturnToPoolOnComplete != false)
                        ResilienceContextPool.Shared.Return(resilienceContext);
                }

                if (response.Exception != null && requestOptionsBuilder.ApizrOptions.ExceptionHandlers.Count > 0)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        Equals(cachedResult, default(TApiResultData))
                            ? $"{methodDetails.MethodInfo.Name}: {nameof(ApizrException<TModelResultData>)} might be handled by callback with InnerException but no cached result"
                            : $"{methodDetails.MethodInfo.Name}: {nameof(ApizrException<TModelResultData>)} might be handled by callback with InnerException and cached result");

                    foreach (var exceptionHandler in requestOptionsBuilder.ApizrOptions.ExceptionHandlers)
                    {
                        var exceptionHandled = await exceptionHandler.HandleAsync(response.Exception).ConfigureAwait(false);
                        if (exceptionHandled)
                            response.Exception.Handled = true;
                    }
                }

                if (response.Exception == null &&
                    _cacheHandler != null &&
                    cacheAttribute != null &&
                    !string.IsNullOrWhiteSpace(cacheAttribute.CacheKey) &&
                    cacheAttribute.Mode != CacheMode.None &&
                    !Equals(apiResult, default(TApiResultData)))
                {
                    var cacheEntries = GetCacheEntries(response, apiResult, cacheAttribute, out var cacheLifeSpan);
                    if (cacheLifeSpan != TimeSpan.Zero)
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Caching result");

                        foreach (var cacheEntry in cacheEntries)
                        {
                            await _cacheHandler.SetAsync(cacheEntry.Key, cacheEntry.Value, cacheLifeSpan,
                                requestOptionsBuilder.ApizrOptions.CancellationToken).ConfigureAwait(false);
                        }
                    }
                }
            }

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Returning mapped result from {typeof(TApiResultData).Name} to {typeof(TModelResultData).Name}");

            return response;
        }

        #endregion

        #endregion

        #region ClearCacheAsync

        /// <inheritdoc />
        public async Task<bool> ClearCacheAsync(CancellationToken cancellationToken = default)
        {
            if (_cacheHandler is VoidCacheHandler)
                _apizrOptions.Logger.Log(_apizrOptions.LogLevels.Medium(),
                    $"Apizr: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

            try
            {
                await _cacheHandler.ClearAsync(cancellationToken).ConfigureAwait(false);
                _apizrOptions.Logger.Log(_apizrOptions.LogLevels.Low(), "Apizr: Cache cleared");

                return true;
            }
            catch (Exception e)
            {
                _apizrOptions.Logger.Log(_apizrOptions.LogLevels.High(),
                    $"Apizr: Clearing all cache threw an exception with message: {e.Message}");

                return false;
            }
        }

        /// <inheritdoc />
        public Task<bool> ClearCacheAsync<TResult>(Expression<Func<TWebApi, Task<TResult>>> executeApiMethod,
            CancellationToken cancellationToken = default)
            => ClearMethodCacheAsync<TResult>(executeApiMethod, CancellationToken.None);

        /// <inheritdoc />
        public Task<bool> ClearCacheAsync<TResult>(
            Expression<Func<TWebApi, Task<ApiResponse<TResult>>>> executeApiMethod,
            CancellationToken cancellationToken = default)
            => ClearMethodCacheAsync<TResult>(executeApiMethod, CancellationToken.None);

        /// <inheritdoc />
        public Task<bool> ClearCacheAsync<TResult>(
            Expression<Func<TWebApi, Task<IApiResponse<TResult>>>> executeApiMethod,
            CancellationToken cancellationToken = default)
            => ClearMethodCacheAsync<TResult>(executeApiMethod, CancellationToken.None);

        private async Task<bool> ClearMethodCacheAsync<TResult>(
            Expression executeApiMethod,
            CancellationToken cancellationToken = default)
        {
            var methodCallExpression = GetMethodCallExpression<TResult>(executeApiMethod);
            _apizrOptions.Logger.Log(_apizrOptions.LogLevels.Low(),
                $"Apizr: Calling cache clear for method {methodCallExpression.Method.Name}");

            if (_cacheHandler is VoidCacheHandler)
                _apizrOptions.Logger.Log(_apizrOptions.LogLevels.Medium(),
                    $"{methodCallExpression.Method.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

            try
            {
                var methodDetails = GetMethodDetails<TResult>(executeApiMethod);
                if (TryGetCacheKey<TResult>(methodDetails, executeApiMethod, out var cacheKey))
                {
                    _apizrOptions.Logger.Log(_apizrOptions.LogLevels.Low(),
                        $"{methodCallExpression.Method.Name}: Clearing cache for key {cacheKey}");

                    var success = await _cacheHandler.RemoveAsync(cacheKey, cancellationToken).ConfigureAwait(false);
                    if (success)
                        _apizrOptions.Logger.Log(_apizrOptions.LogLevels.Low(),
                            $"{methodCallExpression.Method.Name}: Clearing cache for key {cacheKey} succeed");
                    else
                        _apizrOptions.Logger.Log(_apizrOptions.LogLevels.High(),
                            $"{methodCallExpression.Method.Name}: Clearing cache for key {cacheKey} failed");

                    return success;
                }

                _apizrOptions.Logger.Log(_apizrOptions.LogLevels.Low(),
                    $"{methodCallExpression.Method.Name}: Method isn't cacheable");

                return true;
            }
            catch (Exception e)
            {
                _apizrOptions.Logger.Log(_apizrOptions.LogLevels.High(),
                    $"{methodCallExpression.Method.Name}: Clearing keyed cache threw an exception with message: {e.Message}");

                return false;
            }
        }

        #endregion

        #endregion

        #region Requesting

        private List<string> GetRequestHeadersAttributeRedactFactory(MethodDetails methodDetails)
        {
            if (_headersMethodsSet.TryGetValue(methodDetails, out var redactHeaders))
                return redactHeaders;

            HeadersAttribute headersAttribute;
            if (_apizrOptions.IsCrudApi)
            {
                // Crud api headers
                headersAttribute = methodDetails.RequestMethod switch // Request headers
                {
                    var method when method == ApizrRequestMethod.CrudReadAll || method == ApizrRequestMethod.CrudSafeReadAll =>
                        _apizrOptions.TypeInfo.GetCustomAttribute<ReadAllHeadersAttribute>(true),
                    var method when method == ApizrRequestMethod.CrudRead || method == ApizrRequestMethod.CrudSafeRead =>
                        _apizrOptions.TypeInfo.GetCustomAttribute<ReadHeadersAttribute>(true),
                    var method when method == ApizrRequestMethod.CrudCreate || method == ApizrRequestMethod.CrudSafeCreate =>
                        _apizrOptions.TypeInfo.GetCustomAttribute<CreateHeadersAttribute>(true),
                    var method when method == ApizrRequestMethod.CrudUpdate || method == ApizrRequestMethod.CrudSafeUpdate =>
                        _apizrOptions.TypeInfo.GetCustomAttribute<UpdateHeadersAttribute>(true),
                    var method when method == ApizrRequestMethod.CrudDelete || method == ApizrRequestMethod.CrudSafeDelete =>
                        _apizrOptions.TypeInfo.GetCustomAttribute<DeleteHeadersAttribute>(true),
                    _ => null
                };
            }
            else
            {
                // Classic api headers
                headersAttribute = methodDetails.MethodInfo.GetCustomAttribute<HeadersAttribute>(); // Request headers
            }

            // Redact headers
            redactHeaders = [];
            if (headersAttribute?.Headers.Length > 0)
            {
                foreach (var header in headersAttribute.Headers)
                    if (HttpRequestMessageExtensions.TryGetHeaderKeyValue(header, out var key, out var value) && value.StartsWith("*") && value.EndsWith("*"))
                        redactHeaders.Add(key);
            }

            // Return headers attribute
            _headersMethodsSet.TryAdd(methodDetails, redactHeaders);
            return redactHeaders;
        }

        #endregion

        #region Logging

        private LogAttributeBase GetRequestLogAttribute(MethodDetails methodDetails)
        {
            if (_loggingMethodsSet.TryGetValue(methodDetails, out var logAttribute))
                return logAttribute;

            if (_apizrOptions.IsCrudApi)
            {
                // Crud api logging
                logAttribute = methodDetails.RequestMethod switch // Request logging
                {
                    var method when method == ApizrRequestMethod.CrudReadAll || method == ApizrRequestMethod.CrudSafeReadAll =>
                        _apizrOptions.TypeInfo.GetCustomAttribute<LogReadAllAttribute>(true),
                    var method when method == ApizrRequestMethod.CrudRead || method == ApizrRequestMethod.CrudSafeRead =>
                        _apizrOptions.TypeInfo.GetCustomAttribute<LogReadAttribute>(true),
                    var method when method == ApizrRequestMethod.CrudCreate || method == ApizrRequestMethod.CrudSafeCreate =>
                        _apizrOptions.TypeInfo.GetCustomAttribute<LogCreateAttribute>(true),
                    var method when method == ApizrRequestMethod.CrudUpdate || method == ApizrRequestMethod.CrudSafeUpdate =>
                        _apizrOptions.TypeInfo.GetCustomAttribute<LogUpdateAttribute>(true),
                    var method when method == ApizrRequestMethod.CrudDelete || method == ApizrRequestMethod.CrudSafeDelete =>
                        _apizrOptions.TypeInfo.GetCustomAttribute<LogDeleteAttribute>(true),
                    _ => null
                };
            }
            else
            {
                // Classic api logging
                logAttribute = methodDetails.MethodInfo.GetCustomAttribute<LogAttribute>(); // Request logging
                if (logAttribute == null && methodDetails.RequestMethod == ApizrRequestMethod.HttpPostMultipart) // No log attribute and multipart upload?
                {
                    // Then ignore request body (file data)
                    var verbosity = _apizrOptions.TrafficVerbosity.IgnoreMessageParts(HttpMessageParts.RequestBody);
                    logAttribute = new LogAttribute(verbosity, _apizrOptions.HttpTracerMode, _apizrOptions.LogLevels);
                }
            }

            // Return log attribute
            _loggingMethodsSet.TryAdd(methodDetails, logAttribute);
            return logAttribute;
        }

        #endregion

        #region Caching

        private bool ShouldCache<TResult>(MethodDetails methodDetails, Expression originalExpression,
            IApizrRequestOptions requestOptions, out CacheAttributeBase cacheAttribute)
        {
            string cacheKey = null;
            var shouldCache = requestOptions.CacheOptions.TryGetValue(ApizrConfigurationSource.FinalConfiguration, out cacheAttribute) &&
                   cacheAttribute.Mode != CacheMode.None &&
                   TryGetCacheKey<TResult>(methodDetails, originalExpression, out cacheKey);

            if(shouldCache) 
                cacheAttribute.CacheKey = cacheKey;

            return shouldCache;
        }

        private bool TryGetCacheKey<TResult>(MethodDetails methodDetails, Expression restExpression, out string cacheKey)
        {
            lock (this)
            {
                var methodToCacheData = methodDetails;

                // Were we asked for it already ?
                if (_cacheKeyMethodsSet.TryGetValue(methodToCacheData, out cacheKey))
                {
                    // Yes we were so return True if cache key is well-defined
                    return !string.IsNullOrWhiteSpace(cacheKey);
                }

                // Is it an IApiResponse result ?
                if (typeof(TResult) == typeof(IApiResponse))
                {
                    // Yes it is! Save details for next calls and return False
                    _cacheKeyMethodsSet.TryAdd(methodToCacheData, null);
                    return false;
                }

                // Method is cacheable so prepare the cache key
                var methodCallExpression = GetMethodCallExpression<TResult>(restExpression);
                cacheKey = $"{_webApiFriendlyName}.{methodCallExpression.Method.Name}(";

                // Get all method parameters
                var methodParameters = methodToCacheData.MethodInfo.GetParameters().ToList();

                // Are there any parameters except potential CancellationToken and Refit properties ?
                if (!methodParameters.Any(x =>
                        !typeof(CancellationToken).GetTypeInfo().IsAssignableFrom(x.ParameterType.GetTypeInfo()) &&
                        x.CustomAttributes.All(y =>
                            !typeof(PropertyAttribute).GetTypeInfo().IsAssignableFrom(y.AttributeType.GetTypeInfo()))))
                {
                    // No there isn't!
                    cacheKey += ")";

                    // Save details for next calls and return True
                    _cacheKeyMethodsSet.TryAdd(methodToCacheData, cacheKey);
                    return true;
                }

                // Get method parameters suitable for cache key
                var hasCacheKeyAttribute = methodParameters.Any(mp => mp.CustomAttributes.Any(y => y.AttributeType == typeof(CacheKeyAttribute)));
                var cacheKeyMethodParameters = methodParameters
                    .Select((methodParameter, index) => new
                    {
                        Index = index, // parameter position
                        ParameterInfo = methodParameter, // parameter infos
                        CacheKeyAttribute = hasCacheKeyAttribute ? // potential cache key attribute
                            methodParameter.GetCustomAttribute<CacheKeyAttribute>(true) : 
                            null
                    })
                    .Where(x => 
                        (!hasCacheKeyAttribute || // no cache key attribute anywhere
                         x.CacheKeyAttribute != null) && // or cache key attribute on this parameter
                         x.ParameterInfo.ParameterType != typeof(CancellationToken) && // not a cancellation token
                         x.ParameterInfo.CustomAttributes.All(customAttribute => !typeof(PropertyAttribute).GetTypeInfo().IsAssignableFrom(customAttribute.AttributeType.GetTypeInfo()))) // not a Refit property
                    .ToList();

                var parameters = new List<string>();
                foreach (var cacheKeyMethodParameter in cacheKeyMethodParameters)
                {
                    var parameterInfo = cacheKeyMethodParameter.ParameterInfo;
                    var parameterName = parameterInfo.Name;
                    var extractedArgumentValues = ExtractConstants(methodCallExpression.Arguments.ElementAt(cacheKeyMethodParameter.Index)).ToList();
                    var extractedArgumentValue = extractedArgumentValues.FirstOrDefault()?.Value;
                    if (extractedArgumentValue == null)
                        continue;

                    // Prepare formatted name and value pair for our cache key
                    string parameter = null;

                    // Simple param value OR complex type with overriden ToString
                    var value = extractedArgumentValue.ToString();
                    if (!string.IsNullOrWhiteSpace(value) && value != extractedArgumentValue.GetType().ToString())
                    {
                        parameter = value.Contains(":") && !value.Contains("[")
                            ? $"{parameterName}:{{{value}}}"
                            : $"{parameterName}:{value}";
                    }
                    // Dictionary param key values
                    else if (extractedArgumentValue is IDictionary objectDictionary)
                    {
                        parameter = $"{parameterName}:[{objectDictionary.ToString(":", ", ", cacheKeyMethodParameter.CacheKeyAttribute?.PropertyNames)}]";
                    }
                    else
                    {
                        // Complex type param values without override
                        var complexParameters = extractedArgumentValue.ToString(":", ", ", cacheKeyMethodParameter.CacheKeyAttribute?.PropertyNames);
                        if (!string.IsNullOrWhiteSpace(complexParameters))
                            parameter = $"{parameterName}:{{{complexParameters}}}";
                    }

                    if (!string.IsNullOrWhiteSpace(parameter))
                        parameters.Add(parameter);
                }

                cacheKey += $"{string.Join(", ", parameters)})";

                return true;
            }
        }

        private CacheAttributeBase GetMethodCacheAttribute(MethodDetails methodDetails)
        {
            if (methodDetails == null)
                return null;

            if(_cachingMethodsSet.TryGetValue(methodDetails, out var cacheAttribute))
                return cacheAttribute;

            if (_apizrOptions.IsCrudApi) // Crud api method
            {
                cacheAttribute = methodDetails.RequestMethod switch // Specific method policies
                {
                    var method when method == ApizrRequestMethod.CrudReadAll || method == ApizrRequestMethod.CrudSafeReadAll =>
                        _apizrOptions.TypeInfo.GetCustomAttribute<CacheReadAllAttribute>(true),
                    var method when method == ApizrRequestMethod.CrudRead || method == ApizrRequestMethod.CrudSafeRead =>
                        _apizrOptions.TypeInfo.GetCustomAttribute<CacheReadAttribute>(true),
                    _ => null
                };
            }
            else
            {
                // Standard api method
                cacheAttribute = methodDetails.MethodInfo.GetCustomAttribute<CacheAttribute>();
            }

            _cachingMethodsSet.TryAdd(methodDetails, cacheAttribute);

            return cacheAttribute;
        }

        private static IDictionary<string, object> GetCacheEntries<TResult>(IApizrResponse<TResult> apizrResponse,
            CacheAttributeBase cacheAttribute, out TimeSpan? lifeSpan) =>
            GetCacheEntries(apizrResponse, apizrResponse.Result, cacheAttribute, out lifeSpan);

        private static IDictionary<string, object> GetCacheEntries<TResult>(IApizrResponse apizrResponse, TResult result,
            CacheAttributeBase cacheAttribute, out TimeSpan? lifeSpan)
        {
            var cacheEntries = new Dictionary<string, object>();
            var apiResponse = apizrResponse.ApiResponse;
            switch (cacheAttribute.Mode)
            {
                case CacheMode.FetchOrGet or CacheMode.GetOrFetch:
                {
                    lifeSpan = cacheAttribute.LifeSpan;
                    cacheEntries.Add(cacheAttribute.CacheKey, result);
                    return cacheEntries;
                }
                case CacheMode.SetByHeader:
                {
                    var responseHeaders = apiResponse.Headers;
                    var contentHeaders = apiResponse.ContentHeaders;
                    if (responseHeaders.CacheControl is not null)
                    {
                        var cacheControl = responseHeaders.CacheControl;
                        if (!cacheControl.NoStore &&
                            (responseHeaders.Vary.Count is not 1 || !responseHeaders.Vary.Contains("*")))
                        {
                            if (cacheControl.ToString().Contains("immutable"))
                            {
                                lifeSpan = cacheAttribute.LifeSpan;
                                cacheEntries.Add(cacheAttribute.CacheKey, result);
                                cacheEntries.Add(cacheAttribute.FinalModeCacheKey, CacheMode.GetOrFetch);
                                    return cacheEntries;
                            }

                            if (cacheControl.SharedMaxAge.HasValue)
                            {
                                lifeSpan = cacheControl.SharedMaxAge.Value;
                                cacheEntries.Add(cacheAttribute.CacheKey, result);
                                cacheEntries.Add(cacheAttribute.FinalModeCacheKey, CacheMode.GetOrFetch);
                                return cacheEntries;
                            }

                            if (cacheControl.MaxAge.HasValue)
                            {
                                lifeSpan = cacheControl.MaxAge.Value;
                                cacheEntries.Add(cacheAttribute.CacheKey, result);
                                cacheEntries.Add(cacheAttribute.FinalModeCacheKey, CacheMode.GetOrFetch);
                                return cacheEntries;
                            }
                        }
                    }

                    var expires = contentHeaders?.Expires ?? default;
                    if (expires != default || 
                        (contentHeaders?.TryGetValues("Expires", out var expireValues) is true &&
                          DateTimeOffset.TryParse(expireValues.FirstOrDefault(), out expires)))
                    {
                        var startDate = responseHeaders.Date ?? DateTimeOffset.UtcNow;
                        if (expires > startDate)
                        {
                            lifeSpan = expires - startDate;
                            cacheEntries.Add(cacheAttribute.CacheKey, result);
                            cacheEntries.Add(cacheAttribute.FinalModeCacheKey, CacheMode.GetOrFetch);
                            return cacheEntries;
                        }
                    }

                    var etag = responseHeaders.ETag?.ToString() ??
                               (responseHeaders.TryGetValues("ETag", out var etagValues) ? etagValues.FirstOrDefault() : null);
                    if (!string.IsNullOrWhiteSpace(etag))
                    {
                        lifeSpan = cacheAttribute.LifeSpan;
                        cacheEntries.Add(cacheAttribute.CacheKey, result);

                        var cacheHeaders = new[] {$"If-None-Match: {etag}"};
                        cacheEntries.Add(cacheAttribute.HeadersCacheKey, cacheHeaders);
                        cacheEntries.Add(cacheAttribute.FinalModeCacheKey, CacheMode.FetchOrGet);

                        return cacheEntries;
                    }

                    var lastModified = contentHeaders?.LastModified ?? default;
                    if (lastModified != default || 
                        (contentHeaders?.TryGetValues("Last-Modified", out var lastModifiedValues) is true &&
                         DateTimeOffset.TryParse(lastModifiedValues.FirstOrDefault(), out lastModified)))
                    {
                        lifeSpan = cacheAttribute.LifeSpan;
                        cacheEntries.Add(cacheAttribute.CacheKey, result);

                        var cacheHeaders = new[] {$"If-Modified-Since: {lastModified}"};
                        cacheEntries.Add(cacheAttribute.HeadersCacheKey, cacheHeaders);
                        cacheEntries.Add(cacheAttribute.FinalModeCacheKey, CacheMode.FetchOrGet);

                        return cacheEntries;
                    }

                    lifeSpan = TimeSpan.Zero;
                    return cacheEntries;
                }
                default:
                {
                    lifeSpan = TimeSpan.Zero;
                    return cacheEntries;
                }
            }
        }

        #endregion

        #region Resiliencing

        private ResiliencePipeline<TResult> GetMethodResiliencePipeline<TResult>(MethodDetails methodDetails, IApizrRequestOptions requestOptions)
        {
            var resiliencePipelineKeys = requestOptions.ResiliencePipelineOptions
                .Where(kvp => kvp.Key != ApizrConfigurationSource.FinalConfiguration)
                .OrderBy(kvp => kvp.Key)
                .SelectMany(kvp => kvp.Value.SelectMany(attribute => attribute.RegistryKeys))
                .Distinct()
                .ToArray();
            if(resiliencePipelineKeys.Length == 0)
                return ResiliencePipeline<TResult>.Empty;

            var includedKeys = new List<string>();
            if (requestOptions.ResiliencePipelineOptions.TryGetValue(ApizrConfigurationSource.FinalConfiguration, out var yetIncludedAttributeKeys))
                includedKeys.AddRange(yetIncludedAttributeKeys.SelectMany(attribute => attribute.RegistryKeys));

            var keysToInclude = resiliencePipelineKeys.Except(includedKeys).ToList();
            if (keysToInclude.Count == 0)
                return ResiliencePipeline<TResult>.Empty;

            if (_lazyResiliencePipelineRegistry == null)
            {
                _apizrOptions.Logger.Log(requestOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: You asked to apply some resilience pipelines but didn't register any of it. Resilience pipelines will be ignored.");
                return ResiliencePipeline<TResult>.Empty;
            }

            var resiliencePipelineBuilder = new ResiliencePipelineBuilder<TResult>();
            foreach (var resiliencePipelineKey in resiliencePipelineKeys)
            {
                if (_lazyResiliencePipelineRegistry.Value.TryGetPipeline<TResult>(resiliencePipelineKey, out var resiliencePipeline))
                {
                    resiliencePipelineBuilder.AddPipeline(resiliencePipeline);
                    includedKeys.Add(resiliencePipelineKey);
                    _apizrOptions.Logger.Log(requestOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Resilience pipeline named '{resiliencePipelineKey}' will be applied");
                } 
            }

            requestOptions.ResiliencePipelineOptions[ApizrConfigurationSource.FinalConfiguration] = [new ResiliencePipelineAttribute([.. includedKeys])];

            return resiliencePipelineBuilder.Build();
        }

        private ResiliencePipeline GetMethodResiliencePipeline(MethodDetails methodDetails, IApizrRequestOptions requestOptions)
        {
            var resiliencePipelineKeys = requestOptions.ResiliencePipelineOptions
                .Where(kvp => kvp.Key != ApizrConfigurationSource.FinalConfiguration)
                .OrderBy(kvp => kvp.Key)
                .SelectMany(kvp => kvp.Value.SelectMany(attribute => attribute.RegistryKeys))
                .Distinct()
                .ToArray();
            if (resiliencePipelineKeys.Length == 0)
                return ResiliencePipeline.Empty;

            var includedKeys = new List<string>();
            if (requestOptions.ResiliencePipelineOptions.TryGetValue(ApizrConfigurationSource.FinalConfiguration, out var yetIncludedAttributeKeys))
                includedKeys.AddRange(yetIncludedAttributeKeys.SelectMany(attribute => attribute.RegistryKeys));

            var keysToInclude = resiliencePipelineKeys.Except(includedKeys).ToList();
            if (keysToInclude.Count == 0)
                return ResiliencePipeline.Empty;

            if (_lazyResiliencePipelineRegistry == null)
            {
                _apizrOptions.Logger.Log(requestOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: You asked to apply some resilience pipelines but didn't register any of it. Resilience pipelines will be ignored.");
                return ResiliencePipeline.Empty;
            }

            var resiliencePipelineBuilder = new ResiliencePipelineBuilder();
            foreach (var resiliencePipelineKey in resiliencePipelineKeys)
            {
                if (_lazyResiliencePipelineRegistry.Value.TryGetPipeline(resiliencePipelineKey, out var resiliencePipeline))
                {
                    resiliencePipelineBuilder.AddPipeline(resiliencePipeline);
                    includedKeys.Add(resiliencePipelineKey);
                    _apizrOptions.Logger.Log(requestOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Resilience pipeline named '{resiliencePipelineKey}' will be applied");
                }
            }

            requestOptions.ResiliencePipelineOptions[ApizrConfigurationSource.FinalConfiguration] = [new ResiliencePipelineAttribute([.. includedKeys])];

            return resiliencePipelineBuilder.Build();
        }

        private ResiliencePipelineAttributeBase GetMethodResiliencePipelineAttribute(MethodDetails methodDetails)
        {
            if (methodDetails == null || _apizrOptions.IsCrudApi)
                return null;

            if (_resilienceMethodsSet.TryGetValue(methodDetails, out var resilienceAttribute))
                return resilienceAttribute;

            // Standard api method attribute
            resilienceAttribute = methodDetails.MethodInfo.GetCustomAttribute<ResiliencePipelineAttribute>();
            if(resilienceAttribute != null)
                resilienceAttribute.RequestMethod = methodDetails.RequestMethod;

            _resilienceMethodsSet.TryAdd(methodDetails, resilienceAttribute);

            return resilienceAttribute;
        }

        #endregion

        #region Prioritizing

        private IList<HandlerParameterAttribute> GetRequestHandlerParameterAttributes(MethodDetails methodDetails)
        {
            if (_handlerParameterMethodsSet.TryGetValue(methodDetails, out var handlerParameterAttributes))
                return handlerParameterAttributes;

            if (_apizrOptions.IsCrudApi)
            {
                // Crud api parameters
                handlerParameterAttributes = methodDetails.RequestMethod switch // Request parameters
                {
                    var method when method == ApizrRequestMethod.CrudReadAll || method == ApizrRequestMethod.CrudSafeReadAll =>
                        _apizrOptions.TypeInfo
                            .GetCustomAttributes<ReadAllHandlerParameterAttribute>(true)
                            .Cast<HandlerParameterAttribute>()
                            .ToList(),
                    var method when method == ApizrRequestMethod.CrudRead || method == ApizrRequestMethod.CrudSafeRead =>
                        _apizrOptions.TypeInfo
                            .GetCustomAttributes<ReadHandlerParameterAttribute>(true)
                            .Cast<HandlerParameterAttribute>()
                            .ToList(),
                    _ => null
                };
            }
            else
            {
                // Classic api parameters
                handlerParameterAttributes =
                    methodDetails.MethodInfo.GetCustomAttributes<HandlerParameterAttribute>()
                        .ToList(); // Request parameters
            }

            // Return log attribute
            _handlerParameterMethodsSet.TryAdd(methodDetails, handlerParameterAttributes);
            return handlerParameterAttributes;
        }

        #endregion

        #region Cancelling

        private TimeoutAttributeBase GetOperationTimeoutAttribute(MethodDetails methodDetails)
        {
            if (_operationTimeoutMethodsSet.TryGetValue(methodDetails, out var timeoutAttribute))
                return timeoutAttribute;

            if (_apizrOptions.IsCrudApi)
            {
                // Crud api operation timeout
                timeoutAttribute = methodDetails.RequestMethod switch // Operation timeout
                {
                    var method when method == ApizrRequestMethod.CrudReadAll || method == ApizrRequestMethod.CrudSafeReadAll =>
                        _apizrOptions.TypeInfo.GetCustomAttribute<ReadAllOperationTimeoutAttribute>(true),
                    var method when method == ApizrRequestMethod.CrudRead || method == ApizrRequestMethod.CrudSafeRead =>
                        _apizrOptions.TypeInfo.GetCustomAttribute<ReadOperationTimeoutAttribute>(true),
                    var method when method == ApizrRequestMethod.CrudCreate || method == ApizrRequestMethod.CrudSafeCreate =>
                        _apizrOptions.TypeInfo.GetCustomAttribute<CreateOperationTimeoutAttribute>(true),
                    var method when method == ApizrRequestMethod.CrudUpdate || method == ApizrRequestMethod.CrudSafeUpdate =>
                        _apizrOptions.TypeInfo.GetCustomAttribute<UpdateOperationTimeoutAttribute>(true),
                    var method when method == ApizrRequestMethod.CrudDelete || method == ApizrRequestMethod.CrudSafeDelete =>
                        _apizrOptions.TypeInfo.GetCustomAttribute<DeleteOperationTimeoutAttribute>(true),
                    _ => null
                };
            }
            else
            {
                // Classic api operation timeout
                timeoutAttribute = methodDetails.MethodInfo.GetCustomAttribute<OperationTimeoutAttribute>(); // Operation timeout
            }

            // Return operation timeout attribute
            _operationTimeoutMethodsSet.TryAdd(methodDetails, timeoutAttribute);
            return timeoutAttribute;
        }

        private TimeoutAttributeBase GetRequestTimeoutAttribute(MethodDetails methodDetails)
        {
            if (_requestTimeoutMethodsSet.TryGetValue(methodDetails, out var timeoutAttribute))
                return timeoutAttribute;

            if (_apizrOptions.IsCrudApi)
            {
                // Crud api request timeout
                timeoutAttribute = methodDetails.RequestMethod switch // Request timeout
                {
                    var method when method == ApizrRequestMethod.CrudReadAll || method == ApizrRequestMethod.CrudSafeReadAll =>
                        _apizrOptions.TypeInfo.GetCustomAttribute<ReadAllRequestTimeoutAttribute>(true),
                    var method when method == ApizrRequestMethod.CrudRead || method == ApizrRequestMethod.CrudSafeRead =>
                        _apizrOptions.TypeInfo.GetCustomAttribute<ReadRequestTimeoutAttribute>(true),
                    var method when method == ApizrRequestMethod.CrudCreate || method == ApizrRequestMethod.CrudSafeCreate =>
                        _apizrOptions.TypeInfo.GetCustomAttribute<CreateRequestTimeoutAttribute>(true),
                    var method when method == ApizrRequestMethod.CrudUpdate || method == ApizrRequestMethod.CrudSafeUpdate =>
                        _apizrOptions.TypeInfo.GetCustomAttribute<UpdateRequestTimeoutAttribute>(true),
                    var method when method == ApizrRequestMethod.CrudDelete || method == ApizrRequestMethod.CrudSafeDelete =>
                        _apizrOptions.TypeInfo.GetCustomAttribute<DeleteRequestTimeoutAttribute>(true),
                    _ => null
                };
            }
            else
            {
                // Classic api request timeout
                timeoutAttribute = methodDetails.MethodInfo.GetCustomAttribute<RequestTimeoutAttribute>(); // Request logging
            }

            // Return request timeout attribute
            _requestTimeoutMethodsSet.TryAdd(methodDetails, timeoutAttribute);
            return timeoutAttribute;
        }

        #endregion

        #region Mapping

        private TDestination Map<TSource, TDestination>(TSource source)
        {
            TDestination destination;
            try
            {
                if (typeof(TSource) == typeof(TDestination))
                    destination = (TDestination) Convert.ChangeType(source, typeof(TDestination));
                else
                {
                    if (_mappingHandler.GetType() == typeof(VoidMappingHandler))
                        throw new NotImplementedException(
                            $"You asked to map data but did not provide any data mapping handler. Please use ");

                    destination = _mappingHandler.Map<TSource, TDestination>(source);
                }
            }
            catch (Exception e)
            {
                throw new ApizrException(e);
            }

            return destination;
        }

        #endregion

        #region Details

        private MethodDetails GetMethodDetails(Expression restExpression)
        {
            var webApiType = typeof(TWebApi);
            var methodCallExpression = GetMethodCallExpression(restExpression);
            var requestMethod = GetApizrRequestMethod(methodCallExpression);

            return new MethodDetails(webApiType, methodCallExpression.Method, requestMethod);
        }

        private MethodDetails GetMethodDetails<TResult>(Expression restExpression)
        {
            var webApiType = typeof(TWebApi);
            var methodCallExpression = GetMethodCallExpression<TResult>(restExpression);
            var requestMethod = GetApizrRequestMethod(methodCallExpression);
            return new MethodDetails(webApiType, methodCallExpression.Method, requestMethod);
        }

        private ApizrRequestMethod GetApizrRequestMethod(MethodCallExpression methodCallExpression)
        {
            ApizrRequestMethod requestMethod;
            if (_apizrOptions.IsCrudApi) // Crud api method
            {
                requestMethod = methodCallExpression.Method.Name switch // Specific method policies
                {
                    var method when method == ApizrRequestMethod.CrudCreate.Method => ApizrRequestMethod.CrudCreate,
                    var method when method == ApizrRequestMethod.CrudReadAll.Method => ApizrRequestMethod.CrudReadAll,
                    var method when method == ApizrRequestMethod.CrudRead.Method => ApizrRequestMethod.CrudRead,
                    var method when method == ApizrRequestMethod.CrudUpdate.Method => ApizrRequestMethod.CrudUpdate,
                    var method when method == ApizrRequestMethod.CrudDelete.Method => ApizrRequestMethod.CrudDelete,
                    var method when method == ApizrRequestMethod.CrudSafeCreate.Method => ApizrRequestMethod.CrudSafeCreate,
                    var method when method == ApizrRequestMethod.CrudSafeReadAll.Method => ApizrRequestMethod.CrudSafeReadAll,
                    var method when method == ApizrRequestMethod.CrudSafeRead.Method => ApizrRequestMethod.CrudSafeRead,
                    var method when method == ApizrRequestMethod.CrudSafeUpdate.Method => ApizrRequestMethod.CrudSafeUpdate,
                    var method when method == ApizrRequestMethod.CrudSafeDelete.Method => ApizrRequestMethod.CrudSafeDelete,
                    _ => throw new NotImplementedException($"{methodCallExpression.Method.Name} method is not yet handled by Apizr. Please open an issue if needed")
                };
            }
            else
            {
                var httpMethodAttribute = methodCallExpression.Method.GetCustomAttribute<HttpMethodAttribute>(true);
                requestMethod = httpMethodAttribute?.Method.Method switch
                {
                    var method when method == ApizrRequestMethod.HttpGet.Method => ApizrRequestMethod.HttpGet,
                    var method when method == ApizrRequestMethod.HttpPost.Method =>
                        methodCallExpression.Method.GetCustomAttribute<MultipartAttribute>(true) != null
                        ? ApizrRequestMethod.HttpPostMultipart
                        : ApizrRequestMethod.HttpPost,
                    var method when method == ApizrRequestMethod.HttpPut.Method => ApizrRequestMethod.HttpPut,
                    var method when method == ApizrRequestMethod.HttpDelete.Method => ApizrRequestMethod.HttpDelete,
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
                    var method when method == ApizrRequestMethod.HttpPatch.Method => ApizrRequestMethod.HttpPatch,
#endif
                    var method when method == ApizrRequestMethod.HttpHead.Method => ApizrRequestMethod.HttpHead,
                    var method when method == ApizrRequestMethod.HttpOptions.Method => ApizrRequestMethod.HttpOptions,
                    _ => throw new NotImplementedException(
                        $"{httpMethodAttribute?.Method.Method} method is not yet handled by Apizr. Please open an issue if needed")
                };
            }

            return requestMethod;
        }

        private static IEnumerable<ExtractedConstant> ExtractConstants(Expression expression)
        {
            if (expression == null)
                yield break;

            if (expression is ConstantExpression constantsExpression)
                yield return new ExtractedConstant {Value = constantsExpression.Value};

            else if (expression is LambdaExpression lambdaExpression)
                foreach (var constant in ExtractConstants(lambdaExpression.Body))
                    yield return constant;

            else if (expression is UnaryExpression unaryExpression)
                foreach (var constant in ExtractConstants(unaryExpression.Operand))
                    yield return constant;

            else if (expression is MethodCallExpression methodCallExpression)
            {
                foreach (var arg in methodCallExpression.Arguments)
                foreach (var constant in ExtractConstants(arg))
                    yield return constant;
                foreach (var constant in ExtractConstants(methodCallExpression.Object))
                    yield return constant;
            }
            else if (expression is MemberExpression memberExpression)
            {
                foreach (var constants in ExtractConstants(memberExpression.Expression))
                {
                    object value = null;
                    if (constants.Value != null)
                    {
                        switch (memberExpression.Member)
                        {
                            case FieldInfo fieldInfo:
                                value = fieldInfo.GetValue(constants.Value);
                                break;
                            case PropertyInfo propertyInfo:
                                value = propertyInfo.GetValue(constants.Value);
                                break;
                        } 
                    }

                    yield return new ExtractedConstant {Value = value};
                }
            }
            else if (expression is InvocationExpression invocationExpression)
            {
                foreach (var constants in ExtractConstants(invocationExpression.Expression))
                    yield return constants;
            }
            else if (expression is ParameterExpression parameterExpression)
            {
                if (parameterExpression.Type == typeof(CancellationToken))
                    yield return new ExtractedConstant {Value = CancellationToken.None};
                else
                    yield return new ExtractedConstant();
            }
            else if (expression is ListInitExpression listInitExpression)
            {
                if (typeof(IDictionary).IsAssignableFrom(listInitExpression.Type))
                {
                    var parameters = new Dictionary<string, object>();
                    foreach (var initializer in listInitExpression.Initializers)
                    {
                        string key = null;
                        object value = null;
                        foreach (var initializerArgument in initializer.Arguments)
                        {
                            foreach (var constant in ExtractConstants(initializerArgument))
                            {
                                if (string.IsNullOrWhiteSpace(key))
                                    key = constant.Value.ToString();
                                else
                                    value = constant.Value;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(key) && value != null)
                            parameters.Add(key, value);
                    }

                    yield return new ExtractedConstant {Value = parameters};
                }
                else
                    yield return new ExtractedConstant { };
            }
            else if (expression is MemberInitExpression memberInitExpression)
            {
                if (memberInitExpression.Bindings.Any())
                {
                    var parameters = new Dictionary<string, object>();
                    foreach (var memberBinding in memberInitExpression.Bindings)
                    {
                        if (memberBinding is MemberAssignment assignment)
                            foreach (var constant in ExtractConstants(assignment.Expression))
                                parameters.Add(memberBinding.Member.Name, constant.Value);
                        else if (memberBinding is MemberListBinding listBinding)
                        {
                            foreach (var initializer in listBinding.Initializers)
                            {
                                string key = null;
                                object value = null;
                                foreach (var initializerArgument in initializer.Arguments)
                                {
                                    foreach (var constant in ExtractConstants(initializerArgument))
                                    {
                                        if (string.IsNullOrWhiteSpace(key))
                                            key = constant.Value.ToString();
                                        else
                                            value = constant.Value;
                                    }
                                }

                                if (!string.IsNullOrWhiteSpace(key) && value != null)
                                    parameters.Add(key, value);
                            }
                        }
                    }

                    yield return new ExtractedConstant {Value = parameters};
                }
                else
                    foreach (var constants in ExtractConstants(memberInitExpression.NewExpression))
                        yield return constants;
            }
            else if (expression is NewExpression newExpression)
            {
                var parameters = new Dictionary<string, object>();
                var constructorParameters = newExpression.Constructor.GetParameters();
                for (var i = 0; i < constructorParameters.Length; i++)
                {
                    if (newExpression.Arguments[i] is ConstantExpression constantExpression)
                        parameters.Add(constructorParameters[i].Name, constantExpression.Value);
                }

                yield return new ExtractedConstant {Value = parameters};
            }
            else
                throw new NotImplementedException();
        }

        private MethodCallExpression GetMethodCallExpression(
            Expression expression)
        {
            switch (expression)
            {
                case LambdaExpression lambdaExpression:
                    return GetMethodCallExpression(lambdaExpression.Body);
                case InvocationExpression methodInvocationBody:
                {
                    var methodCallExpression = (MethodCallExpression) methodInvocationBody.Expression;
                    return methodCallExpression;
                }
                case MethodCallExpression methodCallExpression:
                    return methodCallExpression;
                default:
                    throw new NotImplementedException();
            }
        }

        private MethodCallExpression GetMethodCallExpression<TResult>(
            Expression expression)
        {
            switch (expression)
            {
                case LambdaExpression lambdaExpression:
                    return GetMethodCallExpression<TResult>(lambdaExpression.Body);
                case InvocationExpression methodInvocationBody:
                {
                    var methodCallExpression = (MethodCallExpression) methodInvocationBody.Expression;
                    return methodCallExpression;
                }
                case MethodCallExpression methodCallExpression:
                    return methodCallExpression;
                default:
                    throw new NotImplementedException();
            }
        }

        private sealed class ExtractedConstant
        {
            public object Value { get; set; }
        }

        class MethodDetails
        {
            public MethodDetails(Type webApiType, MethodInfo methodInfo, ApizrRequestMethod requestMethod)
            {
                WebApiType = webApiType;
                MethodInfo = methodInfo;
                RequestMethod = requestMethod;
            }

            private Type WebApiType { get; }

            public MethodInfo MethodInfo { get; }

            public ApizrRequestMethod RequestMethod { get; }

            public override int GetHashCode() =>
                WebApiType.GetHashCode() * 23 * MethodInfo.GetHashCode() * 23 * 29;

            public override bool Equals(object obj)
            {
                return obj is MethodDetails methodCacheDetails &&
                       methodCacheDetails.WebApiType == WebApiType &&
                       methodCacheDetails.MethodInfo.Equals(MethodInfo);
            }
        }

        #endregion
    }
}
