// Largely inspired by Refit.Insane.PowerPack, but with more features

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Extending;
using Apizr.Logging;
using Apizr.Mapping;
using Apizr.Policing;
using Apizr.Requesting;
using Polly;
using Polly.Registry;
using Refit;

namespace Apizr
{
    public class ApizrManager<TWebApi> : IApizrManager<TWebApi>
    {
        private readonly Dictionary<MethodCacheDetails, MethodCacheAttributes> _cacheableMethodsSet;
        private readonly ILazyWebApi<TWebApi> _lazyWebApi;
        private readonly IConnectivityHandler _connectivityHandler;
        private readonly ICacheHandler _cacheHandler;
        private readonly ILogHandler _logHandler;
        private readonly IMappingHandler _mappingHandler;
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;
        private readonly string _webApiFriendlyName;
        private readonly IApizrOptions<TWebApi> _apizrOptions;

        public ApizrManager(ILazyWebApi<TWebApi> lazyWebApi, IConnectivityHandler connectivityHandler, ICacheHandler cacheHandler, ILogHandler logHandler, IMappingHandler mappingHandler, IReadOnlyPolicyRegistry<string> policyRegistry, IApizrOptions<TWebApi> apizrOptions)
        {
            _cacheableMethodsSet = new Dictionary<MethodCacheDetails, MethodCacheAttributes>();
            _lazyWebApi = lazyWebApi;
            _connectivityHandler = connectivityHandler;
            _cacheHandler = cacheHandler;
            _logHandler = logHandler;
            _policyRegistry = policyRegistry;
            _mappingHandler = mappingHandler;
            _webApiFriendlyName = typeof(TWebApi).GetFriendlyName();
            _apizrOptions = apizrOptions;
        }

        public TWebApi Api => _lazyWebApi.Value;

