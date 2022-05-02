using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring;
using Apizr.Extending.Configuring.Registry;
using Apizr.Logging;
using Apizr.Mediation.Requesting.Sending;
using Apizr.Optional.Extending;
using Apizr.Optional.Requesting.Sending;
using Apizr.Policing;
using Apizr.Requesting;
using Apizr.Tests.Helpers;
using Apizr.Tests.Models;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MonkeyCache.FileStore;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;
using Refit;
using Xunit;
using IHttpBinService = Apizr.Tests.Apis.IHttpBinService;
using IReqResService = Apizr.Tests.Apis.IReqResService;
using UserList = Apizr.Tests.Models.UserList;

namespace Apizr.Tests
{
    public class ApizrExtendedRegistryTests
    {
        private readonly IPolicyRegistry<string> _policyRegistry;
        private readonly RefitSettings _refitSettings;
        private readonly Assembly _assembly;

        public ApizrExtendedRegistryTests()
        {
            _policyRegistry = new PolicyRegistry
            {
                {
                    "TransientHttpError", HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(new[]
                    {
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(10)
                    }, LoggedPolicies.OnLoggedRetry).WithPolicyKey("TransientHttpError")
                }
            };

            var opts = new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };
            _refitSettings = new RefitSettings(new SystemTextJsonContentSerializer(opts));

            _assembly = Assembly.GetExecutingAssembly();

            Barrel.ApplicationId = nameof(ApizrExtendedRegistryTests);
        }

        [Fact]
        public void ServiceCollection_Should_Contain_Registry_And_Managers()
        {
            var services = new ServiceCollection();
            services.AddApizr(registry => registry
                .AddManagerFor<IReqResService>()
                .AddManagerFor<IHttpBinService>()
                .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>());

            services.Should().Contain(x => x.ServiceType == typeof(IApizrExtendedRegistry));
            services.Should().Contain(x => x.ServiceType == typeof(IApizrManager<IReqResService>));
            services.Should().Contain(x => x.ServiceType == typeof(IApizrManager<IHttpBinService>));
            services.Should().Contain(x => x.ServiceType == typeof(IApizrManager<ICrudApi<User, int, PagedResult<User>, IDictionary<string, object>>>));
        }

