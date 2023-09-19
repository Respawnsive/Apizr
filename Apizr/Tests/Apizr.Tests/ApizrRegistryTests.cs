using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring;
using Apizr.Configuring.Registry;
using Apizr.Extending;
using Apizr.Extending.Configuring.Registry;
using Apizr.Logging;
using Apizr.Policing;
using Apizr.Progressing;
using Apizr.Tests.Apis;
using Apizr.Tests.Helpers;
using Apizr.Tests.Models;
using Apizr.Tests.Models.Mappings;
using Apizr.Tests.Settings;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;
using AutoMapper;
using FluentAssertions;
using Fusillade;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MonkeyCache.FileStore;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;
using Refit;
using Xunit;

namespace Apizr.Tests
{
    public class ApizrRegistryTests
    {
        private readonly RefitSettings _refitSettings;

        public ApizrRegistryTests()
        {
            var opts = new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };
            _refitSettings = new RefitSettings(new SystemTextJsonContentSerializer(opts));

            Barrel.ApplicationId = nameof(ApizrExtendedRegistryTests);
        }

        [Fact]
        public void ApizrRegistry_Should_Contain_Managers()
        {
            Func<IApizrRegistry> registryFactory = () => ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddManagerFor<IReqResUserService>()
                .AddManagerFor<IHttpBinService>()

                .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>()

                .AddUploadManager(options => options.WithBaseAddress("https://test.com"))
                .AddUploadManagerFor<ITransferSampleApi>()
                .AddUploadManagerWith<string>(options => options.WithBaseAddress("https://test.com"))

                .AddDownloadManager(options => options.WithBaseAddress("https://test.com"))
                .AddDownloadManagerFor<ITransferSampleApi>()
                .AddDownloadManagerWith<User>(options => options.WithBaseAddress("https://test.com"))

                .AddTransferManager(options => options.WithBaseAddress("https://test.com"))
                .AddTransferManagerFor<ITransferSampleApi>()
                .AddTransferManagerWith<User, string>(options => options.WithBaseAddress("https://test.com")));

            var registry = registryFactory.Should().NotThrow().Which.Should().BeOfType<ApizrRegistry>().Which;

            registry.ContainsManagerFor<IReqResUserService>().Should().BeTrue();
            registry.ContainsManagerFor<IHttpBinService>().Should().BeTrue();

            registry.ContainsCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>().Should().BeTrue();
            
            registry.ContainsUploadManager().Should().BeTrue();
            registry.ContainsUploadManagerFor<ITransferSampleApi>().Should().BeTrue();
            registry.ContainsUploadManagerWith<string>().Should().BeTrue();
            
            registry.ContainsDownloadManager().Should().BeTrue();
            registry.ContainsDownloadManagerFor<ITransferSampleApi>().Should().BeTrue();
            registry.ContainsDownloadManagerWith<User>().Should().BeTrue();
            
