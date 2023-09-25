using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Logging;
using Apizr.Policing;
using Apizr.Progressing;
using Apizr.Tests.Apis;
using Apizr.Tests.Helpers;
using Apizr.Tests.Models;
using Apizr.Tests.Models.Mappings;
using Apizr.Tests.Settings;
using Apizr.Transferring.Requesting;
using AutoMapper;
using FluentAssertions;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonkeyCache.FileStore;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;
using Refit;
using Xunit;

namespace Apizr.Tests
{
    public class ApizrTests
    {
        private readonly RefitSettings _refitSettings;

        public ApizrTests()
        {
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
        public async Task Calling_WithAuthenticationHandler_Should_Authenticate_Request()
        {
            string token = null;

            var httpBinManager = ApizrBuilder.Current.CreateManagerFor<IHttpBinService>(options =>
                        options.WithAuthenticationHandler(_ => Task.FromResult(token = "token")));

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
            token.Should().Be("token");
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_With_Default_Token_Should_Authenticate_Request()
        {
            var testSettings = new TestSettings("token");

            var httpBinManager = ApizrBuilder.Current.CreateManagerFor<IHttpBinService>(options =>
                options.WithAuthenticationHandler(testSettings, settings => settings.TestJsonString));

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public void Calling_WithLogging_Should_Set_LoggingSettings()
        {
            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(
                options => options.WithLogging((HttpTracerMode) HttpTracerMode.ExceptionsOnly,
                    (HttpMessageParts) HttpMessageParts.RequestCookies, LogLevel.Warning));

            reqResManager.Options.HttpTracerMode.Should().Be(HttpTracerMode.ExceptionsOnly);
            reqResManager.Options.TrafficVerbosity.Should().Be(HttpMessageParts.RequestCookies);
            reqResManager.Options.LogLevels.Should().AllBeEquivalentTo(LogLevel.Warning);
        }
        
        [Fact]
        public async Task Calling_Without_Configuring_Logging_Should_Log_With_Default_Values()
        {
            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResSimpleService>();

            var result = await reqResManager.ExecuteAsync(api => api.GetUsersAsync());

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Calling_WithAkavacheCacheHandler_Should_Cache_Result()
        {
            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options => options
                    .WithAkavacheCacheHandler()
                    .AddDelegatingHandler(new TestRequestHandler()));

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

            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options => options
                    .WithPolicyRegistry(policyRegistry)
                    .AddDelegatingHandler(new TestRequestHandler()));

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

            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options => options
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
            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options => options
                    .WithRefitSettings(_refitSettings));

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

            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options => options
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

            var userManager = ApizrBuilder.Current.CreateCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(options => options
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

            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options => options
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
            TypeAdapterConfig<User, MinUser>
                .NewConfig()
                .TwoWays()
                .Map(minUser => minUser.Name, user => user.FirstName);

            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options => options
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
            TypeAdapterConfig<User, MinUser>
                .NewConfig()
                .TwoWays()
                .Map(minUser => minUser.Name, user => user.FirstName);

            var userManager = ApizrBuilder.Current.CreateCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(options => options
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
            TypeAdapterConfig<User, MinUser>
                .NewConfig()
                .TwoWays()
                .Map(minUser => minUser.Name, user => user.FirstName);

            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options => options
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
        public async Task Requesting_With_Context_into_Options_Should_Set_Context()
        {
            var watcher = new WatchingRequestHandler();

            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options => options.AddDelegatingHandler(watcher));

            var testKey = "TestKey1";
            var testValue = 1;
            // Defining Context
            var context = new Context {{ testKey, testValue } };

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

            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                options.WithContext(() => new Context { { testKey1, testValue1 } })
                    .AddDelegatingHandler(watcher));

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

            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options => options.AddDelegatingHandler(watcher)
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
            var apizrTransferManager = ApizrBuilder.Current.CreateTransferManager(options => options
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
            var apizrTransferManager = ApizrBuilder.Current.CreateTransferManager(options => options
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
            var apizrTransferManager = ApizrBuilder.Current.CreateTransferManager(options => options
                    .WithBaseAddress("http://speedtest.ftp.otenet.gr/files")
                    .WithProgress(progress));
            
            apizrTransferManager.Should().NotBeNull(); // Built-in

            var fileInfo = await apizrTransferManager.DownloadAsync(new FileInfo("test10Mb.db")).ConfigureAwait(false);

            percentage.Should().Be(100);
            fileInfo.Length.Should().BePositive();
        }

        [Fact]
        public async Task Uploading_File_Locally_Should_Succeed()
        {
            var apizrUploadManager = ApizrBuilder.Current.CreateUploadManagerWith<string>(options => options
                .WithBaseAddress("https://localhost:7015/upload"));

            apizrUploadManager.Should().NotBeNull(); // Built-in

            // Shortcut
            var result = await apizrUploadManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            result.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Uploading_File_Should_Succeed()
        {
            var apizrTransferManager = ApizrBuilder.Current.CreateTransferManager(options => options
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
            var apizrTransferManager = ApizrBuilder.Current.CreateTransferManager(options => options
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
            var apizrTransferManager = ApizrBuilder.Current.CreateTransferManager(options => options
                        .WithBaseAddress("https://httpbin.org/post")
                        .WithProgress(progress));
            
            apizrTransferManager.Should().NotBeNull(); // Built-in

            var apizrTransferManagerResult = await apizrTransferManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));

            apizrTransferManagerResult.Should().NotBeNull();
            apizrTransferManagerResult.StatusCode.Should().Be(HttpStatusCode.OK);
            percentage.Should().Be(100);
        }

        [Fact]
        public async Task Requesting_With_Headers_Should_Set_Headers()
        {
            var watcher = new WatchingRequestHandler();

            var apizrTransferManager = ApizrBuilder.Current.CreateTransferManagerFor<ITransferUndefinedApi>(options => options
                .WithBaseAddress("https://httpbin.org/post")
                .WithHeaders("testKey2: testValue2")
                .AddDelegatingHandler(watcher));

            // Shortcut
            await apizrTransferManager.UploadAsync(FileHelper.GetTestFileStreamPart("small"));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKey("testKey2");
        }

        [Fact]
        public async Task Sending_A_Get_Request_With_Both_Attribute_And_Fluent_Headers_Should_Set_Merged_Headers()
        {
            var watcher = new WatchingRequestHandler();

            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResSimpleService>(options => options
                .WithHeaders("testKey2: testValue2")
                .AddDelegatingHandler(watcher));

            await reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt),
                options => options.WithHeaders("testKey3: testValue3", "testKey4: testValue4"));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKeys("testKey1", "testKey2", "testKey3", "testKey4");
        }

        [Fact]
        public async Task Sending_A_Post_Request_With_Both_Attribute_And_Fluent_Headers_Should_Set_Merged_Headers()
        {
            var watcher = new WatchingRequestHandler();

            var manager = ApizrBuilder.Current.CreateManagerFor<IHttpBinService>(options => options
                .WithHeaders("testKey2: testValue2")
                .AddDelegatingHandler(watcher));

            var streamPart = FileHelper.GetTestFileStreamPart("medium");

            await manager.ExecuteAsync((opt, api) => api.UploadAsync(streamPart, opt),
                options => options.WithHeaders("testKey3: testValue3", "testKey4: testValue4"));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKeys("testKey1", "testKey2", "testKey3", "testKey4");
        }

        [Fact]
        public async Task Concurrent_Requests_Should_Not_Throw()
        {
            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResSimpleService>();

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
            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>();

            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(2));

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(5, opt),
                    options => options.WithCancellation(cts.Token));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<OperationCanceledException>();
        }

        [Fact]
        public async Task Cancelling_A_Post_Request_Should_Throw_An_OperationCanceledException()
        {
            var manager = ApizrBuilder.Current.CreateManagerFor<IHttpBinService>();

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

            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResSimpleService>(options =>
                options.ConfigureHttpClient(client => client.DefaultRequestHeaders.Add("HttpClientHeaderKey", "HttpClientHeaderValue"))
                    .AddDelegatingHandler(watcher));

            await reqResManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKey("HttpClientHeaderKey");
        }

        [Fact]
        public async Task Calling_WithTimeout_Should_Throw_A_Request_TimeoutException()
        {
            var reqResManager =
                ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(
                    options => options.WithTimeout(TimeSpan.FromSeconds(4)));

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(6, opt),
                    options => options.WithTimeout(TimeSpan.FromSeconds(2)));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutException>();
        }

