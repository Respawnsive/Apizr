using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Caching;
using Apizr.Configuring;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Registry;
using Apizr.Extending;
using Apizr.Logging;
using Apizr.Progressing;
using Apizr.Resiliencing;
using Apizr.Tests.Apis;
using Apizr.Tests.Helpers;
using Apizr.Tests.Models;
using Apizr.Tests.Models.Mappings;
using Apizr.Tests.Settings;
using Apizr.Transferring.Requesting;
using AutoMapper;
using FluentAssertions;
using Fusillade;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MonkeyCache.FileStore;
using Polly;
using Polly.Registry;
using Polly.Retry;
using Polly.Timeout;
using Refit;
using RichardSzalay.MockHttp;
using Xunit;
using Xunit.Abstractions;

namespace Apizr.Tests
{
    public class ApizrRegistryTests
    {
        private readonly RefitSettings _refitSettings;
        private readonly ITestOutputHelper _outputHelper;

        public ApizrRegistryTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
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
            resourceFixture.Options.BaseUri.Should().Be(attributeUri);

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
            resourceFixture.Options.BaseUri.Should().Be(attributeUri);
        }

        [Fact]
        public async Task Calling_AddHttpMessageHandler_Should_Add_The_Handler()
        {
            var mockHttp = new MockHttpMessageHandler();
            var watcher = new WatchingRequestHandler();

            var json =
                "{ \"page\": 1, \"per_page\": 6, \"total\": 12, \"total_pages\": 2, \"data\": [ { \"id\": 1, \"email\": \"george.bluth@reqres.in\", \"first_name\": \"George\", \"last_name\": \"Bluth\", \"avatar\": \"https://reqres.in/img/faces/1-image.jpg\" } ] }";

            // Setup a respond for the user api (including a wildcard in the URL)
            mockHttp.When("https://reqres.in/api/*")
                .Respond("application/json", json); // Respond with JSON

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(
                registry => registry.AddManagerFor<IReqResSimpleService>(),
                options => options.WithLoggerFactory(LoggerFactory.Create(
                        builder => builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithDelegatingHandler(watcher)
                    .WithHttpMessageHandler(mockHttp));

            var reqResManager = apizrRegistry.GetManagerFor<IReqResSimpleService>();

            var result = await reqResManager.ExecuteAsync(api => api.GetUsersAsync());

            result.Should().NotBeNull();
            result.Data.Should().NotBeNullOrEmpty();
            result.Data.First().FirstName.Should().Be("George");
            watcher.Attempts.Should().Be(1);
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_ProperOption_Should_Authenticate_Request()
        {
            string token = null; 
            Task<string> OnRefreshToken(HttpRequestMessage request, string tk, CancellationToken ct) => Task.FromResult(token = "token");

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(
                registry => registry
                    .AddManagerFor<IHttpBinService>(options =>
                        options.WithAuthenticationHandler(OnRefreshToken)),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging());

            var httpBinManager = apizrRegistry.GetManagerFor<IHttpBinService>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
            token.Should().Be("token");
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_ProperOption_With_Local_Token_Method_Should_Authenticate_Request()
        {
            var getCounter = 0;
            var setCounter = 0;
            var refreshCounter = 0;
            string token = null;
            Task<string> OnGetToken(HttpRequestMessage request, CancellationToken ct)
            {
                getCounter++;
                return Task.FromResult(token);
            }

            Task OnSetToken(HttpRequestMessage request, string tk, CancellationToken ct)
            {
                if (token != tk)
                    setCounter++;
                return Task.FromResult(token = tk);
            }

            Task<string> OnRefreshToken(HttpRequestMessage request, string tk, CancellationToken ct)
            {
                refreshCounter++;
                return Task.FromResult("token");
            }

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(
                registry => registry
                    .AddManagerFor<IHttpBinService>(options =>
                        options.WithAuthenticationHandler(OnGetToken, OnSetToken, OnRefreshToken)),
                options => options
                    .WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithHttpMessageHandler(new WatchingRequestHandler()));

            var httpBinManager = apizrRegistry.GetManagerFor<IHttpBinService>();

            var result1 = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result1.IsSuccessStatusCode.Should().BeTrue();
            token.Should().Be("token");
            getCounter.Should().Be(1);
            setCounter.Should().Be(1);
            refreshCounter.Should().Be(1);

            var result2 = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result2.IsSuccessStatusCode.Should().BeTrue();
            token.Should().Be("token");
            getCounter.Should().Be(2);
            setCounter.Should().Be(1);
            refreshCounter.Should().Be(1);
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_ProperOption_With_Local_Token_Property_Should_Authenticate_Request()
        {
            var testSettings = new TestSettings("token");

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(
                registry => registry
                    .AddManagerFor<IHttpBinService>(options =>
                        options.WithAuthenticationHandler(testSettings, settings => settings.TestJsonString)),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging());

            var httpBinManager = apizrRegistry.GetManagerFor<IHttpBinService>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_ProperOption_With_Local_Token_Service_Should_Authenticate_Request()
        {
            var tokenService = new TokenService();

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(
                registry => registry
                    .AddManagerFor<IHttpBinService>(options =>
                        options.WithAuthenticationHandler(
                            tokenService,
                            (ts, request, ct) => ts.GetTokenAsync(request, ct),
                            (ts, request, tk, ct) => ts.SetTokenAsync(request, tk, ct),
                            tokenService,
                            (ts, request, tk, ct) => ts.RefreshTokenAsync(request, tk, ct))),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging());

            var httpBinManager = apizrRegistry.GetManagerFor<IHttpBinService>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_CommonOption_With_Local_Token_Handler_Should_Authenticate_Request()
        {
            var tokenService = new TokenService();

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(
                registry => registry
                    .AddManagerFor<IHttpBinService>(),
                options =>options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithAuthenticationHandler((log, opt) => new AuthHandler<IHttpBinService>(log, opt, tokenService)));

            var httpBinManager = apizrRegistry.GetManagerFor<IHttpBinService>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public void Calling_WithLogging_Should_Set_LoggingSettings()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(
                registry => registry
                    .AddManagerFor<IReqResUserService>(),
                options => options
                    .WithLogging(HttpTracerMode.ExceptionsOnly, HttpMessageParts.RequestCookies, LogLevel.Warning));

            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

            reqResManager.Options.HttpTracerMode.Should().Be(HttpTracerMode.ExceptionsOnly);
            reqResManager.Options.TrafficVerbosity.Should().Be(HttpMessageParts.RequestCookies);
            reqResManager.Options.LogLevels.Should().AllBeEquivalentTo(LogLevel.Warning);
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_CommonOption_Should_Authenticate_Request()
        {
            string token = null;
            Task<string> OnRefreshToken(HttpRequestMessage request, string tk, CancellationToken ct) => Task.FromResult(token = "token");

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IHttpBinService>(),
                config => config.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging().WithAuthenticationHandler(OnRefreshToken));

            var httpBinManager = apizrRegistry.GetManagerFor<IHttpBinService>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
            token.Should().Be("token");
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_Should_Authenticate_Request()
        {
            string token = null;

            Task<string> OnRefreshTokenA(HttpRequestMessage request, string tk, CancellationToken ct) => Task.FromResult(token = "tokenA");
            Task<string> OnRefreshTokenB(HttpRequestMessage request, string tk, CancellationToken ct) => Task.FromResult(token = "tokenB");

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IHttpBinService>(options => options.WithAuthenticationHandler(OnRefreshTokenA)),
                config => config.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging().WithAuthenticationHandler(OnRefreshTokenB));

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
                config => config.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithAkavacheCacheHandler()
                    .WithDelegatingHandler(new TestRequestHandler()));

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
        public async Task Calling_WithAkavacheCacheHandler_Should_Cache_ApizrResponse()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(),
                config => config.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithAkavacheCacheHandler()
                    .WithDelegatingHandler(new TestRequestHandler()));

            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

            // Clearing all cache
            var cleared = await reqResManager.ClearCacheAsync();

            cleared.Should().BeTrue();

            // This one should fail with no cached result
            var response = await reqResManager.ExecuteAsync(api => api.SafeGetUsersAsync(HttpStatusCode.BadRequest));

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeFalse();
            response.ApiResponse.Should().NotBeNull();
            response.ApiResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Result.Should().BeNull();
            response.DataSource.Should().Be(ApizrResponseDataSource.None);

            // This one should succeed with request result
            response = await reqResManager.ExecuteAsync(api => api.SafeGetUsersAsync());

            // and cache result in-memory
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.ApiResponse.Should().NotBeNull();
            response.ApiResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Should().NotBeNull();
            response.Result!.Data.Should().NotBeNullOrEmpty();
            response.DataSource.Should().Be(ApizrResponseDataSource.Request);

            // This one should fail but with cached result
            response = await reqResManager.ExecuteAsync(api => api.SafeGetUsersAsync(HttpStatusCode.BadRequest));

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeFalse();
            response.ApiResponse.Should().NotBeNull();
            response.ApiResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Result.Should().NotBeNull();
            response.Result!.Data.Should().NotBeNullOrEmpty();
            response.DataSource.Should().Be(ApizrResponseDataSource.Cache);

            // Clearing specific cache
            cleared = await reqResManager.ClearCacheAsync(api => api.SafeGetUsersAsync());

            cleared.Should().BeTrue();

            // This one should fail with no cached result
            response = await reqResManager.ExecuteAsync(api => api.SafeGetUsersAsync(HttpStatusCode.BadRequest));

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeFalse();
            response.ApiResponse.Should().NotBeNull();
            response.ApiResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Result.Should().BeNull();
            response.DataSource.Should().Be(ApizrResponseDataSource.None);
        }

        [Fact]
        public async Task Calling_WithAkavacheCacheHandler_With_Multiple_CacheKey_Should_Cache_Result()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(),
                config => config.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithAkavacheCacheHandler()
                    .WithDelegatingHandler(new TestRequestHandler()));

            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

            // Clearing cache
            await reqResManager.ClearCacheAsync();

            var testDictionary = new Dictionary<string, object>
            {
                { "test1", "test1" },
                { "test2", 2 }
            };

            var customTypeParam = new ReadAllUsersParams("test1", 2);

            // Defining a throwing request
            Func<Task> act1 = () => reqResManager.ExecuteAsync(api => api.GetUserAsync(1, testDictionary, customTypeParam, HttpStatusCode.BadRequest));

            // Calling it at first execution should throw as expected without any cached result
            var ex = await act1.Should().ThrowAsync<ApizrException<UserDetails>>();
            ex.And.CachedResult.Should().BeNull();

            // This one should succeed
            var result = await reqResManager.ExecuteAsync(api => api.GetUserAsync(1, testDictionary, customTypeParam, HttpStatusCode.OK));

            // and cache result in-memory
            result.Should().NotBeNull();

            // This one should fail but with cached result
            var ex2 = await act1.Should().ThrowAsync<ApizrException<UserDetails>>();
            ex2.And.CachedResult.Should().NotBeNull();

            // Defining another throwing request
            Func<Task> act2 = () => reqResManager.ExecuteAsync(api => api.GetUserAsync(2, testDictionary, customTypeParam, HttpStatusCode.BadRequest));

            // Calling it again with another cache key value should throw as expected but without any cached result
            var ex3 = await act2.Should().ThrowAsync<ApizrException<UserDetails>>();
            ex3.And.CachedResult.Should().BeNull();
        }

        [Fact]
        public async Task Calling_WithAkavacheCacheHandler_With_Post_CacheKey_And_FetchOrGet_Should_Cache_Result()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithAkavacheCacheHandler()
                    .WithDelegatingHandler(new TestRequestHandler()));

            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

            // Clearing cache
            await reqResManager.ClearCacheAsync();

            var user = new CreateUser
            {
                FirstName = "John",
                LastName = "Doe",
                Avatar = "https://reqres.in/img/faces/1-image.jpg",
                Email = "jd1@email.com" // #1
            };

            // Defining a throwing request
            Func<Task<CreateUser>> act1 = () => reqResManager.ExecuteAsync(api => api.CreateCachedUser(user, "users", HttpStatusCode.BadRequest));

            // Calling it at first execution should throw as expected without any cached result
            var ex = await act1.Should().ThrowAsync<ApizrException<CreateUser>>();
            ex.And.CachedResult.Should().BeNull();

            // This one should succeed
            var result = await reqResManager.ExecuteAsync(api => api.CreateCachedUser(user, "users", HttpStatusCode.OK));

            // and cache result in-memory
            result.Should().NotBeNull();

            // This one should fail but with cached result
            var ex2 = await act1.Should().ThrowAsync<ApizrException<CreateUser>>();
            ex2.And.CachedResult.Should().NotBeNull();

            // Changing cache key value
            user = new CreateUser
            {
                FirstName = "John",
                LastName = "Doe",
                Avatar = "https://reqres.in/img/faces/1-image.jpg",
                Email = "jd2@email.com" // #2
            };

            // Defining another throwing request
            Func<Task<CreateUser>> act2 = () => reqResManager.ExecuteAsync(api => api.CreateCachedUser(user, "users", HttpStatusCode.BadRequest));

            // Calling it again with another cache key value should throw as expected but without any cached result
            var ex3 = await act2.Should().ThrowAsync<ApizrException<CreateUser>>();
            ex3.And.CachedResult.Should().BeNull();
        }

        [Fact]
        public async Task Calling_WithAkavacheCacheHandler_With_Post_CacheKey_And_GetOrFetch_Should_Cache_Result()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(
                        options => options.WithCaching(CacheMode.GetOrFetch)),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithAkavacheCacheHandler()
                    .WithDelegatingHandler(new TestRequestHandler()));

            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

