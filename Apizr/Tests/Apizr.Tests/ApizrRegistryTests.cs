using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring;
using Apizr.Extending;
using Apizr.Logging;
using Apizr.Policing;
using Apizr.Progressing;
using Apizr.Tests.Apis;
using Apizr.Tests.Helpers;
using Apizr.Tests.Models;
using Apizr.Tests.Models.Mappings;
using Apizr.Transferring.Requesting;
using AutoMapper;
using FluentAssertions;
using Fusillade;
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
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddManagerFor<IReqResUserService>()
                .AddManagerFor<IHttpBinService>()
                //.AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>()
                .AddUploadManagerFor<IUploadApi>()
                //.AddUploadManager(options => options.WithBaseAddress("https://test.com"))
                .AddDownloadManagerFor<IDownloadApi>()
                //.AddDownloadManager(options => options.WithBaseAddress("https://test.com"))
                .AddTransferManagerFor<ITransferApi>());
                //.AddTransferManager(options => options.WithBaseAddress("https://test.com"))

            apizrRegistry.Should().NotBeNull();
            apizrRegistry.ContainsManagerFor<IReqResUserService>().Should().BeTrue();
            apizrRegistry.ContainsManagerFor<IHttpBinService>().Should().BeTrue();
            //apizrRegistry.ContainsCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>().Should().BeTrue();
            apizrRegistry.ContainsManagerFor<IUploadApi>().Should().BeTrue();
            apizrRegistry.ContainsUploadManagerFor<IUploadApi>().Should().BeTrue();
            apizrRegistry.ContainsUploadManager().Should().BeTrue();
            apizrRegistry.ContainsManagerFor<IDownloadApi>().Should().BeTrue();
            apizrRegistry.ContainsDownloadManagerFor<IDownloadApi>().Should().BeTrue();
            apizrRegistry.ContainsDownloadManager().Should().BeTrue();
            apizrRegistry.ContainsManagerFor<ITransferApi>().Should().BeTrue();
            apizrRegistry.ContainsTransferManagerFor<ITransferApi>().Should().BeTrue();
            apizrRegistry.ContainsTransferManager().Should().BeTrue();
        }

        [Fact]
        public void ApizrRegistry_Should_Get_Managers()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddManagerFor<IReqResUserService>()
                .AddManagerFor<IHttpBinService>()
                .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>());

            apizrRegistry.TryGetManagerFor<IReqResUserService>(out var reqResManager).Should().BeTrue();
            apizrRegistry.TryGetManagerFor<IHttpBinService>(out var httpBinManager).Should().BeTrue();
            apizrRegistry.TryGetCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>(out var userManager)
                .Should().BeTrue();

            reqResManager.Should().NotBeNull();
            httpBinManager.Should().NotBeNull();
            userManager.Should().NotBeNull();
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
                    .AddDelegatingHandler(new FailingRequestHandler()));

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
                    .AddDelegatingHandler(new FailingRequestHandler()));

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
        public async Task Calling_WithMappingHandler_Should_Map_Data()
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
                                .AddDelegatingHandler(new FailingRequestHandler()))
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
                .AddTransferManager(options => options.WithBaseAddress("http://speedtest.ftp.otenet.gr/files"))
                .AddTransferManagerFor<ITransferSampleApi>());

            var apizrTransferManager = apizrRegistry.GetTransferManager(); // Built-in
            var transferSampleApiManager = apizrRegistry.GetTransferManagerFor<ITransferSampleApi>(); // Custom

            apizrTransferManager.Should().NotBeNull(); // Built-in
            transferSampleApiManager.Should().NotBeNull(); // Custom

            var fileInfo = await transferSampleApiManager.DownloadAsync(new FileInfo("test100k.db")).ConfigureAwait(false);
            fileInfo.Length.Should().BePositive();
        }

        [Fact]
        public async Task Downloading_File_Grouped_Should_Succeed()
        {
            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddGroup(group => group
                    .AddTransferManager(options => options.WithBaseAddress("http://speedtest.ftp.otenet.gr/files"))
                    .AddTransferManagerFor<ITransferSampleApi>()));

            var apizrTransferManager = apizrRegistry.GetTransferManager(); // Built-in
            var transferSampleApiManager = apizrRegistry.GetTransferManagerFor<ITransferSampleApi>(); // Custom

            apizrTransferManager.Should().NotBeNull(); // Built-in
            transferSampleApiManager.Should().NotBeNull(); // Custom

            var fileInfo = await transferSampleApiManager.DownloadAsync(new FileInfo("test100k.db")).ConfigureAwait(false);
            fileInfo.Length.Should().BePositive();
        }

        [Fact]
        public async Task Downloading_File_With_Progress_Should_Report_Progress()
        {
            var percentage = 0;
            var progress = new ApizrProgress();
            progress.ProgressChanged += (sender, args) =>
            {
                percentage = args.ProgressPercentage;
            };

            var apizrRegistry = ApizrBuilder.Current.CreateRegistry(registry => registry
                .AddGroup(group => group
                    .AddTransferManager(options => options
                        .WithBaseAddress("http://speedtest.ftp.otenet.gr/files"))
                    .AddTransferManagerFor<ITransferSampleApi>()),
                options => options.WithProgress(progress));

            var apizrTransferManager = apizrRegistry.GetTransferManager(); // Built-in
            var transferSampleApiManager = apizrRegistry.GetTransferManagerFor<ITransferSampleApi>(); // Custom

            apizrTransferManager.Should().NotBeNull(); // Built-in
            transferSampleApiManager.Should().NotBeNull(); // Custom
            
            var fileInfo = await apizrTransferManager.DownloadAsync(new FileInfo("test10Mb.db")).ConfigureAwait(false);

            percentage.Should().Be(100);
            fileInfo.Length.Should().BePositive();
        }
    }
}
