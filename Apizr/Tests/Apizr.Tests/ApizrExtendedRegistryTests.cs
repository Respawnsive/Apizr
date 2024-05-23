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
using Apizr.Configuring;
using Apizr.Configuring.Manager;
using Apizr.Extending;
using Apizr.Extending.Configuring.Registry;
using Apizr.Logging;
using Apizr.Mediation.Extending;
using Apizr.Mediation.Requesting.Sending;
using Apizr.Optional.Extending;
using Apizr.Optional.Requesting.Sending;
using Apizr.Progressing;
using Apizr.Requesting;
using Apizr.Tests.Apis;
using Apizr.Tests.Helpers;
using Apizr.Tests.Models;
using Apizr.Tests.Settings;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;
using AutoMapper.Internal;
using FluentAssertions;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonkeyCache.FileStore;
using Polly;
using Polly.Retry;
using Polly.Timeout;
using Refit;
using RichardSzalay.MockHttp;
using Xunit;
using Xunit.Abstractions;
using IHttpBinService = Apizr.Tests.Apis.IHttpBinService;

namespace Apizr.Tests
{
    public class ApizrExtendedRegistryTests
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly ResiliencePipelineBuilder<HttpResponseMessage> _resiliencePipelineBuilder;
        private readonly RefitSettings _refitSettings;
        private readonly Assembly _assembly;

        public ApizrExtendedRegistryTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _resiliencePipelineBuilder = new ResiliencePipelineBuilder<HttpResponseMessage>()
                .AddRetry(
                    new RetryStrategyOptions<HttpResponseMessage>
                    {
                        ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                            .Handle<HttpRequestException>()
                            .HandleResult(response =>
                                response.StatusCode is >= HttpStatusCode.InternalServerError
                                    or HttpStatusCode.RequestTimeout),
                        Delay = TimeSpan.FromSeconds(1),
                        MaxRetryAttempts = 3,
                        UseJitter = true,
                        BackoffType = DelayBackoffType.Exponential
                    });

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
            
            services.Should().Contain(x => x.ServiceType == typeof(IApizrManager<IReqResUserService>))
                .And.Contain(x => x.ServiceType == typeof(IApizrManager<IHttpBinService>))

                .And.Contain(x => x.ServiceType == typeof(IApizrManager<ICrudApi<User, int, PagedResult<User>, IDictionary<string, object>>>))

                .And.Contain(x => x.ServiceType == typeof(IApizrUploadManager<IUploadApi>))
                .And.Contain(x => x.ServiceType == typeof(IApizrUploadManager<ITransferSampleApi>))
                .And.Contain(x => x.ServiceType == typeof(IApizrUploadManager<IUploadApi<string>, string>))
                .And.Contain(x => x.ServiceType == typeof(IApizrUploadManagerWith<string>))

                .And.Contain(x => x.ServiceType == typeof(IApizrDownloadManager<IDownloadApi>))
                .And.Contain(x => x.ServiceType == typeof(IApizrDownloadManager<ITransferSampleApi>))
                .And.Contain(x => x.ServiceType == typeof(IApizrDownloadManager<IDownloadApi<User>, User>))
                .And.Contain(x => x.ServiceType == typeof(IApizrDownloadManagerWith<User>))

                .And.Contain(x => x.ServiceType == typeof(IApizrTransferManager<ITransferApi>))
                .And.Contain(x => x.ServiceType == typeof(IApizrTransferManager<ITransferSampleApi>))
                .And.Contain(x => x.ServiceType == typeof(IApizrTransferManager<ITransferApi<User, string>, User, string>))
                .And.Contain(x => x.ServiceType == typeof(IApizrTransferManagerWith<User, string>));