            // Clearing cache
            await reqResManager.ClearCacheAsync();

            var user = new CreateUser
            {
                FirstName = "John",
                LastName = "Doe",
                Avatar = "https://reqres.in/img/faces/1-image.jpg",
                Email = "jd1@email.com" // #1
            };

            // Defining a throwing request
            Func<Task<CreateUser>> act1 = () => reqResManager.ExecuteAsync(api => api.CreateCachedUser(user, "users", HttpStatusCode.BadRequest));

            // Calling it at first execution should throw as expected without any cached result
            var ex = await act1.Should().ThrowAsync<ApizrException<CreateUser>>();
            ex.And.CachedResult.Should().BeNull();

            // This one should succeed
            var result = await reqResManager.ExecuteAsync(api => api.CreateCachedUser(user, "users", HttpStatusCode.OK));

            // and cache result in-memory
            result.Should().NotBeNull();

            // This one should not fail but return cached result
            var actResult = await act1.Should().NotThrowAsync<ApizrException<CreateUser>>();
            actResult.And.Subject.Should().NotBeNull();

            // Changing cache key value
            user = new CreateUser
            {
                FirstName = "John",
                LastName = "Doe",
                Avatar = "https://reqres.in/img/faces/1-image.jpg",
                Email = "jd2@email.com" // #2
            };

            // Defining another throwing request
            Func<Task<CreateUser>> act2 = () => reqResManager.ExecuteAsync(api => api.CreateCachedUser(user, "users", HttpStatusCode.BadRequest));

