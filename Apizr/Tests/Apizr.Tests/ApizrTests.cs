using System;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Policing;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MonkeyCache.FileStore;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;
using Xunit;
using IReqResService = Apizr.Tests.Apis.IReqResService;

namespace Apizr.Tests
{
    public class ApizrTests
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;

        public ApizrTests()
        {
            _loggerFactory = new NullLoggerFactory();
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

            Barrel.ApplicationId = nameof(ApizrTests);
        }

        [Fact]
        public async Task IReqResService_GetUsersAsync_ShouldSucceed()
        {
            var fixture = Apizr.CreateFor<IReqResService>();
            var result = await fixture.ExecuteAsync(api => api.GetUsersAsync());

            result.Should().NotBeNull();
            result.Data.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task IReqResService_GetUsersAsync_WithLogger_ShouldSucceed()
        {
            var fixture = Apizr.CreateFor<IReqResService>(options => options
                .WithLoggerFactory(_loggerFactory)
                .WithLogging());

            var result = await fixture.ExecuteAsync(api => api.GetUsersAsync());

            result.Should().NotBeNull();
            result.Data.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task IReqResService_GetUsersAsync_WithPolicy_ShouldSucceed()
        {
            var fixture = Apizr.CreateFor<IReqResService>(options => options
                .WithPolicyRegistry(_policyRegistry));

            var result = await fixture.ExecuteAsync(api => api.GetUsersAsync());

            result.Should().NotBeNull();
            result.Data.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Calling_WithBaseAddress_Should_Set_BaseAddress()
        {
            var uri = new Uri("http://api.com");

            var fixture = Apizr.CreateFor<IReqResService>(options => options
                .WithBaseAddress(uri));

            fixture.Options.BaseAddress.Should().Be(uri);
        }

        [Fact]
        public async Task IReqResService_GetUsersAsync_WithDummyBaseAddress_ShouldThrow()
        {
            var fixture = Apizr.CreateFor<IReqResService>(options => options
                .WithBaseAddress("http://api.com"));

            Func<Task> act = () => fixture.ExecuteAsync(api => api.GetUsersAsync());

            await act.Should().ThrowAsync<ApizrException>();
        }

        [Fact]
        public async Task IReqResService_GetUsersAsync_WithHttpClientHandler_ShouldSucceed()
        {
            var fixture = Apizr.CreateFor<IReqResService>(options => options
                .WithHttpClientHandler(new HttpClientHandler()));

            var result = await fixture.ExecuteAsync(api => api.GetUsersAsync());

            result.Should().NotBeNull();
            result.Data.Should().NotBeNullOrEmpty();
        }
    }
}