            registry.ContainsTransferManager().Should().BeTrue();
            registry.ContainsTransferManagerFor<ITransferSampleApi>().Should().BeTrue();
            registry.ContainsTransferManagerWith<User, string>().Should().BeTrue();
        }

        [Fact]
        public void ApizrRegistry_Should_Get_Managers()
        {
            var registry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddManagerFor<IReqResUserService>()
                .AddManagerFor<IHttpBinService>()

                .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>()

                .AddUploadManager(options => options.WithBaseAddress("https://test.com"))
                .AddUploadManagerFor<ITransferSampleApi>()
                .AddUploadManagerWith<string>(options => options.WithBaseAddress("https://test.com"))

                .AddDownloadManager(options => options.WithBaseAddress("https://test.com"))
                .AddDownloadManagerFor<ITransferSampleApi>()
                .AddDownloadManagerWith<User>(options => options.WithBaseAddress("https://test.com"))

                .AddTransferManager(options => options.WithBaseAddress("https://test.com"))
                .AddTransferManagerFor<ITransferSampleApi>()
                .AddTransferManagerWith<User, string>(options => options.WithBaseAddress("https://test.com")));

            registry.TryGetManagerFor<IReqResUserService>(out _).Should().BeTrue();
            registry.TryGetManagerFor<IHttpBinService>(out _).Should().BeTrue();

            registry.TryGetCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(out _).Should().BeTrue();

            registry.TryGetUploadManager(out _).Should().BeTrue();
            registry.TryGetUploadManagerFor<ITransferSampleApi>(out _).Should().BeTrue();
            registry.TryGetUploadManagerWith<string>(out _).Should().BeTrue();

            registry.TryGetDownloadManager(out _).Should().BeTrue();
            registry.TryGetDownloadManagerFor<ITransferSampleApi>(out _).Should().BeTrue();
            registry.TryGetDownloadManagerWith<User>(out _).Should().BeTrue();

            registry.TryGetTransferManager(out _).Should().BeTrue();
            registry.TryGetTransferManagerFor<ITransferSampleApi>(out _).Should().BeTrue();
            registry.TryGetTransferManagerWith<User, string>(out _).Should().BeTrue();
        }

        [Fact]
        public void ApizrRegistry_Should_Populate_Managers()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddManagerFor<IReqResUserService>()
                .AddManagerFor<IHttpBinService>()
                .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>());

            var count = 0;
            apizrRegistry.Populate((type, factory) =>
            {
                type.Should().NotBeNull();
                factory.Should().NotBeNull();
                var manager = factory.Invoke();
                manager.Should().NotBeNull();
                manager.GetType().Should().BeAssignableTo(type);
                count++;
            });

            count.Should().Be(apizrRegistry.Count);
        }

        [Fact]
        public void Calling_WithBaseAddress_Should_Set_BaseUri()
        {
            var attributeUri = "https://reqres.in/api";
            var uri1 = new Uri("http://uri1.com");
            var uri2 = new Uri("http://uri2.com");

            // By attribute
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>());
            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();
            reqResManager.Options.BaseUri.Should().Be(attributeUri);

            // By attribute overriding common option
            apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(),
                config => config.WithBaseAddress(uri2));
            reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();
            reqResManager.Options.BaseUri.Should().Be(attributeUri);

            // By proper option overriding attribute
            apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(options =>
                        options.WithBaseAddress(uri2)));

            reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();
            reqResManager.Options.BaseUri.Should().Be(uri2);

            // By proper option overriding common option and attribute
            apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddManagerFor<IReqResUserService>(options => 
                    options.WithBaseAddress(uri2)),
                config => config.WithBaseAddress(uri1));

            reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();
            reqResManager.Options.BaseUri.Should().Be(uri2);
        }

        [Fact]
        public void Calling_WithBaseAddress_And_WithBasePath_Grouped_Should_Set_BaseUri()
        {
            var attributeUri = "https://reqres.in/api";
            var uri1 = new Uri("http://uri1.com");
            var uri2 = new Uri("http://uri2.com");
            var uri3 = new Uri("http://uri3.com");
            var uri4 = new Uri("http://uri4.com");
            var path = "users";
            var fullUri3 = $"{uri3}{path}";
            var fullUri4 = $"{uri4}{path}";

            // By common option overriding attribute
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddGroup(group => group
                            .AddManagerFor<IReqResUserService>()
                            .AddManagerFor<IReqResUserPathService>(config => config.WithBasePath(path)) // completing with base path
                            .AddManagerFor<IReqResResourceService>(),
                        config => config.WithBaseAddress(uri3))
                    .AddManagerFor<IHttpBinService>(options => options.WithBaseAddress(uri2)),
                config => config.WithBaseAddress(uri1));

            var userFixture = apizrRegistry.GetManagerFor<IReqResUserService>();
            userFixture.Options.BaseUri.Should().Be(attributeUri);

            var userPathFixture = apizrRegistry.GetManagerFor<IReqResUserPathService>();
            userPathFixture.Options.BaseUri.Should().Be(fullUri3);

            var resourceFixture = apizrRegistry.GetManagerFor<IReqResResourceService>();
            resourceFixture.Options.BaseUri.Should().Be(uri3);

            // By proper option overriding all common options and attribute
            apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddGroup(group => group
                            .AddManagerFor<IReqResUserService>(config => config.WithBaseAddress(uri4)) // changing base uri
                            .AddManagerFor<IReqResUserPathService>(config => config.WithBaseAddress(uri4).WithBasePath(path)) // changing base uri completing with base path
                            .AddManagerFor<IReqResResourceService>(),
                        config => config.WithBaseAddress(uri3))
                    .AddManagerFor<IHttpBinService>(options => options.WithBaseAddress(uri2)),
                config => config.WithBaseAddress(uri1));


            userFixture = apizrRegistry.GetManagerFor<IReqResUserService>();
            userFixture.Options.BaseUri.Should().Be(uri4);

            userPathFixture = apizrRegistry.GetManagerFor<IReqResUserPathService>();
            userPathFixture.Options.BaseUri.Should().Be(fullUri4);

            resourceFixture = apizrRegistry.GetManagerFor<IReqResResourceService>();
            resourceFixture.Options.BaseUri.Should().Be(uri3);
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_ProperOption_Should_Authenticate_Request()
        {
            string token = null;

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(
                registry => registry
                    .AddManagerFor<IHttpBinService>(options =>
                        options.WithAuthenticationHandler(_ => Task.FromResult(token = "token"))));

            var httpBinManager = apizrRegistry.GetManagerFor<IHttpBinService>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
            token.Should().Be("token");
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_With_Default_Token_Should_Authenticate_Request()
        {
            var testSettings = new TestSettings("token");

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(
                registry => registry
                    .AddManagerFor<IHttpBinService>(options =>
                        options.WithAuthenticationHandler(testSettings, settings => settings.TestJsonString)));

            var httpBinManager = apizrRegistry.GetManagerFor<IHttpBinService>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public void Calling_WithLogging_Should_Set_LoggingSettings()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddManagerFor<IReqResUserService>(options => options.WithLogging((HttpTracerMode) HttpTracerMode.ExceptionsOnly, (HttpMessageParts) HttpMessageParts.RequestCookies, LogLevel.Warning)));

            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

            reqResManager.Options.HttpTracerMode.Should().Be(HttpTracerMode.ExceptionsOnly);
            reqResManager.Options.TrafficVerbosity.Should().Be(HttpMessageParts.RequestCookies);
            reqResManager.Options.LogLevels.Should().AllBeEquivalentTo(LogLevel.Warning);
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_CommonOption_Should_Authenticate_Request()
        {
            string token = null;

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IHttpBinService>(),
                config => config
                    .WithAuthenticationHandler(_ => Task.FromResult(token = "token")));

            var httpBinManager = apizrRegistry.GetManagerFor<IHttpBinService>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
            token.Should().Be("token");
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_Should_Authenticate_Request()
        {
            string token = null;

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IHttpBinService>(options => options
                        .WithAuthenticationHandler(_ => Task.FromResult(token = "tokenA"))),
                config => config
                    .WithAuthenticationHandler(_ => Task.FromResult(token = "tokenB")));

            var httpBinManager = apizrRegistry.GetManagerFor<IHttpBinService>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
            token.Should().Be("tokenA");
        }

        [Fact]
        public async Task Calling_WithAkavacheCacheHandler_Should_Cache_Result()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(),
                config => config
                    .WithAkavacheCacheHandler()
                    .AddDelegatingHandler(new TestRequestHandler()));

            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

            // Clearing cache
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
                    "TransientHttpError", HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(
                        sleepDurations,
                        (_, _, retry, _) => attempts = retry).WithPolicyKey("TransientHttpError")
                }
            };

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(),
                config => config
                    .WithPolicyRegistry(policyRegistry)
                    .AddDelegatingHandler(new TestRequestHandler()));

            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

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

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(),
                config => config
                    .WithConnectivityHandler(() => isConnected));

            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

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
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(),
                config => config
                    .WithRefitSettings(_refitSettings));

            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

            reqResManager.Options.RefitSettings.Should().Be(_refitSettings);
        }

        [Fact]
        public async Task Calling_WithAutoMapperMappingHandler_Should_Map_Data()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<UserDetailsUserInfosProfile>();
                config.AddProfile<UserMinUserProfile>();
            });

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(),
                config => config
                    .WithRefitSettings(_refitSettings)
                    .WithAutoMapperMappingHandler(mapperConfig));

            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

            var minUser = new MinUser { Name = "John" };

            // This one should succeed
            var result =
                await reqResManager.ExecuteAsync<MinUser, User>(
                    (api, user) => api.CreateUser(user, CancellationToken.None), minUser);

            result.Should().NotBeNull();
            result.Name.Should().Be(minUser.Name);
            result.Id.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task Calling_WithMappingHandler_With_AutoMapper_Should_Map_Data()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<UserDetailsUserInfosProfile>();
                config.AddProfile<UserMinUserProfile>();
            });

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(),
                config => config
                    .WithRefitSettings(_refitSettings)
                    .WithMappingHandler(new AutoMapperMappingHandler(mapperConfig.CreateMapper())));

            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

            var minUser = new MinUser { Name = "John" };

            // This one should succeed
            var result =
                await reqResManager.ExecuteAsync<MinUser, User>(
                    (api, user) => api.CreateUser(user, CancellationToken.None), minUser);

            result.Should().NotBeNull();
            result.Name.Should().Be(minUser.Name);
            result.Id.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task Calling_WithMapsterMappingHandler_Should_Map_Data()
        {
            TypeAdapterConfig<User, MinUser>
                .NewConfig()
                .TwoWays()
                .Map(minUser => minUser.Name, user => user.FirstName);

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(),
                config => config
                    .WithRefitSettings(_refitSettings)
                    .WithMapsterMappingHandler(new MapsterMapper.Mapper()));

            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

            var minUser = new MinUser { Name = "John" };

            // This one should succeed
            var result =
                await reqResManager.ExecuteAsync<MinUser, User>(
                    (api, user) => api.CreateUser(user, CancellationToken.None), minUser);

            result.Should().NotBeNull();
            result.Name.Should().Be(minUser.Name);
            result.Id.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task Calling_WithMappingHandler_With_Mapster_Should_Map_Data()
        {
            TypeAdapterConfig<User, MinUser>
                .NewConfig()
                .TwoWays()
                .Map(minUser => minUser.Name, user => user.FirstName);

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(),
                config => config
                    .WithRefitSettings(_refitSettings)
                    .WithMappingHandler(new MapsterMappingHandler(new MapsterMapper.Mapper())));

            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

            var minUser = new MinUser { Name = "John" };

            // This one should succeed
            var result =
                await reqResManager.ExecuteAsync<MinUser, User>(
                    (api, user) => api.CreateUser(user, CancellationToken.None), minUser);

            result.Should().NotBeNull();
            result.Name.Should().Be(minUser.Name);
            result.Id.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public void Grouped_ApizrRegistry_Should_Contain_Managers()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddGroup(group => group
                    .AddManagerFor<IReqResUserService>()
                    .AddManagerFor<IReqResResourceService>())
                .AddManagerFor<IHttpBinService>()
                .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>());

            apizrRegistry.Should().NotBeNull();
            apizrRegistry.ContainsManagerFor<IReqResUserService>().Should().BeTrue();
            apizrRegistry.ContainsManagerFor<IReqResResourceService>().Should().BeTrue();
            apizrRegistry.ContainsManagerFor<IHttpBinService>().Should().BeTrue();
            apizrRegistry.ContainsCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>().Should()
                .BeTrue();
        }

        [Fact]
        public void Grouped_ApizrRegistry_Should_Get_Managers()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddGroup(group => group
                    .AddManagerFor<IReqResUserService>()
                    .AddManagerFor<IReqResResourceService>())
                .AddManagerFor<IHttpBinService>()
                .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>());

            apizrRegistry.TryGetManagerFor<IReqResUserService>(out var userReqResManager).Should().BeTrue();
            apizrRegistry.TryGetManagerFor<IReqResResourceService>(out var resourceReqResManager).Should().BeTrue();
            apizrRegistry.TryGetManagerFor<IHttpBinService>(out var httpBinManager).Should().BeTrue();
            apizrRegistry.TryGetCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(out var userManager)
                .Should().BeTrue();

            userReqResManager.Should().NotBeNull();
            resourceReqResManager.Should().NotBeNull();
            httpBinManager.Should().NotBeNull();
            userManager.Should().NotBeNull();
        }

        [Fact]
        public void Grouped_ApizrRegistry_Should_Populate_Managers()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddGroup(group => group
                    .AddManagerFor<IReqResUserService>()
                    .AddManagerFor<IReqResResourceService>())
                .AddManagerFor<IHttpBinService>()
                .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>());

            var count = 0;
            apizrRegistry.Populate((type, factory) =>
            {
                type.Should().NotBeNull();
                factory.Should().NotBeNull();
                var manager = factory.Invoke();
                manager.Should().NotBeNull();
                manager.GetType().Should().BeAssignableTo(type);
                count++;
            });

            count.Should().Be(apizrRegistry.Count);
        }

        [Fact]
        public async Task Requesting_With_Context_into_Options_Should_Set_Context()
        {
            var watcher = new WatchingRequestHandler();

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry.AddManagerFor<IReqResUserService>(options => options.AddDelegatingHandler(watcher)));
            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

            var testKey = "TestKey1";
            var testValue = 1;
            // Defining Context
            var context = new Context { { testKey, testValue } };

            await reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt), options => options.WithContext(context));
            watcher.Context.Should().NotBeNull();
            watcher.Context.Keys.Should().Contain(testKey);
            watcher.Context.TryGetValue(testKey, out var value).Should().BeTrue();
            value.Should().Be(testValue);
        }

        [Fact]
        public async Task Requesting_With_Context_At_Multiple_Levels_Should_Merge_It_All_At_The_End()
        {
            var watcher = new WatchingRequestHandler();
            var testKey1 = "TestKey1";
            var testValue1 = 1;

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry.AddManagerFor<IReqResUserService>(options =>
                options.WithContext(() => new Context { { testKey1, testValue1 } })
                    .AddDelegatingHandler(watcher)));
            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

            var testKey2 = "TestKey2";
            var testValue2 = 2;
            // Defining Context 2
            var context2 = new Context { { testKey2, testValue2 } };

            await reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt),
                options => options.WithContext(context2));

            watcher.Context.Should().NotBeNull();
            watcher.Context.Keys.Should().Contain(testKey1);
            watcher.Context.TryGetValue(testKey1, out var value1).Should().BeTrue();
            value1.Should().Be(testValue1);
            watcher.Context.Keys.Should().Contain(testKey2);
            watcher.Context.TryGetValue(testKey2, out var value2).Should().BeTrue();
            value2.Should().Be(testValue2);
        }

        [Fact]
        public async Task Requesting_With_LogSettings_Into_Options_Should_Win_Over_All_Others()
        {
            var watcher = new WatchingRequestHandler();

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry.AddManagerFor<IReqResUserService>(options => options.AddDelegatingHandler(watcher)));
            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

            await reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt), options => options.WithLogging(HttpTracerMode.ExceptionsOnly, HttpMessageParts.RequestCookies));
            watcher.Context.Should().NotBeNull();
            watcher.Context.TryGetLogger(out var logger, out var logLevels, out var verbosity, out var tracerMode)
                .Should().BeTrue();
            verbosity.Should().Be(HttpMessageParts.RequestCookies);
            tracerMode.Should().Be(HttpTracerMode.ExceptionsOnly);

            watcher.Options.Should().NotBeNull();
            watcher.Options.TrafficVerbosity.Should().Be(HttpMessageParts.RequestCookies);
            watcher.Options.HttpTracerMode.Should().Be(HttpTracerMode.ExceptionsOnly);
        }

        [Fact]
        public async Task Configuring_Exceptions_Handler_Should_Handle_Exceptions()
        {
            var handledException = 0;
            void OnException(ApizrException ex)
            {
                handledException++;
            }

            // Try to queue ex handlers
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddGroup(group => group
                            .AddManagerFor<IReqResUserService>(options => options
                                .WithExCatching(OnException, strategy: ApizrDuplicateStrategy.Add)
                                .AddDelegatingHandler(new TestRequestHandler()))
                            .AddManagerFor<IReqResResourceService>(),
                        options => options.WithExCatching(OnException, strategy: ApizrDuplicateStrategy.Add))
                    .AddManagerFor<IHttpBinService>()
                    .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(),
                options => options.WithExCatching(OnException, strategy: ApizrDuplicateStrategy.Add));


            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

            // Defining a transient throwing request
            Func<Task> act = () => reqResManager.ExecuteAsync(api => api.GetUsersAsync(HttpStatusCode.RequestTimeout),
                options => options.WithExCatching(OnException, strategy: ApizrDuplicateStrategy.Add));

            // Calling it should throw but handled by Polly
            await act.Should().ThrowAsync<ApizrException>();
            
            handledException.Should().Be(4);

            // Try to replace queued ex handlers by the last one set at request time
            handledException = 0;

            // Defining a transient throwing request
            act = () => reqResManager.ExecuteAsync(api => api.GetUsersAsync(HttpStatusCode.RequestTimeout),
                options => options.WithExCatching(OnException, strategy: ApizrDuplicateStrategy.Replace));

            // Calling it should throw but handled by Polly
            await act.Should().ThrowAsync<ApizrException>();

            handledException.Should().Be(1);
        }

        [Fact]
        public async Task Requesting_With_CancellationToken_Into_Options_Should_Cancel_Request_When_Asked()
        {
            var watcher = new WatchingRequestHandler();

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry.AddManagerFor<IReqResUserService>(options => options.AddDelegatingHandler(watcher)));
            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

            var cts = new CancellationTokenSource(3000);
            Func<Task> act = () => reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt),
                options => options.WithCancellation(cts.Token));
            
            var res = await act.Should().ThrowAsync<ApizrException>();
            res.WithInnerException<TaskCanceledException>();
        }

        [Fact]
        public async Task Configuring_Priority_Should_Prioritize_Request()
        {
            var watcher1 = new WatchingRequestHandler { Context = new Context("watcher1") };
            var watcher2 = new WatchingRequestHandler { Context = new Context("watcher2") };

            // Try to configure priority
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddGroup(group => group
                            .AddManagerFor<IReqResUserService>(options => options
                                .AddDelegatingHandler(watcher1)
                                .WithPriority(Priority.UserInitiated))
                            .AddManagerFor<IReqResResourceService>(),
                        options => options.WithPriority(Priority.Background))
                    .AddManagerFor<IHttpBinService>()
                    .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(options => options
                        .WithBaseAddress("https://reqres.in/api/users")
                        .AddDelegatingHandler(watcher2)
                        .WithPriority(Priority.Speculative)),
                options => options
                    .WithPriority());
            
            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

            // Defining a transient throwing request
            await reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt),
                options => options.WithPriority(Priority.Speculative));


            watcher1.Options.Should().NotBeNull();
            watcher1.Options.HandlersParameters.Should().NotBeNullOrEmpty();
            watcher1.Options.HandlersParameters.TryGetValue(Constants.PriorityKey, out var priority).Should().BeTrue();
            priority.Should().Be((int)Priority.Speculative);

            var userManager = apizrRegistry.GetCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>();
            await userManager.ExecuteAsync((opt, api) => api.ReadAll(opt),
                options => options.WithPriority(Priority.UserInitiated));

            watcher2.Options.Should().NotBeNull();
            watcher2.Options.HandlersParameters.Should().NotBeNullOrEmpty();
            watcher2.Options.HandlersParameters.TryGetValue(Constants.PriorityKey, out priority).Should().BeTrue();
            priority.Should().Be((int)Priority.UserInitiated);
        }

        [Fact]
        public async Task Downloading_File_Should_Succeed()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddTransferManager()
                .AddTransferManagerFor<ITransferUndefinedApi>()
                .AddDownloadManager()
                .AddDownloadManagerFor<ITransferUndefinedApi>(),
                options => options
                        .WithBaseAddress("http://speedtest.ftp.otenet.gr/files"));

            apizrRegistry.TryGetTransferManager(out var regTransferManager).Should().BeTrue(); // Built-in
            apizrRegistry.TryGetTransferManagerFor<ITransferApi>(out var regTransferTypedManager).Should().BeTrue(); // Built-in
            apizrRegistry.TryGetTransferManagerFor<ITransferUndefinedApi>(out var regCustomTransferTypedManager).Should().BeTrue(); // Custom
            apizrRegistry.TryGetDownloadManager(out var regDownloadManager).Should().BeTrue(); // Built-in
            apizrRegistry.TryGetDownloadManagerFor<IDownloadApi>(out var regDownloadTypedManager).Should().BeTrue(); // Built-in
            apizrRegistry.TryGetDownloadManagerFor<ITransferUndefinedApi>(out var regCustomDownloadTypedManager).Should().BeTrue(); // Custom

            regTransferManager.Should().NotBeNull(); // Built-in
            regTransferTypedManager.Should().NotBeNull(); // Built-in
            regCustomTransferTypedManager.Should().NotBeNull(); // Custom
            regDownloadManager.Should().NotBeNull(); // Built-in
            regDownloadTypedManager.Should().NotBeNull(); // Built-in
            regCustomDownloadTypedManager.Should().NotBeNull(); // Custom

            // Shortcut
            var regShortcutResult = await apizrRegistry.DownloadAsync(new FileInfo("test100k.db"));
            regShortcutResult.Should().NotBeNull();
            regShortcutResult.Length.Should().BePositive();

            // Transfer
            // Built-in
            var regTransferManagerResult = await regTransferManager.DownloadAsync(new FileInfo("test100k.db"));
            regTransferManagerResult.Should().NotBeNull();
            regTransferManagerResult.Length.Should().BePositive();

            // Built-in
            var regTransferTypedManagerResult = await regTransferTypedManager.DownloadAsync(new FileInfo("test100k.db"));
            regTransferTypedManagerResult.Should().NotBeNull();
            regTransferTypedManagerResult.Length.Should().BePositive();

            // Custom
            var regCustomTransferTypedManagerResult = await regCustomTransferTypedManager.DownloadAsync(new FileInfo("test100k.db"));
            regCustomTransferTypedManagerResult.Should().NotBeNull();
            regCustomTransferTypedManagerResult.Length.Should().BePositive();

            // Download
            // Built-in
            var regDownloadManagerResult = await regDownloadManager.DownloadAsync(new FileInfo("test100k.db"));
            regDownloadManagerResult.Should().NotBeNull();
            regDownloadManagerResult.Length.Should().BePositive();

            // Built-in
            var regDownloadTypedManagerResult = await regDownloadTypedManager.DownloadAsync(new FileInfo("test100k.db"));
            regDownloadTypedManagerResult.Should().NotBeNull();
            regDownloadTypedManagerResult.Length.Should().BePositive();

            // Custom
            var regCustomDownloadTypedManagerResult = await regCustomDownloadTypedManager.DownloadAsync(new FileInfo("test100k.db"));
            regCustomDownloadTypedManagerResult.Should().NotBeNull();
            regCustomDownloadTypedManagerResult.Length.Should().BePositive();
        }

        [Fact]
        public async Task Downloading_File_Grouped_Should_Succeed()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddGroup(group => group
                        .AddTransferManager()
                        .AddTransferManagerFor<ITransferUndefinedApi>()
                        .AddDownloadManager()
                        .AddDownloadManagerFor<ITransferUndefinedApi>(),
                    options => options.WithBasePath("/files")),
                options => options.WithBaseAddress("http://speedtest.ftp.otenet.gr"));

            apizrRegistry.TryGetTransferManager(out var regTransferManager).Should().BeTrue(); // Built-in
            apizrRegistry.TryGetTransferManagerFor<ITransferApi>(out var regTransferTypedManager).Should().BeTrue(); // Built-in
            apizrRegistry.TryGetTransferManagerFor<ITransferUndefinedApi>(out var regCustomTransferTypedManager).Should().BeTrue(); // Custom
            apizrRegistry.TryGetDownloadManager(out var regDownloadManager).Should().BeTrue(); // Built-in
            apizrRegistry.TryGetDownloadManagerFor<IDownloadApi>(out var regDownloadTypedManager).Should().BeTrue(); // Built-in
            apizrRegistry.TryGetDownloadManagerFor<ITransferUndefinedApi>(out var regCustomDownloadTypedManager).Should().BeTrue(); // Custom

            regTransferManager.Should().NotBeNull(); // Built-in
            regTransferTypedManager.Should().NotBeNull(); // Built-in
            regCustomTransferTypedManager.Should().NotBeNull(); // Custom
            regDownloadManager.Should().NotBeNull(); // Built-in
            regDownloadTypedManager.Should().NotBeNull(); // Built-in
            regCustomDownloadTypedManager.Should().NotBeNull(); // Custom

            // Shortcut
            var regShortcutResult = await apizrRegistry.DownloadAsync(new FileInfo("test100k.db"));
            regShortcutResult.Should().NotBeNull();
            regShortcutResult.Length.Should().BePositive();

            // Transfer
            // Built-in
            var regTransferManagerResult = await regTransferManager.DownloadAsync(new FileInfo("test100k.db"));
            regTransferManagerResult.Should().NotBeNull();
            regTransferManagerResult.Length.Should().BePositive();

            // Built-in
            var regTransferTypedManagerResult = await regTransferTypedManager.DownloadAsync(new FileInfo("test100k.db"));
            regTransferTypedManagerResult.Should().NotBeNull();
            regTransferTypedManagerResult.Length.Should().BePositive();

            // Custom
            var regCustomTransferTypedManagerResult = await regCustomTransferTypedManager.DownloadAsync(new FileInfo("test100k.db"));
            regCustomTransferTypedManagerResult.Should().NotBeNull();
            regCustomTransferTypedManagerResult.Length.Should().BePositive();

            // Download
            // Built-in
            var regDownloadManagerResult = await regDownloadManager.DownloadAsync(new FileInfo("test100k.db"));
            regDownloadManagerResult.Should().NotBeNull();
            regDownloadManagerResult.Length.Should().BePositive();

            // Built-in
            var regDownloadTypedManagerResult = await regDownloadTypedManager.DownloadAsync(new FileInfo("test100k.db"));
            regDownloadTypedManagerResult.Should().NotBeNull();
            regDownloadTypedManagerResult.Length.Should().BePositive();

            // Custom
            var regCustomDownloadTypedManagerResult = await regCustomDownloadTypedManager.DownloadAsync(new FileInfo("test100k.db"));
            regCustomDownloadTypedManagerResult.Should().NotBeNull();
            regCustomDownloadTypedManagerResult.Length.Should().BePositive();
        }

        [Fact]
        public async Task Downloading_File_With_Local_Progress_Should_Report_Progress()
        {
            var percentage = 0;
            var progress = new ApizrProgress();
            progress.ProgressChanged += (sender, args) =>
            {
                percentage = args.ProgressPercentage;
            };
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddTransferManager(options => options
                    .WithBaseAddress("http://speedtest.ftp.otenet.gr/files")
                    .WithProgress()));

            apizrRegistry.TryGetTransferManager(out var regTransferManager).Should().BeTrue(); // Built-in
            regTransferManager.Should().NotBeNull(); // Built-in

            var fileInfo = await regTransferManager.DownloadAsync(new FileInfo("test10Mb.db"), options => options.WithProgress(progress)).ConfigureAwait(false);

            percentage.Should().Be(100);
            fileInfo.Length.Should().BePositive();
        }

        [Fact]
        public async Task Downloading_File_With_Global_Progress_Should_Report_Progress()
        {
            var percentage = 0;
            var progress = new ApizrProgress();
            progress.ProgressChanged += (sender, args) =>
            {
                percentage = args.ProgressPercentage;
            };
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddTransferManager(options => options
                    .WithBaseAddress("http://speedtest.ftp.otenet.gr/files")
                    .WithProgress(progress)));

            apizrRegistry.TryGetTransferManager(out var regTransferManager).Should().BeTrue(); // Built-in
            regTransferManager.Should().NotBeNull(); // Built-in

            var fileInfo = await regTransferManager.DownloadAsync(new FileInfo("test10Mb.db")).ConfigureAwait(false);

            percentage.Should().Be(100);
            fileInfo.Length.Should().BePositive();
        }

        [Fact]
        public async Task Uploading_File_Should_Succeed()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddTransferManager()
                .AddTransferManagerFor<ITransferUndefinedApi>()
                .AddUploadManager()
                .AddUploadManagerFor<ITransferUndefinedApi>(),
                options => options
                        .WithBaseAddress("https://httpbin.org/post"));

            apizrRegistry.TryGetTransferManager(out var regTransferManager).Should().BeTrue(); // Built-in
            apizrRegistry.TryGetTransferManagerFor<ITransferApi>(out var regTransferTypedManager).Should().BeTrue(); // Built-in
            apizrRegistry.TryGetTransferManagerFor<ITransferUndefinedApi>(out var regCustomTransferTypedManager).Should().BeTrue(); // Custom
            apizrRegistry.TryGetUploadManager(out var regUploadManager).Should().BeTrue(); // Built-in
            apizrRegistry.TryGetUploadManagerFor<IUploadApi>(out var regUploadTypedManager).Should().BeTrue(); // Built-in
            apizrRegistry.TryGetUploadManagerFor<ITransferUndefinedApi>(out var regCustomUploadTypedManager).Should().BeTrue(); // Custom

            regTransferManager.Should().NotBeNull(); // Built-in
            regTransferTypedManager.Should().NotBeNull(); // Built-in
            regCustomTransferTypedManager.Should().NotBeNull(); // Custom
            regUploadManager.Should().NotBeNull(); // Built-in
            regUploadTypedManager.Should().NotBeNull(); // Built-in
            regCustomUploadTypedManager.Should().NotBeNull(); // Custom

            // Shortcut
            var regShortcutResult = await apizrRegistry.UploadAsync<IUploadApi, HttpResponseMessage>(FileHelper.GetTestFileStreamPart("small"));
            regShortcutResult.Should().NotBeNull();
            regShortcutResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Transfer
            // Built-in
            var regTransferManagerResult = await regTransferManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            regTransferManagerResult.Should().NotBeNull();
            regTransferManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Built-in
            var regTransferTypedManagerResult = await regTransferTypedManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            regTransferTypedManagerResult.Should().NotBeNull();
            regTransferTypedManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Custom
            var regCustomTransferTypedManagerResult = await regCustomTransferTypedManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            regCustomTransferTypedManagerResult.Should().NotBeNull();
            regCustomTransferTypedManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Upload
            // Built-in
            var regUploadManagerResult = await regUploadManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            regUploadManagerResult.Should().NotBeNull();
            regUploadManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Built-in
            var regUploadTypedManagerResult = await regUploadTypedManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            regUploadTypedManagerResult.Should().NotBeNull();
            regUploadTypedManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Custom
            var regCustomUploadTypedManagerResult = await regCustomUploadTypedManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            regCustomUploadTypedManagerResult.Should().NotBeNull();
            regCustomUploadTypedManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Uploading_File_Grouped_Should_Succeed()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddGroup(group => group
                        .AddTransferManager()
                        .AddTransferManagerFor<ITransferUndefinedApi>()
                        .AddUploadManager()
                        .AddUploadManagerFor<ITransferUndefinedApi>()),
                options => options
                    .WithBaseAddress("https://httpbin.org/post"));

            apizrRegistry.TryGetTransferManager(out var regTransferManager).Should().BeTrue(); // Built-in
            apizrRegistry.TryGetTransferManagerFor<ITransferApi>(out var regTransferTypedManager).Should().BeTrue(); // Built-in
            apizrRegistry.TryGetTransferManagerFor<ITransferUndefinedApi>(out var regCustomTransferTypedManager).Should().BeTrue(); // Custom
            apizrRegistry.TryGetUploadManager(out var regUploadManager).Should().BeTrue(); // Built-in
            apizrRegistry.TryGetUploadManagerFor<IUploadApi>(out var regUploadTypedManager).Should().BeTrue(); // Built-in
            apizrRegistry.TryGetUploadManagerFor<ITransferUndefinedApi>(out var regCustomUploadTypedManager).Should().BeTrue(); // Custom

            regTransferManager.Should().NotBeNull(); // Built-in
            regTransferTypedManager.Should().NotBeNull(); // Built-in
            regCustomTransferTypedManager.Should().NotBeNull(); // Custom
            regUploadManager.Should().NotBeNull(); // Built-in
            regUploadTypedManager.Should().NotBeNull(); // Built-in
            regCustomUploadTypedManager.Should().NotBeNull(); // Custom

            // Shortcut
            var regShortcutResult = await apizrRegistry.UploadAsync<IUploadApi, HttpResponseMessage>(FileHelper.GetTestFileStreamPart("small"));
            regShortcutResult.Should().NotBeNull();
            regShortcutResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Transfer
            // Built-in
            var regTransferManagerResult = await regTransferManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            regTransferManagerResult.Should().NotBeNull();
            regTransferManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Built-in
            var regTransferTypedManagerResult = await regTransferTypedManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            regTransferTypedManagerResult.Should().NotBeNull();
            regTransferTypedManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Custom
            var regCustomTransferTypedManagerResult = await regCustomTransferTypedManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            regCustomTransferTypedManagerResult.Should().NotBeNull();
            regCustomTransferTypedManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Upload
            // Built-in
            var regUploadManagerResult = await regUploadManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            regUploadManagerResult.Should().NotBeNull();
            regUploadManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Built-in
            var regUploadTypedManagerResult = await regUploadTypedManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            regUploadTypedManagerResult.Should().NotBeNull();
            regUploadTypedManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Custom
            var regCustomUploadTypedManagerResult = await regCustomUploadTypedManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            regCustomUploadTypedManagerResult.Should().NotBeNull();
            regCustomUploadTypedManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Uploading_File_With_Local_Progress_Should_Report_Progress()
        {
            var percentage = 0;
            var progress = new ApizrProgress();
            progress.ProgressChanged += (sender, args) =>
            {
                percentage = args.ProgressPercentage;
            };
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddTransferManager(options => options
                    .WithBaseAddress("https://httpbin.org/post")
                    .WithProgress()));

            apizrRegistry.TryGetTransferManager(out var regTransferManager).Should().BeTrue(); // Built-in
            regTransferManager.Should().NotBeNull(); // Built-in

            var apizrTransferManagerResult = await regTransferManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"), options => options.WithProgress(progress));

            apizrTransferManagerResult.Should().NotBeNull();
            apizrTransferManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);
            percentage.Should().Be(100);
        }

        [Fact]
        public async Task Uploading_File_With_Global_Progress_Should_Report_Progress()
        {
            var percentage = 0;
            var progress = new ApizrProgress();
            progress.ProgressChanged += (sender, args) =>
            {
                percentage = args.ProgressPercentage;
            };
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddTransferManager(options => options
                        .WithBaseAddress("https://httpbin.org/post")
                        .WithProgress(progress)));

            apizrRegistry.TryGetTransferManager(out var regTransferManager).Should().BeTrue(); // Built-in
            regTransferManager.Should().NotBeNull(); // Built-in

            var apizrTransferManagerResult = await regTransferManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));

            apizrTransferManagerResult.Should().NotBeNull();
            apizrTransferManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);
            percentage.Should().Be(100);
        }

        [Fact]
        public async Task Requesting_With_Headers_Should_Set_Headers()
        {
            var watcher = new WatchingRequestHandler();

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddTransferManagerFor<ITransferUndefinedApi>(options => options
                    .WithBaseAddress("https://httpbin.org/post")
                    .WithHeaders(() => new []{"testKey2: testValue2.2"})
                    .AddDelegatingHandler(watcher)),
                options => options
                    .WithHeaders("testKey2: testValue2.1", "testKey3: testValue3.1"));

            // Shortcut
            apizrRegistry.TryGetTransferManagerFor<ITransferUndefinedApi>(out var regCustomTransferTypedManager).Should().BeTrue(); // Custom
            await regCustomTransferTypedManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"), options => options.WithHeaders("testKey3: testValue3.2", "testKey4: testValue4"));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKeys("testKey2", "testKey3", "testKey4");
            watcher.Headers.GetValues("testKey2").Should().HaveCount(1).And.Contain("testValue2.2");
            watcher.Headers.GetValues("testKey3").Should().HaveCount(1).And.Contain("testValue3.2");
            watcher.Headers.GetValues("testKey4").Should().HaveCount(1).And.Contain("testValue4");
        }

        [Fact]
        public async Task Requesting_With_Both_Attribute_And_Fluent_Headers_Should_Set_Merged_Headers()
        {
            var watcher = new WatchingRequestHandler();

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddManagerFor<IReqResSimpleService>(options => options
                .WithHeaders("testKey2: testValue2")
                .AddDelegatingHandler(watcher)),
                options => options.WithHeaders("testKey3: testValue3"));

            apizrRegistry.TryGetManagerFor<IReqResSimpleService>(out var reqResManager).Should().BeTrue(); // Custom
            
            await reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt),
                options => options.WithHeaders("testKey4: testValue4"));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKeys("testKey1", "testKey2", "testKey3", "testKey4");
        }

        [Fact]
        public async Task Concurrent_Requests_Should_Not_Throw()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddManagerFor<IReqResSimpleService>());

            apizrRegistry.TryGetManagerFor<IReqResSimpleService>(out var reqResManager).Should().BeTrue();

            var tasks = new List<Task>();
            for (var i = 0; i < 10; ++i)
            {
                tasks.Add(Task.Run(async () =>
                {
                    Func<Task> act = () => reqResManager.ExecuteAsync(api => api.GetUsersAsync());
                    await act.Should().NotThrowAsync();
                }));
            }
            await Task.WhenAll(tasks);
        }

        [Fact]
        public async Task Cancelling_A_Get_Request_Should_Throw_An_OperationCanceledException()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddManagerFor<IReqResUserService>());

            apizrRegistry.TryGetManagerFor<IReqResUserService>(out var reqResManager).Should().BeTrue();

            var ct = new CancellationTokenSource();
            ct.CancelAfter(TimeSpan.FromSeconds(2));

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(5, opt),
                    options => options.WithCancellation(ct.Token));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<OperationCanceledException>();
        }

        [Fact]
        public async Task Cancelling_A_Post_Request_Should_Throw_An_OperationCanceledException()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddManagerFor<IHttpBinService>());

            apizrRegistry.TryGetManagerFor<IHttpBinService>(out var manager).Should().BeTrue();

            var streamPart = FileHelper.GetTestFileStreamPart("medium");
            var ct = new CancellationTokenSource();
            ct.CancelAfter(TimeSpan.FromSeconds(2));

            Func<Task> act = () => manager.ExecuteAsync((opt, api) => api.UploadAsync(streamPart, opt),
                options => options.WithCancellation(ct.Token));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<OperationCanceledException>();
        }
    }
}
