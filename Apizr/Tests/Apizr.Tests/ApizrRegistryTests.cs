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
using IReqResService = Apizr.Tests.Apis.IReqResService;

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
            var apizrRegistry = Apizr.Create(registry => registry
                .AddFor<IReqResService>()
                .AddFor<IHttpBinService>()
                .AddCrudFor<User, int, PagedResult<User>, IDictionary<string, object>>());

            apizrRegistry.Should().NotBeNull();
            apizrRegistry.ContainsFor<IReqResService>().Should().BeTrue();
            apizrRegistry.ContainsFor<IHttpBinService>().Should().BeTrue();
            apizrRegistry.ContainsCrudFor<User, int, PagedResult<User>, IDictionary<string, object>>().Should()
                .BeTrue();
        }

        [Fact]
        public void ApizrRegistry_Should_Get_Managers()
        {
            var apizrRegistry = Apizr.Create(registry => registry
                .AddFor<IReqResService>()
                .AddFor<IHttpBinService>()
                .AddCrudFor<User, int, PagedResult<User>, IDictionary<string, object>>());

            apizrRegistry.TryGetFor<IReqResService>(out var reqResManager).Should().BeTrue();
            apizrRegistry.TryGetFor<IHttpBinService>(out var httpBinManager).Should().BeTrue();
            apizrRegistry.TryGetCrudFor<User, int, PagedResult<User>, IDictionary<string, object>>(out var userManager)
                .Should().BeTrue();

            reqResManager.Should().NotBeNull();
            httpBinManager.Should().NotBeNull();
            userManager.Should().NotBeNull();
        }

        [Fact]
        public void Calling_WithBaseAddress_Should_Set_BaseAddress()
        {
            var uri = new Uri("http://api.com");

            var apizrRegistry = Apizr.Create(registry => registry
                .AddFor<IReqResService>(options => options.WithBaseAddress(uri)));

            var reqResManager = apizrRegistry.GetFor<IReqResService>();

            reqResManager.Options.BaseAddress.Should().Be(uri);
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_ProperOption_Should_Authenticate_Request()
        {
            string token = null;

            var apizrRegistry = Apizr.Create(
                registry => registry
                    .AddFor<IHttpBinService>(options =>
                        options.WithAuthenticationHandler(_ => Task.FromResult(token = "token"))));

            var httpBinManager = apizrRegistry.GetFor<IHttpBinService>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
            token.Should().Be("token");
        }

        [Fact]
        public void Calling_WithLogging_Should_Set_LoggingSettings()
        {
            var apizrRegistry = Apizr.Create(registry => registry
                .AddFor<IReqResService>(options => options
                    .WithLogging(HttpTracerMode.ExceptionsOnly, HttpMessageParts.RequestCookies, LogLevel.Warning)));

            var reqResManager = apizrRegistry.GetFor<IReqResService>();

            reqResManager.Options.HttpTracerMode.Should().Be(HttpTracerMode.ExceptionsOnly);
            reqResManager.Options.TrafficVerbosity.Should().Be(HttpMessageParts.RequestCookies);
            reqResManager.Options.LogLevel.Should().Be(LogLevel.Warning);
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_CommonOption_Should_Authenticate_Request()
        {
            string token = null;

            var apizrRegistry = Apizr.Create(registry => registry
                    .AddFor<IHttpBinService>(),
                config => config
                    .WithAuthenticationHandler(_ => Task.FromResult(token = "token")));

            var httpBinManager = apizrRegistry.GetFor<IHttpBinService>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
            token.Should().Be("token");
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_Should_Authenticate_Request()
        {
            string token = null;

            var apizrRegistry = Apizr.Create(registry => registry
                    .AddFor<IHttpBinService>(options => options
                        .WithAuthenticationHandler(_ => Task.FromResult(token = "tokenA"))),
                config => config
                    .WithAuthenticationHandler(_ => Task.FromResult(token = "tokenB")));

            var httpBinManager = apizrRegistry.GetFor<IHttpBinService>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
            token.Should().Be("tokenA");
        }

        [Fact]
        public async Task Calling_WithAkavacheCacheHandler_Should_Cache_Result()
        {
            var apizrRegistry = Apizr.Create(registry => registry
                    .AddFor<IReqResService>(),
                config => config
                    .WithAkavacheCacheHandler()
                    .AddDelegatingHandler(new FailingRequestHandler()));

            var reqResManager = apizrRegistry.GetFor<IReqResService>();

            // Defining a throwing request
            Func<Task> act = () => reqResManager.ExecuteAsync(api => api.GetUsersAsync(HttpStatusCode.BadRequest));

            // Calling it at first execution should throw as expected without any cached result
            var ex = await act.Should().ThrowAsync<ApizrException<UserList>>();
            ex.And.CachedResult.Should().BeNull();

            // This one should succeed
            var result = await reqResManager.ExecuteAsync(api => api.GetUsersAsync());

            // and cache result in-memory
            result.Should().NotBeNull();
            result.Data.Should().NotBeNullOrEmpty();

            // This one should fail but with cached result
            var ex2 = await act.Should().ThrowAsync<ApizrException<UserList>>();
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

            var apizrRegistry = Apizr.Create(registry => registry
                    .AddFor<IReqResService>(),
                config => config
                    .WithPolicyRegistry(policyRegistry)
                    .AddDelegatingHandler(new FailingRequestHandler()));

            var reqResManager = apizrRegistry.GetFor<IReqResService>();

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

            var apizrRegistry = Apizr.Create(registry => registry
                    .AddFor<IReqResService>(),
                config => config
                    .WithConnectivityHandler(() => isConnected));

            var reqResManager = apizrRegistry.GetFor<IReqResService>();

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
            var apizrRegistry = Apizr.Create(registry => registry
                    .AddFor<IReqResService>(),
                config => config
                    .WithRefitSettings(_refitSettings));

            var reqResManager = apizrRegistry.GetFor<IReqResService>();

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

            var apizrRegistry = Apizr.Create(registry => registry
                    .AddFor<IReqResService>(),
                config => config
                    .WithRefitSettings(_refitSettings)
                    .WithAutoMapperMappingHandler(mapperConfig));

            var reqResManager = apizrRegistry.GetFor<IReqResService>();

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

            var apizrRegistry = Apizr.Create(registry => registry
                    .AddFor<IReqResService>(),
                config => config
                    .WithRefitSettings(_refitSettings)
                    .WithMappingHandler(new AutoMapperMappingHandler(mapperConfig.CreateMapper())));

            var reqResManager = apizrRegistry.GetFor<IReqResService>();

            var minUser = new MinUser { Name = "John" };

            // This one should succeed
            var result =
                await reqResManager.ExecuteAsync<MinUser, User>(
                    (api, user) => api.CreateUser(user, CancellationToken.None), minUser);

            result.Should().NotBeNull();
            result.Name.Should().Be(minUser.Name);
            result.Id.Should().BeGreaterThanOrEqualTo(1);
        }
    }
}
