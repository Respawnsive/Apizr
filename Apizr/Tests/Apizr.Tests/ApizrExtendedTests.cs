using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Manager;
using Apizr.Extending.Configuring.Registry;
using Apizr.Logging;
using Apizr.Mediation.Extending;
using Apizr.Mediation.Requesting.Sending;
using Apizr.Optional.Extending;
using Apizr.Optional.Requesting.Sending;
using Apizr.Policing;
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
    public class ApizrExtendedTests
    {
        private readonly IPolicyRegistry<string> _policyRegistry;
        private readonly RefitSettings _refitSettings;
        private readonly Assembly _assembly;

        public ApizrExtendedTests()
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
        public void ServiceCollection_Should_Contain_Managers()
        {
            var services = new ServiceCollection();
            services.AddApizrManagerFor<IReqResUserService>()
                .AddApizrManagerFor<IHttpBinService>()

                .AddApizrCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>()

                .AddApizrUploadManager(options => options.WithBaseAddress("https://test.com"))
                .AddApizrUploadManagerFor<ITransferSampleApi>()
                .AddApizrUploadManagerWith<string>(options => options.WithBaseAddress("https://test.com"))

                .AddApizrDownloadManager(options => options.WithBaseAddress("https://test.com"))
                .AddApizrDownloadManagerFor<ITransferSampleApi>()
                .AddApizrDownloadManagerWith<User>(options => options.WithBaseAddress("https://test.com"))

                .AddApizrTransferManager(options => options.WithBaseAddress("https://test.com"))
                .AddApizrTransferManagerFor<ITransferSampleApi>()
                .AddApizrTransferManagerWith<User, string>(options => options.WithBaseAddress("https://test.com"));
            
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
        }

        [Fact]
        public void ServiceProvider_Should_Resolve_Managers()
        {
            var services = new ServiceCollection();
            services.AddApizrManagerFor<IReqResUserService>()
                .AddApizrManagerFor<IHttpBinService>()

                .AddApizrCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>()

                .AddApizrUploadManager(options => options.WithBaseAddress("https://test.com"))
                .AddApizrUploadManagerFor<ITransferSampleApi>()
                .AddApizrUploadManagerWith<string>(options => options.WithBaseAddress("https://test.com"))

                .AddApizrDownloadManager(options => options.WithBaseAddress("https://test.com"))
                .AddApizrDownloadManagerFor<ITransferSampleApi>()
                .AddApizrDownloadManagerWith<User>(options => options.WithBaseAddress("https://test.com"))

                .AddApizrTransferManager(options => options.WithBaseAddress("https://test.com"))
                .AddApizrTransferManagerFor<ITransferSampleApi>()
                .AddApizrTransferManagerWith<User, string>(options => options.WithBaseAddress("https://test.com"));

            var serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetService<IApizrManager<IReqResUserService>>().Should().NotBeNull();
            serviceProvider.GetService<IApizrManager<IHttpBinService>>().Should().NotBeNull();

            serviceProvider.GetService<IApizrManager<ICrudApi<User, int, PagedResult<User>, IDictionary<string, object>>>>().Should().NotBeNull();

            serviceProvider.GetService<IApizrUploadManager<IUploadApi>>().Should().NotBeNull();
            serviceProvider.GetService<IApizrUploadManager<ITransferSampleApi>>().Should().NotBeNull();
            serviceProvider.GetService<IApizrUploadManager<IUploadApi<string>, string>>().Should().NotBeNull();
            serviceProvider.GetService<IApizrUploadManagerWith<string>>().Should().NotBeNull();

            serviceProvider.GetService<IApizrDownloadManager<IDownloadApi>>().Should().NotBeNull();
            serviceProvider.GetService<IApizrDownloadManager<ITransferSampleApi>>().Should().NotBeNull();
            serviceProvider.GetService<IApizrDownloadManager<IDownloadApi<User>, User>>().Should().NotBeNull();
            serviceProvider.GetService<IApizrDownloadManagerWith<User>>().Should().NotBeNull();

            serviceProvider.GetService<IApizrTransferManager<ITransferApi>>().Should().NotBeNull();
            serviceProvider.GetService<IApizrTransferManager<ITransferSampleApi>>().Should().NotBeNull();
            serviceProvider.GetService<IApizrTransferManager<ITransferApi<User, string>, User, string>>().Should().NotBeNull();
            serviceProvider.GetService<IApizrTransferManagerWith<User, string>>().Should().NotBeNull();
        }

        [Fact]
        public void ServiceProvider_Should_Resolve_Scanned_Managers()
        {
            var services = new ServiceCollection();
            services.AddApizrCrudManagerFor(_assembly);

            var serviceProvider = services.BuildServiceProvider();
            var userManager = serviceProvider.GetService<IApizrManager<ICrudApi<User, int, PagedResult<User>, IDictionary<string, object>>>>();
            
            userManager.Should().NotBeNull();
        }

        [Fact]
        public void Calling_WithBaseAddress_Should_Set_BaseAddress()
        {
            var attributeUri = "https://reqres.in/api";
            var uri1 = new Uri("http://uri1.com");

            // By attribute
            var services = new ServiceCollection();
            services.AddApizrManagerFor<IReqResUserService>();

            var serviceProvider = services.BuildServiceProvider();
            var fixture = serviceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            fixture.Options.BaseUri.Should().Be(attributeUri);

            // By proper option overriding attribute
            services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizrManagerFor<IReqResUserService>(options => options.WithBaseAddress(uri1));

            serviceProvider = services.BuildServiceProvider();
            fixture = serviceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            fixture.Options.BaseUri.Should().Be(uri1);
        }

        [Fact]
        public void Calling_WithBaseAddress_And_WithBasePath_Should_Set_BaseUri()
        {
            var baseAddress = "https://reqres.in/api";
            var basePath = "users";
            var baseUri = $"{baseAddress}/{basePath}";

            var services = new ServiceCollection();
            services.AddApizrManagerFor<IReqResUserService>(options => options.WithBaseAddress(baseAddress).WithBasePath(basePath));

            var serviceProvider = services.BuildServiceProvider();
            var fixture = serviceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            fixture.Options.BaseUri.Should().Be(baseUri);
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_ProperOption_Should_Authenticate_Request()
        {
            string token = null;

            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizrManagerFor<IHttpBinService>(options => options
                    .WithAuthenticationHandler(_ => Task.FromResult(token = "token")));

            var serviceProvider = services.BuildServiceProvider();
            var httpBinManager = serviceProvider.GetRequiredService<IApizrManager<IHttpBinService>>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
            token.Should().Be("token");
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_With_Default_Token_Should_Authenticate_Request()
        {
            var testSettings = new TestSettings("token");

            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddSingleton(testSettings);

            services.AddApizrManagerFor<IHttpBinService>(options => options
                .WithAuthenticationHandler<TestSettings>(settings => settings.TestJsonString));

            var serviceProvider = services.BuildServiceProvider();
            var httpBinManager = serviceProvider.GetRequiredService<IApizrManager<IHttpBinService>>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public void Calling_WithLogging_Should_Set_LoggingSettings()
        {
            var services = new ServiceCollection();
            services.AddApizrManagerFor<IReqResUserService>(options => options.WithLogging((HttpTracerMode) HttpTracerMode.ExceptionsOnly, (HttpMessageParts) HttpMessageParts.RequestCookies, LogLevel.Warning));

            var serviceProvider = services.BuildServiceProvider();
            var fixture = serviceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

            fixture.Options.HttpTracerMode.Should().Be(HttpTracerMode.ExceptionsOnly);
            fixture.Options.TrafficVerbosity.Should().Be(HttpMessageParts.RequestCookies);
            fixture.Options.LogLevels.Should().AllBeEquivalentTo(LogLevel.Warning);
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_Should_Authenticate_Request()
        {
            string token = null;

            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizrManagerFor<IHttpBinService>(options => options
                        .WithAuthenticationHandler(_ => Task.FromResult(token = "tokenA")));

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

            services.AddApizrManagerFor<IReqResUserService>(config => config
                    .WithAkavacheCacheHandler()
                    .AddDelegatingHandler(new TestRequestHandler()));

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

            services.AddApizrManagerFor<IReqResUserService>(config => config
                    .AddDelegatingHandler(new TestRequestHandler()));

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
            services.AddApizrManagerFor<IReqResUserService>(config => config
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
            services.AddAutoMapper(_assembly);
            services.AddApizrManagerFor<IReqResUserService>(config => config
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
            services.AddApizrManagerFor<IReqResUserService>(config => config
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
        public async Task Calling_WithMappingHandler_With_AutoMapper_Should_Map_Data()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddAutoMapper(_assembly);
            services.AddApizrManagerFor<IReqResUserService>(config => config
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
        public async Task Calling_WithMapsterMappingHandler_Should_Map_Data()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);

            var mapsterConfig = new TypeAdapterConfig();
            mapsterConfig.NewConfig<User, MinUser>()
                .TwoWays()
                .Map(minUser => minUser.Name, user => user.FirstName);

            services.AddSingleton(mapsterConfig);
            services.AddScoped<IMapper, ServiceMapper>();

            services.AddApizrManagerFor<IReqResUserService>(config => config
                    .WithRefitSettings(_refitSettings)
                    .WithMapsterMappingHandler());

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
        public async Task Calling_WithMappingHandler_With_Mapster_Should_Map_Data()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            
            var mapsterConfig = new TypeAdapterConfig();
            mapsterConfig.NewConfig<User, MinUser>()
                .TwoWays()
                .Map(minUser => minUser.Name, user => user.FirstName);

            services.AddSingleton(mapsterConfig);
            services.AddScoped<IMapper, ServiceMapper>();

            services.AddApizrManagerFor<IReqResUserService>(config => config
                    .WithRefitSettings(_refitSettings)
                    .WithMappingHandler<MapsterMappingHandler>());

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
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddApizrManagerFor<IReqResUserService>(config => config
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
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddApizrManagerFor<IReqResUserService>(config => config
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
        public async Task Downloading_File_Should_Succeed()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);

            services.AddApizrTransferManager(options => options.WithBaseAddress("http://speedtest.ftp.otenet.gr/files"))
                .AddApizrTransferManagerFor<ITransferUndefinedApi>(options => options.WithBaseAddress("http://speedtest.ftp.otenet.gr/files"))
                .AddApizrDownloadManager(options => options.WithBaseAddress("http://speedtest.ftp.otenet.gr/files"))
                .AddApizrDownloadManagerFor<ITransferUndefinedApi>(options => options.WithBaseAddress("http://speedtest.ftp.otenet.gr/files"));

            var serviceProvider = services.BuildServiceProvider();

            // Get instances from the container
            var apizrTransferManager = serviceProvider.GetService<IApizrTransferManager>(); // Built-in
            var apizrTransferTypedManager = serviceProvider.GetService<IApizrTransferManager<ITransferApi>>(); // Built-in
            var apizrCustomTransferManager = serviceProvider.GetService<IApizrTransferManager<ITransferUndefinedApi>>(); // Custom
            var apizrDownloadManager = serviceProvider.GetService<IApizrDownloadManager>(); // Built-in
            var apizrDownloadTypedManager = serviceProvider.GetService<IApizrDownloadManager<IDownloadApi>>(); // Built-in
            var apizrCustomDownloadManager = serviceProvider.GetService<IApizrDownloadManager<ITransferUndefinedApi>>(); // Custom

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
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizrTransferManager(options => options
                        .WithBaseAddress("http://speedtest.ftp.otenet.gr/files")
                        .WithProgress());

            var serviceProvider = services.BuildServiceProvider();

            var apizrTransferManager = serviceProvider.GetService<IApizrTransferManager>(); // Built-in
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
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizrTransferManager(options => options
                    .WithBaseAddress("http://speedtest.ftp.otenet.gr/files")
                    .WithProgress(progress));

            var serviceProvider = services.BuildServiceProvider();

            var apizrTransferManager = serviceProvider.GetService<IApizrTransferManager>(); // Built-in
            apizrTransferManager.Should().NotBeNull(); // Built-in

            var fileInfo = await apizrTransferManager.DownloadAsync(new FileInfo("test10Mb.db")).ConfigureAwait(false);

            percentage.Should().Be(100);
            fileInfo.Length.Should().BePositive();
        }

        [Fact]
        public async Task Uploading_File_Should_Succeed()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);

            services.AddApizrTransferManager(options => options.WithBaseAddress("https://httpbin.org/post"))
                .AddApizrTransferManagerFor<ITransferUndefinedApi>(options => options.WithBaseAddress("https://httpbin.org/post"))
                .AddApizrUploadManager(options => options.WithBaseAddress("https://httpbin.org/post"))
                .AddApizrUploadManagerFor<ITransferUndefinedApi>(options => options.WithBaseAddress("https://httpbin.org/post"));

            var serviceProvider = services.BuildServiceProvider();

            // Get instances from the container
            var apizrTransferManager = serviceProvider.GetService<IApizrTransferManager>(); // Built-in
            var apizrTransferTypedManager = serviceProvider.GetService<IApizrTransferManager<ITransferApi>>(); // Built-in
            var apizrCustomTransferManager = serviceProvider.GetService<IApizrTransferManager<ITransferUndefinedApi>>(); // Custom
            var apizrUploadManager = serviceProvider.GetService<IApizrUploadManager>(); // Built-in
            var apizrUploadTypedManager = serviceProvider.GetService<IApizrUploadManager<IUploadApi>>(); // Built-in
            var apizrCustomUploadManager = serviceProvider.GetService<IApizrUploadManager<ITransferUndefinedApi>>(); // Custom

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
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizrTransferManager(options => options
                    .WithBaseAddress("https://httpbin.org/post")
                    .WithProgress());

            var serviceProvider = services.BuildServiceProvider();

            var apizrTransferManager = serviceProvider.GetService<IApizrTransferManager>(); // Built-in
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
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizrTransferManager(options => options
                        .WithBaseAddress("https://httpbin.org/post")
                        .WithProgress(progress));

            var serviceProvider = services.BuildServiceProvider();

            var apizrTransferManager = serviceProvider.GetService<IApizrTransferManager>(); // Built-in
            apizrTransferManager.Should().NotBeNull(); // Built-in

            var apizrTransferManagerResult = await apizrTransferManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));

            apizrTransferManagerResult.Should().NotBeNull();
            apizrTransferManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);
            percentage.Should().Be(100);
        }

        [Fact]
        public async Task Calling_WithFileTransferMediation_Should_Handle_Requests()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddApizrTransferManager(config => config
                .WithBaseAddress("http://speedtest.ftp.otenet.gr/files")
                .WithFileTransferMediation());

            var serviceProvider = services.BuildServiceProvider();
            var apizrMediator = serviceProvider.GetRequiredService<IApizrMediator>();

            apizrMediator.Should().NotBeNull();
            var result = await apizrMediator.SendDownloadQuery(new FileInfo("test100k.db"));
            result.Should().NotBeNull();
            result.Length.Should().BePositive();
        }

        [Fact]
        public async Task Calling_WithFileTransferOptionalMediation_Should_Handle_Requests_With_Optional_Result()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddApizrTransferManager(config => config
                .WithBaseAddress("http://speedtest.ftp.otenet.gr/files")
                .WithFileTransferOptionalMediation());

            var serviceProvider = services.BuildServiceProvider();
            var apizrMediator = serviceProvider.GetRequiredService<IApizrOptionalMediator>();

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
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);

            services.AddApizrTransferManagerFor<ITransferUndefinedApi>(options => options
                .WithBaseAddress("https://httpbin.org/post")
                .WithHeaders("testKey2: testValue2")
                .AddDelegatingHandler(watcher));

            var serviceProvider = services.BuildServiceProvider();

            // Get instances from the container
            var apizrCustomTransferManager = serviceProvider.GetService<IApizrTransferManager<ITransferUndefinedApi>>(); // Custom
            
            // Shortcut
            await apizrCustomTransferManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKey("testKey2");
        }

        [Fact]
        public async Task Requesting_With_Both_Attribute_And_Fluent_Headers_Should_Set_Merged_Headers()
        {
            var watcher = new WatchingRequestHandler();
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);

            services.AddApizrManagerFor<IReqResSimpleService>(options => options
                .WithHeaders("testKey2: testValue2")
                .AddDelegatingHandler(watcher));

            var serviceProvider = services.BuildServiceProvider();

            // Get instances from the container
            var reqResManager = serviceProvider.GetService<IApizrManager<IReqResSimpleService>>(); // Custom

            await reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt),
                options => options.WithHeaders("testKey3: testValue3", "testKey4: testValue4"));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKeys("testKey1", "testKey2", "testKey3", "testKey4");
        }

        [Fact]
        public async Task Concurrent_Requests_Should_Not_Throw()
        {
            var services = new ServiceCollection();

            services.AddApizrManagerFor<IReqResSimpleService>();

            var serviceProvider = services.BuildServiceProvider();
            var reqResManager = serviceProvider.GetRequiredService<IApizrManager<IReqResSimpleService>>();

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
            var services = new ServiceCollection();

            services.AddApizrManagerFor<IReqResUserService>();

            var serviceProvider = services.BuildServiceProvider();
            var reqResManager = serviceProvider.GetRequiredService<IApizrManager<IReqResUserService>>();

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
            var services = new ServiceCollection();

            services.AddApizrManagerFor<IHttpBinService>();

            var serviceProvider = services.BuildServiceProvider();
            var manager = serviceProvider.GetRequiredService<IApizrManager<IHttpBinService>>();

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