            var serviceProvider = services.BuildServiceProvider();
            var registry = serviceProvider.GetService<IApizrExtendedRegistry>();
            registry.Should().NotBeNull();

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
        public void ServiceProvider_Should_Resolve_Registry_And_Managers()
        {
            var services = new ServiceCollection();
            services.AddApizr(registry => registry
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

            var serviceProvider = services.BuildServiceProvider();
            var registry = serviceProvider.GetService<IApizrExtendedRegistry>();
            
            registry.Should().NotBeNull();

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
        public void ServiceProvider_Should_Resolve_Registry_And_Scanned_Managers()
        {
            var services = new ServiceCollection();
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
            services.AddApizr(registry => registry
                .AddManagerFor<IReqResUserService>());

            var serviceProvider = services.BuildServiceProvider();
            var fixture = serviceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            fixture.Options.BaseUri.Should().Be(attributeUri);

            // By proper option overriding attribute
            services = new ServiceCollection();
            services.AddApizr(registry => registry
                .AddManagerFor<IReqResUserService>(options => options.WithBaseAddress(uri1)));

            serviceProvider = services.BuildServiceProvider();
            fixture = serviceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            fixture.Options.BaseUri.Should().Be(uri1);

            // By attribute overriding common option
            services = new ServiceCollection();
            services.AddApizr(registry => registry
                    .AddManagerFor<IReqResUserService>(),
                config => config.WithBaseAddress(uri1));

            serviceProvider = services.BuildServiceProvider();
            fixture = serviceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            fixture.Options.BaseUri.Should().Be(attributeUri);

            // By proper option overriding proper option and attribute
            services = new ServiceCollection();
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

            // By attribute option overriding common options
            services.AddApizr(registry => registry
                    .AddGroup(group => group
                            .AddManagerFor<IReqResUserService>()
                            .AddManagerFor<IReqResUserPathService>() // completing with base path by attribute
                            .AddManagerFor<IReqResResourceService>(config => config.WithBasePath(resPath)), // completing with base path by proper option
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
            resourceFixture.Options.BaseUri.Should().Be(fullResUri);

            // By proper option overriding all common options and attribute
            services = new ServiceCollection();
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
            resourceFixture.Options.BaseUri.Should().Be(attributeUri);

            var resourceAddressFixture = apizrRegistry.GetManagerFor<IReqResResourceAddressService>();
            resourceAddressFixture.Options.BaseUri.Should().Be(fullResUri);
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

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(
                        registry => registry
                            .AddManagerFor<IReqResSimpleService>(),
                        config => config
                            .WithLogging()
                            .WithDelegatingHandler(watcher)
                            .WithHttpMessageHandler(mockHttp));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResSimpleService>>();

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

            Task<string> OnRefreshToken(HttpRequestMessage _) => Task.FromResult(token = "token");

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                        .AddManagerFor<IHttpBinService>(options => options
                            .WithLogging()
                            .WithAuthenticationHandler(OnRefreshToken)));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var httpBinManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IHttpBinService>>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
            token.Should().Be("token");
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_With_Default_Token_Should_Authenticate_Request()
        {
            var testSettings = new TestSettings("token");

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton(testSettings);

                    services.AddApizr(registry => registry
                        .AddManagerFor<IHttpBinService>(options => options
                            .WithLogging()
                            .WithAuthenticationHandler<TestSettings>(settings => settings.TestJsonString)));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var httpBinManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IHttpBinService>>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public void Calling_WithLogging_Should_Set_LoggingSettings()
        {
            var services = new ServiceCollection();
            services.AddLogging(builder => builder.AddXUnit(_outputHelper).SetMinimumLevel(LogLevel.Trace));
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

            Task<string> OnRefreshToken(HttpRequestMessage _) => Task.FromResult(token = "token");

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(
                        registry => registry
                            .AddManagerFor<IHttpBinService>(),
                        config => config
                            .WithLogging()
                            .WithAuthenticationHandler(OnRefreshToken));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var httpBinManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IHttpBinService>>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
            token.Should().Be("token");
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_Should_Authenticate_Request()
        {
            string token = null;

            Task<string> OnRefreshTokenA(HttpRequestMessage _) => Task.FromResult(token = "tokenA");
            Task<string> OnRefreshTokenB(HttpRequestMessage _) => Task.FromResult(token = "tokenB");

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(
                        registry => registry
                            .AddManagerFor<IHttpBinService>(options => options
                                .WithAuthenticationHandler(OnRefreshTokenA)),
                        config => config
                            .WithLogging()
                            .WithAuthenticationHandler(OnRefreshTokenB));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var httpBinManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IHttpBinService>>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
            token.Should().Be("tokenA");
        }

        [Fact]
        public async Task Calling_WithAkavacheCacheHandler_Should_Cache_Result()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(
                        registry => registry
                            .AddManagerFor<IReqResUserService>(),
                        config => config
                            .WithLogging()
                            .WithAkavacheCacheHandler()
                            .WithDelegatingHandler(new TestRequestHandler()));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

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
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddMemoryCache();

                    services.AddApizr(
                        registry => registry
                            .AddManagerFor<IReqResUserService>(),
                        config => config
                            .WithLogging()
                            .WithInMemoryCacheHandler()
                            .WithDelegatingHandler(new TestRequestHandler()));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            // Defining a throwing request
            Func<bool, Action<Exception>, Task<ApiResult<User>>> act = (clearCache, onException) => reqResManager.ExecuteAsync(api => api.GetUsersAsync(HttpStatusCode.BadRequest), options => options.WithCacheClearing(clearCache).WithExCatching(onException));

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
            var maxRetryAttempts = 3;
            var retryCount = 0;

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddMemoryCache();

                    services.AddApizr(
                        registry => registry
                            .AddManagerFor<IReqResUserService>(),
                        config => config
                            .WithLogging()
                            .WithDelegatingHandler(new TestRequestHandler()));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddRetry(
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
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

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

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(
                        registry => registry
                            .AddManagerFor<IReqResUserService>(),
                        config => config
                            .WithLogging()
                            .WithConnectivityHandler(() => isConnected));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

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
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddAutoMapper(_assembly);

                    services.AddApizr(
                        registry => registry
                            .AddManagerFor<IReqResUserService>(),
                        config => config
                            .WithLogging()
                            .WithRefitSettings(_refitSettings)
                            .WithAutoMapperMappingHandler());

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            var minUser = new MinUser { Name = "John" };

            // This one should succeed
            var result = await reqResManager.ExecuteAsync<MinUser, User>((api, user) => api.CreateUser(user, CancellationToken.None), minUser);

            result.Should().NotBeNull();
            result.Name.Should().Be(minUser.Name);
            result.Id.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task Calling_WithMappingHandler_With_AutoMapper_Should_Map_Data()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddAutoMapper(_assembly);

                    services.AddApizr(
                        registry => registry
                            .AddManagerFor<IReqResUserService>(),
                        config => config
                            .WithLogging()
                            .WithRefitSettings(_refitSettings)
                            .WithMappingHandler<AutoMapperMappingHandler>());

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            var minUser = new MinUser { Name = "John" };

            // This one should succeed
            var result = await reqResManager.ExecuteAsync<MinUser, User>((api, user) => api.CreateUser(user, CancellationToken.None), minUser);

            result.Should().NotBeNull();
            result.Name.Should().Be(minUser.Name);
            result.Id.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task Calling_WithMapsterMappingHandler_Should_Map_Data()
        {
            var mapsterConfig = new TypeAdapterConfig();
            mapsterConfig.NewConfig<User, MinUser>()
                .TwoWays()
                .Map(minUser => minUser.Name, user => user.FirstName);

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton(mapsterConfig);
                    services.AddSingleton<IMapper, ServiceMapper>();

                    services.AddApizr(
                        registry => registry
                            .AddManagerFor<IReqResUserService>(),
                        config => config
                            .WithLogging()
                            .WithRefitSettings(_refitSettings)
                            .WithMapsterMappingHandler());

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            var minUser = new MinUser { Name = "John" };

            // This one should succeed
            var result = await reqResManager.ExecuteAsync<MinUser, User>((api, user) => api.CreateUser(user, CancellationToken.None), minUser);

            result.Should().NotBeNull();
            result.Name.Should().Be(minUser.Name);
            result.Id.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task Calling_WithMappingHandler_With_Mapster_Should_Map_Data()
        {
            var mapsterConfig = new TypeAdapterConfig();
            mapsterConfig.NewConfig<User, MinUser>()
                .TwoWays()
                .Map(minUser => minUser.Name, user => user.FirstName);

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton(mapsterConfig);
                    services.AddSingleton<IMapper, ServiceMapper>();

                    services.AddApizr(
                        registry => registry
                            .AddManagerFor<IReqResUserService>(),
                        config => config
                            .WithLogging()
                            .WithRefitSettings(_refitSettings)
                            .WithMappingHandler<MapsterMappingHandler>());

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            var minUser = new MinUser { Name = "John" };

            // This one should succeed
            var result = await reqResManager.ExecuteAsync<MinUser, User>((api, user) => api.CreateUser(user, CancellationToken.None), minUser);

            result.Should().NotBeNull();
            result.Name.Should().Be(minUser.Name);
            result.Id.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task Requesting_With_Context_into_Options_Should_Set_Context()
        {
            var watcher = new WatchingRequestHandler();

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(
                        registry => registry
                            .AddManagerFor<IReqResSimpleService>(options => options
                                .WithLogging()
                                .WithResilienceContextOptions(opt =>
                                    opt.ReturnToPoolOnComplete(false))
                                .WithDelegatingHandler(watcher)));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            // Get instances from the container
            var apizrManager = scope.ServiceProvider.GetService<IApizrManager<IReqResSimpleService>>();
            apizrManager.Should().NotBeNull();

            ResiliencePropertyKey<int> testKey = new("TestKey1");
            var testValue = 1;

            await apizrManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt), options => options.WithResilienceProperty(testKey, testValue));
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

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry =>
                            registry.AddGroup(group => group.AddManagerFor<IReqResSimpleService>(
                                    // proper
                                    options => options.WithResilienceProperty(testKey3, _ => "testValue3.2")
                                        .WithResilienceProperty(testKey4, _ => "testValue4.1")
                                        .WithDelegatingHandler(watcher)),
                                // group
                                options => options.WithResilienceProperty(testKey2, _ => "testValue2.2")
                                    .WithResilienceProperty(testKey3, _ => "testValue3.1")),
                        // common
                        options => options
                            .WithLogging()
                            .WithResilienceContextOptions(opt =>
                                opt.ReturnToPoolOnComplete(false))
                            .WithResilienceProperty(testKey1, _ => "testValue1")
                            .WithResilienceProperty(testKey2, _ => "testValue2.1"));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            // Get instances from the container
            var apizrManager = scope.ServiceProvider.GetService<IApizrManager<IReqResSimpleService>>();
            apizrManager.Should().NotBeNull();

            await apizrManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt),
                // request
                options => options.WithResilienceProperty(testKey4, "testValue4.2")
                    .WithResilienceProperty(testKey5, "testValue5"));

            watcher.Context.Should().NotBeNull();
            watcher.Context.Properties.TryGetValue(testKey1, out var valueKey1).Should().BeTrue(); // Set by common option
            valueKey1.Should().Be("testValue1");
            watcher.Context.Properties.TryGetValue(testKey2, out var valueKey2).Should().BeTrue(); // Set by common option then updated by the group one
            valueKey2.Should().Be("testValue2.2");
            watcher.Context.Properties.TryGetValue(testKey3, out var valueKey3).Should().BeTrue(); // Set by group option then updated by the proper one
            valueKey3.Should().Be("testValue3.2");
            watcher.Context.Properties.TryGetValue(testKey4, out var valueKey4).Should().BeTrue(); // Set by proper option then updated by the request one
            valueKey4.Should().Be("testValue4.2");
            watcher.Context.Properties.TryGetValue(testKey5, out var valueKey5).Should().BeTrue(); // Set by request option
            valueKey5.Should().Be("testValue5");
        }

        [Fact]
        public async Task Calling_WithMediation_Should_Handle_Requests()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

                    services.AddApizr(
                        registry => registry
                            .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>()
                            .AddCrudManagerFor<UserInfos, int, PagedResult<UserInfos>, IDictionary<string, object>>()
                            .AddManagerFor<IReqResUserService>()
                            .AddManagerFor<IReqResResourceService>(),
                        config => config
                            .WithLogging()
                            .WithMediation());

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResMediator = scope.ServiceProvider.GetRequiredService<IApizrMediator<IReqResUserService>>();

            reqResMediator.Should().NotBeNull();
            var result = await reqResMediator.SendFor(api => api.GetUsersAsync());
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Count.Should().BeGreaterOrEqualTo(1);
        }

        [Fact]
        public async Task Calling_WithOptionalMediation_Should_Handle_Requests_With_Optional_Result()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

                    services.AddApizr(
                        registry => registry
                            .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>()
                            .AddCrudManagerFor<UserInfos, int, PagedResult<UserInfos>, IDictionary<string, object>>()
                            .AddManagerFor<IReqResUserService>()
                            .AddManagerFor<IReqResResourceService>(),
                        config => config
                            .WithLogging()
                            .WithOptionalMediation());

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResMediator = scope.ServiceProvider.GetRequiredService<IApizrOptionalMediator<IReqResUserService>>();

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
            resourceFixture.Options.BaseUri.Should().Be(attributeUri);

            // Test 2
            services = new ServiceCollection();
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
            resourceFixture.Options.BaseUri.Should().Be(attributeUri);
        }

        [Fact]
        public async Task Downloading_File_Should_Succeed()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                            .AddTransferManager()
                            .AddTransferManagerFor<ITransferUndefinedApi>()
                            .AddDownloadManager()
                            .AddDownloadManagerFor<ITransferUndefinedApi>(),
                        options => options
                            .WithLogging()
                            .WithBaseAddress("http://speedtest.ftp.otenet.gr/files"));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            // Get instances from the container
            var apizrTransferManager = scope.ServiceProvider.GetService<IApizrTransferManager>(); // Built-in
            var apizrTransferTypedManager = scope.ServiceProvider.GetService<IApizrTransferManager<ITransferApi>>(); // Built-in
            var apizrCustomTransferManager = scope.ServiceProvider.GetService<IApizrTransferManager<ITransferUndefinedApi>>(); // Custom
            var apizrDownloadManager = scope.ServiceProvider.GetService<IApizrDownloadManager>(); // Built-in
            var apizrDownloadTypedManager = scope.ServiceProvider.GetService<IApizrDownloadManager<IDownloadApi>>(); // Built-in
            var apizrCustomDownloadManager = scope.ServiceProvider.GetService<IApizrDownloadManager<ITransferUndefinedApi>>(); // Custom

            apizrTransferManager.Should().NotBeNull(); // Built-in
            apizrTransferTypedManager.Should().NotBeNull(); // Built-in
            apizrCustomTransferManager.Should().NotBeNull(); // Custom
            apizrDownloadManager.Should().NotBeNull(); // Built-in
            apizrDownloadTypedManager.Should().NotBeNull(); // Built-in
            apizrCustomDownloadManager.Should().NotBeNull(); // Custom

            // Transfer
            // Built-in
            var apizrTransferManagerResult = await apizrTransferManager.DownloadAsync(new FileInfo("test100k.db"));
            apizrTransferManagerResult.Should().NotBeNull();
            apizrTransferManagerResult.Length.Should().BePositive();

            // Built-in
            var apizrTransferTypedManagerResult = await apizrTransferTypedManager.DownloadAsync(new FileInfo("test100k.db"));
            apizrTransferTypedManagerResult.Should().NotBeNull();
            apizrTransferTypedManagerResult.Length.Should().BePositive();

            // Custom
            var apizrCustomTransferManagerResult = await apizrCustomTransferManager.DownloadAsync(new FileInfo("test100k.db"));
            apizrCustomTransferManagerResult.Should().NotBeNull();
            apizrCustomTransferManagerResult.Length.Should().BePositive();

            // Download
            // Built-in
            var apizrDownloadManagerResult = await apizrDownloadManager.DownloadAsync(new FileInfo("test100k.db"));
            apizrDownloadManagerResult.Should().NotBeNull();
            apizrDownloadManagerResult.Length.Should().BePositive();

            // Built-in
            var apizrDownloadTypedManagerResult = await apizrDownloadTypedManager.DownloadAsync(new FileInfo("test100k.db"));
            apizrDownloadTypedManagerResult.Should().NotBeNull();
            apizrDownloadTypedManagerResult.Length.Should().BePositive();

            // Custom
            var apizrCustomDownloadManagerResult = await apizrCustomDownloadManager.DownloadAsync(new FileInfo("test100k.db"));
            apizrCustomDownloadManagerResult.Should().NotBeNull();
            apizrCustomDownloadManagerResult.Length.Should().BePositive();

            // Get instances from the registry
            var registry = scope.ServiceProvider.GetRequiredService<IApizrExtendedRegistry>();

            registry.TryGetTransferManager(out var regTransferManager).Should().BeTrue(); // Built-in
            registry.TryGetTransferManagerFor<ITransferApi>(out var regTransferTypedManager).Should().BeTrue(); // Built-in
            registry.TryGetTransferManagerFor<ITransferUndefinedApi>(out var regCustomTransferTypedManager).Should().BeTrue(); // Custom
            registry.TryGetDownloadManager(out var regDownloadManager).Should().BeTrue(); // Built-in
            registry.TryGetDownloadManagerFor<IDownloadApi>(out var regDownloadTypedManager).Should().BeTrue(); // Built-in
            registry.TryGetDownloadManagerFor<ITransferUndefinedApi>(out var regCustomDownloadTypedManager).Should().BeTrue(); // Custom

            regTransferManager.Should().NotBeNull(); // Built-in
            regTransferTypedManager.Should().NotBeNull(); // Built-in
            regCustomTransferTypedManager.Should().NotBeNull(); // Custom
            regDownloadManager.Should().NotBeNull(); // Built-in
            regDownloadTypedManager.Should().NotBeNull(); // Built-in
            regCustomDownloadTypedManager.Should().NotBeNull(); // Custom

            // Shortcut
            var regShortcutResult = await registry.DownloadAsync(new FileInfo("test100k.db"));
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
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                            .AddGroup(group => group
                                    .AddTransferManager()
                                    .AddTransferManagerFor<ITransferUndefinedApi>()
                                    .AddDownloadManager()
                                    .AddDownloadManagerFor<ITransferUndefinedApi>(),
                                options => options.WithBasePath("/files")),
                        options => options
                            .WithLogging()
                            .WithBaseAddress("http://speedtest.ftp.otenet.gr"));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            // Get instances from the container
            var apizrTransferManager = scope.ServiceProvider.GetService<IApizrTransferManager>(); // Built-in
            var apizrTransferTypedManager = scope.ServiceProvider.GetService<IApizrTransferManager<ITransferApi>>(); // Built-in
            var apizrCustomTransferManager = scope.ServiceProvider.GetService<IApizrTransferManager<ITransferUndefinedApi>>(); // Custom
            var apizrDownloadManager = scope.ServiceProvider.GetService<IApizrDownloadManager>(); // Built-in
            var apizrDownloadTypedManager = scope.ServiceProvider.GetService<IApizrDownloadManager<IDownloadApi>>(); // Built-in
            var apizrCustomDownloadManager = scope.ServiceProvider.GetService<IApizrDownloadManager<ITransferUndefinedApi>>(); // Custom