            // Calling it again with another cache key value should throw as expected but without any cached result
            var ex3 = await act2.Should().ThrowAsync<ApizrException<CreateUser>>();
            ex3.And.CachedResult.Should().BeNull();
        }

        [Fact]
        public async Task Calling_WithAkavacheCacheHandler_And_WithCacheControlHeader_Should_Cache_ApizrResponse()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddManagerFor<IApizrTestsApi>(),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithAkavacheCacheHandler()
                    .WithCaching(CacheMode.SetByHeader));

            var reqResManager = apizrRegistry.GetManagerFor<IApizrTestsApi>();

            // Clearing all cache
            var cleared = await reqResManager.ClearCacheAsync();

            cleared.Should().BeTrue();

            // This one should succeed with request result
            var response = await reqResManager.ExecuteAsync((opt, api) => api.GetWeatherForecastAsync("cache-control", opt));

            // and cache result in-memory
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.ApiResponse.Should().NotBeNull();
            response.ApiResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Should().NotBeNullOrEmpty();
            response.DataSource.Should().Be(ApizrResponseDataSource.Request);

            await Task.Delay(3000);

            // This one should succeed with cached result
            response = await reqResManager.ExecuteAsync((opt, api) => api.GetWeatherForecastAsync("cache-control", opt));

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.ApiResponse.Should().BeNull();
            response.Result.Should().NotBeNullOrEmpty();
            response.DataSource.Should().Be(ApizrResponseDataSource.Cache);

            await Task.Delay(3000);

            // This one should succeed with request result
            response = await reqResManager.ExecuteAsync((opt, api) => api.GetWeatherForecastAsync("cache-control", opt));

            // and cache result in-memory
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.ApiResponse.Should().NotBeNull();
            response.ApiResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Should().NotBeNullOrEmpty();
            response.DataSource.Should().Be(ApizrResponseDataSource.Request);
        }

        [Fact]
        public async Task Calling_WithAkavacheCacheHandler_And_WithImmutableCacheControlHeader_Should_Cache_ApizrResponse()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddManagerFor<IApizrTestsApi>(),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithAkavacheCacheHandler()
                    .WithCaching(CacheMode.SetByHeader));

            var reqResManager = apizrRegistry.GetManagerFor<IApizrTestsApi>();

            // Clearing all cache
            var cleared = await reqResManager.ClearCacheAsync();

            cleared.Should().BeTrue();

            // This one should succeed with request result
            var response = await reqResManager.ExecuteAsync((opt, api) => api.GetWeatherForecastAsync("immutable-cache-control", opt));

            // and cache result in-memory
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.ApiResponse.Should().NotBeNull();
            response.ApiResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Should().NotBeNullOrEmpty();
            response.DataSource.Should().Be(ApizrResponseDataSource.Request);

            await Task.Delay(3000);

            // This one should succeed with cached result
            response = await reqResManager.ExecuteAsync((opt, api) => api.GetWeatherForecastAsync("immutable-cache-control", opt));

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.ApiResponse.Should().BeNull();
            response.Result.Should().NotBeNullOrEmpty();
            response.DataSource.Should().Be(ApizrResponseDataSource.Cache);

            await Task.Delay(3000);

            // This one should succeed with request result
            response = await reqResManager.ExecuteAsync((opt, api) => api.GetWeatherForecastAsync("immutable-cache-control", opt));

            // and cache result in-memory
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.ApiResponse.Should().BeNull();
            response.Result.Should().NotBeNullOrEmpty();
            response.DataSource.Should().Be(ApizrResponseDataSource.Cache);
        }

        [Fact]
        public async Task Calling_WithAkavacheCacheHandler_And_WithExpiresHeader_Should_Cache_ApizrResponse()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddManagerFor<IApizrTestsApi>(),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithAkavacheCacheHandler()
                    .WithCaching(CacheMode.SetByHeader));

            var reqResManager = apizrRegistry.GetManagerFor<IApizrTestsApi>();

            // Clearing all cache
            var cleared = await reqResManager.ClearCacheAsync();

            cleared.Should().BeTrue();

            // This one should succeed with request result
            var response = await reqResManager.ExecuteAsync((opt, api) => api.GetWeatherForecastAsync("expires", opt));

            // and cache result in-memory
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.ApiResponse.Should().NotBeNull();
            response.ApiResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Should().NotBeNullOrEmpty();
            response.DataSource.Should().Be(ApizrResponseDataSource.Request);

            await Task.Delay(3000);

            // This one should succeed with cached result
            response = await reqResManager.ExecuteAsync((opt, api) => api.GetWeatherForecastAsync("expires", opt));

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.ApiResponse.Should().BeNull();
            response.Result.Should().NotBeNullOrEmpty();
            response.DataSource.Should().Be(ApizrResponseDataSource.Cache);

            await Task.Delay(3000);

            // This one should succeed with request result
            response = await reqResManager.ExecuteAsync((opt, api) => api.GetWeatherForecastAsync("expires", opt));

            // and cache result in-memory
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.ApiResponse.Should().NotBeNull();
            response.ApiResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Should().NotBeNullOrEmpty();
            response.DataSource.Should().Be(ApizrResponseDataSource.Request);
        }

        [Fact]
        public async Task Calling_WithAkavacheCacheHandler_And_WithETagHeader_Should_Cache_ApizrResponse()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddManagerFor<IApizrTestsApi>(),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithAkavacheCacheHandler()
                    .WithCaching(CacheMode.SetByHeader));

            var reqResManager = apizrRegistry.GetManagerFor<IApizrTestsApi>();

            // Clearing all cache
            var cleared = await reqResManager.ClearCacheAsync();

            cleared.Should().BeTrue();

            // This one should succeed with request result
            var response = await reqResManager.ExecuteAsync((opt, api) => api.GetWeatherForecastAsync("etag", opt));

            // and cache result in-memory
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.ApiResponse.Should().NotBeNull();
            response.ApiResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Should().NotBeNullOrEmpty();
            response.DataSource.Should().Be(ApizrResponseDataSource.Request);

            await Task.Delay(3000);

            // This one should succeed with cached result
            response = await reqResManager.ExecuteAsync((opt, api) => api.GetWeatherForecastAsync("etag", opt));

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeFalse();
            response.ApiResponse.Should().NotBeNull();
            response.ApiResponse.StatusCode.Should().Be(HttpStatusCode.NotModified);
            response.Result.Should().NotBeNullOrEmpty();
            response.DataSource.Should().Be(ApizrResponseDataSource.Cache);

            await Task.Delay(3000);

            // This one should succeed with request result
            response = await reqResManager.ExecuteAsync((opt, api) => api.GetWeatherForecastAsync("etag", opt));

            // and cache result in-memory
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.ApiResponse.Should().NotBeNull();
            response.ApiResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Should().NotBeNullOrEmpty();
            response.DataSource.Should().Be(ApizrResponseDataSource.Request);
        }

        [Fact]
        public async Task Calling_WithAkavacheCacheHandler_And_WithLastModifiedHeader_Should_Cache_ApizrResponse()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddManagerFor<IApizrTestsApi>(),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithAkavacheCacheHandler()
                    .WithCaching(CacheMode.SetByHeader));

            var reqResManager = apizrRegistry.GetManagerFor<IApizrTestsApi>();

            // Clearing all cache
            var cleared = await reqResManager.ClearCacheAsync();

            cleared.Should().BeTrue();

            // This one should succeed with request result
            var response = await reqResManager.ExecuteAsync((opt, api) => api.GetWeatherForecastAsync("last-modified", opt));

            // and cache result in-memory
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.ApiResponse.Should().NotBeNull();
            response.ApiResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Should().NotBeNullOrEmpty();
            response.DataSource.Should().Be(ApizrResponseDataSource.Request);

            await Task.Delay(3000);

            // This one should succeed with cached result
            response = await reqResManager.ExecuteAsync((opt, api) => api.GetWeatherForecastAsync("last-modified", opt));

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeFalse();
            response.ApiResponse.Should().NotBeNull();
            response.ApiResponse.StatusCode.Should().Be(HttpStatusCode.NotModified);
            response.Result.Should().NotBeNullOrEmpty();
            response.DataSource.Should().Be(ApizrResponseDataSource.Cache);

            await Task.Delay(3000);

            // This one should succeed with request result
            response = await reqResManager.ExecuteAsync((opt, api) => api.GetWeatherForecastAsync("last-modified", opt));

            // and cache result in-memory
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.ApiResponse.Should().NotBeNull();
            response.ApiResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Should().NotBeNullOrEmpty();
            response.DataSource.Should().Be(ApizrResponseDataSource.Request);
        }

        [Fact]
        public async Task RequestTimeout_Should_Be_Handled_By_Polly()
        {
            var maxRetryAttempts = 3;
            var retryCount = 0;
            var resiliencePipelineRegistry = new ResiliencePipelineRegistry<string>();
            resiliencePipelineRegistry.TryAddBuilder<HttpResponseMessage>("TransientHttpError", (builder, _) =>
                builder.ConfigureTelemetry(LoggerFactory.Create(loggingBuilder =>
                        loggingBuilder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                .AddRetry(
                    new RetryStrategyOptions<HttpResponseMessage>
                    {
                        ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                            .Handle<HttpRequestException>()
                            .HandleResult(response =>
                                response.StatusCode is >= HttpStatusCode.InternalServerError
                                    or HttpStatusCode.RequestTimeout),
                        Delay = TimeSpan.FromSeconds(1),
                        MaxRetryAttempts = maxRetryAttempts,
                        UseJitter = true,
                        BackoffType = DelayBackoffType.Exponential,
                        OnRetry = args =>
                        {
                            retryCount = args.AttemptNumber+1;
                            return default;
                        }
                    }));

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(),
                config => config.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithResiliencePipelineRegistry(resiliencePipelineRegistry)
                    .WithDelegatingHandler(new TestRequestHandler()));

            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

            // Defining a transient throwing request
            Func<Task> act = () => reqResManager.ExecuteAsync(api => api.GetUsersAsync(HttpStatusCode.RequestTimeout));

            // Calling it should throw but handled by Polly
            await act.Should().ThrowAsync<ApizrException>();

            // attempts should be equal to total retry count
            retryCount.Should().Be(maxRetryAttempts);
        }

        [Fact]
        public async Task Calling_WithConnectivityHandler_Should_Check_Connectivity()
        {
            var isConnected = false;

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(),
                config => config.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
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
                config => config.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
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
                config => config.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
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
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            typeAdapterConfig.RuleMap.Clear();
            typeAdapterConfig.ForType<User, MinUser>()
                .TwoWays()
                .Map(minUser => minUser.Name, user => user.FirstName);

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(),
                config => config.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
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
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            typeAdapterConfig.RuleMap.Clear();
            typeAdapterConfig.ForType<User, MinUser>()
                .TwoWays()
                .Map(minUser => minUser.Name, user => user.FirstName);

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(),
                config => config.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
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
        public async Task Requesting_With_A_ResilienceProperty_into_Options_Should_Set_It_Into_Context()
        {
            var watcher = new WatchingRequestHandler();

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(
                registry => registry.AddManagerFor<IReqResUserService>(options =>
                    options.WithDelegatingHandler(watcher)),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithResilienceContextOptions(opt =>
                        opt.ReturnToPoolOnComplete(false)));
            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

            ResiliencePropertyKey<int> testKey = new("TestKey1");
            var testValue = 1;

            await reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt), options => options.WithResilienceProperty(testKey, testValue));
            watcher.Context.Should().NotBeNull();
            watcher.Context.Properties.TryGetValue(testKey, out var value).Should().BeTrue();
            value.Should().Be(testValue);
        }

        [Fact]
        public async Task Requesting_With_Context_At_Multiple_Levels_Should_Merge_It_All_At_The_End()
        {
            var watcher = new WatchingRequestHandler();

            ResiliencePropertyKey<string> testKey1 = new(nameof(testKey1));
            ResiliencePropertyKey<string> testKey2 = new(nameof(testKey2));
            ResiliencePropertyKey<string> testKey3 = new(nameof(testKey3));
            ResiliencePropertyKey<string> testKey4 = new(nameof(testKey4));
            ResiliencePropertyKey<string> testKey5 = new(nameof(testKey5));
            ResiliencePropertyKey<string> testKey6 = new(nameof(testKey6));
            ResiliencePropertyKey<string> testKey7 = new(nameof(testKey7));
            ResiliencePropertyKey<string> testKey8 = new(nameof(testKey8));
            ResiliencePropertyKey<string> testKey9 = new(nameof(testKey9));

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry =>
                    registry.AddGroup(group => group.AddManagerFor<IReqResSimpleService>(
                            // proper
                            options => options.WithResilienceProperty(testKey3, () => "testValue3.2")
                                .WithResilienceProperty(testKey7, () => "testValue7.1")
                                .WithDelegatingHandler(watcher)
                                .WithRequestOptions(nameof(IReqResUserService.GetUsersAsync), requestOptions =>
                                    requestOptions.WithResilienceProperty(testKey4, "testValue4.2")
                                        .WithResilienceProperty(testKey8, "testValue8.1"))),
                        // group
                        options => options.WithResilienceProperty(testKey2, () => "testValue2.2")
                            .WithResilienceProperty(testKey6, () => "testValue6.1")),
                // common
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithResilienceContextOptions(opt =>
                        opt.ReturnToPoolOnComplete(false))
                    .WithResilienceProperty(testKey1, () => "testValue1.1")
                    .WithResilienceProperty(testKey2, () => "testValue2.1")
                    .WithResilienceProperty(testKey3, () => "testValue3.1")
                    .WithResilienceProperty(testKey4, () => "testValue4.1")
                    .WithResilienceProperty(testKey5, () => "testValue5.1"));

            var reqResManager = apizrRegistry.GetManagerFor<IReqResSimpleService>();

            await reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt),
                // request
                options => options.WithResilienceProperty(testKey5, "testValue5.2")
                    .WithResilienceProperty(testKey9, "testValue9.1"));

            watcher.Context.Should().NotBeNull();
            watcher.Context.Properties.TryGetValue(testKey1, out var valueKey1).Should().BeTrue(); // Set by common option
            valueKey1.Should().Be("testValue1.1");
            watcher.Context.Properties.TryGetValue(testKey2, out var valueKey2).Should().BeTrue(); // Set by common option then updated by the group one
            valueKey2.Should().Be("testValue2.2");
            watcher.Context.Properties.TryGetValue(testKey3, out var valueKey3).Should().BeTrue(); // Set by common option then updated by the proper one
            valueKey3.Should().Be("testValue3.2");
            watcher.Context.Properties.TryGetValue(testKey4, out var valueKey4).Should().BeTrue(); // Set by common option then updated by the proper's request one
            valueKey4.Should().Be("testValue4.2");
            watcher.Context.Properties.TryGetValue(testKey5, out var valueKey5).Should().BeTrue(); // Set by common option then updated by the request one
            valueKey5.Should().Be("testValue5.2");
            watcher.Context.Properties.TryGetValue(testKey6, out var valueKey6).Should().BeTrue(); // Set by group option
            valueKey6.Should().Be("testValue6.1");
            watcher.Context.Properties.TryGetValue(testKey7, out var valueKey7).Should().BeTrue(); // Set by proper option
            valueKey7.Should().Be("testValue7.1");
            watcher.Context.Properties.TryGetValue(testKey8, out var valueKey8).Should().BeTrue(); // Set by proper's request option
            valueKey8.Should().Be("testValue8.1");
            watcher.Context.Properties.TryGetValue(testKey9, out var valueKey9).Should().BeTrue(); // Set by request option
            valueKey9.Should().Be("testValue9.1");
        }

        [Fact]
        public async Task Requesting_With_LogSettings_Into_Options_Should_Win_Over_All_Others()
        {
            var watcher = new WatchingRequestHandler();

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(
                registry => registry.AddManagerFor<IReqResUserService>(options =>
                    options.WithDelegatingHandler(watcher)),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithResilienceContextOptions(opt =>
                        opt.ReturnToPoolOnComplete(false)));
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
        public async Task Configuring_Obsolete_Exceptions_Handler_Should_Handle_Exceptions()
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
                                .WithDelegatingHandler(new TestRequestHandler()))
                            .AddManagerFor<IReqResResourceService>(),
                        options => options.WithExCatching(OnException, strategy: ApizrDuplicateStrategy.Add))
                    .AddManagerFor<IHttpBinService>()
                    .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithExCatching(OnException, strategy: ApizrDuplicateStrategy.Add));


            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

            // Defining a transient throwing request
            Func<Task> act = () => reqResManager.ExecuteAsync(api => api.GetUsersAsync(HttpStatusCode.RequestTimeout),
                options => options.WithExCatching(OnException, strategy: ApizrDuplicateStrategy.Add));

            // Calling it should throw but handled by Polly
            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.And.Handled.Should().BeTrue();

            handledException.Should().Be(4);

            // Try to replace queued ex handlers by the last one set at request time
            handledException = 0;

            // Defining a transient throwing request
            act = () => reqResManager.ExecuteAsync(api => api.GetUsersAsync(HttpStatusCode.RequestTimeout),
                options => options.WithExCatching(OnException, strategy: ApizrDuplicateStrategy.Replace));

            // Calling it should throw but handled by Polly
            ex = await act.Should().ThrowAsync<ApizrException>();
            ex.And.Handled.Should().BeTrue();

            handledException.Should().Be(1);
        }

        [Fact]
        public async Task Configuring_Exceptions_Handler_With_Handle_Flag_Should_Handle_Exceptions()
        {
            var handledException = 0;
            bool OnException(ApizrException ex)
            {
                handledException++;
                return handledException == 3;
            }

            // Try to queue ex handlers
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddGroup(group => group
                            .AddManagerFor<IReqResUserService>(options => options
                                .WithExCatching(OnException, strategy: ApizrDuplicateStrategy.Add)
                                .WithDelegatingHandler(new TestRequestHandler()))
                            .AddManagerFor<IReqResResourceService>(),
                        options => options.WithExCatching(OnException, strategy: ApizrDuplicateStrategy.Add))
                    .AddManagerFor<IHttpBinService>()
                    .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithExCatching(OnException, strategy: ApizrDuplicateStrategy.Add));


            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

            // Defining a transient throwing request
            Func<Task> act = () => reqResManager.ExecuteAsync(api => api.GetUsersAsync(HttpStatusCode.RequestTimeout),
                options => options.WithExCatching(OnException, strategy: ApizrDuplicateStrategy.Add));

            // Calling it should throw
            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.And.Handled.Should().BeTrue();

            handledException.Should().Be(4);

            // Try to replace queued ex handlers by the last one set at request time
            handledException = 0;

            // Defining a transient throwing request
            act = () => reqResManager.ExecuteAsync(api => api.GetUsersAsync(HttpStatusCode.RequestTimeout),
                options => options.WithExCatching(OnException, strategy: ApizrDuplicateStrategy.Replace));

            // Calling it should throw but handled by Polly
            ex = await act.Should().ThrowAsync<ApizrException>();
            ex.And.Handled.Should().BeFalse();

            handledException.Should().Be(1);
        }

        [Fact]
        public async Task Requesting_With_CancellationToken_Into_Options_Should_Cancel_Request_When_Asked()
        {
            var watcher = new WatchingRequestHandler();

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(
                registry => registry.AddManagerFor<IReqResUserService>(options =>
                    options.WithDelegatingHandler(watcher)),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithResilienceContextOptions(opt =>
                        opt.ReturnToPoolOnComplete(false)));
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
            var watcher1 = new WatchingRequestHandler();
            var watcher2 = new WatchingRequestHandler();

            // Try to configure priority
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddGroup(group => group
                            .AddManagerFor<IReqResUserService>(options => options
                                .WithHttpMessageHandler(watcher1)
                                .WithPriority(Priority.UserInitiated))
                            .AddManagerFor<IReqResResourceService>(),
                        options => options.WithPriority(Priority.Background))
                    .AddManagerFor<IHttpBinService>()
                    .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(options => options
                        .WithBaseAddress("https://reqres.in/api/users")
                        .WithHttpMessageHandler(watcher2)
                        .WithPriority(Priority.Speculative)),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithResilienceContextOptions(opt =>
                        opt.ReturnToPoolOnComplete(false))
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
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithBaseAddress("https://proof.ovh.net/files"));

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
            var regShortcutResult = await apizrRegistry.DownloadAsync(new FileInfo("1Mb.dat"));
            regShortcutResult.Should().NotBeNull();
            regShortcutResult.Length.Should().BePositive();

            // Transfer
            // Built-in
            var regTransferManagerResult = await regTransferManager.DownloadAsync(new FileInfo("1Mb.dat"));
            regTransferManagerResult.Should().NotBeNull();
            regTransferManagerResult.Length.Should().BePositive();

            // Built-in
            var regTransferTypedManagerResult = await regTransferTypedManager.DownloadAsync(new FileInfo("1Mb.dat"));
            regTransferTypedManagerResult.Should().NotBeNull();
            regTransferTypedManagerResult.Length.Should().BePositive();

            // Custom
            var regCustomTransferTypedManagerResult = await regCustomTransferTypedManager.DownloadAsync(new FileInfo("1Mb.dat"));
            regCustomTransferTypedManagerResult.Should().NotBeNull();
            regCustomTransferTypedManagerResult.Length.Should().BePositive();

            // Download
            // Built-in
            var regDownloadManagerResult = await regDownloadManager.DownloadAsync(new FileInfo("1Mb.dat"));
            regDownloadManagerResult.Should().NotBeNull();
            regDownloadManagerResult.Length.Should().BePositive();

            // Built-in
            var regDownloadTypedManagerResult = await regDownloadTypedManager.DownloadAsync(new FileInfo("1Mb.dat"));
            regDownloadTypedManagerResult.Should().NotBeNull();
            regDownloadTypedManagerResult.Length.Should().BePositive();

            // Custom
            var regCustomDownloadTypedManagerResult = await regCustomDownloadTypedManager.DownloadAsync(new FileInfo("1Mb.dat"));
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
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithBaseAddress("https://proof.ovh.net"));

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
            var regShortcutResult = await apizrRegistry.DownloadAsync(new FileInfo("1Mb.dat"));
            regShortcutResult.Should().NotBeNull();
            regShortcutResult.Length.Should().BePositive();

            // Transfer
            // Built-in
            var regTransferManagerResult = await regTransferManager.DownloadAsync(new FileInfo("1Mb.dat"));
            regTransferManagerResult.Should().NotBeNull();
            regTransferManagerResult.Length.Should().BePositive();

            // Built-in
            var regTransferTypedManagerResult = await regTransferTypedManager.DownloadAsync(new FileInfo("1Mb.dat"));
            regTransferTypedManagerResult.Should().NotBeNull();
            regTransferTypedManagerResult.Length.Should().BePositive();

            // Custom
            var regCustomTransferTypedManagerResult = await regCustomTransferTypedManager.DownloadAsync(new FileInfo("1Mb.dat"));
            regCustomTransferTypedManagerResult.Should().NotBeNull();
            regCustomTransferTypedManagerResult.Length.Should().BePositive();

            // Download
            // Built-in
            var regDownloadManagerResult = await regDownloadManager.DownloadAsync(new FileInfo("1Mb.dat"));
            regDownloadManagerResult.Should().NotBeNull();
            regDownloadManagerResult.Length.Should().BePositive();

            // Built-in
            var regDownloadTypedManagerResult = await regDownloadTypedManager.DownloadAsync(new FileInfo("1Mb.dat"));
            regDownloadTypedManagerResult.Should().NotBeNull();
            regDownloadTypedManagerResult.Length.Should().BePositive();

            // Custom
            var regCustomDownloadTypedManagerResult = await regCustomDownloadTypedManager.DownloadAsync(new FileInfo("1Mb.dat"));
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
                        .WithBaseAddress("https://proof.ovh.net/files")
                        .WithProgress()),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging());

            apizrRegistry.TryGetTransferManager(out var regTransferManager).Should().BeTrue(); // Built-in
            regTransferManager.Should().NotBeNull(); // Built-in

            var fileInfo = await regTransferManager.DownloadAsync(new FileInfo("10Mb.dat"), options => options.WithProgress(progress)).ConfigureAwait(false);

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
                    .WithBaseAddress("https://proof.ovh.net/files")
                    .WithProgress(progress)),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging());

            apizrRegistry.TryGetTransferManager(out var regTransferManager).Should().BeTrue(); // Built-in
            regTransferManager.Should().NotBeNull(); // Built-in

            var fileInfo = await regTransferManager.DownloadAsync(new FileInfo("10Mb.dat")).ConfigureAwait(false);

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
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
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
            var regShortcutResult = await apizrRegistry.UploadAsync<IUploadApi>(FileHelper.GetTestFileStreamPart("small"));
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
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
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
                    .WithProgress()),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging());

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
                        .WithProgress(progress)),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging());

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
                .AddManagerFor<IReqResSimpleService>(options => options
                    .WithBaseAddress("https://reqres.in/api")
                    .WithHeaders(["testKey3: testValue3.2", "testKey4: testValue4.1"])
                    .WithLoggedHeadersRedactionNames(["testKey2"])
                    .WithDelegatingHandler(watcher)),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithResilienceContextOptions(opt =>
                        opt.ReturnToPoolOnComplete(false))
                    .WithHeaders(["testKey2: testValue2.2", "testKey3: testValue3.1"]));

            // Shortcut
            apizrRegistry.TryGetManagerFor<IReqResSimpleService>(out var apizrTransferManager).Should().BeTrue();

            // Shortcut
            await apizrTransferManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt), 
                options => options.WithHeaders(["testKey4: testValue4.2", "testKey5: testValue5"])
                    .WithLoggedHeadersRedactionRule(header => header == "testKey3"));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKeys("testKey1", "testKey2", "testKey3", "testKey4", "testKey5");
            watcher.Headers.GetValues("testKey1").Should().HaveCount(1).And.Contain("testValue1"); // Set by attribute
            watcher.Headers.GetValues("testKey2").Should().HaveCount(1).And.Contain("testValue2.2"); // Set by attribute then updated by common option
            watcher.Headers.GetValues("testKey3").Should().HaveCount(1).And.Contain("testValue3.2"); // Set by common option then updated by proper option
            watcher.Headers.GetValues("testKey4").Should().HaveCount(1).And.Contain("testValue4.2"); // Set by proper option then updated by request option
            watcher.Headers.GetValues("testKey5").Should().HaveCount(1).And.Contain("testValue5"); // Set by request option
        }

        [Fact]
        public async Task Requesting_With_Headers_Factory_Should_Set_And_Keep_Updated_Headers()
        {
            var watcher = new WatchingRequestHandler();
            var watcher2 = new WatchingRequestHandler();
            var apiHeaders = new List<string> { "testKey2: testValue2.2", "testKey3: testValue3.1" };
            var requestHeaders = new List<string> { "testKey3: testValue3.2", "testKey4: testValue4.1", "testKey5: testValue5.1" };
            var testSettings = new TestSettings("testSettingsKey1: testSettingsValue1.1");
            var testStore = new TestSettings("testStoreKey2: testStoreValue2.1");

            // Json settings (loaded from embedded json settings files and bound to properties)
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{typeof(ApizrTests).Namespace}.appsettings.json");
            var configuration = new ConfigurationBuilder()
                .AddJsonStream(stream!)
                .Build();
            await stream.DisposeAsync();

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry

                .AddManagerFor<IReqResSimpleService>(options => options
                    //.WithConfiguration(context.Configuration.GetSection("Apizr:ProperOptions:IReqResSimpleService")) // Specific section (manual mapped config)
                    .WithBaseAddress("https://reqres.in/api")
                    .WithHeaders(() => requestHeaders, scope: ApizrLifetimeScope.Request)
                    .WithHeaders(["testKey6: testValue6.1", "testKey7: testValue7.2"])
                    .WithHeaders(testSettings, [settings => settings.TestJsonString], scope: ApizrLifetimeScope.Request)
                    .WithHeaders(testStore, [settings => settings.TestJsonString], scope: ApizrLifetimeScope.Request, mode: ApizrRegistrationMode.Store)
                    .WithDelegatingHandler(watcher))

                .AddManagerFor<IHttpBinService>(options => options
                    //.WithConfiguration(context.Configuration.GetSection("Apizr:ProperOptions:IHttpBinService")) // Specific section (manual mapped config)
                    .WithHeaders(["testKey3: testValue3.4", "testKey4: testValue4.2"])
                    .WithLoggedHeadersRedactionNames(["testKey4"])
                    .WithDelegatingHandler(watcher2))

                .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(),

                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithConfiguration(configuration) // Whole configuration (auto mapped config)
                    //.WithConfiguration(configuration.GetSection("Apizr")) // Root section (auto mapped config)
                    //.WithConfiguration(configuration.GetSection("Apizr:CommonOptions")) // Specific section (manual mapped config)
                    //.WithLogging()
                    .WithResilienceContextOptions(opt =>
                        opt.ReturnToPoolOnComplete(false))
                    .WithHeaders(() => apiHeaders, scope: ApizrLifetimeScope.Api)
                    .WithHeaders(["testKey7: testValue7.1", "testKey8: testValue8.1"])
                    .WithHeaders(["testStoreKey1: testStoreValue1.1", "testStoreKey3: testStoreValue3.1",
                        "testSettingsKey4: testSettingsValue4.1", "testSettingsKey5: testSettingsValue5.1"], mode: ApizrRegistrationMode.Store));

            // Shortcut
            apizrRegistry.TryGetManagerFor<IReqResSimpleService>(out var simpleManager).Should().BeTrue();
            apizrRegistry.TryGetManagerFor<IHttpBinService>(out var httpBinManager).Should().BeTrue();
            apizrRegistry.TryGetCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(out var userManager).Should().BeTrue();

            simpleManager.Options.OperationTimeout.Should().Be(TimeSpan.Parse("00:00:10"));
            httpBinManager.Options.OperationTimeout.Should().Be(TimeSpan.Parse("00:00:10"));
            userManager.Options.OperationTimeout.Should().Be(TimeSpan.Parse("00:00:10"));
            simpleManager.Options.RequestTimeout.Should().Be(TimeSpan.Parse("00:00:03"));
            httpBinManager.Options.RequestTimeout.Should().Be(TimeSpan.Parse("00:00:04"));
            userManager.Options.RequestTimeout.Should().Be(TimeSpan.Parse("00:00:05"));

            // Shortcut
            await simpleManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt),
                options => options.WithHeaders(["testKey5: testValue5.2",
                    "testKey6: testValue6.2"]));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKeys("testKey1", "testKey2", "testKey3", "testKey4", "testKey5", "testKey6", "testKey7", "testKey8",
                    "testSettingsKey1", "testSettingsKey2", "testSettingsKey3", "testSettingsKey4", "testSettingsKey5",
                    "testSettingsKey6", "testSettingsKey7", "testStoreKey1", "testStoreKey2")
                .And.NotContainKey("testStoreKey3");
            watcher.Headers.GetValues("testKey1").Should().HaveCount(1).And.Contain("testValue1"); // Set by attribute
            watcher.Headers.GetValues("testKey2").Should().HaveCount(1).And.Contain("testValue2.2"); // Set by attribute then updated by common option within api scope factory
            watcher.Headers.GetValues("testKey3").Should().HaveCount(1).And.Contain("testValue3.2"); // Set by common option within api scope factory then updated by proper option within request scope factory
            watcher.Headers.GetValues("testKey4").Should().HaveCount(1).And.Contain("testValue4.1"); // Set by proper option within request scope factory
            watcher.Headers.GetValues("testKey5").Should().HaveCount(1).And.Contain("testValue5.2"); // Set by proper option within request scope factory then updated by request option
            watcher.Headers.GetValues("testKey6").Should().HaveCount(1).And.Contain("testValue6.2"); // Set by proper option then updated by request option
            watcher.Headers.GetValues("testKey7").Should().HaveCount(1).And.Contain("testValue7.2"); // Set by common option then by proper option
            watcher.Headers.GetValues("testKey8").Should().HaveCount(1).And.Contain("testValue8.1"); // Set by common option
            watcher.Headers.GetValues("testSettingsKey1").Should().HaveCount(1).And.Contain("testSettingsValue1.1"); // Set by common option expression
            watcher.Headers.GetValues("testSettingsKey2").Should().HaveCount(1).And.Contain("testSettingsValue2.1"); // Set by proper option configuration
            watcher.Headers.GetValues("testSettingsKey3").Should().HaveCount(1).And.Contain("testSettingsValue3.1"); // Set by proper option configuration
            watcher.Headers.GetValues("testSettingsKey4").Should().HaveCount(1).And.Contain("testSettingsValue4.1"); // Set by proper option configuration
            watcher.Headers.GetValues("testSettingsKey5").Should().HaveCount(1).And.Contain("testSettingsValue5.1"); // Set by proper option configuration
            watcher.Headers.GetValues("testSettingsKey6").Should().HaveCount(1).And.Contain("testSettingsValue6.1"); // Set by common option configuration
            watcher.Headers.GetValues("testSettingsKey7").Should().HaveCount(1).And.Contain("testSettingsValue7.1"); // Set by proper's request option configuration
            watcher.Headers.GetValues("testStoreKey1").Should().HaveCount(1).And.Contain("testStoreValue1.1"); // Set by common option from Store
            watcher.Headers.GetValues("testStoreKey2").Should().HaveCount(1).And.Contain("testStoreValue2.1"); // Set by common option from Store

            // Keep updated
            apiHeaders[1] = "testKey3: testValue3.3"; // will not be updated (scope: Api)
            requestHeaders[1] = "testKey4: testValue4.2"; // will be updated (scope: Request)
            requestHeaders[2] = "testKey5: testValue5.3"; // should be updated (scope: Request) but updated then by request option
            testSettings.TestJsonString = "testSettingsKey1: testSettingsValue1.2";
            testStore.TestJsonString = "testStoreKey2: testStoreValue2.2";

            await simpleManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt),
                options => options.WithHeaders(["testKey5: testValue5.4",
                    "testKey6: testValue6.3",
                    "testStoreKey1: *testStoreValue1.2*",
                    "testStoreKey3: *{0}*"]));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKeys("testKey1", "testKey2", "testKey3", "testKey4", "testKey5", "testKey6", "testKey7", "testKey8",
                "testSettingsKey1", "testSettingsKey2", "testSettingsKey3", "testSettingsKey4", "testSettingsKey5",
                "testSettingsKey6", "testSettingsKey7", "testStoreKey1", "testStoreKey2", "testStoreKey3");
            watcher.Headers.GetValues("testKey1").Should().HaveCount(1).And.Contain("testValue1"); // Same as previous value
            watcher.Headers.GetValues("testKey2").Should().HaveCount(1).And.Contain("testValue2.2"); // Same as previous value
            watcher.Headers.GetValues("testKey3").Should().HaveCount(1).And.Contain("testValue3.2"); // Same as previous value
            watcher.Headers.GetValues("testKey4").Should().HaveCount(1).And.Contain("testValue4.2"); // Updated at request time (scope: Request)
            watcher.Headers.GetValues("testKey5").Should().HaveCount(1).And.Contain("testValue5.4"); // Updated at request time (scope: Request) then by request option
            watcher.Headers.GetValues("testKey6").Should().HaveCount(1).And.Contain("testValue6.3"); // Updated by request option
            watcher.Headers.GetValues("testKey7").Should().HaveCount(1).And.Contain("testValue7.2"); // Same as previous value
            watcher.Headers.GetValues("testKey8").Should().HaveCount(1).And.Contain("testValue8.1"); // Same as previous value
            watcher.Headers.GetValues("testSettingsKey1").Should().HaveCount(1).And.Contain("testSettingsValue1.2"); // Updated at request time (scope: Request)
            watcher.Headers.GetValues("testSettingsKey2").Should().HaveCount(1).And.Contain("testSettingsValue2.1"); // Same as previous value
            watcher.Headers.GetValues("testSettingsKey3").Should().HaveCount(1).And.Contain("testSettingsValue3.1"); // Same as previous value
            watcher.Headers.GetValues("testSettingsKey4").Should().HaveCount(1).And.Contain("testSettingsValue4.1"); // Same as previous value
            watcher.Headers.GetValues("testSettingsKey5").Should().HaveCount(1).And.Contain("testSettingsValue5.1"); // Same as previous value
            watcher.Headers.GetValues("testSettingsKey6").Should().HaveCount(1).And.Contain("testSettingsValue6.1"); // Same as previous value
            watcher.Headers.GetValues("testSettingsKey7").Should().HaveCount(1).And.Contain("testSettingsValue7.1"); // Same as previous value
            watcher.Headers.GetValues("testStoreKey1").Should().HaveCount(1).And.Contain("testStoreValue1.2"); // Updated by request option
            watcher.Headers.GetValues("testStoreKey2").Should().HaveCount(1).And.Contain("testStoreValue2.2"); // Updated at request time (scope: Request)
            watcher.Headers.GetValues("testStoreKey3").Should().HaveCount(1).And.Contain("testStoreValue3.1"); // Set by request option from Store
        }

        [Fact]
        public async Task Requesting_With_Both_Attribute_And_Fluent_Headers_Should_Set_Merged_Headers()
        {
            var watcher = new WatchingRequestHandler();

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResSimpleService>(options => options.WithHeaders(["testKey2: testValue2"])
                        .WithDelegatingHandler(watcher)),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithResilienceContextOptions(opt =>
                        opt.ReturnToPoolOnComplete(false))
                    .WithHeaders(["testKey3: testValue3"]));

            apizrRegistry.TryGetManagerFor<IReqResSimpleService>(out var reqResManager).Should().BeTrue(); // Custom
            
            await reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt),
                options => options.WithHeaders(["testKey4: testValue4"]));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKeys("testKey1", "testKey2", "testKey3", "testKey4");
        }

        [Fact]
        public async Task Calling_ConfigureClient_Should_Configure_HttpClient()
        {
            var watcher = new WatchingRequestHandler();

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddManagerFor<IReqResUserService>(options =>
                options.ConfigureHttpClient(client => client.DefaultRequestHeaders.Add("HttpClientHeaderKey", "HttpClientHeaderValue"))
                    .WithDelegatingHandler(watcher)),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithResilienceContextOptions(opt =>
                        opt.ReturnToPoolOnComplete(false)));

            apizrRegistry.TryGetManagerFor<IReqResUserService>(out var reqResManager).Should().BeTrue();

            await reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKey("HttpClientHeaderKey");
        }

        [Fact]
        public async Task Concurrent_Requests_Should_Not_Throw()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResSimpleService>(),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging());

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
                    .AddManagerFor<IReqResUserService>(),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging());

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
                    .AddManagerFor<IHttpBinService>(),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging());

            apizrRegistry.TryGetManagerFor<IHttpBinService>(out var manager).Should().BeTrue();

            var streamPart = FileHelper.GetTestFileStreamPart("medium");
            var ct = new CancellationTokenSource();
            ct.CancelAfter(TimeSpan.FromSeconds(2));

            Func<Task> act = () => manager.ExecuteAsync((opt, api) => api.UploadAsync(streamPart, opt),
                options => options.WithCancellation(ct.Token));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<OperationCanceledException>();
        }

        [Fact]
        public async Task When_Calling_BA_WithOperationTimeout_Then_Request_Should_Throw_A_TimeoutException()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(
                        options => options.WithOperationTimeout(TimeSpan.FromSeconds(4))),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging());

            apizrRegistry.TryGetManagerFor<IReqResUserService>(out var reqResManager).Should().BeTrue();

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(6, opt),
                    options => options.WithOperationTimeout(TimeSpan.FromSeconds(2)));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutRejectedException>();
        }

        [Fact]
        public async Task When_Calling_AB_WithOperationTimeout_Then_Client_Should_Throw_A_TimeoutException()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(options =>
                        options.WithOperationTimeout(TimeSpan.FromSeconds(2))),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging());

            apizrRegistry.TryGetManagerFor<IReqResUserService>(out var reqResManager).Should().BeTrue();

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(6, opt),
                    options => options.WithOperationTimeout(TimeSpan.FromSeconds(4)));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutRejectedException>();
        }

        [Fact]
        public async Task When_Calling_BA_WithRequestTimeout_Then_Request_Should_Throw_A_TimeoutException()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(
                        options => options.WithRequestTimeout(TimeSpan.FromSeconds(4))),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging());

            apizrRegistry.TryGetManagerFor<IReqResUserService>(out var reqResManager).Should().BeTrue();

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(6, opt),
                    options => options.WithRequestTimeout(TimeSpan.FromSeconds(2)));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutRejectedException>();
        }

        [Fact]
        public async Task When_Calling_AB_WithRequestTimeout_Then_Client_Should_Throw_A_TimeoutException()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(options =>
                        options.WithRequestTimeout(TimeSpan.FromSeconds(2))),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging());

            apizrRegistry.TryGetManagerFor<IReqResUserService>(out var reqResManager).Should().BeTrue();

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(6, opt),
                    options => options.WithRequestTimeout(TimeSpan.FromSeconds(4)));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutRejectedException>();
        }

        [Fact]
        public async Task When_Calling_DCBA_WithOperationTimeout_And_WithRequestTimeout_Then_Request_Should_Throw_A_TimeoutException()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(
                        options => options.WithOperationTimeout(TimeSpan.FromSeconds(8))
                            .WithRequestTimeout(TimeSpan.FromSeconds(6))),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging());

            apizrRegistry.TryGetManagerFor<IReqResUserService>(out var reqResManager).Should().BeTrue();

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(6, opt),
                    options => options.WithOperationTimeout(TimeSpan.FromSeconds(4))
                        .WithRequestTimeout(TimeSpan.FromSeconds(2)));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutRejectedException>();
        }

        [Fact]
        public async Task When_Calling_ABCD_WithOperationTimeout_And_WithRequestTimeout_Then_Client_Should_Throw_A_TimeoutException()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(options =>
                        options.WithOperationTimeout(TimeSpan.FromSeconds(2))
                            .WithRequestTimeout(TimeSpan.FromSeconds(4))),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging());

            apizrRegistry.TryGetManagerFor<IReqResUserService>(out var reqResManager).Should().BeTrue();

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(6, opt),
                    options => options.WithOperationTimeout(TimeSpan.FromSeconds(6))
                        .WithRequestTimeout(TimeSpan.FromSeconds(8)));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutRejectedException>();
        }

        [Fact]
        public async Task Calling_BCA_Both_WithTimeout_And_WithCancellation_Should_Throw_A_Request_TimeoutRejectedException()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(options =>
                        options.WithOperationTimeout(TimeSpan.FromSeconds(4))),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging());

            apizrRegistry.TryGetManagerFor<IReqResUserService>(out var reqResManager).Should().BeTrue();

            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(6));

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(8, opt),
                    options => options.WithOperationTimeout(TimeSpan.FromSeconds(2))
                        .WithCancellation(cts.Token));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutRejectedException>();
        }

        [Fact]
        public async Task Calling_ACB_Both_WithTimeout_And_WithCancellation_Should_Throw_A_Client_TimeoutRejectedException()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(options =>
                        options.WithOperationTimeout(TimeSpan.FromSeconds(2))),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging());

            apizrRegistry.TryGetManagerFor<IReqResUserService>(out var reqResManager).Should().BeTrue();

            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(6));

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(8, opt),
                    options => options.WithOperationTimeout(TimeSpan.FromSeconds(4))
                        .WithCancellation(cts.Token));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutRejectedException>();
        }

        [Fact]
        public async Task Calling_BAC_Both_WithTimeout_And_WithCancellation_Should_Throw_An_OperationCanceledException()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(options =>
                        options.WithOperationTimeout(TimeSpan.FromSeconds(4))),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging());

            apizrRegistry.TryGetManagerFor<IReqResUserService>(out var reqResManager).Should().BeTrue();

            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(2));

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(8, opt),
                    options => options.WithOperationTimeout(TimeSpan.FromSeconds(6))
                        .WithCancellation(cts.Token));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<OperationCanceledException>();
        }

        [Fact]
        public async Task When_Calling_WithRequestTimeout_With_TimeoutRejected_Resilience_Strategy_Then_It_Should_Retry_3_On_3_Times()
        {
            var maxRetryCount = 3;
            var retryCount = 0;

            var resiliencePipelineRegistry = new ResiliencePipelineRegistry<string>();
            resiliencePipelineRegistry.TryAddBuilder<HttpResponseMessage>("TransientHttpError", (builder, _) =>
                builder.ConfigureTelemetry(LoggerFactory.Create(loggingBuilder =>
                        loggingBuilder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                .AddRetry(
                    new RetryStrategyOptions<HttpResponseMessage>
                    {
                        ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                            .Handle<HttpRequestException>()
                            .Handle<TimeoutRejectedException>()
                            .HandleResult(response =>
                                response.StatusCode is >= HttpStatusCode.InternalServerError
                                    or HttpStatusCode.RequestTimeout),
                        MaxRetryAttempts = maxRetryCount,
                        DelayGenerator = static args =>
                        {
                            var delay = args.AttemptNumber switch
                            {
                                0 => TimeSpan.FromSeconds(1),
                                1 => TimeSpan.FromSeconds(2),
                                _ => TimeSpan.FromSeconds(3)
                            };

                            // This example uses a synchronous delay generator,
                            // but the API also supports asynchronous implementations.
                            return new ValueTask<TimeSpan?>(delay);
                        },
                        OnRetry = args =>
                        {
                            retryCount = args.AttemptNumber+1;
                            return default;
                        }
                    }));

            var watcher = new WatchingRequestHandler();

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(),

                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithResilienceContextOptions(opt =>
                        opt.ReturnToPoolOnComplete(false))
                    .WithResiliencePipelineRegistry(resiliencePipelineRegistry)
                    .WithRequestTimeout(TimeSpan.FromSeconds(3))
                    .WithDelegatingHandler(watcher));

            apizrRegistry.TryGetManagerFor<IReqResUserService>(out var reqResManager).Should().BeTrue();

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(6, opt));//,
            //options => options.WithTimeout(TimeSpan.FromSeconds(3)));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutRejectedException>();

            retryCount.Should().Be(3);
            watcher.Attempts.Should().Be(4);
        }

        [Fact]
        public async Task When_Calling_WithOperationTimeout_With_TimeoutRejected_Resilience_Strategy_Then_It_Should_Retry_2_On_3_Times()
        {
            var maxRetryCount = 3;
            var retryCount = 0;

            var resiliencePipelineRegistry = new ResiliencePipelineRegistry<string>();
            resiliencePipelineRegistry.TryAddBuilder<HttpResponseMessage>("TransientHttpError", (builder, _) =>
                builder.ConfigureTelemetry(LoggerFactory.Create(loggingBuilder =>
                        loggingBuilder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                .AddRetry(
                    new RetryStrategyOptions<HttpResponseMessage>
                    {
                        ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                            .Handle<HttpRequestException>()
                            .Handle<TimeoutRejectedException>()
                            .HandleResult(response =>
                                response.StatusCode is >= HttpStatusCode.InternalServerError
                                    or HttpStatusCode.RequestTimeout),
                        MaxRetryAttempts = maxRetryCount,
                        DelayGenerator = static args =>
                        {
                            var delay = args.AttemptNumber switch
                            {
                                0 => TimeSpan.FromSeconds(1),
                                1 => TimeSpan.FromSeconds(2),
                                _ => TimeSpan.FromSeconds(3)
                            };

                            // This example uses a synchronous delay generator,
                            // but the API also supports asynchronous implementations.
                            return new ValueTask<TimeSpan?>(delay);
                        },
                        OnRetry = args =>
                        {
                            retryCount = args.AttemptNumber+1;
                            return default;
                        }
                    }));

            var watcher = new WatchingRequestHandler();

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(),

                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithResilienceContextOptions(opt =>
                        opt.ReturnToPoolOnComplete(false))
                    .WithResiliencePipelineRegistry(resiliencePipelineRegistry)
                    .WithOperationTimeout(TimeSpan.FromSeconds(10))
                    .WithDelegatingHandler(watcher));

            apizrRegistry.TryGetManagerFor<IReqResUserService>(out var reqResManager).Should().BeTrue();

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(6, opt),
                    options => options.WithRequestTimeout(TimeSpan.FromSeconds(3)));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutRejectedException>();

            // attempts should be equal to 2 as request timed out before the 3rd retry
            retryCount.Should().Be(2);
            watcher.Attempts.Should().Be(3);
        }

        [Fact]
        public async Task When_Calling_WithRequestTimeout_WithOperationTimeout_WithCancellation_And_With_TimeoutRejected_Resilience_Strategy_Then_It_Should_Retry_1_On_3_Times()
        {
            var maxRetryCount = 3;
            var retryCount = 0;

            var resiliencePipelineRegistry = new ResiliencePipelineRegistry<string>();
            resiliencePipelineRegistry.TryAddBuilder<HttpResponseMessage>("TransientHttpError", (builder, _) =>
                builder.ConfigureTelemetry(LoggerFactory.Create(loggingBuilder =>
                        loggingBuilder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                .AddRetry(
                    new RetryStrategyOptions<HttpResponseMessage>
                    {
                        ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                            .Handle<HttpRequestException>()
                            .Handle<TimeoutRejectedException>()
                            .HandleResult(response =>
                                response.StatusCode is >= HttpStatusCode.InternalServerError
                                    or HttpStatusCode.RequestTimeout),
                        MaxRetryAttempts = maxRetryCount,
                        DelayGenerator = static args =>
                        {
                            var delay = args.AttemptNumber switch
                            {
                                0 => TimeSpan.FromSeconds(1),
                                1 => TimeSpan.FromSeconds(2),
                                _ => TimeSpan.FromSeconds(3)
                            };

                            // This example uses a synchronous delay generator,
                            // but the API also supports asynchronous implementations.
                            return new ValueTask<TimeSpan?>(delay);
                        },
                        OnRetry = args =>
                        {
                            retryCount = args.AttemptNumber + 1;
                            return default;
                        }
                    }));

            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5));

            var watcher = new WatchingRequestHandler();

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(),

                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithResilienceContextOptions(opt =>
                        opt.ReturnToPoolOnComplete(false))
                    .WithResiliencePipelineRegistry(resiliencePipelineRegistry)
                    .WithOperationTimeout(TimeSpan.FromSeconds(10))
                    .WithDelegatingHandler(watcher));

            apizrRegistry.TryGetManagerFor<IReqResUserService>(out var reqResManager).Should().BeTrue();

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(6, opt),
                    options => options.WithRequestTimeout(TimeSpan.FromSeconds(3))
                        .WithCancellation(cts.Token));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TaskCanceledException>();

            // attempts should be equal to 1 as request timed out before other retries
            retryCount.Should().Be(1);
            watcher.Attempts.Should().Be(2);
        }

        [Fact]
        public async Task Request_Returning_Timeout_Should_Time_Out_Before_Polly_Could_Complete_All_Retries()
        {
            var maxRetryCount = 3;
            var retryCount = 0;

            var resiliencePipelineRegistry = new ResiliencePipelineRegistry<string>();
            resiliencePipelineRegistry.TryAddBuilder<HttpResponseMessage>("TransientHttpError", (builder, _) =>
                builder.ConfigureTelemetry(LoggerFactory.Create(loggingBuilder =>
                        loggingBuilder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                .AddRetry(
                    new RetryStrategyOptions<HttpResponseMessage>
                    {
                        ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                            .Handle<HttpRequestException>()
                            .HandleResult(response =>
                                response.StatusCode is >= HttpStatusCode.InternalServerError
                                    or HttpStatusCode.RequestTimeout),
                        MaxRetryAttempts = maxRetryCount,
                        DelayGenerator = static args =>
                        {
                            var delay = args.AttemptNumber switch
                            {
                                0 => TimeSpan.FromSeconds(1),
                                1 => TimeSpan.FromSeconds(2),
                                _ => TimeSpan.FromSeconds(3)
                            };

                            // This example uses a synchronous delay generator,
                            // but the API also supports asynchronous implementations.
                            return new ValueTask<TimeSpan?>(delay);
                        },
                        OnRetry = args =>
                        {
                            retryCount = args.AttemptNumber + 1;
                            return default;
                        }
                    }));

            var testHandler = new TestRequestHandler();

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(),

                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithResiliencePipelineRegistry(resiliencePipelineRegistry)
                    .WithDelegatingHandler(testHandler)
                    .WithOperationTimeout(TimeSpan.FromSeconds(3)));

            apizrRegistry.TryGetManagerFor<IReqResUserService>(out var reqResManager).Should().BeTrue();

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(HttpStatusCode.RequestTimeout, opt));//,
                                                                                                                //options => options.WithTimeout(TimeSpan.FromSeconds(3)));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutRejectedException>();

            // attempts should be equal to 2 as request timed out before the 3rd retry
            retryCount.Should().Be(2);
            testHandler.Attempts.Should().Be(2);
        }

        [Fact]
        public async Task Configuring_Exceptions_Handler_Class_Should_Handle_Exceptions()
        {
            var handledException = 0;
            bool OnException(ApizrException ex)
            {
                handledException++;
                return handledException == 1;
            }

            bool mismatchHandled = false;
            bool OnMismatchException(ApizrException<User> ex)
            {
                mismatchHandled = true;
                return false;
            }

            var myExHandler = new MyExHandler();

            // Try to queue ex handlers
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                    .AddGroup(group => group
                            .AddManagerFor<IReqResUserService>(options => options
                                .WithExCatching(OnException, strategy: ApizrDuplicateStrategy.Add)
                                .WithExCatching<User>(OnMismatchException, strategy: ApizrDuplicateStrategy.Add)
                                .WithDelegatingHandler(new TestRequestHandler()))
                            .AddManagerFor<IReqResResourceService>(),
                        options => options.WithExCatching(myExHandler, strategy: ApizrDuplicateStrategy.Add))
                    .AddManagerFor<IHttpBinService>()
                    .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(),
                options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithExCatching(() => myExHandler, strategy: ApizrDuplicateStrategy.Add));


            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();

            // Defining a transient throwing request
            Func<Task> act = () => reqResManager.ExecuteAsync(api => api.GetUsersAsync(HttpStatusCode.RequestTimeout),
                options => options.WithExCatching(myExHandler, strategy: ApizrDuplicateStrategy.Add));

            // Calling it should throw but handled by Polly
            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.And.Handled.Should().BeTrue();

            myExHandler.Counter.Should().Be(3);
            handledException.Should().Be(1);
            mismatchHandled.Should().BeFalse();

            // Try to replace queued ex handlers by the last one set at request time
            myExHandler.Counter = 0;
            handledException = 0;

            // Defining a transient throwing request
            act = () => reqResManager.ExecuteAsync(api => api.GetUsersAsync(HttpStatusCode.RequestTimeout),
                options => options.WithExCatching(myExHandler, strategy: ApizrDuplicateStrategy.Replace));

            // Calling it should throw but handled by Polly
            ex = await act.Should().ThrowAsync<ApizrException>();
            ex.And.Handled.Should().BeFalse();

            myExHandler.Counter.Should().Be(1);
            handledException.Should().Be(0);
            mismatchHandled.Should().BeFalse();
        }

        public class MyExHandler : IApizrExceptionHandler
        {
            public int Counter { get; set; }

            /// <inheritdoc />
            public Task<bool> HandleAsync(ApizrException ex)
            {
                Counter++;
                return Task.FromResult(Counter == 2);
            }
        }
    }
}
