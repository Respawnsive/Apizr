using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Handlers;
//using System.Net.Http.Handlers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Integrations.FileTransfer;
using Apizr.Policing;
using Apizr.Tests.Apis;
using Apizr.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
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
            var progress = new Progress<int>();
            progress.ProgressChanged += (sender, f) =>
            {
                Console.WriteLine(f);
            };
            var fileManager = ApizrBuilder.CreateManagerFor<IFileTransferService>();
            using (var response = await fileManager.ExecuteAsync(api => api.DownloadAsync("test10Mb.db"))
                       .ConfigureAwait(false))
            {
                // Obtenez la taille totale du fichier à télécharger.
                var totalBytes = response.Content.Headers.ContentLength ?? 0;

                // Ouvrez un flux de lecture pour lire les données du fichier.
                using (var downloadStream = await response.Content.ReadAsStreamAsync())
                {
                    // Créez un buffer pour stocker les données téléchargées.
                    var buffer = new byte[4096];
                    int bytesRead;
                    long bytesReceived = 0;

                    // Continuez à lire les données du fichier tant que la tâche n'est pas annulée.
                    while ((bytesRead = await downloadStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        // Mettre à jour le nombre de bytes reçus.
                        bytesReceived += bytesRead;

                        // Mettre à jour la progression de la tâche en appelant la méthode Report sur l'objet IProgress<T>.
                        ((IProgress<int>)progress).Report((int)((double)bytesReceived / totalBytes * 100));
                    }
                }
            }

                //using (Stream file = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                //    using (var response = await fileManager.ExecuteAsync(api => api.DownloadAsync("test10Mb.db")).ConfigureAwait(false))
                //    {
                //        var contentLength = response.Content.Headers.ContentLength;
                //        using (var download = await response.Content.ReadAsStreamAsync())
                //        {
                //            // no progress... no contentLength... very sad
                //            if (progress is null || !contentLength.HasValue)
                //            {
                //                await download.CopyToAsync(file);
                //                return;
                //            }
                //            // Such progress and contentLength much reporting Wow!
                //            var progressWrapper = new Progress<long>(totalBytes => ((IProgress<float>)progress).Report(GetProgressPercentage(totalBytes, contentLength.Value)));
                //            await download.CopyToAsync(file, 81920, progressWrapper, CancellationToken.None);
                //        }
                //    }

                //float GetProgressPercentage(float totalBytes, float currentBytes) => (totalBytes / currentBytes) * 100f;
        }


        [Fact]
        public async Task Downloading_File_Should_Report_Progress_2()
        {
            // for the sake of the example lets add a client definition here
            var client = new HttpClient();
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
            var handler = new HttpClientHandler() { AllowAutoRedirect = true };
            var ph = new ProgressMessageHandler(handler);

            ph.HttpSendProgress += (_, args) =>
            {
                Console.WriteLine($"upload progress: {(double)args.BytesTransferred / args.TotalBytes}");
            };

            ph.HttpReceiveProgress += (_, args) =>
            {
                Console.WriteLine($"download progress: {(double)args.BytesTransferred / args.TotalBytes}");
            };

            // for the sake of the example lets add a client definition here
            var client = new HttpClient(ph);
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
        public async Task Downloading_File_Should_Report_Progress_4()
        {
            var ph = new ProgressMessageHandler();

            ph.HttpSendProgress += (_, args) =>
            {
                Console.WriteLine($"upload progress: {(double)args.BytesTransferred / args.TotalBytes}");
            };

            ph.HttpReceiveProgress += (_, args) =>
            {
                Console.WriteLine($"download progress: {(double)args.BytesTransferred / args.TotalBytes}");
            };

            // for the sake of the example lets add a client definition here
            var guid = Guid.NewGuid();
            var fileInfo = new FileInfo($"{guid}.txt");
            var fileManager = ApizrBuilder.CreateManagerFor<IFileTransferService>(options => options.AddDelegatingHandler(ph));
            using (var response = await fileManager.ExecuteAsync(api => api.DownloadAsync("test10Mb.db"))
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
            using (var response = await fileManager.ExecuteAsync(api => api.DownloadAsync("test10Mb.db"))
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
