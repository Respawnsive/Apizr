// Largely inspired by Refit.Insane.PowerPack, but with more features

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Caching;
using Apizr.Caching.Attributes;
using Apizr.Configuring;
using Apizr.Connecting;
using Apizr.Extending;
using Apizr.Logging;
using Apizr.Logging.Attributes;
using Apizr.Mapping;
using Apizr.Policing;
using Apizr.Requesting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.NoOp;
using Polly.Registry;
using Refit;

namespace Apizr
{
    public class ApizrManager<TWebApi> : IApizrManager<TWebApi>
    {
        private readonly ILazyFactory<TWebApi> _lazyWebApi;
        private readonly IConnectivityHandler _connectivityHandler;
        private readonly ICacheHandler _cacheHandler;
        private readonly IMappingHandler _mappingHandler;
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;
        private readonly string _webApiFriendlyName;
        private readonly IApizrOptions<TWebApi> _apizrOptions;

        private readonly Dictionary<MethodDetails, (CacheAttributeBase cacheAttribute, string cacheKey)> _cachingMethodsSet;
        private readonly Dictionary<MethodDetails, LogAttributeBase> _loggingMethodsSet;
        private readonly Dictionary<MethodDetails, IsPolicy> _policingMethodsSet;

        public ApizrManager(ILazyFactory<TWebApi> lazyWebApi, IConnectivityHandler connectivityHandler, ICacheHandler cacheHandler, IMappingHandler mappingHandler, IReadOnlyPolicyRegistry<string> policyRegistry, IApizrOptions<TWebApi> apizrOptions)
        {
            _lazyWebApi = lazyWebApi;
            _connectivityHandler = connectivityHandler;
            _cacheHandler = cacheHandler;
            _policyRegistry = policyRegistry;
            _mappingHandler = mappingHandler;
            _webApiFriendlyName = typeof(TWebApi).GetFriendlyName();
            _apizrOptions = apizrOptions;

            _cachingMethodsSet = new Dictionary<MethodDetails, (CacheAttributeBase cacheAttribute, string cacheKey)>();
            _loggingMethodsSet = new Dictionary<MethodDetails, LogAttributeBase>();
            _policingMethodsSet = new Dictionary<MethodDetails, IsPolicy>();
        }

        #region Implementation

        public TWebApi Api => _lazyWebApi.Value;

        public IApizrOptionsBase Options => _apizrOptions;

        #region ExecuteAsync

        #region Task

