using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apizr.Extending.Configuring.Registry;
using Apizr.Policing;
using Apizr.Sample.Api;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
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
                .AddFor<IReqResService>(options => options.WithBaseAddress(uri))
                .AddFor<IHttpBinService>());

            var serviceProvider = services.BuildServiceProvider();
            var fixture = serviceProvider.GetRequiredService<IApizrManager<IReqResService>>();

            fixture.Options.BaseAddress.Should().Be(uri);
        }
    }
}
