using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Integrations.FileTransfer;
using Apizr.Policing;
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
        public async Task Downloading_File_Should_Report_Progress_1()
        {
            var percentage = 0;
            var httpHandler = new HttpClientHandler();
            var progressHandler = new ApizrProgressMessageHandler(httpHandler);
            progressHandler.HttpReceiveProgress += (sender, args) =>
            {
                percentage = args.ProgressPercentage;
            };
            // for the sake of the example lets add a client definition here
            var client = new HttpClient(progressHandler);
            var docUrl = "http://speedtest.ftp.otenet.gr/files/test10Mb.db";
            using var response = await client.GetAsync(docUrl).ConfigureAwait(false);
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

        [Fact]
        public async Task Downloading_File_Should_Report_Progress_2()
        {
            var percentage = 0;
            var httpHandler = new HttpClientHandler();
            var progressHandler = new ApizrProgressMessageHandler(httpHandler);
            progressHandler.HttpReceiveProgress += (sender, args) =>
            {
                percentage = args.ProgressPercentage;
            };
            // for the sake of the example lets add a client definition here
            var client = new HttpClient(progressHandler) { BaseAddress = new Uri("http://speedtest.ftp.otenet.gr/files") };
            var fileManager = RestService.For<IFileTransferService>(client);
            using var response = await fileManager.DownloadAsync("test10Mb.db").ConfigureAwait(false);
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

        [Fact]
        public async Task Downloading_File_Should_Report_Progress_3()
        {
            var percentage = 0;
            var progressHandler = new ApizrProgressMessageHandler();
            progressHandler.HttpReceiveProgress += (sender, args) =>
            {
                percentage = args.ProgressPercentage;
            };
            var fileManager = ApizrBuilder.CreateManagerFor<IFileTransferService>(options => options.AddDelegatingHandler(progressHandler));
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
        
        [Fact]
        public async Task Downloading_File_Should_Report_Progress_4()
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