        public async Task ExecuteAsync(Expression<Func<TWebApi, Task>> executeApiMethod)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            try
            {
                if (!_connectivityHandler.IsConnected())
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                    throw new IOException("Connectivity check failed");
                }

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                var policy = GetMethodPolicy(methodDetails, logAttribute.LogLevel);
                if (!(policy is INoOpPolicy))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                var pollyContext = new Context(methodDetails.MethodInfo.Name).WithLogger(_apizrOptions.Logger,
                    logAttribute.LogLevel, logAttribute.TrafficVerbosity, logAttribute.HttpTracerMode);
                await policy.ExecuteAsync(ctx => executeApiMethod.Compile()(webApi), pollyContext);
            }
            catch (Exception e)
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException)} with InnerException");

                throw new ApizrException(e);
            }
        }

        public async Task ExecuteAsync<TModelData, TApiData>(Expression<Func<TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            try
            {
                if (!_connectivityHandler.IsConnected())
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                    throw new IOException("Connectivity check failed");
                }

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                var policy = GetMethodPolicy(methodDetails, logAttribute.LogLevel);
                if (!(policy is INoOpPolicy))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                var apiData = Map<TModelData, TApiData>(modelData);
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: {typeof(TModelData).Name} mapped to {typeof(TApiData).Name}");

                var pollyContext = new Context(methodDetails.MethodInfo.Name).WithLogger(_apizrOptions.Logger,
                    logAttribute.LogLevel, logAttribute.TrafficVerbosity, logAttribute.HttpTracerMode);
                await policy.ExecuteAsync(ctx => executeApiMethod.Compile()(webApi, apiData), pollyContext);
            }
            catch (Exception e)
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException)} with InnerException");

                throw new ApizrException(e);
            }
        }

        public async Task ExecuteAsync(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod,
            CancellationToken cancellationToken = default)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            try
            {
                if (!_connectivityHandler.IsConnected())
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                    throw new IOException("Connectivity check failed");
                }

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                var policy = GetMethodPolicy(methodDetails, logAttribute.LogLevel);
                if (!(policy is INoOpPolicy))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                var pollyContext = new Context(methodDetails.MethodInfo.Name).WithLogger(_apizrOptions.Logger,
                    logAttribute.LogLevel, logAttribute.TrafficVerbosity, logAttribute.HttpTracerMode);
                await policy.ExecuteAsync((ctx, ct) => executeApiMethod.Compile()(ct, webApi), pollyContext,
                    cancellationToken);
            }
            catch (Exception e)
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException)} with InnerException");

                throw new ApizrException(e);
            }
        }

        public async Task ExecuteAsync(Expression<Func<Context, TWebApi, Task>> executeApiMethod,
            Context context = null)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            try
            {
                if (!_connectivityHandler.IsConnected())
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                    throw new IOException("Connectivity check failed");
                }

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                var policy = GetMethodPolicy(methodDetails, logAttribute.LogLevel);
                if (!(policy is INoOpPolicy))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                var pollyContext = new Context(methodDetails.MethodInfo.Name, context ?? new Context());
                pollyContext.WithLogger(_apizrOptions.Logger, logAttribute.LogLevel, logAttribute.TrafficVerbosity,
                    logAttribute.HttpTracerMode);
                await policy.ExecuteAsync(ctx => executeApiMethod.Compile()(ctx, webApi), pollyContext);
            }
            catch (Exception e)
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException)} with InnerException");

                throw new ApizrException(e);
            }
        }

        public async Task ExecuteAsync<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            CancellationToken cancellationToken = default)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            try
            {
                if (!_connectivityHandler.IsConnected())
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                    throw new IOException("Connectivity check failed");
                }

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                var policy = GetMethodPolicy(methodDetails, logAttribute.LogLevel);
                if (!(policy is INoOpPolicy))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                var apiData = Map<TModelData, TApiData>(modelData);
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: {typeof(TModelData).Name} mapped to {typeof(TApiData).Name}");

                var pollyContext = new Context(methodDetails.MethodInfo.Name).WithLogger(_apizrOptions.Logger,
                    logAttribute.LogLevel, logAttribute.TrafficVerbosity, logAttribute.HttpTracerMode);
                await policy.ExecuteAsync((ctx, ct) => executeApiMethod.Compile()(ct, webApi, apiData), pollyContext,
                    cancellationToken);
            }
            catch (Exception e)
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException)} with InnerException");

                throw new ApizrException(e);
            }
        }

        public async Task ExecuteAsync<TModelData, TApiData>(
            Expression<Func<Context, TWebApi, TApiData, Task>> executeApiMethod, TModelData modelData,
            Context context = null)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            try
            {
                if (!_connectivityHandler.IsConnected())
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                    throw new IOException("Connectivity check failed");
                }

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                var policy = GetMethodPolicy(methodDetails, logAttribute.LogLevel);
                if (!(policy is INoOpPolicy))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                var apiData = Map<TModelData, TApiData>(modelData);
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: {typeof(TModelData).Name} mapped to {typeof(TApiData).Name}");

                var pollyContext = new Context(methodDetails.MethodInfo.Name, context ?? new Context());
                pollyContext.WithLogger(_apizrOptions.Logger, logAttribute.LogLevel, logAttribute.TrafficVerbosity,
                    logAttribute.HttpTracerMode);
                await policy.ExecuteAsync(ctx => executeApiMethod.Compile()(ctx, webApi, apiData), pollyContext);
            }
            catch (Exception e)
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException)} with InnerException");

                throw new ApizrException(e);
            }
        }

        public async Task ExecuteAsync(Expression<Func<Context, CancellationToken, TWebApi, Task>> executeApiMethod,
            Context context = null, CancellationToken cancellationToken = default)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            try
            {
                if (!_connectivityHandler.IsConnected())
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                    throw new IOException("Connectivity check failed");
                }

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                var policy = GetMethodPolicy(methodDetails, logAttribute.LogLevel);
                if (!(policy is INoOpPolicy))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                var pollyContext = new Context(methodDetails.MethodInfo.Name, context ?? new Context());
                pollyContext.WithLogger(_apizrOptions.Logger, logAttribute.LogLevel, logAttribute.TrafficVerbosity,
                    logAttribute.HttpTracerMode);
                await policy.ExecuteAsync((ctx, ct) => executeApiMethod.Compile()(ctx, ct, webApi), pollyContext,
                    cancellationToken);
            }
            catch (Exception e)
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException)} with InnerException");

                throw new ApizrException(e);
            }
        }

        public async Task ExecuteAsync<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task>> executeApiMethod,
            TModelData modelData, Context context = null,
            CancellationToken cancellationToken = default)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            try
            {
                if (!_connectivityHandler.IsConnected())
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                    throw new IOException("Connectivity check failed");
                }

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                var policy = GetMethodPolicy(methodDetails, logAttribute.LogLevel);
                if (!(policy is INoOpPolicy))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                var apiData = Map<TModelData, TApiData>(modelData);
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: {typeof(TModelData).Name} mapped to {typeof(TApiData).Name}");

                var pollyContext = new Context(methodDetails.MethodInfo.Name, context ?? new Context());
                pollyContext.WithLogger(_apizrOptions.Logger, logAttribute.LogLevel, logAttribute.TrafficVerbosity,
                    logAttribute.HttpTracerMode);
                await policy.ExecuteAsync((ctx, ct) => executeApiMethod.Compile()(ctx, ct, webApi, apiData),
                    pollyContext, cancellationToken);
            }
            catch (Exception e)
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException)} with InnerException");

                throw new ApizrException(e);
            }
        }

        #endregion

        #region Task<T>

        public async Task<TResult> ExecuteAsync<TResult>(Expression<Func<TWebApi, Task<TResult>>> executeApiMethod)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails<TResult>(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            TResult result = default;

            if (IsMethodCacheable<TResult>(methodDetails, executeApiMethod, out var cacheAttribute, out var cacheKey))
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                result = await _cacheHandler.GetAsync<TResult>(cacheKey);
                if (!Equals(result, default(TResult)))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");
            }

            if (result == null || cacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                try
                {
                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }
                    else
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var policy = GetMethodPolicy<TResult>(methodDetails, logAttribute.LogLevel);
                    if (!(policy is INoOpPolicy))
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                    var pollyContext = new Context(methodDetails.MethodInfo.Name).WithLogger(_apizrOptions.Logger,
                        logAttribute.LogLevel, logAttribute.TrafficVerbosity, logAttribute.HttpTracerMode);
                    result = await policy.ExecuteAsync(ctx => executeApiMethod.Compile()(webApi), pollyContext);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, !Equals(result, default(TResult))
                        ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TResult>)} with InnerException and cached result"
                        : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TResult>)} with InnerException and but no cached result");

                    throw new ApizrException<TResult>(e, result);
                }

                if (result != null && _cacheHandler != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, result, cacheAttribute.LifeSpan);
                }
            }

            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Returning result");

            return result;
        }

        public async Task<TModelResultData> ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(Expression<Func<TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails<TApiResultData>(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiResultData result = default;

            if (IsMethodCacheable<TApiResultData>(methodDetails, executeApiMethod, out var cacheAttribute,
                out var cacheKey))
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                result = await _cacheHandler.GetAsync<TApiResultData>(cacheKey);
                if (!Equals(result, default(TApiResultData)))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");
            }

            if (result == null || cacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                try
                {
                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }
                    else
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var policy = GetMethodPolicy<TApiResultData>(methodDetails, logAttribute.LogLevel);
                    if (!(policy is INoOpPolicy))
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                    var apiRequestData = Map<TModelRequestData, TApiRequestData>(modelRequestData);
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: {typeof(TModelRequestData).Name} mapped to {typeof(TApiRequestData).Name}");

                    var pollyContext = new Context(methodDetails.MethodInfo.Name).WithLogger(_apizrOptions.Logger,
                        logAttribute.LogLevel, logAttribute.TrafficVerbosity, logAttribute.HttpTracerMode);
                    result = await policy.ExecuteAsync(ctx => executeApiMethod.Compile()(webApi, apiRequestData),
                        pollyContext);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, !Equals(result, default(TApiResultData))
                        ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelResultData>)} with InnerException and cached result"
                        : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelResultData>)} with InnerException and but no cached result");

                    throw new ApizrException<TModelResultData>(e, Map<TApiResultData, TModelResultData>(result));
                }

                if (result != null && _cacheHandler != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, result, cacheAttribute.LifeSpan);
                }
            }

            _apizrOptions.Logger.Log(logAttribute.LogLevel,
                $"{methodDetails.MethodInfo.Name}: Returning mapped result from {typeof(TApiResultData).Name} to {typeof(TModelResultData).Name}");

            return Map<TApiResultData, TModelResultData>(result);
        }

        public async Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails<TApiData>(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiData result = default;

            if (IsMethodCacheable<TApiData>(methodDetails, executeApiMethod, out var cacheAttribute,
                out var cacheKey))
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                result = await _cacheHandler.GetAsync<TApiData>(cacheKey);
                if (!Equals(result, default(TApiData)))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");
            }

            if (result == null || cacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                try
                {
                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }
                    else
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var policy = GetMethodPolicy<TApiData>(methodDetails, logAttribute.LogLevel);
                    if (!(policy is INoOpPolicy))
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                    var apiData = Map<TModelData, TApiData>(modelData);
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: {typeof(TModelData).Name} mapped to {typeof(TApiData).Name}");

                    var pollyContext = new Context(methodDetails.MethodInfo.Name).WithLogger(_apizrOptions.Logger,
                        logAttribute.LogLevel, logAttribute.TrafficVerbosity, logAttribute.HttpTracerMode);
                    result = await policy.ExecuteAsync(ctx => executeApiMethod.Compile()(webApi, apiData),
                        pollyContext);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, !Equals(result, default(TApiData))
                        ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelData>)} with InnerException and cached result"
                        : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelData>)} with InnerException and but no cached result");

                    throw new ApizrException<TModelData>(e, Map<TApiData, TModelData>(result));
                }

                if (result != null && _cacheHandler != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, result, cacheAttribute.LifeSpan);
                }
            }

            _apizrOptions.Logger.Log(logAttribute.LogLevel,
                $"{methodDetails.MethodInfo.Name}: Returning mapped result from {typeof(TApiData).Name} to {typeof(TModelData).Name}");

            return Map<TApiData, TModelData>(result);
        }

        public async Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<TWebApi, Task<TApiData>>> executeApiMethod)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails<TApiData>(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiData result = default;

            if (IsMethodCacheable<TApiData>(methodDetails, executeApiMethod, out var cacheAttribute,
                out var cacheKey))
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                result = await _cacheHandler.GetAsync<TApiData>(cacheKey);
                if (!Equals(result, default(TApiData)))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");
            }

            if (result == null || cacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                try
                {
                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }
                    else
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var policy = GetMethodPolicy<TApiData>(methodDetails, logAttribute.LogLevel);
                    if (!(policy is INoOpPolicy))
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                    var pollyContext = new Context(methodDetails.MethodInfo.Name).WithLogger(_apizrOptions.Logger,
                        logAttribute.LogLevel, logAttribute.TrafficVerbosity, logAttribute.HttpTracerMode);
                    result = await policy.ExecuteAsync(ctx => executeApiMethod.Compile()(webApi), pollyContext);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, !Equals(result, default(TApiData))
                        ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelData>)} with InnerException and cached result"
                        : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelData>)} with InnerException and but no cached result");

                    throw new ApizrException<TModelData>(e, Map<TApiData, TModelData>(result));
                }

                if (result != null && _cacheHandler != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, result, cacheAttribute.LifeSpan);
                }
            }

            _apizrOptions.Logger.Log(logAttribute.LogLevel,
                $"{methodDetails.MethodInfo.Name}: Returning mapped result from {typeof(TApiData).Name} to {typeof(TModelData).Name}");

            return Map<TApiData, TModelData>(result);
        }

        public async Task<TResult> ExecuteAsync<TResult>(
            Expression<Func<CancellationToken, TWebApi, Task<TResult>>> executeApiMethod,
            CancellationToken cancellationToken = default)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails<TResult>(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            TResult result = default;

            if (IsMethodCacheable<TResult>(methodDetails, executeApiMethod, out var cacheAttribute, out var cacheKey))
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                result = await _cacheHandler.GetAsync<TResult>(cacheKey, cancellationToken);
                if (!Equals(result, default(TResult)))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");
            }

            if (result == null || cacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                try
                {
                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }
                    else
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var policy = GetMethodPolicy<TResult>(methodDetails, logAttribute.LogLevel);
                    if (!(policy is INoOpPolicy))
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                    var pollyContext = new Context(methodDetails.MethodInfo.Name).WithLogger(_apizrOptions.Logger,
                        logAttribute.LogLevel, logAttribute.TrafficVerbosity, logAttribute.HttpTracerMode);
                    result = await policy.ExecuteAsync((ctx, ct) => executeApiMethod.Compile()(ct, webApi),
                        pollyContext, cancellationToken);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, !Equals(result, default(TResult))
                        ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TResult>)} with InnerException and cached result"
                        : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TResult>)} with InnerException and but no cached result");

                    throw new ApizrException<TResult>(e, result);
                }

                if (result != null && _cacheHandler != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, result, cacheAttribute.LifeSpan, cancellationToken);
                }
            }

            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Returning result");

            return result;
        }

        public async Task<TResult> ExecuteAsync<TResult>(
            Expression<Func<Context, TWebApi, Task<TResult>>> executeApiMethod, Context context = null)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails<TResult>(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            TResult result = default;

            if (IsMethodCacheable<TResult>(methodDetails, executeApiMethod, out var cacheAttribute, out var cacheKey))
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                result = await _cacheHandler.GetAsync<TResult>(cacheKey);
                if (!Equals(result, default(TResult)))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");
            }

            if (result == null || cacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                try
                {
                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }
                    else
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var policy = GetMethodPolicy<TResult>(methodDetails, logAttribute.LogLevel);
                    if (!(policy is INoOpPolicy))
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                    var pollyContext = new Context(methodDetails.MethodInfo.Name, context ?? new Context());
                    pollyContext.WithLogger(_apizrOptions.Logger, logAttribute.LogLevel, logAttribute.TrafficVerbosity,
                        logAttribute.HttpTracerMode);
                    result = await policy.ExecuteAsync(ctx => executeApiMethod.Compile()(ctx, webApi), pollyContext);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, !Equals(result, default(TResult))
                        ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TResult>)} with InnerException and cached result"
                        : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TResult>)} with InnerException and but no cached result");

                    throw new ApizrException<TResult>(e, result);
                }

                if (result != null && _cacheHandler != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, result, cacheAttribute.LifeSpan);
                }
            }

            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Returning result");

            return result;
        }

        public async Task<TModelResultData> ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            Expression<Func<CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, CancellationToken cancellationToken = default)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails<TApiResultData>(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiResultData result = default;

            if (IsMethodCacheable<TApiResultData>(methodDetails, executeApiMethod, out var cacheAttribute,
                out var cacheKey))
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                result = await _cacheHandler.GetAsync<TApiResultData>(cacheKey, cancellationToken);
                if (!Equals(result, default(TApiResultData)))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");
            }

            if (result == null || cacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                try
                {
                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }
                    else
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var policy = GetMethodPolicy<TApiResultData>(methodDetails, logAttribute.LogLevel);
                    if (!(policy is INoOpPolicy))
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                    var apiRequestData = Map<TModelRequestData, TApiRequestData>(modelRequestData);
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: {typeof(TModelRequestData).Name} mapped to {typeof(TApiRequestData).Name}");

                    var pollyContext = new Context(methodDetails.MethodInfo.Name).WithLogger(_apizrOptions.Logger,
                        logAttribute.LogLevel, logAttribute.TrafficVerbosity, logAttribute.HttpTracerMode);
                    result = await policy.ExecuteAsync(
                        (ctx, ct) => executeApiMethod.Compile()(ct, webApi, apiRequestData), pollyContext,
                        cancellationToken);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, !Equals(result, default(TApiResultData))
                        ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelResultData>)} with InnerException and cached result"
                        : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelResultData>)} with InnerException and but no cached result");

                    throw new ApizrException<TModelResultData>(e, Map<TApiResultData, TModelResultData>(result));
                }

                if (result != null && _cacheHandler != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, result, cacheAttribute.LifeSpan, cancellationToken);
                }
            }

            _apizrOptions.Logger.Log(logAttribute.LogLevel,
                $"{methodDetails.MethodInfo.Name}: Returning mapped result from {typeof(TApiResultData).Name} to {typeof(TModelResultData).Name}");

            return Map<TApiResultData, TModelResultData>(result);
        }

        public async Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData,
            CancellationToken cancellationToken = default)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails<TApiData>(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiData result = default;

            if (IsMethodCacheable<TApiData>(methodDetails, executeApiMethod, out var cacheAttribute,
                out var cacheKey))
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                result = await _cacheHandler.GetAsync<TApiData>(cacheKey, cancellationToken);
                if (!Equals(result, default(TApiData)))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");
            }

            if (result == null || cacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                try
                {
                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }
                    else
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var policy = GetMethodPolicy<TApiData>(methodDetails, logAttribute.LogLevel);
                    if (!(policy is INoOpPolicy))
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                    var apiData = Map<TModelData, TApiData>(modelData);
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: {typeof(TModelData).Name} mapped to {typeof(TApiData).Name}");

                    var pollyContext = new Context(methodDetails.MethodInfo.Name).WithLogger(_apizrOptions.Logger,
                        logAttribute.LogLevel, logAttribute.TrafficVerbosity, logAttribute.HttpTracerMode);
                    result = await policy.ExecuteAsync(
                        (ctx, ct) => executeApiMethod.Compile()(ct, webApi, apiData), pollyContext,
                        cancellationToken);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, !Equals(result, default(TApiData))
                        ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelData>)} with InnerException and cached result"
                        : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelData>)} with InnerException and but no cached result");

                    throw new ApizrException<TModelData>(e, Map<TApiData, TModelData>(result));
                }

                if (result != null && _cacheHandler != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, result, cacheAttribute.LifeSpan, cancellationToken);
                }
            }

            _apizrOptions.Logger.Log(logAttribute.LogLevel,
                $"{methodDetails.MethodInfo.Name}: Returning mapped result from {typeof(TApiData).Name} to {typeof(TModelData).Name}");

            return Map<TApiData, TModelData>(result);
        }

        public async Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            CancellationToken cancellationToken = default)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails<TApiData>(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiData result = default;

            if (IsMethodCacheable<TApiData>(methodDetails, executeApiMethod, out var cacheAttribute,
                out var cacheKey))
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                result = await _cacheHandler.GetAsync<TApiData>(cacheKey, cancellationToken);
                if (!Equals(result, default(TApiData)))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");
            }

            if (result == null || cacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                try
                {
                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }
                    else
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var policy = GetMethodPolicy<TApiData>(methodDetails, logAttribute.LogLevel);
                    if (!(policy is INoOpPolicy))
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                    var pollyContext = new Context(methodDetails.MethodInfo.Name).WithLogger(_apizrOptions.Logger,
                        logAttribute.LogLevel, logAttribute.TrafficVerbosity, logAttribute.HttpTracerMode);
                    result = await policy.ExecuteAsync(
                        (ctx, ct) => executeApiMethod.Compile()(ct, webApi), pollyContext,
                        cancellationToken);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, !Equals(result, default(TApiData))
                        ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelData>)} with InnerException and cached result"
                        : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelData>)} with InnerException and but no cached result");

                    throw new ApizrException<TModelData>(e, Map<TApiData, TModelData>(result));
                }

                if (result != null && _cacheHandler != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, result, cacheAttribute.LifeSpan, cancellationToken);
                }
            }

            _apizrOptions.Logger.Log(logAttribute.LogLevel,
                $"{methodDetails.MethodInfo.Name}: Returning mapped result from {typeof(TApiData).Name} to {typeof(TModelData).Name}");

            return Map<TApiData, TModelData>(result);
        }

        public async Task<TModelResultData> ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            Expression<Func<Context, TWebApi, TApiRequestData, Task<TApiResultData>>> executeApiMethod,
            TModelRequestData modelRequestData, Context context = null)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails<TApiResultData>(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiResultData result = default;

            if (IsMethodCacheable<TApiResultData>(methodDetails, executeApiMethod, out var cacheAttribute,
                out var cacheKey))
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                result = await _cacheHandler.GetAsync<TApiResultData>(cacheKey);
                if (!Equals(result, default(TApiResultData)))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");
            }

            if (result == null || cacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                try
                {
                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }
                    else
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var policy = GetMethodPolicy<TApiResultData>(methodDetails, logAttribute.LogLevel);
                    if (!(policy is INoOpPolicy))
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                    var apiRequestData = Map<TModelRequestData, TApiRequestData>(modelRequestData);
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: {typeof(TModelRequestData).Name} mapped to {typeof(TApiRequestData).Name}");

                    var pollyContext = new Context(methodDetails.MethodInfo.Name, context ?? new Context());
                    pollyContext.WithLogger(_apizrOptions.Logger, logAttribute.LogLevel, logAttribute.TrafficVerbosity,
                        logAttribute.HttpTracerMode);
                    result = await policy.ExecuteAsync(ctx => executeApiMethod.Compile()(ctx, webApi, apiRequestData),
                        pollyContext);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, !Equals(result, default(TApiResultData))
                        ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelResultData>)} with InnerException and cached result"
                        : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelResultData>)} with InnerException and but no cached result");

                    throw new ApizrException<TModelResultData>(e, Map<TApiResultData, TModelResultData>(result));
                }

                if (result != null && _cacheHandler != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, result, cacheAttribute.LifeSpan);
                }
            }

            _apizrOptions.Logger.Log(logAttribute.LogLevel,
                $"{methodDetails.MethodInfo.Name}: Returning mapped result from {typeof(TApiResultData).Name} to {typeof(TModelResultData).Name}");

            return Map<TApiResultData, TModelResultData>(result);
        }

        public async Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<Context, TWebApi, TApiData, Task<TApiData>>> executeApiMethod, TModelData modelData,
            Context context = null)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails<TApiData>(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiData result = default;

            if (IsMethodCacheable<TApiData>(methodDetails, executeApiMethod, out var cacheAttribute,
                out var cacheKey))
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                result = await _cacheHandler.GetAsync<TApiData>(cacheKey);
                if (!Equals(result, default(TApiData)))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");
            }

            if (result == null || cacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                try
                {
                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }
                    else
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var policy = GetMethodPolicy<TApiData>(methodDetails, logAttribute.LogLevel);
                    if (!(policy is INoOpPolicy))
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                    var apiData = Map<TModelData, TApiData>(modelData);
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: {typeof(TModelData).Name} mapped to {typeof(TApiData).Name}");

                    var pollyContext = new Context(methodDetails.MethodInfo.Name, context ?? new Context());
                    pollyContext.WithLogger(_apizrOptions.Logger, logAttribute.LogLevel, logAttribute.TrafficVerbosity,
                        logAttribute.HttpTracerMode);
                    result = await policy.ExecuteAsync(ctx => executeApiMethod.Compile()(ctx, webApi, apiData),
                        pollyContext);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, !Equals(result, default(TApiData))
                        ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelData>)} with InnerException and cached result"
                        : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelData>)} with InnerException and but no cached result");

                    throw new ApizrException<TModelData>(e, Map<TApiData, TModelData>(result));
                }

                if (result != null && _cacheHandler != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, result, cacheAttribute.LifeSpan);
                }
            }

            _apizrOptions.Logger.Log(logAttribute.LogLevel,
                $"{methodDetails.MethodInfo.Name}: Returning mapped result from {typeof(TApiData).Name} to {typeof(TModelData).Name}");

            return Map<TApiData, TModelData>(result);
        }

        public async Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<Context, TWebApi, Task<TApiData>>> executeApiMethod, Context context = null)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails<TApiData>(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiData result = default;

            if (IsMethodCacheable<TApiData>(methodDetails, executeApiMethod, out var cacheAttribute,
                out var cacheKey))
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                result = await _cacheHandler.GetAsync<TApiData>(cacheKey);
                if (!Equals(result, default(TApiData)))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");
            }

            if (result == null || cacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                try
                {
                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }
                    else
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var policy = GetMethodPolicy<TApiData>(methodDetails, logAttribute.LogLevel);
                    if (!(policy is INoOpPolicy))
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                    var pollyContext = new Context(methodDetails.MethodInfo.Name, context ?? new Context());
                    pollyContext.WithLogger(_apizrOptions.Logger, logAttribute.LogLevel, logAttribute.TrafficVerbosity,
                        logAttribute.HttpTracerMode);
                    result = await policy.ExecuteAsync(ctx => executeApiMethod.Compile()(ctx, webApi),
                        pollyContext);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, !Equals(result, default(TApiData))
                        ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelData>)} with InnerException and cached result"
                        : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelData>)} with InnerException and but no cached result");

                    throw new ApizrException<TModelData>(e, Map<TApiData, TModelData>(result));
                }

                if (result != null && _cacheHandler != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, result, cacheAttribute.LifeSpan);
                }
            }

            _apizrOptions.Logger.Log(logAttribute.LogLevel,
                $"{methodDetails.MethodInfo.Name}: Returning mapped result from {typeof(TApiData).Name} to {typeof(TModelData).Name}");

            return Map<TApiData, TModelData>(result);
        }

        public async Task<TResult> ExecuteAsync<TResult>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TResult>>> executeApiMethod,
            Context context = null, CancellationToken cancellationToken = default)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails<TResult>(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            TResult result = default;

            if (IsMethodCacheable<TResult>(methodDetails, executeApiMethod, out var cacheAttribute, out var cacheKey))
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                result = await _cacheHandler.GetAsync<TResult>(cacheKey, cancellationToken);
                if (!Equals(result, default(TResult)))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");
            }

            if (result == null || cacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                try
                {
                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }
                    else
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var policy = GetMethodPolicy<TResult>(methodDetails, logAttribute.LogLevel);
                    if (!(policy is INoOpPolicy))
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                    var pollyContext = new Context(methodDetails.MethodInfo.Name, context ?? new Context());
                    pollyContext.WithLogger(_apizrOptions.Logger, logAttribute.LogLevel, logAttribute.TrafficVerbosity,
                        logAttribute.HttpTracerMode);
                    result = await policy.ExecuteAsync((ctx, ct) => executeApiMethod.Compile()(ctx, ct, webApi),
                        pollyContext, cancellationToken);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, !Equals(result, default(TResult))
                        ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TResult>)} with InnerException and cached result"
                        : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TResult>)} with InnerException and but no cached result");

                    throw new ApizrException<TResult>(e, result);
                }

                if (result != null && _cacheHandler != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, result, cacheAttribute.LifeSpan, cancellationToken);
                }
            }

            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Returning result");

            return result;
        }

        public async Task<TModelResultData> ExecuteAsync<TModelResultData, TApiResultData, TApiRequestData,
            TModelRequestData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiRequestData, Task<TApiResultData>>>
                executeApiMethod,
            TModelRequestData modelRequestData, Context context = null, CancellationToken cancellationToken = default)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails<TApiResultData>(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiResultData result = default;

            if (IsMethodCacheable<TApiResultData>(methodDetails, executeApiMethod, out var cacheAttribute,
                out var cacheKey))
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                result = await _cacheHandler.GetAsync<TApiResultData>(cacheKey, cancellationToken);
                if (!Equals(result, default(TApiResultData)))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");
            }

            if (result == null || cacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                try
                {
                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }
                    else
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var policy = GetMethodPolicy<TApiResultData>(methodDetails, logAttribute.LogLevel);
                    if (!(policy is INoOpPolicy))
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                    var apiRequestData = Map<TModelRequestData, TApiRequestData>(modelRequestData);
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: {typeof(TModelRequestData).Name} mapped to {typeof(TApiRequestData).Name}");

                    var pollyContext = new Context(methodDetails.MethodInfo.Name, context ?? new Context());
                    pollyContext.WithLogger(_apizrOptions.Logger, logAttribute.LogLevel, logAttribute.TrafficVerbosity,
                        logAttribute.HttpTracerMode);
                    result = await policy.ExecuteAsync(
                        (ctx, ct) => executeApiMethod.Compile()(ctx, ct, webApi, apiRequestData), pollyContext,
                        cancellationToken);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, !Equals(result, default(TApiResultData))
                        ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelResultData>)} with InnerException and cached result"
                        : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelResultData>)} with InnerException and but no cached result");

                    throw new ApizrException<TModelResultData>(e, Map<TApiResultData, TModelResultData>(result));
                }

                if (result != null && _cacheHandler != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, result, cacheAttribute.LifeSpan, cancellationToken);
                }
            }

            _apizrOptions.Logger.Log(logAttribute.LogLevel,
                $"{methodDetails.MethodInfo.Name}: Returning mapped result from {typeof(TApiResultData).Name} to {typeof(TModelResultData).Name}");

            return Map<TApiResultData, TModelResultData>(result);
        }

        public async Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, TApiData, Task<TApiData>>> executeApiMethod,
            TModelData modelData, Context context = null,
            CancellationToken cancellationToken = default)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails<TApiData>(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiData result = default;

            if (IsMethodCacheable<TApiData>(methodDetails, executeApiMethod, out var cacheAttribute,
                out var cacheKey))
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                result = await _cacheHandler.GetAsync<TApiData>(cacheKey, cancellationToken);
                if (!Equals(result, default(TApiData)))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");
            }

            if (result == null || cacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                try
                {
                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }
                    else
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var policy = GetMethodPolicy<TApiData>(methodDetails, logAttribute.LogLevel);
                    if (!(policy is INoOpPolicy))
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                    var apiData = Map<TModelData, TApiData>(modelData);
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: {typeof(TModelData).Name} mapped to {typeof(TApiData).Name}");

                    var pollyContext = new Context(methodDetails.MethodInfo.Name, context ?? new Context());
                    pollyContext.WithLogger(_apizrOptions.Logger, logAttribute.LogLevel, logAttribute.TrafficVerbosity,
                        logAttribute.HttpTracerMode);
                    result = await policy.ExecuteAsync(
                        (ctx, ct) => executeApiMethod.Compile()(ctx, ct, webApi, apiData), pollyContext,
                        cancellationToken);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, !Equals(result, default(TApiData))
                        ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelData>)} with InnerException and cached result"
                        : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelData>)} with InnerException and but no cached result");

                    throw new ApizrException<TModelData>(e, Map<TApiData, TModelData>(result));
                }

                if (result != null && _cacheHandler != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, result, cacheAttribute.LifeSpan, cancellationToken);
                }
            }

            _apizrOptions.Logger.Log(logAttribute.LogLevel,
                $"{methodDetails.MethodInfo.Name}: Returning mapped result from {typeof(TApiData).Name} to {typeof(TModelData).Name}");

            return Map<TApiData, TModelData>(result);
        }

        public async Task<TModelData> ExecuteAsync<TModelData, TApiData>(
            Expression<Func<Context, CancellationToken, TWebApi, Task<TApiData>>> executeApiMethod,
            Context context = null,
            CancellationToken cancellationToken = default)
        {
            var webApi = _lazyWebApi.Value;
            var methodDetails = GetMethodDetails<TApiData>(executeApiMethod);
            var logAttribute = GetLogAttribute(methodDetails);
            _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Calling method");

            TApiData result = default;

            if (IsMethodCacheable<TApiData>(methodDetails, executeApiMethod, out var cacheAttribute,
                out var cacheKey))
            {
                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Called method is cacheable");

                if (_cacheHandler is VoidCacheHandler)
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                _apizrOptions.Logger.Log(logAttribute.LogLevel,
                    $"{methodDetails.MethodInfo.Name}: Used cache key is {cacheKey}");

                result = await _cacheHandler.GetAsync<TApiData>(cacheKey, cancellationToken);
                if (!Equals(result, default(TApiData)))
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Some cached data found for this cache key");
            }

            if (result == null || cacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                try
                {
                    if (!_connectivityHandler.IsConnected())
                    {
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }
                    else
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Connectivity check succeed");

                    var policy = GetMethodPolicy<TApiData>(methodDetails, logAttribute.LogLevel);
                    if (!(policy is INoOpPolicy))
                        _apizrOptions.Logger.Log(logAttribute.LogLevel,
                            $"{methodDetails.MethodInfo.Name}: Executing request with some policies");

                    var pollyContext = new Context(methodDetails.MethodInfo.Name, context ?? new Context());
                    pollyContext.WithLogger(_apizrOptions.Logger, logAttribute.LogLevel, logAttribute.TrafficVerbosity,
                        logAttribute.HttpTracerMode);
                    result = await policy.ExecuteAsync(
                        (ctx, ct) => executeApiMethod.Compile()(ctx, ct, webApi), pollyContext,
                        cancellationToken);
                }
                catch (Exception e)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel,
                        $"{methodDetails.MethodInfo.Name}: Request throwed an exception with message {e.Message}");
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, !Equals(result, default(TApiData))
                        ? $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelData>)} with InnerException and cached result"
                        : $"{methodDetails.MethodInfo.Name}: Throwing an {nameof(ApizrException<TModelData>)} with InnerException and but no cached result");

                    throw new ApizrException<TModelData>(e, Map<TApiData, TModelData>(result));
                }

                if (result != null && _cacheHandler != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    _apizrOptions.Logger.Log(logAttribute.LogLevel, $"{methodDetails.MethodInfo.Name}: Caching result");

                    await _cacheHandler.SetAsync(cacheKey, result, cacheAttribute.LifeSpan, cancellationToken);
                }
            }

            _apizrOptions.Logger.Log(logAttribute.LogLevel,
                $"{methodDetails.MethodInfo.Name}: Returning mapped result from {typeof(TApiData).Name} to {typeof(TModelData).Name}");

            return Map<TApiData, TModelData>(result);
        }
        
        #endregion

        #endregion

        #region ClearCacheAsync

        public async Task<bool> ClearCacheAsync(CancellationToken cancellationToken = default)
        {
            if (_cacheHandler is VoidCacheHandler)
                _apizrOptions.Logger.Log(_apizrOptions.LogLevel, $"Apizr: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

            try
            {
                await _cacheHandler.ClearAsync(cancellationToken);
                _apizrOptions.Logger.Log(_apizrOptions.LogLevel, "Apizr: Cache cleared");

                return true;
            }
            catch (Exception e)
            {
                _apizrOptions.Logger.Log(_apizrOptions.LogLevel, $"Apizr: Clearing all cache throwed an exception with message: {e.Message}");

                return false;
            }
        }

        public Task<bool> ClearCacheAsync<TResult>(Expression<Func<TWebApi, Task<TResult>>> executeApiMethod)
            => ClearCacheAsync((ct, api) => executeApiMethod.Compile()(api), CancellationToken.None);

        public async Task<bool> ClearCacheAsync<TResult>(Expression<Func<CancellationToken, TWebApi, Task<TResult>>> executeApiMethod, CancellationToken cancellationToken = default)
        {
            var methodCallExpression = GetMethodCallExpression<TResult>(executeApiMethod);
            _apizrOptions.Logger.Log(_apizrOptions.LogLevel, $"Apizr: Calling cache clear for method {methodCallExpression.Method.Name}");

            if (_cacheHandler is VoidCacheHandler)
                _apizrOptions.Logger.Log(_apizrOptions.LogLevel, $"{methodCallExpression.Method.Name}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

            try
            {
                var methodDetails = GetMethodDetails<TResult>(executeApiMethod);
                if (IsMethodCacheable<TResult>(methodDetails, executeApiMethod, out _, out var cacheKey))
                {
                    _apizrOptions.Logger.Log(_apizrOptions.LogLevel, $"{methodCallExpression.Method.Name}: Method is cacheable");
                    _apizrOptions.Logger.Log(_apizrOptions.LogLevel, $"{methodCallExpression.Method.Name}: Clearing cache for key {cacheKey}");

                    var success = await _cacheHandler.RemoveAsync(cacheKey, cancellationToken);
                    _apizrOptions.Logger.Log(_apizrOptions.LogLevel, success
                        ? $"{methodCallExpression.Method.Name}: Clearing cache for key {cacheKey} succeed"
                        : $"{methodCallExpression.Method.Name}: Clearing cache for key {cacheKey} failed");

                    return success;
                }

                _apizrOptions.Logger.Log(_apizrOptions.LogLevel, $"{methodCallExpression.Method.Name}: Method isn't cacheable");

                return true;
            }
            catch (Exception e)
            {
                _apizrOptions.Logger.Log(_apizrOptions.LogLevel, $"{methodCallExpression.Method.Name}: Clearing keyed cache throwed an exception with message: {e.Message}");

                return false;
            }
        }  

        #endregion

        #endregion

        #region Logging

        private LogAttributeBase GetLogAttribute(MethodDetails methodDetails)
        {
            if (_loggingMethodsSet.TryGetValue(methodDetails, out var logAttribute))
                return logAttribute;
            
            if (typeof(ICrudApi<,,,>).IsAssignableFromGenericType(methodDetails.ApiInterfaceType)) // Crud api logging
            {
                var modelType = methodDetails.ApiInterfaceType.GetGenericArguments().First();
                var methodName = methodDetails.MethodInfo.Name;
                switch (methodName) // Specific method logging
                {
                    case "ReadAll":
                        logAttribute = modelType.GetTypeInfo().GetCustomAttribute<LogReadAllAttribute>(true);
                        break;
                    case "Read":
                        logAttribute = modelType.GetTypeInfo().GetCustomAttribute<LogReadAttribute>(true);
                        break;
                    case "Create":
                        logAttribute = modelType.GetTypeInfo().GetCustomAttribute<LogCreateAttribute>(true);
                        break;
                    case "Update":
                        logAttribute = modelType.GetTypeInfo().GetCustomAttribute<LogUpdateAttribute>(true);
                        break;
                    case "Delete":
                        logAttribute = modelType.GetTypeInfo().GetCustomAttribute<LogDeleteAttribute>(true);
                        break;
                }

                if (logAttribute == null) // Global model logging
                    logAttribute = modelType.GetTypeInfo().GetCustomAttribute<LogAttribute>(true);
            }
            else // Classic api logging
            {
                logAttribute =
                    methodDetails.MethodInfo.GetCustomAttribute<LogAttribute>() ?? // Specific method logging
                    methodDetails.ApiInterfaceType.GetTypeInfo().GetCustomAttribute<LogAttribute>(); // Global api interface logging (by attribute decoration)

                if (logAttribute == null && _apizrOptions.LogLevel != LogLevel.None) // Global api interface logging (by fluent configuration)
                    logAttribute = new LogAttribute(_apizrOptions.TrafficVerbosity, _apizrOptions.HttpTracerMode, _apizrOptions.LogLevel);
            }

            if (logAttribute == null) // Global assembly caching
                logAttribute = methodDetails.ApiInterfaceType.Assembly.GetCustomAttribute<LogAttribute>() ??
                               new LogAttribute(HttpMessageParts.None, HttpTracerMode.ExceptionsOnly, LogLevel.None);

            // Return log attribute
            _loggingMethodsSet.Add(methodDetails, logAttribute);
            return logAttribute;
        }

        #endregion

        #region Caching

        private bool IsMethodCacheable<TResult>(MethodDetails methodDetails, Expression restExpression, out CacheAttributeBase cacheAttribute, out string cacheKey)
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

                if (typeof(ICrudApi<,,,>).IsAssignableFromGenericType(methodDetails.ApiInterfaceType)) // Crud api caching
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

                    if(cacheAttribute == null) // Global model caching
                        cacheAttribute = modelType.GetTypeInfo().GetCustomAttribute<CacheAttribute>(true);
                }
                else // Classic api caching
                {
                    cacheAttribute =
                        methodToCacheData.MethodInfo.GetCustomAttribute<CacheAttribute>() ?? // Specific method caching
                        methodDetails.ApiInterfaceType.GetTypeInfo().GetCustomAttribute<CacheAttribute>(); // Global api interface caching
                }

                if (cacheAttribute == null) // Global assembly caching
                    cacheAttribute = methodDetails.ApiInterfaceType.Assembly.GetCustomAttribute<CacheAttribute>();

                // Are we asked to cache this method?
                if (cacheAttribute == null || cacheAttribute.Mode == CacheMode.None)
                {
                    // No we're not! Save details for next calls and return False
                    _cachingMethodsSet.Add(methodToCacheData, (cacheAttribute, cacheKey));
                    return false;
                }

                // Method is cacheable so prepare cache key
                var methodCallExpression = GetMethodCallExpression<TResult>(restExpression);
                cacheKey = $"{_webApiFriendlyName}.{methodCallExpression.Method.Name}(";

                // Get all method parameters
                var methodParameters = methodToCacheData.MethodInfo.GetParameters().ToList();

                // Is there any parameters except potential CancellationToken and Refit properties ?
                if (!methodParameters.Any(x => !typeof(CancellationToken).GetTypeInfo().IsAssignableFrom(x.ParameterType.GetTypeInfo()) &&
                                               x.CustomAttributes.All(y => !typeof(PropertyAttribute).GetTypeInfo().IsAssignableFrom(y.AttributeType.GetTypeInfo()))))
                {
                    // No there isn't!
                    cacheKey += ")"; 

                    // Save details for next calls and return False
                    _cachingMethodsSet.Add(methodToCacheData, (cacheAttribute, cacheKey));
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
                for (var i = 0; i <= extractedArguments.Count-1; i++)
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
                    if(typeof(CancellationToken).GetTypeInfo().IsAssignableFrom(parameterInfo.ParameterType.GetTypeInfo()) 
                       || parameterInfo.CustomAttributes.Any(x => typeof(PropertyAttribute).GetTypeInfo().IsAssignableFrom(x.AttributeType.GetTypeInfo())))
                        continue;

                    var parameterName = parameterInfo.Name;
                    var extractedArgument = extractedArguments[i];
                    var extractedArgumentValue = extractedArgument.Value;
                    if(extractedArgumentValue == null)
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
                        var cacheKeyAttribute = specificCacheKey?.ParameterInfo.GetCustomAttribute<CacheKeyAttribute>(true);
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
                _cachingMethodsSet.Add(methodToCacheData, (cacheAttribute, cacheKey));
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

        #region Policing

        private IAsyncPolicy<TResult> GetMethodPolicy<TResult>(MethodDetails methodDetails, LogLevel logLevel)
        {
            if (_policingMethodsSet.TryGetValue(methodDetails, out var foundPolicy) &&
                foundPolicy is IAsyncPolicy<TResult> policy)
                return policy;

            policy = Policy.NoOpAsync<TResult>();

            if (_policyRegistry == null)
                return policy;

            var policyAttribute = GetMethodPolicyAttribute(methodDetails);
            if (policyAttribute != null)
            {
                foreach (var registryKey in policyAttribute.RegistryKeys)
                {
                    if (_policyRegistry.TryGet<IsPolicy>(registryKey, out var registeredPolicy))
                    {
                        _apizrOptions.Logger.Log(logLevel, $"{methodDetails.MethodInfo.Name}: Found a policy with key {registryKey}");

                        if (registeredPolicy is IAsyncPolicy<TResult> registeredPolicyWithResult)
                        {
                            if (policy is INoOpPolicy)
                                policy = registeredPolicyWithResult;
                            else
                                policy.WrapAsync(registeredPolicyWithResult);

                            _apizrOptions.Logger.Log(logLevel, $"{methodDetails.MethodInfo.Name}: Policy with key {registryKey} will be applied");
                        }
                        else
                            _apizrOptions.Logger.Log(logLevel, $"{methodDetails.MethodInfo.Name}: Policy with key {registryKey} is not of {typeof(IAsyncPolicy<TResult>)} type and will be ignored");
                    }
                    else
                        _apizrOptions.Logger.Log(logLevel, $"{methodDetails.MethodInfo.Name}: No policy found for key {registryKey}");
                } 
            }

            _policingMethodsSet.Add(methodDetails, policy);
            return policy;
        }

        private IAsyncPolicy GetMethodPolicy(MethodDetails methodDetails, LogLevel logLevel)
        {
            if (_policingMethodsSet.TryGetValue(methodDetails, out var foundPolicy) &&
                foundPolicy is IAsyncPolicy policy)
                return policy;

            policy = Policy.NoOpAsync();
            
            if (_policyRegistry == null)
                return policy;

            var policyAttribute = GetMethodPolicyAttribute(methodDetails);
            if (policyAttribute != null)
            {
                foreach (var registryKey in policyAttribute.RegistryKeys)
                {
                    if (_policyRegistry.TryGet<IsPolicy>(registryKey, out var registeredPolicy))
                    {
                        _apizrOptions.Logger.Log(logLevel, $"{methodDetails.MethodInfo.Name}: Found a policy with key {registryKey}");

                        if (registeredPolicy is IAsyncPolicy registeredPolicyWithoutResult)
                        {
                            if (policy == null)
                                policy = registeredPolicyWithoutResult;
                            else
                                policy.WrapAsync(registeredPolicyWithoutResult);

                            _apizrOptions.Logger.Log(logLevel, $"{methodDetails.MethodInfo.Name}: Policy with key {registryKey} will be applied");
                        }
                        else
                            _apizrOptions.Logger.Log(logLevel, $"{methodDetails.MethodInfo.Name}: Policy with key {registryKey} is not of {typeof(IAsyncPolicy)} type and will be ignored");
                    }
                    else
                        _apizrOptions.Logger.Log(logLevel, $"{methodDetails.MethodInfo.Name}: No policy found for key {registryKey}");
                } 
            }

            _policingMethodsSet.Add(methodDetails, policy);
            return policy;
        }

        private PolicyAttributeBase GetMethodPolicyAttribute(MethodDetails methodDetails)
        {
            if (methodDetails == null)
                return null;

            if (typeof(ICrudApi<,,,>).IsAssignableFromGenericType(_apizrOptions.WebApiType)) // Crud api method
            {
                var modelType = _apizrOptions.WebApiType.GetGenericArguments().First();
                switch (methodDetails.MethodInfo.Name) // Specific method policies
                {
                    case "Create":
                        return modelType.GetTypeInfo().GetCustomAttribute<CreatePolicyAttribute>();
                    case "ReadAll":
                        return modelType.GetTypeInfo().GetCustomAttribute<ReadAllPolicyAttribute>();
                    case "Read":
                        return modelType.GetTypeInfo().GetCustomAttribute<ReadPolicyAttribute>();
                    case "Update":
                        return modelType.GetTypeInfo().GetCustomAttribute<UpdatePolicyAttribute>();
                    case "Delete":
                        return modelType.GetTypeInfo().GetCustomAttribute<DeletePolicyAttribute>();
                }
            }
            
            // Standard api method
            return methodDetails.MethodInfo.GetCustomAttribute<PolicyAttribute>();
        }

        #endregion

        #region Mapping

        private TDestination Map<TSource, TDestination>(TSource source)
        {
            TDestination destination = default;
            try
            {
                if (typeof(TSource) == typeof(TDestination))
                    destination = (TDestination)Convert.ChangeType(source, typeof(TDestination));
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
                yield return new ExtractedConstant { Value = constantsExpression.Value };

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
                    switch (memberExpression.Member)
                    {
                        case FieldInfo fieldInfo:
                            value = fieldInfo.GetValue(constants.Value);
                            break;
                        case PropertyInfo propertyInfo:
                            value = propertyInfo.GetValue(constants.Value);
                            break;
                    }

                    yield return new ExtractedConstant { Value = value };
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
                    yield return new ExtractedConstant { Value = CancellationToken.None };
                else
                    yield return new ExtractedConstant { };
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

                    yield return new ExtractedConstant { Value = $"[{parameters.ToString(":", ", ")}]" };
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
                    yield return new ExtractedConstant { Value = parameters.ToString(":", ", ") };
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
                yield return new ExtractedConstant { Value = parameters.ToString(":", ", ") };
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
                        var methodCallExpression = (MethodCallExpression)methodInvocationBody.Expression;
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
                        var methodCallExpression = (MethodCallExpression)methodInvocationBody.Expression;
                        return methodCallExpression;
                    }
                case MethodCallExpression methodCallExpression:
                    return methodCallExpression;
                default:
                    throw new NotImplementedException();
            }
        }

        private class ExtractedConstant
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

            public override int GetHashCode() => ApiInterfaceType.GetHashCode() * 23 * MethodInfo.GetHashCode() * 23 * 29;

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