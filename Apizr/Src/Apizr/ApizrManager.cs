// Largely inspired by Refit.Insane.PowerPack, but with many more features

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
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
using Apizr.Configuring.Request;
using Apizr.Configuring.Shared;
using Apizr.Configuring.Shared.Context;
using Apizr.Connecting;
using Apizr.Extending;
using Apizr.Logging.Attributes;
using Apizr.Mapping;
using Apizr.Requesting;
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
            LogAttributeBase requestLogAttribute = null,
            IList<HandlerParameterAttribute> requestHandlerParameterAttributes = null,
            TimeoutAttributeBase operationTimeoutAttribute = null,
            TimeoutAttributeBase requestTimeoutAttribute = null,
            ResiliencePipelineAttributeBase resiliencePipelineAttribute = null,
            CacheAttributeBase requestCacheAttribute = null)
        {
            // Create base request options from parent options
            var requestOptions = new ApizrRequestOptions(baseOptions,
                requestHandlerParameterAttributes?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ?? [],
                requestLogAttribute?.HttpTracerMode,
                requestLogAttribute?.TrafficVerbosity,
                operationTimeoutAttribute?.Timeout,
                requestTimeoutAttribute?.Timeout,
                resiliencePipelineAttribute?.RegistryKeys,
                requestCacheAttribute,
                requestLogAttribute?.LogLevels);

            // Create request options builder with request options
            var builder = new ApizrRequestOptionsBuilder(requestOptions) as IApizrRequestOptionsBuilder;

            // Only once with full options building
            if (baseOptions != null)
            {
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
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, requestLogAttribute,
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
                    async _ => await executeApiMethod.Compile().Invoke(requestOptionsBuilder.ApizrOptions, webApi),
                    requestOptionsBuilder);
            }
            catch (Exception e)
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                    $"{methodDetails.MethodInfo.Name}: Request threw an exception with message {e.Message}");
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                    $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException)} with InnerException");

                var ex = new ApizrException(e);
                if (requestOptionsBuilder.ApizrOptions.OnException == null)
                    throw ex;

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Exception is handled by a custom action");
                requestOptionsBuilder.ApizrOptions.OnException(ex);
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
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, requestLogAttribute,
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
                    async options => await executeApiMethod.Compile().Invoke(options, webApi, apiData),
                    requestOptionsBuilder);
            }
            catch (Exception e)
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                    $"{methodDetails.MethodInfo.Name}: Request threw an exception with message {e.Message}");
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                    $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException)} with InnerException");

                var ex = new ApizrException(e);
                if (requestOptionsBuilder.ApizrOptions.OnException == null)
                    throw ex;

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Exception is handled by a custom action");
                requestOptionsBuilder.ApizrOptions.OnException(ex);
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
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute, resiliencePipelineAttribute);

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            IApizrResponse response = default;
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
                    async options => await executeApiMethod.Compile().Invoke(options, webApi),
                    requestOptionsBuilder);

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

            if (response.Exception != null && requestOptionsBuilder.ApizrOptions.OnException != null)
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Handling an {nameof(ApizrException)}");

                requestOptionsBuilder.ApizrOptions.OnException(response.Exception);
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
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute,
                resiliencePipelineAttribute, requestCacheAttribute);

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiData result = default;

            if (ShouldCache<TApiData>(methodDetails, originalExpression, requestOptionsBuilder.ApizrOptions, out var cacheAttribute, out var cacheKey))
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                result = await _cacheHandler.GetAsync<TApiData>(cacheKey,
                    requestOptionsBuilder.ApizrOptions.CancellationToken);
                if (!Equals(result, default))
                {
                    if (requestOptionsBuilder.ApizrOptions.ClearCache)
                    {
                        await _cacheHandler.RemoveAsync(cacheKey, requestOptionsBuilder.ApizrOptions.CancellationToken);
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

            if (Equals(result, default) || requestCacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                ResilienceContext resilienceContext = null;
                Exception ex = null;
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
                        async options => await executeApiMethod.Compile().Invoke(options, webApi),
                        requestOptionsBuilder);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                        $"{methodDetails.MethodInfo.Name}: Request threw an exception with message {e.Message}");

                    ex = new ApizrException<TApiData>(e, result);
                    if (requestOptionsBuilder.ApizrOptions.OnException == null)
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                            Equals(result, default(TApiData))
                                ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TApiData>)} with InnerException but no cached result"
                                : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TApiData>)} with InnerException and cached result");

                        throw ex;
                    }

                    if (Equals(result, default(TApiData)))
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Handling an {nameof(ApizrException<TApiData>)} with InnerException but no cached result");

                        requestOptionsBuilder.ApizrOptions.OnException(ex as ApizrException);

                        if (requestOptionsBuilder.ApizrOptions.LetThrowOnExceptionWithEmptyCache)
                            throw ex;
                    }
                    else
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Handling an {nameof(ApizrException<TApiData>)} with InnerException and cached result");

                        requestOptionsBuilder.ApizrOptions.OnException(ex as ApizrException);
                    }
                }
                finally
                {
                    if (resilienceContext != null &&
                        requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ReturnToPoolOnComplete != false)
                        ResilienceContextPool.Shared.Return(resilienceContext);
                }

                if (ex == null && result != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, result, cacheAttribute.LifeSpan,
                        requestOptionsBuilder.ApizrOptions.CancellationToken);
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
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute,
                resiliencePipelineAttribute, requestCacheAttribute);

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiData cachedResult = default;
            IApizrResponse<TApiData> response = default;

            if (ShouldCache<TApiData>(methodDetails, originalExpression, requestOptionsBuilder.ApizrOptions, out var cacheAttribute, out var cacheKey))
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                cachedResult = await _cacheHandler.GetAsync<TApiData>(cacheKey,
                    requestOptionsBuilder.ApizrOptions.CancellationToken);
                if (!Equals(cachedResult, default))
                {
                    if (requestOptionsBuilder.ApizrOptions.ClearCache)
                    {
                        await _cacheHandler.RemoveAsync(cacheKey, requestOptionsBuilder.ApizrOptions.CancellationToken);
                        cachedResult = default;
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

            if (!Equals(cachedResult, default) && cacheAttribute?.Mode == CacheMode.GetOrFetch)
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

                    requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Executing the request");

                    var apiResponse = await resiliencePipeline.ExecuteAsync(
                        async options => await executeApiMethod.Compile().Invoke(options, webApi),
                        requestOptionsBuilder);

                    if (apiResponse.IsSuccessStatusCode && apiResponse.Error == null)
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Request succeed! Using request data for the response");
                    else if(!Equals(cachedResult, default(TApiData)))
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Request failed! Using cached data for the response");
                    else
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Request failed! No cached data to use for the response");

                    response = apiResponse.Error == null ?
                        new ApizrResponse<TApiData>(apiResponse, apiResponse.Content, ApizrResponseDataSource.Request) :
                        new ApizrResponse<TApiData>(apiResponse, new ApizrException<TApiData>(apiResponse.Error, cachedResult));
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

                if (response.Exception != null && requestOptionsBuilder.ApizrOptions.OnException != null)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        Equals(cachedResult, default(TApiData))
                            ? $"{methodDetails.MethodInfo.Name}: Handling an {nameof(ApizrException<TApiData>)} with InnerException but no cached result"
                            : $"{methodDetails.MethodInfo.Name}: Handling an {nameof(ApizrException<TApiData>)} with InnerException and cached result");

                    requestOptionsBuilder.ApizrOptions.OnException(response.Exception);
                }

                if (response.Exception == null && 
                    response.Result != null && 
                    _cacheHandler != null &&
                    !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && 
                    cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, response.Result, cacheAttribute.LifeSpan,
                        requestOptionsBuilder.ApizrOptions.CancellationToken);
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
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute,
                resiliencePipelineAttribute, requestCacheAttribute);

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiData result = default;

            if (ShouldCache<TApiData>(methodDetails, originalExpression, requestOptionsBuilder.ApizrOptions, out var cacheAttribute, out var cacheKey))
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                result = await _cacheHandler.GetAsync<TApiData>(cacheKey,
                    requestOptionsBuilder.ApizrOptions.CancellationToken);
                if (!Equals(result, default))
                {
                    if (requestOptionsBuilder.ApizrOptions.ClearCache)
                    {
                        await _cacheHandler.RemoveAsync(cacheKey, requestOptionsBuilder.ApizrOptions.CancellationToken);
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

            if (Equals(result, default) || cacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                ResilienceContext resilienceContext = null;
                Exception ex = null;
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
                        async options => await executeApiMethod.Compile().Invoke(options, webApi),
                        requestOptionsBuilder);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                        $"{methodDetails.MethodInfo.Name}: Request threw an exception with message {e.Message}");

                    ex = new ApizrException<TModelData>(e, Map<TApiData, TModelData>(result));
                    if (requestOptionsBuilder.ApizrOptions.OnException == null)
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                            Equals(result, default(TApiData))
                                ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelData>)} with InnerException but no cached result"
                                : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelData>)} with InnerException and cached result");

                        throw ex;
                    }

                    if (Equals(result, default(TApiData)))
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Handling an {nameof(ApizrException<TModelData>)} with InnerException but no cached result");

                        requestOptionsBuilder.ApizrOptions.OnException(ex as ApizrException);

                        if (requestOptionsBuilder.ApizrOptions.LetThrowOnExceptionWithEmptyCache)
                            throw ex;
                    }
                    else
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Handling an {nameof(ApizrException<TModelData>)} with InnerException and cached result");

                        requestOptionsBuilder.ApizrOptions.OnException(ex as ApizrException);
                    }
                }
                finally
                {
                    if (resilienceContext != null &&
                        requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ReturnToPoolOnComplete != false)
                        ResilienceContextPool.Shared.Return(resilienceContext);
                }

                if (ex == null && result != null && _cacheHandler != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, result, cacheAttribute.LifeSpan,
                        requestOptionsBuilder.ApizrOptions.CancellationToken);
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
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute,
                resiliencePipelineAttribute, requestCacheAttribute);

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiData cachedResult = default;
            TApiData apiResult = default;
            IApizrResponse<TModelData> response = default;

            if (ShouldCache<TApiData>(methodDetails, originalExpression, requestOptionsBuilder.ApizrOptions, out var cacheAttribute, out var cacheKey))
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                cachedResult = await _cacheHandler.GetAsync<TApiData>(cacheKey,
                    requestOptionsBuilder.ApizrOptions.CancellationToken);
                if (!Equals(cachedResult, default))
                {
                    if (requestOptionsBuilder.ApizrOptions.ClearCache)
                    {
                        await _cacheHandler.RemoveAsync(cacheKey, requestOptionsBuilder.ApizrOptions.CancellationToken);
                        cachedResult = default;
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

            if (!Equals(cachedResult, default) && cacheAttribute?.Mode == CacheMode.GetOrFetch)
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
                        async options => await executeApiMethod.Compile().Invoke(options, webApi),
                        requestOptionsBuilder);

                    if (apiResponse.IsSuccessStatusCode && apiResponse.Error == null)
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Request succeed! Using request data for the response");
                    else if (!Equals(cachedResult, default(TApiData)))
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Request failed! Using cached data for the response");
                    else
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Request failed! No cached data to use for the response");

                    apiResult = apiResponse.Content;

                    response = apiResponse.Error == null ?
                        new ApizrResponse<TModelData>(apiResponse, Map<TApiData, TModelData>(apiResult), ApizrResponseDataSource.Request) :
                        new ApizrResponse<TModelData>(apiResponse, new ApizrException<TModelData>(apiResponse.Error, Map<TApiData, TModelData>(cachedResult)));
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

                if (response.Exception != null && requestOptionsBuilder.ApizrOptions.OnException != null)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        Equals(cachedResult, default(TApiData))
                            ? $"{methodDetails.MethodInfo.Name}: Handling an {nameof(ApizrException<TModelData>)} with InnerException but no cached result"
                            : $"{methodDetails.MethodInfo.Name}: Handling an {nameof(ApizrException<TModelData>)} with InnerException and cached result");

                    requestOptionsBuilder.ApizrOptions.OnException(response.Exception);
                }

                if (response.Exception == null &&
                    _cacheHandler != null &&
                    !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null &&
                    cacheAttribute.Mode != CacheMode.None &&
                    !Equals(apiResult, default(TApiData)))
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, apiResult, cacheAttribute.LifeSpan,
                        requestOptionsBuilder.ApizrOptions.CancellationToken);
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
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute, resiliencePipelineAttribute);

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            IApizrResponse response = default;
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
                    async options => await executeApiMethod.Compile().Invoke(options, webApi, apiData),
                    requestOptionsBuilder);

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

            if (response.Exception != null && requestOptionsBuilder.ApizrOptions.OnException != null)
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Handling an {nameof(ApizrException)}");

                requestOptionsBuilder.ApizrOptions.OnException(response.Exception);
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
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute,
                resiliencePipelineAttribute, requestCacheAttribute);

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiData result = default;

            if (ShouldCache<TApiData>(methodDetails, originalExpression, requestOptionsBuilder.ApizrOptions, out var cacheAttribute, out var cacheKey))
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                result = await _cacheHandler.GetAsync<TApiData>(cacheKey,
                    requestOptionsBuilder.ApizrOptions.CancellationToken);
                if (!Equals(result, default))
                {
                    if (requestOptionsBuilder.ApizrOptions.ClearCache)
                    {
                        await _cacheHandler.RemoveAsync(cacheKey, requestOptionsBuilder.ApizrOptions.CancellationToken);
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

            if (Equals(result, default) || cacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                ResilienceContext resilienceContext = null;
                Exception ex = null;
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
                        async options => await executeApiMethod.Compile().Invoke(options, webApi, apiData),
                        requestOptionsBuilder);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                        $"{methodDetails.MethodInfo.Name}: Request threw an exception with message {e.Message}");

                    ex = new ApizrException<TApiData>(e, result);
                    if (requestOptionsBuilder.ApizrOptions.OnException == null)
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                            Equals(result, default(TApiData))
                                ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TApiData>)} with InnerException but no cached result"
                                : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TApiData>)} with InnerException and cached result");

                        throw ex;
                    }

                    if (Equals(result, default(TApiData)))
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Handling an {nameof(ApizrException<TApiData>)} with InnerException but no cached result");

                        requestOptionsBuilder.ApizrOptions.OnException(ex as ApizrException);

                        if (requestOptionsBuilder.ApizrOptions.LetThrowOnExceptionWithEmptyCache)
                            throw ex;
                    }
                    else
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Handling an {nameof(ApizrException<TApiData>)} with InnerException and cached result");

                        requestOptionsBuilder.ApizrOptions.OnException(ex as ApizrException);
                    }
                }
                finally
                {
                    if (resilienceContext != null &&
                        requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ReturnToPoolOnComplete != false)
                        ResilienceContextPool.Shared.Return(resilienceContext);
                }

                if (ex == null && result != null && _cacheHandler != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, result, cacheAttribute.LifeSpan,
                        requestOptionsBuilder.ApizrOptions.CancellationToken);
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
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute,
                resiliencePipelineAttribute, requestCacheAttribute);

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiData cachedResult = default;
            TApiData apiResult = default;
            IApizrResponse<TModelData> response = default;

            if (ShouldCache<TApiData>(methodDetails, originalExpression, requestOptionsBuilder.ApizrOptions, out var cacheAttribute, out var cacheKey))
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                cachedResult = await _cacheHandler.GetAsync<TApiData>(cacheKey,
                    requestOptionsBuilder.ApizrOptions.CancellationToken);
                if (!Equals(cachedResult, default))
                {
                    if (requestOptionsBuilder.ApizrOptions.ClearCache)
                    {
                        await _cacheHandler.RemoveAsync(cacheKey, requestOptionsBuilder.ApizrOptions.CancellationToken);
                        cachedResult = default;
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

            if (!Equals(cachedResult, default) && cacheAttribute?.Mode == CacheMode.GetOrFetch)
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
                        async options => await executeApiMethod.Compile().Invoke(options, webApi, apiData),
                        requestOptionsBuilder);

                    if (apiResponse.IsSuccessStatusCode && apiResponse.Error == null)
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Request succeed! Using request data for the response");
                    else if (!Equals(cachedResult, default(TApiData)))
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Request failed! Using cached data for the response");
                    else
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Request failed! No cached data to use for the response");

                    apiResult = apiResponse.Content;

                    response = apiResponse.Error == null ?
                        new ApizrResponse<TModelData>(apiResponse, Map<TApiData, TModelData>(apiResult), ApizrResponseDataSource.Request) :
                        new ApizrResponse<TModelData>(apiResponse, new ApizrException<TModelData>(apiResponse.Error, Map<TApiData, TModelData>(cachedResult)));

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

                if (response.Exception != null && requestOptionsBuilder.ApizrOptions.OnException != null)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        Equals(cachedResult, default(TApiData))
                            ? $"{methodDetails.MethodInfo.Name}: Handling an {nameof(ApizrException<TModelData>)} with InnerException but no cached result"
                            : $"{methodDetails.MethodInfo.Name}: Handling an {nameof(ApizrException<TModelData>)} with InnerException and cached result");

                    requestOptionsBuilder.ApizrOptions.OnException(response.Exception);
                }

                if (response.Exception == null && 
                    _cacheHandler != null &&
                    !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && 
                    cacheAttribute.Mode != CacheMode.None &&
                    !Equals(apiResult, default(TApiData)))
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                                    $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, apiResult, cacheAttribute.LifeSpan,
                        requestOptionsBuilder.ApizrOptions.CancellationToken);
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
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute,
                resiliencePipelineAttribute, requestCacheAttribute);

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiResultData result = default;

            if (ShouldCache<TApiResultData>(methodDetails, originalExpression, requestOptionsBuilder.ApizrOptions, out var cacheAttribute, out var cacheKey))
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                result = await _cacheHandler.GetAsync<TApiResultData>(cacheKey,
                    requestOptionsBuilder.ApizrOptions.CancellationToken);
                if (!Equals(result, default))
                {
                    if (requestOptionsBuilder.ApizrOptions.ClearCache)
                    {
                        await _cacheHandler.RemoveAsync(cacheKey, requestOptionsBuilder.ApizrOptions.CancellationToken);
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

            if (Equals(result, default) || cacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                ResilienceContext resilienceContext = null;
                Exception ex = null;
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
                        async options => await executeApiMethod.Compile().Invoke(options, webApi, apiRequestData),
                        requestOptionsBuilder);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                        $"{methodDetails.MethodInfo.Name}: Request threw an exception with message {e.Message}");

                    ex = new ApizrException<TModelResultData>(e, Map<TApiResultData, TModelResultData>(result));
                    if (requestOptionsBuilder.ApizrOptions.OnException == null)
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.High(),
                            Equals(result, default(TApiResultData))
                                ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelResultData>)} with InnerException but no cached result"
                                : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelResultData>)} with InnerException and cached result");

                        throw ex;
                    }

                    if (Equals(result, default(TApiResultData)))
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Handling an {nameof(ApizrException<TModelResultData>)} with InnerException but no cached result");

                        requestOptionsBuilder.ApizrOptions.OnException(ex as ApizrException);

                        if (requestOptionsBuilder.ApizrOptions.LetThrowOnExceptionWithEmptyCache)
                            throw ex;
                    }
                    else
                    {
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Handling an {nameof(ApizrException<TModelResultData>)} with InnerException and cached result");

                        requestOptionsBuilder.ApizrOptions.OnException(ex as ApizrException);
                    }
                }
                finally
                {
                    if (resilienceContext != null &&
                        requestOptionsBuilder.ApizrOptions.ResilienceContextOptions?.ReturnToPoolOnComplete != false)
                        ResilienceContextPool.Shared.Return(resilienceContext);
                }

                if (ex == null && result != null && _cacheHandler != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, result, cacheAttribute.LifeSpan,
                        requestOptionsBuilder.ApizrOptions.CancellationToken);
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
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute,
                resiliencePipelineAttribute, requestCacheAttribute);

            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiResultData cachedResult = default;
            TApiResultData apiResult = default;
            IApizrResponse<TModelResultData> response = default;

            if (ShouldCache<TApiResultData>(methodDetails, originalExpression, requestOptionsBuilder.ApizrOptions, out var cacheAttribute, out var cacheKey))
            {
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Medium(),
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                cachedResult = await _cacheHandler.GetAsync<TApiResultData>(cacheKey,
                    requestOptionsBuilder.ApizrOptions.CancellationToken);
                if (!Equals(cachedResult, default))
                {
                    if (requestOptionsBuilder.ApizrOptions.ClearCache)
                    {
                        await _cacheHandler.RemoveAsync(cacheKey, requestOptionsBuilder.ApizrOptions.CancellationToken);
                        cachedResult = default;
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

            if (!Equals(cachedResult, default) && cacheAttribute?.Mode == CacheMode.GetOrFetch)
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
                        async options => await executeApiMethod.Compile().Invoke(options, webApi, apiRequestData),
                        requestOptionsBuilder);

                    if (apiResponse.IsSuccessStatusCode && apiResponse.Error == null)
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Request succeed! Using request data for the response");
                    else if (!Equals(cachedResult, default(TApiResultData)))
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Request failed! Using cached data for the response");
                    else
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Request failed! No cached data to use for the response");

                    apiResult = apiResponse.Content;

                    response = apiResponse.Error == null ?
                        new ApizrResponse<TModelResultData>(apiResponse, Map<TApiResultData, TModelResultData>(apiResult), ApizrResponseDataSource.Request) :
                        new ApizrResponse<TModelResultData>(apiResponse, new ApizrException<TModelResultData>(apiResponse.Error, Map<TApiResultData, TModelResultData>(cachedResult)));
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

                if (response.Exception != null && requestOptionsBuilder.ApizrOptions.OnException != null)
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        Equals(cachedResult, default(TApiResultData))
                            ? $"{methodDetails.MethodInfo.Name}: Handling an {nameof(ApizrException<TModelResultData>)} with InnerException but no cached result"
                            : $"{methodDetails.MethodInfo.Name}: Handling an {nameof(ApizrException<TModelResultData>)} with InnerException and cached result");

                    requestOptionsBuilder.ApizrOptions.OnException(response.Exception);
                }

                if (response.Exception == null &&
                    _cacheHandler != null &&
                    !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && 
                    cacheAttribute.Mode != CacheMode.None &&
                    !Equals(apiResult, default(TApiResultData)))
                {
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, apiResult, cacheAttribute.LifeSpan,
                        requestOptionsBuilder.ApizrOptions.CancellationToken);
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
                await _cacheHandler.ClearAsync(cancellationToken);
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

                    var success = await _cacheHandler.RemoveAsync(cacheKey, cancellationToken);
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
            if (typeof(ICrudApi<,,,>).IsAssignableFromGenericType(methodDetails.ApiInterfaceType))
            {
                // Crud api headers
                var modelType = methodDetails.ApiInterfaceType.GetGenericArguments().First();
                headersAttribute = methodDetails.MethodInfo.Name switch // Request headers
                {
                    "ReadAll" => modelType.GetTypeInfo().GetCustomAttribute<ReadAllHeadersAttribute>(true),
                    "Read" => modelType.GetTypeInfo().GetCustomAttribute<ReadHeadersAttribute>(true),
                    "Create" => modelType.GetTypeInfo().GetCustomAttribute<CreateHeadersAttribute>(true),
                    "Update" => modelType.GetTypeInfo().GetCustomAttribute<UpdateHeadersAttribute>(true),
                    "Delete" => modelType.GetTypeInfo().GetCustomAttribute<DeleteHeadersAttribute>(true),
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

            if (typeof(ICrudApi<,,,>).IsAssignableFromGenericType(methodDetails.ApiInterfaceType))
            {
                // Crud api logging
                var modelType = methodDetails.ApiInterfaceType.GetGenericArguments().First();
                logAttribute = methodDetails.MethodInfo.Name switch // Request logging
                {
                    "ReadAll" => modelType.GetTypeInfo().GetCustomAttribute<LogReadAllAttribute>(true),
                    "Read" => modelType.GetTypeInfo().GetCustomAttribute<LogReadAttribute>(true),
                    "Create" => modelType.GetTypeInfo().GetCustomAttribute<LogCreateAttribute>(true),
                    "Update" => modelType.GetTypeInfo().GetCustomAttribute<LogUpdateAttribute>(true),
                    "Delete" => modelType.GetTypeInfo().GetCustomAttribute<LogDeleteAttribute>(true),
                    _ => null
                };
            }
            else
            {
                // Classic api logging
                logAttribute = methodDetails.MethodInfo.GetCustomAttribute<LogAttribute>(); // Request logging
            }

            // Return log attribute
            _loggingMethodsSet.TryAdd(methodDetails, logAttribute);
            return logAttribute;
        }

        #endregion

        #region Caching

        private bool ShouldCache<TResult>(MethodDetails methodDetails, Expression originalExpression,
            IApizrRequestOptions requestOptions, out CacheAttributeBase cacheAttribute, out string cacheKey)
        {
            cacheKey = null;
            return requestOptions.CacheOptions.TryGetValue(ApizrConfigurationSource.FinalConfiguration, out cacheAttribute) &&
                   cacheAttribute.Mode != CacheMode.None &&
                   TryGetCacheKey<TResult>(methodDetails, originalExpression, out cacheKey);
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

                // There's one or more parameters!
                // Get arguments values
                var extractedArguments = methodCallExpression.Arguments
                    .SelectMany(ExtractConstants)
                    .ToList();

                // Get a potential specific cache key
                var specificCacheKeys = methodParameters
                    .Select((methodParameter, index) => new
                    {
                        Index = index,
                        ParameterInfo = methodParameter
                    })
                    .Where(x => x.ParameterInfo.CustomAttributes.Any(y => y.AttributeType == typeof(CacheKeyAttribute)))
                    .ToList();

                var parameters = new List<string>();
                for (var i = 0; i <= extractedArguments.Count - 1; i++)
                {
                    // If we get any specific cache keys, ignore all other arguments
                    if (specificCacheKeys.Count > 0)
                    {
                        if (i < specificCacheKeys.Min(specificCacheKey => specificCacheKey.Index))
                            continue;
                        if (i > specificCacheKeys.Max(specificCacheKey => specificCacheKey.Index))
                            break;
                        if(specificCacheKeys.All(specificCacheKey => specificCacheKey.Index != i))
                            continue;
                    }

                    // Get a potential specific cache key
                    var specificCacheKey = specificCacheKeys.FirstOrDefault(x => x.Index == i);

                    // Ignore CancellationToken and Refit Property parameters
                    var parameterInfo = methodParameters[i];
                    if (typeof(CancellationToken).GetTypeInfo()
                            .IsAssignableFrom(parameterInfo.ParameterType.GetTypeInfo())
                        || parameterInfo.CustomAttributes.Any(x =>
                            typeof(PropertyAttribute).GetTypeInfo().IsAssignableFrom(x.AttributeType.GetTypeInfo())))
                        continue;

                    var parameterName = parameterInfo.Name;
                    var extractedArgument = extractedArguments[i];
                    var extractedArgumentValue = extractedArgument.Value;
                    if (extractedArgumentValue == null)
                        continue;

                    object parameterValue = null;
                    var isArgumentValuePrimitive = extractedArgumentValue.GetType().GetTypeInfo().IsPrimitive ||
                                                   extractedArgumentValue is decimal ||
                                                   extractedArgumentValue is string;

                    // If it's a primitive, just set the value
                    if (isArgumentValuePrimitive)
                    {
                        parameterValue = extractedArgument.Value;
                    }
                    else
                    {
                        // Is there a specific cache key with a target field?
                        var cacheKeyAttribute =
                            specificCacheKey?.ParameterInfo.GetCustomAttribute<CacheKeyAttribute>(true);
                        if (!string.IsNullOrWhiteSpace(cacheKeyAttribute?.PropertyName))
                        {
                            // There's a specific cache key with a target field!
                            var cacheKeyField = extractedArgument
                                .Value
                                .GetType()
                                .GetRuntimeFields()
                                .FirstOrDefault(x => x.Name.Equals(cacheKeyAttribute.PropertyName));

                            // If we can find it, we set its value to the cache key
                            if (cacheKeyField != null)
                                parameterValue = cacheKeyField.GetValue(extractedArgument.Value);
                        }
                    }

                    // Set argument value if cache key is still null
                    if (parameterValue == null)
                        parameterValue = extractedArgument.Value;

                    // Add formatted name and value pair to our cache key
                    var parameter = GetParameterKeyValues(parameterName, parameterValue);
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

            if (typeof(ICrudApi<,,,>).IsAssignableFromGenericType(_apizrOptions.WebApiType)) // Crud api method
            {
                var modelType = methodDetails.ApiInterfaceType.GetGenericArguments().First();
                cacheAttribute = methodDetails.MethodInfo.Name switch // Specific method policies
                {
                    "ReadAll" => modelType.GetTypeInfo().GetCustomAttribute<CacheReadAllAttribute>(true),
                    "Read" => modelType.GetTypeInfo().GetCustomAttribute<CacheReadAttribute>(true),
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

        private string GetParameterKeyValues(string parameterName, object parameterValue)
        {
            // Simple param value OR complex type with overriden ToString
            var value = parameterValue.ToString();
            if (!string.IsNullOrWhiteSpace(value) && value != parameterValue.GetType().ToString())
                return value.Contains(":") && !value.Contains("[")
                    ? $"{parameterName}:{{{value}}}"
                    : $"{parameterName}:{value}";

            // Dictionary param key values
            if (parameterValue is IDictionary objectDictionary)
                return $"{parameterName}:[{objectDictionary.ToString(":", ", ")}]";

            // Complex type param values without override
            var parameters = parameterValue.ToString(":", ", ");
            if (!string.IsNullOrWhiteSpace(parameters))
                return $"{parameterName}:{{{parameters}}}";

            return null;
        }

        #endregion

        #region Resiliencing

        private ResiliencePipeline<TResult> GetMethodResiliencePipeline<TResult>(MethodDetails methodDetails, IApizrRequestOptions requestOptions)
        {
            if (_lazyResiliencePipelineRegistry == null)
                return ResiliencePipeline<TResult>.Empty;

            var resiliencePipelineKeys = requestOptions.ResiliencePipelineKeys
                .Where(kvp => kvp.Key != ApizrConfigurationSource.FinalConfiguration)
                .OrderBy(kvp => kvp.Key)
                .SelectMany(kvp => kvp.Value)
                .Distinct()
                .ToArray();
            if(resiliencePipelineKeys.Length == 0)
                return ResiliencePipeline<TResult>.Empty;

            var resiliencePipelineBuilder = new ResiliencePipelineBuilder<TResult>();
            var includedKeys = new List<string>();
            if (requestOptions.ResiliencePipelineKeys.TryGetValue(ApizrConfigurationSource.FinalConfiguration, out var yetIncludedKeys)) 
                includedKeys.AddRange(yetIncludedKeys);

            foreach (var resiliencePipelineKey in resiliencePipelineKeys)
            {
                if (_lazyResiliencePipelineRegistry.Value.TryGetPipeline<TResult>(resiliencePipelineKey, out var resiliencePipeline))
                {
                    resiliencePipelineBuilder.AddPipeline(resiliencePipeline);
                    includedKeys.Add(resiliencePipelineKey);
                    _apizrOptions.Logger.Log(requestOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Resilience pipeline with key {resiliencePipelineKey} will be applied");
                }
            }

            requestOptions.ResiliencePipelineKeys[ApizrConfigurationSource.FinalConfiguration] = includedKeys.ToArray();

            return resiliencePipelineBuilder.Build();
        }

        private ResiliencePipeline GetMethodResiliencePipeline(MethodDetails methodDetails, IApizrRequestOptions requestOptions)
        {
            if (_lazyResiliencePipelineRegistry == null)
                return ResiliencePipeline.Empty;

            var resiliencePipelineKeys = requestOptions.ResiliencePipelineKeys
                .Where(kvp => kvp.Key != ApizrConfigurationSource.FinalConfiguration)
                .OrderBy(kvp => kvp.Key)
                .SelectMany(kvp => kvp.Value)
                .Distinct()
                .ToArray();
            if (resiliencePipelineKeys.Length == 0)
                return ResiliencePipeline.Empty;

            var resiliencePipelineBuilder = new ResiliencePipelineBuilder();
            var includedKeys = new List<string>();
            if (requestOptions.ResiliencePipelineKeys.TryGetValue(ApizrConfigurationSource.FinalConfiguration, out var yetIncludedKeys))
                includedKeys.AddRange(yetIncludedKeys);

            foreach (var resiliencePipelineKey in resiliencePipelineKeys)
            {
                if (_lazyResiliencePipelineRegistry.Value.TryGetPipeline(resiliencePipelineKey, out var resiliencePipeline))
                {
                    resiliencePipelineBuilder.AddPipeline(resiliencePipeline);
                    includedKeys.Add(resiliencePipelineKey);
                    _apizrOptions.Logger.Log(requestOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Resilience pipeline with key {resiliencePipelineKey} will be applied");
                }
            }

            requestOptions.ResiliencePipelineKeys[ApizrConfigurationSource.FinalConfiguration] = includedKeys.ToArray();

            return resiliencePipelineBuilder.Build();
        }

        private ResiliencePipelineAttributeBase GetMethodResiliencePipelineAttribute(MethodDetails methodDetails)
        {
            if (methodDetails == null)
                return null;

            if (_resilienceMethodsSet.TryGetValue(methodDetails, out var resilienceAttribute))
                return resilienceAttribute;

            if (typeof(ICrudApi<,,,>).IsAssignableFromGenericType(_apizrOptions.WebApiType)) // Crud api method
            {
                var modelType = methodDetails.ApiInterfaceType.GetGenericArguments().First();
                resilienceAttribute = methodDetails.MethodInfo.Name switch // Specific method policies
                {
                    "Create" => modelType.GetTypeInfo().GetCustomAttribute<CreateResiliencePipelineAttribute>(),
                    "ReadAll" => modelType.GetTypeInfo().GetCustomAttribute<ReadAllResiliencePipelineAttribute>(),
                    "Read" => modelType.GetTypeInfo().GetCustomAttribute<ReadResiliencePipelineAttribute>(),
                    "Update" => modelType.GetTypeInfo().GetCustomAttribute<UpdateResiliencePipelineAttribute>(),
                    "Delete" => modelType.GetTypeInfo().GetCustomAttribute<DeleteResiliencePipelineAttribute>(),
                    _ => null
                };
            }
            else
            {
                // Standard api method
                resilienceAttribute = methodDetails.MethodInfo.GetCustomAttribute<ResiliencePipelineAttribute>();
            }

            _resilienceMethodsSet.TryAdd(methodDetails, resilienceAttribute);

            return resilienceAttribute;
        }

        #endregion

        #region Prioritizing

        private IList<HandlerParameterAttribute> GetRequestHandlerParameterAttributes(MethodDetails methodDetails)
        {
            if (_handlerParameterMethodsSet.TryGetValue(methodDetails, out var handlerParameterAttributes))
                return handlerParameterAttributes;

            if (typeof(ICrudApi<,,,>).IsAssignableFromGenericType(methodDetails.ApiInterfaceType))
            {
                // Crud api parameters
                var modelType = methodDetails.ApiInterfaceType.GetGenericArguments().First();
                handlerParameterAttributes = methodDetails.MethodInfo.Name switch // Request parameters
                {
                    "ReadAll" => modelType.GetTypeInfo().GetCustomAttributes<ReadAllHandlerParameterAttribute>(true)
                        .Cast<HandlerParameterAttribute>().ToList(),
                    "Read" => modelType.GetTypeInfo().GetCustomAttributes<ReadHandlerParameterAttribute>(true)
                        .Cast<HandlerParameterAttribute>().ToList(),
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

            if (typeof(ICrudApi<,,,>).IsAssignableFromGenericType(methodDetails.ApiInterfaceType))
            {
                // Crud api operation timeout
                var modelType = methodDetails.ApiInterfaceType.GetGenericArguments().First();
                timeoutAttribute = methodDetails.MethodInfo.Name switch // Operation timeout
                {
                    "ReadAll" => modelType.GetTypeInfo().GetCustomAttribute<ReadAllOperationTimeoutAttribute>(true),
                    "Read" => modelType.GetTypeInfo().GetCustomAttribute<ReadOperationTimeoutAttribute>(true),
                    "Create" => modelType.GetTypeInfo().GetCustomAttribute<CreateOperationTimeoutAttribute>(true),
                    "Update" => modelType.GetTypeInfo().GetCustomAttribute<UpdateOperationTimeoutAttribute>(true),
                    "Delete" => modelType.GetTypeInfo().GetCustomAttribute<DeleteOperationTimeoutAttribute>(true),
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

            if (typeof(ICrudApi<,,,>).IsAssignableFromGenericType(methodDetails.ApiInterfaceType))
            {
                // Crud api request timeout
                var modelType = methodDetails.ApiInterfaceType.GetGenericArguments().First();
                timeoutAttribute = methodDetails.MethodInfo.Name switch // Request timeout
                {
                    "ReadAll" => modelType.GetTypeInfo().GetCustomAttribute<ReadAllRequestTimeoutAttribute>(true),
                    "Read" => modelType.GetTypeInfo().GetCustomAttribute<ReadRequestTimeoutAttribute>(true),
                    "Create" => modelType.GetTypeInfo().GetCustomAttribute<CreateRequestTimeoutAttribute>(true),
                    "Update" => modelType.GetTypeInfo().GetCustomAttribute<UpdateRequestTimeoutAttribute>(true),
                    "Delete" => modelType.GetTypeInfo().GetCustomAttribute<DeleteRequestTimeoutAttribute>(true),
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
            return new MethodDetails(webApiType, methodCallExpression.Method);
        }

        private MethodDetails GetMethodDetails<TResult>(Expression restExpression)
        {
            var webApiType = typeof(TWebApi);
            var methodCallExpression = GetMethodCallExpression<TResult>(restExpression);
            return new MethodDetails(webApiType, methodCallExpression.Method);
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

                    yield return new ExtractedConstant {Value = $"[{parameters.ToString(":", ", ")}]"};
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

                    yield return new ExtractedConstant {Value = parameters.ToString(":", ", ")};
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

                yield return new ExtractedConstant {Value = parameters.ToString(":", ", ")};
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
            public MethodDetails(Type apiInterfaceType, MethodInfo methodInfo)
            {
                ApiInterfaceType = apiInterfaceType;
                MethodInfo = methodInfo;
            }

            public Type ApiInterfaceType { get; }

            public MethodInfo MethodInfo { get; }

            public override int GetHashCode() =>
                ApiInterfaceType.GetHashCode() * 23 * MethodInfo.GetHashCode() * 23 * 29;

            public override bool Equals(object obj)
            {
                return obj is MethodDetails methodCacheDetails &&
                       methodCacheDetails.ApiInterfaceType == ApiInterfaceType &&
                       methodCacheDetails.MethodInfo.Equals(MethodInfo);
            }
        }

        #endregion
    }
}
