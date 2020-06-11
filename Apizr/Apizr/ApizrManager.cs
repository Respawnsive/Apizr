using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Caching;
using Apizr.Connecting;
using Apizr.Policing;
using Apizr.Prioritizing;
using Fusillade;
using Polly;
using Polly.Registry;

namespace Apizr
{
    public class ApizrManager<TWebApi> : IApizrManager<TWebApi>
    {
        readonly Dictionary<MethodCacheDetails, MethodCacheAttributes> _cacheableMethodsSet;
        readonly IEnumerable<ILazyPrioritizedWebApi<TWebApi>> _webApis;
        readonly IConnectivityProvider _connectivityProvider;
        readonly ICacheProvider _cacheProvider;
        readonly IReadOnlyPolicyRegistry<string> _policyRegistry;

        public ApizrManager(IEnumerable<ILazyPrioritizedWebApi<TWebApi>> webApis, IConnectivityProvider connectivityProvider, ICacheProvider cacheProvider, IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            _cacheableMethodsSet = new Dictionary<MethodCacheDetails, MethodCacheAttributes>();
            _webApis = webApis;
            _connectivityProvider = connectivityProvider;
            _cacheProvider = cacheProvider;
            _policyRegistry = policyRegistry;
        }

        TWebApi GetWebApi(Priority priority) => _webApis.First(x => x.Priority == priority || x.Priority == Priority.UserInitiated).Value;

        public async Task<TResult> ExecuteAsync<TResult>(Expression<Func<TWebApi, Task<TResult>>> executeApiMethod, Priority priority = Priority.UserInitiated)
        {
            string cacheKey = null;
            TResult result = default;
            MethodCacheAttributes cacheAttributes = null;

            if (IsMethodCacheable(executeApiMethod))
            {
                if(_cacheProvider is VoidCacheProvider)
                    Console.WriteLine("Apizr: You ask for cache but doesn't provide any cache provider");

                cacheKey = GetCacheKey(executeApiMethod);
                result = await _cacheProvider.Get<TResult>(cacheKey); 
                cacheAttributes = GetCacheAttribute(executeApiMethod);
            }

            if (result == null || cacheAttributes?.CacheAttribute.Mode == CacheMode.GetAndFetch)
            {
                try
                {
                    if(!_connectivityProvider.IsConnected())
                        throw new IOException();

                    if (_connectivityProvider is VoidConnectivityProvider)
                        Console.WriteLine("Apizr: Connectivity is not checked as you didn't provide any connectivity provider");

                    var policy = GetMethodPolicy(executeApiMethod.Body as MethodCallExpression);
                    var executeApiMethodTask = executeApiMethod.Compile()(GetWebApi(priority));

                    result = policy != null
                        ? await policy.ExecuteAsync(async () => await executeApiMethodTask)
                        : await executeApiMethodTask;
                }
                catch (Exception e)
                {
                    throw new ApizrException<TResult>(e, result);
                }

                if (result != null && _cacheProvider != null && !string.IsNullOrWhiteSpace(cacheKey) && cacheAttributes != null)
                    await _cacheProvider.Set(cacheKey, result, cacheAttributes.CacheAttribute.LifeSpan);
            }

            return result;
        }

        public Task ExecuteAsync(Expression<Func<TWebApi, Task>> executeApiMethod, Priority priority = Priority.UserInitiated)
        {
            try
            {
                if (!_connectivityProvider.IsConnected())
                    throw new IOException();

                if(_connectivityProvider is VoidConnectivityProvider)
                    Console.WriteLine("Apizr: Connectivity is not checked as you didn't provide any connectivity provider");

                var policy = GetMethodPolicy(executeApiMethod.Body as MethodCallExpression);
                var executeApiMethodTask = executeApiMethod.Compile()(GetWebApi(priority));

                return policy != null
                        ? policy.ExecuteAsync(async () => await executeApiMethodTask)
                        : executeApiMethodTask;
            }
            catch (Exception e)
            {
                throw new ApizrException(e, Unit.Default);
            }
        }

