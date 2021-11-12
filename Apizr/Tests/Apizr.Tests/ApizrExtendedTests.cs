using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Apizr.Policing;
using Apizr.Sample.Api;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;
using Xunit;
using HttpRequestMessageExtensions = Apizr.Policing.HttpRequestMessageExtensions;

namespace Apizr.Tests
{
    public class ApizrExtendedTests
    {
        private readonly IPolicyRegistry<string> _policyRegistry;

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
        }

        [Fact]
        public void ServiceCollection_Should_Contain_IApizrManager_IReqResService()
        {
            var services = new ServiceCollection();
            services.AddApizrFor<IReqResService>();

            services.Should().Contain(x => x.ServiceType == typeof(IApizrManager<IReqResService>));
        }

        [Fact]
        public void ServiceProvider_Should_Resolve_IApizrManager_IReqResService()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizrFor<IReqResService>();

            var serviceProvider = services.BuildServiceProvider();
            var reqResManager = serviceProvider.GetService<IApizrManager<IReqResService>>();

            reqResManager.Should().NotBeNull();
        }

        [Fact]
        public void Calling_WithBaseAddress_Should_Set_BaseAddress()
        {
            var uri = new Uri("http://api.com");

            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizrFor<IReqResService>(options => options
                .WithBaseAddress(uri));

            var serviceProvider = services.BuildServiceProvider();
            var reqResManager = serviceProvider.GetRequiredService<IApizrManager<IReqResService>>();

            reqResManager.Options.BaseAddress.Should().Be(uri);
        }

        [Fact]
        public async Task Unauthenticated_Request_Should_Fail()
        {
            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizrFor<IHttpBinService>();

            var serviceProvider = services.BuildServiceProvider();
            var httpBinManager = serviceProvider.GetRequiredService<IApizrManager<IHttpBinService>>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());
            
            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Calling_WithAuthenticationHandler_Should_Authenticate_Request()
        {
            string token = null;

            var services = new ServiceCollection();
            services.AddPolicyRegistry(_policyRegistry);
            services.AddApizrFor<IHttpBinService>(options => options
                .WithAuthenticationHandler(_ => Task.FromResult(token = "token")));

            var serviceProvider = services.BuildServiceProvider();
            var httpBinManager = serviceProvider.GetRequiredService<IApizrManager<IHttpBinService>>();

            var result = await httpBinManager.ExecuteAsync(api => api.AuthBearerAsync());

            result.IsSuccessStatusCode.Should().BeTrue();
            token.Should().Be("token");
        }
    }
}
