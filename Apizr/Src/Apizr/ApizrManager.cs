// Largely inspired by Refit.Insane.PowerPack, but with many more features

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
using Apizr.Connecting;
using Apizr.Extending;
using Apizr.Logging.Attributes;
using Apizr.Mapping;
using Apizr.Requesting;
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
            TimeoutAttributeBase requestTimeoutAttribute = null)
        {
            var requestOptions = new ApizrRequestOptions(baseOptions,
                requestHandlerParameterAttributes?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ??
                new Dictionary<string, object>(),
                requestLogAttribute?.HttpTracerMode,
                requestLogAttribute?.TrafficVerbosity,
                operationTimeoutAttribute?.Timeout,
                requestTimeoutAttribute?.Timeout, 
                requestLogAttribute?.LogLevels);
            var builder = new ApizrRequestOptionsBuilder(requestOptions);
            optionsBuilder?.Invoke(builder);

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

        private readonly ConcurrentDictionary<MethodDetails, (CacheAttributeBase cacheAttribute, string cacheKey)>
            _cachingMethodsSet;

        private readonly ConcurrentDictionary<MethodDetails, LogAttributeBase> _loggingMethodsSet;
        private readonly ConcurrentDictionary<MethodDetails, object> _resiliencePipelineMethodsSet;
        private readonly ConcurrentDictionary<MethodDetails, IList<HandlerParameterAttribute>> _handlerParameterMethodsSet;
        private readonly ConcurrentDictionary<MethodDetails, TimeoutAttributeBase> _operationTimeoutMethodsSet;
        private readonly ConcurrentDictionary<MethodDetails, TimeoutAttributeBase> _requestTimeoutMethodsSet;

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

            _cachingMethodsSet = new ConcurrentDictionary<MethodDetails, (CacheAttributeBase cacheAttribute, string cacheKey)>();
            _loggingMethodsSet = new ConcurrentDictionary<MethodDetails, LogAttributeBase>();
            _resiliencePipelineMethodsSet = new ConcurrentDictionary<MethodDetails, object>();
            _handlerParameterMethodsSet = new ConcurrentDictionary<MethodDetails, IList<HandlerParameterAttribute>>();
            _operationTimeoutMethodsSet = new ConcurrentDictionary<MethodDetails, TimeoutAttributeBase>();
            _requestTimeoutMethodsSet = new ConcurrentDictionary<MethodDetails, TimeoutAttributeBase>();
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
            var requestLogAttribute = GetRequestLogAttribute(methodDetails);
            var requestHandlerParameterAttributes = GetRequestHandlerParameterAttributes(methodDetails);
            var operationTimeoutAttribute = GetOperationTimeoutAttribute(methodDetails);
            var requestTimeoutAttribute = GetRequestTimeoutAttribute(methodDetails);
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute);

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

                var resiliencePipeline =
                    GetMethodResiliencePipeline(methodDetails, requestOptionsBuilder.ApizrOptions.LogLevels.Low());
                if (resiliencePipeline != ResiliencePipeline.Empty)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Executing request with some resilience strategies");

                resilienceContext = ResilienceContextPool.Shared.Get(methodDetails.MethodInfo.Name,
                    requestOptionsBuilder.ApizrOptions.CancellationToken);
                if (requestOptionsBuilder.ApizrOptions is IApizrGlobalSharedOptionsBase internalOptions &&
                    internalOptions.ResiliencePropertiesFactories.Any())
                    resilienceContext.Properties.SetProperties(internalOptions.ResiliencePropertiesFactories.ToDictionary(kvp => kvp.Key, kvp => kvp.Value()), out _);
                resilienceContext.WithLogger(_apizrOptions.Logger, requestOptionsBuilder.ApizrOptions.LogLevels,
                    requestOptionsBuilder.ApizrOptions.TrafficVerbosity,
                    requestOptionsBuilder.ApizrOptions.HttpTracerMode);
                requestOptionsBuilder.WithContext(resilienceContext);

                requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

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
                if(resilienceContext != null)
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
            var requestLogAttribute = GetRequestLogAttribute(methodDetails);
            var requestHandlerParameterAttributes = GetRequestHandlerParameterAttributes(methodDetails);
            var operationTimeoutAttribute = GetOperationTimeoutAttribute(methodDetails);
            var requestTimeoutAttribute = GetRequestTimeoutAttribute(methodDetails);
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute);

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

                var resiliencePipeline =
                    GetMethodResiliencePipeline(methodDetails, requestOptionsBuilder.ApizrOptions.LogLevels.Low());
                if (resiliencePipeline != ResiliencePipeline.Empty)
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: Executing request with some resilience strategies");

                var apiData = Map<TModelData, TApiData>(modelData);
                _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                    $"{methodDetails.MethodInfo.Name}: {typeof(TModelData).Name} mapped to {typeof(TApiData).Name}");

                resilienceContext = ResilienceContextPool.Shared.Get(methodDetails.MethodInfo.Name,
                    requestOptionsBuilder.ApizrOptions.CancellationToken);
                if (requestOptionsBuilder.ApizrOptions is IApizrGlobalSharedOptionsBase internalOptions &&
                    internalOptions.ResiliencePropertiesFactories.Any())
                    resilienceContext.Properties.SetProperties(internalOptions.ResiliencePropertiesFactories.ToDictionary(kvp => kvp.Key, kvp => kvp.Value()), out _);
                resilienceContext.WithLogger(_apizrOptions.Logger, requestOptionsBuilder.ApizrOptions.LogLevels,
                    requestOptionsBuilder.ApizrOptions.TrafficVerbosity,
                    requestOptionsBuilder.ApizrOptions.HttpTracerMode);
                requestOptionsBuilder.WithContext(resilienceContext);

                requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

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
                if (resilienceContext != null)
                    ResilienceContextPool.Shared.Return(resilienceContext);
            }
        }

        #endregion

        #region Task<T>

        /// <inheritdoc />
        public virtual Task<TApiData> ExecuteAsync<TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => ExecuteAsync((_, api) => executeApiMethod.Compile().Invoke(api),
                optionsBuilder.WithOriginalExpression(executeApiMethod));

        /// <inheritdoc />
        public virtual async Task<TApiData> ExecuteAsync<TApiData>(
            Expression<Func<IApizrRequestOptions, TWebApi, Task<TApiData>>> executeApiMethod,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
        {
            var webApi = _lazyWebApi.Value;
            var requestOnlyOptionsBuilder = CreateRequestOptionsBuilder(optionsBuilder);
            var originalExpression = requestOnlyOptionsBuilder.ApizrOptions.OriginalExpression ?? executeApiMethod;
            var methodDetails = GetMethodDetails<TApiData>(originalExpression);
            var requestLogAttribute = GetRequestLogAttribute(methodDetails);
            var requestHandlerParameterAttributes = GetRequestHandlerParameterAttributes(methodDetails);
            var operationTimeoutAttribute = GetOperationTimeoutAttribute(methodDetails);
            var requestTimeoutAttribute = GetRequestTimeoutAttribute(methodDetails);
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute);
            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiData result = default;

            if (IsMethodCacheable<TApiData>(methodDetails, originalExpression, out var cacheAttribute, out var cacheKey))
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

                    var resiliencePipeline =
                        GetMethodResiliencePipeline<TApiData>(methodDetails, requestOptionsBuilder.ApizrOptions.LogLevels.Low());
                    if (resiliencePipeline != ResiliencePipeline<TApiData>.Empty)
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Executing request with some resilience strategies");

                    resilienceContext = ResilienceContextPool.Shared.Get(methodDetails.MethodInfo.Name,
                        requestOptionsBuilder.ApizrOptions.CancellationToken);
                    if (requestOptionsBuilder.ApizrOptions is IApizrGlobalSharedOptionsBase internalOptions &&
                        internalOptions.ResiliencePropertiesFactories.Any())
                        resilienceContext.Properties.SetProperties(internalOptions.ResiliencePropertiesFactories.ToDictionary(kvp => kvp.Key, kvp => kvp.Value()), out _);
                    resilienceContext.WithLogger(_apizrOptions.Logger, requestOptionsBuilder.ApizrOptions.LogLevels,
                        requestOptionsBuilder.ApizrOptions.TrafficVerbosity,
                        requestOptionsBuilder.ApizrOptions.HttpTracerMode);
                    requestOptionsBuilder.WithContext(resilienceContext);

                    requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                    await resiliencePipeline.ExecuteAsync(
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
                    if (resilienceContext != null)
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
                $"{methodDetails.MethodInfo.Name}: Returning result");

            return result;
        }

        /// <inheritdoc />
        public virtual Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod,
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
            var requestLogAttribute = GetRequestLogAttribute(methodDetails);
            var requestHandlerParameterAttributes = GetRequestHandlerParameterAttributes(methodDetails);
            var operationTimeoutAttribute = GetOperationTimeoutAttribute(methodDetails);
            var requestTimeoutAttribute = GetRequestTimeoutAttribute(methodDetails);
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute);
            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiData result = default;

            if (IsMethodCacheable<TApiData>(methodDetails, originalExpression, out var cacheAttribute,
                    out var cacheKey))
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

                    var resiliencePipeline =
                        GetMethodResiliencePipeline<TApiData>(methodDetails, requestOptionsBuilder.ApizrOptions.LogLevels.Low());
                    if (resiliencePipeline != ResiliencePipeline<TApiData>.Empty)
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Executing request with some resilience strategies");

                    resilienceContext = ResilienceContextPool.Shared.Get(methodDetails.MethodInfo.Name,
                        requestOptionsBuilder.ApizrOptions.CancellationToken);
                    if (requestOptionsBuilder.ApizrOptions is IApizrGlobalSharedOptionsBase internalOptions &&
                        internalOptions.ResiliencePropertiesFactories.Any())
                        resilienceContext.Properties.SetProperties(internalOptions.ResiliencePropertiesFactories.ToDictionary(kvp => kvp.Key, kvp => kvp.Value()), out _);
                    resilienceContext.WithLogger(_apizrOptions.Logger, requestOptionsBuilder.ApizrOptions.LogLevels,
                        requestOptionsBuilder.ApizrOptions.TrafficVerbosity,
                        requestOptionsBuilder.ApizrOptions.HttpTracerMode);
                    requestOptionsBuilder.WithContext(resilienceContext);

                    requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                    await resiliencePipeline.ExecuteAsync(
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
                    if (resilienceContext != null)
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
        public virtual Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => ExecuteAsync<TModelData, TApiData>(
                (_, api, apiData) => executeApiMethod.Compile().Invoke(api, apiData), modelData,
                optionsBuilder.WithOriginalExpression(executeApiMethod));

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
            var requestLogAttribute = GetRequestLogAttribute(methodDetails);
            var requestHandlerParameterAttributes = GetRequestHandlerParameterAttributes(methodDetails);
            var operationTimeoutAttribute = GetOperationTimeoutAttribute(methodDetails);
            var requestTimeoutAttribute = GetRequestTimeoutAttribute(methodDetails);
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute);
            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiData result = default;

            if (IsMethodCacheable<TApiData>(methodDetails, originalExpression, out var cacheAttribute,
                    out var cacheKey))
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

                    var resiliencePipeline =
                        GetMethodResiliencePipeline<TApiData>(methodDetails, requestOptionsBuilder.ApizrOptions.LogLevels.Low());
                    if (resiliencePipeline != ResiliencePipeline<TApiData>.Empty)
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Executing request with some resilience strategies");

                    var apiData = Map<TModelData, TApiData>(modelData);
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: {typeof(TModelData).Name} mapped to {typeof(TApiData).Name}");

                    resilienceContext = ResilienceContextPool.Shared.Get(methodDetails.MethodInfo.Name,
                        requestOptionsBuilder.ApizrOptions.CancellationToken);
                    if (requestOptionsBuilder.ApizrOptions is IApizrGlobalSharedOptionsBase internalOptions &&
                        internalOptions.ResiliencePropertiesFactories.Any())
                        resilienceContext.Properties.SetProperties(internalOptions.ResiliencePropertiesFactories.ToDictionary(kvp => kvp.Key, kvp => kvp.Value()), out _);
                    resilienceContext.WithLogger(_apizrOptions.Logger, requestOptionsBuilder.ApizrOptions.LogLevels,
                        requestOptionsBuilder.ApizrOptions.TrafficVerbosity,
                        requestOptionsBuilder.ApizrOptions.HttpTracerMode);
                    requestOptionsBuilder.WithContext(resilienceContext);

                    requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                    await resiliencePipeline.ExecuteAsync(
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
                    if (resilienceContext != null)
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
        public virtual Task<TModelResultData> ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
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
            var requestLogAttribute = GetRequestLogAttribute(methodDetails);
            var requestHandlerParameterAttributes = GetRequestHandlerParameterAttributes(methodDetails);
            var operationTimeoutAttribute = GetOperationTimeoutAttribute(methodDetails);
            var requestTimeoutAttribute = GetRequestTimeoutAttribute(methodDetails);
            var requestOptionsBuilder = CreateRequestOptionsBuilder(_apizrOptions, optionsBuilder, requestLogAttribute,
                requestHandlerParameterAttributes, operationTimeoutAttribute, requestTimeoutAttribute);
            _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiResultData result = default;

            if (IsMethodCacheable<TApiResultData>(methodDetails, originalExpression, out var cacheAttribute,
                    out var cacheKey))
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
                    else
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var resiliencePipeline =
                        GetMethodResiliencePipeline<TApiResultData>(methodDetails, requestOptionsBuilder.ApizrOptions.LogLevels.Low());
                    if (resiliencePipeline != ResiliencePipeline<TApiResultData>.Empty)
                        _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                            $"{methodDetails.MethodInfo.Name}: Executing request with some resilience strategies");

                    var apiRequestData = Map<TModelRequestData, TApiRequestData>(modelRequestData);
                    _apizrOptions.Logger.Log(requestOptionsBuilder.ApizrOptions.LogLevels.Low(),
                        $"{methodDetails.MethodInfo.Name}: {typeof(TModelRequestData).Name} mapped to {typeof(TApiRequestData).Name}");

                    resilienceContext = ResilienceContextPool.Shared.Get(methodDetails.MethodInfo.Name,
                        requestOptionsBuilder.ApizrOptions.CancellationToken);
                    if (requestOptionsBuilder.ApizrOptions is IApizrGlobalSharedOptionsBase internalOptions &&
                        internalOptions.ResiliencePropertiesFactories.Any())
                        resilienceContext.Properties.SetProperties(internalOptions.ResiliencePropertiesFactories.ToDictionary(kvp => kvp.Key, kvp => kvp.Value()), out _);
                    resilienceContext.WithLogger(_apizrOptions.Logger, requestOptionsBuilder.ApizrOptions.LogLevels,
                        requestOptionsBuilder.ApizrOptions.TrafficVerbosity,
                        requestOptionsBuilder.ApizrOptions.HttpTracerMode);
                    requestOptionsBuilder.WithContext(resilienceContext);

                    requestOptionsBuilder.ApizrOptions.CancellationToken.ThrowIfCancellationRequested();

                    await resiliencePipeline.ExecuteAsync(
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
                    if (resilienceContext != null)
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
        public Task<bool> ClearCacheAsync<TResult>(Expression<Func<TWebApi, Task<TResult>>> executeApiMethod)
            => ClearCacheAsync((ct, api) => executeApiMethod.Compile().Invoke(api), CancellationToken.None);

        /// <inheritdoc />
        public async Task<bool> ClearCacheAsync<TResult>(
            Expression<Func<CancellationToken, TWebApi, Task<TResult>>> executeApiMethod,
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
                if (IsMethodCacheable<TResult>(methodDetails, executeApiMethod, out _, out var cacheKey))
                {
                    _apizrOptions.Logger.Log(_apizrOptions.LogLevels.Low(),
                        $"{methodCallExpression.Method.Name}: Method is cacheable");
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

        private bool IsMethodCacheable<TResult>(MethodDetails methodDetails, Expression restExpression,
            out CacheAttributeBase cacheAttribute, out string cacheKey)
        {
            lock (this)
            {
                var methodToCacheData = methodDetails;

                // Did we ask for it already ?
                if (_cachingMethodsSet.TryGetValue(methodToCacheData, out var methodCacheDetails))
                {
                    // Yes we did so get saved specific details
                    cacheAttribute = methodCacheDetails.cacheAttribute;
                    cacheKey = methodCacheDetails.cacheKey;

                    return cacheAttribute != null && !string.IsNullOrWhiteSpace(cacheKey);
                }

                cacheAttribute = null;
                cacheKey = null;

                if (typeof(ICrudApi<,,,>).IsAssignableFromGenericType(methodDetails
                        .ApiInterfaceType)) // Crud api caching
                {
                    var modelType = methodDetails.ApiInterfaceType.GetGenericArguments().First();
                    var methodName = methodDetails.MethodInfo.Name;
                    switch (methodName) // Specific method caching
                    {
                        case "ReadAll":
                            cacheAttribute = modelType.GetTypeInfo().GetCustomAttribute<CacheReadAllAttribute>(true);
                            break;
                        case "Read":
                            cacheAttribute = modelType.GetTypeInfo().GetCustomAttribute<CacheReadAttribute>(true);
                            break;
                    }

                    if (cacheAttribute == null) // Global model caching
                        cacheAttribute = modelType.GetTypeInfo().GetCustomAttribute<CacheAttribute>(true);
                }
                else // Classic api caching
                {
                    cacheAttribute =
                        methodToCacheData.MethodInfo.GetCustomAttribute<CacheAttribute>() ?? // Specific method caching
                        methodDetails.ApiInterfaceType.GetTypeInfo()
                            .GetCustomAttribute<CacheAttribute>(); // Global api interface caching
                }

                if (cacheAttribute == null) // Global assembly caching
                    cacheAttribute = methodDetails.ApiInterfaceType.Assembly.GetCustomAttribute<CacheAttribute>();

                // Are we asked to cache this method?
                if (cacheAttribute == null || cacheAttribute.Mode == CacheMode.None)
                {
                    // No we're not! Save details for next calls and return False
                    _cachingMethodsSet.TryAdd(methodToCacheData, (cacheAttribute, cacheKey));
                    return false;
                }

                // Method is cacheable so prepare cache key
                var methodCallExpression = GetMethodCallExpression<TResult>(restExpression);
                cacheKey = $"{_webApiFriendlyName}.{methodCallExpression.Method.Name}(";

                // Get all method parameters
                var methodParameters = methodToCacheData.MethodInfo.GetParameters().ToList();

                // Is there any parameters except potential CancellationToken and Refit properties ?
                if (!methodParameters.Any(x =>
                        !typeof(CancellationToken).GetTypeInfo().IsAssignableFrom(x.ParameterType.GetTypeInfo()) &&
                        x.CustomAttributes.All(y =>
                            !typeof(PropertyAttribute).GetTypeInfo().IsAssignableFrom(y.AttributeType.GetTypeInfo()))))
                {
                    // No there isn't!
                    cacheKey += ")";

                    // Save details for next calls and return False
                    _cachingMethodsSet.TryAdd(methodToCacheData, (cacheAttribute, cacheKey));
                    return true;
                }

                // There's one or more parameters!
                // Get arguments values
                var extractedArguments = methodCallExpression.Arguments
                    .SelectMany(ExtractConstants)
                    .ToList();

                // Get a potential specific cache key
                var specificCacheKey = methodParameters
                    .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(CacheKeyAttribute)))
                    .Select((x, index) => new
                    {
                        Index = index,
                        ParameterInfo = x
                    }).FirstOrDefault();

                var parameters = new List<string>();
                for (var i = 0; i <= extractedArguments.Count - 1; i++)
                {
                    // If there's a specific cache key, ignore all other arguments
                    if (specificCacheKey != null)
                    {
                        if (i < specificCacheKey.Index)
                            continue;
                        if (i > specificCacheKey.Index)
                            break;
                    }

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

                // Save details for next calls and return False
                _cachingMethodsSet.TryAdd(methodToCacheData, (cacheAttribute, cacheKey));
                return true;
            }
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

        private ResiliencePipeline<TResult> GetMethodResiliencePipeline<TResult>(MethodDetails methodDetails, LogLevel logLevel)
        {
            if (_resiliencePipelineMethodsSet.TryGetValue(methodDetails, out var resiliencePipelineObject) &&
                resiliencePipelineObject is ResiliencePipeline<TResult> resiliencePipeline)
                return resiliencePipeline;

            resiliencePipeline = ResiliencePipeline<TResult>.Empty;

            if (_lazyResiliencePipelineRegistry == null)
                return resiliencePipeline;

            var resiliencePipelineAttribute = GetMethodResiliencePipelineAttribute(methodDetails);
            if (resiliencePipelineAttribute != null)
            {
                var resiliencePipelineBuilder = new ResiliencePipelineBuilder<TResult>();
                foreach (var registryKey in resiliencePipelineAttribute.RegistryKeys)
                {
                    if (_lazyResiliencePipelineRegistry.Value.TryGetPipeline(registryKey, out var registeredResiliencePipeline))
                    {
                        _apizrOptions.Logger.Log(logLevel,
                            $"{methodDetails.MethodInfo.Name}: Found a resilience pipeline with key {registryKey}");

                        resiliencePipelineBuilder.AddPipeline(registeredResiliencePipeline);

                        _apizrOptions.Logger.Log(logLevel,
                            $"{methodDetails.MethodInfo.Name}: Resilience pipeline with key {registryKey} will be applied");
                    }
                    else
                        _apizrOptions.Logger.Log(logLevel,
                            $"{methodDetails.MethodInfo.Name}: No resilience pipeline found for key {registryKey}");
                }

                resiliencePipeline = resiliencePipelineBuilder.Build();
            }

            _resiliencePipelineMethodsSet.TryAdd(methodDetails, resiliencePipeline);

            return resiliencePipeline;
        }

        private ResiliencePipeline GetMethodResiliencePipeline(MethodDetails methodDetails, LogLevel logLevel)
        {
            if (_resiliencePipelineMethodsSet.TryGetValue(methodDetails, out var resiliencePipelineObject) &&
                resiliencePipelineObject is ResiliencePipeline resiliencePipeline)
                return resiliencePipeline;

            resiliencePipeline = ResiliencePipeline.Empty;

            if (_lazyResiliencePipelineRegistry == null)
                return resiliencePipeline;

            var resiliencePipelineAttribute = GetMethodResiliencePipelineAttribute(methodDetails);
            if (resiliencePipelineAttribute != null)
            {
                var resiliencePipelineBuilder = new ResiliencePipelineBuilder();
                foreach (var registryKey in resiliencePipelineAttribute.RegistryKeys)
                {
                    if (_lazyResiliencePipelineRegistry.Value.TryGetPipeline(registryKey, out var registeredResiliencePipeline))
                    {
                        _apizrOptions.Logger.Log(logLevel,
                            $"{methodDetails.MethodInfo.Name}: Found a resilience pipeline with key {registryKey}");

                        resiliencePipelineBuilder.AddPipeline(registeredResiliencePipeline);

                        _apizrOptions.Logger.Log(logLevel,
                            $"{methodDetails.MethodInfo.Name}: Resilience pipeline with key {registryKey} will be applied");
                    }
                    else
                        _apizrOptions.Logger.Log(logLevel,
                            $"{methodDetails.MethodInfo.Name}: No resilience pipeline found for key {registryKey}");
                }

                resiliencePipeline = resiliencePipelineBuilder.Build();
            }

            _resiliencePipelineMethodsSet.TryAdd(methodDetails, resiliencePipeline);

            return resiliencePipeline;
        }

        private ResiliencePipelineAttributeBase GetMethodResiliencePipelineAttribute(MethodDetails methodDetails)
        {
            if (methodDetails == null)
                return null;

            if (typeof(ICrudApi<,,,>).IsAssignableFromGenericType(_apizrOptions.WebApiType)) // Crud api method
            {
                var modelType = methodDetails.ApiInterfaceType.GetGenericArguments().First();
                ResiliencePipelineAttributeBase resiliencePipelineAttribute = methodDetails.MethodInfo.Name switch // Specific method policies
                {
                    "Create" => modelType.GetTypeInfo().GetCustomAttribute<CreateResiliencePipelineAttribute>(),
                    "ReadAll" => modelType.GetTypeInfo().GetCustomAttribute<ReadAllResiliencePipelineAttribute>(),
                    "Read" => modelType.GetTypeInfo().GetCustomAttribute<ReadResiliencePipelineAttribute>(),
                    "Update" => modelType.GetTypeInfo().GetCustomAttribute<UpdateResiliencePipelineAttribute>(),
                    "Delete" => modelType.GetTypeInfo().GetCustomAttribute<DeleteResiliencePipelineAttribute>(),
                    _ => null
                };

                return resiliencePipelineAttribute;
            }

            // Standard api method
            return methodDetails.MethodInfo.GetCustomAttribute<ResiliencePipelineAttribute>();
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
            _operationTimeoutMethodsSet.TryAdd(methodDetails, timeoutAttribute);
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
