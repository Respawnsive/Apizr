using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Logging;
using Apizr.Tests.Apis;
using Apizr.Tests.Helpers;
using Apizr.Tests.Models;
using Apizr.Tests.Models.Mappings;
using AutoMapper;
using FluentAssertions;
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
            var apizrRegistry = ApizrBuilder.CreateRegistry(registry => registry
                .AddManagerFor<IReqResUserService>()
                .AddManagerFor<IHttpBinService>()
                .AddCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>());

            apizrRegistry.Should().NotBeNull();
            apizrRegistry.ContainsManagerFor<IReqResUserService>().Should().BeTrue();
            apizrRegistry.ContainsManagerFor<IHttpBinService>().Should().BeTrue();
            apizrRegistry.ContainsCrudManagerFor<User, int, PagedResult<User>, IDictionary<string, object>>().Should()
                .BeTrue();
        }

        [Fact]
        public void ApizrRegistry_Should_Get_Managers()
        {
            var apizrRegistry = ApizrBuilder.CreateRegistry(registry => registry
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
            var apizrRegistry = ApizrBuilder.CreateRegistry(registry => registry
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
        public void Calling_WithBaseAddress_Should_Set_BaseAddress()
        {
            var attributeUri = "https://reqres.in/api";
            var uri1 = new Uri("http://uri1.com");
            var uri2 = new Uri("http://uri2.com");

            // By attribute
            var apizrRegistry = ApizrBuilder.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>());
            var reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();
            reqResManager.Options.BaseUri.Should().Be(attributeUri);

            // By attribute overriding common option
            apizrRegistry = ApizrBuilder.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(),
                config => config.WithBaseUri(uri2));
            reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();
            reqResManager.Options.BaseUri.Should().Be(attributeUri);

            // By proper option overriding attribute
            apizrRegistry = ApizrBuilder.CreateRegistry(registry => registry
                    .AddManagerFor<IReqResUserService>(options =>
                        options.WithBaseUri(uri2)));

            reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();
            reqResManager.Options.BaseUri.Should().Be(uri2);

            // By proper option overriding common option and attribute
            apizrRegistry = ApizrBuilder.CreateRegistry(registry => registry
                .AddManagerFor<IReqResUserService>(options => 
                    options.WithBaseUri(uri2)),
                config => config.WithBaseUri(uri1));

            reqResManager = apizrRegistry.GetManagerFor<IReqResUserService>();
            reqResManager.Options.BaseUri.Should().Be(uri2);
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_ProperOption_Should_Authenticate_Request()
        {
            string token = null;

            var apizrRegistry = ApizrBuilder.CreateRegistry(
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
            var apizrRegistry = ApizrBuilder.CreateRegistry(registry => registry
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

            var apizrRegistry = ApizrBuilder.CreateRegistry(registry => registry
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

            var apizrRegistry = ApizrBuilder.CreateRegistry(registry => registry
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
            var apizrRegistry = ApizrBuilder.CreateRegistry(registry => registry
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

            var apizrRegistry = ApizrBuilder.CreateRegistry(registry => registry
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

            var apizrRegistry = ApizrBuilder.CreateRegistry(registry => registry
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
            var apizrRegistry = ApizrBuilder.CreateRegistry(registry => registry
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

            var apizrRegistry = ApizrBuilder.CreateRegistry(registry => registry
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

            var apizrRegistry = ApizrBuilder.CreateRegistry(registry => registry
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
            var apizrRegistry = ApizrBuilder.CreateRegistry(registry => registry
                .AddRegistryGroup(group => group
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
            var apizrRegistry = ApizrBuilder.CreateRegistry(registry => registry
                .AddRegistryGroup(group => group
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
            var apizrRegistry = ApizrBuilder.CreateRegistry(registry => registry
                .AddRegistryGroup(group => group
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
        public void Calling_WithBaseAddress_Grouped_Should_Set_BaseAddress()
        {
            var attributeUri = "https://reqres.in/api";
            var uri1 = new Uri("http://uri1.com");
            var uri2 = new Uri("http://uri2.com");
            var uri3 = new Uri("http://uri3.com");
            var uri4 = new Uri("http://uri4.com");

            // By proper option overriding common option and attribute
            var apizrRegistry = ApizrBuilder.CreateRegistry(registry => registry
                    .AddRegistryGroup(group => group
                            .AddManagerFor<IReqResUserService>()
                            .AddManagerFor<IReqResResourceService>(),
                        config => config.WithBaseUri(uri3))
                    .AddManagerFor<IHttpBinService>(options => options.WithBaseUri(uri2)),
                config => config.WithBaseUri(uri1));

            var userFixture = apizrRegistry.GetManagerFor<IReqResUserService>();
            userFixture.Options.BaseUri.Should().Be(attributeUri);

            var resourceFixture = apizrRegistry.GetManagerFor<IReqResResourceService>();
            resourceFixture.Options.BaseUri.Should().Be(uri3);

            apizrRegistry = ApizrBuilder.CreateRegistry(registry => registry
                    .AddRegistryGroup(group => group
                            .AddManagerFor<IReqResUserService>(config => config.WithBaseUri(uri4))
                            .AddManagerFor<IReqResResourceService>(),
                        config => config.WithBaseUri(uri3))
                    .AddManagerFor<IHttpBinService>(options => options.WithBaseUri(uri2)),
                config => config.WithBaseUri(uri1));


            userFixture = apizrRegistry.GetManagerFor<IReqResUserService>();
            userFixture.Options.BaseUri.Should().Be(uri4);

            resourceFixture = apizrRegistry.GetManagerFor<IReqResResourceService>();
            resourceFixture.Options.BaseUri.Should().Be(uri3);
        }
    }
}