            apizrTransferManager.Should().NotBeNull(); // Built-in
            apizrTransferTypedManager.Should().NotBeNull(); // Built-in
            apizrCustomTransferManager.Should().NotBeNull(); // Custom
            apizrDownloadManager.Should().NotBeNull(); // Built-in
            apizrDownloadTypedManager.Should().NotBeNull(); // Built-in
            apizrCustomDownloadManager.Should().NotBeNull(); // Custom

            // Transfer
            // Built-in
            var apizrTransferManagerResult = await apizrTransferManager.DownloadAsync(new FileInfo("test100k.db"));
            apizrTransferManagerResult.Should().NotBeNull();
            apizrTransferManagerResult.Length.Should().BePositive();

            // Built-in
            var apizrTransferTypedManagerResult = await apizrTransferTypedManager.DownloadAsync(new FileInfo("test100k.db"));
            apizrTransferTypedManagerResult.Should().NotBeNull();
            apizrTransferTypedManagerResult.Length.Should().BePositive();

            // Custom
            var apizrCustomTransferManagerResult = await apizrCustomTransferManager.DownloadAsync(new FileInfo("test100k.db"));
            apizrCustomTransferManagerResult.Should().NotBeNull();
            apizrCustomTransferManagerResult.Length.Should().BePositive();

