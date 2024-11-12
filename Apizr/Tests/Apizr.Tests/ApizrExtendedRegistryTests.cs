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
using Apizr.Extending;
using Apizr.Extending.Configuring.Registry;
using Apizr.Logging;
using Apizr.Mediation.Cruding.Sending;
using Apizr.Mediation.Extending;
using Apizr.Mediation.Requesting.Sending;
using Apizr.Optional.Cruding.Sending;
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
using FluentAssertions;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http.Resilience;
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
        public void ServiceCollection_With_Many_Registries_Should_Contain_Single_Registry_And_Many_Managers()
        {
            var services = new ServiceCollection();

            services.AddApizr(registry => registry
                .AddManagerFor<IReqResUserService>()
                .AddManagerFor<IHttpBinService>()

                .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>());

            services.AddApizr(registry => registry
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
        public void ServiceCollection_Should_Contain_Registry_And_Scanned_Managers()
        {
            var services = new ServiceCollection();
            services.AddApizr(registry => registry.AddManagerFor([_assembly])
                .AddCrudManagerFor([_assembly]));

            services.Should().Contain(x => x.ServiceType == typeof(IApizrManager<IReqResUserService>))
                .And.Contain(x => x.ServiceType == typeof(IApizrManager<IHttpBinService>))
                .And.Contain(x => x.ServiceType == typeof(IApizrManager<ICrudApi<User, int, PagedResult<User>, IDictionary<string, object>>>));


            var serviceProvider = services.BuildServiceProvider();
            var registry = serviceProvider.GetService<IApizrExtendedRegistry>();
            registry.Should().NotBeNull();

            registry.ContainsManagerFor<IReqResUserService>().Should().BeTrue();
            registry.ContainsManagerFor<IHttpBinService>().Should().BeTrue();
            registry.ContainsCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>().Should().BeTrue();
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
        public void ServiceCollection_With_Many_Registries_Should_Resolve_Single_Registry_And_Many_Managers()
        {
            var services = new ServiceCollection();

            services.AddApizr(registry => registry
                .AddManagerFor<IReqResUserService>(options => options.WithBaseAddress("http://test.com/1"))
                .AddManagerFor<IHttpBinService>()

                .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>());

            services.AddApizr(registry => registry
                .AddManagerFor<IReqResUserService>(options => options.WithBaseAddress("http://test.com/2")) // Should be ignored because it's not an override but a mistake

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

            registry.TryGetManagerFor<IReqResUserService>(out var reqResManager).Should().BeTrue();
            reqResManager.Options.BaseUri.Should().Be("http://test.com/1"); // Second registration should be ignored

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
                .AddCrudManagerFor([_assembly]));

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
            Task<string> OnRefreshToken(HttpRequestMessage request, string tk, CancellationToken ct) => Task.FromResult(token = "token");

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                        .AddManagerFor<IHttpBinService>(options => options
                            .WithLogging().WithAuthenticationHandler(OnRefreshToken)));

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
        public async Task Calling_WithAuthenticationHandler_With_Local_Token_Should_Authenticate_Request()
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

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                        .AddManagerFor<IHttpBinService>(
                            options => options
                                .WithLogging()
                                .WithHttpMessageHandler(new WatchingRequestHandler())
                                .WithAuthenticationHandler(OnGetToken, OnSetToken, OnRefreshToken)
                                .WithLoggedHeadersRedactionNames(["Authorization"])));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var httpBinManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IHttpBinService>>();

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
                    .AddGroup(group => group
                            .AddManagerFor<IReqResUserService>()
                            .AddManagerFor<IReqResResourceService>(
                                options => options
                                    .WithLogging(HttpTracerMode.Everything, HttpMessageParts.AllButRequestBody, LogLevel.Trace)),
                        options => options
                            .WithLogging(HttpTracerMode.ExceptionsOnly, HttpMessageParts.RequestCookies, LogLevel.Warning))
                    .AddManagerFor<IHttpBinService>()
                    .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(),
                options => options
                    .WithLogging(HttpTracerMode.ErrorsAndExceptionsOnly, HttpMessageParts.HeadersOnly, LogLevel.Debug));

            var serviceProvider = services.BuildServiceProvider();
            var apizrRegistry = serviceProvider.GetService<IApizrExtendedRegistry>();

            var reqResUserManager = apizrRegistry.GetManagerFor<IReqResUserService>();
            reqResUserManager.Options.HttpTracerMode.Should().Be(HttpTracerMode.ExceptionsOnly);
            reqResUserManager.Options.TrafficVerbosity.Should().Be(HttpMessageParts.RequestCookies);
            reqResUserManager.Options.LogLevels.Should().AllBeEquivalentTo(LogLevel.Warning);

            var reqResResourceManager = apizrRegistry.GetManagerFor<IReqResResourceService>();
            reqResResourceManager.Options.HttpTracerMode.Should().Be(HttpTracerMode.Everything);
            reqResResourceManager.Options.TrafficVerbosity.Should().Be(HttpMessageParts.AllButRequestBody);
            reqResResourceManager.Options.LogLevels.Should().AllBeEquivalentTo(LogLevel.Trace);

            var httpBinManager = apizrRegistry.GetManagerFor<IHttpBinService>();
            httpBinManager.Options.HttpTracerMode.Should().Be(HttpTracerMode.ExceptionsOnly);
            httpBinManager.Options.TrafficVerbosity.Should().Be(HttpMessageParts.None);
            httpBinManager.Options.LogLevels.Should().AllBeEquivalentTo(LogLevel.Critical);

            var userManager = apizrRegistry.GetCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>();
            userManager.Options.HttpTracerMode.Should().Be(HttpTracerMode.ErrorsAndExceptionsOnly);
            userManager.Options.TrafficVerbosity.Should().Be(HttpMessageParts.HeadersOnly);
            userManager.Options.LogLevels.Should().AllBeEquivalentTo(LogLevel.Debug);
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_CommonOption_Should_Authenticate_Request()
        {
            string token = null;
            Task<string> OnRefreshToken(HttpRequestMessage request, string tk, CancellationToken ct) => Task.FromResult(token = "token");

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
                            .WithLogging().WithAuthenticationHandler(OnRefreshToken));

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
            Task<string> OnRefreshTokenA(HttpRequestMessage request, string tk, CancellationToken ct) => Task.FromResult(token = "tokenA");
            Task<string> OnRefreshTokenB(HttpRequestMessage request, string tk, CancellationToken ct) => Task.FromResult(token = "tokenB");

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(
                        registry => registry
                            .AddManagerFor<IHttpBinService>(options => options.WithAuthenticationHandler(OnRefreshTokenA)),
                        config => config
                            .WithLogging().WithAuthenticationHandler(OnRefreshTokenB));

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
        public async Task Calling_WithAuthenticationHandler_With_Local_Token_Service_Should_Authenticate_Request()
        {
            var tokenService = new TokenService();

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton(tokenService);

                    services.AddApizr(
                        registry => registry
                            .AddManagerFor<IHttpBinService>(),
                        options => options
                            .WithLogging()
                            .WithAuthenticationHandler<TokenService, TokenService>(
                                (ts, request, ct) => ts.GetTokenAsync(request, ct),
                                (ts, request, tk, ct) => ts.SetTokenAsync(request, tk, ct),
                                (ts, request, tk, ct) => ts.RefreshTokenAsync(request, tk, ct)));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var httpBinManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IHttpBinService>>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_With_Local_Token_Handler_Should_Authenticate_Request()
        {
            var tokenService = new TokenService();

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton(tokenService);
                    services.AddTransient(typeof(AuthHandler<>));

                    services.AddApizr(
                        registry => registry
                            .AddManagerFor<IHttpBinService>(),
                        options => options
                            .WithLogging()
                            .WithAuthenticationHandler(typeof(AuthHandler<>)));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var httpBinManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IHttpBinService>>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
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
        public async Task Calling_WithAkavacheCacheHandler_Should_Cache_ApizrResponse()
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
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(
                        registry => registry
                            .AddManagerFor<IReqResUserService>(),
                        options => options
                            .WithLogging()
                            .WithAkavacheCacheHandler()
                            .WithDelegatingHandler(new TestRequestHandler()));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

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
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(
                        registry => registry
                            .AddManagerFor<IReqResUserService>(
                                options => options.WithCaching(CacheMode.GetOrFetch)),
                        options => options
                            .WithLogging()
                            .WithAkavacheCacheHandler()
                            .WithDelegatingHandler(new TestRequestHandler()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();


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
            Func<bool, Func<ApizrException, bool>, Task<ApiResult<User>>> act = (clearCache, onException) => reqResManager.ExecuteAsync(api => api.GetUsersAsync(HttpStatusCode.BadRequest), options => options.WithCacheClearing(clearCache).WithExCatching(onException, false));

            // Calling it at first execution should throw as expected without any cached result
            var ex = await act.Invoking(x => x.Invoke(false, _ => false)).Should().ThrowAsync<ApizrException<ApiResult<User>>>();
            ex.And.CachedResult.Should().BeNull();

            // This one should succeed
            var result = await reqResManager.ExecuteAsync(api => api.GetUsersAsync());

            // and cache result in-memory
            result.Should().NotBeNull();
            result.Data.Should().NotBeNullOrEmpty();

            // This one should fail but with cached result
            ex = await act.Invoking(x => x.Invoke(false, _ => false)).Should().ThrowAsync<ApizrException<ApiResult<User>>>();
            ex.And.CachedResult.Should().NotBeNull();

            // this one should return cached result and handle exception
            result = await act.Invoke(false, e =>
            {
                // The handled exception with cached result on this side
                e.Should().BeOfType<ApizrException<ApiResult<User>>>().Which.CachedResult.Should().NotBeNull();
                return true;
            });

            // The returned result on the other side
            result.Should().NotBeNull();

            // This one should fail but without any cached result as we asked for clearing it
            ex = await act.Invoking(x => x.Invoke(true, _ => false)).Should().ThrowAsync<ApizrException<ApiResult<User>>>();
            ex.And.CachedResult.Should().BeNull();
        }

        [Fact]
        public async Task Calling_WithAkavacheCacheHandler_And_WithCacheControlHeader_Should_Cache_ApizrResponse()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(
                        registry => registry.AddManagerFor<IApizrTestsApi>(),
                        options => options.WithLogging()
                            .WithAkavacheCacheHandler()
                            .WithCaching(CacheMode.SetByHeader));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IApizrTestsApi>>();

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
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(
                        registry => registry.AddManagerFor<IApizrTestsApi>(),
                        options => options.WithLogging()
                            .WithAkavacheCacheHandler()
                            .WithCaching(CacheMode.SetByHeader));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IApizrTestsApi>>();

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
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(
                        registry => registry.AddManagerFor<IApizrTestsApi>(),
                        options => options.WithLogging()
                            .WithAkavacheCacheHandler()
                            .WithCaching(CacheMode.SetByHeader));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IApizrTestsApi>>();

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
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(
                        registry => registry.AddManagerFor<IApizrTestsApi>(),
                        options => options.WithLogging()
                            .WithAkavacheCacheHandler()
                            .WithCaching(CacheMode.SetByHeader));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IApizrTestsApi>>();

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
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(
                        registry => registry.AddManagerFor<IApizrTestsApi>(),
                        options => options.WithLogging()
                            .WithAkavacheCacheHandler()
                            .WithCaching(CacheMode.SetByHeader));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IApizrTestsApi>>();

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
        public async Task RequestTimeout_Should_Be_Handled_By_Microsoft_Resilience()
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
                            .WithDelegatingHandler(new TestRequestHandler())
                            .ConfigureHttpClientBuilder(builder => builder
                                .AddStandardResilienceHandler()
                                .Configure(options => options.Retry.OnRetry = args =>
                                {
                                    retryCount = args.AttemptNumber + 1;
                                    return default;
                                })));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            // Defining a transient throwing request
            Func<Task> act = () => reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(HttpStatusCode.RequestTimeout, opt));

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
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            typeAdapterConfig.RuleMap.Clear();
            typeAdapterConfig.ForType<User, MinUser>()
                .TwoWays()
                .Map(minUser => minUser.Name, user => user.FirstName);

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton(typeAdapterConfig);
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
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            typeAdapterConfig.RuleMap.Clear();
            typeAdapterConfig.ForType<User, MinUser>()
                .TwoWays()
                .Map(minUser => minUser.Name, user => user.FirstName);

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton(typeAdapterConfig);
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
            ResiliencePropertyKey<string> testKey6 = new(nameof(testKey6));
            ResiliencePropertyKey<string> testKey7 = new(nameof(testKey7));
            ResiliencePropertyKey<string> testKey8 = new(nameof(testKey8));
            ResiliencePropertyKey<string> testKey9 = new(nameof(testKey9));

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
                                        .WithResilienceProperty(testKey7, _ => "testValue7.1")
                                        .WithDelegatingHandler(watcher)
                                        .WithRequestOptions(nameof(IReqResUserService.GetUsersAsync), requestOptions =>
                                            requestOptions.WithResilienceProperty(testKey4, "testValue4.2")
                                                .WithResilienceProperty(testKey8, "testValue8.1"))),
                                // group
                                options => options.WithResilienceProperty(testKey2, _ => "testValue2.2")
                                    .WithResilienceProperty(testKey6, _ => "testValue6.1")),
                        // common
                        options => options
                            .WithLogging()
                            .WithResilienceContextOptions(opt =>
                                opt.ReturnToPoolOnComplete(false))
                            .WithResilienceProperty(testKey1, _ => "testValue1.1")
                            .WithResilienceProperty(testKey2, _ => "testValue2.1")
                            .WithResilienceProperty(testKey3, _ => "testValue3.1")
                            .WithResilienceProperty(testKey4, _ => "testValue4.1")
                            .WithResilienceProperty(testKey5, _ => "testValue5.1"));

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
        public async Task Calling_WithMediation_Should_Handle_Requests()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(_assembly));

                    services.AddApizr(
                        registry => registry
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

            var response = await reqResMediator.SendFor(api => api.SafeGetUsersAsync());
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Exception.Should().BeNull();
            response.Result.Should().NotBeNull();
            response.Result.Data.Should().NotBeNull();
            response.Result.Data.Count.Should().BeGreaterOrEqualTo(1);
        }

        [Fact]
        public async Task Calling_WithMediation_With_Crud_Should_Handle_Requests()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(_assembly));

                    services.AddApizr(
                        registry => registry
                            .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>()
                            .AddCrudManagerFor<UserInfos, int, PagedResult<UserInfos>, IDictionary<string, object>>(),
                        config => config
                        .WithLogging()
                        .WithRefitSettings(_refitSettings)
                        .WithMediation());

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var userMediator = scope.ServiceProvider.GetRequiredService<IApizrCrudMediator<User, int, PagedResult<User>, IDictionary<string, object>>>();

            userMediator.Should().NotBeNull();

            var user = new User { FirstName = "Test" };

            var result = await userMediator.SendCreateCommand(user);
            result.Should().NotBeNull();
            result.FirstName.Should().Be(user.FirstName);
            result.Id.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task Calling_WithMediation_With_Scanning_Should_Handle_Requests()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(_assembly));

                    services.AddApizr(
                        registry => registry
                            .AddManagerFor([_assembly]),
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

            var response = await reqResMediator.SendFor(api => api.SafeGetUsersAsync());
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Exception.Should().BeNull();
            response.Result.Should().NotBeNull();
            response.Result.Data.Should().NotBeNull();
            response.Result.Data.Count.Should().BeGreaterOrEqualTo(1);
        }

        [Fact]
        public async Task Calling_WithMediation_With_Crud_Scanning_Should_Handle_Requests()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(_assembly));

                    services.AddApizr(
                        registry => registry
                            .AddCrudManagerFor([_assembly]),
                        config => config
                            .WithLogging()
                            .WithRefitSettings(_refitSettings)
                            .WithMediation());

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var userMediator = scope.ServiceProvider.GetRequiredService<IApizrCrudMediator<User, int, PagedResult<User>, IDictionary<string, object>>>();

            userMediator.Should().NotBeNull();

            var user = new User { FirstName = "Test" };

            var result = await userMediator.SendCreateCommand(user);
            result.Should().NotBeNull();
            result.FirstName.Should().Be(user.FirstName);
            result.Id.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task Calling_Both_WithMediation_And_WithAutoMapperMappingHandler_Should_Handle_Requests()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(_assembly));
                    services.AddAutoMapper(_assembly);

                    services.AddApizr(
                        registry => registry
                            .AddManagerFor<IReqResUserService>()
                            .AddManagerFor<IReqResResourceService>(),
                        config => config
                            .WithLogging()
                            .WithRefitSettings(_refitSettings)
                            .WithMediation()
                            .WithAutoMapperMappingHandler());

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResMediator = scope.ServiceProvider.GetRequiredService<IApizrMediator<IReqResUserService>>();

            reqResMediator.Should().NotBeNull();

            // Unmapped
            var user1 = new User { FirstName = "John" };

            var result1 = await reqResMediator.SendFor(api => api.CreateUser(user1, CancellationToken.None));
            result1.Should().NotBeNull();
            result1.FirstName.Should().Be(user1.FirstName);
            result1.Id.Should().BeGreaterThanOrEqualTo(1);

            // Mapped
            var user2 = new MinUser { Name = "John" };

            var result2 = await reqResMediator.SendFor<MinUser, User>((api, user) => api.CreateUser(user, CancellationToken.None), user2);
            result2.Should().NotBeNull();
            result2.Name.Should().Be(user2.Name);
            result2.Id.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task Calling_Both_WithMediation_And_WithAutoMapperMappingHandler_With_Crud_Should_Handle_Requests()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(_assembly));
                    services.AddAutoMapper(_assembly);

                    services.AddApizr(
                        registry => registry
                            .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>()
                            .AddCrudManagerFor<UserInfos, int, PagedResult<UserInfos>, IDictionary<string, object>>(),
                        config => config
                            .WithLogging()
                            .WithRefitSettings(_refitSettings)
                            .WithMediation()
                            .WithAutoMapperMappingHandler());

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var userMediator = scope.ServiceProvider.GetRequiredService<IApizrCrudMediator<User, int, PagedResult<User>, IDictionary<string, object>>>();

            userMediator.Should().NotBeNull();

            // Unmapped
            var user1 = new User { FirstName = "John" };

            var result1 = await userMediator.SendCreateCommand(user1);
            result1.Should().NotBeNull();
            result1.FirstName.Should().Be(user1.FirstName);
            result1.Id.Should().BeGreaterThanOrEqualTo(1);

            // Mapped
            var user2 = new MinUser { Name = "John" };

            var result2 = await userMediator.SendCreateCommand(user2);
            result2.Should().NotBeNull();
            result2.Name.Should().Be(user2.Name);
            result2.Id.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task Calling_Both_WithMediation_And_WithAutoMapperMappingHandler_With_Scanning_Should_Handle_Requests()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(_assembly));
                    services.AddAutoMapper(_assembly);

                    services.AddApizr(
                        registry => registry
                            .AddManagerFor([_assembly]),
                        config => config
                        .WithLogging()
                        .WithRefitSettings(_refitSettings)
                        .WithMediation()
                        .WithAutoMapperMappingHandler());

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResMediator = scope.ServiceProvider.GetRequiredService<IApizrMediator<IReqResUserService>>();

            reqResMediator.Should().NotBeNull();

            // Unmapped
            var user1 = new User { FirstName = "John" };

            var result1 = await reqResMediator.SendFor(api => api.CreateUser(user1, CancellationToken.None));
            result1.Should().NotBeNull();
            result1.FirstName.Should().Be(user1.FirstName);
            result1.Id.Should().BeGreaterThanOrEqualTo(1);

            // Mapped
            var user2 = new MinUser { Name = "John" };

            var result2 = await reqResMediator.SendFor<MinUser, User>((api, user) => api.CreateUser(user, CancellationToken.None), user2);
            result2.Should().NotBeNull();
            result2.Name.Should().Be(user2.Name);
            result2.Id.Should().BeGreaterThanOrEqualTo(1);
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
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(_assembly));

                    services.AddApizr(
                        registry => registry
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
                e =>
                {
                    // ignore error
                });
        }

        [Fact]
        public async Task Calling_WithOptionalMediation_With_Crud_Should_Handle_Requests_With_Optional_Result()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(_assembly));

                    services.AddApizr(
                        registry => registry
                            .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>()
                            .AddCrudManagerFor<UserInfos, int, PagedResult<UserInfos>, IDictionary<string, object>>(),
                        config => config
                            .WithLogging()
                            .WithRefitSettings(_refitSettings)
                            .WithOptionalMediation());

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var userMediator = scope.ServiceProvider.GetRequiredService<IApizrCrudOptionalMediator<User, int, PagedResult<User>, IDictionary<string, object>>>();

            userMediator.Should().NotBeNull();

            var user = new User { FirstName = "Test" };

            var result = await userMediator.SendCreateOptionalCommand(user);
            result.Should().NotBeNull();
            result.Match(userResult =>
            {
                userResult.Should().NotBeNull();
                userResult.FirstName.Should().Be(user.FirstName);
                userResult.Id.Should().BeGreaterThanOrEqualTo(1);
            },
                e =>
                {
                    // ignore error
                });
        }

        [Fact]
        public async Task Calling_WithOptionalMediation_With_Scanning_Should_Handle_Requests_With_Optional_Result()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(_assembly));

                    services.AddApizr(
                        registry => registry
                            .AddManagerFor([_assembly]),
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
                e =>
                {
                    // ignore error
                });
        }

        [Fact]
        public async Task Calling_WithOptionalMediation_With_Crud_Scanning_Should_Handle_Requests_With_Optional_Result()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(_assembly));

                    services.AddApizr(
                        registry => registry
                            .AddCrudManagerFor([_assembly]),
                        config => config
                        .WithLogging()
                        .WithRefitSettings(_refitSettings)
                        .WithOptionalMediation());

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var userMediator = scope.ServiceProvider.GetRequiredService<IApizrCrudOptionalMediator<User, int, PagedResult<User>, IDictionary<string, object>>>();

            userMediator.Should().NotBeNull();

            var user = new User { FirstName = "Test" };

            var result = await userMediator.SendCreateOptionalCommand(user);
            result.Should().NotBeNull();
            result.Match(userResult =>
            {
                userResult.Should().NotBeNull();
                userResult.FirstName.Should().Be(user.FirstName);
                userResult.Id.Should().BeGreaterThanOrEqualTo(1);
            },
                e =>
                {
                    // ignore error
                });
        }

        [Fact]
        public async Task Calling_Both_WithOptionalMediation_And_WithAutoMapperMappingHandler_Should_Handle_Requests_With_Optional_Result()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(_assembly));
                    services.AddAutoMapper(_assembly);

                    services.AddApizr(
                        registry => registry
                            .AddManagerFor<IReqResUserService>()
                            .AddManagerFor<IReqResResourceService>(),
                        config => config
                            .WithLogging()
                            .WithRefitSettings(_refitSettings)
                            .WithOptionalMediation()
                            .WithAutoMapperMappingHandler());

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResMediator = scope.ServiceProvider.GetRequiredService<IApizrOptionalMediator<IReqResUserService>>();

            reqResMediator.Should().NotBeNull();

            // Unmapped
            var user1 = new User { FirstName = "John" };

            var result1 = await reqResMediator.SendFor(api => api.CreateUser(user1, CancellationToken.None));
            result1.Should().NotBeNull();
            result1.Match(userResult =>
            {
                userResult.Should().NotBeNull();
                userResult.FirstName.Should().Be(user1.FirstName);
                userResult.Id.Should().BeGreaterThanOrEqualTo(1);
            },
                e =>
                {
                    // ignore error
                });

            // Mapped
            var user2 = new MinUser { Name = "John" };

            var result2 = await reqResMediator.SendFor<MinUser, User>((api, user) => api.CreateUser(user, CancellationToken.None), user2);
            result2.Should().NotBeNull();
            result2.Match(userResult =>
            {
                userResult.Should().NotBeNull();
                userResult.Name.Should().Be(user2.Name);
                userResult.Id.Should().BeGreaterThanOrEqualTo(1);
            },
                e =>
                {
                    // ignore error
                });
        }

        [Fact]
        public async Task Calling_Both_WithOptionalMediation_And_WithAutoMapperMappingHandler_With_Crud_Should_Handle_Requests_With_Optional_Result()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(_assembly));
                    services.AddAutoMapper(_assembly);

                    services.AddApizr(
                        registry => registry
                            .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>()
                            .AddCrudManagerFor<UserInfos, int, PagedResult<UserInfos>, IDictionary<string, object>>(),
                        config => config
                            .WithLogging()
                            .WithRefitSettings(_refitSettings)
                            .WithOptionalMediation()
                            .WithAutoMapperMappingHandler());

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var userMediator = scope.ServiceProvider.GetRequiredService<IApizrCrudOptionalMediator<User, int, PagedResult<User>, IDictionary<string, object>>>();

            userMediator.Should().NotBeNull();

            // Unmapped
            var user1 = new User { FirstName = "John" };

            var result1 = await userMediator.SendCreateOptionalCommand(user1);
            result1.Should().NotBeNull();
            result1.Match(userResult =>
            {
                userResult.Should().NotBeNull();
                userResult.FirstName.Should().Be(user1.FirstName);
                userResult.Id.Should().BeGreaterThanOrEqualTo(1);
            },
                e =>
                {
                    // ignore error
                });

            // Mapped
            var user2 = new MinUser { Name = "John" };

            var result2 = await userMediator.SendCreateOptionalCommand(user2);
            result2.Should().NotBeNull();
            result2.Match(userResult =>
            {
                userResult.Should().NotBeNull();
                userResult.Name.Should().Be(user2.Name);
                userResult.Id.Should().BeGreaterThanOrEqualTo(1);
            },
                e =>
                {
                    // ignore error
                });
        }

        [Fact]
        public async Task Calling_Both_WithOptionalMediation_And_WithAutoMapperMappingHandler_With_Scanning_Should_Handle_Requests_With_Optional_Result()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(_assembly));
                    services.AddAutoMapper(_assembly);

                    services.AddApizr(
                        registry => registry
                            .AddManagerFor([_assembly]),
                        config => config
                        .WithLogging()
                        .WithRefitSettings(_refitSettings)
                        .WithOptionalMediation()
                        .WithAutoMapperMappingHandler());

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResMediator = scope.ServiceProvider.GetRequiredService<IApizrOptionalMediator<IReqResUserService>>();

            reqResMediator.Should().NotBeNull();

            // Unmapped
            var user1 = new User { FirstName = "John" };

            var result1 = await reqResMediator.SendFor(api => api.CreateUser(user1, CancellationToken.None));
            result1.Should().NotBeNull();
            result1.Match(userResult =>
            {
                userResult.Should().NotBeNull();
                userResult.FirstName.Should().Be(user1.FirstName);
                userResult.Id.Should().BeGreaterThanOrEqualTo(1);
            },
                e =>
                {
                    // ignore error
                });

            // Mapped
            var user2 = new MinUser { Name = "John" };

            var result2 = await reqResMediator.SendFor<MinUser, User>((api, user) => api.CreateUser(user, CancellationToken.None), user2);
            result2.Should().NotBeNull();
            result2.Match(userResult =>
            {
                userResult.Should().NotBeNull();
                userResult.Name.Should().Be(user2.Name);
                userResult.Id.Should().BeGreaterThanOrEqualTo(1);
            },
                e =>
                {
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
                            .WithBaseAddress("https://proof.ovh.net/files"));

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
            var apizrTransferManagerResult = await apizrTransferManager.DownloadAsync(new FileInfo("1Mb.dat"));
            apizrTransferManagerResult.Should().NotBeNull();
            apizrTransferManagerResult.Length.Should().BePositive();

            // Built-in
            var apizrTransferTypedManagerResult = await apizrTransferTypedManager.DownloadAsync(new FileInfo("1Mb.dat"));
            apizrTransferTypedManagerResult.Should().NotBeNull();
            apizrTransferTypedManagerResult.Length.Should().BePositive();

            // Custom
            var apizrCustomTransferManagerResult = await apizrCustomTransferManager.DownloadAsync(new FileInfo("1Mb.dat"));
            apizrCustomTransferManagerResult.Should().NotBeNull();
            apizrCustomTransferManagerResult.Length.Should().BePositive();

            // Download
            // Built-in
            var apizrDownloadManagerResult = await apizrDownloadManager.DownloadAsync(new FileInfo("1Mb.dat"));
            apizrDownloadManagerResult.Should().NotBeNull();
            apizrDownloadManagerResult.Length.Should().BePositive();

            // Built-in
            var apizrDownloadTypedManagerResult = await apizrDownloadTypedManager.DownloadAsync(new FileInfo("1Mb.dat"));
            apizrDownloadTypedManagerResult.Should().NotBeNull();
            apizrDownloadTypedManagerResult.Length.Should().BePositive();

            // Custom
            var apizrCustomDownloadManagerResult = await apizrCustomDownloadManager.DownloadAsync(new FileInfo("1Mb.dat"));
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
            var regShortcutResult = await registry.DownloadAsync(new FileInfo("1Mb.dat"));
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
                            .WithBaseAddress("https://proof.ovh.net"));

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
            var apizrTransferManagerResult = await apizrTransferManager.DownloadAsync(new FileInfo("1Mb.dat"));
            apizrTransferManagerResult.Should().NotBeNull();
            apizrTransferManagerResult.Length.Should().BePositive();

            // Built-in
            var apizrTransferTypedManagerResult = await apizrTransferTypedManager.DownloadAsync(new FileInfo("1Mb.dat"));
            apizrTransferTypedManagerResult.Should().NotBeNull();
            apizrTransferTypedManagerResult.Length.Should().BePositive();

            // Custom
            var apizrCustomTransferManagerResult = await apizrCustomTransferManager.DownloadAsync(new FileInfo("1Mb.dat"), options => options.IgnoreMessageParts(HttpMessageParts.ResponseBody));
            apizrCustomTransferManagerResult.Should().NotBeNull();
            apizrCustomTransferManagerResult.Length.Should().BePositive();

            // Download
            // Built-in
            var apizrDownloadManagerResult = await apizrDownloadManager.DownloadAsync(new FileInfo("1Mb.dat"));
            apizrDownloadManagerResult.Should().NotBeNull();
            apizrDownloadManagerResult.Length.Should().BePositive();

            // Built-in
            var apizrDownloadTypedManagerResult = await apizrDownloadTypedManager.DownloadAsync(new FileInfo("1Mb.dat"));
            apizrDownloadTypedManagerResult.Should().NotBeNull();
            apizrDownloadTypedManagerResult.Length.Should().BePositive();

            // Custom
            var apizrCustomDownloadManagerResult = await apizrCustomDownloadManager.DownloadAsync(new FileInfo("1Mb.dat"), options => options.IgnoreMessageParts(HttpMessageParts.ResponseBody));
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
            var regShortcutResult = await registry.DownloadAsync(new FileInfo("1Mb.dat"));
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
            var regCustomTransferTypedManagerResult = await regCustomTransferTypedManager.DownloadAsync(new FileInfo("1Mb.dat"), options => options.IgnoreMessageParts(HttpMessageParts.ResponseBody));
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
            var regCustomDownloadTypedManagerResult = await regCustomDownloadTypedManager.DownloadAsync(new FileInfo("1Mb.dat"), options => options.IgnoreMessageParts(HttpMessageParts.ResponseBody));
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
                            .WithBaseAddress("https://proof.ovh.net/files")
                            .WithProgress()
                            .IgnoreMessageParts(HttpMessageParts.ResponseBody)));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var apizrTransferManager = scope.ServiceProvider.GetService<IApizrTransferManager>(); // Built-in
            apizrTransferManager.Should().NotBeNull(); // Built-in

            var fileInfo = await apizrTransferManager.DownloadAsync(new FileInfo("10Mb.dat"), options => options.WithProgress(progress)).ConfigureAwait(false);

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
                            .WithBaseAddress("https://proof.ovh.net/files")
                            .WithProgress(progress)
                            .IgnoreMessageParts(HttpMessageParts.ResponseBody)));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var apizrTransferManager = scope.ServiceProvider.GetService<IApizrTransferManager>(); // Built-in
            apizrTransferManager.Should().NotBeNull(); // Built-in

            var fileInfo = await apizrTransferManager.DownloadAsync(new FileInfo("10Mb.dat")).ConfigureAwait(false);

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
                            .WithBaseAddress("https://httpbin.org/post")
                            .IgnoreMessageParts(HttpMessageParts.ResponseBody));

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
                            .WithBaseAddress("https://httpbin.org/post")
                            .IgnoreMessageParts(HttpMessageParts.ResponseBody));

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
                            .WithProgress()
                            .IgnoreMessageParts(HttpMessageParts.ResponseBody)));

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
                                    .WithBaseAddress("https://proof.ovh.net/files"))),
                        config => config
                            .WithLogging()
                            .WithFileTransferMediation()
                            .IgnoreMessageParts(HttpMessageParts.ResponseBody));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var apizrMediator = scope.ServiceProvider.GetRequiredService<IApizrMediator>();

            apizrMediator.Should().NotBeNull();
            var result = await apizrMediator.SendDownloadQuery(new FileInfo("1Mb.dat"), options => options.IgnoreMessageParts(HttpMessageParts.ResponseBody));
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
                                .WithBaseAddress("https://proof.ovh.net/files")),
                        config => config
                            .WithLogging()
                            .WithFileTransferOptionalMediation()
                            .IgnoreMessageParts(HttpMessageParts.ResponseBody));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var apizrMediator = scope.ServiceProvider.GetRequiredService<IApizrOptionalMediator>();

            apizrMediator.Should().NotBeNull();
            var result = await apizrMediator.SendDownloadOptionalQuery(new FileInfo("1Mb.dat"), options => options.IgnoreMessageParts(HttpMessageParts.ResponseBody));
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
            var watcher1 = new WatchingRequestHandler();
            var watcher2 = new WatchingRequestHandler();

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
                                .WithDelegatingHandler(watcher1))
                            .AddManagerFor<IReqResUserService>(options => options
                                .WithBaseAddress("https://reqres.in/api")
                                .WithHeaders(["testKey3: testValue3.4", "testKey4: testValue4.2"])
                                .WithLoggedHeadersRedactionNames(["testKey4"])
                                .WithDelegatingHandler(watcher2)),
                        options => options
                            .WithLogging()
                            .WithHeaders(["testKey2: testValue2.2", "testKey3: testValue3.1"]));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            // Get instances from the container
            var simpleManager = scope.ServiceProvider.GetService<IApizrManager<IReqResSimpleService>>(); // Custom

            // Shortcut
            await simpleManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt), 
                options => options.WithHeaders(["testKey4: testValue4.2", "testKey5: testValue5"])
                    .WithLoggedHeadersRedactionRule(header => header == "testKey3"));
            watcher1.Headers.Should().NotBeNull();
            watcher1.Headers.Should().ContainKeys("testKey1", "testKey2", "testKey3", "testKey4", "testKey5");
            watcher1.Headers.GetValues("testKey1").Should().HaveCount(1).And.Contain("testValue1"); // Set by attribute
            watcher1.Headers.GetValues("testKey2").Should().HaveCount(1).And.Contain("testValue2.2"); // Set by attribute then updated by common option
            watcher1.Headers.GetValues("testKey3").Should().HaveCount(1).And.Contain("testValue3.2"); // Set by common option then updated by proper option
            watcher1.Headers.GetValues("testKey4").Should().HaveCount(1).And.Contain("testValue4.2"); // Set by proper option then updated by request option
            watcher1.Headers.GetValues("testKey5").Should().HaveCount(1).And.Contain("testValue5"); // Set by request option

            // Get instances from the container
            var userManager = scope.ServiceProvider.GetService<IApizrManager<IReqResUserService>>(); // Custom

            // Shortcut
            await userManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt),
                options => options.WithHeaders(["testKey4: testValue4.3", "testKey5: testValue5"])
                    .WithLoggedHeadersRedactionRule(header => header == "testKey5"));
            watcher2.Headers.Should().NotBeNull();
            watcher2.Headers.Should().ContainKeys("testKey1", "testKey2", "testKey3", "testKey4", "testKey5");
            watcher2.Headers.GetValues("testKey1").Should().HaveCount(1).And.Contain("testValue1"); // Set by attribute
            watcher2.Headers.GetValues("testKey2").Should().HaveCount(1).And.Contain("testValue2.2"); // Set by attribute then updated by common option
            watcher2.Headers.GetValues("testKey3").Should().HaveCount(1).And.Contain("testValue3.4"); // Set by common option then updated by proper option
            watcher2.Headers.GetValues("testKey4").Should().HaveCount(1).And.Contain("testValue4.3"); // Set by proper option then updated by request option
            watcher2.Headers.GetValues("testKey5").Should().HaveCount(1).And.Contain("testValue5"); // Set by request option
        }

        [Fact]
        public async Task Requesting_With_Headers_Factory_Should_Set_And_Keep_Updated_Headers()
        {
            var watcher = new WatchingRequestHandler();
            var watcher2 = new WatchingRequestHandler();
            var apiHeaders = new List<string> { "testKey2: testValue2.2", "testKey3: testValue3.1" };
            var requestHeaders = new List<string> { "testKey3: testValue3.2", "testKey4: testValue4.1", "testKey5: testValue5.1" };
            var testStore = new TestSettings("testStoreKey2: testStoreValue2.1");

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((context, services) =>
                {
                    services.Configure<TestSettings>(context.Configuration.GetSection(nameof(TestSettings)),
                        option => option.BindNonPublicProperties = true);
                    services.AddSingleton(_ => testStore);

                    services.AddApizr(registry => registry

                            .AddManagerFor<IReqResSimpleService>(options => options
                                //.WithConfiguration(context.Configuration.GetSection("Apizr:ProperOptions:IReqResSimpleService")) // Specific section (manual mapped config)
                                .WithBaseAddress("https://reqres.in/api")
                                .WithHeaders(_ => requestHeaders, scope: ApizrLifetimeScope.Request)
                                .WithHeaders(["testKey6: testValue6.1", "testKey7: testValue7.2"])
                                .WithHeaders<IOptions<TestSettings>>([settings => settings.Value.TestJsonString], scope: ApizrLifetimeScope.Request)
                                .WithHeaders<TestSettings>([settings => settings.TestJsonString], scope: ApizrLifetimeScope.Request, mode: ApizrRegistrationMode.Store)
                                .WithDelegatingHandler(watcher))

                            .AddManagerFor<IHttpBinService>(options => options
                                //.WithConfiguration(context.Configuration.GetSection("Apizr:ProperOptions:IHttpBinService")) // Specific section (manual mapped config)
                                .WithHeaders(["testKey3: testValue3.4", "testKey4: testValue4.2"])
                                .WithLoggedHeadersRedactionNames(["testKey4"])
                                .WithDelegatingHandler(watcher2))

                            .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(),

                        options => options
                            .WithConfiguration(context.Configuration) // Whole configuration (auto mapped config)
                            //.WithConfiguration(context.Configuration.GetSection("Apizr")) // Root section (auto mapped config)
                            //.WithConfiguration(context.Configuration.GetSection("Apizr:CommonOptions")) // Specific section (manual mapped config)
                            //.WithLogging()
                            .WithResilienceContextOptions(opt =>
                                opt.ReturnToPoolOnComplete(false))
                            .WithHeaders(_ => apiHeaders, scope: ApizrLifetimeScope.Api)
                            .WithHeaders(["testKey7: testValue7.1", "testKey8: testValue8.1"])
                            .WithHeaders(["testStoreKey1: testStoreValue1.1", "testStoreKey3: testStoreValue3.1", 
                                "testSettingsKey4: testSettingsValue4.1", "testSettingsKey5: testSettingsValue5.1"], mode: ApizrRegistrationMode.Store));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            // Get instances from the container
            var simpleManager = scope.ServiceProvider.GetService<IApizrManager<IReqResSimpleService>>(); // Custom
            var httpBinManager = scope.ServiceProvider.GetService<IApizrManager<IHttpBinService>>(); // Custom
            var userManager = scope.ServiceProvider.GetService<IApizrManager<ICrudApi<User, int, PagedResult<User>, IDictionary<string, object>>>>(); // Custom

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
                    "testSettingsKey6", "testStoreKey1", "testStoreKey2")
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
            testStore.TestJsonString = "testStoreKey2: testStoreValue2.2";
            var settings = scope.ServiceProvider.GetService<IOptions<TestSettings>>();
            settings.Value.TestJsonString = "testSettingsKey1: testSettingsValue1.2"; // will be updated (scope: Request)

            await simpleManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt),
                options => options.WithHeaders(["testKey5: testValue5.4",
                    "testKey6: testValue6.3",
                    "testStoreKey1: testStoreValue1.2",
                    "testStoreKey3: {0}"]));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKeys("testKey1", "testKey2", "testKey3", "testKey4", "testKey5", "testKey6", "testKey7", "testKey8",
                "testSettingsKey1", "testSettingsKey2", "testSettingsKey3", "testSettingsKey4", "testSettingsKey5",
                "testSettingsKey6", "testStoreKey1", "testStoreKey2", "testStoreKey3");
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

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((context, services) =>
                {
                    services.Configure<TestSettings>(context.Configuration.GetSection(nameof(TestSettings)),
                        option => option.BindNonPublicProperties = true);

                    services.AddApizr(registry => registry
                            .AddManagerFor<IReqResSimpleService>(options => options.WithHeaders(["testKey2: testValue2"])
                                .WithDelegatingHandler(watcher)),
                        options => options
                            .WithLogging()
                            .WithHeaders(serviceProvider => new[]
                            {
                                serviceProvider.GetRequiredService<IOptions<TestSettings>>().Value.TestJsonString,
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
            watcher.Headers.Should().ContainKeys("testKey1", "testKey2", "testSettingsKey1", "testKey3", "testKey4");
            watcher.Headers.GetValues("testSettingsKey1").Should().HaveCount(1).And.Contain("testSettingsValue1.1");
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

        [Fact]
        public async Task Configuring_Exceptions_Handler_Should_Handle_Exceptions()
        {
            var handledException = 0;
            void OnException(ApizrException ex)
            {
                handledException++;
            }

            // Try to queue ex handlers
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                            .AddGroup(group => group
                                    .AddManagerFor<IReqResUserService>(options => options
                                        .WithExCatching(OnException, strategy: ApizrDuplicateStrategy.Add)
                                        .WithDelegatingHandler(new TestRequestHandler()))
                                    .AddManagerFor<IReqResResourceService>(),
                                options => options.WithExCatching(OnException, strategy: ApizrDuplicateStrategy.Add))
                            .AddManagerFor<IHttpBinService>()
                            .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(),
                        options => options
                            .WithLogging()
                            .WithExCatching(OnException, strategy: ApizrDuplicateStrategy.Add));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

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
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                            .AddGroup(group => group
                                    .AddManagerFor<IReqResUserService>(options => options
                                        .WithExCatching(OnException, strategy: ApizrDuplicateStrategy.Add)
                                        .WithDelegatingHandler(new TestRequestHandler()))
                                    .AddManagerFor<IReqResResourceService>(),
                                options => options.WithExCatching(OnException, strategy: ApizrDuplicateStrategy.Add))
                            .AddManagerFor<IHttpBinService>()
                            .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(),
                        options => options
                            .WithLogging()
                            .WithExCatching(OnException, strategy: ApizrDuplicateStrategy.Add));
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

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
            ex.And.Handled.Should().BeFalse();

            handledException.Should().Be(1);
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

            bool OnResolvedException(IServiceProvider sp, ApizrException ex)
            {
                var logger = sp.GetRequiredService<ILogger<ApizrExtendedRegistryTests>>();
                logger.LogError(ex, "Handled by resolved exception handler");

                return true;
            }

            bool mismatchHandled = false;
            bool OnMismatchException(ApizrException<User> ex)
            {
                mismatchHandled = true;
                return false;
            }

            var myExHandler = new MyExHandler();

            // Try to queue ex handlers
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {
                    services.AddApizr(registry => registry
                            .AddGroup(group => group
                                    .AddManagerFor<IReqResUserService>(options => options
                                        .WithExCatching(OnException, strategy: ApizrDuplicateStrategy.Add)
                                        .WithExCatching(OnResolvedException, strategy: ApizrDuplicateStrategy.Add)
                                        .WithExCatching<User>(OnMismatchException, strategy: ApizrDuplicateStrategy.Add)
                                        .WithDelegatingHandler(new TestRequestHandler()))
                                    .AddManagerFor<IReqResResourceService>(),
                                options => options.WithExCatching<MyExHandler>(strategy: ApizrDuplicateStrategy.Add))
                            .AddManagerFor<IHttpBinService>()
                            .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(),
                        options => options
                            .WithLogging()
                            .WithExCatching<MyExHandler>(strategy: ApizrDuplicateStrategy.Add));

                    services.AddSingleton(myExHandler);
                })
                .Build();

            var scope = host.Services.CreateScope();

            var reqResManager = scope.ServiceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

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
