// Largely inspired by Refit.Insane.PowerPack, but with more features

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reactive;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Extending;
using Apizr.Logging;
using Apizr.Mapping;
using Apizr.Policing;
using Apizr.Prioritizing;
using Fusillade;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using Polly.Registry;

namespace Apizr
{
    public class ApizrManager<TWebApi> : IApizrManager<TWebApi>
    {
        private readonly Dictionary<MethodCacheDetails, MethodCacheAttributes> _cacheableMethodsSet;
        private readonly IEnumerable<ILazyPrioritizedWebApi<TWebApi>> _webApis;
        private readonly IConnectivityHandler _connectivityHandler;
        private readonly ICacheHandler _cacheHandler;
        private readonly ILogHandler _logHandler;
        private readonly IMappingHandler _mappingHandler;
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;
        private readonly string _webApiFriendlyName;
        private readonly IApizrOptionsBase _apizrOptions;

        public ApizrManager(IEnumerable<ILazyPrioritizedWebApi<TWebApi>> webApis, IConnectivityHandler connectivityHandler, ICacheHandler cacheHandler, ILogHandler logHandler, IMappingHandler mappingHandler, IReadOnlyPolicyRegistry<string> policyRegistry, IApizrOptionsBase apizrOptions)
        {
            _cacheableMethodsSet = new Dictionary<MethodCacheDetails, MethodCacheAttributes>();
            _webApis = webApis;
            _connectivityHandler = connectivityHandler;
            _cacheHandler = cacheHandler;
            _logHandler = logHandler;
            _policyRegistry = policyRegistry;
            _mappingHandler = mappingHandler;
            _webApiFriendlyName = typeof(TWebApi).GetFriendlyName();
            _apizrOptions = apizrOptions;
        }

        TWebApi GetWebApi(Priority priority) => _webApis.First(x => x.Priority == priority || x.Priority == Priority.UserInitiated).Value;
        
        public async Task ExecuteAsync(Expression<Func<TWebApi, Task>> executeApiMethod,
            Priority priority = Priority.UserInitiated)
        {
            var webApi = GetWebApi(priority);
            var methodCallExpression = GetMethodCallExpression<Unit>(executeApiMethod);
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
                        _logHandler.Write($"Apizr - {methodName}: Executing {priority} request with some policies");

                    var pollyContext = new Context().WithLogHandler(_logHandler);
                    await policy.ExecuteAsync(ctx => executeApiMethod.Compile()(webApi), pollyContext);
                }
                else
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Executing {priority} request without specific policies");