        [Fact]
        public void ServiceProvider_Should_Resolve_Registry_And_Managers()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                .AddManagerFor<IReqResService>()
                .AddManagerFor<IHttpBinService>()
                .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>());

            var serviceProvider = services.BuildServiceProvider();
            var registry = serviceProvider.GetService<IApizrExtendedRegistry>();
            var reqResManager = serviceProvider.GetService<IApizrManager<IReqResService>>();
            var httpBinManager = serviceProvider.GetService<IApizrManager<IHttpBinService>>();
            var userManager = serviceProvider.GetService<IApizrManager<ICrudApi<User, int, PagedResult<User>, IDictionary<string, object>>>>();

            registry.Should().NotBeNull();
            reqResManager.Should().NotBeNull();
            httpBinManager.Should().NotBeNull();
            userManager.Should().NotBeNull();
        }

        [Fact]
        public void ServiceProvider_Should_Resolve_Registry_And_Scanned_Managers()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                .AddCrudManagerFor(_assembly));

            var serviceProvider = services.BuildServiceProvider();
            var registry = serviceProvider.GetService<IApizrExtendedRegistry>();
            var userManager = serviceProvider.GetService<IApizrManager<ICrudApi<User, int, PagedResult<User>, IDictionary<string, object>>>>();

            registry.Should().NotBeNull();
            userManager.Should().NotBeNull();
        }

        [Fact]
        public void Registry_Should_Resolve_Managers()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                .AddManagerFor<IReqResService>()
                .AddManagerFor<IHttpBinService>()
                .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>());

            var serviceProvider = services.BuildServiceProvider();
            var registry = serviceProvider.GetRequiredService<IApizrExtendedRegistry>();
            
            registry.TryGetManagerFor<IReqResService>(out var reqResManager).Should().BeTrue();
            registry.TryGetManagerFor<IHttpBinService>(out var httpBinManager).Should().BeTrue();
            registry.TryGetCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(out var userManager);

            reqResManager.Should().NotBeNull();
            httpBinManager.Should().NotBeNull();
            userManager.Should().NotBeNull();
        }

        [Fact]
        public void Calling_WithBaseAddress_Should_Set_BaseAddress()
        {
            var attributeUri = "https://reqres.in/api";
            var uri1 = new Uri("http://uri1.com");
            var uri2 = new Uri("http://uri2.com");

            // By attribute
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                .AddManagerFor<IReqResService>());

            var serviceProvider = services.BuildServiceProvider();
            var fixture = serviceProvider.GetRequiredService<IApizrManager<IReqResService>>();

            fixture.Options.BaseAddress.Should().Be(attributeUri);

            // By proper option overriding attribute
            services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                .AddManagerFor<IReqResService>(options => options.WithBaseAddress(uri1)));

            serviceProvider = services.BuildServiceProvider();
            fixture = serviceProvider.GetRequiredService<IApizrManager<IReqResService>>();

            fixture.Options.BaseAddress.Should().Be(uri1);

            // By attribute overriding common option
            services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                    .AddManagerFor<IReqResService>(),
                config => config.WithBaseAddress(uri1));

            serviceProvider = services.BuildServiceProvider();
            fixture = serviceProvider.GetRequiredService<IApizrManager<IReqResService>>();

            fixture.Options.BaseAddress.Should().Be(attributeUri);

            // By proper option overriding proper option and attribute
            services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                .AddManagerFor<IReqResService>(options => options.WithBaseAddress(uri2)),
                config => config.WithBaseAddress(uri1));

            serviceProvider = services.BuildServiceProvider();
            fixture = serviceProvider.GetRequiredService<IApizrManager<IReqResService>>();

            fixture.Options.BaseAddress.Should().Be(uri2);
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_ProperOption_Should_Authenticate_Request()
        {
            string token = null;

            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                .AddManagerFor<IHttpBinService>(options => options
                    .WithAuthenticationHandler(_ => Task.FromResult(token = "token"))));

            var serviceProvider = services.BuildServiceProvider();
            var httpBinManager = serviceProvider.GetRequiredService<IApizrManager<IHttpBinService>>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
            token.Should().Be("token");
        }

        [Fact]
        public void Calling_WithLogging_Should_Set_LoggingSettings()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                .AddManagerFor<IReqResService>(options => options.WithLogging((HttpTracerMode) HttpTracerMode.ExceptionsOnly, (HttpMessageParts) HttpMessageParts.RequestCookies, LogLevel.Warning)));

            var serviceProvider = services.BuildServiceProvider();
            var fixture = serviceProvider.GetRequiredService<IApizrManager<IReqResService>>();

            fixture.Options.HttpTracerMode.Should().Be(HttpTracerMode.ExceptionsOnly);
            fixture.Options.TrafficVerbosity.Should().Be(HttpMessageParts.RequestCookies);
            fixture.Options.LogLevels.Should().AllBeEquivalentTo(LogLevel.Warning);
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_CommonOption_Should_Authenticate_Request()
        {
            string token = null;

            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(
                registry => registry
                    .AddManagerFor<IHttpBinService>(),
                config => config
                    .WithAuthenticationHandler(_ => Task.FromResult(token = "token")));

            var serviceProvider = services.BuildServiceProvider();
            var httpBinManager = serviceProvider.GetRequiredService<IApizrManager<IHttpBinService>>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
            token.Should().Be("token");
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_Should_Authenticate_Request()
        {
            string token = null;

            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(
                registry => registry
                    .AddManagerFor<IHttpBinService>(options => options
                        .WithAuthenticationHandler(_ => Task.FromResult(token = "tokenA"))),
                config => config
                    .WithAuthenticationHandler(_ => Task.FromResult(token = "tokenB")));

            var serviceProvider = services.BuildServiceProvider();
            var httpBinManager = serviceProvider.GetRequiredService<IApizrManager<IHttpBinService>>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
            token.Should().Be("tokenA");
        }

        [Fact]
        public async Task Calling_WithAkavacheCacheHandler_Should_Cache_Result()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);

            services.AddApizr(
                registry => registry
                    .AddManagerFor<IReqResService>(),
                config => config
                    .WithAkavacheCacheHandler()
                    .AddDelegatingHandler(new FailingRequestHandler()));

            var serviceProvider = services.BuildServiceProvider();
            var reqResManager = serviceProvider.GetRequiredService<IApizrManager<IReqResService>>();

            // Clear cache
            await reqResManager.ClearCacheAsync();

            // Defining a throwing request
            Func<Task> act = () => reqResManager.ExecuteAsync(api => api.GetUsersAsync(HttpStatusCode.BadRequest));

            // Calling it at first execution should throw as expected without any cached result
            var ex = await act.Should().ThrowAsync<ApizrException<UserList>>();
            ex.And.CachedResult.Should().BeNull();

            // This one should succeed
            var result = await reqResManager.ExecuteAsync(api => api.GetUsersAsync());

            // and cache result in-memory
            result.Should().NotBeNull();
            result.Data.Should().NotBeNullOrEmpty();

            // This one should fail but with cached result
            var ex2 = await act.Should().ThrowAsync<ApizrException<UserList>>();
            ex2.And.CachedResult.Should().NotBeNull();
        }

        [Fact]
        public async Task Calling_WithInMemoryCacheHandler_Should_Cache_Result()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddMemoryCache();

            services.AddApizr(
                registry => registry
                    .AddManagerFor<IReqResService>(),
                config => config
                    .WithInMemoryCacheHandler()
                    .AddDelegatingHandler(new FailingRequestHandler()));

            var serviceProvider = services.BuildServiceProvider();
            var reqResManager = serviceProvider.GetRequiredService<IApizrManager<IReqResService>>();

            // Defining a throwing request
            Func<bool, Action<Exception>, Task<UserList>> act = (clearCache, onException) => reqResManager.ExecuteAsync(api => api.GetUsersAsync(HttpStatusCode.BadRequest), clearCache, onException);

            // Calling it at first execution should throw as expected without any cached result
            var ex = await act.Invoking(x => x.Invoke(false, null)).Should().ThrowAsync<ApizrException<UserList>>();
            ex.And.CachedResult.Should().BeNull();

            // This one should succeed
            var result = await reqResManager.ExecuteAsync(api => api.GetUsersAsync());

            // and cache result in-memory
            result.Should().NotBeNull();
            result.Data.Should().NotBeNullOrEmpty();

            // This one should fail but with cached result
            ex = await act.Invoking(x => x.Invoke(false, null)).Should().ThrowAsync<ApizrException<UserList>>();
            ex.And.CachedResult.Should().NotBeNull();

            // this one should return cached result and handle exception
            result = await act.Invoke(false, e =>
            {
                // The handled exception with cached result on this side
                e.Should().BeOfType<ApizrException<UserList>>().Which.CachedResult.Should().NotBeNull();
            });

            // The returned result on the other side
            result.Should().NotBeNull();

            // This one should fail but without any cached result as we asked for clearing it
            ex = await act.Invoking(x => x.Invoke(true, null)).Should().ThrowAsync<ApizrException<UserList>>();
            ex.And.CachedResult.Should().BeNull();
        }

        [Fact]
        public async Task RequestTimeout_Should_Be_Handled_By_Polly()
        {
            var attempts = 0;
            var sleepDurations = new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
            };
            var policyRegistry = new PolicyRegistry
            {
                {
                    "TransientHttpError", HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(sleepDurations, 
                        (_, _, retry, _) => attempts = retry).WithPolicyKey("TransientHttpError")
                }
            };

            var services = new ServiceCollection();
            services.AddPolicyRegistry(policyRegistry);
            services.AddMemoryCache();

            services.AddApizr(
                registry => registry
                    .AddManagerFor<IReqResService>(),
                config => config
                    .AddDelegatingHandler(new FailingRequestHandler()));

            var serviceProvider = services.BuildServiceProvider();
            var reqResManager = serviceProvider.GetRequiredService<IApizrManager<IReqResService>>();

            // Defining a transient throwing request
            Func<Task> act = () => reqResManager.ExecuteAsync(api => api.GetUsersAsync(HttpStatusCode.RequestTimeout));

            // Calling it should throw but handled by Polly
            await act.Should().ThrowAsync<ApizrException>();

            // attempts should be equal to total retry count
            attempts.Should().Be(sleepDurations.Length);
        }

        [Fact]
        public async Task Calling_WithConnectivityHandler_Should_Check_Connectivity()
        {
            var isConnected = false;

            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(
                registry => registry
                    .AddManagerFor<IReqResService>(),
                config => config
                    .WithConnectivityHandler(() => isConnected));

            var serviceProvider = services.BuildServiceProvider();
            var reqResManager = serviceProvider.GetRequiredService<IApizrManager<IReqResService>>();

            // Defining a request
            Func<Task> act = () => reqResManager.ExecuteAsync(api => api.GetUsersAsync());

            // Calling it should throw as isConnected is at false
            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<IOException>();

            // Setting isConnected to true
            isConnected = true;

            // Then request should succeed
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public void Calling_WithRefitSettings_Should_Set_Settings()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddAutoMapper(_assembly);
            services.AddApizr(
                registry => registry
                    .AddManagerFor<IReqResService>(),
                config => config
                    .WithRefitSettings(_refitSettings));

            var serviceProvider = services.BuildServiceProvider();
            var apizrOptions = serviceProvider.GetRequiredService<IApizrOptions<IReqResService>>();
            
            apizrOptions.RefitSettings.Should().Be(_refitSettings);
        }

        [Fact]
        public async Task Calling_WithAutoMapperMappingHandler_Should_Map_Data()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddAutoMapper(_assembly);
            services.AddApizr(
                registry => registry
                    .AddManagerFor<IReqResService>(),
                config => config
                    .WithRefitSettings(_refitSettings)
                    .WithAutoMapperMappingHandler());

            var serviceProvider = services.BuildServiceProvider();
            var reqResManager = serviceProvider.GetRequiredService<IApizrManager<IReqResService>>();

            var minUser = new MinUser { Name = "John" };

            // This one should succeed
            var result = await reqResManager.ExecuteAsync<MinUser, User>((api, user) => api.CreateUser(user, CancellationToken.None), minUser);

            result.Should().NotBeNull();
            result.Name.Should().Be(minUser.Name);
            result.Id.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task Calling_WithMappingHandler_Should_Map_Data()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddAutoMapper(_assembly);
            services.AddApizr(
                registry => registry
                    .AddManagerFor<IReqResService>(),
                config => config
                    .WithRefitSettings(_refitSettings)
                    .WithMappingHandler<AutoMapperMappingHandler>());

            var serviceProvider = services.BuildServiceProvider();
            var reqResManager = serviceProvider.GetRequiredService<IApizrManager<IReqResService>>();

            var minUser = new MinUser { Name = "John" };

            // This one should succeed
            var result = await reqResManager.ExecuteAsync<MinUser, User>((api, user) => api.CreateUser(user, CancellationToken.None), minUser);

            result.Should().NotBeNull();
            result.Name.Should().Be(minUser.Name);
            result.Id.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task Calling_WithMediation_Should_Handle_Requests()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddApizr(
                registry => registry
                    .AddManagerFor<IReqResService>(),
                config => config
                    .WithMediation());

            var serviceProvider = services.BuildServiceProvider();
            var reqResMediator = serviceProvider.GetRequiredService<IApizrMediator<IReqResService>>();

            reqResMediator.Should().NotBeNull();
            var result = await reqResMediator.SendFor(api => api.GetUsersAsync());
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Count.Should().BeGreaterOrEqualTo(1);
        }

        [Fact]
        public async Task Calling_WithOptionalMediation_Should_Handle_Requests_With_Optional_Result()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddApizr(
                registry => registry
                    .AddManagerFor<IReqResService>(),
                config => config
                    .WithOptionalMediation());

            var serviceProvider = services.BuildServiceProvider();
            var reqResMediator = serviceProvider.GetRequiredService<IApizrOptionalMediator<IReqResService>>();

            reqResMediator.Should().NotBeNull();
            var result = await reqResMediator.SendFor(api => api.GetUsersAsync());
            result.Should().NotBeNull();
            result.Match(userList =>
                {
                    userList.Should().NotBeNull();
                    userList.Data.Should().NotBeNull();
                    userList.Data.Count.Should().BeGreaterOrEqualTo(1);
                },
                e => { 
                    // ignore error
                });
        }
    }
}
