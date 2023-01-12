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
        public async Task Downloading_File_Should_Report_Progress()
        {
            var filePath = Path.Combine(Path.GetTempPath(), "test10Mb.db");

            // Setup your progress reporter
            var progress = new Progress<float>();
            progress.ProgressChanged += (sender, f) =>
            {
                Console.WriteLine(f);
            };
            var fileManager = ApizrBuilder.CreateManagerFor<IFileTransferService>();
            using (var response = await fileManager.ExecuteAsync((opt, api) => api.DownloadAsync("test10Mb.db", opt))
                       .ConfigureAwait(false))
            {
                // Obtenez la taille totale du fichier à télécharger.
                var totalRequestBytes = response.Content.Headers.ContentLength ?? 0;

                using (Stream file = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var contentLength = response.Content.Headers.ContentLength;
                    using (var download = await response.Content.ReadAsStreamAsync())
                    {
                        // no progress... no contentLength... very sad
                        if (progress is null || !contentLength.HasValue)
                        {
                            await download.CopyToAsync(file);
                            return;
                        }

                        // Such progress and contentLength much reporting Wow!
                        var progressWrapper = new Progress<long>(totalBytes =>
                            ((IProgress<float>) progress).Report(GetProgressPercentage(totalBytes,
                                contentLength.Value)));
                        await download.CopyToAsync(file, 81920, progressWrapper, CancellationToken.None);
                    }
                }

                float GetProgressPercentage(float totalBytes, float currentBytes) => (totalBytes / currentBytes) * 100f;
            }
        }


        [Fact]
        public async Task Downloading_File_Should_Report_Progress_2()
        {
            var httpHandler = new HttpClientHandler();
            var progresHandler = new ProgressMessageHandler(httpHandler);
            // for the sake of the example lets add a client definition here
            var client = new HttpClient(progresHandler);
            var docUrl = "http://speedtest.ftp.otenet.gr/files/test10Mb.db";
            var filePath = Path.Combine(Path.GetTempPath(), "test10Mb.db");

            // Setup your progress reporter
            var progress = new Progress<float>();
            progress.ProgressChanged += (sender, f) =>
            {
                Console.WriteLine(f);
            };

            // Use the provided extension method
            using (var file = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                await client.DownloadDataAsync(docUrl, file, progress);
        }


        [Fact]
        public async Task Downloading_File_Should_Report_Progress_3()
        {
            var httpHandler = new HttpClientHandler();
            var progresHandler = new ProgressMessageHandler(httpHandler);
            progresHandler.HttpReceiveProgress += (sender, args) =>
            {
                Console.WriteLine(args.ProgressPercentage);
            };
            // for the sake of the example lets add a client definition here
            var client = new HttpClient(progresHandler);
            var docUrl = "http://speedtest.ftp.otenet.gr/files/test10Mb.db";
            var response = await client.GetAsync(docUrl);
            response.EnsureSuccessStatusCode();
            var guid = Guid.NewGuid();
            var fileInfo = new FileInfo($"{guid}.txt");
            await using var ms = await response.Content.ReadAsStreamAsync();
            await using var fs = File.Create(fileInfo.FullName);
            ms.Seek(0, SeekOrigin.Begin);
            await ms.CopyToAsync(fs);

            fileInfo.Length.Should().BePositive();
        }


        [Fact]
        public async Task Downloading_File_Should_Report_Progress_5()
        {
            var progress = new ApizrProgress();
            progress.ProgressChanged += (sender, f) =>
            {
                Console.WriteLine(f);
            };
            var guid = Guid.NewGuid();
            var fileInfo = new FileInfo($"{guid}.txt");
            var fileManager = ApizrBuilder.CreateManagerFor<IFileTransferService>(options => options.WithProgress(progress));
            using (var response = await fileManager.ExecuteAsync((opt, api) => api.DownloadAsync("test10Mb.db", opt))
                       .ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                await using var ms = await response.Content.ReadAsStreamAsync();
                await using var fs = File.Create(fileInfo.FullName);
                ms.Seek(0, SeekOrigin.Begin);
                await ms.CopyToAsync(fs);
            }

            fileInfo.Length.Should().BePositive();
        }
    }

    public static class HttpClientProgressExtensions
    {
        public static async Task DownloadDataAsync(this HttpClient client, string requestUrl, Stream destination, IProgress<float> progress = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var rq = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            using (var response = await client.SendAsync(rq, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false))
            {
                var contentLength = response.Content.Headers.ContentLength;
                using (var download = await response.Content.ReadAsStreamAsync(cancellationToken))
                {
                    // no progress... no contentLength... very sad
                    if (progress is null || !contentLength.HasValue)
                    {
                        await download.CopyToAsync(destination);
                        return;
                    }
                    // Such progress and contentLength much reporting Wow!
                    var progressWrapper = new Progress<long>(totalBytes => progress.Report(GetProgressPercentage(totalBytes, contentLength.Value)));
                    await download.CopyToAsync(destination, 81920, progressWrapper, cancellationToken);
                }
            }

            float GetProgressPercentage(float totalBytes, float currentBytes) => (totalBytes / currentBytes) * 100f;
        }

        public static async Task CopyToAsync(this Stream source, Stream destination, int bufferSize, IProgress<long> progress = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (!source.CanRead)
                throw new InvalidOperationException($"'{nameof(source)}' is not readable.");
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (!destination.CanWrite)
                throw new InvalidOperationException($"'{nameof(destination)}' is not writable.");

            var buffer = new byte[bufferSize];
            long totalBytesRead = 0;
            int bytesRead;
            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) != 0)
            {
                await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
                totalBytesRead += bytesRead;
                progress?.Report(totalBytesRead);
            }
        }
    }
}