                    await executeApiMethod.Compile()(webApi);
                }
            }
            catch (Exception e)
            {
                if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                {
                    _logHandler.Write($"Apizr - {methodName}: Request throwed an exception with message {e.Message}");
                    _logHandler.Write($"Apizr - {methodName}: Throwing an {nameof(ApizrException)} with InnerException");
                }

                throw new ApizrException(e, Unit.Default);
            }
        }

        public async Task ExecuteAsync(Expression<Func<TWebApi, IMappingHandler, Task>> executeApiMethod, Priority priority = Priority.UserInitiated)
        {
            var webApi = GetWebApi(priority);
            var methodCallExpression = GetMethodCallExpression<Unit>(executeApiMethod);
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
                        _logHandler.Write($"Apizr - {methodName}: Executing {priority} request with some policies");

                    var pollyContext = new Context().WithLogHandler(_logHandler);
                    await policy.ExecuteAsync(ctx => executeApiMethod.Compile()(webApi, _mappingHandler), pollyContext);
                }
                else
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Executing {priority} request without specific policies");

                    await executeApiMethod.Compile()(webApi, _mappingHandler);
                }
            }
            catch (Exception e)
            {
                if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                {
                    _logHandler.Write($"Apizr - {methodName}: Request throwed an exception with message {e.Message}");
                    _logHandler.Write($"Apizr - {methodName}: Throwing an {nameof(ApizrException)} with InnerException");
                }

                throw new ApizrException(e, Unit.Default);
            }
        }

        public async Task ExecuteAsync(Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod, CancellationToken cancellationToken,
            Priority priority = Priority.UserInitiated)
        {
            var webApi = GetWebApi(priority);
            var methodCallExpression = GetMethodCallExpression<Unit>(executeApiMethod);
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
                        _logHandler.Write($"Apizr - {methodName}: Executing {priority} request with some policies");

                    var pollyContext = new Context().WithLogHandler(_logHandler);
                    await policy.ExecuteAsync((ctx, ct) => executeApiMethod.Compile()(ct, webApi), pollyContext, cancellationToken);
                }
                else
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Executing {priority} request without specific policies");

                    await executeApiMethod.Compile()(cancellationToken, webApi);
                }
            }
            catch (Exception e)
            {
                if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                {
                    _logHandler.Write($"Apizr - {methodName}: Request throwed an exception with message {e.Message}");
                    _logHandler.Write($"Apizr - {methodName}: Throwing an {nameof(ApizrException)} with InnerException");
                }

                throw new ApizrException(e, Unit.Default);
            }
        }

        public async Task ExecuteAsync(Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task>> executeApiMethod, CancellationToken cancellationToken,
            Priority priority = Priority.UserInitiated)
        {
            var webApi = GetWebApi(priority);
            var methodCallExpression = GetMethodCallExpression<Unit>(executeApiMethod);
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
                        _logHandler.Write($"Apizr - {methodName}: Executing {priority} request with some policies");

                    var pollyContext = new Context().WithLogHandler(_logHandler);
                    await policy.ExecuteAsync((ctx, ct) => executeApiMethod.Compile()(ct, webApi, _mappingHandler), pollyContext, cancellationToken);
                }
                else
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Executing {priority} request without specific policies");

                    await executeApiMethod.Compile()(cancellationToken, webApi, _mappingHandler);
                }
            }
            catch (Exception e)
            {
                if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                {
                    _logHandler.Write($"Apizr - {methodName}: Request throwed an exception with message {e.Message}");
                    _logHandler.Write($"Apizr - {methodName}: Throwing an {nameof(ApizrException)} with InnerException");
                }

                throw new ApizrException(e, Unit.Default);
            }
        }

        public async Task<TResult> ExecuteAsync<TResult>(Expression<Func<TWebApi, Task<TResult>>> executeApiMethod,
            Priority priority = Priority.UserInitiated)
        {
            var webApi = GetWebApi(priority);
            var methodCallExpression = GetMethodCallExpression<TResult>(executeApiMethod);
            var methodName = $"{_webApiFriendlyName}.{methodCallExpression.Method.Name}";
            if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                _logHandler.Write($"Apizr - {methodName}: Calling method");

            string cacheKey = null;
            TResult result = default;
            MethodCacheAttributes cacheAttributes = null;

            if (IsMethodCacheable<TResult>(executeApiMethod))
            {
                if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Called method is cacheable");
                if (_cacheHandler is VoidCacheHandler && _apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    _logHandler.Write($"Apizr - {methodName}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                cacheKey = GetCacheKey<TResult>(executeApiMethod);
                if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Used cache key is {cacheKey}");

                result = await _cacheHandler.Get<TResult>(cacheKey);
                if (!Equals(result, default(TResult)) && _apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Some cached data found for this cache key");

                cacheAttributes = GetCacheAttribute<TResult>(executeApiMethod);
            }

            if (result == null || cacheAttributes?.CacheAttribute.Mode == CacheMode.GetAndFetch)
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
                            _logHandler.Write($"Apizr - {methodName}: Executing {priority} request with some policies");

                        var pollyContext = new Context().WithLogHandler(_logHandler);
                        result = await policy.ExecuteAsync(ctx => executeApiMethod.Compile()(webApi), pollyContext);
                    }
                    else
                    {
                        if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                            _logHandler.Write($"Apizr - {methodName}: Executing {priority} request without specific policies");

                        result = await executeApiMethod.Compile()(webApi);
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
                    cacheAttributes != null)
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Caching result");

                    await _cacheHandler.Set(cacheKey, result, cacheAttributes.CacheAttribute.LifeSpan);
                }
            }

            if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                _logHandler.Write($"Apizr - {methodName}: Returning result");

            return result;
        }

        public async Task<TResult> ExecuteAsync<TResult>(Expression<Func<TWebApi, IMappingHandler, Task<TResult>>> executeApiMethod, Priority priority = Priority.UserInitiated)
        {
            var webApi = GetWebApi(priority);
            var methodCallExpression = GetMethodCallExpression<TResult>(executeApiMethod);
            var methodName = $"{_webApiFriendlyName}.{methodCallExpression.Method.Name}";
            if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                _logHandler.Write($"Apizr - {methodName}: Calling method");

            string cacheKey = null;
            TResult result = default;
            MethodCacheAttributes cacheAttributes = null;

            if (IsMethodCacheable<TResult>(executeApiMethod))
            {
                if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Called method is cacheable");
                if (_cacheHandler is VoidCacheHandler && _apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    _logHandler.Write($"Apizr - {methodName}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                cacheKey = GetCacheKey<TResult>(executeApiMethod);
                if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Used cache key is {cacheKey}");

                result = await _cacheHandler.Get<TResult>(cacheKey);
                if (!Equals(result, default(TResult)) && _apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Some cached data found for this cache key");

                cacheAttributes = GetCacheAttribute<TResult>(executeApiMethod);
            }

            if (result == null || cacheAttributes?.CacheAttribute.Mode == CacheMode.GetAndFetch)
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
                            _logHandler.Write($"Apizr - {methodName}: Executing {priority} request with some policies");

                        var pollyContext = new Context().WithLogHandler(_logHandler);
                        result = await policy.ExecuteAsync(ctx => executeApiMethod.Compile()(webApi, _mappingHandler), pollyContext);
                    }
                    else
                    {
                        if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                            _logHandler.Write($"Apizr - {methodName}: Executing {priority} request without specific policies");

                        result = await executeApiMethod.Compile()(webApi, _mappingHandler);
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
                    cacheAttributes != null)
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Caching result");

                    await _cacheHandler.Set(cacheKey, result, cacheAttributes.CacheAttribute.LifeSpan);
                }
            }

            if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                _logHandler.Write($"Apizr - {methodName}: Returning result");

            return result;
        }

        public async Task<TResult> ExecuteAsync<TResult>(Expression<Func<CancellationToken, TWebApi, Task<TResult>>> executeApiMethod, CancellationToken cancellationToken,
            Priority priority = Priority.UserInitiated)
        {
            var webApi = GetWebApi(priority);
            var methodCallExpression = GetMethodCallExpression<TResult>(executeApiMethod);
            var methodName = $"{_webApiFriendlyName}.{methodCallExpression.Method.Name}";
            if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                _logHandler.Write($"Apizr - {methodName}: Calling method");

            string cacheKey = null;
            TResult result = default;
            MethodCacheAttributes cacheAttributes = null;

            if (IsMethodCacheable<TResult>(executeApiMethod))
            {
                if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Called method is cacheable");
                if (_cacheHandler is VoidCacheHandler && _apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    _logHandler.Write($"Apizr - {methodName}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                cacheKey = GetCacheKey<TResult>(executeApiMethod);
                if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Used cache key is {cacheKey}");

                result = await _cacheHandler.Get<TResult>(cacheKey, cancellationToken);
                if (!Equals(result, default(TResult)) && _apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Some cached data found for this cache key");

                cacheAttributes = GetCacheAttribute<TResult>(executeApiMethod);
            }

            if (result == null || cacheAttributes?.CacheAttribute.Mode == CacheMode.GetAndFetch)
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
                            _logHandler.Write($"Apizr - {methodName}: Executing {priority} request with some policies");

                        var pollyContext = new Context().WithLogHandler(_logHandler);
                        result = await policy.ExecuteAsync((ctx, ct) => executeApiMethod.Compile()(ct, webApi), pollyContext, cancellationToken);
                    }
                    else
                    {
                        if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                            _logHandler.Write($"Apizr - {methodName}: Executing {priority} request without specific policies");

                        result = await executeApiMethod.Compile()(cancellationToken, webApi);
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
                    cacheAttributes != null)
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Caching result");

                    await _cacheHandler.Set(cacheKey, result, cacheAttributes.CacheAttribute.LifeSpan, cancellationToken);
                }
            }

            if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                _logHandler.Write($"Apizr - {methodName}: Returning result");

            return result;
        }

        public async Task<TResult> ExecuteAsync<TResult>(Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task<TResult>>> executeApiMethod, CancellationToken cancellationToken,
            Priority priority = Priority.UserInitiated)
        {
            var webApi = GetWebApi(priority);
            var methodCallExpression = GetMethodCallExpression<TResult>(executeApiMethod);
            var methodName = $"{_webApiFriendlyName}.{methodCallExpression.Method.Name}";
            if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                _logHandler.Write($"Apizr - {methodName}: Calling method");

            string cacheKey = null;
            TResult result = default;
            MethodCacheAttributes cacheAttributes = null;

            if (IsMethodCacheable<TResult>(executeApiMethod))
            {
                if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Called method is cacheable");
                if (_cacheHandler is VoidCacheHandler && _apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    _logHandler.Write($"Apizr - {methodName}: You ask for cache but doesn't provide any cache handler. {nameof(VoidCacheHandler)} will fake it.");

                cacheKey = GetCacheKey<TResult>(executeApiMethod);
                if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Used cache key is {cacheKey}");

                result = await _cacheHandler.Get<TResult>(cacheKey, cancellationToken);
                if (!Equals(result, default(TResult)) && _apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                    _logHandler.Write($"Apizr - {methodName}: Some cached data found for this cache key");

                cacheAttributes = GetCacheAttribute<TResult>(executeApiMethod);
            }

            if (result == null || cacheAttributes?.CacheAttribute.Mode == CacheMode.GetAndFetch)
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
                            _logHandler.Write($"Apizr - {methodName}: Executing {priority} request with some policies");

                        var pollyContext = new Context().WithLogHandler(_logHandler);
                        result = await policy.ExecuteAsync((ctx, ct) => executeApiMethod.Compile()(ct, webApi, _mappingHandler), pollyContext, cancellationToken);
                    }
                    else
                    {
                        if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                            _logHandler.Write($"Apizr - {methodName}: Executing {priority} request without specific policies");

                        result = await executeApiMethod.Compile()(cancellationToken, webApi, _mappingHandler);
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
                    cacheAttributes != null)
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Caching result");

                    await _cacheHandler.Set(cacheKey, result, cacheAttributes.CacheAttribute.LifeSpan, cancellationToken);
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
                if (IsMethodCacheable<TResult>(executeApiMethod))
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Method is cacheable");

                    var cacheKey = GetCacheKey<TResult>(executeApiMethod);
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

        private bool IsMethodCacheable<TResult>(Expression restExpression)
        {
            var methodToCacheDetails = GetMethodToCacheData<TResult>(restExpression);

            lock (this)
            {
                var methodToCacheData = methodToCacheDetails;
                if (_cacheableMethodsSet.ContainsKey(methodToCacheData))
                    return true;

                var cacheAttribute =
                    methodToCacheDetails.ApiInterfaceType.GetTypeInfo().GetCustomAttribute<CacheAttribute>() ??
                    methodToCacheData.MethodInfo.GetCustomAttribute<CacheAttribute>();

                if (cacheAttribute == null)
                    return false;

                var methodParameters = methodToCacheData.MethodInfo.GetParameters()
                    .Where(x => !typeof(CancellationToken).GetTypeInfo().IsAssignableFrom(x.ParameterType.GetTypeInfo()))
                    .ToList();
                var cachePrimaryKey =
                    methodParameters
                        .Select((x, index) => new
                        {
                            Index = index,
                            ParameterInfo = x
                        })
                        .Where(x => x.ParameterInfo.CustomAttributes.Any(y => y.AttributeType == typeof(CacheKeyAttribute)))
                        .Select(x => new
                        {
                            ParameterName = x.ParameterInfo.Name,
                            x.ParameterInfo.ParameterType,
                            CacheAttribute = x.ParameterInfo.GetCustomAttribute<CacheKeyAttribute>(),
                            ParameterOrder = x.Index
                        }).FirstOrDefault();

                if (cachePrimaryKey == null && methodParameters.Any())
                    return false;

                //if (cachePrimaryKey == null && methodParameters.Any())
                //    throw new InvalidOperationException($"{methodToCacheData.MethodInfo.Name} method has {nameof(CacheAttribute)}, " +
                //                                        $"it has method parameters but none of that contain {nameof(CacheKeyAttribute)}");


                _cacheableMethodsSet.Add(
                    methodToCacheData,
                    new MethodCacheAttributes(cacheAttribute, cachePrimaryKey?.CacheAttribute, cachePrimaryKey?.ParameterName, cachePrimaryKey?.ParameterType,
                        cachePrimaryKey?.ParameterOrder ?? 0)
                );
            }

            return true;
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
            {
                var requestString = JsonConvert.SerializeObject(constantsExpression.Value);
                if (requestString.Contains("Parameters"))
                {
                    var requestRoot = JsonConvert.DeserializeObject<JObject>(requestString);
                    if (requestRoot.HasValues && 
                        requestRoot.First.HasValues && 
                        requestRoot.First.First.HasValues)
                    {
                        var parameters = requestRoot.First.First.SelectToken("Parameters")?.ToObject<IDictionary<string, object>>();
                        if (parameters != null)
                        {
                            var value = string.Join("&", parameters.Where(kvp => kvp.Value != null).Select(kvp => $"{kvp.Key}={kvp.Value}"));
                            yield return new ExtractedConstant { Name = constantsExpression.Type.Name, Value = value }; 
                        }
                    }
                }
                else
                    yield return new ExtractedConstant { Name = constantsExpression.Type.Name, Value = constantsExpression.Value };
            }


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
                if (memberExpression.Member.Name == "Key")
                {
                    var value = Expression.Lambda(memberExpression).Compile().DynamicInvoke();
                    yield return new ExtractedConstant { Name = memberExpression.Type.Name, Value = value };
                }
                foreach (var constants in ExtractConstants(memberExpression.Expression))
                    yield return constants;
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
                if (typeof(IDictionary<string, object>).IsAssignableFrom(listInitExpression.Type))
                {
                    var stringBuilder = new StringBuilder();
                    var suffix = string.Empty;
                    foreach (var initializer in listInitExpression.Initializers)
                    {
                        foreach (var initializerArgument in initializer.Arguments)
                        {
                            if (initializerArgument is ConstantExpression constantExpression)
                            {
                                stringBuilder.Append(suffix);
                                stringBuilder.Append(constantExpression.Value);
                                suffix = "=";
                            }
                        }
                        suffix = "&";
                    }

                    yield return new ExtractedConstant { Name = listInitExpression.Type.Name, Value = stringBuilder.ToString() };
                }
                else
                    yield return new ExtractedConstant { Name = listInitExpression.Type.Name };
            }
            else if (expression is MemberInitExpression memberInitExpression)
            {
                var parameters = memberInitExpression.Bindings.Select(b =>
                    b.ToString().Replace("\"", string.Empty).Replace(" ", string.Empty)).ToList();
                if (parameters.Any())
                {
                    var value = string.Join("&", parameters);
                    yield return new ExtractedConstant { Name = memberInitExpression.Type.Name, Value = value };
                }
                else
                    foreach(var constants in ExtractConstants(memberInitExpression.NewExpression))
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
                var value = string.Join("&", parameters.Where(kvp => kvp.Value != null).Select(kvp => $"{kvp.Key}={kvp.Value}"));
                yield return new ExtractedConstant { Name = newExpression.Type.Name, Value = value };
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

            var extractedArguments = methodCallExpression.Arguments
                .SelectMany(ExtractConstants)
                .Where(x => x != null)
                .Where(x => x.Value is CancellationToken == false)
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
                throw new InvalidOperationException($"{nameof(CacheKeyAttribute)} primary key found for: " + cacheKeyPrefix);

            // Simple param value OR complex type with overriden ToString
            var value = primaryKeyValue.ToString();
            if (!string.IsNullOrWhiteSpace(value) && value != primaryKeyValue.GetType().ToString())
                return value.Contains(":")
                    ? $"{cacheKeyPrefix}({value})"
                    : $"{cacheKeyPrefix}({primaryKeyName}:{value})";
            
            // Dictionary param key values
            if (primaryKeyValue is IDictionary<string, string> dictionary)
                return $"{cacheKeyPrefix}({dictionary.ToString(":", ", ")})";

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

        private MethodCallExpression GetMethodCallExpression<TResult>(
            Expression expression)
        {
            switch (expression)
            {
                case Expression<Func<TWebApi, Task>> executeApiMethod:
                    return GetMethodCallExpression<TResult>(executeApiMethod.Body);
                case Expression<Func<TWebApi, IMappingHandler, Task>> executeApiMethod:
                    return GetMethodCallExpression<TResult>(executeApiMethod.Body);
                case Expression<Func<CancellationToken, TWebApi, Task>> executeApiMethod:
                    return GetMethodCallExpression<TResult>(executeApiMethod.Body);
                case Expression<Func<CancellationToken, TWebApi, IMappingHandler, Task>> executeApiMethod:
                    return GetMethodCallExpression<TResult>(executeApiMethod.Body);
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

            var policyAttribute = methodCallExpression.Method.GetCustomAttribute<PolicyAttribute>();
            if (policyAttribute == null)
                return null;

            var methodName = $"{_webApiFriendlyName}.{methodCallExpression.Method.Name}";

            IAsyncPolicy<TResult> policy = null;
            foreach (var registryKey in policyAttribute.RegistryKeys)
            {
                if (_policyRegistry.TryGet<IsPolicy>(registryKey, out var registeredPolicy))
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Found a policy with key {registryKey}");
                    if (registeredPolicy is IAsyncPolicy<TResult> registeredPolicyWithResult)
                    {
                        if (policy == null)
                            policy = registeredPolicyWithResult;
                        else
                            policy.WrapAsync(registeredPolicyWithResult);

                        if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                            _logHandler.Write($"Apizr - {methodName}: Policy with key {registryKey} will be applied");
                    }
                    else if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    {
                        _logHandler.Write($"Apizr - {methodName}: Policy with key {registryKey} is not of {typeof(IAsyncPolicy<TResult>)} type and will be ignored");
                    }
                }
                else if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                {
                    _logHandler.Write($"Apizr - {methodName}: No policy found for key {registryKey}");
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

            var policyAttribute = methodCallExpression.Method.GetCustomAttribute<PolicyAttribute>();
            if (policyAttribute == null)
                return null;

            var methodName = $"{_webApiFriendlyName}.{methodCallExpression.Method.Name}";

            IAsyncPolicy policy = null;
            foreach (var registryKey in policyAttribute.RegistryKeys)
            {
                if (_policyRegistry.TryGet<IsPolicy>(registryKey, out var registeredPolicy))
                {
                    if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                        _logHandler.Write($"Apizr - {methodName}: Found a policy with key {registryKey}");
                    if (registeredPolicy is IAsyncPolicy registeredPolicyWithoutResult)
                    {
                        if (policy == null)
                            policy = registeredPolicyWithoutResult;
                        else
                            policy.WrapAsync(registeredPolicyWithoutResult);

                        if (_apizrOptions.ApizrVerbosity == ApizrLogLevel.High)
                            _logHandler.Write($"Apizr - {methodName}: Policy with key {registryKey} will be applied");
                    }
                    else if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                    {
                        _logHandler.Write($"Apizr - {methodName}: Policy with key {registryKey} is not of {typeof(IAsyncPolicy)} type and will be ignored");
                    }
                }
                else if (_apizrOptions.ApizrVerbosity >= ApizrLogLevel.Low)
                {
                    _logHandler.Write($"Apizr - {methodName}: No policy found for key {registryKey}");
                }
            }

            return policy;
        }

        #endregion
    }
}