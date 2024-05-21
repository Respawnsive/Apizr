using Apizr.Tests.Apis;
using Apizr.Tests.Helpers;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Polly;
using Xunit;
using Xunit.Abstractions;
using MonkeyCache.FileStore;
using Polly.Retry;
using Refit;
using System.Net;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Apizr.Tests.Maui
{
    public class PlatformSpecificTests
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly ResiliencePipelineBuilder<HttpResponseMessage> _resiliencePipelineBuilder;
        private readonly RefitSettings _refitSettings;
        private readonly Assembly _assembly;

        public PlatformSpecificTests(ITestOutputHelper outputHelper)
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
        }

        [Fact]
        public async Task Maui_Requesting_With_Headers_Should_Set_Headers()
        {
            var watcher = new WatchingRequestHandler();

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, builder) =>
                    builder.AddXUnit(_outputHelper)
                        .SetMinimumLevel(LogLevel.Trace))
                .ConfigureServices((_, services) =>
                {

                    services.AddApizrManagerFor<IReqResSimpleService>(options => options
                        .WithLogging()
                        .WithBaseAddress("https://reqres.in/api")
                        .WithHeaders(["testKey2: testValue2.2", "testKey3: testValue3.1"])
                        .WithLoggedHeadersRedactionNames(["testKey2"])
                        .WithDelegatingHandler(watcher));

                    services.AddResiliencePipeline<string, HttpResponseMessage>("TransientHttpError",
                        builder => builder.AddPipeline(_resiliencePipelineBuilder.Build()));
                })
                .Build();

            var scope = host.Services.CreateScope();

            // Get instances from the container
            var apizrManager = scope.ServiceProvider.GetService<IApizrManager<IReqResSimpleService>>();

            // Shortcut
            await apizrManager.ExecuteAsync((opt, api) => api.GetUsersAsync(opt), options =>
                options.WithHeaders(["testKey3: testValue3.2", "testKey4: testValue4"])
                    .WithLoggedHeadersRedactionRule(header => header == "testKey3"));
            watcher.Headers.Should().NotBeNull();
            watcher.Headers.Should().ContainKeys("testKey1", "testKey2", "testKey3", "testKey4");
            watcher.Headers.GetValues("testKey1").Should().HaveCount(1).And.Contain("testValue1"); // Set by attribute
            watcher.Headers.GetValues("testKey2").Should().HaveCount(1).And.Contain("testValue2.2"); // Set by attribute then updated by common option
            watcher.Headers.GetValues("testKey3").Should().HaveCount(1).And.Contain("testValue3.2"); // Set by common option then updated by request option
            watcher.Headers.GetValues("testKey4").Should().HaveCount(1).And.Contain("testValue4"); // Set by request option
        }
    }
}
