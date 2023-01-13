using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Policing;
using Apizr.Progressing;
using Apizr.Tests.Apis;
using FluentAssertions;
using MonkeyCache.FileStore;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;
using Refit;
using Xunit;

namespace Apizr.Tests
{
    public class ApizrRequestingTests
    {
        private readonly IPolicyRegistry<string> _policyRegistry;
        private readonly RefitSettings _refitSettings;
        private readonly Assembly _assembly;

        public ApizrRequestingTests()
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
        public async Task Downloading_File_Should_Report_Progress()
        {
            var percentage = 0;
            var progress = new ApizrProgress();
            progress.ProgressChanged += (sender, args) =>
            {
                percentage = args.ProgressPercentage;
            };
            var fileManager = ApizrBuilder.CreateManagerFor<IFileTransferService>(options => options.WithProgress(progress));
            using var response = await fileManager.ExecuteAsync((opt, api) => api.DownloadAsync("test10Mb.db", opt)).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var guid = Guid.NewGuid();
            var fileInfo = new FileInfo($"{guid}.txt");
            await using var ms = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            await using var fs = File.Create(fileInfo.FullName);
            if (ms.CanSeek) ms.Seek(0, SeekOrigin.Begin);
            await ms.CopyToAsync(fs);

            percentage.Should().Be(100);
            fileInfo.Length.Should().BePositive();
        }
    }
}
