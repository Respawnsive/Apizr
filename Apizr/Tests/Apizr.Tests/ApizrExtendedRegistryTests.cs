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
using Apizr.Configuring.Manager;
using Apizr.Extending;
using Apizr.Extending.Configuring.Registry;
using Apizr.Logging;
using Apizr.Mediation.Requesting.Sending;
using Apizr.Optional.Extending;
using Apizr.Optional.Requesting.Sending;
using Apizr.Policing;
using Apizr.Requesting;
using Apizr.Tests.Apis;
using Apizr.Tests.Helpers;
using Apizr.Tests.Models;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;
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
                .AddManagerFor<IReqResUserService>()
                .AddManagerFor<IHttpBinService>()//);
                //.AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>());
                .AddUploadManager(uploadRegistry => uploadRegistry.AddFor<IUploadApi>()));

            services.Should().Contain(x => x.ServiceType == typeof(IApizrExtendedRegistry));
            services.Should().Contain(x => x.ServiceType == typeof(IApizrManager<IReqResUserService>));
            services.Should().Contain(x => x.ServiceType == typeof(IApizrManager<IHttpBinService>));
            //services.Should().Contain(x => x.ServiceType == typeof(IApizrManager<ICrudApi<User, int, PagedResult<User>, IDictionary<string, object>>>));
            services.Should().Contain(x => x.ServiceType == typeof(IApizrManager<IUploadApi>));
            services.Should().Contain(x => x.ServiceType == typeof(IApizrUploadManager<IUploadApi>));
        }

        [Fact]
        public void ServiceProvider_Should_Resolve_Registry_And_Managers()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                .AddManagerFor<IReqResUserService>()
                .AddManagerFor<IHttpBinService>()
                .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>());

            var serviceProvider = services.BuildServiceProvider();
            var registry = serviceProvider.GetService<IApizrExtendedRegistry>();
            var reqResManager = serviceProvider.GetService<IApizrManager<IReqResUserService>>();
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
                .AddManagerFor<IReqResUserService>()
                .AddManagerFor<IHttpBinService>()
                .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>());

            var serviceProvider = services.BuildServiceProvider();
            var registry = serviceProvider.GetRequiredService<IApizrExtendedRegistry>();
            
            registry.TryGetManagerFor<IReqResUserService>(out var reqResManager).Should().BeTrue();
            registry.TryGetManagerFor<IHttpBinService>(out var httpBinManager).Should().BeTrue();
            registry.TryGetCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(out var userManager).Should().BeTrue();

            reqResManager.Should().NotBeNull();
            httpBinManager.Should().NotBeNull();
            userManager.Should().NotBeNull();
        }

        [Fact]
        public void Calling_WithBaseAddress_Should_Set_BaseUri()
        {
            var attributeUri = "https://reqres.in/api";
            var uri1 = new Uri("http://uri1.com");
            var uri2 = new Uri("http://uri2.com");

            // By attribute
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                .AddManagerFor<IReqResUserService>());

            var serviceProvider = services.BuildServiceProvider();
            var fixture = serviceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            fixture.Options.BaseUri.Should().Be(attributeUri);

            // By proper option overriding attribute
            services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                .AddManagerFor<IReqResUserService>(options => options.WithBaseAddress(uri1)));

            serviceProvider = services.BuildServiceProvider();
            fixture = serviceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            fixture.Options.BaseUri.Should().Be(uri1);

            // By attribute overriding common option
            services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                    .AddManagerFor<IReqResUserService>(),
                config => config.WithBaseAddress(uri1));

            serviceProvider = services.BuildServiceProvider();
            fixture = serviceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            fixture.Options.BaseUri.Should().Be(attributeUri);

            // By proper option overriding proper option and attribute
            services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                .AddManagerFor<IReqResUserService>(options => options.WithBaseAddress(uri2)),
                config => config.WithBaseAddress(uri1));

            serviceProvider = services.BuildServiceProvider();
            fixture = serviceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            fixture.Options.BaseUri.Should().Be(uri2);
        }

        [Fact]
        public void Calling_WithBaseAddress_And_WithBasePath_Grouped_Should_Set_BaseUri()
        {
            var attributeUri = "https://reqres.in/api";
            var uri1 = new Uri("http://uri1.com");
            var uri2 = new Uri("http://uri2.com");
            var uri3 = new Uri("http://uri3.com");
            var uri4 = new Uri("http://uri4.com");
            var userPath = "users";
            var fullUri3 = $"{uri3}{userPath}";
            var fullUri4 = $"{uri4}{userPath}";
            var resPath = "resources";
            var fullResUri = $"{attributeUri}/{resPath}";

            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);

            // By attribute option overriding common options
            services.AddApizr(registry => registry
                    .AddGroup(group => group
                            .AddManagerFor<IReqResUserService>()
                            .AddManagerFor<IReqResUserPathService>() // completing with base path by attribute
                            .AddManagerFor<IReqResResourceService>(config => config.WithBasePath(userPath)), // completing with base path by proper option
                        config => config.WithBaseAddress(uri3))
                    .AddManagerFor<IHttpBinService>(options => options.WithBaseAddress(uri2)),
                config => config.WithBaseAddress(uri1));

            var serviceProvider = services.BuildServiceProvider();
            var apizrRegistry = serviceProvider.GetRequiredService<IApizrExtendedRegistry>();

            var userFixture = apizrRegistry.GetManagerFor<IReqResUserService>();
            userFixture.Options.BaseUri.Should().Be(attributeUri);

            var userPathFixture = apizrRegistry.GetManagerFor<IReqResUserPathService>();
            userPathFixture.Options.BaseUri.Should().Be(fullUri3);

            var resourceFixture = apizrRegistry.GetManagerFor<IReqResResourceService>();
            resourceFixture.Options.BaseUri.Should().Be(fullUri3);

            // By proper option overriding all common options and attribute
            services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                    .AddGroup(group => group
                            .AddManagerFor<IReqResUserService>(config => config.WithBaseAddress(uri4)) // changing base uri
                            .AddManagerFor<IReqResUserPathService>(config => config.WithBaseAddress(uri4).WithBasePath(userPath)) // changing base uri completing with base path
                            .AddManagerFor<IReqResResourceService>()
                            .AddManagerFor<IReqResResourceAddressService>(config => config.WithBasePath(resPath)), // completing attribute address with base path by proper options
                        config => config.WithBaseAddress(uri3))
                    .AddManagerFor<IHttpBinService>(options => options.WithBaseAddress(uri2)),
                config => config.WithBaseAddress(uri1));

            serviceProvider = services.BuildServiceProvider();
            apizrRegistry = serviceProvider.GetRequiredService<IApizrExtendedRegistry>();

            userFixture = apizrRegistry.GetManagerFor<IReqResUserService>();
            userFixture.Options.BaseUri.Should().Be(uri4);

            userPathFixture = apizrRegistry.GetManagerFor<IReqResUserPathService>();
            userPathFixture.Options.BaseUri.Should().Be(fullUri4);

            resourceFixture = apizrRegistry.GetManagerFor<IReqResResourceService>();
            resourceFixture.Options.BaseUri.Should().Be(uri3);

            var resourceAddressFixture = apizrRegistry.GetManagerFor<IReqResResourceAddressService>();
            resourceAddressFixture.Options.BaseUri.Should().Be(fullResUri);
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
                .AddManagerFor<IReqResUserService>(options => options.WithLogging((HttpTracerMode) HttpTracerMode.ExceptionsOnly, (HttpMessageParts) HttpMessageParts.RequestCookies, LogLevel.Warning)));

            var serviceProvider = services.BuildServiceProvider();
            var fixture = serviceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

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
                    .AddManagerFor<IReqResUserService>(),
                config => config
                    .WithAkavacheCacheHandler()
                    .AddDelegatingHandler(new FailingRequestHandler()));

            var serviceProvider = services.BuildServiceProvider();
            var reqResManager = serviceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            // Clear cache
            await reqResManager.ClearCacheAsync();

            // Defining a throwing request
            Func<Task> act = () => reqResManager.ExecuteAsync(api => api.GetUsersAsync(HttpStatusCode.BadRequest));

            // Calling it at first execution should throw as expected without any cached result
            var ex = await act.Should().ThrowAsync<ApizrException<ApiResult<User>>>();
            ex.And.CachedResult.Should().BeNull();

            // This one should succeed
            var result = await reqResManager.ExecuteAsync(api => api.GetUsersAsync());

            // and cache result in-memory
            result.Should().NotBeNull();
            result.Data.Should().NotBeNullOrEmpty();

            // This one should fail but with cached result
            var ex2 = await act.Should().ThrowAsync<ApizrException<ApiResult<User>>>();
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
                    .AddManagerFor<IReqResUserService>(),
                config => config
                    .WithInMemoryCacheHandler()
                    .AddDelegatingHandler(new FailingRequestHandler()));

            var serviceProvider = services.BuildServiceProvider();
            var reqResManager = serviceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            // Defining a throwing request
            Func<bool, Action<Exception>, Task<ApiResult<User>>> act = (clearCache, onException) => reqResManager.ExecuteAsync(api => api.GetUsersAsync(HttpStatusCode.BadRequest), clearCache, onException);

            // Calling it at first execution should throw as expected without any cached result
            var ex = await act.Invoking(x => x.Invoke(false, null)).Should().ThrowAsync<ApizrException<ApiResult<User>>>();
            ex.And.CachedResult.Should().BeNull();

            // This one should succeed
            var result = await reqResManager.ExecuteAsync(api => api.GetUsersAsync());

            // and cache result in-memory
            result.Should().NotBeNull();
            result.Data.Should().NotBeNullOrEmpty();

            // This one should fail but with cached result
            ex = await act.Invoking(x => x.Invoke(false, null)).Should().ThrowAsync<ApizrException<ApiResult<User>>>();
            ex.And.CachedResult.Should().NotBeNull();

            // this one should return cached result and handle exception
            result = await act.Invoke(false, e =>
            {
                // The handled exception with cached result on this side
                e.Should().BeOfType<ApizrException<ApiResult<User>>>().Which.CachedResult.Should().NotBeNull();
            });

            // The returned result on the other side
            result.Should().NotBeNull();

            // This one should fail but without any cached result as we asked for clearing it
            ex = await act.Invoking(x => x.Invoke(true, null)).Should().ThrowAsync<ApizrException<ApiResult<User>>>();
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
                    .AddManagerFor<IReqResUserService>(),
                config => config
                    .AddDelegatingHandler(new FailingRequestHandler()));

            var serviceProvider = services.BuildServiceProvider();
            var reqResManager = serviceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

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
                    .AddManagerFor<IReqResUserService>(),
                config => config
                    .WithConnectivityHandler(() => isConnected));

            var serviceProvider = services.BuildServiceProvider();
            var reqResManager = serviceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

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
                    .AddManagerFor<IReqResUserService>(),
                config => config
                    .WithRefitSettings(_refitSettings));

            var serviceProvider = services.BuildServiceProvider();
            var apizrOptions = serviceProvider.GetRequiredService<IApizrManagerOptions<IReqResUserService>>();
            
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
                    .AddManagerFor<IReqResUserService>(),
                config => config
                    .WithRefitSettings(_refitSettings)
                    .WithAutoMapperMappingHandler());

            var serviceProvider = services.BuildServiceProvider();
            var reqResManager = serviceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

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
                    .AddManagerFor<IReqResUserService>(),
                config => config
                    .WithRefitSettings(_refitSettings)
                    .WithMappingHandler<AutoMapperMappingHandler>());

            var serviceProvider = services.BuildServiceProvider();
            var reqResManager = serviceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

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
                    .AddManagerFor<IReqResUserService>(),
                config => config
                    .WithMediation());

            var serviceProvider = services.BuildServiceProvider();
            var reqResMediator = serviceProvider.GetRequiredService<IApizrMediator<IReqResUserService>>();

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
                    .AddManagerFor<IReqResUserService>(),
                config => config
                    .WithOptionalMediation());

            var serviceProvider = services.BuildServiceProvider();
            var reqResMediator = serviceProvider.GetRequiredService<IApizrOptionalMediator<IReqResUserService>>();

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
        
        [Fact]
        public void ServiceCollection_Should_Contain_Grouped_Registry_And_Managers()
        {
            var services = new ServiceCollection();
            services.AddApizr(registry => registry
                .AddGroup(group => group
                    .AddManagerFor<IReqResUserService>()
                    .AddManagerFor<IReqResResourceService>())
                .AddManagerFor<IHttpBinService>()
                .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>());

            services.Should().Contain(x => x.ServiceType == typeof(IApizrExtendedRegistry));
            services.Should().Contain(x => x.ServiceType == typeof(IApizrManager<IReqResUserService>));
            services.Should().Contain(x => x.ServiceType == typeof(IApizrManager<IReqResResourceService>));
            services.Should().Contain(x => x.ServiceType == typeof(IApizrManager<IHttpBinService>));
            services.Should().Contain(x => x.ServiceType == typeof(IApizrManager<ICrudApi<User, int, PagedResult<User>, IDictionary<string, object>>>));
        }

        [Fact]
        public void ServiceProvider_Should_Resolve_Grouped_Registry_And_Managers()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                .AddGroup(group => group
                    .AddManagerFor<IReqResUserService>()
                    .AddManagerFor<IReqResResourceAddressService>())
                .AddManagerFor<IHttpBinService>()
                .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>());

            var serviceProvider = services.BuildServiceProvider();
            var registry = serviceProvider.GetService<IApizrExtendedRegistry>();
            var reqResUserManager = serviceProvider.GetService<IApizrManager<IReqResUserService>>();
            var reqResResourceManager = serviceProvider.GetService<IApizrManager<IReqResResourceAddressService>>();
            var httpBinManager = serviceProvider.GetService<IApizrManager<IHttpBinService>>();
            var userManager = serviceProvider.GetService<IApizrManager<ICrudApi<User, int, PagedResult<User>, IDictionary<string, object>>>>();

            registry.Should().NotBeNull();
            reqResUserManager.Should().NotBeNull();
            reqResResourceManager.Should().NotBeNull();
            httpBinManager.Should().NotBeNull();
            userManager.Should().NotBeNull();
        }

        [Fact]
        public void Grouped_Registry_Should_Resolve_Managers()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                .AddGroup(group => group
                    .AddManagerFor<IReqResUserService>()
                    .AddManagerFor<IReqResResourceAddressService>())
                .AddManagerFor<IHttpBinService>()
                .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>());

            var serviceProvider = services.BuildServiceProvider();
            var registry = serviceProvider.GetRequiredService<IApizrExtendedRegistry>();

            registry.TryGetManagerFor<IReqResUserService>(out var reqResUserManager).Should().BeTrue();
            registry.TryGetManagerFor<IReqResResourceAddressService>(out var reqResResourceManager).Should().BeTrue();
            registry.TryGetManagerFor<IHttpBinService>(out var httpBinManager).Should().BeTrue();
            registry.TryGetCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(out var userManager).Should().BeTrue();

            reqResUserManager.Should().NotBeNull();
            reqResResourceManager.Should().NotBeNull();
            httpBinManager.Should().NotBeNull();
            userManager.Should().NotBeNull();
        }

        [Fact]
        public void Calling_WithBaseAddress_Grouped_Should_Set_BaseAddress()
        {
            var attributeUri = "https://reqres.in/api";
            var uri1 = new Uri("http://uri1.com");
            var uri2 = new Uri("http://uri2.com");
            var uri3 = new Uri("http://uri3.com");
            var uri4 = new Uri("http://uri4.com");

            // Test 1
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                    .AddGroup(group => group
                        .AddManagerFor<IReqResUserService>()
                        .AddManagerFor<IReqResResourceService>(),
                        config => config.WithBaseAddress(uri3))
                    .AddManagerFor<IHttpBinService>(options => options.WithBaseAddress(uri2)),
                config => config.WithBaseAddress(uri1));

            var serviceProvider = services.BuildServiceProvider();

            var userFixture = serviceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();
            userFixture.Options.BaseUri.Should().Be(attributeUri);

            var resourceFixture = serviceProvider.GetRequiredService<IApizrManager<IReqResResourceService>>();
            resourceFixture.Options.BaseUri.Should().Be(uri3);

            // Test 2
            services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                    .AddGroup(group => group
                            .AddManagerFor<IReqResUserService>(config => config.WithBaseAddress(uri4))
                            .AddManagerFor<IReqResResourceService>(),
                        config => config.WithBaseAddress(uri3))
                    .AddManagerFor<IHttpBinService>(options => options.WithBaseAddress(uri2)),
                config => config.WithBaseAddress(uri1));

            serviceProvider = services.BuildServiceProvider();

            userFixture = serviceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();
            userFixture.Options.BaseUri.Should().Be(uri4);

            resourceFixture = serviceProvider.GetRequiredService<IApizrManager<IReqResResourceService>>();
            resourceFixture.Options.BaseUri.Should().Be(uri3);
        }
    }
}