            // Download
            // Built-in
            var apizrDownloadManagerResult = await apizrDownloadManager.DownloadAsync(new FileInfo("test100k.db"));
            apizrDownloadManagerResult.Should().NotBeNull();
            apizrDownloadManagerResult.Length.Should().BePositive();

            // Built-in
            var apizrDownloadTypedManagerResult = await apizrDownloadTypedManager.DownloadAsync(new FileInfo("test100k.db"));
            apizrDownloadTypedManagerResult.Should().NotBeNull();
            apizrDownloadTypedManagerResult.Length.Should().BePositive();

            // Custom
            var apizrCustomDownloadManagerResult = await apizrCustomDownloadManager.DownloadAsync(new FileInfo("test100k.db"));
            apizrCustomDownloadManagerResult.Should().NotBeNull();
            apizrCustomDownloadManagerResult.Length.Should().BePositive();

            // Get instances from the registry
            var registry = scope.ServiceProvider.GetRequiredService<IApizrExtendedRegistry>();

            registry.TryGetTransferManager(out var regTransferManager).Should().BeTrue(); // Built-in
            registry.TryGetTransferManagerFor<ITransferApi>(out var regTransferTypedManager).Should().BeTrue(); // Built-in
            registry.TryGetTransferManagerFor<ITransferUndefinedApi>(out var regCustomTransferTypedManager).Should().BeTrue(); // Custom
            registry.TryGetDownloadManager(out var regDownloadManager).Should().BeTrue(); // Built-in
            registry.TryGetDownloadManagerFor<IDownloadApi>(out var regDownloadTypedManager).Should().BeTrue(); // Built-in
            registry.TryGetDownloadManagerFor<ITransferUndefinedApi>(out var regCustomDownloadTypedManager).Should().BeTrue(); // Custom

            regTransferManager.Should().NotBeNull(); // Built-in
            regTransferTypedManager.Should().NotBeNull(); // Built-in
            regCustomTransferTypedManager.Should().NotBeNull(); // Custom
            regDownloadManager.Should().NotBeNull(); // Built-in
            regDownloadTypedManager.Should().NotBeNull(); // Built-in
            regCustomDownloadTypedManager.Should().NotBeNull(); // Custom

