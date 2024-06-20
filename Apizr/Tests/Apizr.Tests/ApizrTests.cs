using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring;
using Apizr.Logging;
using Apizr.Progressing;
using Apizr.Resiliencing;
using Apizr.Tests.Apis;
using Apizr.Tests.Helpers;
using Apizr.Tests.Models;
using Apizr.Tests.Models.Mappings;
using Apizr.Tests.Settings;
using AutoMapper;
using FluentAssertions;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileSystemGlobbing;
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
    public class ApizrTests
    {
        private readonly RefitSettings _refitSettings;
        private readonly ITestOutputHelper _outputHelper;

        public ApizrTests(ITestOutputHelper outputHelper)
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
        public void Apizr_Should_Create_Manager()
        {
            Func<IApizrManager> reqResManagerFactory = () => ApizrBuilder.Current.CreateManagerFor<IReqResUserService>();
            reqResManagerFactory.Should().NotThrow().Which.Should().NotBeNull();

            Func<IApizrManager> httpBinManagerFactory = () => ApizrBuilder.Current.CreateManagerFor<IHttpBinService>();
            httpBinManagerFactory.Should().NotThrow().Which.Should().NotBeNull();

            Func<IApizrManager> userManagerFactory = () => ApizrBuilder.Current.CreateCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>();
            userManagerFactory.Should().NotThrow().Which.Should().NotBeNull();

            Func<IApizrManager> uploadManagerFactory = () => ApizrBuilder.Current.CreateUploadManager(options => options.WithBaseAddress("http://speedtest.ftp.otenet.gr/files"));
            uploadManagerFactory.Should().NotThrow().Which.Should().NotBeNull();

            Func<IApizrManager> uploadManagerForFactory = () => ApizrBuilder.Current.CreateUploadManagerFor<ITransferSampleApi>();
            uploadManagerForFactory.Should().NotThrow().Which.Should().NotBeNull();

            Func<IApizrManager> uploadManagerWithFactory = () => ApizrBuilder.Current.CreateUploadManagerWith<string>(options => options.WithBaseAddress("http://speedtest.ftp.otenet.gr/files"));
            uploadManagerWithFactory.Should().NotThrow().Which.Should().NotBeNull();

            Func<IApizrManager> downloadManagerFactory = () => ApizrBuilder.Current.CreateDownloadManager(options => options.WithBaseAddress("http://speedtest.ftp.otenet.gr/files"));
            downloadManagerFactory.Should().NotThrow().Which.Should().NotBeNull();

            Func<IApizrManager> downloadManagerForFactory = () => ApizrBuilder.Current.CreateDownloadManagerFor<ITransferSampleApi>();
            downloadManagerForFactory.Should().NotThrow().Which.Should().NotBeNull();

            Func<IApizrManager> downloadManagerWithFactory = () => ApizrBuilder.Current.CreateDownloadManagerWith<User>(options => options.WithBaseAddress("http://speedtest.ftp.otenet.gr/files"));
            downloadManagerWithFactory.Should().NotThrow().Which.Should().NotBeNull();

            Func<IApizrManager> transferManagerFactory = () => ApizrBuilder.Current.CreateTransferManager(options => options.WithBaseAddress("http://speedtest.ftp.otenet.gr/files"));
            transferManagerFactory.Should().NotThrow().Which.Should().NotBeNull();

            Func<IApizrManager> transferManagerForFactory = () => ApizrBuilder.Current.CreateTransferManagerFor<ITransferSampleApi>();
            transferManagerForFactory.Should().NotThrow().Which.Should().NotBeNull();

            Func<IApizrManager> transferManagerWithFactory = () => ApizrBuilder.Current.CreateTransferManagerWith<User, string>(options => options.WithBaseAddress("http://speedtest.ftp.otenet.gr/files"));
            transferManagerWithFactory.Should().NotThrow().Which.Should().NotBeNull();
        }

        [Fact]
        public void Calling_WithBaseAddress_Should_Set_BaseUri()
        {
            var attributeAddress = "https://reqres.in/api";
            var uri1 = new Uri("http://uri1.com");

            // By attribute
            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>();
            reqResManager.Options.BaseUri.Should().Be(attributeAddress);

            // By proper option overriding attribute
            reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                options.WithBaseAddress(uri1));
            reqResManager.Options.BaseUri.Should().Be(uri1);
        }

        [Fact]
        public void Calling_WithBaseAddress_And_WithBasePath_Should_Set_BaseUri()
        {
            var baseAddress = "https://reqres.in/api";
            var basePath = "users";
            var baseUri = $"{baseAddress}/{basePath}";

            // By proper option
            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserPathService>(options => options.WithBaseAddress(baseAddress).WithBasePath(basePath));
            reqResManager.Options.BaseUri.Should().Be(baseUri);
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

            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResSimpleService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                    .WithDelegatingHandler(watcher)
                    .WithHttpMessageHandler(mockHttp));

            var result = await reqResManager.ExecuteAsync(api => api.GetUsersAsync());

            result.Should().NotBeNull();
            result.Data.Should().NotBeNullOrEmpty();
            result.Data.First().FirstName.Should().Be("George");
            watcher.Attempts.Should().Be(1);
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_Should_Authenticate_Request()
        {
            string token = null;
            Task<string> OnRefreshToken(HttpRequestMessage _) => Task.FromResult(token = "token");

            var httpBinManager = ApizrBuilder.Current.CreateManagerFor<IHttpBinService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithHttpMessageHandler(new WatchingRequestHandler())
                    .WithAuthenticationHandler(OnRefreshToken));

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
            token.Should().Be("token");
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_With_Default_Token_Should_Authenticate_Request()
        {
            var testSettings = new TestSettings("token");

            var httpBinManager = ApizrBuilder.Current.CreateManagerFor<IHttpBinService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithAuthenticationHandler(testSettings, settings => settings.TestJsonString));

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public void Calling_WithLogging_Should_Set_LoggingSettings()
        {
            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(
                options => options.WithLogging(HttpTracerMode.ExceptionsOnly,
                    HttpMessageParts.RequestCookies, LogLevel.Warning));

            reqResManager.Options.HttpTracerMode.Should().Be(HttpTracerMode.ExceptionsOnly);
            reqResManager.Options.TrafficVerbosity.Should().Be(HttpMessageParts.RequestCookies);
            reqResManager.Options.LogLevels.Should().AllBeEquivalentTo(LogLevel.Warning);
        }
        
        [Fact]
        public async Task Calling_Without_Configuring_Logging_Should_Log_With_Default_Values()
        {
            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResSimpleService>(options => 
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))));

            var result = await reqResManager.ExecuteAsync(api => api.GetUsersAsync());

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Calling_WithAkavacheCacheHandler_Should_Cache_Result()
        {
            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options => 
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace)))
                .WithLogging()
                .WithAkavacheCacheHandler()
                .WithDelegatingHandler(new TestRequestHandler()));

            // Clearing cache
            await reqResManager.ClearCacheAsync();

            // Defining a throwing request
            Func<Task> act1 = () => reqResManager.ExecuteAsync(api => api.GetUserAsync(1, HttpStatusCode.BadRequest));

            // Calling it at first execution should throw as expected without any cached result
            var ex = await act1.Should().ThrowAsync<ApizrException<UserDetails>>();
            ex.And.CachedResult.Should().BeNull();

            // This one should succeed
            var result = await reqResManager.ExecuteAsync(api => api.GetUserAsync(1));

            // and cache result in-memory
            result.Should().NotBeNull();

            // This one should fail but with cached result
            var ex2 = await act1.Should().ThrowAsync<ApizrException<UserDetails>>();
            ex2.And.CachedResult.Should().NotBeNull();

            // Defining another throwing request
            Func<Task> act2 = () => reqResManager.ExecuteAsync(api => api.GetUserAsync(2, HttpStatusCode.BadRequest));

            // Calling it again with another cache key value should throw as expected but without any cached result
            var ex3 = await act2.Should().ThrowAsync<ApizrException<UserDetails>>();
            ex3.And.CachedResult.Should().BeNull();
        }

        [Fact]
        public async Task Calling_WithAkavacheCacheHandler_Should_Cache_ApizrResponse()
        {
            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithAkavacheCacheHandler()
                    .WithDelegatingHandler(new TestRequestHandler()));

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
            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithAkavacheCacheHandler()
                    .WithDelegatingHandler(new TestRequestHandler()));

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
                                retryCount = args.AttemptNumber + 1;
                                return default;
                            }
                        }));

            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithResiliencePipelineRegistry(resiliencePipelineRegistry)
                    .WithDelegatingHandler(new TestRequestHandler()));

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

            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithConnectivityHandler(() => isConnected));

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
            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options => 
                options.WithRefitSettings(_refitSettings));

            reqResManager.Options.RefitSettings.Should().Be(_refitSettings);
        }

        [Fact]
        public async Task Calling_Classic_WithAutoMapperMappingHandler_Should_Map_Data()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<UserDetailsUserInfosProfile>();
                config.AddProfile<UserMinUserProfile>();
            });

            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithRefitSettings(_refitSettings)
                    .WithAutoMapperMappingHandler(mapperConfig));

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
        public async Task Calling_Crud_WithAutoMapperMappingHandler_Should_Map_Data()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<UserDetailsUserInfosProfile>();
                config.AddProfile<UserMinUserProfile>();
            });

            var userManager =
                ApizrBuilder.Current.CreateCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(
                    options =>
                        options.WithLoggerFactory(LoggerFactory.Create(builder =>
                                builder.AddXUnit(_outputHelper)
                                    .SetMinimumLevel(LogLevel.Trace)))
                            .WithLogging()
                            .WithRefitSettings(_refitSettings)
                            .WithAutoMapperMappingHandler(mapperConfig));

            var minUser = new MinUser { Name = "John" };

            // This one should succeed
            var result =
                await userManager.ExecuteAsync<MinUser, User>(
                    (api, user) => api.Create(user), minUser);

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

            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithRefitSettings(_refitSettings)
                    .WithMappingHandler(new AutoMapperMappingHandler(mapperConfig.CreateMapper())));

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
        public async Task Calling_Classic_WithMapsterMappingHandler_Should_Map_Data()
        {
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            typeAdapterConfig.RuleMap.Clear();
            typeAdapterConfig.ForType<User, MinUser>()
                .TwoWays()
                .Map(minUser => minUser.Name, user => user.FirstName);

            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithRefitSettings(_refitSettings)
                    .WithMapsterMappingHandler(new MapsterMapper.Mapper()));

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
        public async Task Calling_Crud_WithMapsterMappingHandler_Should_Map_Data()
        {
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            typeAdapterConfig.RuleMap.Clear();
            typeAdapterConfig.ForType<User, MinUser>()
                .TwoWays()
                .Map(minUser => minUser.Name, user => user.FirstName);

            var userManager =
                ApizrBuilder.Current.CreateCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(
                    options =>
                        options.WithLoggerFactory(LoggerFactory.Create(builder =>
                                builder.AddXUnit(_outputHelper)
                                    .SetMinimumLevel(LogLevel.Trace)))
                            .WithLogging()
                            .WithRefitSettings(_refitSettings)
                            .WithMapsterMappingHandler(new MapsterMapper.Mapper()));

            var minUser = new MinUser { Name = "John" };

            // This one should succeed
            var result =
                await userManager.ExecuteAsync<MinUser, User>(
                    (api, user) => api.Create(user), minUser);

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

            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithRefitSettings(_refitSettings)
                    .WithMappingHandler(new MapsterMappingHandler(new MapsterMapper.Mapper())));

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
        public async Task Requesting_With_A_ResilienceProperty_into_Options_Should_Set_It_Into_Context()
        {
            var watcher = new WatchingRequestHandler();

            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithResilienceContextOptions(opt => 
                        opt.ReturnToPoolOnComplete(false))
                    .WithDelegatingHandler(watcher));

            ResiliencePropertyKey<int> testKey = new("TestKey1");
            var testValue = 1;

            await reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt), options => options.WithResilienceProperty(testKey, testValue));
            watcher.Context.Should().NotBeNull();
            watcher.Context.Properties.TryGetValue(testKey, out var value).Should().BeTrue();
            value.Should().Be(testValue);
        }

        [Fact]
        public async Task Requesting_With_ResilienceProperties_At_Multiple_Levels_Should_Merge_It_All_At_The_End()
        {
            var watcher = new WatchingRequestHandler();

            ResiliencePropertyKey<string> testKey1 = new(nameof(testKey1));
            ResiliencePropertyKey<string> testKey2 = new(nameof(testKey2));
            ResiliencePropertyKey<string> testKey3 = new(nameof(testKey3));

            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithResilienceContextOptions(opt =>
                        opt.ReturnToPoolOnComplete(false))
                    .WithResilienceProperty(testKey1, () => "testValue1")
                    .WithResilienceProperty(testKey2, () => "testValue2.1")
                    .WithDelegatingHandler(watcher));

            await reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt),
                options => options.WithResilienceProperty(testKey2, "testValue2.2")
                    .WithResilienceProperty(testKey3, "testValue3"));

            watcher.Context.Should().NotBeNull();
            watcher.Context.Properties.TryGetValue(testKey1, out var valueKey1).Should().BeTrue(); // Set by manager option
            valueKey1.Should().Be("testValue1");
            watcher.Context.Properties.TryGetValue(testKey2, out var valueKey2).Should().BeTrue(); // Set by manager option then updated by the request one
            valueKey2.Should().Be("testValue2.2");
            watcher.Context.Properties.TryGetValue(testKey3, out var valueKey3).Should().BeTrue(); // Set by request option
            valueKey3.Should().Be("testValue3");
        }

        [Fact]
        public async Task Requesting_With_LogSettings_Into_Options_Should_Win_Over_All_Others()
        {
            var watcher = new WatchingRequestHandler();

            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithResilienceContextOptions(opt =>
                        opt.ReturnToPoolOnComplete(false))
                    .WithDelegatingHandler(watcher)
                    .WithLogging(HttpTracerMode.ExceptionsOnly, HttpMessageParts.ResponseBody, LogLevel.Debug));
            
            await reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt), options => options
                .WithLogging(HttpTracerMode.Everything, HttpMessageParts.RequestCookies, LogLevel.Error));

            watcher.Context.Should().NotBeNull();
            watcher.Context.TryGetLogger(out var logger, out var logLevels, out var verbosity, out var tracerMode)
                .Should().BeTrue();
            tracerMode.Should().Be(HttpTracerMode.Everything);
            verbosity.Should().Be(HttpMessageParts.RequestCookies);
            logLevels.Should().Contain(LogLevel.Error);

            watcher.Options.Should().NotBeNull();
            watcher.Options.HttpTracerMode.Should().Be(HttpTracerMode.Everything);
            watcher.Options.TrafficVerbosity.Should().Be(HttpMessageParts.RequestCookies);
            watcher.Options.LogLevels.Should().Contain(LogLevel.Error);
        }

        [Fact]
        public async Task Downloading_File_Should_Succeed()
        {
            var apizrTransferManager = ApizrBuilder.Current.CreateTransferManager(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithBaseAddress("http://speedtest.ftp.otenet.gr/files"));

            apizrTransferManager.Should().NotBeNull(); // Built-in
            
            var result = await apizrTransferManager.DownloadAsync(new FileInfo("test10Mb.db"));
            result.Should().NotBeNull();
            result.Length.Should().BePositive();
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
            var apizrTransferManager = ApizrBuilder.Current.CreateTransferManager(options => 
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithBaseAddress("http://speedtest.ftp.otenet.gr/files")
                    .WithProgress());
            
            apizrTransferManager.Should().NotBeNull(); // Built-in

            var fileInfo = await apizrTransferManager.DownloadAsync(new FileInfo("test10Mb.db"), options => options.WithProgress(progress)).ConfigureAwait(false);

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
            var apizrTransferManager = ApizrBuilder.Current.CreateTransferManager(options => 
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithBaseAddress("http://speedtest.ftp.otenet.gr/files")
                    .WithProgress(progress));
            
            apizrTransferManager.Should().NotBeNull(); // Built-in

            var fileInfo = await apizrTransferManager.DownloadAsync(new FileInfo("test10Mb.db")).ConfigureAwait(false);

            percentage.Should().Be(100);
            fileInfo.Length.Should().BePositive();
        }

        //[Fact]
        //public async Task Uploading_File_Locally_Should_Succeed()
        //{
        //    var apizrUploadManager = ApizrBuilder.Current.CreateUploadManagerWith<string>(options =>
        //        options.WithLoggerFactory(LoggerFactory.Create(builder =>
        //                builder.AddXUnit(_outputHelper)
        //                    .SetMinimumLevel(LogLevel.Trace)))
        //            .WithBaseAddress("https://localhost:7015/upload"));

        //    apizrUploadManager.Should().NotBeNull(); // Built-in

        //    // Shortcut
        //    var result = await apizrUploadManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
        //    result.Should().NotBeNullOrWhiteSpace();
        //}

        [Fact]
        public async Task Uploading_File_Should_Succeed()
        {
            var apizrTransferManager = ApizrBuilder.Current.CreateTransferManager(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithBaseAddress("https://httpbin.org/post"));

            apizrTransferManager.Should().NotBeNull(); // Built-in

            // Shortcut
            var regShortcutResult = await apizrTransferManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            regShortcutResult.Should().NotBeNull();
            regShortcutResult.StatusCode.Should().Be(HttpStatusCode.OK);
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
            var apizrTransferManager = ApizrBuilder.Current.CreateTransferManager(options => 
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithBaseAddress("https://httpbin.org/post")
                    .WithProgress());
            
            apizrTransferManager.Should().NotBeNull(); // Built-in

            var apizrTransferManagerResult = await apizrTransferManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"), options => options.WithProgress(progress));

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
            var apizrTransferManager = ApizrBuilder.Current.CreateTransferManager(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithBaseAddress("https://httpbin.org/post")
                    .WithProgress(progress));
            
            apizrTransferManager.Should().NotBeNull(); // Built-in

            var apizrTransferManagerResult = await apizrTransferManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));

            apizrTransferManagerResult.Should().NotBeNull();
            apizrTransferManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);
            percentage.Should().Be(100);
        }

        [Fact]
        public async Task Requesting_With_Inherited_Headers_Should_Set_Headers()
        {
            var watcher = new WatchingRequestHandler();
            var apizrTransferManager = ApizrBuilder.Current.CreateTransferManagerFor<ITransferUndefinedApi>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithBaseAddress("https://httpbin.org/post")
                    .WithHeaders(["testKey2: testValue2"])
                    .WithDelegatingHandler(watcher));

            // Shortcut
            await apizrTransferManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKey("testKey2");
        }

        [Fact]
        public async Task Requesting_With_Headers_Should_Set_Headers()
        {
            var watcher = new WatchingRequestHandler();

            var apizrTransferManager = ApizrBuilder.Current.CreateManagerFor<IReqResSimpleService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithBaseAddress("https://reqres.in/api")
                    .WithHeaders(["testKey2: testValue2.2", "testKey3: testValue3.1"])
                    .WithLoggedHeadersRedactionNames(["testKey2"])
                    .WithDelegatingHandler(watcher));

            // Shortcut
            await apizrTransferManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt), options => 
                options.WithHeaders(["testKey3: testValue3.2", "testKey4: testValue4"])
                    .WithLoggedHeadersRedactionRule(header => header == "testKey3"));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKeys("testKey1", "testKey2", "testKey3", "testKey4");
            watcher.Headers.GetValues("testKey1").Should().HaveCount(1).And.Contain("testValue1"); // Set by attribute
            watcher.Headers.GetValues("testKey2").Should().HaveCount(1).And.Contain("testValue2.2"); // Set by attribute then updated by common option
            watcher.Headers.GetValues("testKey3").Should().HaveCount(1).And.Contain("testValue3.2"); // Set by common option then updated by request option
            watcher.Headers.GetValues("testKey4").Should().HaveCount(1).And.Contain("testValue4"); // Set by request option
        }

        [Fact]
        public async Task Requesting_With_Headers_Factory_Should_Set_And_Keep_Updated_Headers()
        {
            var watcher = new WatchingRequestHandler();
            var apiHeaders = new List<string> { "testKey2: testValue2.2" };
            var requestHeaders = new List<string> { "testKey3: testValue3.1", "testKey4: testValue4.1" };
            var testSettings = new TestSettings("testSettingsKey1: testSettingsValue1.1");
            var testStore = new TestSettings("testStoreKey2: testStoreValue2.1");

            // Json settings (loaded from embedded json settings files and bound to properties)
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{typeof(ApizrTests).Namespace}.appsettings.json");
            var configuration = new ConfigurationBuilder()
                .AddJsonStream(stream!)
                .Build();
            await stream.DisposeAsync();

            var apizrManager = ApizrBuilder.Current.CreateManagerFor<IReqResSimpleService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithConfiguration(configuration) // Whole configuration (auto mapped config)
                    //.WithConfiguration(configuration.GetSection("Apizr")) // Root section (auto mapped config)
                    //.WithConfiguration(configuration.GetSection("Apizr:Common")) // Specific section (manual mapped config)
                    //.WithConfiguration(configuration.GetSection("Apizr:IReqResSimpleService")) // Specific section (manual mapped config)
                    //.WithLogging()
                    //.WithBaseAddress("https://reqres.in/api")
                    .WithHeaders(() => apiHeaders, scope: ApizrLifetimeScope.Api)
                    .WithHeaders(() => requestHeaders, scope: ApizrLifetimeScope.Request)
                    .WithHeaders(testSettings, [settings => settings.TestJsonString], scope: ApizrLifetimeScope.Request)
                    .WithHeaders(["testKey5: testValue5.1", "testKey6: testValue6.1"])
                    .WithHeaders(["testStoreKey1: testStoreValue1.1", "testStoreKey3: testStoreValue3.1", "testSettingsKey4: testSettingsValue4.1", "testSettingsKey5: testSettingsValue5.1"], mode: ApizrRegistrationMode.Store)
                    .WithHeaders(testStore, [settings => settings.TestJsonString], scope: ApizrLifetimeScope.Request, mode: ApizrRegistrationMode.Store)
                    .WithDelegatingHandler(watcher));

            // Merge all
            await apizrManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt),
                options => options.WithHeaders(["testKey4: testValue4.2",
                    "testKey5: testValue5.2"]));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKeys("testKey1", "testKey2", "testKey3", "testKey4", "testKey5", "testKey6",
                    "testSettingsKey1", "testSettingsKey2", "testSettingsKey3", "testSettingsKey4", "testSettingsKey5",
                    "testSettingsKey6", "testStoreKey1", "testStoreKey2")
                .And.NotContainKey("testStoreKey3");
            watcher.Headers.GetValues("testKey1").Should().HaveCount(1).And.Contain("testValue1"); // Set by attribute
            watcher.Headers.GetValues("testKey2").Should().HaveCount(1).And.Contain("testValue2.2"); // Set by attribute then updated by common option within api scope factory
            watcher.Headers.GetValues("testKey3").Should().HaveCount(1).And.Contain("testValue3.1"); // Set by common option within request scope factory
            watcher.Headers.GetValues("testKey4").Should().HaveCount(1).And.Contain("testValue4.2"); // Set by common option within request scope factory then updated by request option
            watcher.Headers.GetValues("testKey5").Should().HaveCount(1).And.Contain("testValue5.2"); // Set by common option then updated by request option
            watcher.Headers.GetValues("testKey6").Should().HaveCount(1).And.Contain("testValue6.1"); // Set by common option
            watcher.Headers.GetValues("testSettingsKey1").Should().HaveCount(1).And.Contain("testSettingsValue1.1"); // Set by common option expression
            watcher.Headers.GetValues("testSettingsKey2").Should().HaveCount(1).And.Contain("testSettingsValue2.1"); // Set by common option configuration
            watcher.Headers.GetValues("testSettingsKey3").Should().HaveCount(1).And.Contain("testSettingsValue3.1"); // Set by common option configuration
            watcher.Headers.GetValues("testSettingsKey4").Should().HaveCount(1).And.Contain("testSettingsValue4.1"); // Set by common option configuration
            watcher.Headers.GetValues("testSettingsKey5").Should().HaveCount(1).And.Contain("testSettingsValue5.1"); // Set by common option configuration
            watcher.Headers.GetValues("testSettingsKey6").Should().HaveCount(1).And.Contain("testSettingsValue6.1"); // Set by common option configuration
            watcher.Headers.GetValues("testStoreKey1").Should().HaveCount(1).And.Contain("testStoreValue1.1"); // Set by common option from Store
            watcher.Headers.GetValues("testStoreKey2").Should().HaveCount(1).And.Contain("testStoreValue2.1"); // Set by common option from Store

            // Keep updated
            apiHeaders[0] = "testKey2: testValue2.3"; // will not be updated (scope: Api)
            requestHeaders[0] = "testKey3: testValue3.2"; // will be updated (scope: Request)
            requestHeaders[1] = "testKey4: testValue4.3"; // should be updated (scope: Request) but updated then by request option
            testSettings.TestJsonString = "testSettingsKey1: testSettingsValue1.2";
            testStore.TestJsonString = "testStoreKey2: testStoreValue2.2";

            await apizrManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt),
                options => options.WithHeaders(["testKey4: testValue4.4", 
                    "testKey5: testValue5.3",
                    "testStoreKey1: *testStoreValue1.2*",
                    "testStoreKey3: *{0}*"]));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKeys("testKey1", "testKey2", "testKey3", "testKey4", "testKey5", "testKey6",
                "testSettingsKey1", "testSettingsKey2", "testSettingsKey3", "testSettingsKey4", "testSettingsKey5",
                "testSettingsKey6", "testStoreKey1", "testStoreKey2", "testStoreKey3");
            watcher.Headers.GetValues("testKey1").Should().HaveCount(1).And.Contain("testValue1"); // Same as previous value
            watcher.Headers.GetValues("testKey2").Should().HaveCount(1).And.Contain("testValue2.2"); // Same as previous value
            watcher.Headers.GetValues("testKey3").Should().HaveCount(1).And.Contain("testValue3.2"); // Updated at request time (scope: Request)
            watcher.Headers.GetValues("testKey4").Should().HaveCount(1).And.Contain("testValue4.4"); // Updated at request time (scope: Request) then by request option
            watcher.Headers.GetValues("testKey5").Should().HaveCount(1).And.Contain("testValue5.3"); // Updated by request option
            watcher.Headers.GetValues("testKey6").Should().HaveCount(1).And.Contain("testValue6.1"); // Same as previous value
            watcher.Headers.GetValues("testSettingsKey1").Should().HaveCount(1).And.Contain("testSettingsValue1.2"); // Updated at request time (scope: Request)
            watcher.Headers.GetValues("testSettingsKey2").Should().HaveCount(1).And.Contain("testSettingsValue2.1"); // Same as previous value
            watcher.Headers.GetValues("testSettingsKey3").Should().HaveCount(1).And.Contain("testSettingsValue3.1"); // Same as previous value
            watcher.Headers.GetValues("testSettingsKey4").Should().HaveCount(1).And.Contain("testSettingsValue4.1"); // Same as previous value
            watcher.Headers.GetValues("testSettingsKey5").Should().HaveCount(1).And.Contain("testSettingsValue5.1"); // Same as previous value
            watcher.Headers.GetValues("testSettingsKey6").Should().HaveCount(1).And.Contain("testSettingsValue6.1"); // Same as previous value
            watcher.Headers.GetValues("testStoreKey1").Should().HaveCount(1).And.Contain("testStoreValue1.2"); // Updated by request option
            watcher.Headers.GetValues("testStoreKey2").Should().HaveCount(1).And.Contain("testStoreValue2.2"); // Updated at request time (scope: Request)
            watcher.Headers.GetValues("testStoreKey3").Should().HaveCount(1).And.Contain("testStoreValue3.1"); // Set by request option from Store
        }

        [Fact]
        public async Task Sending_A_Get_Request_With_Both_Attribute_And_Fluent_Headers_Should_Set_Merged_Headers()
        {
            var watcher = new WatchingRequestHandler();

            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResSimpleService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithHeaders(["testKey2: testValue2"])
                    .WithDelegatingHandler(watcher));

            await reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt),
                options => options.WithHeaders(["testKey3: testValue3", "testKey4: testValue4"]));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKeys("testKey1", "testKey2", "testKey3", "testKey4");
        }

        [Fact]
        public async Task Sending_A_Post_Request_With_Both_Attribute_And_Fluent_Headers_Should_Set_Merged_Headers()
        {
            var watcher = new WatchingRequestHandler();

            var manager = ApizrBuilder.Current.CreateManagerFor<IHttpBinService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithHeaders(["testKey2: testValue2"])
                    .WithDelegatingHandler(watcher));

            var streamPart = FileHelper.GetTestFileStreamPart("medium");

            await manager.ExecuteAsync((opt, api) => api.UploadAsync(streamPart, opt),
                options => options.WithHeaders(["testKey3: testValue3", "testKey4: testValue4"]));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKeys("testKey1", "testKey2", "testKey3", "testKey4");
        }

        [Fact]
        public async Task Concurrent_Requests_Should_Not_Throw()
        {
            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResSimpleService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging());

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
            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging());

            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(3));

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(5, opt),
                    options => options.WithCancellation(cts.Token));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<OperationCanceledException>();
        }

        [Fact]
        public async Task Cancelling_A_Post_Request_Should_Throw_An_OperationCanceledException()
        {
            var manager = ApizrBuilder.Current.CreateManagerFor<IHttpBinService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging());

            var streamPart = FileHelper.GetTestFileStreamPart("medium");
            var ct = new CancellationTokenSource();
            ct.CancelAfter(TimeSpan.FromSeconds(1));

            Func<Task> act = () => manager.ExecuteAsync((opt, api) => api.UploadAsync(streamPart, opt),
                options => options.WithCancellation(ct.Token));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<OperationCanceledException>();
        }

        [Fact]
        public async Task Calling_ConfigureClient_Should_Configure_HttpClient()
        {
            var watcher = new WatchingRequestHandler();

            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResSimpleService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .ConfigureHttpClient(client =>
                        client.DefaultRequestHeaders.Add("HttpClientHeaderKey", "HttpClientHeaderValue"))
                    .WithDelegatingHandler(watcher));

            await reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKey("HttpClientHeaderKey");
        }

        [Fact]
        public async Task When_Calling_BA_WithOperationTimeout_Then_Request_Should_Throw_A_TimeoutException()
        {
            var reqResManager =
                ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(
                    options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                            builder.AddXUnit(_outputHelper)
                                .SetMinimumLevel(LogLevel.Trace)))
                        .WithLogging()
                        .WithOperationTimeout(TimeSpan.FromSeconds(4)));

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(6, opt),
                    options => options.WithOperationTimeout(TimeSpan.FromSeconds(2)));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutRejectedException>();
        }

        [Fact]
        public async Task When_Calling_AB_WithOperationTimeout_Then_Client_Should_Throw_A_TimeoutException()
        {
            var reqResManager =
                ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                    options.WithLoggerFactory(LoggerFactory.Create(builder =>
                            builder.AddXUnit(_outputHelper)
                                .SetMinimumLevel(LogLevel.Trace)))
                        .WithLogging()
                        .WithOperationTimeout(TimeSpan.FromSeconds(2)));

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(6, opt),
                    options => options.WithOperationTimeout(TimeSpan.FromSeconds(4)));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutRejectedException>();
        }

        [Fact]
        public async Task When_Calling_BA_WithRequestTimeout_Then_Request_Should_Throw_A_TimeoutException()
        {
            var reqResManager =
                ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                    options.WithLoggerFactory(LoggerFactory.Create(builder =>
                            builder.AddXUnit(_outputHelper)
                                .SetMinimumLevel(LogLevel.Trace)))
                        .WithLogging());
                    //options => options.WithRequestTimeout(TimeSpan.FromSeconds(4)));

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(6, opt),
                    options => options.WithRequestTimeout(TimeSpan.FromSeconds(2)));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutRejectedException>();
        }

        [Fact]
        public async Task When_Calling_AB_WithRequestTimeout_Then_Client_Should_Throw_A_TimeoutException()
        {
            var reqResManager =
                ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                    options.WithLoggerFactory(LoggerFactory.Create(builder =>
                            builder.AddXUnit(_outputHelper)
                                .SetMinimumLevel(LogLevel.Trace)))
                        .WithLogging()
                        .WithRequestTimeout(TimeSpan.FromSeconds(2)));

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(6, opt),
                    options => options.WithRequestTimeout(TimeSpan.FromSeconds(4)));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutRejectedException>();
        }

        [Fact]
        public async Task When_Calling_DCBA_WithOperationTimeout_And_WithRequestTimeout_Then_Request_Should_Throw_A_TimeoutException()
        {
            var reqResManager =
                ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(
                    options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                            builder.AddXUnit(_outputHelper)
                                .SetMinimumLevel(LogLevel.Trace)))
                        .WithLogging()
                        .WithOperationTimeout(TimeSpan.FromSeconds(8))
                        .WithRequestTimeout(TimeSpan.FromSeconds(6)));

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
            var reqResManager =
                ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                    options.WithLoggerFactory(LoggerFactory.Create(builder =>
                            builder.AddXUnit(_outputHelper)
                                .SetMinimumLevel(LogLevel.Trace)))
                        .WithLogging()
                        .WithOperationTimeout(TimeSpan.FromSeconds(2))
                        .WithRequestTimeout(TimeSpan.FromSeconds(4)));

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
            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithOperationTimeout(TimeSpan.FromSeconds(4)));

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
            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithOperationTimeout(TimeSpan.FromSeconds(2)));

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
            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                        builder.AddXUnit(_outputHelper)
                            .SetMinimumLevel(LogLevel.Trace)))
                    .WithLogging()
                    .WithOperationTimeout(TimeSpan.FromSeconds(4)));

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
                            retryCount = args.AttemptNumber + 1;
                            return default;
                        }
                    }));

            var watcher = new WatchingRequestHandler();

            var reqResManager =
                ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(
                    options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                            builder.AddXUnit(_outputHelper)
                                .SetMinimumLevel(LogLevel.Trace)))
                        .WithLogging()
                        .WithResilienceContextOptions(opt =>
                            opt.ReturnToPoolOnComplete(false))
                        .WithResiliencePipelineRegistry(resiliencePipelineRegistry)
                        .WithRequestTimeout(TimeSpan.FromSeconds(3))
                        .WithDelegatingHandler(watcher));

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
                            retryCount = args.AttemptNumber + 1;
                            return default;
                        }
                    }));

            var watcher = new WatchingRequestHandler();

            var reqResManager =
                ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(
                    options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                            builder.AddXUnit(_outputHelper)
                                .SetMinimumLevel(LogLevel.Trace)))
                        .WithLogging()
                        .WithResilienceContextOptions(opt =>
                            opt.ReturnToPoolOnComplete(false))
                        .WithResiliencePipelineRegistry(resiliencePipelineRegistry)
                        .WithOperationTimeout(TimeSpan.FromSeconds(10))
                        .WithDelegatingHandler(watcher));

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(6, opt),
                    options => options.WithRequestTimeout(TimeSpan.FromSeconds(3)));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutRejectedException>();

            // retry attempts should be equal to 2 as request timed out before the 3rd retry
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
                            retryCount = args.AttemptNumber+1;
                            return default;
                        }
                    }));

            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5));

            var watcher = new WatchingRequestHandler();

            var reqResManager =
                ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(
                    options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                            builder.AddXUnit(_outputHelper)
                                .SetMinimumLevel(LogLevel.Trace)))
                        .WithLogging()
                        .WithResilienceContextOptions(opt =>
                            opt.ReturnToPoolOnComplete(false))
                        .WithResiliencePipelineRegistry(resiliencePipelineRegistry)
                        .WithOperationTimeout(TimeSpan.FromSeconds(10))
                        .WithDelegatingHandler(watcher));

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(6, opt),
                    options => options.WithRequestTimeout(TimeSpan.FromSeconds(3))
                        .WithCancellation(cts.Token));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<OperationCanceledException>();

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
                            retryCount = args.AttemptNumber+1;
                            return default;
                        }
                    }));

            var testHandler = new TestRequestHandler();

            var reqResManager =
                ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(
                    options => options.WithLoggerFactory(LoggerFactory.Create(builder =>
                            builder.AddXUnit(_outputHelper)
                                .SetMinimumLevel(LogLevel.Trace)))
                        .WithLogging()
                        .WithResilienceContextOptions(opt =>
                            opt.ReturnToPoolOnComplete(false))
                        .WithResiliencePipelineRegistry(resiliencePipelineRegistry)
                        .WithDelegatingHandler(testHandler)
                        .WithOperationTimeout(TimeSpan.FromSeconds(3)));

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
        public async Task Inherited_Apis_Should_Merge_Attributes()
        {
            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResChildApi>(options =>
                options.WithLoggerFactory(LoggerFactory.Create(builder =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))));

            var result = await reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt));

            result.Should().NotBeNull();
        }
    }
}