        [Fact]
        public async Task Calling_WithTimeout_Should_Throw_A_Client_TimeoutException()
        {
            var reqResManager =
                ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                    options.WithTimeout(TimeSpan.FromSeconds(2)));

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(6, opt),
                    options => options.WithTimeout(TimeSpan.FromSeconds(4)));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutException>();
        }

        [Fact]
        public async Task Calling_Both_WithTimeout_And_WithCancellation_Should_Throw_A_Request_TimeoutException()
        {
            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                options.WithTimeout(TimeSpan.FromSeconds(4)));

            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(6));

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(8, opt),
                    options => options.WithTimeout(TimeSpan.FromSeconds(2))
                        .WithCancellation(cts.Token));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutException>();
        }

        [Fact]
        public async Task Calling_Both_WithTimeout_And_WithCancellation_Should_Throw_A_Client_TimeoutException()
        {
            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                options.WithTimeout(TimeSpan.FromSeconds(2)));

            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(6));

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(8, opt),
                    options => options.WithTimeout(TimeSpan.FromSeconds(4))
                        .WithCancellation(cts.Token));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<TimeoutException>();
        }

        [Fact]
        public async Task Calling_Both_WithTimeout_And_WithCancellation_Should_Throw_An_OperationCanceledException()
        {
            var reqResManager = ApizrBuilder.Current.CreateManagerFor<IReqResUserService>(options =>
                options.WithTimeout(TimeSpan.FromSeconds(4)));

            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(2));

            Func<Task> act = () =>
                reqResManager.ExecuteAsync((opt, api) => api.GetDelayedUsersAsync(8, opt),
                    options => options.WithTimeout(TimeSpan.FromSeconds(6))
                        .WithCancellation(cts.Token));

            var ex = await act.Should().ThrowAsync<ApizrException>();
            ex.WithInnerException<OperationCanceledException>();
        }
    }
}