            // Shortcut
            var regShortcutResult = await registry.DownloadAsync(new FileInfo("test100k.db"));
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

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                        .AddTransferManager(options => options
                            .WithLogging()
                            .WithBaseAddress("http://speedtest.ftp.otenet.gr/files")
                            .WithProgress()));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var apizrTransferManager = scope.ServiceProvider.GetService<IApizrTransferManager>(); // Built-in
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

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                        .AddTransferManager(options => options
                            .WithLogging()
                            .WithBaseAddress("http://speedtest.ftp.otenet.gr/files")
                            .WithProgress(progress)));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var apizrTransferManager = scope.ServiceProvider.GetService<IApizrTransferManager>(); // Built-in
            apizrTransferManager.Should().NotBeNull(); // Built-in

            var fileInfo = await apizrTransferManager.DownloadAsync(new FileInfo("test10Mb.db")).ConfigureAwait(false);

            percentage.Should().Be(100);
            fileInfo.Length.Should().BePositive();
        }

        [Fact]
        public async Task Uploading_File_Should_Succeed()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                            .AddTransferManager()
                            .AddTransferManagerFor<ITransferUndefinedApi>()
                            .AddUploadManager()
                            .AddUploadManagerFor<ITransferUndefinedApi>(),
                        options => options
                            .WithLogging()
                            .WithBaseAddress("https://httpbin.org/post"));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            // Get instances from the container
            var apizrTransferManager = scope.ServiceProvider.GetService<IApizrTransferManager>(); // Built-in
            var apizrTransferTypedManager = scope.ServiceProvider.GetService<IApizrTransferManager<ITransferApi>>(); // Built-in
            var apizrCustomTransferManager = scope.ServiceProvider.GetService<IApizrTransferManager<ITransferUndefinedApi>>(); // Custom
            var apizrUploadManager = scope.ServiceProvider.GetService<IApizrUploadManager>(); // Built-in
            var apizrUploadTypedManager = scope.ServiceProvider.GetService<IApizrUploadManager<IUploadApi>>(); // Built-in
            var apizrCustomUploadManager = scope.ServiceProvider.GetService<IApizrUploadManager<ITransferUndefinedApi>>(); // Custom

            apizrTransferManager.Should().NotBeNull(); // Built-in
            apizrTransferTypedManager.Should().NotBeNull(); // Built-in
            apizrCustomTransferManager.Should().NotBeNull(); // Custom
            apizrUploadManager.Should().NotBeNull(); // Built-in
            apizrUploadTypedManager.Should().NotBeNull(); // Built-in
            apizrCustomUploadManager.Should().NotBeNull(); // Custom
            
            // Transfer
            // Built-in
            var apizrTransferManagerResult = await apizrTransferManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            apizrTransferManagerResult.Should().NotBeNull();
            apizrTransferManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Built-in
            var apizrTransferTypedManagerResult = await apizrTransferTypedManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            apizrTransferTypedManagerResult.Should().NotBeNull();
            apizrTransferTypedManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Custom
            var apizrCustomTransferManagerResult = await apizrCustomTransferManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            apizrCustomTransferManagerResult.Should().NotBeNull();
            apizrCustomTransferManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Download
            // Built-in
            var apizrUploadManagerResult = await apizrUploadManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            apizrUploadManagerResult.Should().NotBeNull();
            apizrUploadManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Built-in
            var apizrUploadTypedManagerResult = await apizrUploadTypedManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            apizrUploadTypedManagerResult.Should().NotBeNull();
            apizrUploadTypedManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Custom
            var apizrCustomUploadManagerResult = await apizrCustomUploadManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            apizrCustomUploadManagerResult.Should().NotBeNull();
            apizrCustomUploadManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Get instances from the registry
            var registry = scope.ServiceProvider.GetRequiredService<IApizrExtendedRegistry>();

            registry.TryGetTransferManager(out var regTransferManager).Should().BeTrue(); // Built-in
            registry.TryGetTransferManagerFor<ITransferApi>(out var regTransferTypedManager).Should().BeTrue(); // Built-in
            registry.TryGetTransferManagerFor<ITransferUndefinedApi>(out var regCustomTransferTypedManager).Should().BeTrue(); // Custom
            registry.TryGetUploadManager(out var regUploadManager).Should().BeTrue(); // Built-in
            registry.TryGetUploadManagerFor<IUploadApi>(out var regUploadTypedManager).Should().BeTrue(); // Built-in
            registry.TryGetUploadManagerFor<ITransferUndefinedApi>(out var regCustomUploadTypedManager).Should().BeTrue(); // Custom

            regTransferManager.Should().NotBeNull(); // Built-in
            regTransferTypedManager.Should().NotBeNull(); // Built-in
            regCustomTransferTypedManager.Should().NotBeNull(); // Custom
            regUploadManager.Should().NotBeNull(); // Built-in
            regUploadTypedManager.Should().NotBeNull(); // Built-in
            regCustomUploadTypedManager.Should().NotBeNull(); // Custom

            // Shortcut
            var regShortcutResult = await registry.UploadAsync<IUploadApi>(FileHelper.GetTestFileStreamPart("small"));
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
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                            .AddGroup(group => group
                                .AddTransferManager()
                                .AddTransferManagerFor<ITransferUndefinedApi>()
                                .AddUploadManager()
                                .AddUploadManagerFor<ITransferUndefinedApi>()),
                        options => options
                            .WithLogging()
                            .WithBaseAddress("https://httpbin.org/post"));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            // Get instances from the container
            var apizrTransferManager = scope.ServiceProvider.GetService<IApizrTransferManager>(); // Built-in
            var apizrTransferTypedManager = scope.ServiceProvider.GetService<IApizrTransferManager<ITransferApi>>(); // Built-in
            var apizrCustomTransferManager = scope.ServiceProvider.GetService<IApizrTransferManager<ITransferUndefinedApi>>(); // Custom
            var apizrUploadManager = scope.ServiceProvider.GetService<IApizrUploadManager>(); // Built-in
            var apizrUploadTypedManager = scope.ServiceProvider.GetService<IApizrUploadManager<IUploadApi>>(); // Built-in
            var apizrCustomUploadManager = scope.ServiceProvider.GetService<IApizrUploadManager<ITransferUndefinedApi>>(); // Custom

            apizrTransferManager.Should().NotBeNull(); // Built-in
            apizrTransferTypedManager.Should().NotBeNull(); // Built-in
            apizrCustomTransferManager.Should().NotBeNull(); // Custom
            apizrUploadManager.Should().NotBeNull(); // Built-in
            apizrUploadTypedManager.Should().NotBeNull(); // Built-in
            apizrCustomUploadManager.Should().NotBeNull(); // Custom

            // Transfer
            // Built-in
            var apizrTransferManagerResult = await apizrTransferManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            apizrTransferManagerResult.Should().NotBeNull();
            apizrTransferManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Built-in
            var apizrTransferTypedManagerResult = await apizrTransferTypedManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            apizrTransferTypedManagerResult.Should().NotBeNull();
            apizrTransferTypedManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Custom
            var apizrCustomTransferManagerResult = await apizrCustomTransferManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            apizrCustomTransferManagerResult.Should().NotBeNull();
            apizrCustomTransferManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Download
            // Built-in
            var apizrUploadManagerResult = await apizrUploadManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            apizrUploadManagerResult.Should().NotBeNull();
            apizrUploadManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Built-in
            var apizrUploadTypedManagerResult = await apizrUploadTypedManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            apizrUploadTypedManagerResult.Should().NotBeNull();
            apizrUploadTypedManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Custom
            var apizrCustomUploadManagerResult = await apizrCustomUploadManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            apizrCustomUploadManagerResult.Should().NotBeNull();
            apizrCustomUploadManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);

            // Get instances from the registry
            var registry = scope.ServiceProvider.GetRequiredService<IApizrExtendedRegistry>();

            registry.TryGetTransferManager(out var regTransferManager).Should().BeTrue(); // Built-in
            registry.TryGetTransferManagerFor<ITransferApi>(out var regTransferTypedManager).Should().BeTrue(); // Built-in
            registry.TryGetTransferManagerFor<ITransferUndefinedApi>(out var regCustomTransferTypedManager).Should().BeTrue(); // Custom
            registry.TryGetUploadManager(out var regUploadManager).Should().BeTrue(); // Built-in
            registry.TryGetUploadManagerFor<IUploadApi>(out var regUploadTypedManager).Should().BeTrue(); // Built-in
            registry.TryGetUploadManagerFor<ITransferUndefinedApi>(out var regCustomUploadTypedManager).Should().BeTrue(); // Custom

            regTransferManager.Should().NotBeNull(); // Built-in
            regTransferTypedManager.Should().NotBeNull(); // Built-in
            regCustomTransferTypedManager.Should().NotBeNull(); // Custom
            regUploadManager.Should().NotBeNull(); // Built-in
            regUploadTypedManager.Should().NotBeNull(); // Built-in
            regCustomUploadTypedManager.Should().NotBeNull(); // Custom

            // Shortcut
            var regShortcutResult = await registry.UploadAsync<IUploadApi, HttpResponseMessage>(FileHelper.GetTestFileStreamPart("small"));
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
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                        .AddTransferManager(options => options
                            .WithLogging()
                            .WithBaseAddress("https://httpbin.org/post")
                            .WithProgress()));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var apizrTransferManager = scope.ServiceProvider.GetService<IApizrTransferManager>(); // Built-in
            apizrTransferManager.Should().NotBeNull(); // Built-in

            var percentage = 0;
            var progress = new ApizrProgress();
            progress.ProgressChanged += (sender, args) =>
            {
                percentage = args.ProgressPercentage;
            };

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

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                        .AddTransferManager(options => options
                            .WithLogging()
                            .WithBaseAddress("https://httpbin.org/post")
                            .WithProgress(progress)));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var apizrTransferManager = scope.ServiceProvider.GetService<IApizrTransferManager>(); // Built-in
            apizrTransferManager.Should().NotBeNull(); // Built-in

            var apizrTransferManagerResult = await apizrTransferManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));

            apizrTransferManagerResult.Should().NotBeNull();
            apizrTransferManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);
            percentage.Should().Be(100);
        }

        [Fact]
        public async Task Calling_WithFileTransferMediation_Should_Handle_Requests()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

                    services.AddApizr(registry => registry
                            .AddGroup(transferRegistry => transferRegistry
                                .AddTransferManagerFor<ITransferSampleApi>()
                                .AddTransferManager(options => options
                                    .WithBaseAddress("http://speedtest.ftp.otenet.gr/files"))),
                        config => config
                            .WithLogging()
                            .WithFileTransferMediation());

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var apizrMediator = scope.ServiceProvider.GetRequiredService<IApizrMediator>();

            apizrMediator.Should().NotBeNull();
            var result = await apizrMediator.SendDownloadQuery(new FileInfo("test100k.db"));
            result.Should().NotBeNull();
            result.Length.Should().BePositive();
        }

        [Fact]
        public async Task Calling_WithFileTransferOptionalMediation_Should_Handle_Requests_With_Optional_Result()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

                    services.AddApizr(
                        registry => registry
                            .AddTransferManagerFor<ITransferSampleApi>()
                            .AddTransferManager(options => options
                                .WithBaseAddress("http://speedtest.ftp.otenet.gr/files")),
                        config => config
                            .WithLogging()
                            .WithFileTransferOptionalMediation());

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var apizrMediator = scope.ServiceProvider.GetRequiredService<IApizrOptionalMediator>();

            apizrMediator.Should().NotBeNull();
            var result = await apizrMediator.SendDownloadOptionalQuery(new FileInfo("test100k.db"));
            result.Should().NotBeNull();
            result.Match(fileInfo =>
                {
                    fileInfo.Length.Should().BePositive();
                },
                e => {
                    // ignore error
                });
        }

        [Fact]
        public async Task Requesting_With_Headers_Should_Set_Headers()
        {
            var watcher = new WatchingRequestHandler();

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                            .AddManagerFor<IReqResSimpleService>(options => options
                                .WithBaseAddress("https://reqres.in/api")
                                .WithHeaders(["testKey3: testValue3.2", "testKey4: testValue4.1"])
                                .WithLoggedHeadersRedactionNames(["testKey2"])
                                .WithDelegatingHandler(watcher)),
                        options => options
                            .WithLogging()
                            .WithHeaders(["testKey2: testValue2.2", "testKey3: testValue3.1"]));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            // Get instances from the container
            var apizrManager = scope.ServiceProvider.GetService<IApizrManager<IReqResSimpleService>>(); // Custom

            // Shortcut
            await apizrManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt), 
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
            var apiHeaders = new List<string> { "testKey2: testValue2.2", "testKey3: testValue3.1" };
            var requestHeaders = new List<string> { "testKey3: testValue3.2", "testKey4: testValue4.1", "testKey5: testValue5.1" };
            var testStore = new TestSettings("testStoreKey2: testStoreValue2.1");

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddSettings();
                    services.AddSingleton(_ => testStore);

                    services.AddApizr(registry => registry
                            .AddManagerFor<IReqResSimpleService>(options => options
                                .WithBaseAddress("https://reqres.in/api")
                                .WithHeaders(_ => requestHeaders, scope: ApizrLifetimeScope.Request)
                                .WithHeaders(["testKey6: testValue6.1", "testKey7: testValue7.2"])
                                .WithHeaders<IOptions<TestSettings>>([settings => settings.Value.TestJsonString], scope: ApizrLifetimeScope.Request)
                                .WithHeaders<TestSettings>([settings => settings.TestJsonString], scope: ApizrLifetimeScope.Request, mode: ApizrRegistrationMode.Store)
                                .WithDelegatingHandler(watcher)),
                        options => options.WithLogging()
                            .WithResilienceContextOptions(opt =>
                                opt.ReturnToPoolOnComplete(false))
                            .WithHeaders(_ => apiHeaders, scope: ApizrLifetimeScope.Api)
                            .WithHeaders(["testKey7: testValue7.1", "testKey8: testValue8.1"])
                            .WithHeaders(["testStoreKey1: testStoreValue1.1", "testStoreKey3: testStoreValue3.1"], mode: ApizrRegistrationMode.Store));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            // Get instances from the container
            var apizrManager = scope.ServiceProvider.GetService<IApizrManager<IReqResSimpleService>>(); // Custom

            // Shortcut
            await apizrManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt),
                options => options.WithHeaders(["testKey5: testValue5.2",
                    "testKey6: testValue6.2"]));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKeys("testKey1", "testKey2", "testKey3", "testKey4", "testKey5", "testKey6", "testKey7", "testKey8", "testSettingsKey1", "testStoreKey1", "testStoreKey2")
                .And.NotContainKey("testStoreKey3:");
            watcher.Headers.GetValues("testKey1").Should().HaveCount(1).And.Contain("testValue1"); // Set by attribute
            watcher.Headers.GetValues("testKey2").Should().HaveCount(1).And.Contain("testValue2.2"); // Set by attribute then updated by common option within api scope factory
            watcher.Headers.GetValues("testKey3").Should().HaveCount(1).And.Contain("testValue3.2"); // Set by common option within api scope factory then updated by proper option within request scope factory
            watcher.Headers.GetValues("testKey4").Should().HaveCount(1).And.Contain("testValue4.1"); // Set by proper option within request scope factory
            watcher.Headers.GetValues("testKey5").Should().HaveCount(1).And.Contain("testValue5.2"); // Set by proper option within request scope factory then updated by request option
            watcher.Headers.GetValues("testKey6").Should().HaveCount(1).And.Contain("testValue6.2"); // Set by proper option then updated by request option
            watcher.Headers.GetValues("testKey7").Should().HaveCount(1).And.Contain("testValue7.2"); // Set by common option then by proper option
            watcher.Headers.GetValues("testKey8").Should().HaveCount(1).And.Contain("testValue8.1"); // Set by common option
            watcher.Headers.GetValues("testSettingsKey1").Should().HaveCount(1).And.Contain("testSettingsValue1.1"); // Set by common option expression
            watcher.Headers.GetValues("testStoreKey1").Should().HaveCount(1).And.Contain("testStoreValue1.1"); // Set by common option from Store
            watcher.Headers.GetValues("testStoreKey2").Should().HaveCount(1).And.Contain("testStoreValue2.1"); // Set by common option from Store

            // Keep updated
            apiHeaders[1] = "testKey3: testValue3.3"; // will not be updated (scope: Api)
            requestHeaders[1] = "testKey4: testValue4.2"; // will be updated (scope: Request)
            requestHeaders[2] = "testKey5: testValue5.3"; // should be updated (scope: Request) but updated then by request option
            testStore.TestJsonString = "testStoreKey2: testStoreValue2.2";
            var settings = scope.ServiceProvider.GetService<IOptions<TestSettings>>();
            settings.Value.TestJsonString = "testSettingsKey1: testSettingsValue1.2"; // will be updated (scope: Request)

            await apizrManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt),
                options => options.WithHeaders(["testKey5: testValue5.4",
                    "testKey6: testValue6.3",
                    "testStoreKey1: testStoreValue1.2",
                    "testStoreKey3: {0}"]));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKeys("testKey1", "testKey2", "testKey3", "testKey4", "testKey5", "testKey6", "testKey7", "testKey8");
            watcher.Headers.GetValues("testKey1").Should().HaveCount(1).And.Contain("testValue1"); // Same as previous value
            watcher.Headers.GetValues("testKey2").Should().HaveCount(1).And.Contain("testValue2.2"); // Same as previous value
            watcher.Headers.GetValues("testKey3").Should().HaveCount(1).And.Contain("testValue3.2"); // Same as previous value
            watcher.Headers.GetValues("testKey4").Should().HaveCount(1).And.Contain("testValue4.2"); // Updated at request time (scope: Request)
            watcher.Headers.GetValues("testKey5").Should().HaveCount(1).And.Contain("testValue5.4"); // Updated at request time (scope: Request) then by request option
            watcher.Headers.GetValues("testKey6").Should().HaveCount(1).And.Contain("testValue6.3"); // Updated by request option
            watcher.Headers.GetValues("testKey7").Should().HaveCount(1).And.Contain("testValue7.2"); // Same as previous value
            watcher.Headers.GetValues("testKey8").Should().HaveCount(1).And.Contain("testValue8.1"); // Same as previous value
            watcher.Headers.GetValues("testSettingsKey1").Should().HaveCount(1).And.Contain("testSettingsValue1.2"); // Updated at request time (scope: Request)
            watcher.Headers.GetValues("testStoreKey1").Should().HaveCount(1).And.Contain("testStoreValue1.2"); // Updated by request option
            watcher.Headers.GetValues("testStoreKey2").Should().HaveCount(1).And.Contain("testStoreValue2.2"); // Updated at request time (scope: Request)
            watcher.Headers.GetValues("testStoreKey3").Should().HaveCount(1).And.Contain("testStoreValue3.1"); // Set by request option from Store
        }

        [Fact]
        public async Task Requesting_With_Both_Attribute_And_Fluent_Headers_Should_Set_Merged_Headers()
        {
            var watcher = new WatchingRequestHandler();

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddSettings();

                    services.AddApizr(registry => registry
                            .AddManagerFor<IReqResSimpleService>(options => options.WithHeaders(["testKey2: testValue2"])
                                .WithDelegatingHandler(watcher)),
                        options => options
                            .WithLogging()
                            .WithHeaders(serviceProvider => new[]
                            {
                                $"TestJsonString: {serviceProvider.GetRequiredService<IOptions<TestSettings>>().Value.TestJsonString}",
                                "testKey3: testValue3"
                            }));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            // Get instances from the container
            var reqResManager = scope.ServiceProvider.GetService<IApizrManager<IReqResSimpleService>>(); // Custom

            await reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt),
                options => options.WithHeaders(["testKey4: testValue4"]));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKeys("testKey1", "testKey2", "TestJsonString", "testKey3", "testKey4");
            watcher.Headers.GetValues("TestJsonString").Should().HaveCount(1).And.Contain("TestJsonString");
        }

        [Fact]
        public async Task Cancelling_A_Get_Request_Should_Throw_An_OperationCanceledException()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                        .AddManagerFor<IReqResUserService>(options => options
                            .WithLogging()
                            .WithDelegatingHandler(new TestRequestHandler())));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

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
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                        .AddManagerFor<IHttpBinService>(options => options
                            .WithLogging()));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var manager = scope.ServiceProvider.GetRequiredService<IApizrManager<IHttpBinService>>();

            var streamPart = FileHelper.GetTestFileStreamPart("medium");
            var ct = new CancellationTokenSource();
            ct.CancelAfter(TimeSpan.FromSeconds(2));

            Func<Task> act = () => manager.ExecuteAsync((opt, api) => api.UploadAsync(streamPart, opt),
                options => options.WithCancellation(ct.Token));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<OperationCanceledException>();
        }

        [Fact]
        public async Task Calling_ConfigureClient_Should_Configure_HttpClient()
        {
            var watcher = new WatchingRequestHandler();

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                        .AddManagerFor<IReqResUserService>(options =>
                            options
                                .WithLogging()
                                .ConfigureHttpClientBuilder(builder => builder.ConfigureHttpClient(client =>
                                    client.DefaultRequestHeaders.Add("HttpClientHeaderKey", "HttpClientHeaderValue")))
                                .WithDelegatingHandler(watcher)));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            await reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKey("HttpClientHeaderKey");
        }

        [Fact]
        public async Task When_Calling_BA_WithOperationTimeout_Then_Request_Should_Throw_A_TimeoutException()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizrManagerFor<IReqResUserService>(
                        options => options
                            .WithLogging()
                            .WithOperationTimeout(TimeSpan.FromSeconds(4)));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(6, opt),
                    options => options.WithOperationTimeout(TimeSpan.FromSeconds(2)));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutRejectedException>();
        }

        [Fact]
        public async Task When_Calling_AB_WithOperationTimeout_Then_Client_Should_Throw_A_TimeoutException()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                        .AddManagerFor<IReqResUserService>(options =>
                            options
                                .WithLogging()
                                .WithOperationTimeout(TimeSpan.FromSeconds(2))));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(6, opt),
                    options => options.WithOperationTimeout(TimeSpan.FromSeconds(4)));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutRejectedException>();
        }

        [Fact]
        public async Task When_Calling_BA_WithRequestTimeout_Then_Request_Should_Throw_A_TimeoutException()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                        .AddManagerFor<IReqResUserService>(
                            options => options
                                .WithLogging()
                                .WithRequestTimeout(TimeSpan.FromSeconds(4))));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(6, opt),
                    options => options.WithRequestTimeout(TimeSpan.FromSeconds(2)));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutRejectedException>();
        }

        [Fact]
        public async Task When_Calling_AB_WithRequestTimeout_Then_Client_Should_Throw_A_TimeoutException()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                        .AddManagerFor<IReqResUserService>(options =>
                            options
                                .WithLogging()
                                .WithRequestTimeout(TimeSpan.FromSeconds(2))));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(6, opt),
                    options => options.WithRequestTimeout(TimeSpan.FromSeconds(4)));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutRejectedException>();
        }

        [Fact]
        public async Task When_Calling_DCBA_WithOperationTimeout_And_WithRequestTimeout_Then_Request_Should_Throw_A_TimeoutException()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                        .AddManagerFor<IReqResUserService>(
                            options => options
                                .WithLogging()
                                .WithOperationTimeout(TimeSpan.FromSeconds(8))
                                .WithRequestTimeout(TimeSpan.FromSeconds(6))));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

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
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                        .AddManagerFor<IReqResUserService>(options =>
                            options
                                .WithLogging()
                                .WithOperationTimeout(TimeSpan.FromSeconds(2))
                                .WithRequestTimeout(TimeSpan.FromSeconds(4))));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

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
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                        .AddManagerFor<IReqResUserService>(options =>
                            options
                                .WithLogging()
                                .WithOperationTimeout(TimeSpan.FromSeconds(4))));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

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
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                        .AddManagerFor<IReqResUserService>(options =>
                            options
                                .WithLogging()
                                .WithOperationTimeout(TimeSpan.FromSeconds(2))));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

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
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                        .AddManagerFor<IReqResUserService>(options =>
                            options
                                .WithLogging()
                                .WithOperationTimeout(TimeSpan.FromSeconds(4))));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

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
        public async Task When_Calling_WithRequestTimeout_With_TimeoutRejected_Strategy_Then_It_Should_Retry_3_On_3_Times()
        {
            var watcher = new WatchingRequestHandler();

            var maxRetryCount = 3;
            var retryCount = 0;

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                        .AddManagerFor<IReqResUserService>(
                            options => options
                                .WithLogging()
                                .WithResilienceContextOptions(opt =>
                                    opt.ReturnToPoolOnComplete(false))
                                .WithRequestTimeout(TimeSpan.FromSeconds(3))
                                .WithDelegatingHandler(watcher)));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddRetry(
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
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(6, opt));//,
            //options => options.WithTimeout(TimeSpan.FromSeconds(3)));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutRejectedException>();

            // attempts should be equal to 2 as request timed out before the 3rd retry
            retryCount.Should().Be(3);
            watcher.Attempts.Should().Be(4);
        }

        [Fact]
        public async Task When_Calling_WithOperationTimeout_With_TimeoutRejected_Strategy_Then_It_Should_Retry_2_On_3_Times()
        {
            var watcher = new WatchingRequestHandler();

            var maxRetryCount = 3;
            var retryCount = 0;

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                        .AddManagerFor<IReqResUserService>(
                            options => options
                                .WithLogging()
                                .WithResilienceContextOptions(opt =>
                                    opt.ReturnToPoolOnComplete(false))
                                .WithOperationTimeout(TimeSpan.FromSeconds(10))
                                .WithDelegatingHandler(watcher)));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddRetry(
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
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

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
        public async Task When_Calling_WithRequestTimeout_WithOperationTimeout_WithCancellation_And_With_TimeoutRejected_Strategy_Then_It_Should_Retry_1_On_3_Times()
        {
            var watcher = new WatchingRequestHandler();

            var maxRetryCount = 3;
            var retryCount = 0;

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                        .AddManagerFor<IReqResUserService>(
                            options => options
                                .WithLogging()
                                .WithResilienceContextOptions(opt =>
                                    opt.ReturnToPoolOnComplete(false))
                                .WithOperationTimeout(TimeSpan.FromSeconds(10))
                                .WithDelegatingHandler(watcher)));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddRetry(
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
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5));

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
            var testHandler = new TestRequestHandler();

            var maxRetryCount = 3;
            var retryCount = 0;

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                        .AddManagerFor<IReqResUserService>(
                            options => options
                                .WithLogging()
                                .WithResilienceContextOptions(opt =>
                                    opt.ReturnToPoolOnComplete(false))
                                .WithDelegatingHandler(testHandler)
                                .WithOperationTimeout(TimeSpan.FromSeconds(3))));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddRetry(
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
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(HttpStatusCode.RequestTimeout, opt));//,
                                                                                                                //options => options.WithTimeout(TimeSpan.FromSeconds(3)));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutRejectedException>();

            // attempts should be equal to 2 as request timed out before the 3rd retry
            retryCount.Should().Be(2);
            testHandler.Attempts.Should().Be(2);
        }
    }
}
