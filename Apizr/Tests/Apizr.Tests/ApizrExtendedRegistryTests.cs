using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Apizr.Extending.Configuring.Registry;
using Apizr.Logging;
using Apizr.Policing;
using Apizr.Sample.Api;
using Apizr.Sample.Api.Models;
using Apizr.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MonkeyCache.FileStore;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;
using Xunit;

namespace Apizr.Tests
{
    public class ApizrExtendedRegistryTests
    {
        private readonly IPolicyRegistry<string> _policyRegistry;

        public ApizrExtendedRegistryTests()
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
            
            Barrel.ApplicationId = nameof(ApizrExtendedRegistryTests);
        }

        [Fact]
        public void ServiceCollection_Should_Contain_Registry_And_Managers()
        {
            var services = new ServiceCollection();
            services.AddApizr(registry => registry
                .AddFor<IReqResService>()
                .AddFor<IHttpBinService>());

            services.Should().Contain(x => x.ServiceType == typeof(IApizrExtendedRegistry));
            services.Should().Contain(x => x.ServiceType == typeof(IApizrManager<IReqResService>));
            services.Should().Contain(x => x.ServiceType == typeof(IApizrManager<IHttpBinService>));
        }

        [Fact]
        public void ServiceProvider_Should_Resolve_Registry_And_Managers()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                .AddFor<IReqResService>()
                .AddFor<IHttpBinService>());

            var serviceProvider = services.BuildServiceProvider();
            var registry = serviceProvider.GetService<IApizrExtendedRegistry>();
            var reqResManager = serviceProvider.GetService<IApizrManager<IReqResService>>();
            var httpBinManager = serviceProvider.GetService<IApizrManager<IHttpBinService>>();

            registry.Should().NotBeNull();
            reqResManager.Should().NotBeNull();
            httpBinManager.Should().NotBeNull();
        }

        [Fact]
        public void Registry_Should_Resolve_Managers()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                .AddFor<IReqResService>()
                .AddFor<IHttpBinService>());

            var serviceProvider = services.BuildServiceProvider();
            var registry = serviceProvider.GetRequiredService<IApizrExtendedRegistry>();
            
            registry.TryGetFor<IReqResService>(out var reqResManager).Should().BeTrue();
            registry.TryGetFor<IHttpBinService>(out var httpBinManager).Should().BeTrue();

            reqResManager.Should().NotBeNull();
            httpBinManager.Should().NotBeNull();
        }

        [Fact]
        public void Calling_WithBaseAddress_Should_Set_BaseAddress()
        {
            var uri = new Uri("http://api.com");

            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                .AddFor<IReqResService>(options => options.WithBaseAddress(uri)));

            var serviceProvider = services.BuildServiceProvider();
            var fixture = serviceProvider.GetRequiredService<IApizrManager<IReqResService>>();

            fixture.Options.BaseAddress.Should().Be(uri);
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_ProperOption_Should_Authenticate_Request()
        {
            string token = null;

            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                .AddFor<IHttpBinService>(options => options
                    .WithAuthenticationHandler(_ => Task.FromResult(token = "token"))));

            var serviceProvider = services.BuildServiceProvider();
            var httpBinManager = serviceProvider.GetRequiredService<IApizrManager<IHttpBinService>>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
            token.Should().Be("token");
        }

        [Fact]
        public void Calling_WithLogging_Should_Set_LoggingSettings()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(registry => registry
                .AddFor<IReqResService>(options => options
                    .WithLogging(HttpTracerMode.ExceptionsOnly, HttpMessageParts.RequestCookies, LogLevel.Warning)));

            var serviceProvider = services.BuildServiceProvider();
            var fixture = serviceProvider.GetRequiredService<IApizrManager<IReqResService>>();

            fixture.Options.HttpTracerMode.Should().Be(HttpTracerMode.ExceptionsOnly);
            fixture.Options.TrafficVerbosity.Should().Be(HttpMessageParts.RequestCookies);
            fixture.Options.LogLevel.Should().Be(LogLevel.Warning);
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_CommonOption_Should_Authenticate_Request()
        {
            string token = null;

            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(
                registry => registry
                    .AddFor<IHttpBinService>(),
                config => config
                    .WithAuthenticationHandler(_ => Task.FromResult(token = "token")));

            var serviceProvider = services.BuildServiceProvider();
            var httpBinManager = serviceProvider.GetRequiredService<IApizrManager<IHttpBinService>>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
            token.Should().Be("token");
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_Should_Authenticate_Request()
        {
            string token = null;

            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizr(
                registry => registry
                    .AddFor<IHttpBinService>(options => options
                        .WithAuthenticationHandler(_ => Task.FromResult(token = "tokenA"))),
                config => config
                    .WithAuthenticationHandler(_ => Task.FromResult(token = "tokenB")));

            var serviceProvider = services.BuildServiceProvider();
            var httpBinManager = serviceProvider.GetRequiredService<IApizrManager<IHttpBinService>>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
            token.Should().Be("tokenA");
        }

        [Fact]
        public async Task Calling_WithCacheHandler_Should_Cache_Result()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddMemoryCache();

            services.AddApizr(
                registry => registry
                    .AddFor<IReqResService>(),
                config => config
                    .WithInMemoryCacheHandler()
                    .AddDelegatingHandler(new FailingRequestHandler()));

            var serviceProvider = services.BuildServiceProvider();
            var reqResManager = serviceProvider.GetRequiredService<IApizrManager<IReqResService>>();

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
                    "TransientHttpError", HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(sleepDurations, 
                        (_, _, retry, _) => attempts = retry).WithPolicyKey("TransientHttpError")
                }
            };

            var services = new ServiceCollection();
            services.AddPolicyRegistry(policyRegistry);
            services.AddMemoryCache();

            services.AddApizr(
                registry => registry
                    .AddFor<IReqResService>(),
                config => config
                    .AddDelegatingHandler(new FailingRequestHandler()));

            var serviceProvider = services.BuildServiceProvider();
            var reqResManager = serviceProvider.GetRequiredService<IApizrManager<IReqResService>>();

            // Defining a transient throwing request
            Func<Task> act = () => reqResManager.ExecuteAsync(api => api.GetUsersAsync(HttpStatusCode.RequestTimeout));

            // Calling it should throw but handled by Polly
            await act.Should().ThrowAsync<ApizrException>();

            // attempts should be equal to total retry count
            attempts.Should().Be(sleepDurations.Length);
        }
    }
}