        public async Task<bool> ClearCacheAsync<TResult>(Expression<Func<TWebApi, Task<TResult>>> executeApiMethod = null)
        {
            if (_cacheProvider is VoidCacheProvider)
                Console.WriteLine("Apizr: You ask for cache but doesn't provide any cache provider");

            try
            {
                if (executeApiMethod == null)
                {
                    await _cacheProvider.Clear();
                }
                else if (IsMethodCacheable(executeApiMethod))
                {
                    var cacheKey = GetCacheKey(executeApiMethod);
                    return await _cacheProvider.Remove(cacheKey);
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Apizr: clearing cache throwed an exception with message: {e.Message}");
                return false;
            }
        }

        #region Caching

        bool IsMethodCacheable<TApi, TResult>(Expression<Func<TApi, Task<TResult>>> restExpression)
        {
            var methodToCacheDetails = GetMethodToCacheData(restExpression);

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
                    throw new InvalidOperationException($"{methodToCacheData.MethodInfo.Name} method has {nameof(CacheAttribute)}, " +
                                                        $"it has method parameters but none of that contain {nameof(CacheKeyAttribute)}");


                _cacheableMethodsSet.Add(
                    methodToCacheData,
                    new MethodCacheAttributes(cacheAttribute, cachePrimaryKey?.CacheAttribute, cachePrimaryKey?.ParameterName, cachePrimaryKey?.ParameterType,
                        cachePrimaryKey?.ParameterOrder ?? 0)
                );
            }

            return true;
        }

        MethodCacheDetails GetMethodToCacheData<TApi, TResult>(Expression<Func<TApi, Task<TResult>>> restExpression)
        {
            var apiInterfaceType = typeof(TApi);
            var methodBody = (MethodCallExpression)restExpression.Body;
            var methodInfo = methodBody.Method;
            return new MethodCacheDetails(apiInterfaceType, methodInfo);
        }

        static IEnumerable<ExtractedConstant> ExtractConstants(Expression expression)
        {
            if (expression == null)
                yield break;

            if (expression is ConstantExpression constantsExpression)
                yield return new ExtractedConstant { Name = constantsExpression.Type.Name, Value = constantsExpression.Value };


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
                    yield return constants;
            }
            else if (expression is InvocationExpression invocationExpression)
            {
                foreach (var constants in ExtractConstants(invocationExpression.Expression))
                    yield return constants;
            }

            else
                throw new NotImplementedException();
        }

        string GetCacheKey<TApi, TResult>(Expression<Func<TApi, Task<TResult>>> fromExpression)
        {
            var methodCallExpression = (MethodCallExpression)fromExpression.Body;

            var cacheKeyPrefix = $"{typeof(TApi)}.{methodCallExpression.Method.Name}";
            if (!methodCallExpression.Arguments.Any())
                return $"{cacheKeyPrefix}()";

            var cacheAttributes = GetCacheAttribute(fromExpression);

            var extractedArguments = methodCallExpression.Arguments
                .SelectMany(x => ExtractConstants(x))
                .Where(x => x != null)
                .Where(x => x.Value is CancellationToken == false)
                .ToList();

            if (!extractedArguments.Any())
                return $"{cacheKeyPrefix}()";

            var primaryKeyName = cacheAttributes.ParameterName;
            object primaryKeyValue;
            var extractedArgument = extractedArguments[cacheAttributes.ParameterOrder];
            var extractedArgumentValue = extractedArgument.Value;


            var isArgumentValuePrimitve = extractedArgumentValue.GetType().GetTypeInfo().IsPrimitive ||
                                          extractedArgumentValue is decimal ||
                                          extractedArgumentValue is string;

            if (isArgumentValuePrimitve)
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

            return $"{cacheKeyPrefix}({primaryKeyName}:{primaryKeyValue})";
        }

        MethodCacheAttributes GetCacheAttribute<TApi, TResult>(Expression<Func<TApi, Task<TResult>>> expression)
        {
            lock (this)
            {
                var methodToCacheData = GetMethodToCacheData(expression);
                return _cacheableMethodsSet[methodToCacheData];
            }
        }

        class ExtractedConstant
        {
            public object Value { get; set; }

            public string Name { get; set; }
        }

        #endregion

        #region Policing

        IAsyncPolicy GetMethodPolicy(MethodCallExpression methodCallExpression)
        {
            if (methodCallExpression == null)
                return null;

            if (_policyRegistry == null)
                return null;

            var policyAttribute = methodCallExpression.Method.GetCustomAttribute<PolicyAttribute>();
            if (policyAttribute == null)
                return null;

            IAsyncPolicy policy = null;
            foreach (var registryKey in policyAttribute.RegistryKeys)
            {
                if (_policyRegistry.TryGet<IAsyncPolicy>(registryKey, out var registeredPolicy))
                {
                    if (policy == null)
                        policy = registeredPolicy;
                    else
                        policy.WrapAsync(registeredPolicy);
                }
            }

            return policy;
        } 

        #endregion
    }
}