        public async Task ExecuteAsync(Expression<Func<TWebApi, Task>> executeApiMethod)
        {
            var methodCallExpression = GetMethodCallExpression(executeApiMethod);
            var methodName = $"{_webApiFriendlyName}.{methodCallExpression.Method.Name}";
            if(_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                _logHandler.Write($"Apizr - {methodName}: Calling method");

            try
            {
                if (_connectivityHandler is VoidConnectivityHandler && _apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Connectivity is not checked as you didn't provide any connectivity handler");
                else if (!_connectivityHandler.IsConnected() && _apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                {
                    _logHandler.Write($"Apizr - {methodName}: Connectivity check failed, throw {nameof(IOException)}");
                    throw new IOException("Connectivity check failed");
                }
                else if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Connectivity check succeed");

                var policy = GetMethodPolicy(executeApiMethod.Body as MethodCallExpression);
                if (policy != null)
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Executing request with some policies");

                    var pollyContext = new Context().WithLogHandler(_logHandler);
                    await policy.ExecuteAsync(ctx => executeApiMethod.Compile()(Api), pollyContext);
                }
                else
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Executing request without specific policies");

                    await executeApiMethod.Compile()(Api);
                }
            }
            catch (Exception e)
            {
                if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                {
                    _logHandler.Write($"Apizr - {methodName}: Request throwed an exception with message {e.Message}");
                    _logHandler.Write($"Apizr - {methodName}: Throwing an {nameof(ApizrException)} with InnerException");
                }

                throw new ApizrException(e);
            }
        }

        public async Task ExecuteAsync(Expression<Func<TWebApi, IMappingHandler, Task>> executeApiMethod)
        {
            var methodCallExpression = GetMethodCallExpression(executeApiMethod);
            var methodName = $"{_webApiFriendlyName}.{methodCallExpression.Method.Name}";
            if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                _logHandler.Write($"Apizr - {methodName}: Calling method");

            try
            {
                if (_connectivityHandler is VoidConnectivityHandler && _apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Connectivity is not checked as you didn't provide any connectivity handler");
                else if (!_connectivityHandler.IsConnected() && _apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                {
                    _logHandler.Write($"Apizr - {methodName}: Connectivity check failed, throw {nameof(IOException)}");
                    throw new IOException("Connectivity check failed");
                }
                else if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Connectivity check succeed");

                var policy = GetMethodPolicy(executeApiMethod.Body as MethodCallExpression);
                if (policy != null)
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Executing request with some policies");

                    var pollyContext = new Context().WithLogHandler(_logHandler);
                    await policy.ExecuteAsync(ctx => executeApiMethod.Compile()(Api, _mappingHandler), pollyContext);
                }
                else
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Executing request without specific policies");

                    await executeApiMethod.Compile()(Api, _mappingHandler);
                }
            }
            catch (Exception e)
            {
                if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                {
                    _logHandler.Write($"Apizr - {methodName}: Request throwed an exception with message {e.Message}");
                    _logHandler.Write($"Apizr - {methodName}: Throwing an {nameof(ApizrException)} with InnerException");
                }

                throw new ApizrException(e);
            }
        }

        public async Task ExecuteAsync(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod, CancellationToken cancellationToken)
        {
            var methodCallExpression = GetMethodCallExpression(executeApiMethod);
            var methodName = $"{_webApiFriendlyName}.{methodCallExpression.Method.Name}";
            if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                _logHandler.Write($"Apizr - {methodName}: Calling method");

            try
            {
                if (_connectivityHandler is VoidConnectivityHandler && _apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Connectivity is not checked as you didn't provide any connectivity handler");
                else if (!_connectivityHandler.IsConnected() && _apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                {
                    _logHandler.Write($"Apizr - {methodName}: Connectivity check failed, throw {nameof(IOException)}");
                    throw new IOException("Connectivity check failed");
                }
                else if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Connectivity check succeed");

                var policy = GetMethodPolicy(executeApiMethod.Body as MethodCallExpression);
                if (policy != null)
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Executing request with some policies");

                    var pollyContext = new Context().WithLogHandler(_logHandler);
                    await policy.ExecuteAsync((ctx, ct) => executeApiMethod.Compile()(ct, Api), pollyContext, cancellationToken);
                }
                else
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Executing request without specific policies");

                    await executeApiMethod.Compile()(cancellationToken, Api);
                }
            }
            catch (Exception e)
            {
                if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                {
                    _logHandler.Write($"Apizr - {methodName}: Request throwed an exception with message {e.Message}");
                    _logHandler.Write($"Apizr - {methodName}: Throwing an {nameof(ApizrException)} with InnerException");
                }

                throw new ApizrException(e);
            }
        }

        public async Task ExecuteAsync(Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task>> executeApiMethod, CancellationToken cancellationToken)
        {
            var methodCallExpression = GetMethodCallExpression(executeApiMethod);
            var methodName = $"{_webApiFriendlyName}.{methodCallExpression.Method.Name}";
            if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                _logHandler.Write($"Apizr - {methodName}: Calling method");

            try
            {
                if (_connectivityHandler is VoidConnectivityHandler && _apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Connectivity is not checked as you didn't provide any connectivity handler");
                else if (!_connectivityHandler.IsConnected() && _apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                {
                    _logHandler.Write($"Apizr - {methodName}: Connectivity check failed, throw {nameof(IOException)}");
                    throw new IOException("Connectivity check failed");
                }
                else if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Connectivity check succeed");

                var policy = GetMethodPolicy(executeApiMethod.Body as MethodCallExpression);
                if (policy != null)
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Executing request with some policies");

                    var pollyContext = new Context().WithLogHandler(_logHandler);
                    await policy.ExecuteAsync((ctx, ct) => executeApiMethod.Compile()(ct, Api, _mappingHandler), pollyContext, cancellationToken);
                }
                else
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Executing request without specific policies");

                    await executeApiMethod.Compile()(cancellationToken, Api, _mappingHandler);
                }
            }
            catch (Exception e)
            {
                if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                {
                    _logHandler.Write($"Apizr - {methodName}: Request throwed an exception with message {e.Message}");
                    _logHandler.Write($"Apizr - {methodName}: Throwing an {nameof(ApizrException)} with InnerException");
                }

                throw new ApizrException(e);
            }
        }

        public async Task<TResult> ExecuteAsync<TResult>(Expression<Func<TWebApi, Task<TResult>>> executeApiMethod)
        {
            var methodCallExpression = GetMethodCallExpression<TResult>(executeApiMethod);
            var methodName = $"{_webApiFriendlyName}.{methodCallExpression.Method.Name}";
            if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                _logHandler.Write($"Apizr - {methodName}: Calling method");

            TResult result = default;

            if (IsMethodCacheable<TResult>(executeApiMethod, out var cacheAttribute, out var cacheKey))
            {
                if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Called method is cacheable");
                if (_cacheHandler is VoidCacheHandler && _apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    _logHandler.Write($"Apizr - {methodName}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");
                if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Used cache key is {cacheKey}");

                result = await _cacheHandler.Get<TResult>(cacheKey);
                if (!Equals(result, default(TResult)) && _apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Some cached data found for this cache key");
            }

            if (result == null || cacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                try
                {
                    if (_connectivityHandler is VoidConnectivityHandler && _apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Connectivity is not checked as you didn't provide any connectivity handler");
                    else if (!_connectivityHandler.IsConnected() && _apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    {
                        _logHandler.Write($"Apizr - {methodName}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }
                    else if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Connectivity check succeed");

                    var policy = GetMethodPolicy<TResult>(executeApiMethod.Body as MethodCallExpression);
                    if (policy != null)
                    {
                        if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                            _logHandler.Write($"Apizr - {methodName}: Executing request with some policies");

                        var pollyContext = new Context().WithLogHandler(_logHandler);
                        result = await policy.ExecuteAsync(ctx => executeApiMethod.Compile()(Api), pollyContext);
                    }
                    else
                    {
                        if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                            _logHandler.Write($"Apizr - {methodName}: Executing request without specific policies");

                        result = await executeApiMethod.Compile()(Api);
                    }
                }
                catch (Exception e)
                {
                    if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    {
                        _logHandler.Write($"Apizr - {methodName}: Request throwed an exception with message {e.Message}");
                        _logHandler.Write(!Equals(result, default(TResult))
                            ? $"Apizr - {methodName}: Throwing an {nameof(ApizrException<TResult>)} with InnerException and cached result"
                            : $"Apizr - {methodName}: Throwing an {nameof(ApizrException<TResult>)} with InnerException and but no cached result");
                    }

                    throw new ApizrException<TResult>(e, result);
                }

                if (result != null && _cacheHandler != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Caching result");

                    await _cacheHandler.Set(cacheKey, result, cacheAttribute.LifeSpan);
                }
            }

            if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                _logHandler.Write($"Apizr - {methodName}: Returning result");

            return result;
        }

        public async Task<TResult> ExecuteAsync<TResult>(Expression<Func<TWebApi, IMappingHandler, Task<TResult>>> executeApiMethod)
        {
            var methodCallExpression = GetMethodCallExpression<TResult>(executeApiMethod);
            var methodName = $"{_webApiFriendlyName}.{methodCallExpression.Method.Name}";
            if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                _logHandler.Write($"Apizr - {methodName}: Calling method");

            TResult result = default;

            if (IsMethodCacheable<TResult>(executeApiMethod, out var cacheAttribute, out var cacheKey))
            {
                if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Called method is cacheable");
                if (_cacheHandler is VoidCacheHandler && _apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    _logHandler.Write($"Apizr - {methodName}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");
                if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Used cache key is {cacheKey}");

                result = await _cacheHandler.Get<TResult>(cacheKey);
                if (!Equals(result, default(TResult)) && _apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Some cached data found for this cache key");
            }

            if (result == null || cacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                try
                {
                    if (_connectivityHandler is VoidConnectivityHandler && _apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Connectivity is not checked as you didn't provide any connectivity handler");
                    else if (!_connectivityHandler.IsConnected() && _apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    {
                        _logHandler.Write($"Apizr - {methodName}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }
                    else if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Connectivity check succeed");

                    var policy = GetMethodPolicy<TResult>(executeApiMethod.Body as MethodCallExpression);
                    if (policy != null)
                    {
                        if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                            _logHandler.Write($"Apizr - {methodName}: Executing request with some policies");

                        var pollyContext = new Context().WithLogHandler(_logHandler);
                        result = await policy.ExecuteAsync(ctx => executeApiMethod.Compile()(Api, _mappingHandler), pollyContext);
                    }
                    else
                    {
                        if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                            _logHandler.Write($"Apizr - {methodName}: Executing request without specific policies");

                        result = await executeApiMethod.Compile()(Api, _mappingHandler);
                    }
                }
                catch (Exception e)
                {
                    if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    {
                        _logHandler.Write($"Apizr - {methodName}: Request throwed an exception with message {e.Message}");
                        _logHandler.Write(!Equals(result, default(TResult))
                            ? $"Apizr - {methodName}: Throwing an {nameof(ApizrException<TResult>)} with InnerException and cached result"
                            : $"Apizr - {methodName}: Throwing an {nameof(ApizrException<TResult>)} with InnerException and but no cached result");
                    }

                    throw new ApizrException<TResult>(e, result);
                }

                if (result != null && _cacheHandler != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Caching result");

                    await _cacheHandler.Set(cacheKey, result, cacheAttribute.LifeSpan);
                }
            }

            if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                _logHandler.Write($"Apizr - {methodName}: Returning result");

            return result;
        }

        public async Task<TResult> ExecuteAsync<TResult>(Expression<Func<CancellationToken, TWebApi, Task<TResult>>> executeApiMethod, CancellationToken cancellationToken)
        {
            var methodCallExpression = GetMethodCallExpression<TResult>(executeApiMethod);
            var methodName = $"{_webApiFriendlyName}.{methodCallExpression.Method.Name}";
            if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                _logHandler.Write($"Apizr - {methodName}: Calling method");

            TResult result = default;

            if (IsMethodCacheable<TResult>(executeApiMethod, out var cacheAttribute, out var cacheKey))
            {
                if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Called method is cacheable");
                if (_cacheHandler is VoidCacheHandler && _apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    _logHandler.Write($"Apizr - {methodName}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");
                if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Used cache key is {cacheKey}");

                result = await _cacheHandler.Get<TResult>(cacheKey, cancellationToken);
                if (!Equals(result, default(TResult)) && _apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Some cached data found for this cache key");
            }

            if (result == null || cacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                try
                {
                    if (_connectivityHandler is VoidConnectivityHandler && _apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Connectivity is not checked as you didn't provide any connectivity handler");
                    else if (!_connectivityHandler.IsConnected() && _apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    {
                        _logHandler.Write($"Apizr - {methodName}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }
                    else if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Connectivity check succeed");

                    var policy = GetMethodPolicy<TResult>(executeApiMethod.Body as MethodCallExpression);
                    if (policy != null)
                    {
                        if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                            _logHandler.Write($"Apizr - {methodName}: Executing request with some policies");

                        var pollyContext = new Context().WithLogHandler(_logHandler);
                        result = await policy.ExecuteAsync((ctx, ct) => executeApiMethod.Compile()(ct, Api), pollyContext, cancellationToken);
                    }
                    else
                    {
                        if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                            _logHandler.Write($"Apizr - {methodName}: Executing request without specific policies");

                        result = await executeApiMethod.Compile()(cancellationToken, Api);
                    }
                }
                catch (Exception e)
                {
                    if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    {
                        _logHandler.Write($"Apizr - {methodName}: Request throwed an exception with message {e.Message}");
                        _logHandler.Write(!Equals(result, default(TResult))
                            ? $"Apizr - {methodName}: Throwing an {nameof(ApizrException<TResult>)} with InnerException and cached result"
                            : $"Apizr - {methodName}: Throwing an {nameof(ApizrException<TResult>)} with InnerException and but no cached result");
                    }

                    throw new ApizrException<TResult>(e, result);
                }

                if (result != null && _cacheHandler != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Caching result");

                    await _cacheHandler.Set(cacheKey, result, cacheAttribute.LifeSpan, cancellationToken);
                }
            }

            if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                _logHandler.Write($"Apizr - {methodName}: Returning result");

            return result;
        }

        public async Task<TResult> ExecuteAsync<TResult>(Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TResult>>> executeApiMethod, CancellationToken cancellationToken)
        {
            var methodCallExpression = GetMethodCallExpression<TResult>(executeApiMethod);
            var methodName = $"{_webApiFriendlyName}.{methodCallExpression.Method.Name}";
            if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                _logHandler.Write($"Apizr - {methodName}: Calling method");

            TResult result = default;

            if (IsMethodCacheable<TResult>(executeApiMethod, out var cacheAttribute, out var cacheKey))
            {
                if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Called method is cacheable");
                if (_cacheHandler is VoidCacheHandler && _apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    _logHandler.Write($"Apizr - {methodName}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");
                if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Used cache key is {cacheKey}");

                result = await _cacheHandler.Get<TResult>(cacheKey, cancellationToken);
                if (!Equals(result, default(TResult)) && _apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Some cached data found for this cache key");
            }

            if (result == null || cacheAttribute?.Mode != CacheMode.GetOrFetch)
            {
                try
                {
                    if (_connectivityHandler is VoidConnectivityHandler && _apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Connectivity is not checked as you didn't provide any connectivity handler");
                    else if (!_connectivityHandler.IsConnected() && _apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    {
                        _logHandler.Write($"Apizr - {methodName}: Connectivity check failed, throw {nameof(IOException)}");
                        throw new IOException("Connectivity check failed");
                    }
                    else if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Connectivity check succeed");

                    var policy = GetMethodPolicy<TResult>(executeApiMethod.Body as MethodCallExpression);
                    if (policy != null)
                    {
                        if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                            _logHandler.Write($"Apizr - {methodName}: Executing request with some policies");

                        var pollyContext = new Context().WithLogHandler(_logHandler);
                        result = await policy.ExecuteAsync((ctx, ct) => executeApiMethod.Compile()(ct, Api, _mappingHandler), pollyContext, cancellationToken);
                    }
                    else
                    {
                        if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                            _logHandler.Write($"Apizr - {methodName}: Executing request without specific policies");

                        result = await executeApiMethod.Compile()(cancellationToken, Api, _mappingHandler);
                    }
                }
                catch (Exception e)
                {
                    if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    {
                        _logHandler.Write($"Apizr - {methodName}: Request throwed an exception with message {e.Message}");
                        _logHandler.Write(!Equals(result, default(TResult))
                            ? $"Apizr - {methodName}: Throwing an {nameof(ApizrException<TResult>)} with InnerException and cached result"
                            : $"Apizr - {methodName}: Throwing an {nameof(ApizrException<TResult>)} with InnerException and but no cached result");
                    }

                    throw new ApizrException<TResult>(e, result);
                }

                if (result != null && _cacheHandler != null && !string.IsNullOrWhiteSpace(cacheKey) &&
                    cacheAttribute != null && cacheAttribute.Mode != CacheMode.None)
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Caching result");

                    await _cacheHandler.Set(cacheKey, result, cacheAttribute.LifeSpan, cancellationToken);
                }
            }

            if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                _logHandler.Write($"Apizr - {methodName}: Returning result");

            return result;
        }

        public async Task<bool> ClearCacheAsync(CancellationToken cancellationToken = default)
        {
            if (_cacheHandler is VoidCacheHandler && _apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                _logHandler.Write($"Apizr: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

            try
            {
                await _cacheHandler.Clear(cancellationToken);
                if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    _logHandler.Write("Apizr: Cache cleared");

                return true;
            }
            catch (Exception e)
            {
                if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    _logHandler.Write($"Apizr: Clearing all cache throwed an exception with message: {e.Message}");
                return false;
            }
        }

        public Task<bool> ClearCacheAsync<TResult>(Expression<Func<TWebApi, Task<TResult>>> executeApiMethod)
            => ClearCacheAsync((ct, api) => executeApiMethod.Compile()(api), CancellationToken.None);

        public async Task<bool> ClearCacheAsync<TResult>(Expression<Func<CancellationToken, TWebApi, Task<TResult>>> executeApiMethod, CancellationToken cancellationToken)
        {
            var methodCallExpression = GetMethodCallExpression<TResult>(executeApiMethod);
            var methodName = $"{_webApiFriendlyName}.{methodCallExpression.Method.Name}";
            if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                _logHandler.Write($"Apizr: Calling cache clear for method {methodName}");

            if (_cacheHandler is VoidCacheHandler && _apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                _logHandler.Write($"Apizr - {methodName}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

            try
            {
                if (IsMethodCacheable<TResult>(executeApiMethod, out var cacheAttribute, out var cacheKey))
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Method is cacheable");
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Clearing cache for key {cacheKey}");

                    var success = await _cacheHandler.Remove(cacheKey, cancellationToken);
                    if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                        _logHandler.Write(success
                        ? $"Apizr - {methodName}: Clearing cache for key {cacheKey} succeed"
                        : $"Apizr - {methodName}: Clearing cache for key {cacheKey} failed");

                    return success;
                }

                if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    _logHandler.Write($"Apizr - {methodName}: Method isn't cacheable");
                return true;
            }
            catch (Exception e)
            {
                if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    _logHandler.Write($"Apizr - {methodName}: Clearing keyed cache throwed an exception with message: {e.Message}");
                return false;
            }
        }

        #region Caching

        private bool IsMethodCacheable<TResult>(Expression restExpression, out CacheAttributeBase cacheAttribute, out string cacheKey)
        {
            var methodToCacheDetails = GetMethodToCacheData<TResult>(restExpression);

            lock (this)
            {
                var methodToCacheData = methodToCacheDetails;
                cacheAttribute = null;
                cacheKey = null;

                if (typeof(ICrudApi<,,,>).IsAssignableFromGenericType(methodToCacheDetails.ApiInterfaceType)) // Crud api caching
                {
                    var modelType = methodToCacheDetails.ApiInterfaceType.GetGenericArguments().First();
                    var methodName = methodToCacheData.MethodInfo.Name;
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
                        cacheAttribute = modelType.GetTypeInfo().GetCustomAttribute<CacheItAttribute>(true);
                }
                else // Classic api caching
                {
                    cacheAttribute =
                        methodToCacheData.MethodInfo.GetCustomAttribute<CacheItAttribute>() ?? // Specific method caching
                        methodToCacheDetails.ApiInterfaceType.GetTypeInfo().GetCustomAttribute<CacheItAttribute>(); // Global api interface caching
                }

                if (cacheAttribute == null) // Global assembly caching
                    cacheAttribute = methodToCacheDetails.ApiInterfaceType.Assembly.GetCustomAttribute<CacheItAttribute>();

                // Are we asked to cache this method?
                if (cacheAttribute == null || cacheAttribute.Mode == CacheMode.None)
                {
                    // No we're not!
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

        private MethodCacheDetails GetMethodToCacheData<TResult>(Expression restExpression)
        {
            var webApiType = typeof(TWebApi);
            var methodCallExpression = GetMethodCallExpression<TResult>(restExpression);
            return new MethodCacheDetails(webApiType, methodCallExpression.Method);
        }

        private static IEnumerable<ExtractedConstant> ExtractConstants(Expression expression)
        {
            if (expression == null)
                yield break;

            if (expression is ConstantExpression constantsExpression)
                yield return new ExtractedConstant {Name = constantsExpression.Type.Name, Value = constantsExpression.Value};

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

                    yield return new ExtractedConstant { Name = memberExpression.Type.Name, Value = value };
                }
            }
            else if (expression is InvocationExpression invocationExpression)
            {
                foreach (var constants in ExtractConstants(invocationExpression.Expression))
                    yield return constants;
            }
            else if (expression is ParameterExpression parameterExpression)
            {
                if(parameterExpression.Type == typeof(CancellationToken))
                    yield return new ExtractedConstant { Name = parameterExpression.Type.Name, Value = CancellationToken.None };
                else
                    yield return new ExtractedConstant { Name = parameterExpression.Type.Name };
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

                    yield return new ExtractedConstant { Name = listInitExpression.Type.Name, Value = $"[{parameters.ToString(":", ", ")}]" };
                }
                else
                    yield return new ExtractedConstant { Name = listInitExpression.Type.Name };
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
                    yield return new ExtractedConstant { Name = memberInitExpression.Type.Name, Value = parameters.ToString(":", ", ") };
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
                    if(newExpression.Arguments[i] is ConstantExpression constantExpression)
                        parameters.Add(constructorParameters[i].Name, constantExpression.Value);
                }
                yield return new ExtractedConstant { Name = newExpression.Type.Name, Value = parameters.ToString(":", ", ") };
            }
            else
                throw new NotImplementedException();
        }

        private string GetCacheKey<TResult>(Expression restExpression)
        {
            var methodCallExpression = GetMethodCallExpression<TResult>(restExpression);

            var cacheKeyPrefix = $"{_webApiFriendlyName}.{methodCallExpression.Method.Name}";
            if (!methodCallExpression.Arguments.Any())
                return $"{cacheKeyPrefix}()";

            var cacheAttributes = GetCacheAttribute<TResult>(restExpression);
            if (string.IsNullOrWhiteSpace(cacheAttributes.ParameterName))
                return $"{cacheKeyPrefix}()";

            var parametersInfos = methodCallExpression.Method.GetParameters().Where(p =>
                p.CustomAttributes.Any(a => a.AttributeType == typeof(PropertyAttribute))).ToList();

            var extractedArguments = methodCallExpression.Arguments
                .SelectMany(ExtractConstants)
                .Where(x => x != null && x.Value is CancellationToken == false && parametersInfos.All(p => !string.Equals(p.Name, x.Name, StringComparison.CurrentCultureIgnoreCase)))
                .ToList();

            if (!extractedArguments.Any())
                return $"{cacheKeyPrefix}()";

            var primaryKeyName = cacheAttributes.ParameterName;
            object primaryKeyValue;
            var extractedArgument = extractedArguments[cacheAttributes.ParameterOrder];
            var extractedArgumentValue = extractedArgument.Value;

            if (extractedArgumentValue == null)
                return $"{cacheKeyPrefix}()";

            var isArgumentValuePrimitive = extractedArgumentValue.GetType().GetTypeInfo().IsPrimitive ||
                                          extractedArgumentValue is decimal ||
                                          extractedArgumentValue is string;

            if (isArgumentValuePrimitive)
            {
                primaryKeyValue = extractedArgument.Value;
            }
            else
            {
                var primaryKeyValueField = extractedArgumentValue.GetType().GetRuntimeFields().Select((x, i) => new
                {
                    Index = i,
                    Field = x
                }).First(x => x.Index == cacheAttributes.ParameterOrder);

                primaryKeyValue = primaryKeyValueField.Field.GetValue(extractedArgumentValue);
            }

            foreach (var argument in extractedArguments)
            {
                var primaryKeyCacheField = argument
                    .Value
                    .GetType()
                    .GetRuntimeFields()
                    .FirstOrDefault(x => x.Name.Equals(cacheAttributes.ParameterName));

                if (primaryKeyCacheField != null)
                {
                    primaryKeyValue = primaryKeyCacheField.GetValue(argument.Value);
                    break;
                }
            }

            if (primaryKeyValue == null)
                throw new InvalidOperationException($"No {nameof(CacheKeyAttribute)} primary key found for: " + cacheKeyPrefix);

            // Simple param value OR complex type with overriden ToString
            var value = primaryKeyValue.ToString();
            if (!string.IsNullOrWhiteSpace(value) && value != primaryKeyValue.GetType().ToString())
                return value.Contains(":")
                    ? $"{cacheKeyPrefix}({value})"
                    : $"{cacheKeyPrefix}({primaryKeyName}:{value})";
            
            // Dictionary param key values
            if (primaryKeyValue is IDictionary objectDictionary)
                return $"{cacheKeyPrefix}({objectDictionary.ToString(":", ", ")})";

            // Complex type param values without override
            var parameters = primaryKeyValue.ToString(":", ", ");
            if (!string.IsNullOrWhiteSpace(parameters))
                return $"{cacheKeyPrefix}({parameters})";

            // Default caching without param
            return $"{cacheKeyPrefix}()";
        }

        private MethodCacheAttributes GetCacheAttribute<TResult>(Expression expression)
        {
            lock (this)
            {
                var methodToCacheData = GetMethodToCacheData<TResult>(expression);
                return _cacheableMethodsSet[methodToCacheData];
            }
        }

        private MethodCallExpression GetMethodCallExpression(
            Expression expression)
        {
            switch (expression)
            {
                case Expression<Func<TWebApi, Task>> executeApiMethod:
                    return GetMethodCallExpression(executeApiMethod.Body);
                case Expression<Func<TWebApi, IMappingHandler, Task>> executeApiMethod:
                    return GetMethodCallExpression(executeApiMethod.Body);
                case Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod:
                    return GetMethodCallExpression(executeApiMethod.Body);
                case Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task>> executeApiMethod:
                    return GetMethodCallExpression(executeApiMethod.Body);
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
                case Expression<Func<TWebApi, Task<TResult>>> executeApiMethod:
                    return GetMethodCallExpression<TResult>(executeApiMethod.Body);
                case Expression<Func<TWebApi, IMappingHandler, Task<TResult>>> executeApiMethod:
                    return GetMethodCallExpression<TResult>(executeApiMethod.Body);
                case Expression<Func<CancellationToken, TWebApi, Task<TResult>>> executeApiMethod:
                    return GetMethodCallExpression<TResult>(executeApiMethod.Body);
                case Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TResult>>> executeApiMethod:
                    return GetMethodCallExpression<TResult>(executeApiMethod.Body);
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

            public string Name { get; set; }
        }

        #endregion

        #region Policing

        private IAsyncPolicy<TResult> GetMethodPolicy<TResult>(MethodCallExpression methodCallExpression)
        {
            if (methodCallExpression == null)
                return null;

            if (_policyRegistry == null)
                return null;

            PolicyAttributeBase policyAttribute = null;

            if (typeof(ICrudApi<,,,>).IsAssignableFromGenericType(_apizrOptions.WebApiType)) // Crud api caching
            {
                var modelType = _apizrOptions.WebApiType.GetGenericArguments().First();
                var methodName = methodCallExpression.Method.Name;
                switch (methodName) // Specific method policies
                {
                    case "Create":
                        policyAttribute = modelType.GetTypeInfo().GetCustomAttribute<CreatePolicyAttribute>();
                        break;
                    case "ReadAll":
                        policyAttribute = modelType.GetTypeInfo().GetCustomAttribute<ReadAllPolicyAttribute>();
                        break;
                    case "Read":
                        policyAttribute = modelType.GetTypeInfo().GetCustomAttribute<ReadPolicyAttribute>();
                        break;
                    case "Update":
                        policyAttribute = modelType.GetTypeInfo().GetCustomAttribute<UpdatePolicyAttribute>();
                        break;
                    case "Delete":
                        policyAttribute = modelType.GetTypeInfo().GetCustomAttribute<DeletePolicyAttribute>();
                        break;
                }
            }
            else // Global model policies
            {
                policyAttribute = methodCallExpression.Method.GetCustomAttribute<PolicyAttribute>();
            }

            if (policyAttribute == null)
                return null;

            var fullMethodName = $"{_webApiFriendlyName}.{methodCallExpression.Method.Name}";

            IAsyncPolicy<TResult> policy = null;
            foreach (var registryKey in policyAttribute.RegistryKeys)
            {
                if (_policyRegistry.TryGet<IsPolicy>(registryKey, out var registeredPolicy))
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {fullMethodName}: Found a policy with key {registryKey}");
                    if (registeredPolicy is IAsyncPolicy<TResult> registeredPolicyWithResult)
                    {
                        if (policy == null)
                            policy = registeredPolicyWithResult;
                        else
                            policy.WrapAsync(registeredPolicyWithResult);

                        if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                            _logHandler.Write($"Apizr - {fullMethodName}: Policy with key {registryKey} will be applied");
                    }
                    else if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    {
                        _logHandler.Write($"Apizr - {fullMethodName}: Policy with key {registryKey} is not of {typeof(IAsyncPolicy<TResult>)} type and will be ignored");
                    }
                }
                else if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                {
                    _logHandler.Write($"Apizr - {fullMethodName}: No policy found for key {registryKey}");
                }
            }

            return policy;
        }

        private IAsyncPolicy GetMethodPolicy(MethodCallExpression methodCallExpression)
        {
            if (methodCallExpression == null)
                return null;

            if (_policyRegistry == null)
                return null;

            PolicyAttributeBase policyAttribute = null;

            if (typeof(ICrudApi<,,,>).IsAssignableFromGenericType(_apizrOptions.WebApiType)) // Crud api caching
            {
                var modelType = _apizrOptions.WebApiType.GetGenericArguments().First();
                var methodName = methodCallExpression.Method.Name;
                switch (methodName) // Specific method policies
                {
                    case "Create":
                        policyAttribute = modelType.GetTypeInfo().GetCustomAttribute<CreatePolicyAttribute>();
                        break;
                    case "ReadAll":
                        policyAttribute = modelType.GetTypeInfo().GetCustomAttribute<ReadAllPolicyAttribute>();
                        break;
                    case "Read":
                        policyAttribute = modelType.GetTypeInfo().GetCustomAttribute<ReadPolicyAttribute>();
                        break;
                    case "Update":
                        policyAttribute = modelType.GetTypeInfo().GetCustomAttribute<UpdatePolicyAttribute>();
                        break;
                    case "Delete":
                        policyAttribute = modelType.GetTypeInfo().GetCustomAttribute<DeletePolicyAttribute>();
                        break;
                }
            }
            else // Global model policies
            {
                policyAttribute = methodCallExpression.Method.GetCustomAttribute<PolicyAttribute>();
            }

            var fullMethodName = $"{_webApiFriendlyName}.{methodCallExpression.Method.Name}";

            IAsyncPolicy policy = null;
            foreach (var registryKey in policyAttribute.RegistryKeys)
            {
                if (_policyRegistry.TryGet<IsPolicy>(registryKey, out var registeredPolicy))
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {fullMethodName}: Found a policy with key {registryKey}");
                    if (registeredPolicy is IAsyncPolicy registeredPolicyWithoutResult)
                    {
                        if (policy == null)
                            policy = registeredPolicyWithoutResult;
                        else
                            policy.WrapAsync(registeredPolicyWithoutResult);

                        if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                            _logHandler.Write($"Apizr - {fullMethodName}: Policy with key {registryKey} will be applied");
                    }
                    else if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    {
                        _logHandler.Write($"Apizr - {fullMethodName}: Policy with key {registryKey} is not of {typeof(IAsyncPolicy)} type and will be ignored");
                    }
                }
                else if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                {
                    _logHandler.Write($"Apizr - {fullMethodName}: No policy found for key {registryKey}");
                }
            }

            return policy;
        }

        #endregion
    }
}